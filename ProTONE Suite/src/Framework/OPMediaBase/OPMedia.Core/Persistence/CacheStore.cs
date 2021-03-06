﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core;
using OPMedia.Core.Logging;
using System.Threading;

namespace OPMedia.Core.Persistence
{
    public class CacheItem
    {
        // Time-To-Live: 30 seconds
        public const int MaxCachedItemTTL = 30 * 1000;

        public object Key { get; private set; }

        object _accessLock = new object();

        public object _value = null;
        public object Value 
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

        public CacheItem(string key, object value)
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

                _persistence.Ping(null);

            }
            catch(Exception ex)
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

        #region ReadObject / ReadBlob

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            return __Read(persistenceId, persistenceContext, false) as string;
        }

        public byte[] ReadBlob(string persistenceId, string persistenceContext)
        {
            return __Read(persistenceId, persistenceContext, true) as byte[];
        }

        private object __Read(string persistenceId, string persistenceContext, bool isBlob)
        {
            if (string.IsNullOrEmpty(persistenceContext))
                persistenceContext = "*";

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
            object s = null;

            if (isBlob)
                s = _persistence.ReadBlob(persistenceId, persistenceContext);
            else
                s = _persistence.ReadObject(persistenceId, persistenceContext);

            if (s != null)
                Logger.LogToConsole("ReadObject: Object with key {0} was found in DB.", key);
            else
                Logger.LogToConsole("ReadObject: Object with key {0} was not found in DB also.", key);


            CacheItem ci = new CacheItem(key, s);

            lock (_cachePollerLock)
            {
                Logger.LogToConsole("ReadObject: Object with key {0} was added to cache with a TTL of {1} sec", key, CacheItem.MaxCachedItemTTL);
                _cache.Add(key, ci);
            }

            return ci.Value;
        }
        #endregion

        #region SaveObject / SaveBlob

        public bool SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            return __Save(persistenceId, persistenceContext, objectContent);
        }

        public bool SaveBlob(string persistenceId, string persistenceContext, byte[] objectBlob)
        {
            return __Save(persistenceId, persistenceContext, objectBlob);
        }

        private bool __Save(string persistenceId, string persistenceContext, object objectContent)
        {
            bool retVal = false;

            try
            {
                if (string.IsNullOrEmpty(persistenceContext))
                    persistenceContext = "*";

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

                        retVal = true;
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

                        retVal = true;
                    }
                }

                ThreadPool.QueueUserWorkItem((c) =>
                    {
                        Logger.LogToConsole("SaveObject: Object with key {0} is now saved also in DB.", key);

                        if (objectContent is byte[])
                            _persistence.SaveBlob(persistenceId, persistenceContext, objectContent as byte[]);
                        else
                            _persistence.SaveObject(persistenceId, persistenceContext, objectContent as string);
                    });
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        #endregion

        public bool DeleteObject(string persistenceId, string persistenceContext)
        {
            bool retVal = false;

            try
            {
                if (string.IsNullOrEmpty(persistenceContext))
                    persistenceContext = "*";

                string key = BuildCacheKey(persistenceId, persistenceContext);

                lock (_cachePollerLock)
                {
                    if (_cache.ContainsKey(key))
                    {
                        Logger.LogToConsole("DeleteObject: Object with key {0} was found in cache and removed.", key);
                        _cache.Remove(key);

                        retVal = true;
                    }
                }

                ThreadPool.QueueUserWorkItem((c) =>
                    {
                        Logger.LogToConsole("DeleteObject: Object with key {0} is now removed also from the DB.", key);
                        _persistence.DeleteObject(persistenceId, persistenceContext);
                    });
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        private string BuildCacheKey(string persistenceId, string persistenceContext)
        {
            return string.Format("{0}_{1}", persistenceId, persistenceContext);
        }

        internal bool RefreshObject(ChangeType changeType, string persistenceId, string persistenceContext, object objectContent)
        {
            bool success = false;

            try
            {
                Logger.LogToConsole($"RefreshObject request: ({changeType}, Id={persistenceId}, Context={persistenceContext}, Data={objectContent}");

                string key = BuildCacheKey(persistenceId, persistenceContext);
                lock (_cachePollerLock)
                {
                    bool isInCache = _cache.ContainsKey(key);

                    switch (changeType)
                    {
                        case ChangeType.Deleted:
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

                        case ChangeType.Saved:
                            {
                                CacheItem ci = new CacheItem(key, objectContent);

                                if (isInCache)
                                {
                                    Logger.LogToConsole($"ForceUpdate({changeType}): Object with key {key} was found in cache and assigned to value={objectContent}");
                                    _cache[key] = ci;
                                    success = true;
                                }
                                else
                                {
                                    Logger.LogToConsole($"ForceUpdate({changeType}): Object with key {key} was NOT found in cache and added now with value={objectContent}");
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
