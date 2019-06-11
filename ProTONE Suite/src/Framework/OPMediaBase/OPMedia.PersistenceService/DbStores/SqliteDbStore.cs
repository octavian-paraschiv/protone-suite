#if HAVE_LITE_DB
#else
using System;
using System.Collections.Generic;
using System.Linq;
using OPMedia.Core;
using OP_Logger = OPMedia.Core.Logging.Logger;
using System.IO;
using SQLite;

namespace OPMedia.PersistenceService
{
    public class SqliteDbStore : IPersistenceService
    {
        private static string __peristenceDbPath = @"..\\Persistence.db3";
        private SQLiteConnection _db = null;

        public SqliteDbStore()
        {
            if (File.Exists(__peristenceDbPath) == false)
                __peristenceDbPath = "Persistence.db3";

            _db = new SQLiteConnection(__peristenceDbPath);
        }

        public List<PersistedObject> PersistedObjects
        {
            get { return _db.Table<PersistedObject>().ToList(); }
        }


        public string ReadObject(string persistenceId, string persistenceContext)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == persistenceId && po.PersistenceContext == persistenceContext
                         select po.Content).FirstOrDefault();

                return x;
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }

            return null;
        }

        public void SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == persistenceId && po.PersistenceContext == persistenceContext
                         select po).FirstOrDefault();

                if (x == null)
                {
                    x = new PersistedObject
                    {
                        PersistenceId = persistenceId,
                        PersistenceContext = persistenceContext,
                        Content = objectContent
                    };

                    _db.Insert(x);
                }
                else
                {
                    x.Content = objectContent;
                    _db.Update(x);
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }
        }

        public byte[] ReadBlob(string persistenceId, string persistenceContext)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == persistenceId && po.PersistenceContext == persistenceContext
                         select po.ContentBlob).FirstOrDefault();

                return x;
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }

            return null;
        }

        public void SaveBlob(string persistenceId, string persistenceContext, byte[] objectBlob)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == persistenceId && po.PersistenceContext == persistenceContext
                         select po).FirstOrDefault();

                if (x == null)
                {
                    x = new PersistedObject
                    {
                        PersistenceId = persistenceId,
                        PersistenceContext = persistenceContext,
                        ContentBlob = objectBlob
                    };

                    _db.Insert(x);
                }
                else
                {
                    x.ContentBlob = objectBlob;
                    _db.Update(x);
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }
        }

        public void DeleteObject(string persistenceId, string persistenceContext)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == persistenceId && po.PersistenceContext == persistenceContext
                         select po).FirstOrDefault();

                if (x != null)
                {
                    _db.Delete(x);
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
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