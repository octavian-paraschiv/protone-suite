using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core;
using OPMedia.Core.Logging;
using System.Threading;

namespace OPMedia.PersistenceService
{
    public class CacheItem
    {
        public string Key { get; private set; }
        public string Value { get; set; }
        public DateTime TimeStamp { get; private set; }

        public bool IsExpired
        {
            get
            {
                TimeSpan ts = DateTime.Now - TimeStamp;
                return (ts.TotalMilliseconds >= CacheStore.MaxCachedItemTTL);
            }
        }

        public CacheItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.TimeStamp = DateTime.Now;
        }
    }

    public class CacheStore : IPersistenceService
    {
        public const int MaxCachedItemTTL = 5000;

        private static CacheStore _instance = new CacheStore();
        public static CacheStore Instance
        {
            get
            {
                return _instance;
            }
        }

        private Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();

        private System.Timers.Timer _tmrCachePoller = null;
        private object _cachePollerLock = new object();

        private CacheStore()
        {
            _tmrCachePoller = new System.Timers.Timer();
            _tmrCachePoller.Interval = MaxCachedItemTTL;
            _tmrCachePoller.Elapsed += new System.Timers.ElapsedEventHandler(_tmrCachePoller_Elapsed);
            _tmrCachePoller.Start();
        }

        void _tmrCachePoller_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _tmrCachePoller.Stop();
                PollForExpiredCachedItems();
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                _tmrCachePoller.Start();
            }
        }

        private void PollForExpiredCachedItems()
        {
            lock (_cachePollerLock)
            {
                List<string> keysToDelete = new List<string>();

                foreach(KeyValuePair<string, CacheItem> kvp in _cache)
                {
                    if (kvp.Value.IsExpired)
                    {
                        Logger.LogToConsole("PollForExpiredCachedItems: Object with key {0} seems to be expired.", kvp.Key);
                        keysToDelete.Add(kvp.Key);
                    }
                }

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

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            string key = BuildCacheKey(persistenceId, persistenceContext);

            lock (_cachePollerLock)
            {
                // Is the requested object in the cache ?
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole("ReadObject: Object with key {0} was found in cache.", key);

                    // If it is, return it from the cache.
                    return _cache[key].Value;
                }
            }

            Logger.LogToConsole("ReadObject: Object with key {0} was not found in cache.", key);

            // If it is not, get it from the persistence DB and also add it in the cache.
            string s = DbStore.ReadObject(persistenceId, persistenceContext);
            if (s != null)
            {
                Logger.LogToConsole("ReadObject: Object with key {0} was found in DB.", key);

                CacheItem ci = new CacheItem(key, s);

                lock (_cachePollerLock)
                {
                    Logger.LogToConsole("ReadObject: Object with key {0} was added to cache with a TTL of 5 sec", key);
                    _cache.Add(key, ci);
                }

                return ci.Value;
            }

            Logger.LogToConsole("ReadObject: Object with key {0} was not found in DB also.", key);

            return null;
        }

        public void SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            string key = BuildCacheKey(persistenceId, persistenceContext);
            bool foundInCache = false;

            lock (_cachePollerLock)
            {
                // Is the requested object in the cache ?
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole("SaveObject: Object with key {0} was found and updated in cache.", key);

                    // If it is, update it in the cache.
                    _cache[key].Value = objectContent;
                    foundInCache = true;
                }
            }

            // We need to update in DB anyways and also make sure it is in the cache.
            if (foundInCache == false)
            {
                CacheItem ci = new CacheItem(key, objectContent);

                lock (_cachePollerLock)
                {
                    Logger.LogToConsole("SaveObject: Object with key {0} was not found cache => adding it now", key);
                    _cache.Add(key, ci);
                }
            }

            ThreadPool.QueueUserWorkItem((c) => 
                {
                    Logger.LogToConsole("SaveObject: Object with key {0} is now saved also in DB.", key);
                    DbStore.SaveObject(persistenceId, persistenceContext, objectContent);
                });
        }

        public void DeleteObject(string persistenceId, string persistenceContext)
        {
            string key = BuildCacheKey(persistenceId, persistenceContext);

            lock (_cachePollerLock)
            {
                if (_cache.ContainsKey(key))
                {
                    Logger.LogToConsole("DeleteObject: Object with key {0} was found in cache and removed.", key);
                    _cache.Remove(key);
                }
            }

            ThreadPool.QueueUserWorkItem((c) =>
                {
                    Logger.LogToConsole("DeleteObject: Object with key {0} is now removed also from the DB.", key);
                    DbStore.DeleteObject(persistenceId, persistenceContext);
                });
        }

        private string BuildCacheKey(string persistenceId, string persistenceContext)
        {
            return string.Format("{0}_{1}", persistenceId, persistenceContext);
        }

    }

}
