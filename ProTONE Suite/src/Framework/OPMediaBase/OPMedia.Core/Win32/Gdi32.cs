using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;

namespace OPMedia.Core
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public class LOGFONT 
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szFaceName;
    };
    
    public class Gdi32
    {
        const string GDI32 = "gdi32.dll";

        [DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
        public static extern uint SetTextColor(IntPtr hdc, int crColor);

        [DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
        public static extern uint SetBkColor(IntPtr hdc, int crColor);
    }
}
