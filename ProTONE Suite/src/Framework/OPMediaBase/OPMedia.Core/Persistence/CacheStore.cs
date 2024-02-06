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
                var allNodes = ReadAll(ApplicationInfo.ApplicationName, context);
                if (allNodes?.Count > 0)
                {
                    lock (_cacheLock)
                    {
                        foreach (var node in allNodes)
                        {
                            AddOrUpdateCacheItem(node.Key, context, node.Value);
                        }
                    }
                }
            });
        }

        public Dictionary<string, string> ReadAll(string appName, string context)
        {
            return _persistence.ReadAll(appName, context);
        }

        #region ReadNode

        public string ReadNode(string nodeId, string context)
        {
            if (string.IsNullOrEmpty(context))
                context = "*";

            string ret = ReadCacheItem(nodeId, context);

            if (ret == null)
            {
                ret = _persistence.ReadNode(nodeId, context);
                AddOrUpdateCacheItem(nodeId, context, ret);
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
                if (string.IsNullOrEmpty(context))
                    context = "*";

                AddOrUpdateCacheItem(nodeId, context, content);

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
                if (string.IsNullOrEmpty(context))
                    context = "*";

                DeleteCacheItem(nodeId, context);

                retVal = true;

                ThreadPool.QueueUserWorkItem(_ => _persistence.DeleteNode(nodeId, context));
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }
        #endregion


        private void AddOrUpdateCacheItem(string nodeId, string context, string value)
        {
            string key = string.Format("{0}_{1}", nodeId, context);

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

        private void DeleteCacheItem(string nodeId, string context)
        {
            string key = string.Format("{0}_{1}", nodeId, context);

            lock (_cacheLock)
            {
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole($"[DELETE CACHE] [{nodeId}][{context}]");
                    _cache.Remove(key);
                }
            }
        }

        private string ReadCacheItem(string nodeId, string context)
        {
            string ret = null;
            string key = string.Format("{0}_{1}", nodeId, context);

            lock (_cacheLock)
            {
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole($"[READ CACHE] [{nodeId}][{context}] = {ret}");
                    ret = _cache[key];
                }
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
                            DeleteCacheItem(nodeId, context);
                            success = true;
                        }
                        break;

                    case NotificationType.NodeSaved:
                        {
                            AddOrUpdateCacheItem(nodeId, context, content);
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

    }

}
