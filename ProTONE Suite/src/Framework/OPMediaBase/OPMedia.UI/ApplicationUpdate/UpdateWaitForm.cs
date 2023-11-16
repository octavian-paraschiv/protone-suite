using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.NetworkAccess;
using OPMedia.UI.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OPMedia.UI.ApplicationUpdate
{
    public partial class UpdateWaitForm : GenericWaitDialog
    {
        WebFileRetriever _wfr = null;
        string _file = string.Empty;

        public UpdateWaitForm(string file) : base()
        {
            _file = file;

            InitializeComponent();

            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ShowProgress = true;

            this.Shown += OnShown;
            this.FormClosing += new FormClosingEventHandler(UpdateWaitForm_FormClosing);
        }

        private void OnShown(object sender, EventArgs e)
        {
            StartDownload();
        }

        protected override bool AllowCloseOnKeyDown(Keys key)
        {
            return (key == Keys.Escape);
        }

        void UpdateWaitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbortDownload();
        }

        private void StartDownload()
        {
            string tmpFileName = Path.GetFileName(_file);
            string tempFile = Path.Combine(Path.GetTempPath(), tmpFileName);

            Logger.LogInfo("Downloading update file from {0} ...", _file);

            _wfr = new WebFileRetriever(AppConfig.ProxySettings, _file, tempFile);
            _wfr.FileRetrieveComplete += OnDownloadComplete;
            _wfr.DownloadProgress += OnDownloadProgress;
            _wfr.PerformDownload(true);
        }

        private void OnDownloadProgress(int percentage, long bytesReceived, long totalBytes)
        {
            double perc = (100f * (double)bytesReceived / (double)totalBytes);
            base.SetProgress(perc);
        }

        void OnDownloadComplete(string path, bool success, bool cancelled, string errorDetails)
        {
            if (success)
            {
                Logger.LogInfo("Update file downloaded succesfully and saved as {0}", path);

                AbortDownload();
                if (LaunchSetup(path))
                    Process.GetCurrentProcess().Kill();
            }
            else
            {
                if (cancelled)
                {
                    // Manually cancelled.
                    Logger.LogInfo("Update process was manually cancelled.");
                }
                else
                {
                    Logger.LogInfo("Failed to download update file. Details: ", errorDetails);
                    MessageDisplay.Show(errorDetails, "TXT_APP_NAME", MessageBoxIcon.Question);
                    AbortDownload();
                }

                Close();
            }
        }

        private bool LaunchSetup(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path);

            psi.Arguments = string.Format("/SILENT /SUPRESSMSGBOXES /APPRESTART \"{0}\"",
                ApplicationInfo.ApplicationLaunchPath);

            psi.ErrorDialog = false;
            psi.UseShellExecute = true;
            psi.WorkingDirectory = Path.GetTempPath();

            try
            {
                Process.Start(psi);
                return true;
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
                return false;
            }
        }

        private void AbortDownload()
        {
            if (_wfr != null)
            {
                _wfr.Dispose();
                _wfr = null;
            }
        }

    }
}
