using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.RestApi;
using OPMedia.Runtime.ProTONE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Core.Utilities;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public class DeezerJsonFilter
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("track")]
        public string Title { get; set; }


        public bool IsEmpty
        {
            get
            {
                return (this.SearchText.Length == 0);
            }
        }

        public string SearchText
        {
            get
            {
                string text = "";

                if (string.IsNullOrEmpty(Artist) == false)
                    text += $"artist:\"{Artist}\" ";

                if (string.IsNullOrEmpty(Album) == false)
                    text += $"album:\"{Album}\" ";

                if (string.IsNullOrEmpty(Title) == false)
                    text += $"track:\"{Title}\" ";

                return text.Trim();
            }
        }

        public static DeezerJsonFilter FromSearchText(string query)
        {
            try
            {
                // A filter string was specified by the user, so try to parse its fields ...
                // Instead of old-school parse using string.Split or regex, we use JSON, it is cooler :)
                string jsonSearchBody = query;

                jsonSearchBody = jsonSearchBody.Replace("artist:", ",\"artist\":");
                jsonSearchBody = jsonSearchBody.Replace("album:", ",\"album\":");
                jsonSearchBody = jsonSearchBody.Replace("track:", ",\"track\":");
                jsonSearchBody = jsonSearchBody.Trim(",".ToCharArray());

                string jsonSearch = string.Format("{{{0}}}", jsonSearchBody);

                return JsonConvert.DeserializeObject<DeezerJsonFilter>(jsonSearch);
            }
            catch { }

            return null;
        }

        public static bool IsNullOrEmpty(DeezerJsonFilter flt)
        {
            return (flt == null || flt.IsEmpty);
        }
    }

    public class DeezerTrackSearcher : OnlineContentSearcher
    {
        protected override bool HasValidConfig
        {
            get
            {
                return ProTONEConfig.DeezerHasValidConfig;
            }
        }

        protected override List<OnlineMediaItem> Search(OnlineContentSearchParameters searchParams, ManualResetEvent abortEvent)
        {
            List<OnlineMediaItem> results = new List<OnlineMediaItem>();

            if (abortEvent.WaitOne(5))
                return results;

            if (HasValidConfig)
            {
                DeezerRuntime dzr = DeezerRuntimeFactory.GetRuntime();

                searchParams.Filter = OnlineContentSearchFilter.None;

                bool isFilteredSearch = false;

                isFilteredSearch |= searchParams.SearchText.Contains("artist:");
                isFilteredSearch |= searchParams.SearchText.Contains("track:");
                isFilteredSearch |= searchParams.SearchText.Contains("album:");

                DeezerJsonFilter filter = null;

                if (isFilteredSearch)
                    filter = DeezerJsonFilter.FromSearchText(searchParams.SearchText);

                List<Track> tracks = dzr.ExecuteSearch(searchParams.SearchText, -1, abortEvent);

                if (tracks != null)
                {
                    foreach (var t in tracks)
                    {
                        try
                        {
                            DeezerTrackItem dti = new DeezerTrackItem();
                            dti.LoadTrackData(t);

                            bool shouldAddTrack = true;

                            if (DeezerJsonFilter.IsNullOrEmpty(filter) == false)
                            {
                                if (shouldAddTrack && string.IsNullOrEmpty(filter.Artist) == false)
                                    shouldAddTrack &= dti.Artist.ToLowerInvariant().Contains(filter.Artist);

                                if (shouldAddTrack && string.IsNullOrEmpty(filter.Album) == false)
                                    shouldAddTrack &= dti.Album.ToLowerInvariant().Contains(filter.Album);

                                if (shouldAddTrack && string.IsNullOrEmpty(filter.Title) == false)
                                    shouldAddTrack &= dti.Title.ToLowerInvariant().Contains(filter.Title);
                            }

                            if (shouldAddTrack)
                                results.Add(dti);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                        
                    }
                }
            }

            if (abortEvent.WaitOne(5))
                return results;

            SortResults(ref results);

            return results;
        }


        protected override List<OnlinePlaylist> GetMyPlaylists(ManualResetEvent abortEvent)
        {
            List<OnlinePlaylist> results = new List<OnlinePlaylist>();

            if (abortEvent.WaitOne(5))
                return results;

            if (HasValidConfig)
            {
                DeezerRuntime dzr = DeezerRuntimeFactory.GetRuntime();

                List<Playlist> playlists = dzr.GetMyPlaylists(abortEvent);
                if (playlists != null)
                {
                    foreach (var p in playlists)
                    {
                        OnlinePlaylist op = new OnlinePlaylist
                        {
                            Id = p.Id,
                            Description = p.Description,
                            Title = p.Title
                        };

                        results.Add(op);
                    }
                }
            }

            return results;
        }

        protected override List<OnlineMediaItem> ExpandOnlinePlaylist(OnlinePlaylist op, ManualResetEvent abortEvent)
        {
            List<OnlineMediaItem> results = new List<OnlineMediaItem>();

            if (abortEvent.WaitOne(5))
                return results;

            if (HasValidConfig)
            {
                DeezerRuntime dzr = DeezerRuntimeFactory.GetRuntime();

                Playlist p = dzr.GetPlaylist(op.Id);
                if (p != null && p.Tracks != null)
                {
                    foreach (var t in p.Tracks)
                    {
                        try
                        {
                            DeezerTrackItem dti = new DeezerTrackItem
                            {
                                Album = (t.Album != null) ? t.Album.Title : String.Empty,
                                Artist = (t.Artist != null) ? t.Artist.Name : string.Empty,
                                Title = t.Title ?? string.Empty,
                                Url = string.Format(DeezerTrackItem.DeezerTrackUrlFmt, t.Id),
                                Duration = t.Duration
                            };

                            results.Add(dti);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }

                    }
                }
            }

            if (abortEvent.WaitOne(5))
                return results;

            SortResults(ref results);

            return results;
        }

        private void SortResults(ref List<OnlineMediaItem> results)
        {
            results.Sort((r1, r2) =>
            {
                int cmp = 0;

                DeezerTrackItem dti1 = r1 as DeezerTrackItem;
                DeezerTrackItem dti2 = r2 as DeezerTrackItem;
                if (dti1 != null && dti2 != null)
                {
                    cmp = string.Compare(dti1.Artist, dti2.Artist, true);
                    if (cmp != 0)
                        return cmp;
                    cmp = string.Compare(dti1.Album, dti2.Album, true);
                    if (cmp != 0)
                        return cmp;
                }

                cmp = string.Compare(r1.Url, r2.Url, true);
                if (cmp != 0)
                    return cmp;

                cmp = string.Compare(r1.Title, r2.Title, true);
                if (cmp != 0)
                    return cmp;

                return 0;
            });
        }
    }
}
