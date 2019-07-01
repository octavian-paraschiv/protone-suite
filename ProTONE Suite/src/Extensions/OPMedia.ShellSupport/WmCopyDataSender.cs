using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OPMedia.ShellSupport;

namespace OPMedia.LiteCore
{
    public static class WmCopyDataSender
    {
        /// <summary>
        /// The CopyData Constant for SendMessage
        /// </summary>
        public const Int32 WM_COPYDATA = 0x004A;

        public static bool SendData(string appName, string data)
        {
            Trace.WriteLine($"WmCopyDataSender.SendData to {appName}: {data}");

            string wndName = appName.Replace(".", "_").Replace(" ", "").Trim().ToUpperInvariant()
                +"_WMCOPYDATA";

            IntPtr hWnd = DllImports.FindWindow(null, wndName);
            if (hWnd == IntPtr.Zero)
            {
                Trace.WriteLine($"WmCopyDataSender.SendData to {appName}: {data} ... window not found: {wndName}");
                return false;
            }

            byte[] sb = Encoding.Unicode.GetBytes(data);

            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = UIntPtr.Zero;
            cds.lpData = Marshal.StringToHGlobalUni(data);
            cds.cbData = (uint)DllImports.GlobalSize(cds.lpData);

            IntPtr ret = IntPtr.Zero;
            DllImports.SendMessageTimeout(hWnd, WM_COPYDATA, IntPtr.Zero, ref cds,
                SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 200, out ret);

            if (ret.ToInt32() != 1)
                return false;

            return true;
        }
    }
}
