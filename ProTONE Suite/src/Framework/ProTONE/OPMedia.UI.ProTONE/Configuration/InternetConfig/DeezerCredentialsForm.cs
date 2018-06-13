using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Themes;
using OPMedia.Runtime.ProTONE.Configuration;
using System.Net;
using System.Diagnostics;
using System.Threading;
using OPMedia.Core.Logging;
using Microsoft.Win32;

namespace OPMedia.UI.ProTONE.Configuration.InternetConfig
{
    public partial class DeezerCredentialsForm : ToolForm
    {
        string _redirectUri = "";

        public DeezerCredentialsForm()
        {
            InitializeComponent();
            wbDeezer.Navigated += new WebBrowserNavigatedEventHandler(wbDeezer_Navigated);
        }

        void wbDeezer_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string url = e.Url.ToString();

            Logger.LogInfo("DeezerCredentialsForm: URL={0}", url);

            if (url.ToLowerInvariant().StartsWith(_redirectUri))
            {
                string fragment = url.Replace(_redirectUri, string.Empty).Trim('#');
                string[] fields = fragment.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fields != null && fields.Length > 0)
                {
                    foreach (string f in fields)
                    {
                        if (f.StartsWith("access_token"))
                        {
                            wbDeezer.DocumentText = f;

                            string token = f.Replace("access_token=", string.Empty);
                            ProTONEConfig.DeezerUserAccessToken = token;

                            Thread.Sleep(1000);

                            DialogResult = DialogResult.OK;
                            Close();
                            return;
                        }
                    }
                }
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            _redirectUri = txtAppId.Text.ToLowerInvariant();
            string appId = ProTONEConfig.DeezerApplicationId;

            string url = string.Format("https://connect.deezer.com/oauth/auth.php?app_id={0}&redirect_uri={1}&perms=basic_access,offline_access&response_type=token",
                appId, _redirectUri);

            bool bFailed = true;

            try
            {
                string ieVer = "";
                using (var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer"))
                {
                    ieVer = key.GetValue("Version") as string;
                }

                if (ieVer != null)
                {
                    Version ver = new Version(ieVer);
                    Logger.LogInfo($"Installed Internet Explorer version: {ieVer}");

                    Version minVer = new Version("9.0");
                    if (ver >= minVer)
                    {
                        using (var key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
                        {
                            int emuVal = (int)key.GetValue("OPMedia.ProTONE.exe", 0);
                            Logger.LogInfo($"Feature Browser Emulation is set to: {emuVal}");

                            if (emuVal < 9000)
                            {
                                Logger.LogInfo($"Setting Feature Browser Emulation to 9000");
                                key.SetValue("OPMedia.ProTONE.exe", 9000);
                            }
                        }

                        wbDeezer.Navigate(url);
                        bFailed = false;
                    }
                }
                else
                {
                    Logger.LogInfo($"Internet Explorer is not installed on this machine");
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                bFailed = true;
            }

            if (bFailed)
            {
                MessageDisplay.Show("TXT_INTERNET_EXPLORER_HINT", "TXT_APP_NAME", MessageBoxIcon.Exclamation);

                // Open with default browser
                Process.Start(url);
            }
        }
    }
}
