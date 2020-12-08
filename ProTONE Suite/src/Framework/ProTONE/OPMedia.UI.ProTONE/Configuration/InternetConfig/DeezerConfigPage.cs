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
using OPMedia.UI.Themes;
using OPMedia.DeezerInterop.OAuth;
using OPMedia.Core.TranslationSupport;
using System.Threading.Tasks;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Core.Utilities;

namespace OPMedia.UI.ProTONE.Configuration.InternetConfig
{
    public partial class DeezerConfigPage : BaseCfgPanel
    {
        static readonly Dictionary<dz_track_quality_t, string> QualityTextHint = new Dictionary<dz_track_quality_t, string>()
        {
            { dz_track_quality_t.DZ_TRACK_QUALITY_CDQUALITY, "CD Quality" },
            { dz_track_quality_t.DZ_TRACK_QUALITY_DATA_EFFICIENT, "Data Efficient" },
            { dz_track_quality_t.DZ_TRACK_QUALITY_HIGHQUALITY, "High Quality" },
            { dz_track_quality_t.DZ_TRACK_QUALITY_STANDARD, "Standard" }
        };

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

            txtDeezerToken.TextChanged += new EventHandler(OnSettingsChanged);
            chkUseServices.CheckedChanged += new EventHandler(OnSettingsChanged);
            btnNew.Image = OPMedia.UI.Properties.Resources.Reload16;

            cmbTrackQuality.DataSource = (from dz_track_quality_t q in Enum.GetValues(typeof(dz_track_quality_t))
                                          where QualityTextHint.ContainsKey(q)
                                          select new
                                          {
                                              Value = q,
                                              Text = QualityTextHint[q]
                                          }).ToList();

            cmbTrackQuality.ValueMember = "Value";
            cmbTrackQuality.DisplayMember = "Text";

            cmbTrackQuality.SelectedValue = ProTONEConfig.DeezerTrackQuality;
        }

        void OnSettingsChanged(object sender, EventArgs e)
        {
            base.Modified = true;
        }

        void DeezerConfigPage_HandleCreated(object sender, EventArgs e)
        {
            txtDeezerToken.Text = ProTONEConfig.DeezerUserAccessToken;
            chkUseServices.Checked = ProTONEConfig.DeezerUseServicesForFileMetadata;
            OnThemeUpdatedInternal();
        }

        protected override void SaveInternal()
        {
            ProTONEConfig.DeezerUserAccessToken = txtDeezerToken.Text;
            ProTONEConfig.DeezerUseServicesForFileMetadata = chkUseServices.Checked;
            ProTONEConfig.DeezerTrackQuality = (dz_track_quality_t)cmbTrackQuality.SelectedValue;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string token = null;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (AuthApi auth = new AuthApi())
                    {
                        token = auth.GetOAuthAccessToken();
                    }
                }
                catch
                {
                }

            }).ContinueWith(_ =>
            {
                if (this.IsDisposed == false && this.Visible)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        MessageDisplay.Show("Something went wrong. Your user could not be successfully authenticated. Please retry.",
                            Translator.Translate("TXT_ERROR"), MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        txtDeezerToken.Text = token;
                    }
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void opmLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://developers.deezer.com/sdk/native#_main_features_and_changes");
        }
    }
}
