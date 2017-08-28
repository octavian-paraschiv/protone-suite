using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net;
using OPMedia.Core.Logging;

namespace OPMedia.DeezerInterop.RestApi
{
    public class DeezerRuntime
    {
        private const string API_ENDPOINT = "http://api.deezer.com/";

        private WebClient _webClient;

        private Tuple<string, string> _applicationCredentials;

        public override bool Equals(object obj)
        {
            bool bEq = false;

            DeezerRuntime dzr = obj as DeezerRuntime;
            if (dzr != null)
            {
                if (dzr._applicationCredentials != null && this._applicationCredentials != null)
                {
                    bEq = (dzr._applicationCredentials.Item1 == this._applicationCredentials.Item1 &&
                        dzr._applicationCredentials.Item2 == this._applicationCredentials.Item2);
                }
            }

            return bEq;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DeezerRuntime"/> class.
        /// </summary>
        public DeezerRuntime()
        {
            _applicationCredentials = null;
            _webClient = new WebClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeezerRuntime"/> class.
        /// Sets the application credentials for use with needed requests.
        /// </summary>
        /// <param name="applicationId">Your Application ID.</param>
        /// <param name="applicationSecretKey">Your Application secret key.</param>
        public DeezerRuntime(string applicationId, string applicationSecretKey)
        {
            _applicationCredentials = new Tuple<string, string>(applicationId, applicationSecretKey);
            _webClient = new WebClient();
        }

        public CountryInfos GetCountryInfos()
        {
            string response = _webClient.DownloadString(string.Format("{0}/infos", API_ENDPOINT));
            CheckResponseForErrors(response);
            return JsonConvert.DeserializeObject<CountryInfos>(response);
        }

        internal void CheckResponseForErrors(string response)
        {
            if (string.IsNullOrEmpty(response))
                    throw new DeezerRuntimeException("empty response from server");

            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (jsonResult.ContainsKey("error"))
            {
                ErrorInfo ei = JsonConvert.DeserializeObject<ErrorInfo>(jsonResult["error"].ToString());
                if (ErrorInfo.IsNullOrEmpty(ei) == false)
                    throw new DeezerRuntimeException(ei.ToString());
            }
        }

        internal string ExecuteHttpGet(string method)
        {
            string response = _webClient.DownloadString(API_ENDPOINT + method);
            CheckResponseForErrors(response);
            return response;
        }

        public Artist GetArtist(uint artistId)
        {
            string response = this.ExecuteHttpGet(string.Format("/artist/{0}", artistId));

            Artist artist = JsonConvert.DeserializeObject<Artist>(response);

            artist.CurrentRuntime = this;

            return artist;
        }

        public Playlist GetPlaylist(uint playlistId)
        {
            string response = this.ExecuteHttpGet(string.Format("/playlist/{0}", playlistId));

            // There an issue with the API. We only get the first 400 tracks from the playlist
            // Working on that internally (because the /playlist/:id/tracks only has pages of 50 tracks...
            Playlist playlist = JsonConvert.DeserializeObject<Playlist>(response);

            playlist.CurrentRuntime = this;
            playlist.LoadTracks();

            return playlist;
        }

        public List<Track> ExecuteSearch(string query, ManualResetEvent abortEvent)
        {
            List<Track> tracks = new List<Track>();

            if (abortEvent.WaitOne(5))
                return tracks;

            string response = this.ExecuteHttpGet(string.Format("/search?limit=1000&q={0}", query));

            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            List<Track> tracksChunk = JsonConvert.DeserializeObject<List<Track>>(jsonResult["data"].ToString());
            if (tracksChunk != null && tracksChunk.Count > 0)
                tracks.AddRange(tracksChunk);

            var albums = JsonConvert.DeserializeObject<List<Album>>(jsonResult["data"].ToString());
            if (albums != null)
            {
                foreach (Album album in albums)
                {
                    if (abortEvent.WaitOne(5))
                        break;

                    album.CurrentRuntime = this;

                    tracksChunk = album.LoadTracks();

                    if (tracksChunk != null && tracksChunk.Count > 0)
                        tracks.AddRange(tracksChunk);
                }
            }
            
            return tracks;
        }


       
    }
}
