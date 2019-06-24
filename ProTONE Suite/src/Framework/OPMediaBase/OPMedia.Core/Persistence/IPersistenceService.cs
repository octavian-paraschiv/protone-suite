using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace OPMedia.Core
{
    [DataContract]
    public enum ChangeType
    {
        [EnumMember]
        /// <summary>
        /// No change
        /// </summary>
        None = 0,

        [EnumMember]
        /// <summary>
        /// Saved (i.e. added or modified - both handled the same)
        /// </summary>
        Saved,

        [EnumMember]
        /// <summary>
        /// Deleted
        /// </summary>
        Deleted,
    }

    [ServiceContract(CallbackContract = typeof(IPersistenceNotification))]
    public interface IPersistenceService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(string appId);

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(string appId);

        [OperationContract(IsOneWay = false)]
        string ReadObject(string persistenceId, string persistenceContext);

        [OperationContract(IsOneWay = true)]
        void SaveObject(string persistenceId, string persistenceContext, string objectContent);

        [OperationContract(IsOneWay = true)]
        void DeleteObject(string persistenceId, string persistenceContext);

        [OperationContract(IsOneWay = true)]
        void Ping(string appId);

        [OperationContract(IsOneWay = true)]
        void SaveBlob(string persistenceId, string persistenceContext, byte[] objectBlob);

        [OperationContract(IsOneWay = false)]
        byte[] ReadBlob(string persistenceId, string persistenceContext);
    }

    public interface IPersistenceNotification
    {
        [OperationContract(IsOneWay = true)]
        void Notify(ChangeType changeType, string persistenceId, string persistenceContext, string objectContent);

        [OperationContract(IsOneWay = true)]
        void NotifyBlob(ChangeType changeType, string persistenceId, string persistenceContext, byte[] objectContent);
    }

    public static class PersistenceConstants
    {
        public const int TcpPort = 10200;

        static string _userName = null;
        static string _password = null;
        static string _persistenceLocation = null;
        static bool _useRemoteServer = false;

        static PersistenceConstants()
        {
            string persistenceServer = null;

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OPMedia Research"))
                {
                    persistenceServer = key.GetValue("PersistenceServer", "localhost") as string;
                    if (string.IsNullOrEmpty(persistenceServer) == false)
                    {
                        _userName = key.GetValue("userName", "") as string;
                        _password = key.GetValue("password", "") as string;
                    }
                }
            }
            catch
            {
                persistenceServer = null;
            }

            if (string.IsNullOrEmpty(persistenceServer))
                persistenceServer = "localhost";
            else
                _useRemoteServer = true;

            _persistenceLocation = $"net.tcp://{persistenceServer}:{PersistenceConstants.TcpPort}/PersistenceService.svc";
        }

        public static string PersistenceServiceAddress
        {
            get
            {
                return _persistenceLocation;
            }
        }

        public static bool UseRemoteServer
        {
            get
            {
                return _useRemoteServer;
            }
        }

        public static string UserName
        {
            get
            {
                return _userName;
            }
        }

        public static string Password
        {
            get
            {
                return _password;
            }
        }

    }
}
