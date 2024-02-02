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
                return PersistenceProxy.ReadNode(true, "SearchPaths", string.Empty);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SearchPaths", value);
            }
        }

        public static string SearchTexts
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SearchTexts", string.Empty);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SearchTexts", value);
            }
        }

        public static string SearchPatterns
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SearchPatterns", string.Empty);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SearchPatterns", value);
            }
        }


        public static string SearchTextsMC
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SearchTextsMC", string.Empty);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SearchTextsMC", value);
            }
        }

        public static string SearchPatternsMC
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SearchPatternsMC", string.Empty);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SearchPatternsMC", value);
            }
        }

        public static int SplitterDistanceMC
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SplitterDistanceMC", 200);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SplitterDistanceMC", value);
            }
        }

        public static decimal FEPreviewTimer
        {
            get
            {
                try
                {
                    return (decimal)new DecimalConverter().ConvertFromInvariantString(
                    PersistenceProxy.ReadNode(true, "FEPreviewTimer",
                        new DecimalConverter().ConvertToInvariantString(0.5M)));
                }
                catch { }

                return 0.5M;
            }

            set
            {
                PersistenceProxy.SaveNode(true, "FEPreviewTimer",
                    new DecimalConverter().ConvertToInvariantString(value));
            }
        }

        public static string MCLastOpenedFolder
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "MCLastOpenedFolder", string.Empty);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "MCLastOpenedFolder", value);
            }
        }

        public static bool MCOpenLastCatalog
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "MCOpenLastCatalog", false);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "MCOpenLastCatalog", value);
            }
        }

        public static bool MCRememberRecentFiles
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "MCRememberRecentFiles", false);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "MCRememberRecentFiles", value);
            }
        }

        public static int MCRecentFilesCount
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "MCRecentFilesCount", 5);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "MCRecentFilesCount", value);
            }
        }

        public static string MCRecentFiles
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "MCRecentFiles", string.Empty);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "MCRecentFiles", value);
            }
        }

    }
}
