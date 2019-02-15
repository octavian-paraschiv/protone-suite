using System;
using System.Collections.Generic;
using System.Text;
using OPMedia.Core.Logging;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Xml;
using OPMedia.Core.Configuration;
using OPMedia.Core;
using OPMedia.Core.Utilities;

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


        public static string GetDefaultLoggingFolder()
        {
            try
            {
                string installPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string defaultPath = Path.Combine(installPath, "Logs");

                if (PathUtils.CanWriteToFolder(defaultPath))
                    return defaultPath;

                defaultPath = Path.Combine(ApplicationInfo.AltLogsFolder, "Logs");
                if (PathUtils.CanWriteToFolder(defaultPath))
                    return defaultPath;
            }
            catch
            {
            }

            return ".";
        }
    }
}
