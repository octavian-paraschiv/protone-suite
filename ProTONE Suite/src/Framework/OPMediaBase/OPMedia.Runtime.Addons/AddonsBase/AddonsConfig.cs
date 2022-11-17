using System;
using System.Collections.Generic;
using System.Text;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using System.IO;
using System.Diagnostics;
using OPMedia.UI;
using OPMedia.Core;
using System.Windows.Forms;
using OPMedia.Runtime.Addons.Configuration;
using OPMedia.Runtime;
using OPMedia.UI.Configuration;
using System.Reflection;
using System.Globalization;
using OPMedia.UI.FileOperations.Tasks;
using System.Configuration;
using Newtonsoft.Json;

namespace OPMedia.Runtime.Addons.AddonsBase
{
    public static class AddonsConfig
    {
        static string[] _navAddons = null;
        static string[] _propAddons = null;
        static string[] _previewAddons = null;

        static Dictionary<string, string> _assemblies = new Dictionary<string, string>();

        static Dictionary<string, string> _diskAddonsConfig = null;

        public static string[] NavigationAddons
        {
            get
            {
                return _navAddons;
            }
            
        }

        public static string[] PropertyAddons
        {
            get
            {
                return _propAddons;
            }
        }

        public static string[] PreviewAddons
        {
            get
            {
                return _previewAddons;
            }
        }

        public static bool IsInitialConfig { get; set; }

        public static string GetAssemblyInfo(string addonName)
        {
            if (_assemblies.Count > 0 &&
                _assemblies.ContainsKey(addonName))
            {
                return _assemblies[addonName];
            }

            return string.Empty;
        }

        public static void Init() { }

        static AddonsConfig()
        {
            IsInitialConfig = false;

            try
            {
                try
                {
                    string cfgJsonPath = $"{AppConfig.InstallationPath}\\{ApplicationInfo.ApplicationName}.addons.json";
                    if (File.Exists(cfgJsonPath))
                    {
                        var addonsConfigJson = File.ReadAllText(cfgJsonPath);
                        _diskAddonsConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(addonsConfigJson);
                    }
                }
                catch
                {
                    _diskAddonsConfig = null;
                }

                if (_diskAddonsConfig == null)
                    UninstallMarkedItems();

                _navAddons = ReadAddonConfig("NavigationAddons");
                _previewAddons = ReadAddonConfig("PreviewAddons");
                _propAddons = ReadAddonConfig("PropertyAddons");
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
        }

        private static string[] ReadAddonConfig(string keyBase)
        {
            string[] names = null;
            string namesRaw = null;

            if (_diskAddonsConfig == null || 
                _diskAddonsConfig.ContainsKey(keyBase) == false)
            {
                namesRaw = PersistenceProxy.ReadObject(true, keyBase, string.Empty, false);
            }
            else
            {
                namesRaw = _diskAddonsConfig[keyBase];
            }

            if (!string.IsNullOrEmpty(namesRaw))
            {
                names = namesRaw.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in names)
                {
                    string val = null;

                    if (_diskAddonsConfig == null ||
                        _diskAddonsConfig.ContainsKey(name) == false)
                    {
                        val = PersistenceProxy.ReadObject(true, name, string.Empty, false);
                    }
                    else
                    {
                        val = _diskAddonsConfig[name];
                    }

                    _assemblies.Add(name, val);
                }
            }

            return names;
        }

        internal static void MarkForUninstall(string assembly)
        {
            string markedForUninstall = PersistenceProxy.ReadObject(true, "MarkedForUninstall", string.Empty, false);

            List<string> filesToDelete = new List<string>();

            IEnumerable<string> files = PathUtils.EnumFiles(Application.StartupPath, string.Format("{0}*", assembly));
            foreach (string asmFile in files)
            {
                Assembly asm = Assembly.LoadFrom(asmFile);
                if (asm != null)
                {
                    filesToDelete.Add(asmFile);

                    foreach (CultureInfo ci in AppConfig.SupportedCultures)
                    {
                        try
                        {
                            Assembly satAsm = asm.GetSatelliteAssembly(ci);
                            if (satAsm != null)
                            {
                                string path = satAsm.Location.ToLowerInvariant();
                                if (!filesToDelete.Contains(path))
                                {
                                    filesToDelete.Add(path.ToLowerInvariant());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }

                    foreach (string file in filesToDelete)
                    {
                        markedForUninstall += file;
                        markedForUninstall += "|";
                    }
                }
            }

            PersistenceProxy.SaveObject(true, "MarkedForUninstall", markedForUninstall, false);
        }

        private static void UninstallMarkedItems()
        {
            string[] names = null;
            string namesRaw = PersistenceProxy.ReadObject(true, "MarkedForUninstall", string.Empty, false);

            if (!string.IsNullOrEmpty(namesRaw))
            {
                names = namesRaw.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in names)
                {
                    try
                    {
                        UninstallAddonLibrary(name);
                    }
                    catch(Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }

                PersistenceProxy.SaveObject(true, "MarkedForUninstall", string.Empty, false);
            }
        }

        private static void UninstallAddonLibrary(string name)
        {
            if (File.Exists(name))
            {
                File.SetAttributes(name, FileAttributes.Normal);
                File.Delete(name);
            }
        }

        internal static void InstallAddonLibrary(string fileName)
        {
            try
            {
                List<string> filesToCopy = new List<string>();

                Assembly asm = Assembly.LoadFrom(fileName);
                if (asm != null)
                {
                    filesToCopy.Add(asm.Location.ToLowerInvariant());

                    foreach (CultureInfo ci in AppConfig.SupportedCultures)
                    {
                        try
                        {
                            Assembly satAsm = asm.GetSatelliteAssembly(ci);
                            if (satAsm != null)
                            {
                                string path = satAsm.Location.ToLowerInvariant();
                                if (!filesToCopy.Contains(path))
                                {
                                    filesToCopy.Add(path.ToLowerInvariant());
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }

                    foreach (string file in filesToCopy)
                    {
                        CopyToRunLocation(file.ToLowerInvariant(), 
                            Path.GetDirectoryName(fileName.ToLowerInvariant()));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
        }

        private static void CopyToRunLocation(string filePath, string baseFilePath)
        {
            string diffPath = filePath.Replace(baseFilePath, string.Empty);
            string destFileName = string.Format("{0}{1}{2}", 
                Application.StartupPath, PathUtils.DirectorySeparator, diffPath).Replace(@"\\", @"\");

            string destFolder = Path.GetDirectoryName(destFileName);
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            File.Copy(filePath, destFileName, true);
        }
    }
}
