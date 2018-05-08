using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;
using System.IO;
using System.Diagnostics;
using OPMedia.Core.Configuration;

namespace OPMedia.Core.InstanceManagement
{
    public abstract class OpMediaApplication
    {
        protected static OpMediaApplication appInstance = null;

        protected string _appName;
        protected bool _allowRealTimeGUIUpdate = false;

        public static bool AllowRealTimeGUIUpdate
        {
            get
            {
                if (appInstance == null)
                    return false;

                return appInstance._allowRealTimeGUIUpdate;
            }
        }

        protected OpMediaApplication()
        {
        }

        protected void DoStart(string appName, bool allowRealTimeGUIUpdate)
        {
            _appName = appName;
            _allowRealTimeGUIUpdate = allowRealTimeGUIUpdate;

            DoInitialize();
        }

        protected void DoStop()
        {
            DoTerminate();
        }

        public static void Restart()
        {
            if (appInstance != null)
            {
                appInstance.DoTerminate();
            }

            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }

        protected abstract void DoInitialize();
        protected abstract void DoTerminate();
    }
}
