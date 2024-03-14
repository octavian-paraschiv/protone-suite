using Microsoft.Win32;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.ShellSupport;
using SharpShell;
using SharpShell.ServerRegistration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

namespace OPMedia.Runtime.ProTONE
{
    public enum KnownFileType
    {
        AudioFile,
        VideoFile,
        Playlist,
        Bookmark,
        Catalog,
        Subtitle,
    }

    public static class RegistrationSupport
    {
        static bool _initialized = false;
        static readonly Dictionary<string, KnownFileType> _knownFileTypes = new Dictionary<string, KnownFileType>();
        static readonly Dictionary<KnownFileType, string> _launchPaths = new Dictionary<KnownFileType, string>();
        static readonly Dictionary<KnownFileType, string> _descriptions = new Dictionary<KnownFileType, string>();

        public class KnownFileTypeInfo
        {
            public string FileType { get; private set; }
            public KnownFileType KnownFileType { get; private set; }
            public string LaunchPath { get; private set; }
            public string Description { get; private set; }
            public string MediaType { get; private set; }

            public bool IsValid { get; private set; }

            public KnownFileTypeInfo(string fileType, bool forceSetting, bool restoreLegacyMediaType)
            {
                try
                {
                    this.FileType = fileType.ToLowerInvariant();

                    if (_knownFileTypes.ContainsKey(this.FileType))
                    {
                        this.KnownFileType = _knownFileTypes[this.FileType];
                        this.LaunchPath = _launchPaths[this.KnownFileType];
                        this.Description = _descriptions[this.KnownFileType];

                        if (string.IsNullOrEmpty(this.Description))
                        {
                            this.Description = string.Format("OPMedia {0} (.{1})", this.KnownFileType, fileType);
                        }

                        this.MediaType = string.Format("OPMedia.{0}", this.KnownFileType);

                        IsValid = GetFileClass(forceSetting, restoreLegacyMediaType);
                        return;
                    }
                }
                catch { }

                IsValid = false;
            }

            private bool GetFileClass(bool forceSetting, bool restoreLegacyMediaType)
            {
                string newMediaType = string.Empty;
                string keyPath = string.Format(".{0}", this.FileType);

                using (RegistryKey key = (forceSetting) ?
                    Registry.ClassesRoot.CreateSubKey(keyPath) :
                    Registry.ClassesRoot.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        if (forceSetting)
                        {
                            // First erase all existing subkeys
                            string[] subKeyNames = key.GetSubKeyNames();
                            foreach (string subKeyName in subKeyNames)
                            {
                                try
                                {
                                    key.DeleteSubKeyTree(subKeyName);
                                }
                                catch
                                {
                                }
                            }

                            keyPath = string.Format(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.{0}", this.FileType);
                            using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(keyPath))
                            {
                                if (key2 != null)
                                {
                                    subKeyNames = key2.GetSubKeyNames();
                                    foreach (string subKeyName in subKeyNames)
                                    {
                                        try
                                        {
                                            key2.DeleteSubKeyTree(subKeyName);
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                key2.DeleteSubKey(subKeyName);
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }
                        }

                        if (restoreLegacyMediaType)
                        {
                            string legacyMediaType = key.GetValue("LegacyMediaType") as string;
                            if (!string.IsNullOrEmpty(legacyMediaType))
                            {
                                key.SetValue("", legacyMediaType);
                                key.DeleteValue("LegacyMediaType");
                            }
                            else
                            {
                                key.SetValue("", "");
                            }
                        }
                        else
                        {
                            if (forceSetting)
                            {
                                string oldMediaType = key.GetValue("") as string;
                                if (oldMediaType != null && oldMediaType != this.MediaType)
                                {
                                    key.SetValue("LegacyMediaType", oldMediaType);
                                }

                                key.SetValue("", this.MediaType);
                            }
                            else
                            {
                                this.MediaType = key.GetValue("") as string;
                            }
                        }

                        key.Close();
                        return true;
                    }

                    return false;
                }
            }
        }

        static RegistrationSupport()
        {
            _launchPaths.Add(KnownFileType.AudioFile, ProTONEConfig.PlayerInstallationPath);
            _launchPaths.Add(KnownFileType.VideoFile, ProTONEConfig.PlayerInstallationPath);
            _launchPaths.Add(KnownFileType.Playlist, ProTONEConfig.PlayerInstallationPath);
            _launchPaths.Add(KnownFileType.Bookmark, ProTONEConfig.PlayerInstallationPath);
            _launchPaths.Add(KnownFileType.Catalog, ProTONEConfig.LibraryInstallationPath);
            //_launchPaths.Add(KnownFileType.Subtitle, AppConfig.LibraryInstallationPath);
            _launchPaths.Add(KnownFileType.Subtitle, "");

            _descriptions.Add(KnownFileType.AudioFile, "");
            _descriptions.Add(KnownFileType.VideoFile, "");
            _descriptions.Add(KnownFileType.Playlist, "");
            _descriptions.Add(KnownFileType.Bookmark, Core.Constants.BookmarkFileTypeDesc);
            _descriptions.Add(KnownFileType.Catalog, Core.Constants.CatalogFileTypeDesc);
            _descriptions.Add(KnownFileType.Subtitle, Core.Constants.SubtitleFileTypeDesc);
        }

        public static void Init()
        {
            if (_initialized)
                return;

            _knownFileTypes.Clear();

            foreach (string s in SupportedFileProvider.Instance.SupportedAudioTypes)
            {
                try
                {
                    _knownFileTypes.Add(s.ToLowerInvariant(), KnownFileType.AudioFile);
                }
                catch { }
            }

            foreach (string s in SupportedFileProvider.Instance.SupportedVideoTypes)
            {
                try
                {
                    _knownFileTypes.Add(s.ToLowerInvariant(), KnownFileType.VideoFile);
                }
                catch { }
            }

            foreach (string s in SupportedFileProvider.Instance.SupportedPlaylists)
            {
                try
                {
                    _knownFileTypes.Add(s.ToLowerInvariant(), KnownFileType.Playlist);
                }
                catch { }
            }

            foreach (string s in SupportedFileProvider.Instance.SupportedSubtitles)
            {
                try
                {
                    _knownFileTypes.Add(s.ToLowerInvariant(), KnownFileType.Subtitle);
                }
                catch { }
            }

            _knownFileTypes.Add("bmk", KnownFileType.Bookmark);
            _knownFileTypes.Add("ctx", KnownFileType.Catalog);
        }

        #region File registration API

        public static void RegisterKnownFileTypes()
        {
            foreach (string s in _knownFileTypes.Keys)
            {
                RegisterFileType(s, true);
            }

            RegisterFileType("bmk", true);
            RegisterFileType("ctx", true);
        }

        public static void UnregisterKnownFileTypes()
        {
            foreach (string s in _knownFileTypes.Keys)
            {
                UnregisterFileType(s, true);
            }

            UnregisterFileType("bmk", true);
            UnregisterFileType("ctx", true);
        }

        public static void RegisterFileType(string fileType, bool regMediaType)
        {
            KnownFileTypeInfo info = new KnownFileTypeInfo(fileType, true, false);

            if (info.IsValid)
            {
                //if (regMediaType)
                {
                    // ==== Register media type ====
                    using (RegistryKey mediaTypeKey = Registry.ClassesRoot.CreateSubKey(info.MediaType))
                    {
                        if (mediaTypeKey != null)
                        {
                            mediaTypeKey.SetValue("", info.MediaType);
                        }

                        // ==== Register icon ====
                        using (RegistryKey defaultIconKey = mediaTypeKey.CreateSubKey("DefaultIcon"))
                        {
                            if (defaultIconKey != null)
                            {
                                string newValue = string.Format(@"{0}\Resources\{1}.ico", AppConfig.InstallationPath, info.KnownFileType);
                                defaultIconKey.SetValue("", newValue);
                            }
                        }

                        if (File.Exists(info.LaunchPath))
                        {
                            using (RegistryKey shellKey = mediaTypeKey.CreateSubKey("shell"))
                            {
                                if (shellKey != null)
                                {
                                    // ==== Change default action to OPEN ====
                                    shellKey.SetValue("", "open");

                                    // ==== Update OPEN action command ====
                                    using (RegistryKey key = shellKey.CreateSubKey("open\\command"))
                                    {
                                        if (key != null)
                                        {
                                            key.SetValue("", string.Format("\"{0}\" launch \"%L\"", info.LaunchPath));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            mediaTypeKey.DeleteSubKeyTree("shell", false);
                        }
                    }
                }
            }
        }

        public static void UnregisterFileType(string fileType, bool unregMediaType)
        {
            KnownFileTypeInfo info = new KnownFileTypeInfo(fileType, true, true);

            if (info.IsValid)
            {
                //if (unregMediaType)
                {
                    // ==== Unregister media type ====
                    using (RegistryKey mediaTypeKey = Registry.ClassesRoot.CreateSubKey(info.MediaType))
                    {
                        if (mediaTypeKey != null)
                        {
                            mediaTypeKey.SetValue("", "");

                            // ==== Unregister icon ====
                            using (RegistryKey defaultIconKey = mediaTypeKey.CreateSubKey("DefaultIcon"))
                            {
                                if (defaultIconKey != null)
                                {
                                    defaultIconKey.SetValue("", "");
                                }
                            }

                            mediaTypeKey.DeleteSubKeyTree("shell", false);
                        }
                    }
                }
            }
        }

        public static bool IsFileTypeRegistered(string fileType)
        {
            try
            {
                KnownFileTypeInfo info = new KnownFileTypeInfo(fileType, false, false);

                string keyPath = string.Format("{0}\\shell\\open\\command", info.MediaType);
                string expectedOpenCommand = string.Format("\"{0}\" launch \"%L\"", info.LaunchPath);

                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        string openCommand = key.GetValue("") as string;
                        return (openCommand == expectedOpenCommand);
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        #endregion

        public static void ReloadFileAssociations()
        {
            Shell32.SHChangeNotify(HChangeNotifyEventID.SHCNE_ASSOCCHANGED,
                HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }

        static Lazy<ISharpShellServer> _shellExt = GetShellExtension();

        public static bool IsShellExtensionRegistered()
        {
            bool isReg32 = false, isReg64 = false;

            var reg32 = ServerRegistrationManager.GetServerRegistrationInfo(_shellExt?.Value, RegistrationType.OS32Bit);
            isReg32 = (reg32?.IsApproved).GetValueOrDefault();

            if (Environment.Is64BitOperatingSystem)
            {
                var reg64 = ServerRegistrationManager.GetServerRegistrationInfo(_shellExt?.Value, RegistrationType.OS64Bit);
                isReg64 = (reg64?.IsApproved).GetValueOrDefault();
            }

            return isReg32 || isReg64;
        }

        public static bool RegisterShellExtension()
        {
            if (!IsShellExtensionRegistered())
            {
                try
                {
                    ServerRegistrationManager.InstallServer(_shellExt.Value, RegistrationType.OS32Bit, true);
                    ServerRegistrationManager.RegisterServer(_shellExt.Value, RegistrationType.OS32Bit);
                }
                catch { }

                if (Environment.Is64BitOperatingSystem)
                {
                    try
                    {
                        ServerRegistrationManager.InstallServer(_shellExt.Value, RegistrationType.OS64Bit, true);
                        ServerRegistrationManager.RegisterServer(_shellExt.Value, RegistrationType.OS64Bit);
                    }
                    catch { }
                }
            }

            return IsShellExtensionRegistered();
        }

        public static bool UnregisterShellExtension()
        {
            if (IsShellExtensionRegistered())
            {
                try
                {
                    ServerRegistrationManager.UnregisterServer(_shellExt.Value, RegistrationType.OS32Bit);
                    ServerRegistrationManager.UninstallServer(_shellExt.Value, RegistrationType.OS32Bit);
                }
                catch { }

                if (Environment.Is64BitOperatingSystem)
                {
                    try
                    {
                        ServerRegistrationManager.UnregisterServer(_shellExt.Value, RegistrationType.OS64Bit);
                        ServerRegistrationManager.UninstallServer(_shellExt.Value, RegistrationType.OS64Bit);
                    }
                    catch { }
                }
            }

            return !IsShellExtensionRegistered();
        }

        private static Lazy<ISharpShellServer> GetShellExtension()
        {
            try
            {
                var cat = new AssemblyCatalog($"{AppConfig.InstallationPath}/OPMedia.ShellSupport.dll");
                var servers = new CompositionContainer(cat).GetExports<ISharpShellServer>();
                return (from srv in servers
                        where (srv?.Value?.DisplayName?.Equals("OPMedia Shell Extension", StringComparison.OrdinalIgnoreCase)).GetValueOrDefault()
                        select srv).FirstOrDefault();
            }
            catch
            { return null; }
        }

        public static string GetInstalledLocationForCLSID(string clsid)
        {
            try
            {
                string keyPath = string.Format(@"SOFTWARE\Classes\CLSID\{0}\InprocServer32", clsid);
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        return key.GetValue("") as string;
                    }
                }
            }
            catch
            {
            }

            return null;
        }
    }
}