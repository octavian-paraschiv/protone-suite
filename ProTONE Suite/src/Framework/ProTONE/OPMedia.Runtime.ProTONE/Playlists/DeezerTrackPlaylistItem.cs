using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core.Utilities;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.IO;
using OPMedia.Core.NetworkAccess;
using OPMedia.Core.Logging;
using Newtonsoft.Json;

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
                return "URL";
            }
        }

        public override string SubType
        {
            get
            {
                return "Deezer";
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
                //return string.Format("{0}`{1}`{2}`{3}`{4}`{5}`{6}`{7}", 
                //    _dti.Artist ?? "", 
                //    _dti.Title ?? "", 
                //    _dti.Album ?? "",
                //    _dti.ReleaseDate ?? "",
                //    _dti.AlbumUriImageSmall ?? "",
                //    _dti.AlbumUriImageLarge ?? "",
                //    _dti.ArtistUriImageSmall ?? "",
                //    _dti.ArtistUriImageLarge ?? "");

                return JsonConvert.SerializeObject(_dti);
            }
        }

        public override TimeSpan Duration
        {
            get
            {
                return _dti.Duration;
            }
        }

        public string AlbumUriImageSmall
        {
            get
            {
                return _dti.AlbumUriImageSmall;
            }
        }

        public string AlbumUriImageLarge
        {
            get
            {
                return _dti.AlbumUriImageLarge;
            }
        }

        public string ArtistUriImageSmall
        {
            get
            {
                return _dti.ArtistUriImageSmall;
            }
        }


        public string ArtistUriImageLarge
        {
            get
            {
                return _dti.ArtistUriImageLarge;
            }
        }

        private Image _customImage = null;

        public override Image CustomImage
        {
            get
            {
                if (_customImage == null)
                {
                    try
                    {
                        string url = AlbumUriImageSmall;
                        if (string.IsNullOrEmpty(url))
                            url = ArtistUriImageSmall;

                        if (string.IsNullOrEmpty(url) == false)
                        {
                            using (WebClientWithTimeout wc = new WebClientWithTimeout(1000))
                            {
                                byte[] data = wc.DownloadData(url);
                                if (data != null && data.Length > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream(data))
                                    {
                                        _customImage = Image.FromStream(ms);
                                    }
                                }

                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.LogException(ex);
                        _customImage = null;
                    }
                }

                return _customImage;
            }
        }

        public override Dictionary<string, string> MediaInfo
        {
            get            
            {
                Dictionary<string, string> info = base.MediaInfo;

                if (_dti != null)
                {
                    // add specific details for DeezerTrackItem
                    info.Add("TXT_ARTIST:", _dti.Artist);
                    info.Add("TXT_TITLE:", _dti.Title);
                    info.Add("TXT_ALBUM:", _dti.Album);

                    try
                    {
                        if (string.IsNullOrEmpty(_dti.ReleaseDate) == false)
                        {
                            DateTimeConverter c = new DateTimeConverter();
                            DateTime dt = (DateTime)c.ConvertFromInvariantString(_dti.ReleaseDate);
                            info.Add("TXT_YEAR:", dt.Year.ToString());
                        }
                    }
                    catch { }

                    info.Add("TXT_DURATION:", _dti.Duration.ToString());
                }

                return info;
            }
        }

        public DeezerTrackPlaylistItem(DeezerTrackItem dti) : 
            base(dti.Url, false, false)
        {
            _dti = dti;
        }
    }
}
