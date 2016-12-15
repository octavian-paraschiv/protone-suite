using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using OPMedia.Core;
using OPMedia.Core.Logging;
using MainContext;


namespace OPMedia.PersistenceService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PersistenceServiceImpl : IPersistenceService
    {
        const string DbConnString = "data source = \".\\Persistence.db3\"";

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                using (MainDataContext db = new MainDataContext(DbConnString))
                {
                    var s = (from po in db.PersistedObjects
                            where
                            ( 
                                po.PersistenceID == persistenceId
                                && (string.IsNullOrEmpty(persistenceContext) || string.Compare(persistenceContext, po.PersistenceContext, true) == 0)
                            )
                            select po.Content).FirstOrDefault();

                    return s;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public void SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                TransactionOptions opt = new TransactionOptions();
                //opt.IsolationLevel = IsolationLevel.Snapshot;
                opt.Timeout = TimeSpan.FromSeconds(3);

                using (MainDataContext db = new MainDataContext(DbConnString))
                {
                    var obj = (from po in db.PersistedObjects
                                where
                                (
                                    po.PersistenceID == persistenceId
                                    && (string.IsNullOrEmpty(persistenceContext) || string.Compare(persistenceContext, po.PersistenceContext, true) == 0)
                                )
                                select po);

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, opt))
                    {
                        if (obj != null && obj.Count() > 0)
                        {
                            db.PersistedObjects.DeleteAllOnSubmit(obj);
                            db.SubmitChanges();
                        }

                        PersistedObject po = new PersistedObject();
                        po.PersistenceID = persistenceId;
                        po.Content = objectContent;

                        po.PersistenceContext = string.IsNullOrEmpty(persistenceContext) ?
                            string.Empty : persistenceContext;

                        db.PersistedObjects.InsertOnSubmit(po);
                        db.SubmitChanges();

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void DeleteObject(string persistenceId, string persistenceContext)
        {
            try
            {
                TransactionOptions opt = new TransactionOptions();
                //opt.IsolationLevel = IsolationLevel.Snapshot;
                opt.Timeout = TimeSpan.FromSeconds(3);

                using (MainDataContext db = new MainDataContext(DbConnString))
                {
                    var obj = (from po in db.PersistedObjects
                                where
                                (
                                    po.PersistenceID == persistenceId
                                    && (string.IsNullOrEmpty(persistenceContext) || string.Compare(persistenceContext, po.PersistenceContext, true) == 0)
                                )
                                select po);

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, opt))
                    {
                        if (obj != null && obj.Count() > 0)
                        {
                            db.PersistedObjects.DeleteAllOnSubmit(obj);
                            db.SubmitChanges();
                        }

                        db.SubmitChanges();

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
