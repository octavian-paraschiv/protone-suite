using OPMedia.Core;
using System.Windows.Forms;

namespace OPMedia.Runtime.Addons.Configuration
{
    public static class AddonAppConfig
    {
        public static int VSplitterDistance
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "VSplitterDistance", 4 * Screen.PrimaryScreen.Bounds.Width / 9);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "VSplitterDistance", value);
            }
        }

        public static int HSplitterDistance
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "HSplitterDistance", Screen.PrimaryScreen.Bounds.Height / 3 - 50);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "HSplitterDistance", value);
            }
        }

        public static string LastNavAddon
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "LastNavAddon", "FileExplorer");
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "LastNavAddon", value);
            }
        }

        public static int MaxProcessedEntries
        {
            get { return SettingsProxy.Instance.ReadNode(true, "MaxProcessedEntries", 100); }
            set { SettingsProxy.Instance.SaveNode(true, "MaxProcessedEntries", value); }
        }
    }
}
