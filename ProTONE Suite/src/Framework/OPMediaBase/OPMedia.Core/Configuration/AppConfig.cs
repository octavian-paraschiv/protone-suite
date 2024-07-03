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
        public const int VerWin2000 = 50;
        public const int VerWinXP = 51;

        public const int VerWinVista = 60;
        public const int VerWin7 = 61;

        public const int VerWin8 = 62;
        public const int VerWin8_1 = 63;
        public const int VerWin10 = 100;

        static Dictionary<string, Language> _languages = new Dictionary<string, Language>();

        static string _skinType = string.Empty;
        static string _languageId = string.Empty;

        public const string UriBase = "http://ocpa.ro/protone/";

        const string VersionApiUriBase = UriBase + "?release={0}";
        const string DefaultHelpUriBase = UriBase + "protone-suite-docs/#VERSION#/";

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

            if (IsAppUsingPersistence)
            {
                _skinType = PersistenceProxy.ReadNode("SkinType", UnconfiguredThemeName);

                string defLangId = Regedit.InstallLanguageID;
                _languageId = PersistenceProxy.ReadNode("LanguageID", defLangId);
            }
        }

        public static void OnSettingsChanged(string nodeId, string context, object content)
        {
            if (OpMediaApplication.AllowRealTimeGUIUpdate)
            {
                lock (_settingsChangesLock)
                {
                    try
                    {
                        switch (nodeId)
                        {
                            case "LanguageID":
                                {
                                    string langId = content as string;
                                    if (langId != _languageId)
                                    {
                                        _languageId = langId;
                                        MainThread.Post((c) => Translator.SetInterfaceLanguage(_languageId));
                                    }
                                }
                                break;

                            case "SkinType":
                                {
                                    string skinType = content as string;
                                    if (skinType != _skinType)
                                    {
                                        _skinType = skinType;
                                        MainThread.Post((c) => EventDispatch.DispatchEvent(EventNames.ThemeUpdated));
                                    }
                                }
                                break;
                        }
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

        public static uint OSVersion
        {
            get
            {
                uint winVer = 0;

                winVer += (uint)Environment.OSVersion.Version.Major * 10;
                winVer += (uint)Environment.OSVersion.Version.Minor;

                return winVer;
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

                    if (OSVersion < VerWinVista)
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
                    return _skinType;

                return UnconfiguredThemeName;
            }

            set
            {
                if (OpMediaApplication.AllowRealTimeGUIUpdate && value != _skinType)
                {
                    _skinType = value;

                    Logger.LogTrace("AllowRealTimeGUIUpdate => PersistenceProxy.SaveNode(SkinType) called ...");
                    PersistenceProxy.SaveNode("SkinType", value);

                    EventDispatch.DispatchEvent(EventNames.ThemeUpdated);
                }
            }
        }

        public static string LanguageID
        {
            get
            {
                return _languageId;
            }

            set
            {
                if (value != _languageId)
                {
                    _languageId = value;
                    PersistenceProxy.SaveNode("LanguageID", value);
                    Translator.SetInterfaceLanguage(_languageId);
                }
            }
        }

        public static string HelpUriBase
        {
            get
            {
                try
                {
                    if (UseOnlineDocumentation)
                    {
                        string val = DefaultHelpUriBase;
                        if (!string.IsNullOrEmpty(val))
                        {
                            Version ver = new Version(SuiteVersion.Version);
                            val = val.Replace("#VERSION#", string.Format("{0}.{1}", ver.Major, ver.Minor));
                            return val;
                        }
                    }
                }
                catch { }

                return string.Format("file:///{0}/docs", AppConfig.InstallationPath);
            }
        }

        public static bool UseOnlineDocumentation
        {
            get
            {
                return PersistenceProxy.ReadNode("UseOnlineDocumentation", true);
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
                    return PersistenceProxy.ReadNode("AllowAutomaticUpdates", false);

                return false;
            }

            set
            {
                if (OpMediaApplication.AllowRealTimeGUIUpdate)
                    PersistenceProxy.SaveNode("AllowAutomaticUpdates", value);
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
                return (ProxyType)PersistenceProxy.ReadNode("ProxyType", (int)ProxyType.NoProxy);
            }
            set
            {
                PersistenceProxy.SaveNode("ProxyType", (int)value);
            }
        }

        public static string ProxyAddress
        {
            get
            {
                return PersistenceProxy.ReadNode("ProxyAddress", "your.proxy.address");
            }
            set
            {
                PersistenceProxy.SaveNode("ProxyAddress", value);
            }
        }

        public static int ProxyPort
        {
            get
            {
                return PersistenceProxy.ReadNode("ProxyPort", 8080);
            }
            set
            {
                PersistenceProxy.SaveNode("ProxyPort", value);
            }
        }

        public static string ProxyUser
        {
            get
            {
                return PersistenceProxy.ReadNode("ProxyUser", "user.name");
            }
            set
            {
                PersistenceProxy.SaveNode("ProxyUser", value);
            }
        }

        public static string ProxyPassword
        {
            get
            {
                return PersistenceProxy.ReadNode("ProxyPassword", string.Empty);
            }
            set
            {
                PersistenceProxy.SaveNode("ProxyPassword", value);
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
                    string str = PersistenceProxy.ReadNode(true, "WindowLocation", string.Empty);
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
                    PersistenceProxy.SaveNode(true, "WindowLocation", new PointConverter().ConvertToInvariantString(value));
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
                    string str = PersistenceProxy.ReadNode(true, "WindowSize", string.Empty);
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
                    PersistenceProxy.SaveNode(true, "WindowSize", new SizeConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static FormWindowState WindowState
        {
            get
            {
                try
                {
                    return (FormWindowState)PersistenceProxy.ReadNode(true, "WindowState", (int)FormWindowState.Normal);
                }
                catch
                {
                }

                return FormWindowState.Normal;
            }
            set
            {
                PersistenceProxy.SaveNode(true, "WindowState", (int)value);
            }
        }

        public static bool MimimizedToTray
        {
            get
            {
                return (PersistenceProxy.ReadNode(true, "MimimizedToTray", false) && CanSendToTray);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "MimimizedToTray", value && CanSendToTray);
            }
        }

        public static bool CanSendToTray
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "CanSendToTray", false);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "CanSendToTray", value);
            }
        }

        #endregion

        #region Application state persistence
        public static string LastExploredFolder
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LastExploredFolder", PathUtils.CurrentDir);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LastExploredFolder", value);
            }
        }
        #endregion

        #endregion

    }
}
