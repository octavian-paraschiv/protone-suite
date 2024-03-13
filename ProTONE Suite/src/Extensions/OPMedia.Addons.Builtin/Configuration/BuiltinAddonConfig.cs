using OPMedia.Core;
using System.ComponentModel;

namespace OPMedia.Addons.Builtin.Configuration
{
    public static class BuiltinAddonConfig
    {
        public static string SearchPaths
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "SearchPaths", string.Empty);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "SearchPaths", value);
            }
        }

        public static string SearchTexts
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "SearchTexts", string.Empty);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "SearchTexts", value);
            }
        }

        public static string SearchPatterns
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "SearchPatterns", string.Empty);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "SearchPatterns", value);
            }
        }


        public static string SearchTextsMC
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "SearchTextsMC", string.Empty);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "SearchTextsMC", value);
            }
        }

        public static string SearchPatternsMC
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "SearchPatternsMC", string.Empty);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "SearchPatternsMC", value);
            }
        }

        public static int SplitterDistanceMC
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "SplitterDistanceMC", 200);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "SplitterDistanceMC", value);
            }
        }

        public static decimal FEPreviewTimer
        {
            get
            {
                try
                {
                    return (decimal)new DecimalConverter().ConvertFromInvariantString(
                    SettingsProxy.Instance.ReadNode(true, "FEPreviewTimer",
                        new DecimalConverter().ConvertToInvariantString(0.5M)));
                }
                catch { }

                return 0.5M;
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "FEPreviewTimer",
                    new DecimalConverter().ConvertToInvariantString(value));
            }
        }

        public static string MCLastOpenedFolder
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "MCLastOpenedFolder", string.Empty);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "MCLastOpenedFolder", value);
            }
        }

        public static bool MCOpenLastCatalog
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "MCOpenLastCatalog", false);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "MCOpenLastCatalog", value);
            }
        }

        public static bool MCRememberRecentFiles
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "MCRememberRecentFiles", false);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "MCRememberRecentFiles", value);
            }
        }

        public static int MCRecentFilesCount
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "MCRecentFilesCount", 5);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "MCRecentFilesCount", value);
            }
        }

        public static string MCRecentFiles
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "MCRecentFiles", string.Empty);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "MCRecentFiles", value);
            }
        }

    }
}
