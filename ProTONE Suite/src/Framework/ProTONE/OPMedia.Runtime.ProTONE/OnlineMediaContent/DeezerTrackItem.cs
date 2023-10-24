using OPMedia.Core.Logging;
using OPMedia.Core.Utilities;
using OPMedia.DeezerInterop.RestApi;
using System;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    [Serializable]
    public class DeezerTrackItem : OnlineMediaItem
    {
        public const string DeezerTrackUrlBase = "dzmedia:///track/";
        public const string DeezerTrackUrlFmt = DeezerTrackUrlBase + "{0}";

        public DeezerTrackItem()
        {
            this.Source = OnlineMediaSource.Deezer;
        }

        public override string ToString()
        {
            //return string.Format("{0} - {1} [{2}]", Artist, Title, Url);
            return string.Format("{0} - {1}", Artist, Title);
        }

        public void LoadTrackData(Track t)
        {
            if (t != null)
            {
                this.Album = (t.Album != null) ? t.Album.Title : String.Empty;
                this.Artist = (t.Artist != null) ? t.Artist.Name : string.Empty;
                this.Title = t.Title ?? string.Empty;
                this.Duration = t.Duration;
                this.ReleaseDate = t.ReleaseDate;

                if (t.Album != null)
                {
                    string uri = t.Album.ImageUriSmall.ToUrl();
                    if (string.IsNullOrEmpty(uri) == false)
                        this.AlbumUriImageSmall = uri;

                    uri = t.Album.ImageUriLarge.ToUrl();
                    if (string.IsNullOrEmpty(uri) == false)
                        this.AlbumUriImageLarge = uri;

                    if (string.IsNullOrEmpty(this.ReleaseDate))
                        this.ReleaseDate = t.Album.ReleaseDate;
                }

                if (t.Artist != null)
                {
                    string uri = t.Artist.ImageUriSmall.ToUrl();
                    if (string.IsNullOrEmpty(uri) == false)
                        this.ArtistUriImageSmall = uri;

                    uri = t.Artist.ImageUriLarge.ToUrl();
                    if (string.IsNullOrEmpty(uri) == false)
                        this.ArtistUriImageLarge = uri;
                }

                this.Url = string.Format(DeezerTrackItem.DeezerTrackUrlFmt, t.Id);
            }
        }

        internal void Rebuild()
        {
            try
            {
                ulong trackId = 0;
                string track = this.Url.ToLowerInvariant().Replace(DeezerTrackUrlBase, string.Empty);

                if (ulong.TryParse(track, out trackId))
                {
                    DeezerRuntime dzr = DeezerRuntimeFactory.GetRuntime();
                    if (dzr != null)
                    {
                        Track t = dzr.GetTrack(trackId);
                        LoadTrackData(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
