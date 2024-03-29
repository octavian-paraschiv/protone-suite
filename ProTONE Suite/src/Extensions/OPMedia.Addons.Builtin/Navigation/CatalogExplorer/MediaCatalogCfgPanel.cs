using OPMedia.Addons.Builtin.Configuration;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Configuration;
using OPMedia.UI.Generic;
using System;
using System.Drawing;

namespace OPMedia.Addons.Builtin.CatalogExplorer
{
    public partial class CatalogExplorerCfgPanel : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return ImageProcessing.Library16;
            }
        }

        public CatalogExplorerCfgPanel()
        {
            InitializeComponent();

            this.Title = "TXT_ADDON_MC_SETTINGS";
            this.HandleCreated += new EventHandler(FileExplorerCfgPanel_HandleCreated);

            chkReopenLastCatalog.Checked = BuiltinAddonConfig.MCOpenLastCatalog;
            chkRememberRecentFiles.Checked = BuiltinAddonConfig.MCRememberRecentFiles;
            nudRecentFilesCount.Value = BuiltinAddonConfig.MCRecentFilesCount;

            nudRecentFilesCount.Enabled = chkRememberRecentFiles.Checked;
        }

        void FileExplorerCfgPanel_HandleCreated(object sender, EventArgs e)
        {
            Translator.TranslateControl(this, DesignMode);
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            Modified = true;

            nudRecentFilesCount.Enabled = chkRememberRecentFiles.Checked;
        }

        protected override void SaveInternal()
        {
            BuiltinAddonConfig.MCOpenLastCatalog = chkReopenLastCatalog.Checked;
            BuiltinAddonConfig.MCRememberRecentFiles = chkRememberRecentFiles.Checked;
            BuiltinAddonConfig.MCRecentFilesCount = (int)nudRecentFilesCount.Value;


        }


    }
}
