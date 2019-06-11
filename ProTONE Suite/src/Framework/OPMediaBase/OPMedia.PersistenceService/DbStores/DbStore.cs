# if HAVE_LITE_DB
using System;
using System.Linq;
using OPMedia.Core;
using LiteDB;
using OP_Logger = OPMedia.Core.Logging.Logger;
using System.IO;

namespace OPMedia.PersistenceService
{
    public class DbStore : IPersistenceService
    {
        private static string __peristenceDbPath = @"..\\Persistence.db";

        public DbStore()
        {
            if (File.Exists(__peristenceDbPath) == false)
                __peristenceDbPath = "Persistence.db";

            using (var db = new LiteDatabase(__peristenceDbPath))
                db.Shrink();
        }

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                using (var db = new LiteDatabase(__peristenceDbPath))
                {
                    var obj = GetObjects<PersistedObject>(db);
                    var qry = GetQuery(persistenceId, persistenceContext);

                    var cnt = obj.Find(qry).Select(p => p.Content).FirstOrDefault();
                    return cnt;
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }

            return null;
        }

        public byte[] ReadBlob(string persistenceId, string persistenceContext)
        {
            return null;
        }

        public void SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                using (var db = new LiteDatabase(__peristenceDbPath))
                {
                    var obj = GetObjects<PersistedObject>(db);
                    var qry = GetQuery(persistenceId, persistenceContext);

                    var po = obj.Find(qry).FirstOrDefault();

                    if (po == null)
                    {
                        po = new PersistedObject
                        {
                            PersistenceId = persistenceId,
                            PersistenceContext = persistenceContext,
                            Content = objectContent
                        };

                        obj.Insert(po);
                    }
                    else
                    {
                        po.Content = objectContent;
                        obj.Update(po);
                    }
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }
        }

        public void SaveBlob(string persistenceId, string persistenceContext, byte[] objectContent)
        {
        }

        public void DeleteObject(string persistenceId, string persistenceContext)
        {
            try
            {
                using (var db = new LiteDatabase(__peristenceDbPath))
                {
                    var obj = GetObjects<PersistedObject>(db);
                    var qry = GetQuery(persistenceId, persistenceContext);

                    obj.Delete(qry);
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }
        }

        private LiteCollection<T> GetObjects<T>(LiteDatabase db)
        {
            string typeName = typeof(T).Name;
            string pluralizedTypeName = string.Format("{0}s", typeName);
            return db.GetCollection<T>(pluralizedTypeName);
        }

        private Query GetQuery(string persistenceId, string persistenceContext)
        {
            if (string.IsNullOrEmpty(persistenceContext))
            {
                return Query.EQ("PersistenceId", persistenceId);
            }
            else
            {
                return Query.And
                (
                    Query.EQ("PersistenceId", persistenceId),
                    Query.EQ("PersistenceContext", persistenceContext)
                );
            }
        }

        public void Subscribe(string appId)
        {
            // Do nothing
        }

        public void Unsubscribe(string appId)
        {
            // Do nothing
        }

        public void Ping(string appid)
        {
            PersistenceServiceImpl.ReversePing();
        }
    }
}
#endif