using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OPMedia.Core.Logging;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;

namespace OPMedia.Core
{
    public class PersistenceProxy : IDisposable, IPersistenceService
    {
        protected static IPersistenceService _proxy = null;

        protected string _persistenceContext = string.Empty;

        private static int _unsuccesfulAttempts = 0;

        protected static PersistenceProxy CreateProxy()
        {
            return new PersistenceProxy();
        }

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

                try
                {
                    _persistenceContext = WindowsIdentity.GetCurrent().Name;
                }
                catch
                {
                    _persistenceContext = string.Format("{0}\\{1}",
                        Environment.UserDomainName, Environment.UserName);
                }
            }
            catch
            {
                _persistenceContext = string.Empty;
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
            var myBinding = new NetNamedPipeBinding();
            myBinding.MaxReceivedMessageSize = int.MaxValue;
            myBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            var myEndpoint = new EndpointAddress("net.pipe://localhost/PersistenceService.svc");
            var myChannelFactory = new ChannelFactory<IPersistenceService>(myBinding, myEndpoint);
            _proxy = myChannelFactory.CreateChannel();
        }

        protected void Abort()
        {
            try
            {
                ICommunicationObject channel = _proxy as ICommunicationObject;
                if (channel != null)
                    channel.Abort();
            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                ICommunicationObject channel = _proxy as ICommunicationObject;
                if (channel != null)
                    channel.Close();
            }
            catch { }

            _proxy = null;
        }

        string IPersistenceService.ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                return _proxy.ReadObject(persistenceId, persistenceContext);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }

            return null;
        }

        void IPersistenceService.SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                _proxy.SaveObject(persistenceId, persistenceContext, objectContent);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }
        }

        void IPersistenceService.DeleteObject(string persistenceId, string persistenceContext)
        {
            try
            {
                _proxy.DeleteObject(persistenceId, persistenceContext);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Abort();
                Open();
            }
        }

        static string BuildPersistenceId(bool includeAppName, string persistenceId)
        {
            if (includeAppName)
                return string.Format("{0}_{1}", ApplicationInfo.ApplicationName, persistenceId);

            return persistenceId;
        }

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
                using (PersistenceProxy pp = new PersistenceProxy())
                {
                    string content = usePersistenceContext ?
                        (pp as IPersistenceService).ReadObject(persistenceId, pp._persistenceContext) :
                        (pp as IPersistenceService).ReadObject(persistenceId, string.Empty);

                    if (!string.IsNullOrEmpty(content))
                    {
                        try
                        {
                            if (typeof(T).IsSubclassOf(typeof(Enum)))
                            {
                                retVal = (T)Enum.Parse(typeof(T), content);
                            }
                            else
                            {
                                try
                                {
                                    retVal = (T)Convert.ChangeType(content, typeof(T));
                                }
                                catch (InvalidCastException)
                                {
                                    retVal = (T)Enum.Parse(typeof(T), content);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            retVal = defaultValue;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                retVal = defaultValue;
            }

            return retVal;
        }

        public static void SaveObject<T>(bool includeAppName, string persistenceId, T objectContent, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, persistenceId);
            SaveObject<T>(id, objectContent, usePersistenceContext);
        }

        public static void SaveObject<T>(string persistenceId, T objectContent, bool usePersistenceContext = true)
        {
            try
            {
                using (PersistenceProxy pp = new PersistenceProxy())
                {
                    if (usePersistenceContext)
                        (pp as IPersistenceService).SaveObject(persistenceId, pp._persistenceContext, objectContent.ToString());
                    else
                        (pp as IPersistenceService).SaveObject(persistenceId, string.Empty, objectContent.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void DeleteObject(bool includeAppName, string persistenceId, bool usePersistenceContext = true)
        {
            string id = BuildPersistenceId(includeAppName, persistenceId);
            DeleteObject(id, usePersistenceContext);
        }

        public static void DeleteObject(string persistenceId, bool usePersistenceContext = true)
        {
            try
            {
                using (PersistenceProxy pp = new PersistenceProxy())
                {
                    if (usePersistenceContext)
                        (pp as IPersistenceService).DeleteObject(persistenceId, pp._persistenceContext);
                    else
                        (pp as IPersistenceService).DeleteObject(persistenceId, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
