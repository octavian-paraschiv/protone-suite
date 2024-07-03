using OPMedia.Core;
using OPMedia.Core.Logging;

namespace OPMedia.Runtime.ProTONE.Playlists
{
    public class PersistentPlaylist
    {
        static bool _isLoading = false;

        static PersistentPlaylist()
        {
        }

        public static void Load(ref Playlist playlist)
        {
            string persistedPlaylist = PersistenceProxy.ReadNode(true, "PersistentPlaylist", string.Empty);
            if (string.IsNullOrEmpty(persistedPlaylist) == false)
            {
                try
                {
                    _isLoading = true;
                    playlist.LoadM3UPlaylistFromString(persistedPlaylist);
                }
                finally
                {
                    _isLoading = false;
                }
            }
        }

        public static void Save(Playlist playlist)
        {
            if (_isLoading)
            {
                playlist.AbortLoad();
            }
            else
            {
                Logger.LogTrace("PersistentPlaylist::Save called to persist current playlist ...");
                string persistedPlaylist = playlist.SaveM3UPlaylistAsString();
                PersistenceProxy.SaveNode(true, "PersistentPlaylist", persistedPlaylist);
            }
        }
    }
}
