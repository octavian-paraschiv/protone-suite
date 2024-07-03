using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.DirectX;
using OPMedia.Runtime.ProTONE.FfdShowApi;
using OPMedia.Runtime.ProTONE.Haali;
using OPMedia.UI.Configuration;
using OPMedia.UI.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

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

            lblActCodecSupport.Click += OnInstallCodecSupport;
            lblActHDSupport.Click += OnInstallHDSupport;
            lblActDirectX.Click += OnInstallDirectX;
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
            bool installed = PathUtils.Exists(HaaliConfig.InstallLocation);
            lblHDSupport.Text = Translator.Translate(installed ? "TXT_DIAG_HDSUPPORT_OK" : "TXT_DIAG_HDSUPPORT_NG");
            lblActHDSupport.Visible = !installed;
            pbHDSupport.Image = GetStatusImage(installed);
            return installed;
        }

        private bool DetectCodecSupport()
        {
            bool installed = PathUtils.Exists(FfdShowConfig.InstallLocation);
            lblCodecSupport.Text = Translator.Translate(installed ? "TXT_DIAG_CODECSUPPORT_OK" : "TXT_DIAG_CODECSUPPORT_NG");
            lblActCodecSupport.Visible = !installed;
            pbCodecSupport.Image = GetStatusImage(installed);
            return installed;
        }

        private bool DetectDirectX()
        {
            Version actualVersion = DirectXConfig.GetDirectXVersion(out string dxFriendlyName);
            Version minimumVersion = DirectXConfig.Dx9cVersion;

            bool isOk = (actualVersion.CompareTo(minimumVersion) >= 0);

            lblDirectX.Text = Translator.Translate("TXT_DIAG_DIRECTX_ACTUAL", dxFriendlyName, actualVersion);
            if (!isOk)
                lblDirectX.Text += "\n" + Translator.Translate("TXT_DIAG_DIRECTX_REQUIRED", minimumVersion);

            lblActDirectX.Visible = !isOk;

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
            EventDispatch.DispatchEvent(GlobalEvents.EventNames.RestoreRenderingRegionPosition);

            ProTONEConfig.OnlineContentBrowser_WindowLocation = FindForm().Location;
            ProTONEConfig.OnlineContentBrowser_WindowSize = new Size(800, 600);
            EventDispatch.DispatchEvent(GlobalEvents.EventNames.RestoreOnlineBrowserPosition);
        }

        private void InstallDirectX()
        {
            Process.Start("https://www.google.com/search?q=download+directx");
        }

        private void InstallCodecSupport()
        {
            if (!InstallCOMServer("Codecs\\ffdshow.ax"))
                Process.Start("https://www.google.com/search?q=download+ffdshow+tryouts");
        }

        private void InstallHDSupport()
        {
            if (!InstallCOMServer("HDSupport\\avi.dll") ||
                !InstallCOMServer("HDSupport\\avs.dll") ||
                !InstallCOMServer("HDSupport\\dxr.dll") ||
                !InstallCOMServer("HDSupport\\mkx.dll") ||
                !InstallCOMServer("HDSupport\\mp4.dll") ||
                !InstallCOMServer("HDSupport\\ogm.dll") ||
                !InstallCOMServer("HDSupport\\splitter.ax") ||
                !InstallCOMServer("HDSupport\\ts.dll"))
                Process.Start("https://www.google.com/search?q=download+Haali+media+splitter");
        }

        private bool InstallCOMServer(string subPath)
        {
            try
            {
                var path = Path.Combine(AppConfig.InstallationPath, subPath);
                if (!File.Exists(path))
                    throw new FileNotFoundException(path);

                var regsvrPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "regsvr32.exe");
                if (!File.Exists(regsvrPath))
                    throw new FileNotFoundException(regsvrPath);

                var proc = Process.Start(new ProcessStartInfo
                {
                    Arguments = path,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = regsvrPath
                });

                if (proc?.Id > 0)
                {
                    if (!proc.HasExited)
                        proc.WaitForExit();

                    return true;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return false;
        }
    }
}
