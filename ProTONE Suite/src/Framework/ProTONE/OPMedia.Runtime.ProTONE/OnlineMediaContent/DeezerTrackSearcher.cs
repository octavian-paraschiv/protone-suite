﻿using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.RestApi;
using OPMedia.Runtime.ProTONE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public class DeezerTrackSearcher : OnlineContentSearcher
    {
        private static DeezerRuntime _dzr = null;

        protected override bool HasValidConfig
        {
            get
            {
                string userAccessToken = ProTONEConfig.DeezerUserAccessToken;
                string applicationId = ProTONEConfig.DeezerApplicationId;

                return (string.IsNullOrEmpty(userAccessToken) == false &&
                    string.IsNullOrEmpty(applicationId) == false);
            }
        }

        protected override List<IOnlineMediaItem> Search(string search, ManualResetEvent abortEvent)
        {
            List<IOnlineMediaItem> results = new List<IOnlineMediaItem>();

            if (abortEvent.WaitOne(5))
                return results;

            if (HasValidConfig)
            {
                string userAccessToken = ProTONEConfig.DeezerUserAccessToken;
                string applicationId = ProTONEConfig.DeezerApplicationId;

                DeezerRuntime dzr = new DeezerRuntime(userAccessToken, applicationId);
                if (_dzr != dzr)
                    _dzr = dzr;

                List<Track> tracks = _dzr.ExecuteSearch(search, abortEvent);

                if (tracks != null)
                {
                    foreach (var t in tracks)
                    {
                        try
                        {
                            DeezerTrackItem dti = new DeezerTrackItem
                            {
                                Album = (t.Album != null) ? t.Album.Title : String.Empty,
                                Artist = (t.Artist != null) ? t.Artist.Name : string.Empty,
                                Title = t.Title ?? string.Empty,
                                Url = string.Format("dzmedia:///track/{0}", t.Id),
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

            return results;
        }
    }
}
