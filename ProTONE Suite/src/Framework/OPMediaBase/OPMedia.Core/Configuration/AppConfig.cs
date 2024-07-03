using Iso639;
using OPMedia.Core.InstanceManagement;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Core.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace OPMedia.Core.Configuration
{
    public enum ProxyType
    {
        NotDefined = -1,
        NoProxy = 0,
        HttpProxy,
        Socks4Proxy,
        Socks5Proxy,
        InternetExplorerProxy,
        ApplicationProxy,

        GlobalProxy,
    }

    public class ProxySettings
    {
        public static ProxySettings Empty = new ProxySettings();
        public ProxyType ProxyType = ProxyType.NoProxy;
        public int ProxyPort = 8080;

        public string ProxyAddress { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }

        private ProxySettings()
        {
            PerformTranslation();
        }

        public void PerformTranslation()
        {
            ProxyAddress = Translator.Translate("TXT_DEFINE_PROXYSERVERADDRESS");
            ProxyUser = Translator.Translate("TXT_DEFINE_PROXYSERVERUSERNAME");
            ProxyPassword = string.Empty;
        }
    }

    public static class AppConfig
    {
        static Dictionary<string, Language> _languages = new Dictionary<string, Language>();

        // static string _skinType = string.Empty;
        // static string _languageId = string.Empty;

        public const string UriBase = "http://ocpa.ro";

        const string VersionApiUriBase = UriBase + "/protone?release={0}";
        const string DefaultHelpUriBase = UriBase + "/api/wiki/protone/#VERSION#";

        public const string UnconfiguredThemeName = "Light";

        public static string InstallationPath
        {
            get
            {
                string retVal = string.Empty;
                try
                {
                    if (string.IsNullOrEmpty(retVal))
                    {
                        Assembly asm = Assembly.GetAssembly(typeof(AppConfig));
                        if (asm != null)
                        {
                            retVal = Path.GetDirectoryName(asm.Location);
                        }
                    }
                }
                catch
                {
                    retVal = string.Empty;
                }

                return retVal;
            }
        }

        public static Language GetLanguage(string name)
        {
            if (_languages.ContainsKey(name))
                return _languages[name];

            return LanguageHelper.Lookup("en");
        }

        private static bool IsAppUsingPersistence
        {
            get
            {
                bool ret = false;

                ret |= ApplicationInfo.ApplicationName.ToLowerInvariant().Contains("protone");
                ret |= ApplicationInfo.ApplicationName.ToLowerInvariant().Contains("library");

                return ret;
            }
        }

        static AppConfig()
        {
            _languages.Add("en", LanguageHelper.Lookup("en"));
            _languages.Add("de", LanguageHelper.Lookup("de"));
            _languages.Add("fr", LanguageHelper.Lookup("fr"));
            _languages.Add("ro", LanguageHelper.Lookup("ro"));
        }

        public static void OnSettingsChanged()
        {
            if (OpMediaApplication.AllowRealTimeGUIUpdate)
            {
                lock (_settingsChangesLock)
                {
                    try
                    {
                        MainThread.Post(_ =>
                        {
                            Translator.SetInterfaceLanguage(LanguageID);
                            EventDispatch.DispatchEvent(EventNames.PerformTranslation);
                            EventDispatch.DispatchEvent(EventNames.ThemeUpdated);
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
            }
        }

        #region Generic Purpose API (Level 0 settings)

        private static bool _detectSettingsChanges = true;
        private static object _settingsChangesLock = new object();
        public static bool DetectSettingsChanges
        {
            get
            {
                lock (_settingsChangesLock)
                {
                    return _detectSettingsChanges;
                }
            }
            set
            {
                lock (_settingsChangesLock)
                {
                    _detectSettingsChanges = value;
                }
            }
        }

        public static bool CurrentUserIsAdministrator
        {
            get
            {
                try
                {
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);

                    if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                        return true;

                    if (WindowsVersions.CurrentVersion < WindowsVersions.WinVista)
                        // Operating system does not support UAC; skipping elevation check.
                        return false;

                    int tokenInfLength = Marshal.SizeOf(typeof(int));
                    IntPtr tokenInformation = Marshal.AllocHGlobal(tokenInfLength);

                    try
                    {
                        var token = identity.Token;
                        var result = Advapi32.GetTokenInformation(token,
                            Advapi32.TokenInformationClass.TokenElevationType, tokenInformation, tokenInfLength, out tokenInfLength);

                        if (!result)
                        {
                            var exception = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                            throw new InvalidOperationException("Couldn't get token information", exception);
                        }

                        var elevationType = (Advapi32.TokenElevationType)Marshal.ReadInt32(tokenInformation);

                        switch (elevationType)
                        {
                            case Advapi32.TokenElevationType.TokenElevationTypeDefault:
                                // TokenElevationTypeDefault - User is not using a split token, so they cannot elevate.
                                return false;

                            case Advapi32.TokenElevationType.TokenElevationTypeFull:
                                // TokenElevationTypeFull - User has a split token, and the process is running elevated. Assuming they're an administrator.
                                return true;

                            case Advapi32.TokenElevationType.TokenElevationTypeLimited:
                                // TokenElevationTypeLimited - User has a split token, but the process is not running elevated. Assuming they're an administrator.
                                return true;

                            default:
                                // Unknown token elevation type.
                                return false;
                        }
                    }
                    finally
                    {
                        if (tokenInformation != IntPtr.Zero)
                            Marshal.FreeHGlobal(tokenInformation);
                    }
                }
                catch
                {
                }

                return false;
            }
        }

        public static Language[] SupportedUiLanguages
        {
            get
            {
                return _languages.Values.ToArray();
            }
        }

        public static string SkinType
        {
            get
            {

                if (OpMediaApplication.AllowRealTimeGUIUpdate)
                    return SettingsProxy.Instance.ReadNode("SkinType", UnconfiguredThemeName);

                return UnconfiguredThemeName;
            }

            set
            {
                if (OpMediaApplication.AllowRealTimeGUIUpdate)
                {
                    Logger.LogTrace("AllowRealTimeGUIUpdate => PersistenceProxy.SaveNode(SkinType) called ...");
                    SettingsProxy.Instance.SaveNode("SkinType", value);
                    EventDispatch.DispatchEvent(EventNames.ThemeUpdated);
                }
            }
        }

        public static string LanguageID
        {
            get
            {
                return SettingsProxy.Instance.ReadNode("LanguageID", "en");
            }

            set
            {
                SettingsProxy.Instance.SaveNode("LanguageID", value);
                Translator.SetInterfaceLanguage(value);
            }
        }

        static readonly string _overrideDocLocation =
            SettingsProxy.Instance.ReadNode("OverrideDocumentationLocation", default(string));

        public static string OverrideDocLocation => _overrideDocLocation;

        public static string HelpUriBase
        {
            get
            {
                try
                {
                    if (_overrideDocLocation?.Length > 0)
                    {
                        if (_overrideDocLocation.StartsWith("http", StringComparison.OrdinalIgnoreCase) ||
                            _overrideDocLocation.StartsWith("file", StringComparison.OrdinalIgnoreCase))
                            return _overrideDocLocation;

                        return string.Format("file:///{0}", _overrideDocLocation.Replace("\\", "/"));
                    }

                    string val = DefaultHelpUriBase;

                    if (val?.Length > 0)
                    {
                        Version ver = new Version(SuiteVersion.Version);
                        if (ver.Major == 1)
                            val = val.Replace("#VERSION#", "dev");
                        else
                            val = val.Replace("#VERSION#", string.Format("{0}.{1}",
                                Math.Max(2, ver.Major), ver.Minor));

                        return val;
                    }
                }
                catch { }

                return string.Format("file:///{0}/docs", AppConfig.InstallationPath.Replace("\\", "/"));
            }
        }

        public static string VersionApiUri
        {
            get
            {
                bool release = (SuiteVersion.IsRelease && !Regedit.IsDevelopmentMachine);
                return string.Format(VersionApiUriBase, release ? "true" : "all").ToLowerInvariant();
            }
        }

        public static bool AllowAutomaticUpdates
        {
            get
            {
                if (OpMediaApplication.AllowRealTimeGUIUpdate)
                    return SettingsProxy.Instance.ReadNode("AllowAutomaticUpdates", false);

                return false;
            }

            set
            {
                if (OpMediaApplication.AllowRealTimeGUIUpdate)
                    SettingsProxy.Instance.SaveNode("AllowAutomaticUpdates", value);
            }
        }

        #endregion

        #region Level 1 settings using Settings File (Combined per-app and per-user settings)

        #region Network preferences


        public static IWebProxy GetWebProxy()
        {
            IWebProxy wp = null;
            ProxySettings ps = ProxySettings;

            if (ps == null || ps.ProxyType == ProxyType.NoProxy)
            {
                wp = new WebProxy();
            }
            else if (ps.ProxyType != ProxyType.InternetExplorerProxy)
            {
                wp = new WebProxy(ps.ProxyAddress, ps.ProxyPort);
                wp.Credentials = new NetworkCredential(ps.ProxyUser, ps.ProxyPassword);
                (wp as WebProxy).BypassProxyOnLocal = true;
            }

            return wp;
        }

        public static ProxySettings ProxySettings
        {
            get
            {
                ProxySettings ps = ProxySettings.Empty;
                ps.ProxyAddress = ProxyAddress;
                ps.ProxyPassword = ProxyPassword;
                ps.ProxyPort = ProxyPort;
                ps.ProxyType = ProxyType;
                ps.ProxyUser = ProxyUser;

                return ps;
            }

            set
            {
                ProxyAddress = value.ProxyAddress;
                ProxyPassword = value.ProxyPassword;
                ProxyPort = value.ProxyPort;
                ProxyType = value.ProxyType;
                ProxyUser = value.ProxyUser;
            }
        }

        public static ProxyType ProxyType
        {
            get
            {
                return (ProxyType)SettingsProxy.Instance.ReadNode("ProxyType", (int)ProxyType.NoProxy);
            }
            set
            {
                SettingsProxy.Instance.SaveNode("ProxyType", (int)value);
            }
        }

        public static string ProxyAddress
        {
            get
            {
                return SettingsProxy.Instance.ReadNode("ProxyAddress", "your.proxy.address");
            }
            set
            {
                SettingsProxy.Instance.SaveNode("ProxyAddress", value);
            }
        }

        public static int ProxyPort
        {
            get
            {
                return SettingsProxy.Instance.ReadNode("ProxyPort", 8080);
            }
            set
            {
                SettingsProxy.Instance.SaveNode("ProxyPort", value);
            }
        }

        public static string ProxyUser
        {
            get
            {
                return SettingsProxy.Instance.ReadNode("ProxyUser", "user.name");
            }
            set
            {
                SettingsProxy.Instance.SaveNode("ProxyUser", value);
            }
        }

        public static string ProxyPassword
        {
            get
            {
                return SettingsProxy.Instance.ReadNode("ProxyPassword", string.Empty);
            }
            set
            {
                SettingsProxy.Instance.SaveNode("ProxyPassword", value);
            }
        }
        #endregion

        #region User interface persistence

        public static Point WindowLocation
        {
            get
            {
                Point point = new Point(100, 100);
                if (ApplicationInfo.IsSuiteApplication)
                {
                    point = new Point(Screen.PrimaryScreen.Bounds.Width / 6, Screen.PrimaryScreen.Bounds.Height / 6);
                }

                try
                {
                    string str = SettingsProxy.Instance.ReadNode(true, "WindowLocation", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        point = (Point)new PointConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }

                return point;
            }
            set
            {
                if ((value.X >= 0) && (value.Y >= 0))
                {
                    SettingsProxy.Instance.SaveNode(true, "WindowLocation", new PointConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static Size WindowSize
        {
            get
            {
                Size size = new Size(800, 600);
                if (ApplicationInfo.IsSuiteApplication)
                {
                    size = new Size(2 * Screen.PrimaryScreen.Bounds.Width / 3, 2 * Screen.PrimaryScreen.Bounds.Height / 3);
                }

                try
                {
                    string str = SettingsProxy.Instance.ReadNode(true, "WindowSize", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        size = (Size)new SizeConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }
                return size;
            }
            set
            {
                if ((value.Width >= 0) && (value.Height >= 0))
                {
                    SettingsProxy.Instance.SaveNode(true, "WindowSize", new SizeConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static FormWindowState WindowState
        {
            get
            {
                try
                {
                    return (FormWindowState)SettingsProxy.Instance.ReadNode(true, "WindowState", (int)FormWindowState.Normal);
                }
                catch
                {
                }

                return FormWindowState.Normal;
            }
            set
            {
                SettingsProxy.Instance.SaveNode(true, "WindowState", (int)value);
            }
        }

        public static bool MimimizedToTray
        {
            get
            {
                return (SettingsProxy.Instance.ReadNode(true, "MimimizedToTray", false) && CanSendToTray);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "MimimizedToTray", value && CanSendToTray);
            }
        }

        public static bool CanSendToTray
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "CanSendToTray", false);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "CanSendToTray", value);
            }
        }

        #endregion

        #region Application state persistence
        public static string LastExploredFolder
        {
            get
            {
                return SettingsProxy.Instance.ReadNode(true, "LastExploredFolder", PathUtils.CurrentDir);
            }

            set
            {
                SettingsProxy.Instance.SaveNode(true, "LastExploredFolder", value);
            }
        }
        #endregion

        #endregion

    }
}
