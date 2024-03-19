using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Generic;
using OPMedia.UI.ProTONE.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.Configuration
{
    public partial class MiscellaneousSettingsPanel : BaseCfgPanel, IMultiPageCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.Miscellaneous;
            }
        }

        public override string GetHelpTopic()
        {
            if (tabMisc.SelectedTab != null)
                return string.Format("{0}/{1}", this.Name, tabMisc.SelectedTab.Name);

            return base.GetHelpTopic();
        }

        public MiscellaneousSettingsPanel()
            : base()
        {
            this.Title = "TXT_S_MISC_SETTINGS";

            InitializeComponent();

            Translator.TranslateControl(this, DesignMode);

            tabMisc.SizeMode = TabSizeMode.Fixed;
            tabMisc.ItemSize = new System.Drawing.Size(110, 28);

            tabMisc.ImageList = new ImageList();
            tabMisc.ImageList.ImageSize = new System.Drawing.Size(24, 24);

            Image bmp = Core.Properties.Resources.DVD;
            tabMisc.ImageList.Images.Add(bmp);

            bmp = ImageProcessing.DVD;
            tabMisc.ImageList.Images.Add(bmp);

            bmp = ImageProcessing.Playlist;
            tabMisc.ImageList.Images.Add(bmp);

            bmp = Resources.IconTime;
            tabMisc.ImageList.Images.Add(bmp);

            tabMisc.ImageList.Images.Add(OPMedia.UI.Properties.Resources.Favorites);

            int i = 0;
            foreach (OPMTabPage tp in tabMisc.TabPages)
            {
                tp.ImageIndex = i++;
            }

            pagePlaylist.ModifiedActive += new EventHandler(OnModifiedActive);
            pageFavoriteFolders.ModifiedActive += new EventHandler(OnModifiedActive);
            pageDisksOptions.ModifiedActive += new EventHandler(OnModifiedActive);
            pageScheduler.ModifiedActive += new EventHandler(OnModifiedActive);

            this.HandleCreated += new EventHandler(MiscellaneousSettingsPanel_HandleCreated);
        }

        void MiscellaneousSettingsPanel_HandleCreated(object sender, EventArgs e)
        {
            tabMisc.SelectedIndex = 0;
        }

        void OnModifiedActive(object sender, EventArgs e)
        {
            Modified = true;
        }

        protected override void SaveInternal()
        {
            pageDisksOptions.Save();
            pagePlaylist.Save();
            pageScheduler.Save();
            pageFavoriteFolders.Save();
            Modified = false;
        }

        void IMultiPageCfgPanel.SelectSubPage(string itemName)
        {
            foreach (OPMTabPage tp in tabMisc.TabPages)
            {
                if (tp.Text == itemName)
                {
                    tabMisc.SelectedTab = tp;
                    break;
                }
            }
        }
    }
}
