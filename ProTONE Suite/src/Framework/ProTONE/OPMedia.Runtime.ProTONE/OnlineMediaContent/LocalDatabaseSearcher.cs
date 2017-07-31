using OPMedia.Core;
using OPMedia.Runtime.ProTONE.Playlists;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public class LocalDatabaseSearcher : OnlineContentSearcher
    {
        protected override bool HasValidConfig
        {
            get
            {
                return true;
            }
        }

        protected override List<IOnlineMediaItem> Search(string search, ManualResetEvent abortEvent)
        {
            List<IOnlineMediaItem> results = new List<IOnlineMediaItem>();

            if (abortEvent.WaitOne(5))
                return results;

            RadioStationsData internalDatabase = null;

            // Fill with Internal stations list from Persistence Service
            string xml = PersistenceProxy.ReadObject("RadioStationsData", string.Empty, false);
            if (!string.IsNullOrEmpty(xml))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (StringReader sr = new StringReader(xml))
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(RadioStationsData));
                    internalDatabase = xs.Deserialize(xr) as RadioStationsData;
                }
            }

            if (abortEvent.WaitOne(5))
                return results;

            if (internalDatabase != null &&
                internalDatabase.RadioStations != null &&
                internalDatabase.RadioStations.Count > 0)
            {
                foreach (var rs in internalDatabase.RadioStations)
                {
                    if (abortEvent.WaitOne(5))
                        break;

                    var content = rs.Content ?? string.Empty;
                    var genre = rs.Genre ?? string.Empty;
                    var title = rs.Title ?? string.Empty;
                    var url = rs.Url ?? string.Empty;

                    if (content.ToLowerInvariant().Contains(search) ||
                        genre.ToLowerInvariant().Contains(search) ||
                        title.ToLowerInvariant().Contains(search) ||
                        url.ToLowerInvariant().Contains(search))
                        results.Add(rs);
                }
            }

            return results;
        }
    }
}
