#region Using directives
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
#endregion

namespace OPMedia.Core.Logging
{
    #region Logger
    public class Logger
    {
        const string LogDateFormat = "dd-MM-yyyy";

        static object _readWriteLock = new object();

        #region Members
        private static Logger instance = new Logger();
        private ConcurrentQueue<LogEntry> entries = new ConcurrentQueue<LogEntry>();
        private System.Threading.Thread loggerThread = null;
        private bool loggerThreadMustRun = true;
        private static long sessionID;
        private static string logFileFolder = ".";

        #endregion

        #region Methods
        public static void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public static void CopyCurrentLogFile(string path)
        {
            string fileName = GetCurrentLogFileName();
            if (!string.IsNullOrEmpty(fileName))
            {
                File.Copy(fileName, path, true);
            }

            Logger.LogInfo("Log file contents was copied to: " + path);
        }

        public static void PurgeLogFile(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = GetCurrentLogFileName();
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                File.Delete(fileName);
            }

            Logger.LogInfo("Log file contents was purged.");
        }

        public static string GetCurrentLogFileName()
        {
            string currentDayStamp = DateTime.Now.ToString(LogDateFormat);
            string appName = Assembly.GetEntryAssembly().GetName().Name;

            string logFilePath =
                System.IO.Path.Combine(logFileFolder, appName + "_" + currentDayStamp + ".log");

            if (File.Exists(logFilePath))
            {
                return logFilePath;
            }

            return string.Empty;
        }

        public static void LogHelpTrace(string format, params object[] args)
        {
            if (Environment.CommandLine?.Contains("--ENHANCED_TRACES") ?? false)
            {
                string message = FormatMessage(format, args);
                LogToConsole("HELP: " + message);
            }
        }

        public static void LogUntranslatable(string format, params object[] args)
        {
            if (Environment.CommandLine?.Contains("--ENHANCED_TRACES") ?? false)
            {
                string message = FormatMessage(format, args);
                LogToConsole("NOT TRANSLATABLE: " + message);
            }
        }

        public static void LogUntranslated(string format, params object[] args)
        {
            if (Environment.CommandLine?.Contains("--ENHANCED_TRACES") ?? false)
            {
                string message = FormatMessage(format, args);
                LogToConsole("NOT TRANSLATED: " + message);
            }
        }

        public static void LogToConsole(string format, params object[] args)
        {
            string message = FormatMessage(format, args);
            Debug.WriteLine(message);
            Debug.Flush();
        }

        public static void LogTrace_WithStackDump(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            string message = FormatMessage(format, args);
            message += st.ToString();

            string[] lines = message.Split('\n');
            foreach (string line in lines)
            {
                Debug.WriteLine("TRC: " + line);
            }

            if (LoggingConfiguration.TraceLevelEnabled)
            {
                LogEntry entry = new LogEntry(SeverityLevels.Trace, message,
                    Assembly.GetCallingAssembly().GetName().Name);
                instance.EnqueueLogEntry(entry);
            }
        }

        public static void LogTrace(string format, params object[] args)
        {
            string message = FormatMessage(format, args);
            Debug.WriteLine("TRC: " + message);

            if (LoggingConfiguration.TraceLevelEnabled)
            {
                LogEntry entry = new LogEntry(SeverityLevels.Trace, message,
                    Assembly.GetCallingAssembly().GetName().Name);
                instance.EnqueueLogEntry(entry);
            }
        }

        public static void LogInfo(string format, params object[] args)
        {
            string message = FormatMessage(format, args);
            Debug.WriteLine("INF: " + message);

            if (LoggingConfiguration.InfoLevelEnabled)
            {
                LogEntry entry = new LogEntry(SeverityLevels.Info, message,
                    Assembly.GetCallingAssembly().GetName().Name);
                instance.EnqueueLogEntry(entry);
            }
        }

        public static void LogWarning(string format, params object[] args)
        {
            string message = FormatMessage(format, args);
            Debug.WriteLine("WRN: " + message);

            if (LoggingConfiguration.WarningLevelEnabled)
            {
                LogEntry entry = new LogEntry(SeverityLevels.Warning, message,
                    Assembly.GetCallingAssembly().GetName().Name);
                instance.EnqueueLogEntry(entry);
            }
        }

        public static void LogError(string format, params object[] args)
        {
            string message = FormatMessage(format, args);
            Debug.WriteLine("ERR: " + message);

            if (LoggingConfiguration.ErrorLevelEnabled)
            {
                LogEntry entry = new LogEntry(SeverityLevels.Error, message,
                    Assembly.GetCallingAssembly().GetName().Name);
                instance.EnqueueLogEntry(entry);
            }
        }

        public static void LogException(Exception ex)
        {
            LogException(ex, string.Empty);
        }

        public static void LogException(Exception ex, string format, params object[] args)
        {
            string message = FormatMessage(format, args);
            Debug.WriteLine("EXC: " + ErrorDispatcher.GetErrorMessageForException(ex, true));

            if (LoggingConfiguration.LoggingEnabled)
            {
                LogEntry entry = new LogEntry(message, ex,
                    Assembly.GetCallingAssembly().GetName().Name);
                instance.EnqueueLogEntry(entry);
            }
        }

        public static void LogComPortData(string comName, byte[] bytes)
        {
            if (LoggingConfiguration.LoggingEnabled &&
                LoggingConfiguration.TraceLevelEnabled)
            {
                if (!LoggingConfiguration.LoggingEnabled)
                {
                    return;
                }

                System.IO.TextWriter logWriter = null;

                try
                {
                    if (!Directory.Exists(logFileFolder))
                    {
                        Directory.CreateDirectory(logFileFolder);
                    }

                    string currentDayStamp = DateTime.Now.ToString(LogDateFormat);
                    string logFilePath =
                        System.IO.Path.Combine(logFileFolder, comName + "_" + currentDayStamp + ".log");

                    logWriter = System.IO.File.AppendText(logFilePath);

                    logWriter.Write(Encoding.Unicode.GetString(bytes));
                }
                catch
                {
                }
                finally
                {
                    if (logWriter != null)
                    {
                        // Ensure file is not kept open.
                        logWriter.Close();
                    }
                }
            }
        }

        public static void StopLogger()
        {
            if (LoggingConfiguration.LoggingEnabled)
            {
                if (instance.loggerThread.IsAlive)
                {
                    LogTrace("Logger thread was requested to stop...");
                    instance.loggerThreadMustRun = false;
                    instance.loggerThread.Join();

                    LogEntry entry = new LogEntry(SeverityLevels.Info,
                        "Logger thread has stopped.",
                        Assembly.GetCallingAssembly().GetName().Name);
                    WriteLogEntry(entry);
                }
            }
        }

        public static void LogTimeDifference(string text, DateTime start)
        {
            int diff = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
            Logger.LogToConsole("{0}: operation took {1} msec", text, diff);
        }

#endregion

        #region Construction
        static Logger()
        {
            logFileFolder = LoggingConfiguration.GetDefaultLoggingFolder();

            if (string.IsNullOrEmpty(logFileFolder))
                logFileFolder = ".";
        }


        private Logger()
        {
            if (LoggingConfiguration.LoggingEnabled)
            {
                WriteLogSessionStart();

                loggerThread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(ThreadLoop));
                loggerThread.Start();
            }
        }

        #endregion

        #region Implementation
        private void EnqueueLogEntry(LogEntry entry)
        {
            // Don't add any more entries after thread was requested to stop.
            if (loggerThreadMustRun)
            {
                entries.Enqueue(entry);
            }
        }

        private LogEntry DequeueLogEntry()
        {
            LogEntry entry = null;
            if (entries.TryDequeue(out entry))
            {
                return entry;
            }

            return null;
        }

        private static void ThreadLoop()
        {
            LogInfo("Logger thread loop has begun ...");

            // Purge old log files on startup
            PurgeOldLogFiles();

            while (instance.loggerThreadMustRun ||
                    instance.entries.Count > 0)
            {
                ProcessQueue();

                DateTime now = DateTime.Now;
                if (now.Minute % 60 == 0)
                {
                    // Each hour of continous uptime, purge old log files
                    PurgeOldLogFiles();
                }

                System.Threading.Thread.Sleep(10);
            }

            LogEntry entry = new LogEntry(SeverityLevels.Info,
                "Logger thread loop has terminated.",
                Assembly.GetCallingAssembly().GetName().Name);
            WriteLogEntry(entry);
        }

        private static void PurgeOldLogFiles()
        {
            List<string> logFiles = PathUtils.EnumFiles(LoggingConfiguration.GetDefaultLoggingFolder(), "*.log");
            foreach (string logFile in logFiles)
            {
                try
                {
                    if (File.Exists(logFile))
                    {
                        string name = Path.GetFileNameWithoutExtension(logFile);
                        DateTime logFileDate = GetDateFromLogFileName(name);

                        TimeSpan diff = DateTime.Now.Subtract(logFileDate);
                        if (diff.Days >= LoggingConfiguration.DaysToKeepLogs)
                        {
                            // Old file. Delete it.
                            File.Delete(logFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogEntry entry = new LogEntry(SeverityLevels.Exception,
                        ex.Message, "Logger thread");

                    WriteLogEntry(entry);
                }
            }
        }

        private static DateTime GetDateFromLogFileName(string logFile)
        {
            DateTime retVal = DateTime.MaxValue;

            if (logFile.Length > LogDateFormat.Length)
            {
                string date = logFile.Substring(logFile.Length - LogDateFormat.Length);

                DateTime dt;
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = LogDateFormat;

                if (DateTime.TryParse(date, dtfi, DateTimeStyles.AssumeLocal,
                    out dt))
                {
                    retVal = dt;
                }
            }

            return retVal;
        }

        private static void ProcessQueue()
        {
            LogEntry lastEntry = instance.DequeueLogEntry();
            if (lastEntry != null)
            {
                WriteLogEntry(lastEntry);
            }
        }

        internal static void WriteLogEntry(LogEntry entry)
        {
            if (entry.SeverityLevel == SeverityLevels.HeavyTrace)
            {
                Debug.WriteLine("HTRC: " + entry.Message);
                Debug.Flush();
            }
            else
            {
                WriteLogString(entry.AppName, entry.ToString());
            }
        }

        private static void WriteLogSessionStart()
        {
            string appName = Process.GetCurrentProcess().ProcessName;
            DateTime now = DateTime.Now;
            sessionID = now.Ticks;

            LogEntry entry = new LogEntry(SeverityLevels.Automatic, "BEGIN_LOG_SESSION",
                Assembly.GetCallingAssembly().GetName().Name);

            WriteLogEntry(entry);

            string msg = string.Format("Application started.");

            entry = new LogEntry(SeverityLevels.Info,
                msg, Assembly.GetCallingAssembly().GetName().Name);

            WriteLogEntry(entry);
        }

        internal static void WriteLogSessionEnd()
        {
            string appName = Process.GetCurrentProcess().ProcessName;
            DateTime now = DateTime.Now;

            string msg = string.Format("Application ended.");

            LogEntry entry = new LogEntry(SeverityLevels.Info,
                msg, Assembly.GetCallingAssembly().GetName().Name);

            WriteLogEntry(entry);

            entry = new LogEntry(SeverityLevels.Automatic, "END_LOG_SESSION",
                Assembly.GetCallingAssembly().GetName().Name);

            WriteLogEntry(entry);
        }

        private static void WriteLogString(string appName, string message)
        {
            if (!LoggingConfiguration.LoggingEnabled)
            {
                return;
            }

            System.IO.TextWriter logWriter = null;

            try
            {
                if (!Directory.Exists(logFileFolder))
                {
                    Directory.CreateDirectory(logFileFolder);
                }

                string currentDayStamp = DateTime.Now.ToString(LogDateFormat);
                string logFilePath =
                    System.IO.Path.Combine(logFileFolder, appName + "_" + currentDayStamp + ".log");

                logWriter = System.IO.File.AppendText(logFilePath);


                string logLine = string.Format("~~{0:X20}|{1}", sessionID, message);

                lock (_readWriteLock)
                {
                    logWriter.WriteLine(logLine);
                }
            }
            catch
            {
            }
            finally
            {
                if (logWriter != null)
                {
                    // Ensure file is not kept open.
                    logWriter.Close();
                }
            }
        }
        #endregion

        private static string FormatMessage(string fmt, params object[] args)
        {
            if (fmt == null)
                return "<null>";

            if (args == null || args.Length < 1)
                return fmt;

            return string.Format(fmt, args);
        }
    }
#endregion
}