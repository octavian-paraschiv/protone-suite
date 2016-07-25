using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;
using System.Runtime.InteropServices;
using OPMedia.Core.Logging;
using OPMedia.Core.Configuration;

namespace OPMedia.Runtime.InterProcessCommunication
{
    public delegate void DataReceivedHandler(string data);

    public sealed class WmCopyDataReceiver : IDisposable
    {
        private WmCopyDataWindow _wnd;

        public event DataReceivedHandler DataReceived = null;

        public WmCopyDataReceiver(string appName)
        {
            appName = appName.Replace(".", "_").Replace(" ", "").Trim().ToUpperInvariant();
            _wnd = new WmCopyDataWindow(appName);
            _wnd.DataReceived += new DataReceivedHandler(_wnd_DataReceived);
        }

        void _wnd_DataReceived(string data)
        {
            if (DataReceived != null)
            {
                DataReceived(data);
            }
        }

        ~WmCopyDataReceiver()
        {
            Dispose();
        }

        public void Dispose()
        {
            _wnd.DestroyHandle();
        }
    }

    internal sealed class WmCopyDataWindow : NativeWindow
    {
        internal event DataReceivedHandler DataReceived = null;

        string _wndName = "";

        internal WmCopyDataWindow(string wndName)
        {
            _wndName = wndName + "_WMCOPYDATA";

            CreateParams cp = new CreateParams();
            cp.ClassName = "Message";
            cp.Caption = _wndName;
            cp.Width = cp.Height = 0;
            cp.X = cp.Y = 10000;

            try
            {
                CreateHandle(cp);
                Logger.LogTrace("WmCopyDataWindow created,  wndName: {0}", cp.Caption);

                int osVersion = AppConfig.OSVersion;
                if (osVersion == AppConfig.VerWinVista)
                {
                    // Allow WM_COPYDATA through UIPI
                    // On Vista, there is no way to do it per-window; you have to do it per-process
                    User32.ChangeWindowMessageFilter((int)Messages.WM_COPYDATA, ChangeWindowMessageFilterFlags.Add);
                    Logger.LogTrace("[Windows VISTA] Asking UIPI to allow WM_COPYDATA for WmCopyDataWindow ...");
                }
                else if (AppConfig.OSVersion >= AppConfig.VerWin7)
                {
                    // Allow WM_COPYDATA through UIPI
                    CHANGEFILTERSTRUCT cfs = new CHANGEFILTERSTRUCT();
                    cfs.size = (uint)Marshal.SizeOf(cfs);
                    cfs.info = MessageFilterInfo.None;

                    User32.ChangeWindowMessageFilterEx(this.Handle, (int)Messages.WM_COPYDATA, ChangeWindowMessageFilterExAction.Allow, ref cfs);
                    Logger.LogTrace("[Windows 8 or later] Asking UIPI to allow WM_COPYDATA for WmCopyDataWindow ...");
                }

            }
            catch
            {
                int err = Kernel32.GetLastError();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)Messages.WM_COPYDATA:
                    {
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                        string strData = Marshal.PtrToStringUni(cds.lpData);

                        Logger.LogTrace("WmCopyDataWindow wndName: {0} received data: {1}", _wndName, strData);

                        if (DataReceived != null)
                        {
                            DataReceived(strData);
                        }
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
