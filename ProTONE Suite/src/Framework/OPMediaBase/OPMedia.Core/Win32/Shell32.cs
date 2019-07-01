using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OPMedia.Core
{
    [Flags]
    public enum SHGetFileInfoConstants : int
    {
        SHGFI_ICON = 0x100,                // get icon 
        SHGFI_DISPLAYNAME = 0x200,         // get display name 
        SHGFI_TYPENAME = 0x400,            // get type name 
        SHGFI_ATTRIBUTES = 0x800,          // get attributes 
        SHGFI_ICONLOCATION = 0x1000,       // get icon location 
        SHGFI_EXETYPE = 0x2000,            // return exe type 
        SHGFI_SYSICONINDEX = 0x4000,       // get system icon index 
        SHGFI_LINKOVERLAY = 0x8000,        // put a link overlay on icon 
        SHGFI_SELECTED = 0x10000,          // show icon in selected state 
        SHGFI_ATTR_SPECIFIED = 0x20000,    // get only specified attributes 
        SHGFI_LARGEICON = 0x0,             // get large icon 
        SHGFI_SMALLICON = 0x1,             // get small icon 
        SHGFI_OPENICON = 0x2,              // get open icon 
        SHGFI_SHELLICONSIZE = 0x4,         // get shell size icon 
        SHGFI_USEFILEATTRIBUTES = 0x10,     // use passed dwFileAttribute 
        SHGFI_ADDOVERLAYS = 0x000000020,     // apply the appropriate overlays
        SHGFI_OVERLAYINDEX = 0x000000040     // Get the index of the overlay
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public int dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Kernel32.MAX_PATH)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    /// <summary>
    /// Helper class that holds all the data types and unmanaged
    /// functions imported from Shell32.dll. Refer to 
    /// MSDN documentation for further information.
    /// </summary>
    public static class Shell32
    {
        const string SHELL32 = "Shell32.dll";

        public static readonly Guid IID_IImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

        [DllImport(SHELL32)]
        public extern static int SHGetImageList(int iImageList, ref Guid riid, ref IntPtr hIL);

        [DllImport(SHELL32)]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi,
            uint cbFileInfo, uint uFlags);

        [DllImport(SHELL32, CharSet = CharSet.Auto)]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, 
            IntPtr[] phiconSmall, uint nIcons);
    }


    public static class ComCtl32
    {
        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr ImageList_GetIcon(IntPtr hIL, int i, int flags);
    }
}
