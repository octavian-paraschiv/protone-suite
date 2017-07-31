using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    [Flags]
    public enum OnlineMediaSource
    {
        None = 0x00,

        Internal = 0x01,
        ShoutCast = 0x02,
        Deezer = 0x04,

        All = 0xFF,
    }

    public interface IOnlineMediaItem
    {
        string Url { get; set; }

        string Title { get; set; }

        OnlineMediaSource Source { get; set; }
    }


}
