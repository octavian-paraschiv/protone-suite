using OPMedia.Core.Utilities;
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

        public static SettingsProxy Instance { get; } = new SettingsProxy();

        private SettingsProxy()
        {
        }

        #region ReadNode

        public T ReadNode<T>(bool appScoped, string nodeId, T defaultValue)
        {
            // TODO implement
            return default(T);
        }

        public T ReadNode<T>(string nodeId, T defaultValue)
        {
            // TODO implement
            return default(T);

        }

        #endregion

        #region SaveNode

        public void SaveNode<T>(bool appScoped, string nodeId, T content)
        {
            // TODO implement
        }

        public void SaveNode<T>(string nodeId, T content)
        {
            // TODO implement
        }

        #endregion

        #region DeleteNode

        public void DeleteNode(bool appScoped, string nodeId)
        {
            // TODO implement
        }

        public void DeleteNode(string nodeId)
        {
            // TODO implement
        }

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
        }
    }
}
