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

        public string ReadNode(string nodeId, string context)
        {
            return _cache.ReadNode(nodeId, context);
        }

        public bool SaveNode(string nodeId, string context, string content)
        {
            return _cache.SaveNode(nodeId, context, content);
        }

        public bool DeleteNode(string nodeId, string context)
        {
            return _cache.DeleteNode(nodeId, context);
        }
    }
}
