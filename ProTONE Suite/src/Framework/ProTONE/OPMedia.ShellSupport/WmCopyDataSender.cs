using OPMedia.Core;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OPMedia.ShellSupport
{
    public static class WmCopyDataSender
    {
        public static bool SendData(string appName, string data)
        {
            string wndName = appName.Replace(".", "_").Replace(" ", "").Trim().ToUpperInvariant()
                + "_WMCOPYDATA";

            IntPtr hWnd = S_User32.FindWindow(null, wndName);
            if (hWnd == IntPtr.Zero)
                return false;

            byte[] sb = Encoding.Unicode.GetBytes(data);

            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = UIntPtr.Zero;
            cds.lpData = Marshal.StringToHGlobalUni(data);
            cds.cbData = (uint)S_Kernel32.GlobalSize(cds.lpData);

            IntPtr ret = IntPtr.Zero;
            S_User32.SendMessageTimeout(hWnd, (int)WmCopyDataWindow.WM_COPYDATA, IntPtr.Zero, ref cds,
                SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 200, out ret);

            if (ret.ToInt32() != 1)
                return false;

            return true;
        }
    }
}
