using Newtonsoft.Json;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.Persistence;
using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace OPMedia.Core
{
    public class PersistenceProxy : IDisposable, IPersistenceService, INotificationService
    {
        protected readonly static PersistenceProxy _proxy = new PersistenceProxy();
        private static CacheStore _cache = new CacheStore(_proxy);

        protected string _persistenceContext = string.Empty;
        protected Guid _appId = Guid.NewGuid();

        private static int _unsuccesfulAttempts = 0;

        private PersistenceClient _cl = null;
        private ManualResetEventSlim _evtConnected = new ManualResetEventSlim(false);

        protected PersistenceProxy()
        {
            try
            {
                if (ActivatePersistenceService())
                {
                    try
                    {
                        _persistenceContext = WindowsIdentity.GetCurrent().Name;
                    }
                    catch
                    {
                        _persistenceContext = string.Format("{0}\\{1}",
                            Environment.UserDomainName, Environment.UserName);
                    }

                    _cl = new PersistenceClient();
                    _cl.ConnectionOpen += OnConnectionOpen;
                    _cl.Connect(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (!_evtConnected.Wait(5000))
                    ErrorDispatcher.DisplayErrorAndTerminate("Unable to connect to Persistence Service in a timely manner.");
            });
        }

        private bool ActivatePersistenceService()
        {
            int attempts = 5;

            while (attempts > 0)
            {

                try
                {
                    var count = Process.GetProcesses()?
                        .Where(p => (p.ProcessName ?? "").ToUpperInvariant().Contains(Constants.PersistenceServiceShortName.ToUpperInvariant()))
                        .Count();

                    if (count > 0)
                        return true;

                    Process.Start(new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        FileName = Path.Combine(AppConfig.InstallationPath, Constants.PersistenceServiceBinary),
                    });

                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

                attempts--;
            }

            return false;
        }

        private void OnConnectionOpen(string connId, bool reconnect)
        {
            _evtConnected.Set();
            _cl.SendPdu(new ServicePDU { SvcActionType = ServiceActionType.Subscribe, Content = _appId.ToString() });
            _cache.Init(_persistenceContext);
        }

        public void Dispose()
        {
            try
            {
                _cl?.SendPdu(new ServicePDU { SvcActionType = ServiceActionType.Unsubscribe, Content = _appId.ToString() });
                _cl?.SendGracefulEnd();
                _cl?.Disconnect();
                _cl?.Dispose();
            }
            catch { }

            _cl = null;
        }

        static string BuildPersistenceId(bool includeAppName, string nodeId)
        {
            if (includeAppName)
                return string.Format("{0}_{1}", ApplicationInfo.ApplicationName, nodeId);

            return nodeId;
        }


        #region ReadNode

        public static T ReadNode<T>(bool includeAppName, string nodeId, T defaultValue, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, nodeId);
            return ReadNode<T>(id, defaultValue, usePersistenceContext);
        }

        public static T ReadNode<T>(string nodeId, T defaultValue, bool usePersistenceContext = true)
        {
            T retVal = defaultValue;

            try
            {
                string content = usePersistenceContext ?
                    _cache.ReadNode(nodeId, _proxy._persistenceContext) :
                    _cache.ReadNode(nodeId, string.Empty);

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

        string IPersistenceService.ReadNode(string nodeId, string context)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.ReadNode nodeId={nodeId} context={context}");


                var pdu = _cl.SendPduAndWaitResponse(new PersistencePDU
                {
                    ActionType = PersistenceActionType.ReadNode,
                    Context = context,
                    NodeId = nodeId,
                });

                if (pdu != null)
                    return pdu.Content;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        Dictionary<string, string> IPersistenceService.ReadAll(string appName, string context)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.ReadAll appName={appName} context={context}");


                var pdu = _cl.SendPduAndWaitResponse(new PersistencePDU
                {
                    ActionType = PersistenceActionType.ReadAll,
                    Context = context,
                    AppName = appName,
                    NodeId = null,

                });

                if (pdu?.Content?.Length > 0)
                {
                    byte[] data = Convert.FromBase64String(pdu.Content);
                    string dataStr = Encoding.UTF8.GetString(data);
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataStr);
                    return dictionary;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        #endregion

        #region SaveNode

        public static void SaveNode<T>(bool includeAppName, string nodeId, T content, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, nodeId);
            SaveNode<T>(id, content, usePersistenceContext);
        }

        public static void SaveNode<T>(string nodeId, T content, bool usePersistenceContext = true)
        {
            try
            {
                if (usePersistenceContext)
                    _cache.SaveNode(nodeId, _proxy._persistenceContext, content.ToString());
                else
                    _cache.SaveNode(nodeId, string.Empty, content.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void IPersistenceService.SaveNode(string nodeId, string context, string content)
        {
            try
            {
                _cl.SendPdu(new PersistencePDU
                {
                    ActionType = PersistenceActionType.SaveNode,
                    Content = content,
                    Context = context,
                    NodeId = nodeId,
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region DeleteNode

        public static void DeleteNode(bool includeAppName, string nodeId, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, nodeId);
            DeleteNode(id, usePersistenceContext);
        }

        public static void DeleteNode(string nodeId, bool usePersistenceContext = true)
        {
            try
            {
                if (usePersistenceContext)
                    _cache.DeleteNode(nodeId, _proxy._persistenceContext);
                else
                    _cache.DeleteNode(nodeId, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void IPersistenceService.DeleteNode(string nodeId, string context)
        {
            try
            {
                _cl.SendPdu(new PersistencePDU
                {
                    ActionType = PersistenceActionType.DeleteNode,
                    Context = context,
                    NodeId = nodeId,
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region Notifications
        public static void SendIpcEvent(string eventName, string eventArgs)
        {
            string content = StringUtils.FromStringArray(new string[] { eventName, eventArgs }, '>');
            (_proxy as INotificationService).Notify(NotificationType.IpcEvent, NotificationType.IpcEvent.ToString(), _proxy._persistenceContext, content);
        }

        void INotificationService.Notify(NotificationType changeType, string nodeId, string context, string content)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.Notify nodeId={nodeId} context={context}");

                _cl.SendPdu(new NotificationPDU
                {
                    ChangeType = changeType,
                    Context = context,
                    NodeId = nodeId,
                    Content = content
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void DispatchNotification(NotificationType changeType, string nodeId, string context, string content)
        {
            if (changeType == NotificationType.IpcEvent)
            {
                if (content?.Length > 0)
                {
                    string evtName = null, evtData = null;
                    string[] fields = StringUtils.ToStringArray(content, '>');

                    if (fields?.Length > 0)
                        evtName = fields[0];
                    if (fields?.Length > 1)
                        evtData = fields[1];

                    EventDispatch.DispatchEvent(evtName, evtData);
                }
            }
            else if (_cache.RefreshObject(changeType, nodeId, context, content))
            {
                Logger.LogToConsole($"Notification from PersistenceService ({changeType}, Id={nodeId}, Context={context}, Data={content} => Cache updated. Bubbling up event to its potential consumers.");
                if (changeType == NotificationType.NodeSaved)
                    AppConfig.OnSettingsChanged(nodeId, context, content);
            }
            else
            {
                Logger.LogToConsole($"Notification from PersistenceService ({changeType}, Id={nodeId}, Context={context}, Data={content} => Ignored, as we failed to update the cache.");
            }
        }
        #endregion
    }
}
