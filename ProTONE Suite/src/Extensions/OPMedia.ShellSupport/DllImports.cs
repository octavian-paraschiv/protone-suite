using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace OPMedia.ShellSupport
{
    public static class DllImports
    {
        const string OLE32 = "ole32.dll";
        const string SHELL32 = "shell32.dll";
        const string USER32 = "user32.dll";
        const string KERNEL32 = "kernel32.dll";

        [DllImport(OLE32)]
        public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

        [DllImport(SHELL32, CharSet = CharSet.Auto)]
        public static extern uint DragQueryFile(IntPtr hDrop, uint iFile, StringBuilder buffer, int cch);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern bool InsertMenu(IntPtr hmenu, uint position, MFMENU uflags, IntPtr uIDNewItemOrSubmenu, string text);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern bool SetMenuItemBitmaps(IntPtr hMenu, uint uPosition, MFMENU uFlags,
            IntPtr hBitmapUnchecked, IntPtr hBitmapChecked);

        [DllImport(SHELL32, CharSet = CharSet.Auto)]
        public static extern void SHChangeNotify(HChangeNotifyEventID wEventId,
                                 HChangeNotifyFlags uFlags,
                                 IntPtr dwItem1,
                                 IntPtr dwItem2);

        // Import the GlobalSize function
        [DllImport(KERNEL32, CharSet = CharSet.Auto)]
        public static extern Int32 GlobalSize(IntPtr hmem);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
           IntPtr windowHandle,
           int Msg,
           IntPtr wParam,
           ref COPYDATASTRUCT cds,
           SendMessageTimeoutFlags flags,
           int timeout,
           out IntPtr result);

        [DllImport(USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            [MarshalAs(UnmanagedType.U4)]
                int Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            int timeout,
            out IntPtr result);
    }
}
