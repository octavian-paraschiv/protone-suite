#if HAVE_LITE_DB
#else
using OPMedia.Core;
using OPMedia.Core.Win32;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OP_Logger = OPMedia.Core.Logging.Logger;

namespace OPMedia.PersistenceService
{
    public class SqliteDbStore : IPersistenceService
    {
        private static string __peristenceDbPath = @"..\\Persistence.db3";
        private SQLiteConnection _db = null;

        public SqliteDbStore()
        {
            DllUtility.SelectProperDllVersion("sqlite3");

            if (File.Exists(__peristenceDbPath) == false)
                __peristenceDbPath = "Persistence.db3";

            _db = new SQLiteConnection(__peristenceDbPath);
        }

        public List<PersistedObject> PersistedObjects
        {
            get { return _db.Table<PersistedObject>().ToList(); }
        }


        public string ReadNode(string nodeId, string context)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == nodeId && po.PersistenceContext == context
                         select po.Content).FirstOrDefault();

                return x;
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }

            return null;
        }

        public void SaveNode(string nodeId, string context, string content)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == nodeId && po.PersistenceContext == context
                         select po).FirstOrDefault();

                if (x == null)
                {
                    x = new PersistedObject
                    {
                        PersistenceId = nodeId,
                        PersistenceContext = context,
                        Content = content
                    };

                    _db.Insert(x);
                }
                else
                {
                    x.Content = content;
                    _db.Update(x);
                }
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }
        }

        public void DeleteNode(string nodeId, string context)
        {
            try
            {
                var x = (from po in PersistedObjects
                         where po.PersistenceId == nodeId && po.PersistenceContext == context
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


        public void Notify(NotificationType changeType, string nodeId, string context, string content) { }
        public void SendNotificationBlob(NotificationType changeType, string nodeId, string context, byte[] objectBlob) { }
    }
}
#endif