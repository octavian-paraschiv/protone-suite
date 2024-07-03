using System.IO;

namespace OPMedia.Core.Logging
{
    public static class LoggingConfiguration
    {
        public static bool TraceLevelEnabled => true;

        public static bool InfoLevelEnabled => true;

        public static bool WarningLevelEnabled => true;

        public static bool ErrorLevelEnabled => true;

        public static bool LoggingEnabled => true;

        public static int DaysToKeepLogs => 2;

        public const string LogSubfolder = "Logs";


        public static string GetDefaultLoggingFolder()
        {
            try
            {
                string logFolder = PathUtils.ProgramDataDir;
                logFolder = Path.Combine(logFolder, Constants.CompanyName);
                logFolder = Path.Combine(logFolder, Constants.SuiteName);
                logFolder = Path.Combine(logFolder, LogSubfolder);

                if (PathUtils.CanWriteToFolder(logFolder))
                    return logFolder;

                logFolder = Path.Combine(ApplicationInfo.AltLogsFolder, LogSubfolder);
                if (PathUtils.CanWriteToFolder(logFolder))
                    return logFolder;
            }
            catch
            {
            }

            return ".";
        }
    }
}
