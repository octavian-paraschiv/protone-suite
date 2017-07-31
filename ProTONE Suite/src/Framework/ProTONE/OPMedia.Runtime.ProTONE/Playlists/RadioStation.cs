using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OPMedia.Core;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using OPMedia.Core.Utilities;
using System.Net;
using Newtonsoft.Json.Linq;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Core.Logging;
using System.Web;
using OPMedia.Core.TranslationSupport;
using System.Threading;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;

namespace OPMedia.Runtime.ProTONE.Playlists
{
    [DataContract]
    public class RadioStation : IOnlineMediaItem
    {
        [DataMember(Order = 0)]
        public string Url { get; set; }

        [DataMember(Order = 1)]
        public string Title { get; set; }

        [DataMember(Order = 2)]
        public string Genre { get; set; }

        [DataMember(Order = 3)]
        public string Type { get; set; }

        [DataMember(Order = 4)]
        public OnlineMediaSource Source { get; set; }

        [DataMember(Order = 5)]
        public int Bitrate { get; set; }

        [DataMember(Order = 5)]
        public string Content { get; set; }

        private bool _isFake = false;

        public bool IsFake
        {
            get
            {
                return _isFake;
            }
        }

        public static RadioStation Empty
        {
            get
            {
                RadioStation rs = new RadioStation(true);
                rs.Title = "TXT_NO_STATIONS_LOADED";
                return rs;
            }
        }

        public static RadioStation NotFound
        {
            get
            {
                RadioStation rs = new RadioStation(true);
                rs.Title = "TXT_NO_STATIONS_FOUND";
                return rs;
            }
        }

        public RadioStation(bool isFake)
        {
            this.Source = OnlineMediaSource.Internal;
            _isFake = isFake;
        }

        public RadioStation()
        {
            this.Source = OnlineMediaSource.Internal;
            _isFake = false;
        }

        public RadioStation(OnlineMediaSource source)
        {
            this.Source = source;
            _isFake = false;
        }
    }

    public class SearchParams
    {
        public string Text { get; set; }
        public OnlineMediaSource Source { get; set; }
        public int MaxResults { get; set; }
    }


    [DataContract]
    public class RadioStationsData
    {
        [DataMember(Order = 0)]
        public List<RadioStation> RadioStations { get; set; }

        public RadioStationsData()
        {
            RadioStations = new List<RadioStation>();
        }

        public static RadioStationsData GetDefault()
        {
            RadioStationsData rsd = new RadioStationsData();

            rsd.RadioStations.Add(RadioStation.Empty);

            return rsd;
        }
    }
}
