using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OPMedia.Core.Persistence
{
    public class CacheStore
    {
        private IPersistenceService _persistence = null;
        private Dictionary<string, string> _cache = new Dictionary<string, string>();
        private object _cacheLock = new object();

        public CacheStore(IPersistenceService persistence)
        {
            _persistence = persistence;
        }

        public void Init(string context)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_cacheLock)
                {
                    var allNodes = ReadAll(ApplicationInfo.ApplicationName, context);
                    if (allNodes?.Count > 0)
                    {
                        _cache = new Dictionary<string, string>(allNodes);
                    }
                }
            });
        }

        public Dictionary<string, string> ReadAll(string appName, string context)
        {
            if (string.IsNullOrEmpty(context))
                context = "*";

            return _persistence.ReadAll(appName, context);
        }

        #region ReadNode

        public string ReadNode(string nodeId, string context)
        {
            string ret = ReadCacheItem(nodeId, ref context);

            if (ret == null)
            {
                ret = _persistence.ReadNode(nodeId, context);
                AddOrUpdateCacheItem(nodeId, ref context, ret);
            }

            return ret;
        }
        #endregion

        #region SaveNode

        public bool SaveNode(string nodeId, string context, string content)
        {
            bool retVal = false;

            try
            {
                AddOrUpdateCacheItem(nodeId, ref context, content);
                ThreadPool.QueueUserWorkItem(_ => _persistence.SaveNode(nodeId, context, content));
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        #endregion

        #region DeleteNode
        public bool DeleteNode(string nodeId, string context)
        {
            bool retVal = false;

            try
            {
                DeleteCacheItem(nodeId, ref context);

                retVal = true;
                var ctx = context;

                ThreadPool.QueueUserWorkItem(_ => _persistence.DeleteNode(nodeId, ctx));
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }
        #endregion


        private void AddOrUpdateCacheItem(string nodeId, ref string context, string value)
        {
            string key = BuildKey(nodeId, ref context);

            lock (_cacheLock)
            {
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole($"[UPDATE CACHE] [{nodeId}][{context}] = {value}");
                    _cache[key] = value;
                }
                else
                {
                    Logger.LogToConsole($"[ADD CACHE] [{nodeId}][{context}] = {value}");
                    _cache.Add(key, value);
                }
            }
        }

        private void DeleteCacheItem(string nodeId, ref string context)
        {
            string key = BuildKey(nodeId, ref context);

            lock (_cacheLock)
            {
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole($"[DELETE CACHE] [{nodeId}][{context}]");
                    _cache.Remove(key);
                }
            }
        }

        private string ReadCacheItem(string nodeId, ref string context)
        {
            string ret = null;


            string key = BuildKey(nodeId, ref context);

            lock (_cacheLock)
            {
                if (_cache.ContainsKey(key))
                    ret = _cache[key];
                else
                    Logger.LogToConsole($"[READ CACHE] [{nodeId}][{context}] not found in cache");

            }

            return ret;
        }

        internal bool RefreshObject(NotificationType changeType, string nodeId, string context, string content)
        {
            bool success = false;

            try
            {
                Logger.LogToConsole($"RefreshObject request: ({changeType}, Id={nodeId}, Context={context}, Data={content}");
                switch (changeType)
                {
                    case NotificationType.NodeDeleted:
                        {
                            DeleteCacheItem(nodeId, ref context);
                            success = true;
                        }
                        break;

                    case NotificationType.NodeSaved:
                        {
                            AddOrUpdateCacheItem(nodeId, ref context, content);
                            success = true;
                        }
                        break;

                    default:
                        Logger.LogToConsole($"ForceUpdate({changeType}): Ignoring event.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                success = false;
            }

            return success;
        }

        private string BuildKey(string nodeId, ref string context)
        {
            if (string.IsNullOrEmpty(context))
                context = "*";

            return string.Format("{0}_{1}", nodeId, context);
        }
    }

}
