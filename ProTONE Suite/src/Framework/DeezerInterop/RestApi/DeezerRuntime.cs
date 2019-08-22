using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net;
using OPMedia.Core.Logging;
using OPMedia.Core.NetworkAccess;

namespace OPMedia.DeezerInterop.RestApi
{
    public class DeezerRuntime
    {
        private string _apiEndpoint = "http://api.deezer.com/";

        private WebClientWithTimeout _webClient;

        public string UserAccessToken { get; set; }
        public string ApplicationId { get; set; }


        public override bool Equals(object obj)
        {
            bool bEq = false;

            DeezerRuntime dzr = obj as DeezerRuntime;
            if (dzr != null)
            {
                if (dzr.UserAccessToken != null && this.ApplicationId != null)
                {
                    bEq = (dzr.UserAccessToken == this.UserAccessToken &&
                        dzr.ApplicationId == this.ApplicationId);
                }
            }

            return bEq;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DeezerRuntime"/> class.
        /// </summary>
        public DeezerRuntime(string apiEndpoint)
        {
            _apiEndpoint = apiEndpoint;
            _webClient = new WebClientWithTimeout(30000);

            this.UserAccessToken = null;
            this.ApplicationId = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeezerRuntime"/> class.
        /// Sets the application credentials for use with needed requests.
        /// </summary>
        public DeezerRuntime(string apiEndpoint, string applicationId, string userAccessToken)
            : this(apiEndpoint)
        {
            this.ApplicationId = applicationId;
            this.UserAccessToken = userAccessToken;
        }

        internal void CheckResponseForErrors(string response)
        {
            if (string.IsNullOrEmpty(response))
                    throw new WebException("Empty response received from server.");

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
            string response = null;
            string url = _apiEndpoint + method;

            try
            {
                response = _webClient.DownloadString(url);
                CheckResponseForErrors(response);
            }
            catch (DeezerRuntimeException drex)
            {
                // Don't do anything ... mostly this would indicate that the requested objects were not found
                // on the Deezer server. This would be part of normal app operation so no need to log them.
                string msg = drex.Message;
            }
            catch (WebException wex)
            {
                // This catches web-related errors (Not Found, Unaouthorized, timeout ... etc ...)
                // Since these either indicate problems with the server or an internal app bug, we should log them
                Logger.LogWarning($"HTTP GET error: URL={url} => {wex.Message}");
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }

            return response;
        }

        internal string ExecuteHttPost(string method, string data)
        {
            string response = null;
            string url = _apiEndpoint + method;

            try
            {
                response = _webClient.UploadString(url, data);
                CheckResponseForErrors(response);
            }
            catch (DeezerRuntimeException drex)
            {
                // Don't do anything ... mostly this would indicate that the requested objects were not found
                // on the Deezer server. This would be part of normal app operation so no need to log them.
                string msg = drex.Message;
            }
            catch (WebException wex)
            {
                // This catches web-related errors (Not Found, Unaouthorized, timeout ... etc ...)
                // Since these either indicate problems with the server or an internal app bug, we should log them
                Logger.LogWarning($"HTTP POST error: URL={url} => {wex.Message}");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return response;
        }

        public Track GetTrack(UInt64 trackId)
        {
            Track track = null;

            string response = this.ExecuteHttpGet(string.Format("/track/{0}", trackId));
            if (string.IsNullOrEmpty(response) == false)
            {
                track = JsonConvert.DeserializeObject<Track>(response);
                track.CurrentRuntime = this;
            }

            return track;
        }

        public Artist GetArtist(UInt64 artistId)
        {
            Artist artist = null;

            string response = this.ExecuteHttpGet(string.Format("/artist/{0}", artistId));
            if (string.IsNullOrEmpty(response) == false)
            {
                artist = JsonConvert.DeserializeObject<Artist>(response);
                artist.CurrentRuntime = this;
            }

            return artist;
        }

        public Playlist GetPlaylist(UInt64 playlistId)
        {
            Playlist playlist = null;

            string response = this.ExecuteHttpGet(string.Format("/playlist/{0}", playlistId));
            if (string.IsNullOrEmpty(response) == false)
            {
                // There an issue with the API. We only get the first 400 tracks from the playlist
                // Working on that internally (because the /playlist/:id/tracks only has pages of 50 tracks...
                playlist = JsonConvert.DeserializeObject<Playlist>(response);

                playlist.CurrentRuntime = this;
                playlist.LoadTracks();
            }

            return playlist;
        }

        public List<Track> ExecuteSearch(string query, int limit, ManualResetEvent abortEvent)
        {
            List<Track> tracks = new List<Track>();

            if (abortEvent.WaitOne(5))
                return tracks;

            if (limit < 0)
                limit = 10000;

            string response = this.ExecuteHttpGet(string.Format("/search?limit={0}&q={1}", limit, query));
            if (string.IsNullOrEmpty(response) == false)
            {
                var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                List<Track> tracksChunk = JsonConvert.DeserializeObject<List<Track>>(jsonResult["data"].ToString());
                if (tracksChunk != null && tracksChunk.Count > 0)
                {
                    tracksChunk.ForEach(t =>
                    {
                        Track trackToAdd = t;
                        if (t.HasDetails == false)
                        {
                            Track actualTrack = this.GetTrack(t.Id);
                            if (actualTrack != null)
                                trackToAdd = actualTrack;
                        }

                        tracks.Add(trackToAdd);
                    });
                }

                //var albums = JsonConvert.DeserializeObject<List<Album>>(jsonResult["data"].ToString());
                //if (albums != null)
                //{
                //    foreach (Album album in albums)
                //    {
                //        if (abortEvent.WaitOne(5))
                //            break;

                //        album.CurrentRuntime = this;

                //        tracksChunk = album.LoadTracks();

                //        if (tracksChunk != null && tracksChunk.Count > 0)
                //            tracks.AddRange(tracksChunk);
                //    }
                //}
            }

            return tracks;
        }


        public List<Playlist> GetMyPlaylists(ManualResetEvent abortEvent)
        {
            List<Playlist> playlists = new List<Playlist>();

            if (abortEvent.WaitOne(5))
                return playlists;

            string response = this.ExecuteHttpGet(string.Format("user/me/playlists?access_token={0}", this.UserAccessToken));
            if (string.IsNullOrEmpty(response) == false)
            {
                var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                List<Playlist> playlistsChunk = JsonConvert.DeserializeObject<List<Playlist>>(jsonResult["data"].ToString());
                if (playlistsChunk != null && playlistsChunk.Count > 0)
                    playlists.AddRange(playlistsChunk);
            }

            return playlists;
        }

        public UInt64 CreatePlaylist(string playlistName, ManualResetEvent abortEvent)
        {
            if (abortEvent.WaitOne(5))
                return 0;

            string response = this.ExecuteHttPost(string.Format("user/me/playlists?access_token={0}&title={1}", this.UserAccessToken, playlistName), string.Empty);
            if (string.IsNullOrEmpty(response) == false)
            {
                var p = JsonConvert.DeserializeObject<Playlist>(response);
                if (p != null)
                    return p.Id;
            }

            return 0;
        }

        public bool AddToPlaylist(UInt64 playlistId, string tracks, ManualResetEvent abortEvent)
        {
            if (abortEvent.WaitOne(5))
                return false;

            string response = this.ExecuteHttPost(string.Format("playlist/{0}/tracks?access_token={1}&songs={2}", playlistId, this.UserAccessToken, tracks), string.Empty);
            if (string.IsNullOrEmpty(response) == false)
            {
                return bool.Parse(response);
            }

            return false;
        }
    }
}
