using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Configuration;
using OPMedia.Core.TranslationSupport;

using OPMedia.Core;
using Microsoft.Win32;
using OPMedia.UI.Properties;
using OPMedia.Runtime.ProTONE.Haali;
using OPMedia.Runtime.ProTONE.FfdShowApi;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.DirectX;
using OPMedia.Runtime.ProTONE.Configuration;

namespace OPMedia.UI.ProTONE.Configuration.MiscConfig
{
    public partial class DiagnosticsPage : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.Assistance;
            }
        }

        public DiagnosticsPage()
        {
            this.Title = "TXT_ASSISTANCE";
            InitializeComponent();
            this.HandleCreated += new EventHandler(SystemDiagnosticsPanel_Load);
        }

        void SystemDiagnosticsPanel_Load(object sender, EventArgs e)
        {
            ScanSystem();
        }

        private void ScanSystem()
        {
            bool globalOK = true;
            globalOK &= DetectDirectX();
            globalOK &= DetectCodecSupport();
            globalOK &= DetectHDSupport();

            pbGlobalStatus.Image = GetStatusImage(globalOK);

            string text = Translator.Translate(globalOK ? "TXT_SYSTEM_OK" : "TXT_SYSTEM_NOT_OK");
            lblGlobalStatus.Text = text;
        }

        private bool DetectHDSupport()
        {
            bool ok = !string.IsNullOrEmpty(HaaliConfig.InstallLocation);

            if (ok)
            {
                lblHDSupport.Text = Translator.Translate("TXT_DIAG_HDSUPPORT_OK");
                lblActHDSupport.Visible = false;
            }
            else
            {
                lblHDSupport.Text = Translator.Translate("TXT_DIAG_HDSUPPORT_NG");
                lblActHDSupport.Visible = true;
            }

            pbHDSupport.Image = GetStatusImage(ok);

            return ok;
        }

        private bool DetectCodecSupport()
        {
            bool ok = !string.IsNullOrEmpty(FfdShowConfig.InstallLocation);

            if (!ok)
            {
                lblCodecSupport.Text = Translator.Translate("TXT_DIAG_CODECSUPPORT_NG");
                lblActCodecSupport.Visible = true;
            }
            else
            {
                lblCodecSupport.Text = Translator.Translate("TXT_DIAG_CODECSUPPORT_OK");
                lblActCodecSupport.Visible = false;
            }

            pbCodecSupport.Image = GetStatusImage(ok);

            return ok;
        }

        private bool DetectDirectX()
        {
            bool isOk = false;

            string dxFriendlyName = "";
            
            Version actualVersion = DirectXConfig.GetDirectXVersion(out dxFriendlyName);
            Version minimumVersion = DirectXConfig.Dx9cVersion;

            isOk = (actualVersion.CompareTo(minimumVersion) >= 0);

            lblDirectX.Text = Translator.Translate("TXT_DIAG_DIRECTX_ACTUAL", dxFriendlyName, actualVersion);
            if (!isOk)
            {
                lblDirectX.Text += "\n" + Translator.Translate("TXT_DIAG_DIRECTX_REQUIRED", minimumVersion);
                lblActDirectX.Visible = true;
            }
            else
            {
                lblActDirectX.Visible = false;
            }

            pbDirectX.Image = GetStatusImage(isOk);

            return isOk;
        }

        private void OnInstallDirectX(object sender, EventArgs e)
        {
            InstallDirectX();
            ScanSystem();
        }

        private void OnInstallCodecSupport(object sender, EventArgs e)
        {
            InstallCodecSupport();
            ScanSystem();
        }

        private void OnInstallHDSupport(object sender, EventArgs e)
        {
            InstallHDSupport();
            ScanSystem();
        }

        private void InstallDirectX()
        {
        }

        private void InstallCodecSupport()
        {
        }

        private void InstallHDSupport()
        {
        }

        private Image GetStatusImage(bool isOK)
        {
            Bitmap bmp = isOK ? Resources.OK : Resources.Error;
            bmp.MakeTransparent(Color.White);

            return bmp;
        }

        private void opmButton1_Click(object sender, EventArgs e)
        {
            ProTONEConfig.DetachedWindowLocation = FindForm().Location;
            ProTONEConfig.DetachedWindowSize = new Size(800, 600);
            EventDispatch.DispatchEvent(OPMedia.UI.ProTONE.GlobalEvents.EventNames.RestoreRenderingRegionPosition);

            ProTONEConfig.OnlineContentBrowser_WindowLocation = FindForm().Location;
            ProTONEConfig.OnlineContentBrowser_WindowSize = new Size(800, 600);
            EventDispatch.DispatchEvent(OPMedia.UI.ProTONE.GlobalEvents.EventNames.RestoreOnlineBrowserPosition);
        }
    }
}
