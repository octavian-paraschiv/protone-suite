using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using System.Threading;
using System.IO;
using OPMedia.Core.NetworkAccess;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using System.Windows.Forms;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Dialogs;
using OPMedia.Runtime.AssemblyInfo;
using System.Reflection;
using System.ComponentModel;
using OPMedia.Core.InstanceManagement;
using System.Net;

namespace OPMedia.UI.ApplicationUpdate
{
    public class ApplicationUpdateHelper
    {
        BackgroundWorker _bwDetect = null;

        [EventSink(EventNames.CheckForUpdates)]
        public void CheckUpdates()
        {
            _bwDetect.RunWorkerAsync(true);
        }

        public ApplicationUpdateHelper()
        {
            EventDispatch.RegisterHandler(this);

            _bwDetect = new BackgroundWorker();
            _bwDetect.WorkerReportsProgress = _bwDetect.WorkerSupportsCancellation = false;
            _bwDetect.DoWork += new DoWorkEventHandler(OnBackgroundDetect);
            _bwDetect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundDetectComplete);

            if (AppConfig.AllowAutomaticUpdates)
            {
                _bwDetect.RunWorkerAsync(false);
            }
        }

        void OnBackgroundDetectComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.LogException(e.Error);
            }
        }

        void OnBackgroundDetect(object sender, DoWorkEventArgs e)
        {
            bool detectOnDemand = (bool)e.Argument;

            try
            {
                Version current = new Version(SuiteVersion.Version);

                using (WebClient wc = new WebClient())
                {
                    string availableVersion = wc.DownloadString(AppConfig.VersionApiUri);

                    if (string.IsNullOrEmpty(availableVersion))
                    {
                        // This version cannot be automatically updated.
                        Logger.LogInfo("Current version: {0} cannot be automatically updated.", current);

                        if (detectOnDemand)
                            EventDispatch.DispatchEvent(EventNames.ShowMessageBox, "TXT_NOUPDATEPOSSIBLE", "TXT_APP_NAME", MessageBoxIcon.Information);
                    }
                    else
                    {
                        Version available = new Version(availableVersion);

                        if (available.CompareTo(current) > 0)
                        {
                            Logger.LogInfo("Current version: {0}, available on server: {1}. Update is required.",
                                current, available);

                            EventDispatch.DispatchEvent(EventNames.NewVersionAvailable, availableVersion);
                        }
                        else
                        {
                            Logger.LogInfo("Current version: {0}, available on server: {1}. Update is NOT required.",
                               current, available);

                            if (detectOnDemand)
                                EventDispatch.DispatchEvent(EventNames.ShowMessageBox, "TXT_NOUPDATEREQUIRED", "TXT_APP_NAME", MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    

        [EventSink(EventNames.NewVersionAvailable)]
        public void ProcessNewVersionAvailable(string newVersion)
        {
            DialogResult dlgRes = DialogResult.None;
            bool addCheck = false;

            dlgRes = MessageDisplay.QueryEx(
                Translator.Translate("TXT_NOTIFYUPDATE", newVersion),
                Translator.Translate("TXT_APP_NAME"),
                Translator.Translate("TXT_DISABLEAUTODOWNLOADS"), 
                ref addCheck,
                MessageBoxIcon.Question);

            if (addCheck)
            {
                AppConfig.AllowAutomaticUpdates = false;
            }

            if (dlgRes == DialogResult.Yes)
            {
                Logger.LogInfo("Started update process to version: {0}", newVersion);
                new UpdateWaitForm(newVersion).ShowDialog("TXT_WAITDOWNLOADUPDATE");
            }

            
        }

        ~ApplicationUpdateHelper()
        {
            EventDispatch.UnregisterHandler(this);
        }
    }
}
