namespace OPMedia.Core
{
    public enum NotificationType
    {
        None = 0,
        ObjectSaved,
        ObjectDeleted,
        IpcEvent,
    }

    public interface IPersistenceService
    {
        string ReadObject(string persistenceId, string persistenceContext);
        void SaveObject(string persistenceId, string persistenceContext, string objectContent);
        void DeleteObject(string persistenceId, string persistenceContext);
    }

    public interface INotificationService
    {
        void SendNotification(NotificationType changeType, string persistenceId, string persistenceContext, string objectContent);
    }
}
