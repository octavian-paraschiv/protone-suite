using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.UI.Configuration;
using System;

namespace OPMedia.UI.ProTONE.Configuration.MiscConfig
{
    public partial class DisksOptionsPage : BaseCfgPanel
    {
        public DisksOptionsPage()
        {
            InitializeComponent();

            this.Load += new EventHandler(OnLoad);

            foreach (var x in Enum.GetValues(typeof(CddaInfoSource)))
            {
                string raw = string.Format("TXT_OPT_{0}", x).ToUpperInvariant();
                cmbAudioCdInfoSource.Items.Add(Translator.Translate(raw));
            }

            cmbAudioCdInfoSource.SelectedIndex = (int)ProTONEConfig.AudioCdInfoSource;
            cbDisableDVDMenu.Checked = ProTONEConfig.DisableDVDMenu;
            txtCddbServerName.Text = ProTONEConfig.CddbServerName;
            txtCddbServerPort.Text = ProTONEConfig.CddbServerPort.ToString();

            txtCddbServerName.Visible = txtCddbServerPort.Visible = lblCddbServerName.Visible = lblCddbServerPort.Visible =
                (ProTONEConfig.AudioCdInfoSource >= CddaInfoSource.Cddb);
        }

        void OnLoad(object sender, EventArgs e)
        {
            dvdGroupBox.Visible = VideoDVDHelpers.IsOSSupported;
            dvdGroupBox.Enabled = VideoDVDHelpers.IsOSSupported;

            cmbAudioCdInfoSource.SelectedIndexChanged += new EventHandler(OnSettingsChanged);
            cbDisableDVDMenu.CheckedChanged += new EventHandler(OnSettingsChanged);
            txtCddbServerName.TextChanged += new EventHandler(OnSettingsChanged);
            txtCddbServerPort.TextChanged += new EventHandler(OnSettingsChanged);
        }

        protected override void SaveInternal()
        {
            ProTONEConfig.DisableDVDMenu = cbDisableDVDMenu.Checked;
            ProTONEConfig.AudioCdInfoSource = (CddaInfoSource)cmbAudioCdInfoSource.SelectedIndex;
            ProTONEConfig.CddbServerName = txtCddbServerName.Text;

            int val = 8880;
            int.TryParse(txtCddbServerPort.Text, out val);
            ProTONEConfig.CddbServerPort = val;
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            Modified = true;

            if (sender == cmbAudioCdInfoSource)
            {
                txtCddbServerName.Visible = txtCddbServerPort.Visible = lblCddbServerName.Visible = lblCddbServerPort.Visible =
                    (cmbAudioCdInfoSource.SelectedIndex >= (int)CddaInfoSource.Cddb);
            }
        }


    }


}
