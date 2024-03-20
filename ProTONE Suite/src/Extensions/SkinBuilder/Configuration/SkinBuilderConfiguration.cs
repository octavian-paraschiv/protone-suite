using OPMedia.Core;

namespace OPMedia.SkinBuilder.Configuration
{
    public class SkinBuilderConfiguration
    {
        public static string LastOpenedFolder
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "LastOpenedFolder", string.Empty);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "LastOpenedFolder", value);
            }
        }

        public static bool OpenLastFile
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "OpenLastFile", false);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "OpenLastFile", value);
            }
        }

        public static bool RememberRecentFiles
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "RememberRecentFiles", false);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "RememberRecentFiles", value);
            }
        }

        public static int RecentFilesCount
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "RecentFilesCount", 5);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "RecentFilesCount", value);
            }
        }

        public static string RecentFiles
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "RecentFiles", string.Empty);
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "RecentFiles", value);
            }
        }
    }
}
