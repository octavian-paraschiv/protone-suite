using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using OPMedia.Core;
using OPMedia.Core.Logging;
using System.Threading;
using OPMedia.Core.Utilities;
using OPMedia.Core.InterProcessCommunication;
using Newtonsoft.Json;
using OPMedia.Core.Persistence;
using OPMedia.Core.Configuration;

namespace OPMedia.PersistenceService
{
    public class PersistenceServiceImpl : IPersistenceService, IDisposable
    {
        static TicToc _readTicToc = new TicToc("Persistence.Service.ReadObject");
        static TicToc _saveTicToc = new TicToc("Persistence.Service.SaveObject");
        static TicToc _deleteTicToc = new TicToc("Persistence.Service.DeleteObject");

        private PersistenceServer _server;

        private object _subscriptionsLock = new object();
        private Dictionary<string, string> _subscriptions = new Dictionary<string, string>();

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
                            Logger.LogTrace($"Subscribing for {connId}/{spdu.ObjectContent} ...");
                            lock (_subscriptionsLock)
                            {
                                if (_subscriptions.ContainsKey(connId))
                                    _subscriptions[connId] = spdu.ObjectContent;
                                else
                                    _subscriptions.Add(connId, spdu.ObjectContent);
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
                    case PersistenceActionType.ReadObject:
                        {
                            if (rpdu.IsBlob)
                            {
                                var blob = ReadBlob(rpdu.PersistenceId, rpdu.PersistenceContext);
                                if (blob?.Length > 0)
                                    rpdu.ObjectContent = Convert.ToBase64String(blob);
                                else
                                    rpdu.ObjectContent = "";
                            }
                            else
                            {
                                rpdu.ObjectContent = ReadObject(rpdu.PersistenceId, rpdu.PersistenceContext) ?? "";
                            }

                            string data = JsonConvert.SerializeObject(rpdu);
                            _server.SendTo(connId, data);
                        }
                        break;

                    case PersistenceActionType.SaveObject:
                        {
                            if (rpdu.IsBlob)
                            {
                                var data = Convert.FromBase64String(rpdu.ObjectContent);
                                SaveBlob(rpdu.PersistenceId, rpdu.PersistenceContext, data);
                            }
                            else
                            {
                                SaveObject(rpdu.PersistenceId, rpdu.PersistenceContext, rpdu.ObjectContent);
                            }
                        }
                        break;

                    case PersistenceActionType.DeleteObject:
                        DeleteObject(rpdu.PersistenceId, rpdu.PersistenceContext);
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

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                _readTicToc.Tic();
                return SingletonCacheStore.Instance.ReadObject(persistenceId, persistenceContext);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                _readTicToc.Toc();
            }

            return null;
        }

        public byte[] ReadBlob(string persistenceId, string persistenceContext)
        {
            try
            {
                _readTicToc.Tic();
                return SingletonCacheStore.Instance.ReadBlob(persistenceId, persistenceContext);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                _readTicToc.Toc();
            }

            return null;
        }


        public void SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                _saveTicToc.Tic();
                
                bool ok = SingletonCacheStore.Instance.SaveObject(persistenceId, persistenceContext, objectContent);
                if (ok)
                {
                    Notify(new NotificationPDU
                    {
                        ChangeType = NotificationType.ObjectSaved,
                        PersistenceId = persistenceId,
                        PersistenceContext = persistenceContext,
                        ObjectContent = objectContent
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                _saveTicToc.Toc();
            }
        }

        public void SaveBlob(string persistenceId, string persistenceContext, byte[] objectContent)
        {
            try
            {
                _saveTicToc.Tic();

                bool ok = SingletonCacheStore.Instance.SaveBlob(persistenceId, persistenceContext, objectContent);
                if (ok)
                {
                    Notify(new NotificationPDU
                    {
                        ChangeType = NotificationType.ObjectSaved,
                        PersistenceId = persistenceId,
                        PersistenceContext = persistenceContext,
                        ObjectContent = Convert.ToBase64String(objectContent),
                        IsBlob = true
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                _saveTicToc.Toc();
            }
        }

        public void DeleteObject(string persistenceId, string persistenceContext)
        {
            try
            {
                _deleteTicToc.Tic();

                bool ok = SingletonCacheStore.Instance.DeleteObject(persistenceId, persistenceContext);
                if (ok)
                {
                    Notify(new NotificationPDU
                    {
                        ChangeType = NotificationType.ObjectDeleted,
                        PersistenceId = persistenceId,
                        PersistenceContext = persistenceContext,
                        ObjectContent = ""
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                _deleteTicToc.Toc();
            }
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}
