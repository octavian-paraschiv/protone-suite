using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OPMedia.DeezerInterop.RestApi
{
    [DebuggerDisplay("Artist({Id},{Name})")]
    public class Artist : DeezerEntity
    {
        private List<Album> albums;

        public UInt64 Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("link")]
        public string WebUri { get; set; }

        [JsonProperty("picture_medium")]
        public Uri ImageUriSmall { get; set; }

        [JsonProperty("picture_big")]
        public Uri ImageUriLarge { get; set; }

        [JsonProperty("nb_fan")]
        public int FansCount { get; set; }

        public int AlbumsCount { get; set; }

        [JsonProperty("radio")]
        public bool HasArtistRadio { get; set; }

        public List<Album> Albums
        {
            get
            {
                return this.albums;
            }
            set
            {
                SetProperty(ref albums, value);
            }
        }

        public Artist()
        {
            Albums = new List<Album>();
            AlbumsCount = -1;
        }

        public List<Album> LoadAlbums()
        {
            Albums = new List<Album>();

            string response = CurrentRuntime.ExecuteHttpGet(string.Format("/artist/{0}/albums", Id));
            if (string.IsNullOrEmpty(response) == false)
            {
                var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                Albums = JsonConvert.DeserializeObject<List<Album>>(jsonResult["data"].ToString());

                foreach (Album album in Albums)
                {
                    album.CurrentRuntime = CurrentRuntime;
                    album.Artist = this;
                }

                AlbumsCount = Albums.Count;
            }

            return Albums;
        }

        public override string ToString()
        {
            return string.Format("[ID={0}, Name={1}]",
                this.Id, this.Name);
        }
    }
}
