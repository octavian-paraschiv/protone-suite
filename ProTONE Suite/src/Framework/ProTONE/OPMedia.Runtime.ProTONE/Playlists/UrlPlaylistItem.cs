namespace OPMedia.Runtime.ProTONE.Playlists
{
    public class UrlPlaylistItem : PlaylistItem
    {
        protected override bool _SupportsBookmarkInfo()
        {
            return false;
        }

        protected override bool _SupportsTrackInfo()
        {
            return false;
        }

        protected override bool _IsTrackInfoEditable()
        {
            return false;
        }

        protected override bool _IsBookmarkInfoEditable()
        {
            return false;
        }

        public UrlPlaylistItem(string uri) :
            base(uri, false, false)
        {

        }
    }
}
