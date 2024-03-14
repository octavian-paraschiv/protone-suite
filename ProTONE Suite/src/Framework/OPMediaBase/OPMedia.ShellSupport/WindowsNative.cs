using System;
using System.Runtime.InteropServices;

namespace OPMedia.Core
{
    public enum ChangeWindowMessageFilterFlags : uint
    {
        Add = 1,
        Remove = 2
    }

    public enum MessageFilterInfo : uint
    {
        None = 0,
        AlreadyAllowed = 1,
        AlreadyDisAllowed = 2,
        AllowedHigher = 3
    }

    public enum ChangeWindowMessageFilterExAction : uint
    {
        Reset = 0,
        Allow = 1,
        DisAllow = 2
    }

    public enum SendMessageTimeoutFlags
    {
        SMTO_NORMAL = 0x0000,
        SMTO_BLOCK = 0x0001,
        SMTO_ABORTIFHUNG = 0x0002,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public UIntPtr dwData;
        public uint cbData;
        public IntPtr lpData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHANGEFILTERSTRUCT
    {
        public uint size;
        public MessageFilterInfo info;
    }

    public static class WindowsVersions
    {
        public const int Win2000 = 50;
        public const int WinXP = 51;

        public const int WinVista = 60;
        public const int Win7 = 61;

        public const int Win8 = 62;
        public const int Win8_1 = 63;

        public const int Win10 = 100;

        public static uint CurrentVersion
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

    public class S_Kernel32
    {
        public const string KERNEL32 = "kernel32.dll";

        [DllImport(KERNEL32, CharSet = CharSet.Auto)]
        internal extern static int GetLastError();

        [DllImport(KERNEL32, CharSet = CharSet.Auto)]
        internal static extern Int32 GlobalSize(IntPtr hmem);
    }

    public class S_User32
    {
        public const string USER32 = "user32.dll";

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(USER32)]
        public static extern bool ChangeWindowMessageFilter(uint message,
            ChangeWindowMessageFilterFlags dwFlag);

        [DllImport(USER32)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd,
            uint msg, ChangeWindowMessageFilterExAction action,
            ref CHANGEFILTERSTRUCT changeInfo);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
           IntPtr windowHandle,
           int Msg,
           IntPtr wParam,
           ref COPYDATASTRUCT cds,
           SendMessageTimeoutFlags flags,
           int timeout,
           out IntPtr result);

        public static bool UIPI_AllowWindowsMessage(IntPtr wndHandle, uint msg, string desc)
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
