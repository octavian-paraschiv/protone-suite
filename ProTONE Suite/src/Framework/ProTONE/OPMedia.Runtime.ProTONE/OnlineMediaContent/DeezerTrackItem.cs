using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public class DeezerTrackItem : IOnlineMediaItem
    {
        public OnlineMediaSource Source { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public TimeSpan Duration { get; set; }

        public DeezerTrackItem()
        {
            this.Source = OnlineMediaSource.Deezer;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} [{2}]", Artist, Title, Url);
        }
    }
}
