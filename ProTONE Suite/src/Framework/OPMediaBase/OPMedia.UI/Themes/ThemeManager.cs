#region Using directives
using Microsoft.Win32;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.InstanceManagement;
using OPMedia.Core.Logging;
using OPMedia.UI.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

#endregion

namespace OPMedia.UI.Themes
{
    public enum FontSizes
    {
        Smallest = 0,
        Small,
        Normal,
        NormalBold,
        Large,
        VeryLarge,
        ExtremeLarge,
        Huge
    }

    public class FontSizeInfo
    {
        public FontSizes SizeName { get; private set; }
        public float Size { get; private set; }
        public FontStyle Style { get; private set; }

        public FontSizeInfo(FontSizes sizeName, float size, FontStyle style)
        {
            SizeName = sizeName;
            Size = size;
            Style = style;
        }
    }

    public static class ResourceManagerExtension
    {
        public static Bitmap GetImage(this ResourceManager resManager, string name)
        {
            Bitmap bmp = resManager.GetObject(name) as Bitmap;
            if (bmp != null)
            {
                bmp.MakeTransparent(ThemeManager.TransparentColor);
            }
            else
            {
                bmp = ImageProcessing.ColoredBitmap(16, 16, Brushes.White);
            }

            return bmp;
        }
    }

    /// <summary>
    /// Class that deals with skin selection. It provides the colors and
    /// fonts that are to be used in the applications for a specified skin. 
    /// </summary>

    public static class ThemeManager
    {
        static Dictionary<string, Dictionary<string, string>> _allThemesElements = null;

        static FileSystemWatcher _fsw = null;

        const string _fontFamily = "Segoe UI";
        static readonly float _step;

        static readonly Dictionary<FontSizes, FontSizeInfo> _fontSizeInfo = new List<FontSizeInfo>
        {
            new FontSizeInfo ( FontSizes.Smallest,     11, FontStyle.Regular ),
            new FontSizeInfo ( FontSizes.Small,        11, FontStyle.Bold    ),
            new FontSizeInfo ( FontSizes.Normal,       12, FontStyle.Regular ),
            new FontSizeInfo ( FontSizes.NormalBold,   12, FontStyle.Bold    ),
            new FontSizeInfo ( FontSizes.Large,        14, FontStyle.Bold    ),
            new FontSizeInfo ( FontSizes.VeryLarge,    16, FontStyle.Bold    ),
            new FontSizeInfo ( FontSizes.ExtremeLarge, 20, FontStyle.Bold    ),
            new FontSizeInfo ( FontSizes.Huge,         24, FontStyle.Bold    ),

        }.ToDictionary(fi => fi.SizeName);

        static Dictionary<FontSizes, Font> _fonts = new Dictionary<FontSizes, Font>();

        #region Members

        private static ColorConverter cc = new ColorConverter();

        #endregion

        #region Properties

        static string _defaultTheme = "";
        public static string DefaultTheme
        {
            get
            {
                return _defaultTheme;
            }
        }

        public static List<string> Themes
        {
            get
            {
                if (_allThemesElements != null &&
                    _allThemesElements.Count > 0)
                {
                    return new List<String>(_allThemesElements.Keys);
                }

                return new List<string> { string.Empty };
            }
        }

        public static Color ListActiveItemColor
        {
            get
            {
                return ThemeElement("ListActiveItemColor", SafeColorFromString("255, 000, 000"));
            }
        }

        public static Color ListHotItemColor
        { get { return ThemeElement("ListHotItemColor", SafeColorFromString("255, 000, 000")); } }

        public static Color BackColor
        { get { return ThemeElement("BackColor", SafeColorFromString("252, 252, 252")); } }

        public static Color ForeColor
        { get { return ThemeElement("ForeColor", SafeColorFromString("000, 000, 000")); } }

        public static Color HighlightColor
        { get { return ThemeElement("HighlightColor", SafeColorFromString("010, 110, 080")); } }

        public static Color SelectedColor
        { get { return ThemeElement("SelectedColor", SafeColorFromString("159, 207, 255")); } }

        public static Color BorderColor
        { get { return ThemeElement("BorderColor", SafeColorFromString("150, 150, 150")); } }

        #region Normal buttons

        public static Color GradientNormalColor1
        { get { return ThemeElement("GradientNormalColor1", SafeColorFromString("224, 227, 206")); } }

        public static Color GradientNormalColor2
        { get { return ThemeElement("GradientNormalColor2", SafeColorFromString("224, 227, 206")); } }

        public static Color GradientHoverColor1
        { get { return ThemeElement("GradientHoverColor1", SafeColorFromString("159, 207, 255")); } }

        public static Color GradientHoverColor2
        { get { return ThemeElement("GradientHoverColor2", SafeColorFromString("159, 207, 255")); } }

        public static Color GradientFocusColor1
        { get { return ThemeElement("GradientFocusColor1", SafeColorFromString("224, 227, 206")); } }

        public static Color GradientFocusColor2
        { get { return ThemeElement("GradientFocusColor2", SafeColorFromString("224, 227, 206")); } }

        public static Color GradientFocusHoverColor1
        { get { return ThemeElement("GradientFocusHoverColor1", SafeColorFromString("170, 220, 255")); } }

        public static Color GradientFocusHoverColor2
        { get { return ThemeElement("GradientFocusHoverColor2", SafeColorFromString("170, 220, 255")); } }

        #endregion

        #region Default button
        public static Color DefaultButtonNormalColor1
        { get { return ThemeElement("DefaultButtonNormalColor1", SafeColorFromString("224, 227, 206")); } }

        public static Color DefaultButtonNormalColor2
        { get { return ThemeElement("DefaultButtonNormalColor2", SafeColorFromString("224, 227, 206")); } }

        public static Color DefaultButtonHoverColor1
        { get { return ThemeElement("DefaultButtonHoverColor1", SafeColorFromString("159, 207, 255")); } }

        public static Color DefaultButtonHoverColor2
        { get { return ThemeElement("DefaultButtonHoverColor2", SafeColorFromString("159, 207, 255")); } }

        public static Color DefaultButtonFocusColor1
        { get { return ThemeElement("DefaultButtonFocusColor1", SafeColorFromString("224, 227, 206")); } }

        public static Color DefaultButtonFocusColor2
        { get { return ThemeElement("DefaultButtonFocusColor2", SafeColorFromString("224, 227, 206")); } }

        public static Color DefaultButtonFocusHoverColor1
        { get { return ThemeElement("DefaultButtonFocusHoverColor1", SafeColorFromString("170, 220, 255")); } }

        public static Color DefaultButtonFocusHoverColor2
        { get { return ThemeElement("DefaultButtonFocusHoverColor2", SafeColorFromString("170, 220, 255")); } }
        #endregion


        public static Color FocusBorderColor
        { get { return ThemeElement("FocusBorderColor", SafeColorFromString("051, 153, 255")); } }

        public static Color WndValidColor
        { get { return ThemeElement("WndValidColor", SafeColorFromString("255, 255, 255")); } }

        public static Color WndTextColor
        { get { return ThemeElement("WndTextColor", SafeColorFromString("000, 000, 000")); } }

        public static Color LinkColor
        { get { return ThemeElement("LinkColor", SafeColorFromString("018, 097, 225")); } }

        public static Color CheckedMenuColor
        { get { return ThemeElement("CheckedMenuColor", SafeColorFromString("255, 209, 024")); } }

        public static Color CaptionBarColor1
        { get { return ThemeElement("CaptionBarColor1", SafeColorFromString("200, 200, 200")); } }

        public static Color CaptionBarColor2
        { get { return ThemeElement("CaptionBarColor2", SafeColorFromString("200, 200, 200")); } }

        public static Color CaptionButtonColor1
        { get { return ThemeElement("CaptionButtonColor1", SafeColorFromString("224, 227, 206")); } }

        public static Color CaptionButtonColor2
        { get { return ThemeElement("CaptionButtonColor2", SafeColorFromString("224, 227, 206")); } }

        public static Color CaptionCloseButtonColor1
        { get { return ThemeElement("CaptionCloseButtonColor1", SafeColorFromString("225, 100, 100")); } }

        public static Color CaptionCloseButtonColor2
        { get { return ThemeElement("CaptionCloseButtonColor2", SafeColorFromString("225, 100, 100")); } }

        public static Color CaptionButtonForeColor
        { get { return ThemeElement("CaptionButtonForeColor", SafeColorFromString("225, 100, 100")); } }

        public static Color CaptionCloseButtonForeColor
        { get { return ThemeElement("CaptionCloseButtonForeColor", SafeColorFromString("225, 100, 100")); } }

        public static Color SelectedTextColor
        { get { return ThemeElement("SelectedTextColor", SafeColorFromString("000, 000, 000")); } }

        public static Color SeparatorColor
        { get { return ThemeElement("SeparatorColor", SafeColorFromString("150, 150, 150")); } }

        public static Color MenuTextColor
        { get { return ThemeElement("MenuTextColor", SafeColorFromString("000, 000, 000")); } }

        public static Color MenuBackColor
        { get { return ThemeElement("MenuBackColor", SafeColorFromString("255, 000, 000")); } }

        public static Color CheckedMenuTextColor
        { get { return ThemeElement("CheckedMenuTextColor", SafeColorFromString("000, 000, 000")); } }

        public static Color HeaderMenuTextColor
        { get { return ThemeElement("HeaderMenuTextColor", SafeColorFromString("000, 000, 000")); } }

        public static Color HighlightMenuTextColor
        { get { return ThemeElement("HighlightMenuTextColor", SafeColorFromString("000, 000, 000")); } }

        public static Color HighlightHeaderMenuTextColor
        { get { return ThemeElement("HighlightHeaderMenuTextColor", SafeColorFromString("000, 000, 000")); } }



        public static Color GradientGaugeColor1
        { get { return ThemeElement("GradientGaugeColor1", SafeColorFromString("000, 255, 000")); } }

        public static Color GradientGaugeColor2
        { get { return ThemeElement("GradientGaugeColor2", SafeColorFromString("255, 000, 000")); } }

        public static Color GradientGaugeColor1a
        { get { return ThemeElement("GradientGaugeColor1a", SafeColorFromString("255, 242, 000")); } }


        public static Color AltRowColor
        { get { return ThemeElement("AltRowColor", SafeColorFromString("250, 250, 250")); } }

        public static int FormBorderWidth
        { get { return ThemeElement("FormBorderWidth", 3); } }

        public static string ResourcesFolder
        {
            get
            {
                string currentTheme = AppConfig.SkinType;
                return ThemeElement("ResourcesFolder", currentTheme);
            }
        }

        public static Color ColorValidationFailed
        { get { return Color.MistyRose; } }

        public static Color TransparentColor
        { get { return Color.White; } }

        public static Font SmallFont => _fonts[FontSizes.Small];
        public static Font SmallestFont => _fonts[FontSizes.Smallest];
        public static Font NormalFont => _fonts[FontSizes.Normal];
        public static Font NormalBoldFont => _fonts[FontSizes.NormalBold];
        public static Font LargeFont => _fonts[FontSizes.Large];
        public static Font VeryLargeFont => _fonts[FontSizes.VeryLarge];
        public static Font ExtremeLargeFont => _fonts[FontSizes.ExtremeLarge];
        public static Font HugeFont => _fonts[FontSizes.Huge];

        public static bool IsDarkTheme
        {
            get
            {
                float fFore = ThemeManager.ForeColor.GetBrightness();
                float fBack = ThemeManager.BackColor.GetBrightness();
                return (fFore < fBack);
            }
        }

        #endregion

        #region Methods

        public static Font GetFontBySize(FontSizes fnSize)
        {
            return _fonts[fnSize];
        }

        public static void PrepareGraphics(Graphics g)
        {
            if (g != null)
            {
                // g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.AssumeLinear;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.TextContrast = 1;

            }
        }

        private static List<Control> _skinnedControls = new List<Control>();

        public static void SetDoubleBuffer(Control c)
        {
            if (c != null && _skinnedControls.Contains(c) == false)
            {
                if (c.Controls != null && c.Controls.Count > 0)
                {
                    foreach (Control child in c.Controls)
                    {
                        SetDoubleBuffer(child);
                    }
                }

                _skinnedControls.Add(c);
                c.HandleDestroyed += new EventHandler(c_HandleDestroyed);

                SetDoubleBufferControl(c);
            }
        }

        static void c_HandleDestroyed(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && _skinnedControls.Contains(c))
            {
                c.HandleDestroyed -= new EventHandler(c_HandleDestroyed);
                _skinnedControls.Remove(c);
            }
        }

        public static void RecreateFonts()
        {
            _fonts.Clear();

            foreach (FontSizes fs in Enum.GetValues(typeof(FontSizes)))
            {
                var fis = _fontSizeInfo[fs];
                var font = new Font(_fontFamily, _step * fis.Size, fis.Style);
                _fonts.Add(fs, font);
            }
        }

        public static void SetFont(Control ctl, FontSizes size, bool recursive = false)
        {
            try
            {
                if (ctl != null)
                {
                    if (recursive && ctl.Controls != null)
                    {
                        foreach (Control child in ctl.Controls)
                        {
                            SetFont(child, size);
                        }
                    }

                    ctl.Font = _fonts[size];
                    if (ctl is DataGridView)
                    {
                        (ctl as DataGridView).ColumnHeadersDefaultCellStyle.Font = _fonts[size];
                        (ctl as DataGridView).RowTemplate.DefaultCellStyle.Font = _fonts[size];
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Construction
        /// <summary>
        /// Private constructor. The class does not need to be
        /// instantiated, since all of its member are static.
        /// </summary>
        static ThemeManager()
        {
            using (Label l = new Label())
            {
                l.CreateControl();
                using (Graphics g = Graphics.FromHwnd(l.Handle))
                {
                    _step = 72f / g.DpiX;
                }
            }

            InitThemeElements();

            try
            {
                RecreateFonts();
                SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        static void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            RecreateFonts();
        }

        //static void OnThemeChanged(object sender, EventArgs e)
        //{
        //    RecreateFonts();
        //}

        #endregion

        #region Helper methods
        private static void SetDoubleBufferControl(Control c)
        {
            Type t = typeof(Control);
            BindingFlags all = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            PropertyInfo pi = t.GetProperty("DoubleBuffered", all);
            if (pi != null)
            {
                pi.SetValue(c, true, null);
            }

            ControlStyles doubleBufferStyles = ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer;

            FieldInfo fi = t.GetField("controlStyle", all);
            if (fi != null)
            {
                ControlStyles cs = (ControlStyles)fi.GetValue(c);
                if ((cs & doubleBufferStyles) != doubleBufferStyles)
                {
                    cs |= doubleBufferStyles;
                    fi.SetValue(c, cs);
                }
            }
        }
        #endregion

        private static object _loadThemeLock = new object();

        private static void InitThemeElements()
        {
            lock (_loadThemeLock)
            {
                try
                {
                    XDocument doc = null;

                    using (MemoryStream ms = new MemoryStream(OPMedia.UI.Properties.Resources.Themes))
                        doc = XDocument.Load(ms);

                    string themeFolder = Path.Combine(AppConfig.InstallationPath, "Themes");
                    string themeFile = Path.Combine(AppConfig.InstallationPath, "Themes\\Themes.thm");
                    if (File.Exists(themeFile))
                        doc = XDocument.Load(themeFile);

                    if (_fsw == null && Directory.Exists(themeFolder))
                    {
                        _fsw = new FileSystemWatcher(themeFolder, "Themes.thm");
                        _fsw.Changed += new FileSystemEventHandler(OnFileChanged);
                        _fsw.EnableRaisingEvents = true;
                    }

                    if (doc != null)
                    {
                        var allThemes = (from theme in doc.Descendants("Theme")
                                         select new
                                         {
                                             Name = theme.Attribute("Name").Value.Trim(),
                                             IsDefault = theme.Attribute("IsDefault").Value.ToLowerInvariant() == "true"
                                         }).ToList();

                        string lastThemeName = string.Empty;
                        foreach (var theme in allThemes)
                        {
                            lastThemeName = theme.Name;
                            if (theme.IsDefault)
                            {
                                _defaultTheme = theme.Name;
                            }

                            var themeElements = (from themeElement in doc.Descendants("ThemeElement")
                                                 where themeElement.Parent.Attribute("Name").Value == theme.Name
                                                 select new
                                                 {
                                                     Name = themeElement.Attribute("Name").Value.Trim(),
                                                     Value = themeElement.Attribute("Value").Value
                                                 }).ToList();

                            foreach (var themeElement in themeElements)
                                ThemeElement(theme.Name, themeElement.Name, themeElement.Value);
                        }

                        if (string.IsNullOrEmpty(_defaultTheme))
                            _defaultTheme = lastThemeName;
                    }

                    // This is just to enforce reading theme settings from Registry
                    int sz = ThemeElement("CornerSize", 1);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    if (_allThemesElements != null)
                        _allThemesElements.Clear();
                }
            }
        }

        static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (OpMediaApplication.AllowRealTimeGUIUpdate)
            {
                string themeFile = Path.Combine(AppConfig.InstallationPath, "Themes\\Themes.thm");
                if (e.ChangeType == WatcherChangeTypes.Changed && e.FullPath == themeFile)
                {
                    Logger.LogInfo("Theme file changed. Reloading themes ...");

                    lock (_loadThemeLock)
                    {
                        string themeBefore = AppConfig.SkinType;
                        Thread.Sleep(200);
                        InitThemeElements();
                        AppConfig.SkinType = _defaultTheme;
                        AppConfig.SkinType = themeBefore;

                        RecreateFonts();
                    }

                    Logger.LogInfo("Themes reloaded succesfully.");
                }
            }
        }

        #region assignment
        private static void ThemeElement(string themeName, string elementName, string value)
        {
            try
            {
                if (_allThemesElements == null)
                    _allThemesElements = new Dictionary<string, Dictionary<string, string>>();

                if (_allThemesElements.ContainsKey(themeName) == false)
                    _allThemesElements.Add(themeName, new Dictionary<string, string>());

                if (_allThemesElements[themeName].ContainsKey(elementName))
                    _allThemesElements[themeName][elementName] = value;
                else
                    _allThemesElements[themeName].Add(elementName, value);
            }
            catch { }
        }
        #endregion

        #region backwards conversion
        private static int ThemeElement(string elementName, int defaultValue = -1)
        {
            int retVal = defaultValue;
            try
            {
                string val = ThemeElement(elementName, defaultValue.ToString());
                if (int.TryParse(val, out retVal) == false)
                {
                    retVal = defaultValue;
                }
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        private static Color ThemeElement(string elementName, Color defaultValue = default(Color))
        {
            Color retVal = defaultValue;
            try
            {
                string val = ThemeElement(elementName, cc.ConvertToString(defaultValue));
                retVal = SafeColorFromString(val);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static Color SafeColorFromString(string value)
        {
            try
            {
                if (value != "null" && string.IsNullOrEmpty(value) == false)
                    return (Color)cc.ConvertFromInvariantString(value);
            }
            catch
            {
            }

            return Color.Empty;
        }

        static string _previousTheme = AppConfig.UnconfiguredThemeName;

        private static string ThemeElement(string elementName, string defaultValue = null)
        {
            lock (_loadThemeLock)
            {
                string currentTheme = AppConfig.SkinType;

                string elementValue = defaultValue;

                if (_previousTheme != currentTheme)
                    _previousTheme = currentTheme;

                if (_allThemesElements == null)
                    _allThemesElements = new Dictionary<string, Dictionary<string, string>>();

                if (string.IsNullOrEmpty(currentTheme) || _allThemesElements.ContainsKey(currentTheme) == false)
                {
                    currentTheme = _defaultTheme;

                    if (string.IsNullOrEmpty(currentTheme))
                        currentTheme = AppConfig.UnconfiguredThemeName;

                    AppConfig.SkinType = _defaultTheme;
                }

                try
                {
                    if (_allThemesElements.ContainsKey(currentTheme))
                    {
                        if (_allThemesElements[currentTheme].ContainsKey(elementName))
                            return _allThemesElements[currentTheme][elementName];
                    }
                }
                catch
                {
                }

                return defaultValue;
            }
        }

        public static Dictionary<string, string> GetAllThemeElements(string themeName)
        {
            lock (_loadThemeLock)
            {
                if (_allThemesElements != null)
                {
                    return _allThemesElements[themeName];
                }

                return null;
            }
        }

        private static PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private static FontFamily LoadFont(byte[] fontResource)
        {
            int dataLength = fontResource.Length;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontResource, 0, fontPtr, dataLength);

            uint cFonts = 0;
            Gdi32.AddFontMemResourceEx(fontPtr, (uint)fontResource.Length, IntPtr.Zero, ref cFonts);
            privateFontCollection.AddMemoryFont(fontPtr, dataLength);

            return privateFontCollection.Families.Last();
        }


        #endregion
    }
}
