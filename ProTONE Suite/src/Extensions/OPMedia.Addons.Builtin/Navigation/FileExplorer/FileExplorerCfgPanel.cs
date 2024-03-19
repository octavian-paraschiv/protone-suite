using OPMedia.Addons.Builtin.Configuration;
using OPMedia.Addons.Builtin.Properties;
using OPMedia.Core;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.Addons.Configuration;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.ShellSupport;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OPMedia.Addons.Builtin.FileExplorer
{
    public partial class FileExplorerCfgPanel : BaseCfgPanel
    {
        OPMToolTip _tip = new OPMToolTip();

        public override Image Image
        {
            get
            {
                return Resources.FileExplorer.Resize(false);
            }
        }

        public FileExplorerCfgPanel()
        {
            InitializeComponent();

            this.Title = "TXT_ADDON_FE_SETTINGS";
            this.HandleCreated += new EventHandler(FileExplorerCfgPanel_HandleCreated);

            connectedFilesConfigCtl1.ModifiedActive += new EventHandler(OnSettingsChanged);
        }

        void FileExplorerCfgPanel_HandleCreated(object sender, EventArgs e)
        {
            Translator.TranslateControl(this, DesignMode);

            nudMaxProcessedFiles.Value = AddonAppConfig.MaxProcessedEntries;
            nudPreviewTimer.Value = BuiltinAddonConfig.FEPreviewTimer;

            Dictionary<string, string> tableLinkedFiles = ProTONEConfig.LinkedFilesTable;
            if (tableLinkedFiles.Count < 1)
            {
                List<string> supChildrenForAudioTypes = new List<string>();
                supChildrenForAudioTypes.Add("BMK");

                List<string> supChildrenForVideoTypes = new List<string>();
                supChildrenForVideoTypes.AddRange(SupportedFileProvider.Instance.SupportedSubtitles);
                supChildrenForVideoTypes.Add("BMK");

                tableLinkedFiles.Add(
                    StringUtils.FromStringArray(SupportedFileProvider.Instance.SupportedAudioTypes.ToArray(), ';'),
                    StringUtils.FromStringArray(supChildrenForAudioTypes.ToArray(), ';'));

                tableLinkedFiles.Add(
                    StringUtils.FromStringArray(SupportedFileProvider.Instance.SupportedVideoTypes.ToArray(), ';'),
                    StringUtils.FromStringArray(supChildrenForVideoTypes.ToArray(), ';'));

                ProTONEConfig.LinkedFilesTable = new Dictionary<string, string>(tableLinkedFiles);
            }
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        protected override void SaveInternal()
        {
            connectedFilesConfigCtl1.Save();
            AddonAppConfig.MaxProcessedEntries = (int)nudMaxProcessedFiles.Value;
            BuiltinAddonConfig.FEPreviewTimer = nudPreviewTimer.Value;

        }
    }
}
