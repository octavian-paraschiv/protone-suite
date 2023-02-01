using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OPMedia.Core.Logging;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Diagnostics;
using OPMedia.Core.Configuration;
using OPMedia.Core.Persistence;
using OPMedia.Core.Utilities;
using Newtonsoft.Json;
using OPMedia.Core.InterProcessCommunication;
using System.Threading.Tasks;

namespace OPMedia.Core
{
    public class PersistenceProxy : IDisposable, IPersistenceService, INotificationService
    {
        protected readonly static PersistenceProxy _proxy = new PersistenceProxy();
        private static CacheStore _cache = new CacheStore(_proxy as IPersistenceService);

        protected string _persistenceContext = string.Empty;
        protected Guid _appId = Guid.NewGuid();

        private static int _unsuccesfulAttempts = 0;

        static TicToc _readTicToc = new TicToc("Persistence.Proxy.ReadObject");

        private PersistenceClient _cl = null;

        protected PersistenceProxy()
        {
            try
            {
                if (_unsuccesfulAttempts < 5)
                {
                    if (ActivatePersistenceService())
                        _unsuccesfulAttempts = 0;
                    else
                        _unsuccesfulAttempts++;
                }
            }
            catch
            {
            }

            try
            {
                _persistenceContext = WindowsIdentity.GetCurrent().Name;
            }
            catch
            {
                _persistenceContext = string.Format("{0}\\{1}",
                    Environment.UserDomainName, Environment.UserName);
            }

            Open();
        }

        private bool ActivatePersistenceService()
        {
            int attempts = 5;

            while(attempts > 0)
            {
                try
                {
                    ServiceController srv = new ServiceController(Constants.PersistenceServiceShortName);
                    if (srv.Status == ServiceControllerStatus.Running)
                        return true;

                    srv.Start();
                    Thread.Sleep(500);
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex);
                }

                attempts--;
            }

            return false;
        }

        protected virtual void Open()
        {
            try
            {
                _cl = new PersistenceClient();

                _cl.ConnectionOpen = (connId, reconnect) => _cl.SendPdu(new ServicePDU { ActionType = ServiceActionType.Subscribe, ObjectContent = _appId.ToString() });

                _cl.PduReceived = (connId, pdu) =>
                {
                    if (pdu is NotificationPDU npdu)
                    {
                        Task.Factory.StartNew(() => DispatchNotification(npdu.ChangeType, npdu.PersistenceId, npdu.PersistenceContext, npdu.ObjectContent));
                    }
                };
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                _cl?.SendPdu(new ServicePDU { ActionType = ServiceActionType.Unsubscribe, ObjectContent = _appId.ToString() });
                _cl?.SendGracefulEnd();
                _cl?.Disconnect();
                _cl?.Dispose();
            }
            catch { }

            _cl = null;
        }

        static string BuildPersistenceId(bool includeAppName, string persistenceId)
        {
            if (includeAppName)
                return string.Format("{0}_{1}", ApplicationInfo.ApplicationName, persistenceId);

            return persistenceId;
        }


        #region ReadObject

        public static T ReadObject<T>(bool includeAppName, string persistenceId, T defaultValue, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, persistenceId);
            return ReadObject<T>(id, defaultValue, usePersistenceContext);
        }

        public static T ReadObject<T>(string persistenceId, T defaultValue, bool usePersistenceContext = true)
        {
            T retVal = defaultValue;

            try
            {
                _readTicToc.Tic();

                {
                    if (typeof(T) == typeof(byte[]))
                    {
                        byte[] blob = usePersistenceContext ?
                            _cache.ReadBlob(persistenceId, _proxy._persistenceContext) :
                            _cache.ReadBlob(persistenceId, string.Empty);
                    }
                    else
                    {
                        string content = usePersistenceContext ?
                            _cache.ReadObject(persistenceId, _proxy._persistenceContext) :
                            _cache.ReadObject(persistenceId, string.Empty);

                        if (!string.IsNullOrEmpty(content))
                        {
                            try
                            {
                                retVal = StringUtils.Coerce<T>(content);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex);
                                retVal = defaultValue;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                retVal = defaultValue;
            }
            finally
            {
                _readTicToc.Toc();
            }

            return retVal;
        }

        string IPersistenceService.ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.ReadObject persistenceId={persistenceId} persistenceContext={persistenceContext}");


                var pdu = _cl.SendPduAndWaitResponse(new PersistencePDU
                {
                    ActionType = PersistenceActionType.ReadObject,
                    PersistenceContext = persistenceContext,
                    PersistenceId = persistenceId,
                });

                if (pdu != null)
                    return pdu.ObjectContent;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        byte[] IPersistenceService.ReadBlob(string persistenceId, string persistenceContext)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.ReadBlob persistenceId={persistenceId} persistenceContext={persistenceContext}");

                var pdu = _cl.SendPduAndWaitResponse(new PersistencePDU
                {
                    ActionType = PersistenceActionType.ReadObject,
                    IsBlob = true,
                    PersistenceContext = persistenceContext,
                    PersistenceId = persistenceId,
                });

                if (pdu != null)
                    return Convert.FromBase64String(pdu.ObjectContent);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }
        #endregion

        #region SaveObject

        public static void SaveObject<T>(bool includeAppName, string persistenceId, T objectContent, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, persistenceId);
            SaveObject<T>(id, objectContent, usePersistenceContext);
        }

        static TicToc _saveTicToc = new TicToc("Persistence.Proxy.SaveObject");

        public static void SaveObject<T>(string persistenceId, T objectContent, bool usePersistenceContext = true)
        {
            try
            {
                _saveTicToc.Tic();

                {
                    if (typeof(T) == typeof(byte[]))
                    {
                        if (usePersistenceContext)
                            _cache.SaveBlob(persistenceId, _proxy._persistenceContext, objectContent as byte[]);
                        else
                            _cache.SaveBlob(persistenceId, string.Empty, objectContent as byte[]);
                    }
                    else
                    {
                        if (usePersistenceContext)
                            _cache.SaveObject(persistenceId, _proxy._persistenceContext, objectContent.ToString());
                        else
                            _cache.SaveObject(persistenceId, string.Empty, objectContent.ToString());
                    }
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

        void IPersistenceService.SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                _cl.SendPdu(new PersistencePDU
                {
                    ActionType = PersistenceActionType.SaveObject,
                    ObjectContent = objectContent,
                    PersistenceContext =  persistenceContext,
                    PersistenceId = persistenceId,
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void IPersistenceService.SaveBlob(string persistenceId, string persistenceContext, byte[] objectBlob)
        {
            try
            {
                _cl.SendPdu(new PersistencePDU
                {
                    ActionType = PersistenceActionType.SaveObject,
                    IsBlob = true,
                    ObjectContent = Convert.ToBase64String(objectBlob),
                    PersistenceContext = persistenceContext,
                    PersistenceId = persistenceId,
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region DeleteObject

        public static void DeleteObject(bool includeAppName, string persistenceId, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, persistenceId);
            DeleteObject(id, usePersistenceContext);
        }

        static TicToc _deleteTicToc = new TicToc("Persistence.Proxy.DeleteObject", 5);

        public static void DeleteObject(string persistenceId, bool usePersistenceContext = true)
        {
            try
            {
                _deleteTicToc.Tic();

                {
                    if (usePersistenceContext)
                        _cache.DeleteObject(persistenceId, _proxy._persistenceContext);
                    else
                        _cache.DeleteObject(persistenceId, string.Empty);
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

        void IPersistenceService.DeleteObject(string persistenceId, string persistenceContext)
        {
            try
            {
                _cl.SendPdu(new PersistencePDU
                {
                    ActionType = PersistenceActionType.DeleteObject,
                    PersistenceContext = persistenceContext,
                    PersistenceId = persistenceId,
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region Notifications
        public static void SendIpcEvent(string eventName, params string[] eventArgs)
        {
            string content = eventName;

            if (eventArgs?.Length > 0)
                content += $"?{StringUtils.FromStringArray(eventArgs, '|')}";

            (_proxy as INotificationService).SendNotification(NotificationType.IpcEvent, NotificationType.IpcEvent.ToString(), _proxy._persistenceContext, content);
        }

        void INotificationService.SendNotification(NotificationType changeType, string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.SendNotification persistenceId={persistenceId} persistenceContext={persistenceContext}");

                _cl.SendPdu(new NotificationPDU
                {
                    ChangeType = changeType,
                    PersistenceContext = persistenceContext,
                    PersistenceId = persistenceId,
                    ObjectContent = objectContent
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void DispatchNotification(NotificationType changeType, string persistenceId, string persistenceContext, string objectContent)
        {
            if (changeType == NotificationType.IpcEvent)
            {
                if (objectContent is string content && !string.IsNullOrEmpty(content))
                {
                    var ss = content.Split('?');
                    if (ss.Length > 0)
                    {
                        string evtName = ss[0];
                        string[] evtData = null;

                        if (ss.Length > 1)
                            evtData = StringUtils.ToStringArray(ss[1], '|');

                        EventDispatch.DispatchEvent(evtName, evtData);
                    }
                }
            }
            else if (_cache.RefreshObject(changeType, persistenceId, persistenceContext, objectContent))
            {
                Logger.LogToConsole($"Notification from PersistenceService ({changeType}, Id={persistenceId}, Context={persistenceContext}, Data={objectContent} => Cache updated. Bubbling up event to its potential consumers.");
                if (changeType == NotificationType.ObjectSaved)
                    AppConfig.OnSettingsChanged(persistenceId, persistenceContext, objectContent);
            }
            else
            {
                Logger.LogToConsole($"Notification from PersistenceService ({changeType}, Id={persistenceId}, Context={persistenceContext}, Data={objectContent} => Ignored, as we failed to update the cache.");
            }
        }
        #endregion
    }
}
