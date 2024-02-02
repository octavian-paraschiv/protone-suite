using Newtonsoft.Json;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OPMedia.PersistenceService
{
    public class PersistenceServiceImpl : IPersistenceService, IDisposable
    {
        private PersistenceServer _server;

        private object _subscriptionsLock = new object();
        private Dictionary<string, string> _subscriptions = new Dictionary<string, string>();
        private SqliteDbStore _db = new SqliteDbStore();

        public PersistenceServiceImpl()
        {
            Environment.CurrentDirectory = AppConfig.InstallationPath;

            _server = new PersistenceServer();
            _server.TextLineReceived = OnLineReceived;
            _server.ConnectionClosed = OnConnectionClosed;
        }

        private void OnConnectionClosed(string connId, bool isGracefully)
        {
            lock (_subscriptionsLock)
            {
                if (_subscriptions.ContainsKey(connId))
                    _subscriptions.Remove(connId);
            }
        }

        private void OnLineReceived(string connId, string line)
        {
            var pdu = PduFactory.Decode(line);

            // --------------------------------------------------------
            if (pdu is ServicePDU spdu)
            {
                switch (spdu.SvcActionType)
                {
                    case ServiceActionType.Subscribe:
                        {
                            Logger.LogTrace($"Subscribing for {connId}/{spdu.Content} ...");
                            lock (_subscriptionsLock)
                            {
                                if (_subscriptions.ContainsKey(connId))
                                    _subscriptions[connId] = spdu.Content;
                                else
                                    _subscriptions.Add(connId, spdu.Content);
                            }
                        }
                        break;

                    case ServiceActionType.Unsubscribe:
                        {
                            Logger.LogTrace($"Unsubscribing for {connId} ...");
                            lock (_subscriptionsLock)
                            {
                                if (_subscriptions.ContainsKey(connId))
                                    _subscriptions.Remove(connId);
                            }
                        }
                        break;

                }
            }

            // --------------------------------------------------------
            else if (pdu is PersistencePDU rpdu)
            {
                switch (rpdu.ActionType)
                {
                    case PersistenceActionType.ReadAll:
                        {
                            var dict = ReadAll(rpdu.AppName, rpdu.Context);
                            var str = JsonConvert.SerializeObject(dict);
                            var strBytes = Encoding.UTF8.GetBytes(str);
                            rpdu.Content = Convert.ToBase64String(strBytes);
                            string data = JsonConvert.SerializeObject(rpdu);
                            _server.SendTo(connId, data);
                        }
                        break;

                    case PersistenceActionType.ReadNode:
                        {
                            rpdu.Content = ReadNode(rpdu.NodeId, rpdu.Context) ?? "";
                            string data = JsonConvert.SerializeObject(rpdu);
                            _server.SendTo(connId, data);
                        }
                        break;

                    case PersistenceActionType.SaveNode:
                        SaveNode(rpdu.NodeId, rpdu.Context, rpdu.Content);
                        break;

                    case PersistenceActionType.DeleteNode:
                        DeleteNode(rpdu.NodeId, rpdu.Context);
                        break;
                }
            }

            // --------------------------------------------------------
            else if (pdu is NotificationPDU npdu)
            {
                Notify(npdu);
            }
        }
        public void Notify(NotificationPDU npdu)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_subscriptionsLock)
                {
                    List<string> appsToRemove = new List<string>();

                    foreach (KeyValuePair<string, string> appRecord in _subscriptions)
                    {
                        try
                        {
                            string data = JsonConvert.SerializeObject(npdu);
                            _server.SendTo(appRecord.Key, data);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogTrace($"Notify: Marking record for connId {appRecord.Key} for deletion, it looks faulted ...");
                            appsToRemove.Add(appRecord.Key);
                        }
                    }

                    appsToRemove.ForEach(connId =>
                    {
                        if (_subscriptions.ContainsKey(connId))
                            _subscriptions.Remove(connId);
                    });
                }

            }, null);
        }

        public Dictionary<string, string> ReadAll(string appName, string context)
        {
            try
            {
                return _db.ReadAll(appName, context);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public string ReadNode(string nodeId, string context)
        {
            try
            {
                return _db.ReadNode(nodeId, context);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }


        public void SaveNode(string nodeId, string context, string content)
        {
            try
            {
                _db.SaveNode(nodeId, context, content);
                Notify(new NotificationPDU
                {
                    ChangeType = NotificationType.NodeSaved,
                    NodeId = nodeId,
                    Context = context,
                    Content = content
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void DeleteNode(string nodeId, string context)
        {
            try
            {
                _db.DeleteNode(nodeId, context);

                Notify(new NotificationPDU
                {
                    ChangeType = NotificationType.NodeDeleted,
                    NodeId = nodeId,
                    Context = context,
                    Content = ""
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}
