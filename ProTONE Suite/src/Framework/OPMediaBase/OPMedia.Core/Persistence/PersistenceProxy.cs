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

namespace OPMedia.Core
{
    public class PersistenceProxy : IDisposable, IPersistenceService, IPersistenceNotification
    {
        protected readonly static PersistenceProxy _proxy = new PersistenceProxy();
        private static CacheStore _cache = new CacheStore(_proxy);

        protected IPersistenceService _channel = null;

        protected string _persistenceContext = string.Empty;
        protected Guid _appId = Guid.NewGuid();

        private static int _unsuccesfulAttempts = 0;

        static TicToc _readTicToc = new TicToc("Persistence.Proxy.ReadObject");
        static int _readCount = 0;


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
                var myBinding = new NetTcpBinding();
                myBinding.MaxReceivedMessageSize = int.MaxValue;
                myBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

                myBinding.OpenTimeout = TimeSpan.FromSeconds(4);
                myBinding.CloseTimeout = TimeSpan.FromSeconds(4);
                myBinding.SendTimeout = TimeSpan.FromMilliseconds(500);
                myBinding.ReceiveTimeout = TimeSpan.FromSeconds(30);
                myBinding.ReliableSession.InactivityTimeout = TimeSpan.FromSeconds(30);

                var instanceContext = new InstanceContext(this);
                var myEndpoint = new EndpointAddress(PersistenceConstants.PersistenceServiceAddress);
                var myChannelFactory = new DuplexChannelFactory<IPersistenceService>(instanceContext, myBinding, myEndpoint);

                _channel = myChannelFactory.CreateChannel();

                var appId = _appId.ToString();
                _channel.Subscribe(appId);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        protected void Abort()
        {
            try
            {
                ICommunicationObject channel = _channel as ICommunicationObject;
                if (channel != null)
                    channel.Abort();
            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                if (_channel != null)
                {
                    var appId = _appId.ToString();
                    _channel.Unsubscribe(appId);

                    ICommunicationObject channel = _channel as ICommunicationObject;
                    if (channel != null)
                        channel.Close();
                }
            }
            catch { }

            _channel = null;
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

                        _readCount++;
                    }
                    else
                    {
                        string content = usePersistenceContext ?
                            _cache.ReadObject(persistenceId, _proxy._persistenceContext) :
                            _cache.ReadObject(persistenceId, string.Empty);

                        _readCount++;

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

                if (_channel != null) 
                    return _channel.ReadObject(persistenceId, persistenceContext);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }

            return null;
        }

        byte[] IPersistenceService.ReadBlob(string persistenceId, string persistenceContext)
        {
            try
            {
                Logger.LogTrace($"IPersistenceService.ReadBlob persistenceId={persistenceId} persistenceContext={persistenceContext}");

                if (_channel != null)
                    return _channel.ReadBlob(persistenceId, persistenceContext);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
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
                if (_channel != null)
                    _channel.SaveObject(persistenceId, persistenceContext, objectContent);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }
        }

        void IPersistenceService.SaveBlob(string persistenceId, string persistenceContext, byte[] objectBlob)
        {
            try
            {
                if (_channel != null)
                    _channel.SaveBlob(persistenceId, persistenceContext, objectBlob);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
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
                if (_channel != null)
                    _channel.DeleteObject(persistenceId, persistenceContext);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }
        }
        #endregion

        void IPersistenceService.Subscribe(string appId)
        {
            throw new NotSupportedException();
        }

        void IPersistenceService.Unsubscribe(string appId)
        {
            throw new NotSupportedException();
        }

        void IPersistenceService.Ping(string appid)
        {
            try
            {
                if (_channel != null)
                    _channel.Ping(_appId.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }
        }
        
        void IPersistenceNotification.Notify(ChangeType changeType, string persistenceId, string persistenceContext, string objectContent)
        {
            if (changeType == ChangeType.None && objectContent == "ping")
            {
                Logger.LogToConsole("Ping received from PersistenceService");
                return;
            }

            if (persistenceContext == null || persistenceContext == "*" || persistenceContext == _persistenceContext)
                ThreadPool.QueueUserWorkItem((c) => ThreadedNotify(changeType, persistenceId, persistenceContext, objectContent));
        }

        void IPersistenceNotification.NotifyBlob(ChangeType changeType, string persistenceId, string persistenceContext, byte[] objectContent)
        {
            if (persistenceContext == null || persistenceContext == "*" || persistenceContext == _persistenceContext)
                ThreadPool.QueueUserWorkItem((c) => ThreadedNotify(changeType, persistenceId, persistenceContext, objectContent));
        }

        void ThreadedNotify(ChangeType changeType, string persistenceId, string persistenceContext, object objectContent)
        {
            if (_cache.RefreshObject(changeType, persistenceId, persistenceContext, objectContent))
            {
                Logger.LogToConsole($"Notification from PersistenceService ({changeType}, Id={persistenceId}, Context={persistenceContext}, Data={objectContent} => Cache updated. Bubbling up event to its potential consumers.");
                AppConfig.OnSettingsChanged(changeType, persistenceId, persistenceContext, objectContent);
            }
            else
            {
                Logger.LogToConsole($"Notification from PersistenceService ({changeType}, Id={persistenceId}, Context={persistenceContext}, Data={objectContent} => Ignored, as we failed to update the cache.");
            }
        }
    }
}
