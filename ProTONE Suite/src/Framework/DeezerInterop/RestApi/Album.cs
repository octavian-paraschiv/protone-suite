using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OPMedia.Core.Logging;

namespace OPMedia.DeezerInterop.RestApi
{

    [DebuggerDisplay("Album({Id},{Title})")]
    public class Album : DeezerEntity
    {
        public uint Id { get; set; }

        public string Title { get; set; }

        [JsonProperty("cover")]
        public Uri AlbumImageUri { get; set; }

        [JsonProperty("genre_id")]
        public MusicGenre Genre { get; set; }

        public int TracksCount { get; set; }

        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }


        public List<Track> LoadTracks()
        {
            try
            {
                string response = CurrentRuntime.ExecuteHttpGet(string.Format("/album/{0}/tracks", Id));

                var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                Tracks = JsonConvert.DeserializeObject<List<Track>>(jsonResult["data"].ToString());

                foreach (Track track in Tracks)
                {
                    track.CurrentRuntime = CurrentRuntime;
                }

            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                Tracks = new List<Track>();
            }

            TracksCount = Tracks.Count;
            return Tracks;

        }
    }
}
