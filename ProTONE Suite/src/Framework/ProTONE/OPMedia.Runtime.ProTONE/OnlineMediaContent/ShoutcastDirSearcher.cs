﻿using Newtonsoft.Json.Linq;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public class ShoutcastDirSearcher : OnlineContentSearcher
    {
        const int MaxSearchCount = 100;

        protected override bool HasValidConfig
        {
            get
            {
                string shoutCastdevId = ProTONEConfig.ShoutCastApiDevID;
                string shoutCastSearchBaseUrl = ProTONEConfig.ShoutCastSearchBaseURL;
                string shoutCastTuneInBaseUrl = ProTONEConfig.ShoutCastTuneInBaseURL;

                return (string.IsNullOrEmpty(shoutCastdevId) == false &&
                    string.IsNullOrEmpty(shoutCastSearchBaseUrl) == false &&
                    string.IsNullOrEmpty(shoutCastTuneInBaseUrl) == false);
            }
        }

        protected override List<IOnlineMediaItem> Search(string search, ManualResetEvent abortEvent)
        {
            List<IOnlineMediaItem> results = new List<IOnlineMediaItem>();

            if (abortEvent.WaitOne(5))
                return results;

            if (HasValidConfig)
            {
                string shoutCastdevId = ProTONEConfig.ShoutCastApiDevID;
                string shoutCastSearchBaseUrl = ProTONEConfig.ShoutCastSearchBaseURL;
                string shoutCastTuneInBaseUrl = ProTONEConfig.ShoutCastTuneInBaseURL;

                // ShoutCast API DevID available, try to get stations list from ShoutCast Directory
                string searchUrl = string.Format("{0}/advancedsearch?mt=audio/mpeg&f=json&limit={1}&k={2}",
                        shoutCastSearchBaseUrl, MaxSearchCount, shoutCastdevId);

                if (string.IsNullOrEmpty(search) == false)
                {
                    if (search.ToLowerInvariant().StartsWith("now:"))
                    {
                        // search by "now playing"
                        search = search.ToLowerInvariant().Replace("now:", "").Trim();
                        searchUrl = string.Format("{0}/nowplaying?mt=audio/mpeg&f=json&limit={1}&k={2}&ct={3}",
                        shoutCastSearchBaseUrl, MaxSearchCount, shoutCastdevId,
                        StringUtils.UrlEncode(search));
                    }
                    else
                    {
                        // generic search
                        searchUrl += string.Format("&search={0}", StringUtils.UrlEncode(search));
                    }
                }

                if (abortEvent.WaitOne(5))
                    return results;

                using (WebClient wc = new WebClient())
                {
                    string jsonReply = wc.DownloadString(searchUrl);

                    if (abortEvent.WaitOne(5))
                        return results;

                    dynamic obj2 = JObject.Parse(jsonReply);

                    if (abortEvent.WaitOne(5))
                        return results;

                    var response = obj2.response;
                    if (response != null && response.statusCode == "200")
                    {
                        if (abortEvent.WaitOne(5))
                            return results;

                        if (response.data != null &&
                            response.data.stationlist != null)
                        {
                            if (abortEvent.WaitOne(5))
                                return results;

                            var stations = response.data.stationlist.station;
                            if (stations != null)
                            {
                                if (abortEvent.WaitOne(5))
                                    return results;

                                string tuneInBase = string.Empty;
                                try
                                {
                                    tuneInBase = response.data.stationlist.tunein["base"] as string;
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogException(ex);
                                    tuneInBase = null;
                                }

                                if (abortEvent.WaitOne(5))
                                    return results;

                                if (string.IsNullOrEmpty(tuneInBase))
                                    tuneInBase = "/sbin/tunein-station.pls";

                                for (int i = 0; i < stations.Count; i++)
                                {
                                    if (abortEvent.WaitOne(5))
                                        return results;

                                    var station = stations[i];

                                    try
                                    {
                                        RadioStation rs = new RadioStation(OnlineMediaSource.ShoutCast);
                                        rs.Title = station.name;
                                        rs.Type = station.mt;
                                        rs.Genre = station.genre;
                                        rs.Bitrate = station.br;

                                        string ct = "";
                                        string cst = "";

                                        try { ct = station.ct; }
                                        catch { }
                                        try { cst = station.cst; }
                                        catch { }

                                        if (string.IsNullOrEmpty(ct) == false)
                                            rs.Content = ct;
                                        else if (string.IsNullOrEmpty(cst) == false)
                                            rs.Content = cst;
                                        else
                                            rs.Content = Translator.Translate("TXT_NA");

                                        int stationId = station.id;

                                        string tuneInUrl = string.Format("{0}/{1}?id={2}",
                                            shoutCastTuneInBaseUrl, tuneInBase, stationId);

                                        string reply = wc.DownloadString(tuneInUrl);

                                        string stationUrl = ParseReply(reply);
                                        if (string.IsNullOrEmpty(stationUrl))
                                            continue;

                                        rs.Url = stationUrl;

                                        if (rs.Title.ToLowerInvariant().Contains("radionomy") ||
                                            rs.Url.ToLowerInvariant().Contains("radionomy"))
                                            continue;

                                        results.Add(rs);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.LogException(ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }

        private string ParseReply(string reply)
        {
            string url = string.Empty;

            if (string.IsNullOrEmpty(reply) == false)
            {
                string[] lines = reply.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (lines != null)
                {
                    foreach (string line in lines)
                    {
                        string[] fields = line.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (fields != null && fields.Length > 1)
                        {
                            string name = fields[0];
                            string value = fields[1];

                            if (name.ToLowerInvariant().StartsWith("file"))
                            {
                                try
                                {
                                    Uri uri = new Uri(value);
                                    url = uri.ToString();
                                }
                                catch { }

                                break;
                            }
                        }
                    }
                }
            }

            return url;
        }
    }
}