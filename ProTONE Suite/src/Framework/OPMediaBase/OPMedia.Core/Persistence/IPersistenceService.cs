using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Microsoft.Win32;

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
        byte[] ReadBlob(string persistenceId, string persistenceContext);

        void SaveObject(string persistenceId, string persistenceContext, string objectContent);
        void SaveBlob(string persistenceId, string persistenceContext, byte[] objectBlob);

        void DeleteObject(string persistenceId, string persistenceContext);
    }

    public interface INotificationService
    {
        void SendNotification(NotificationType changeType, string persistenceId, string persistenceContext, string objectContent);
    }
}
