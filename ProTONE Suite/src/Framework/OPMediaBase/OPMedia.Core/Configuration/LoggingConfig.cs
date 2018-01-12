using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using OPMedia.Core.Utilities;

namespace OPMedia.Core.Configuration
{
    public static class LoggingConfig
    {
        #region Logging

        public static string GetDefaultLoggingFolder()
        {
            try
            {
                string installPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string defaultPath = Path.Combine(installPath, "Logs");

                if (CanWriteToFolder(defaultPath))
                    return defaultPath;

                defaultPath = Path.Combine(ApplicationInfo.AltLogsFolder, "Logs");
                if (CanWriteToFolder(defaultPath))
                    return defaultPath;
            }
            catch
            {
            }

            return Path.GetTempPath();
        }

        public static bool CanWriteToFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string randomName = StringUtils.GenerateRandomToken(32);
                string randomFilePath = Path.Combine(path, randomName);

                StreamWriter sw = File.CreateText(randomFilePath);
                if (sw != null)
                {
                    sw.WriteLine(randomName);
                    sw.Close();
                }

                if (File.Exists(randomFilePath))
                {
                    File.Delete(randomFilePath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool LogEnabled
        {
            get { return PersistenceProxy.ReadObject("LogEnabled", true); }
            set { PersistenceProxy.SaveObject("LogEnabled", value); }
        }

        public static bool LogHeavyTraceLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("LogHeavyTraceLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("LogHeavyTraceLevelEnabled", value); }
        }

        public static bool LogTraceLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("LogTraceLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("LogTraceLevelEnabled", value); }
        }

        public static bool LogInfoLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("LogInfoLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("LogInfoLevelEnabled", value); }
        }

        public static bool LogWarningLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("LogWarningLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("LogWarningLevelEnabled", value); }
        }

        public static bool LogErrorLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("LogErrorLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("LogErrorLevelEnabled", value); }
        }

        public static string LogFilePath
        {
            get { return PersistenceProxy.ReadObject("LogFilePath", GetDefaultLoggingFolder()); }
            set { PersistenceProxy.SaveObject("LogFilePath", value); }
        }

        public static int DaysToKeepLogs
        {
            get { return PersistenceProxy.ReadObject("DaysToKeepLogs", 2); }
            set { PersistenceProxy.SaveObject("DaysToKeepLogs", value); }
        }


        public static bool FilterTraceLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("FilterTraceLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("FilterTraceLevelEnabled", value); }
        }

        public static bool FilterInfoLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("FilterInfoLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("FilterInfoLevelEnabled", value); }
        }

        public static bool FilterWarningLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("LogWarningLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("FilterWarningLevelEnabled", value); }
        }

        public static bool FilterErrorLevelEnabled
        {
            get { return PersistenceProxy.ReadObject("FilterErrorLevelEnabled", true); }
            set { PersistenceProxy.SaveObject("FilterErrorLevelEnabled", value); }
        }

        public static int FilterLogLinesCount
        {
            get { return PersistenceProxy.ReadObject("FilterLogLinesCount", 20); }
            set { PersistenceProxy.SaveObject("FilterLogLinesCount", value); }
        }
        #endregion
    }
}
