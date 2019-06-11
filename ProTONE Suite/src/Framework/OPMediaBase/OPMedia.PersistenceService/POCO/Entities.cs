//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
// See the blog post here for help on using the generated code: http://erikej.blogspot.dk/2014/10/database-first-with-sqlite-in-universal.html
using SQLite;
using System;

namespace OPMedia.PersistenceService
{
#if HAVE_LITE_DB
    public class PersistedObject
    {
        public Int64 _id { get; set; }
        public string PersistenceId { get; set; }
        public string PersistenceContext  { get; set; }
        public string Content { get; set; }
    }
#else
    public partial class PersistedObject
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public Int64 _id { get; set; }

        public String PersistenceId { get; set; }

        public String PersistenceContext { get; set; }

        public String Content { get; set; }

        public Byte[] ContentBlob { get; set; }
    }
#endif
}
