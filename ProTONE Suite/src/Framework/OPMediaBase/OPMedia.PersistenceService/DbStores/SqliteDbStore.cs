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
        const string DbName = "Persistence.db3";
        private SQLiteConnection _db = null;

        public SqliteDbStore()
        {
            DllUtility.SelectProperDllVersion("sqlite3");

            var dbPath = Path.Combine(OperationDbFolder, DbName);
            if (!File.Exists(dbPath))
            {
                var currentDbPath = Path.Combine(PathUtils.CurrentDir, DbName);
                File.Copy(currentDbPath, dbPath);
            }

            _db = new SQLiteConnection(dbPath);
        }

        private static string OperationDbFolder
        {
            get
            {
                try
                {
                    string dbFolder = PathUtils.ProgramDataDir;
                    dbFolder = Path.Combine(dbFolder, Constants.CompanyName);
                    dbFolder = Path.Combine(dbFolder, Constants.SuiteName);
                    return dbFolder;
                }
                catch
                {
                }

                return PathUtils.CurrentDir;
            }
        }

        public Dictionary<string, string> ReadAll(string appName, string context)
        {
            Dictionary<string, string> all = new Dictionary<string, string>();

            try
            {
                var list = FindObjects(null, context)?
                    .Where(po => po.PersistenceId != null && (po.PersistenceId.StartsWith("OPMedia") == false || po.PersistenceId.StartsWith(appName)))?
                    .ToList();

                list?.ForEach(po =>
                {
                    if (all.ContainsKey(po.PersistenceId))
                        all[po.PersistenceId] = po.Content;
                    else
                        all.Add(po.PersistenceId, po.Content);
                });

            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }

            return all;
        }

        public string ReadNode(string nodeId, string context)
        {
            try
            {
                return FindObjects(nodeId, context)?.FirstOrDefault()?.Content;
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
                var x = FindObjects(nodeId, context)?.FirstOrDefault();
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
                var x = FindObjects(nodeId, context)?.FirstOrDefault();
                if (x != null)
                    _db.Delete(x);
            }
            catch (Exception ex)
            {
                OP_Logger.LogException(ex);
            }
        }

        private TableQuery<PersistedObject> FindObjects(string nodeId, string context)
        {
            return _db.Table<PersistedObject>()
                .Where(po =>
                    (context == null || context == "*" || po.PersistenceContext == null || po.PersistenceContext == "*" || po.PersistenceContext.ToUpper() == context.ToUpper()) &&
                    (nodeId == null || nodeId == "*" || (po.PersistenceId != null && po.PersistenceId.ToUpper() == nodeId.ToUpper()))
                );
        }
    }
}