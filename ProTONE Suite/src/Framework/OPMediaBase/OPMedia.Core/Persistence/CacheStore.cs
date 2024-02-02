using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OPMedia.Core.Persistence
{
    public class CacheItem
    {
        // Time-To-Live: 30 seconds
        public const int MaxCachedItemTTL = 30 * 1000;

        public object Key { get; private set; }

        object _accessLock = new object();

        public string _value = null;
        public string Value
        {
            get
            {
                lock (_accessLock)
                {
                    this.TimeStamp = DateTime.Now;
                    return _value;
                }
            }

            set
            {
                lock (_accessLock)
                {
                    this.TimeStamp = DateTime.Now;
                    _value = value;
                }
            }
        }

        public DateTime TimeStamp { get; private set; }

        public bool IsExpired
        {
            get
            {
                TimeSpan ts = DateTime.Now - TimeStamp;
                return (ts.TotalMilliseconds >= MaxCachedItemTTL);
            }
        }

        public CacheItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public class CacheStore
    {
        // Poll for expired items at each 10 seconds.
        const int ExpiredItemsPollTimer = 10 * 1000;
        const int IterationsUntilGarbageCollection = (10 * 60 * 1000) / ExpiredItemsPollTimer;

        private IPersistenceService _persistence = null;

        private Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();

        private System.Timers.Timer _tmrCachePoller = null;
        private object _cachePollerLock = new object();

        public CacheStore(IPersistenceService persistence)
        {
            _persistence = persistence;

            _tmrCachePoller = new System.Timers.Timer();
            _tmrCachePoller.Interval = ExpiredItemsPollTimer;
            _tmrCachePoller.Elapsed += new System.Timers.ElapsedEventHandler(_tmrCachePoller_Elapsed);
            _tmrCachePoller.Start();
        }

        int _iter = 0;
        void _tmrCachePoller_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _tmrCachePoller.Stop();
                PollForExpiredCachedItems();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                try
                {
                    _iter = (_iter + 1) % IterationsUntilGarbageCollection;
                    if (_iter == 0)
                        GC.Collect();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

                _tmrCachePoller.Start();
            }
        }

        private void PollForExpiredCachedItems()
        {
            lock (_cachePollerLock)
            {
                List<string> keysToDelete = new List<string>();

                foreach (KeyValuePair<string, CacheItem> kvp in _cache)
                {
                    if (kvp.Value.IsExpired)
                    {
                        Logger.LogToConsole("PollForExpiredCachedItems: Object with key {0} seems to be expired.", kvp.Key);
                        keysToDelete.Add(kvp.Key);
                    }
                }

                Logger.LogToConsole("PollForExpiredCachedItems: A number of {0} objects will be removed from the cache...", keysToDelete.Count);

                foreach (string key in keysToDelete)
                {
                    if (_cache.ContainsKey(key))
                    {
                        Logger.LogToConsole("PollForExpiredCachedItems: Object with key {0} was removed from cache because it was expired.", key);
                        _cache.Remove(key);
                    }
                }
            }
        }

        #region ReadNode

        public string ReadNode(string nodeId, string context)
        {
            return __Read(nodeId, context);
        }

        private string __Read(string nodeId, string context)
        {
            if (string.IsNullOrEmpty(context))
                context = "*";

            string ret = null;

            string key = BuildCacheKey(nodeId, context);

            lock (_cachePollerLock)
            {
                // Is the requested object in the cache ?
                if (_cache.ContainsKey(key))
                {
                    ret = _cache[key].Value;
                    Logger.LogToConsole($"ReadNode: {key} from cache => {ret ?? "<null>"}");
                }
            }

            if (ret == null)
            {
                Logger.LogToConsole($"ReadNode: {key} not found in cache, or it was null");

                ret = _persistence.ReadNode(nodeId, context);

                if (ret != null)
                    Logger.LogToConsole($"ReadNode: {key} from DB => {ret}");
                else
                    Logger.LogToConsole($"ReadNode: {key} not found in DB");


                CacheItem ci = new CacheItem(key, ret);

                lock (_cachePollerLock)
                {
                    _cache.Add(key, ci);
                    Logger.LogToConsole($"ReadNode: {key} added to cache with TTL = {CacheItem.MaxCachedItemTTL} sec");
                }
            }

            return ret;
        }
        #endregion

        #region SaveNode

        public bool SaveNode(string nodeId, string context, string content)
        {
            return __Save(nodeId, context, content);
        }

        private bool __Save(string nodeId, string context, string content)
        {
            bool retVal = false;

            try
            {
                if (string.IsNullOrEmpty(context))
                    context = "*";

                string key = BuildCacheKey(nodeId, context);
                bool foundInCache = false;

                lock (_cachePollerLock)
                {
                    // Is the requested object in the cache ?
                    if (_cache.ContainsKey(key))
                    {
                        Logger.LogToConsole($"SaveNode: {key} updated in cache to value {content}");

                        // If it is, update it in the cache.
                        _cache[key].Value = content;
                        foundInCache = true;

                        retVal = true;
                    }
                }

                // We need to update in DB anyways and also make sure it is in the cache.
                if (foundInCache == false)
                {
                    CacheItem ci = new CacheItem(key, content);

                    lock (_cachePollerLock)
                    {
                        Logger.LogToConsole($"SaveNode: {key} added in cache with value {content}");
                        _cache.Add(key, ci);

                        retVal = true;
                    }
                }

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    _persistence.SaveNode(nodeId, context, content);
                    Logger.LogToConsole($"SaveNode: {key} saved in DB with value {content}");
                });
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        #endregion

        public bool DeleteNode(string nodeId, string context)
        {
            bool retVal = false;

            try
            {
                if (string.IsNullOrEmpty(context))
                    context = "*";

                string key = BuildCacheKey(nodeId, context);

                lock (_cachePollerLock)
                {
                    if (_cache.ContainsKey(key))
                    {
                        Logger.LogToConsole("DeleteNode: Object with key {0} was found in cache and removed.", key);
                        _cache.Remove(key);

                        retVal = true;
                    }
                }

                ThreadPool.QueueUserWorkItem((c) =>
                    {
                        Logger.LogToConsole("DeleteNode: Object with key {0} is now removed also from the DB.", key);
                        _persistence.DeleteNode(nodeId, context);
                    });
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        private string BuildCacheKey(string nodeId, string context)
        {
            return string.Format("{0}_{1}", nodeId, context);
        }

        internal bool RefreshObject(NotificationType changeType, string nodeId, string context, string content)
        {
            bool success = false;

            try
            {
                Logger.LogToConsole($"RefreshObject request: ({changeType}, Id={nodeId}, Context={context}, Data={content}");

                string key = BuildCacheKey(nodeId, context);
                lock (_cachePollerLock)
                {
                    bool isInCache = _cache.ContainsKey(key);

                    switch (changeType)
                    {
                        case NotificationType.NodeDeleted:
                            if (isInCache)
                            {
                                Logger.LogToConsole($"ForceUpdate({changeType}): Object with key {key} was found in cache and removed");
                                success = _cache.Remove(key);
                            }
                            else
                            {
                                Logger.LogToConsole($"ForceUpdate({changeType}): Object with key {key} was not found in cache. Ignoring event.");
                            }
                            break;

                        case NotificationType.NodeSaved:
                            {
                                CacheItem ci = new CacheItem(key, content);

                                if (isInCache)
                                {
                                    Logger.LogToConsole($"ForceUpdate({changeType}): Object with key {key} was found in cache and assigned to value={content}");
                                    _cache[key] = ci;
                                    success = true;
                                }
                                else
                                {
                                    Logger.LogToConsole($"ForceUpdate({changeType}): Object with key {key} was NOT found in cache and added now with value={content}");
                                    _cache.Add(key, ci);
                                    success = true;
                                }
                            }
                            break;

                        default:
                            Logger.LogToConsole($"ForceUpdate({changeType}): Ignoring event.");
                            break;
                    }
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
