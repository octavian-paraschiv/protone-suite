#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using OPMedia.Core;
using System.Collections.Concurrent;
using OPMedia.Core.Utilities;
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

        private static string assemblyLocation;
        private static string logFileFolder;

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

        public static string GetCurrentLogFolder()
        {
            return logFileFolder;
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

        public static List<string> GetCurrentLogFileLines(SeverityLevels filter)
        {
            return GetCurrentLogFileLines(filter, 0);
        }

        public static List<string> GetCurrentLogFileLines(SeverityLevels filter, int lineCount)
        {
            return GetCurrentLogFileLines(filter, lineCount, false);
        }

        public static List<string> GetCurrentLogFileLines(SeverityLevels filter, int lineCount, bool lastRows)
        {
            string logFile = GetCurrentLogFileName();
            lock (_readWriteLock)
            {
                return GetLogFileLines(filter, logFile, lineCount, lastRows);
            }
        }

        public static List<string> GetLogFileLines(SeverityLevels filter, string logFile, int lineCount, bool lastRows)
        {
            try
            {
                return LogFileReader.ReadLogFileLines(filter, logFile, lineCount, lastRows);
            }
            catch
            {
                return new List<string>();
            }
        }

        public static void LogTranslationTrace(string format, params object[] args)
        {
#if HAVE_TRACE_UNTRANSLATED
            string message = string.Format(format, args);
            Debug.WriteLine("UNTRANSLATED: " + message);
            Debug.Flush();
#endif
        }

        public static void LogHelpTrace(string format, params object[] args)
        {
#if HAVE_TRACE_HELP
            string message = string.Format(format, args);
            Debug.WriteLine("HELP: " + message);
            Debug.Flush();
#endif
        }

        public static void LogTrace_WithStackDump(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            string message = string.Format(format, args);
            message += st.ToString();

            string[] lines = message.Split('\n');
            foreach (string line in lines)
            {
                Debug.WriteLine("TRC: " + line);
            }
        }

        public static void LogTrace(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Debug.WriteLine("TRC: " + message);
        }

        public static void LogInfo(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Debug.WriteLine("INF: " + message);
        }

        public static void LogWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Debug.WriteLine("WRN: " + message);
        }

        public static void LogError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Debug.WriteLine("ERR: " + message);
        }

        public static void LogException(Exception ex, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Debug.WriteLine("EXC: " + ErrorDispatcher.GetErrorMessageForException(ex, true));
        }
        
        public static void LogException(Exception ex)
        {
            LogException(ex, string.Empty);
        }

        public static void LogTimeDifference(string text, DateTime start)
        {
            int diff = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
            Logger.LogTrace("{0}: operation took {1} msec", text, diff);
        }
        #endregion
    }
    #endregion
}
