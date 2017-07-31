using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.Playlists
{
    public class DeezerTrackPlaylistItem : PlaylistItem
    {
        protected override bool _IsBookmarkInfoEditable()
        {
            return false;
        }

        protected override bool _IsTrackInfoEditable()
        {
            return false;
        }

        protected override bool _SupportsBookmarkInfo()
        {
            return false;
        }

        protected override bool _SupportsTrackInfo()
        {
            return false;
        }

        DeezerTrackItem _dti = null;

        public override string Type
        {
            get
            {
                return "deezer";
            }
        }

        public override string DisplayName
        {
            get
            {
                return _dti.ToString();
            }
        }

        public override string Details
        {
            get
            {
                return _dti.Url;
            }
        }

        public override string PersistentPlaylistName
        {
            get
            {
                return string.Format("{0}`{1}`{2}", _dti.Artist, _dti.Title, _dti.Album);
            }
        }

        public override TimeSpan Duration
        {
            get
            {
                return _dti.Duration;
            }
        }

        public DeezerTrackPlaylistItem(DeezerTrackItem dti) : 
            base(dti.Url, false, false)
        {
            _dti = dti;
        }
    }
}
