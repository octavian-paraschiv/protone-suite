using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using OPMedia.Core;
using OPMedia.Core.Logging;
using System.Threading;

namespace OPMedia.PersistenceService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class PersistenceServiceImpl : IPersistenceService
    {
        static TicToc _readTicToc = new TicToc("Persistence.Service.ReadObject");
        static TicToc _saveTicToc = new TicToc("Persistence.Service.SaveObject");
        static TicToc _deleteTicToc = new TicToc("Persistence.Service.DeleteObject");

        static object _notifyLock = new object();

        static Dictionary<string, IPersistenceNotification> _notifiedApps = new Dictionary<string,IPersistenceNotification>();


        public void Subscribe(string appId)
        {
            lock (_notifyLock)
            {
                try
                {
                    var channel = OperationContext.Current.GetCallbackChannel<IPersistenceNotification>();

                    if (_notifiedApps.ContainsKey(appId) == false)
                    {
                        _notifiedApps.Add(appId, channel);
                        Logger.LogTrace("Subscribe: Adding record for appId {0} ...", appId);
                    }
                    else
                    {
                        _notifiedApps[appId] = channel;
                        Logger.LogTrace("Subscribe: Updating record for appId {0} ...", appId);
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        public void Unsubscribe(string appId)
        {
            DoUnsubscribe(appId);
        }

        static void DoUnsubscribe(string appId)
        {
            lock (_notifyLock)
            {
                try
                {
                    if (_notifiedApps.ContainsKey(appId))
                    {
                        _notifiedApps.Remove(appId);
                        Logger.LogTrace("Unsubscribe: Removing record for appId {0} ...", appId);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }


        public void Notify(ChangeType changeType, string persistenceId, string persistenceContext, object objectContent)
        {
            DoNotify(changeType, persistenceId, persistenceContext, objectContent);
        }

        static void DoNotify(ChangeType changeType, string persistenceId, string persistenceContext, object objectContent)
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                lock (_notifyLock)
                {

                    List<string> appsToRemove = new List<string>();

                    foreach (KeyValuePair<string, IPersistenceNotification> appRecord in _notifiedApps)
                    {
                        try
                        {
                            if (changeType == ChangeType.None && (objectContent as string == "ping"))
                            {
                                Logger.LogTrace("Sending Ping to appId {0}", appRecord.Key);
                            }

                            if (objectContent is byte[])
                                appRecord.Value.NotifyBlob(changeType, persistenceId, persistenceContext, objectContent as byte[]);
                            else
                                appRecord.Value.Notify(changeType, persistenceId, persistenceContext, objectContent as string);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogTrace("Notify: Marking record for appId {0} for deletion, it looks faulted ...",
                                appRecord.Key);

                            appsToRemove.Add(appRecord.Key);
                            Logger.LogException(ex);
                        }
                    }

                    appsToRemove.ForEach((appId) => DoUnsubscribe(appId));
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
                    Notify(ChangeType.Saved, persistenceId, persistenceContext, objectContent);
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
                    Notify(ChangeType.Saved, persistenceId, persistenceContext, objectContent);
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
                    Notify(ChangeType.Deleted, persistenceId, persistenceContext, string.Empty);
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

        public void Ping(string appId)
        {
            Logger.LogTrace($"Ping received from appId={appId}");
        }

        public static void ReversePing()
        {
            DoNotify(ChangeType.None, string.Empty, string.Empty, "ping");
        }
    }
}
