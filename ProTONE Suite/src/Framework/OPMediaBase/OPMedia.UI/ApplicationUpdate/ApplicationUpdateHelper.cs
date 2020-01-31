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
using Newtonsoft.Json;

namespace OPMedia.UI.ApplicationUpdate
{
    public class ApplicationUpdateHelper : SelfRegisteredEventSinkObject
    {
        static ApplicationUpdateHelper _instance = null;

        BackgroundWorker _bwDetect = null;

        public static ApplicationUpdateHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(ApplicationUpdateHelper))
                    {
                        if (_instance == null)
                            _instance = new ApplicationUpdateHelper();
                    }
                }
                return _instance;
            }
        }

        public bool IsBusy
        {
            get
            {
                return _bwDetect.IsBusy;
            }
        }

        public void CheckUpdates(bool onDemand)
        {
            if (IsBusy)
                return;

            _bwDetect.RunWorkerAsync(onDemand);
        }

        private ApplicationUpdateHelper() : base()
        {
            _bwDetect = new BackgroundWorker();
            _bwDetect.WorkerReportsProgress = _bwDetect.WorkerSupportsCancellation = false;
            _bwDetect.DoWork += new DoWorkEventHandler(OnBackgroundDetect);
            _bwDetect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundDetectComplete);
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
            BuildInfo build = null;
            string msg = null;

            if (detectOnDemand)
                ShowWaitDialog("Please wait while checking for updates ...");

            try
            {
                Version current = new Version(SuiteVersion.Version);

                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(AppConfig.VersionApiUri);
                    var builds = JsonConvert.DeserializeObject<List<BuildInfo>>(json);

                    if (builds != null && builds.Count > 0)
                    {
                        builds.Sort((b1, b2) =>
                        {
                            return b2.Version.CompareTo(b1.Version);
                        });

                        if (builds[0].Version.CompareTo(current) > 0)
                        {
                            Logger.LogInfo("Current version: {0}, available on server: {1}. Update is required.",
                                current, builds[0].Version);

                            build = builds[0];
                        }
                        else
                        {
                            Logger.LogInfo("Current version: {0}, available on server: {1}. Update is NOT required.",
                             current, builds[0].Version);

                            if (detectOnDemand)
                                msg = "TXT_NOUPDATEREQUIRED";
                        }
                    }
                    else
                    {
                        // This version cannot be automatically updated.
                        Logger.LogInfo("Current version: {0} cannot be automatically updated.", current);

                        if (detectOnDemand)
                            msg = Translator.Translate("TXT_NOUPDATEPOSSIBLE", Constants.SuiteName, AppConfig.UriBase);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                if (detectOnDemand)
                {
                    CloseWaitDialog();
                    _waitDialogClosed.WaitOne();
                }

                EventDispatch.DispatchEvent(EventNames.UpdateCheckCompleted, build, msg, detectOnDemand);
            }
        }


        [EventSink(EventNames.UpdateCheckCompleted)]
        public void ProcessNewVersionAvailable(BuildInfo build, string msg, bool onDemand)
        {
            if (build != null)
            {
                DialogResult dlgRes = DialogResult.None;

                if (onDemand)
                {
                    dlgRes = MessageDisplay.Query(
                        Translator.Translate("TXT_NOTIFYUPDATE", build.Version),
                        Translator.Translate("TXT_APP_NAME"),
                        MessageBoxIcon.Question);
                }
                else
                {
                    bool addCheck = false;

                    dlgRes = MessageDisplay.QueryEx(
                        Translator.Translate("TXT_NOTIFYUPDATE", build.Version),
                        Translator.Translate("TXT_APP_NAME"),
                        Translator.Translate("TXT_DISABLEAUTODOWNLOADS"),
                        ref addCheck,
                        MessageBoxIcon.Question);

                    AppConfig.AllowAutomaticUpdates = !addCheck;
                }

                if (dlgRes == DialogResult.Yes)
                {
                    Logger.LogInfo("Started update process to version: {0} from url: {1}", build.Version, build.URL);
                    new UpdateWaitForm(build.URL).ShowDialog("TXT_WAITDOWNLOADUPDATE");
                }
            }
            else if (msg != null)
            {
                EventDispatch.DispatchEvent(EventNames.ShowMessageBox, msg, "TXT_APP_NAME", MessageBoxIcon.Information);
            }
        }

        ~ApplicationUpdateHelper()
        {
            EventDispatch.UnregisterHandler(this);
        }


        GenericWaitDialog _waitDialog = null;
        ManualResetEvent _waitDialogClosed = new ManualResetEvent(false);

        private delegate void ShowWaitDialogDG(string message);
        protected void ShowWaitDialog(string message)
        {
            MainThread.Post((d) =>
            {
                CloseWaitDialog();

                _waitDialogClosed.Reset();
                _waitDialog = new GenericWaitDialog();

                _waitDialog.FormClosed -= OnWaitDialogClosed;
                _waitDialog.FormClosed += OnWaitDialogClosed;

                _waitDialog.ShowDialog(message);
            });
        }

        private void OnWaitDialogClosed(object sender, FormClosedEventArgs e)
        {
            _waitDialogClosed.Set();
        }

        protected void CloseWaitDialog()
        {
            MainThread.Post((d) =>
            {
                if (_waitDialog != null)
                    _waitDialog.Close();
            });
        }
    }
}
