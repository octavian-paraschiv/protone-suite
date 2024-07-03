using Newtonsoft.Json;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace OPMedia.Core.TranslationSupport
{
    internal class Translation
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    internal class TranslationData
    {
        [JsonProperty("data")]
        public List<Translation> Translations { get; set; }
    }

    public delegate void TranslationUpdatedHandler(string lang);

    internal class TranslationFile
    {
        object _lock = new object();
        Dictionary<string, string> _translations = new Dictionary<string, string>();
        FileSystemWatcher _fsw;
        string _filePath;
        string _lang;

        public event TranslationUpdatedHandler TranslationUpdated;

        public Dictionary<string, string> Translations
        {
            get
            {
                lock (_lock)
                {
                    return _translations;
                }
            }
        }

        public string this[string tag]
        {
            get
            {
                lock (_lock)
                {
                    if (_translations.ContainsKey(tag))
                        return _translations[tag];

                    return null;
                }
            }
        }

        public TranslationFile(string lang)
        {
            _lang = lang;

            FileInfo fi = new FileInfo($"{AppConfig.InstallationPath}/Translations/{ApplicationInfo.ApplicationName}-{lang}.json");
            _filePath = fi.FullName;

            if (ReadFile())
            {
                _fsw = new FileSystemWatcher(fi.DirectoryName, "*.json");
                _fsw.Changed += _fsw_Changed;
                _fsw.EnableRaisingEvents = true;
            }
        }

        private void _fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (string.Equals(e.FullPath, _filePath, StringComparison.OrdinalIgnoreCase))
                ReadFile();
        }

        private bool ReadFile()
        {
            Dictionary<string, string> translations = null;
            try
            {
                var content = File.ReadAllText(_filePath);
                translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                if (translations?.Count > 0)
                {
                    lock (_lock)
                    {
                        foreach (var x in translations)
                        {
                            if (!_translations.ContainsKey(x.Key))
                                _translations.Add(x.Key, x.Value);
                            else
                                _translations[x.Key] = x.Value;
                        }
                    }

                    TranslationUpdated?.Invoke(_lang);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
    }
}
