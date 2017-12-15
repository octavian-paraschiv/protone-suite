using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.SimpleCacheService
{
    public class PersistedObject
    {
        public int Id { get; set; }
        public string PersistenceId { get; set; }
        public string PersistenceContext  { get; set; }
        public string Content { get; set; }
    }
}
