using System.Collections.Generic;

namespace OPMedia.Core
{
    public enum NotificationType
    {
        None = 0,
        NodeSaved,
        NodeDeleted,
        IpcEvent,
    }

    public interface IPersistenceService
    {
        Dictionary<string, string> ReadAll(string appName, string context);
        string ReadNode(string nodeId, string context);
        void SaveNode(string nodeId, string context, string content);
        void DeleteNode(string nodeId, string context);
    }

    public interface INotificationService
    {
        void Notify(NotificationType changeType, string nodeId, string context, string content);
    }
}
