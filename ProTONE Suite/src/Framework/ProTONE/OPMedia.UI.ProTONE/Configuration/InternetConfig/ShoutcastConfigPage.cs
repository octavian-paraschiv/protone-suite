using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.UI.Configuration;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.UI.Themes;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.Configuration.InternetConfig
{
    public partial class ShoutcastConfigPage : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.Shoutcast;
            }
        }

        public ShoutcastConfigPage()
        {
            InitializeComponent();
            this.Title = "ShoutCast";

            this.HandleCreated += new EventHandler(ShoutcastConfigPage_HandleCreated);

            txtShoutcastDevId.TextChanged += new EventHandler(OnTextChanged);
            txtShoutcastSearchURL.TextChanged += new EventHandler(OnTextChanged);
            txtShoutcastTuneinURL.TextChanged += new EventHandler(OnTextChanged);
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            base.Modified = true;
        }

        void ShoutcastConfigPage_HandleCreated(object sender, EventArgs e)
        {
            txtShoutcastDevId.Text = ProTONEConfig.ShoutCastApiDevID;
            txtShoutcastSearchURL.Text = ProTONEConfig.ShoutCastSearchBaseURL;
            txtShoutcastTuneinURL.Text = ProTONEConfig.ShoutCastTuneInBaseURL;

            this.Enabled = AppConfig.CurrentUserIsAdministrator;
            OnThemeUpdatedInternal();
        }

        protected override void SaveInternal()
        {
            ProTONEConfig.ShoutCastApiDevID = txtShoutcastDevId.Text;
            ProTONEConfig.ShoutCastSearchBaseURL = txtShoutcastSearchURL.Text;
            ProTONEConfig.ShoutCastTuneInBaseURL = txtShoutcastTuneinURL.Text;
        }

        protected override void OnThemeUpdatedInternal()
        {
            lblHint.OverrideForeColor = ThemeManager.LinkColor;
        }

        private void opmLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.shoutcast.com/Developer");
        }
    }
}
