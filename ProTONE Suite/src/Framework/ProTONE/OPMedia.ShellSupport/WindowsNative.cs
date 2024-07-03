using System;
using System.Runtime.InteropServices;

namespace OPMedia.Core
{
    internal enum ChangeWindowMessageFilterFlags : uint
    {
        Add = 1,
        Remove = 2
    }

    internal enum MessageFilterInfo : uint
    {
        None = 0,
        AlreadyAllowed = 1,
        AlreadyDisAllowed = 2,
        AllowedHigher = 3
    }

    internal enum ChangeWindowMessageFilterExAction : uint
    {
        Reset = 0,
        Allow = 1,
        DisAllow = 2
    }

    internal enum SendMessageTimeoutFlags
    {
        SMTO_NORMAL = 0x0000,
        SMTO_BLOCK = 0x0001,
        SMTO_ABORTIFHUNG = 0x0002,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct COPYDATASTRUCT
    {
        internal UIntPtr dwData;
        internal uint cbData;
        internal IntPtr lpData;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CHANGEFILTERSTRUCT
    {
        internal uint size;
        internal MessageFilterInfo info;
    }

    internal static class WindowsVersions
    {
        internal const int Win2000 = 50;
        internal const int WinXP = 51;

        internal const int WinVista = 60;
        internal const int Win7 = 61;

        internal const int Win8 = 62;
        internal const int Win8_1 = 63;

        internal const int Win10 = 100;

        internal static uint CurrentVersion
        {
            get
            {
                uint winVer = 0;

                winVer += (uint)Environment.OSVersion.Version.Major * 10;
                winVer += (uint)Environment.OSVersion.Version.Minor;

                return winVer;
            }
        }
    }

    internal static class S_Kernel32
    {
        internal const string KERNEL32 = "kernel32.dll";

        [DllImport(KERNEL32, CharSet = CharSet.Auto)]
        internal extern static int GetLastError();

        [DllImport(KERNEL32, CharSet = CharSet.Auto)]
        internal static extern Int32 GlobalSize(IntPtr hmem);
    }

    internal static class S_User32
    {
        internal const string USER32 = "user32.dll";

        [DllImport(USER32, CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(USER32)]
        internal static extern bool ChangeWindowMessageFilter(uint message,
            ChangeWindowMessageFilterFlags dwFlag);

        [DllImport(USER32)]
        internal static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd,
            uint msg, ChangeWindowMessageFilterExAction action,
            ref CHANGEFILTERSTRUCT changeInfo);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessageTimeout(
           IntPtr windowHandle,
           int Msg,
           IntPtr wParam,
           ref COPYDATASTRUCT cds,
           SendMessageTimeoutFlags flags,
           int timeout,
           out IntPtr result);

        internal static bool UIPI_AllowWindowsMessage(IntPtr wndHandle, uint msg, string desc)
        {
            bool ret = true;

            uint osVersion = WindowsVersions.CurrentVersion;
            decimal ver = osVersion / 10;

            if (osVersion == WindowsVersions.WinVista)
            {
                // Allow WM_COPYDATA through UIPI
                // On Vista, there is no way to do it per-window; you have to do it per-process
                ret = S_User32.ChangeWindowMessageFilter(msg, ChangeWindowMessageFilterFlags.Add);
            }
            else if (osVersion >= WindowsVersions.Win7)
            {
                // Allow WM_COPYDATA through UIPI
                CHANGEFILTERSTRUCT cfs = new CHANGEFILTERSTRUCT();
                cfs.size = (uint)Marshal.SizeOf(cfs);
                cfs.info = MessageFilterInfo.None;

                ret = ChangeWindowMessageFilterEx(wndHandle, msg, ChangeWindowMessageFilterExAction.Allow, ref cfs);
            }
            else
            {
                ret = true;
            }

            return ret;
        }
    }
}
