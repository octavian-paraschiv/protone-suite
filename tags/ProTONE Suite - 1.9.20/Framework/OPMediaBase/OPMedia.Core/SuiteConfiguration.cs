using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.ServiceProcess;
using System.Drawing;
using System.Globalization;
using OPMedia.Core.TranslationSupport;
using System.Runtime.InteropServices;
using OPMedia.Core.Logging;

namespace OPMedia.Core
{
    public static class SuiteConfiguration
    {
        public const int VerWin2000 =  50;
        public const int VerWinXP =    51;
        public const int VerWinVista = 60;
        public const int VerWin7     = 61;

        static readonly string ConfigRegPath = 
            string.Format("Software\\{0}\\{1}", Constants.CompanyName, Constants.SuiteName);

        static object _languageSyncRoot = new object();

        static System.Windows.Forms.Timer _tmrReadRegistry = null;

        static Dictionary<string, CultureInfo> _cultures = new Dictionary<string, CultureInfo>();

        static SuiteConfiguration()
        {
            _cultures.Add("en", new CultureInfo("en"));
            _cultures.Add("de", new CultureInfo("de"));
            _cultures.Add("fr", new CultureInfo("fr"));
            _cultures.Add("ro", new CultureInfo("ro"));
            _cultures.Add("hu", new CultureInfo("hu"));
            _cultures.Add("ru", new CultureInfo("ru"));

            lock (_languageSyncRoot)
            {
                try
                {
                    // Is the install language setting overriden by current user ?
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ConfigRegPath))
                    {
                        if (key == null)
                        {
                            // Nop, it is not => Read the install language
                            _languageId = InstallLanguageID;
                        }
                        else
                        {
                            _languageId = key.GetValue("LanguageID", InstallLanguageID) as string;
                        }
                    }
                }
                catch 
                {
                    _languageId = InstallLanguageID;
                }

                if (string.IsNullOrEmpty(_languageId))
                {
                    _languageId = InstallLanguageID;
                }

                LanguageID = _languageId;

                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ConfigRegPath))
                    {
                        if (key == null)
                        {
                            _skinType = (int)Theme.Default.Value;
                        }
                        else
                        {
                            int skinType = (int)key.GetValue("SkinType", (int)Theme.Default.Value);
                            if (!Theme.IsAllowedValue(skinType))
                            {
                                skinType = (int)Theme.Default.Value;
                            }


                            if (skinType != _skinType)
                            {
                                _skinType = skinType;
                            }
                        }
                    }
                }
                catch
                {
                    _skinType = (int)Theme.Default.Value; 
                }

                LanguageID = _languageId;
                SkinType = (ThemeEnum)_skinType;
            }

            _tmrReadRegistry = new System.Windows.Forms.Timer();
            _tmrReadRegistry.Interval = 5000;
            _tmrReadRegistry.Tick += new EventHandler(OnReadRegistry);
            _tmrReadRegistry.Start();
        }

        static void OnReadRegistry(object sender, EventArgs e)
        {
            lock (_languageSyncRoot)
            {
                try
                {
                    _tmrReadRegistry.Stop();

                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ConfigRegPath))
                    {
                        if (key != null)
                        {
                            string langId = key.GetValue("LanguageID", InstallLanguageID) as string;
                            if (langId != _languageId)
                            {
                                _languageId = langId;
                                Translator.SetInterfaceLanguage(_languageId);
                            }

                            int skinType = (int)key.GetValue("SkinType", (int)Theme.Default.Value);
                            if (skinType != _skinType)
                            {
                                _skinType = skinType;
                                EventDispatch.DispatchEvent(EventNames.ThemeUpdated);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex);
                }
                finally
                {
                    _tmrReadRegistry.Start();
                }
            }
        }

        #region Generic config API

        public static CultureInfo[] SupportedCultures
        {
            get
            {
                CultureInfo[] retVal = new CultureInfo[_cultures.Count];
               _cultures.Values.CopyTo(retVal, 0);
                return retVal;
            }
        }

        static int _skinType = (int)Theme.Default.Value;
        public static ThemeEnum SkinType
        {
            get
            {
                return (ThemeEnum)_skinType;
            }

            set
            {
                if ((int)value != _skinType)
                {
                    _skinType = (int)value;
                    EventDispatch.DispatchEvent(EventNames.ThemeUpdated);
                }

                try
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRegPath))
                    {
                        if (key != null)
                        {
                            key.SetValue("SkinType", (int)value);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        static string _languageId = InstallLanguageID;
        public static string LanguageID
        {
            get
            {
                if (string.IsNullOrEmpty(_languageId))
                {
                    _languageId = InstallLanguageID;
                }

                return _languageId;
            }

            set
            {
                try
                {
                    if (value != _languageId)
                    {
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRegPath))
                        {
                            if (key != null)
                            {
                                key.SetValue("LanguageID", value);
                            }
                        }

                        _languageId = value;
                        Translator.SetInterfaceLanguage(_languageId);
                    }
                }
                catch
                {
                } 
            }
        }

        public static string InstallLanguageID
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(ConfigRegPath))
                {
                    if (key != null)
                    {
                        return key.GetValue("InstallLanguageID", "en") as string;
                    }
                    else
                    {
                        return "en";
                    }
                }
            }
        }

        public static string HelpUriBase
        {
            get
            {
                if (UseOnlineDocumentation)
                {
                    const string defaultUri = "http://opmedia.3x.ro/docs";
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(ConfigRegPath))
                    {
                        if (key != null)
                        {
                            return key.GetValue("HelpUriBase", defaultUri) as string;
                        }
                        else
                        {
                            return defaultUri;
                        }
                    }
                }

                return string.Format("file://{0}/docs", SuiteConfiguration.InstallationPath);
            }
        }

        public static bool UseOnlineDocumentation
        {
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ConfigRegPath))
                {
                    if (key != null)
                    {
                        int val = (int)key.GetValue("UseOnlineDocumentation", 0);
                        return (val != 0);
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            set
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRegPath))
                    {
                        if (key != null)
                        {
                            key.SetValue("UseOnlineDocumentation", value ? 1 : 0);
                        }
                    }
                }
                catch
                {
                }
            }
        }
        

        public static string DownloadUriBase
        {
            get 
            {
                const string defaultUri = "http://opmedia.3x.ro/downloads/";

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(ConfigRegPath))
                {
                    if (key != null)
                    {
                        return key.GetValue("DownloadUriBase", defaultUri) as string;
                    }
                    else
                    {
                        return defaultUri;
                    }
                }
            }
        }
        
        public static bool AllowAutomaticUpdates
        {
            get 
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ConfigRegPath))
                {
                    if (key != null)
                    {
                        int val = (int)key.GetValue("AllowAutomaticUpdates", 1);
                        return (val != 0);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            
            set 
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRegPath))
                    {
                        if (key != null)
                        {
                            key.SetValue("AllowAutomaticUpdates", value ? 1 : 0);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static int OSVersion
        {
            get
            {

                int winVer = Environment.OSVersion.Version.Major * 10;
                winVer += Environment.OSVersion.Version.Minor;
                return winVer;
            }
        }

        public static bool LogFullyDisabled
        {
            get
            {
                bool retVal = false;
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OPMedia Research");
                    if (key != null)
                    {
                        retVal = ((int)key.GetValue("LogFullyDisabled", 0) != 0);
                    }
                }
                catch
                {
                }

                return retVal;
            }
        }

        internal static void RunProcess(string cmdLine, string args, bool wait)
        {
            RunProcess(cmdLine, args, wait, false);
        }

        internal static bool RunProcess(string cmdLine, string args, bool wait, bool window)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmdLine, args);
                psi.CreateNoWindow = !window;
                psi.ErrorDialog = true;
                psi.WindowStyle = (window) ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

                Process p = Process.Start(psi);
                if (p != null && wait)
                {
                    p.WaitForExit();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Suite installation API
        

        public static string InstallationPath
        {
            get
            {
                string retVal = string.Empty;
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OPMedia Research\" + Constants.PlayerName);
                    if (key != null)
                    {
                        retVal = key.GetValue("InstallPathOverride") as string;
                    }

                    if (string.IsNullOrEmpty(retVal))
                    {
                        Assembly asm = Assembly.GetAssembly(typeof(SuiteConfiguration));
                        if (asm != null)
                        {
                            FileInfo fi = new FileInfo(asm.Location);
                            retVal = fi.DirectoryName;
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

        public static string ShellSupportInstallationPath
        {
            get
            {
                return Path.Combine(InstallationPath, Constants.ShellSupportBinary);
            }
        }

        public static string PlayerInstallationPath
        {
            get
            {
                return Path.Combine(InstallationPath, Constants.PlayerBinary);
            }
        }

        public static string LibraryInstallationPath
        {
            get
            {
                return Path.Combine(InstallationPath, Constants.LibraryBinary);
            }
        }

        public static string MediaHostInstallationPath
        {
            get
            {
                return Path.Combine(InstallationPath, Constants.MediaHostBinary);
            }
        }
        #endregion

        #region RCC Service API
        public static bool IsRCCServiceInstalled
        {
            get
            {
                try
                {
                    ServiceController sc = new ServiceController(Constants.RCCServiceShortName);
                    ServiceControllerStatus scs = sc.Status;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string RCCManagerInstallationPath
        {
            get
            {
                return Path.Combine(InstallationPath, Constants.RCCManagerBinary);
            }
        }

        public static string RCCServiceInstallationPath
        {
            get
            {
                return Path.Combine(InstallationPath, Constants.RCCServiceBinary);
            }
        }
        #endregion

        #region Generic purpoise API

        public static bool CanModifyRegistry
        {
            get
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\OPMedia Research\opmtest"))
                    {
                        if (key != null)
                        {
                            key.SetValue("aa", "bb");
                        }
                    }

                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\OPMedia Research\opmtest");
                    return true;
                }
                catch
                {
                }

                return false;
            }
        }
        #endregion

        public static string DefaultSubtitleURIs 
        {
            get
            {
                return @"BSP_V1;http://api.bsplayer-subtitles.com/v1.php;1\Osdb;http://api.opensubtitles.org/xml-rpc;1\NuSoap;http://api.getsubtitle.com/server.php;0";
            }
        }
    }
}
