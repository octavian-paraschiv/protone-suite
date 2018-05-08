using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core.Logging;
using System.Threading;
using System.Reflection;
using OPMedia.Core.InstanceManagement;
using System.IO;
using System.Security.AccessControl;
using OPMedia.Core.NetworkAccess;
using OPMedia.Core.Configuration;

namespace OPMedia.Core.Logging
{
    public class LoggedApplication : OpMediaApplication
    {
        protected Mutex _appMutex = null;
        protected string _appMutexName = null;

        public static void Start(string appName, bool allowRealtimeGUISetup)
        {
            if (appInstance == null)
            {
                appInstance = new LoggedApplication();
                (appInstance as LoggedApplication).DoStart(appName, allowRealtimeGUISetup);
            }
            else
            {
                Logger.LogError("Error encountered: {0}",
                    "Only one instance of OpMediaApplication (or derived) can be started per process !!");
            }
        }

        public static void Stop()
        {
            LoggedApplication app = appInstance as LoggedApplication;
            if (app != null)
                app.DoStop();
        }

        public new static void Restart()
        {
            Logger.LogInfo("Application is restarting.");
            OpMediaApplication.Restart();
        }

        protected LoggedApplication()
        {
        }

        ~LoggedApplication()
        {
            Logger.LogInfo("Application has finished.");
        }

        private void OnApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ErrorDispatcher.DispatchFatalError(e.Exception);
        }

        protected override void DoInitialize()
        {
            Logger.LogInfo("Application is starting up ...");

            _appMutexName = _appName.Replace(" ", "").ToLowerInvariant() + @".mutex";

            InstallApplicationEventHandlers();
            RegisterAppMutex();
        }

        protected virtual void RegisterAppMutex()
        {
            bool isNew = false;
            _appMutex = new Mutex(false, "Global\\" + _appMutexName, out isNew);
        }

        private void InstallApplicationEventHandlers()
        {
            Application.ThreadException +=
                new ThreadExceptionEventHandler(OnApplicationThreadException);
        }

        protected override void DoTerminate()
        {
            ReleaseAppMutex();
        }

        protected void ReleaseAppMutex()
        {
            if (_appMutex != null)
            {
                Logger.LogTrace("Tring to release the app instance mutex ...");

                _appMutex.Close();
                _appMutex = null;

                Logger.LogTrace("App instance mutex is released now");

            }
        }
    }
}
