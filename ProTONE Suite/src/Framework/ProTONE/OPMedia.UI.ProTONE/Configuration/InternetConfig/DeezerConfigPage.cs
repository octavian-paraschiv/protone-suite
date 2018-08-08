using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Configuration;
using OPMedia.UI.ProTONE.Properties;
using System.Diagnostics;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering.DS;

namespace OPMedia.UI.ProTONE.Configuration.InternetConfig
{
    public partial class DeezerConfigPage : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.deezer16;
            }
        }

        public DeezerConfigPage()
        {
            InitializeComponent();
            this.Title = "Deezer";
            this.HandleCreated += new EventHandler(DeezerConfigPage_HandleCreated);

            txtDeezerAppID.InnerTextBox.TextChanged += new EventHandler(OnSettingsChanged);
            txtDeezerToken.InnerTextBox.TextChanged += new EventHandler(OnSettingsChanged);
            chkUseWorkerProcess.CheckedChanged += new EventHandler(OnSettingsChanged);

            btnNew.Image = OPMedia.UI.Properties.Resources.Reload16;
        }

        void OnSettingsChanged(object sender, EventArgs e)
        {
            base.Modified = true;
        }

        void DeezerConfigPage_HandleCreated(object sender, EventArgs e)
        {
            txtDeezerAppID.Text = ProTONEConfig.DeezerApplicationId;
            txtDeezerToken.Text = ProTONEConfig.DeezerUserAccessToken;
            chkUseWorkerProcess.Checked = ProTONEConfig.DeezerUseWorkerProcess;
        }

        protected override void SaveInternal()
        {
            ProTONEConfig.DeezerApplicationId = txtDeezerAppID.Text;
            ProTONEConfig.DeezerUserAccessToken = txtDeezerToken.Text;
            ProTONEConfig.DeezerUseWorkerProcess = chkUseWorkerProcess.Checked;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ProTONEConfig.DeezerApplicationId = txtDeezerAppID.Text;

            new DeezerCredentialsForm().ShowDialog();
            DeezerConfigPage_HandleCreated(sender, e);
        }

        private void chkUseWorkerProcess_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
