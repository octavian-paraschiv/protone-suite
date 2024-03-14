using OPMedia.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OPMedia.ShellSupport
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

        public const uint WM_COPYDATA = 0x004A;

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
                S_User32.UIPI_AllowWindowsMessage(this.Handle, WM_COPYDATA, "WmCopyDataWindow");
            }
            catch
            {
                int err = S_Kernel32.GetLastError();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WM_COPYDATA:
                    {
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                        string strData = Marshal.PtrToStringUni(cds.lpData);

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
