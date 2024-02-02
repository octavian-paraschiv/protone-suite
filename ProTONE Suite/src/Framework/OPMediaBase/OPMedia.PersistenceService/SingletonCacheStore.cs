using OPMedia.Core;
using OPMedia.Core.Persistence;

namespace OPMedia.PersistenceService
{
    public class SingletonCacheStore
    {
        private static SingletonCacheStore _instance = new SingletonCacheStore();

        public static SingletonCacheStore Instance
        {
            get
            {
                return _instance;
            }
        }

        private CacheStore _cache = null;
        private IPersistenceService _db = null;

        private SingletonCacheStore()
        {
#if HAVE_LITE_DB
            _db = new DbStore();
#else
            _db = new SqliteDbStore();
#endif

            _cache = new CacheStore(_db);
        }

        public string ReadObject(string persistenceId, string persistenceContext)
        {
            return _cache.ReadObject(persistenceId, persistenceContext);
        }

        public bool SaveObject(string persistenceId, string persistenceContext, string objectContent)
        {
            return _cache.SaveObject(persistenceId, persistenceContext, objectContent);
        }

        public bool DeleteObject(string persistenceId, string persistenceContext)
        {
            return _cache.DeleteObject(persistenceId, persistenceContext);
        }
    }
}
