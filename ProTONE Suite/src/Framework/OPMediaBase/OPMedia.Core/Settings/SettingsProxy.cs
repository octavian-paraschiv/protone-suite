using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.Settings;
using OPMedia.Core.Utilities;
using System;
using System.IO;

namespace OPMedia.Core
{
    public enum NotificationType
    {
        None = 0,
        NodeSaved,
        NodeDeleted,
        IpcEvent,
    }

    public class SettingsProxy
    {
        public const string SettingsSubfolder = "Settings";

        static readonly string SettingsFolder = GetSettingsFolder();
        static readonly string AppSettingsStorePath = $"{SettingsFolder}/{ApplicationInfo.ApplicationName}.json";
        static readonly string SuiteSettingsStorePath = $"{SettingsFolder}/{Constants.SuiteName}.json";

        private DictionaryFile _appSettingsFile;
        private DictionaryFile _suiteSettingsFile;

        public static SettingsProxy Instance { get; } = new SettingsProxy();

        private SettingsProxy()
        {
            if (!ApplicationInfo.ApplicationName.EndsWith("worker", StringComparison.OrdinalIgnoreCase))
            {
                // Worker processes don't have an application settings store
                _appSettingsFile = new DictionaryFile(AppSettingsStorePath);
                _appSettingsFile.DictionaryUpdated += () => AppConfig.OnSettingsChanged();
            }

            _suiteSettingsFile = new DictionaryFile(SuiteSettingsStorePath);
            _suiteSettingsFile.DictionaryUpdated += () => AppConfig.OnSettingsChanged();
        }

        #region ReadNode

        public T ReadNode<T>(string nodeId, T defaultValue) => ReadNode<T>(false, nodeId, defaultValue);

        public T ReadNode<T>(bool appScoped, string nodeId, T defaultValue)
        {
            T retVal = defaultValue;

            try
            {
                string content = appScoped ? _appSettingsFile[nodeId] : _suiteSettingsFile[nodeId];

                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        retVal = StringUtils.CastAs<T>(content, defaultValue);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        retVal = defaultValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                retVal = defaultValue;
            }

            return retVal;
        }
        #endregion

        #region SaveNode

        public void SaveNode<T>(string nodeId, T content) => SaveNode<T>(false, nodeId, content);

        public void SaveNode<T>(bool appScoped, string nodeId, T content)
        {
            try
            {
                if (appScoped)
                    _appSettingsFile[nodeId] = content?.ToString();
                else
                    _suiteSettingsFile[nodeId] = content?.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region DeleteNode

        public void DeleteNode(string nodeId) => DeleteNode(false, nodeId);
        public void DeleteNode(bool appScoped, string nodeId) => SaveNode<string>(appScoped, nodeId, null);

        #endregion

        #region Notifications
        public void SendIpcEvent(string eventName, string eventArgs)
        {
            string content = StringUtils.FromStringArray(new string[] { eventName, eventArgs }, '>');

            // TODO implement
        }


        void DispatchNotification(NotificationType changeType, string nodeId, string content)
        {
            // TODO: implement
        }
        #endregion

        static string GetSettingsFolder()
        {
            try
            {
                string settingsFolder = PathUtils.ProgramDataDir;
                settingsFolder = Path.Combine(settingsFolder, Constants.CompanyName);
                settingsFolder = Path.Combine(settingsFolder, Constants.SuiteName);
                settingsFolder = Path.Combine(settingsFolder, SettingsSubfolder);

                if (PathUtils.CanWriteToFolder(settingsFolder))
                    return settingsFolder;

                settingsFolder = Path.Combine(ApplicationInfo.AltLogsFolder, SettingsSubfolder);
                if (PathUtils.CanWriteToFolder(settingsFolder))
                    return settingsFolder;
            }
            catch
            {
            }

            return ".";
        }

        public void SaveSettings()
        {
            _appSettingsFile?.SaveFile();
            _suiteSettingsFile?.SaveFile();
        }
    }
}
