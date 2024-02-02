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
                return PersistenceProxy.ReadNode(true, "VSplitterDistance", 4 * Screen.PrimaryScreen.Bounds.Width / 9);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "VSplitterDistance", value);
            }
        }

        public static int HSplitterDistance
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "HSplitterDistance", Screen.PrimaryScreen.Bounds.Height / 3 - 50);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "HSplitterDistance", value);
            }
        }

        public static string LastNavAddon
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LastNavAddon", "FileExplorer");
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LastNavAddon", value);
            }
        }

        public static int MaxProcessedEntries
        {
            get { return PersistenceProxy.ReadNode(true, "MaxProcessedEntries", 100); }
            set { PersistenceProxy.SaveNode(true, "MaxProcessedEntries", value); }
        }
    }
}
