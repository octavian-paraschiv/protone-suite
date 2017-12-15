using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using OPMedia.Core;
using OPMedia.Core.Logging;

namespace OPMedia.PersistenceService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PersistenceServiceImpl : IPersistenceService
    {
        static TicToc _readTicToc = new TicToc("Persistence.Service.ReadObject");
        static TicToc _saveTicToc = new TicToc("Persistence.Service.SaveObject");
        static TicToc _deleteTicToc = new TicToc("Persistence.Service.DeleteObject");

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                _readTicToc.Tic();
                return CacheStore.Instance.ReadObject(persistenceId, persistenceContext);
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
                CacheStore.Instance.SaveObject(persistenceId, persistenceContext, objectContent);
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
                CacheStore.Instance.DeleteObject(persistenceId, persistenceContext);
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
    }
}
