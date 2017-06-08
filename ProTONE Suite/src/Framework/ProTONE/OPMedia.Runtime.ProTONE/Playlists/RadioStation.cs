using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OPMedia.Core;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using OPMedia.Core.Utilities;
using System.Net;
using Newtonsoft.Json.Linq;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Core.Logging;
using System.Web;
using OPMedia.Core.TranslationSupport;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.Playlists
{
    public enum RadioStationSource
    {
        Internal = 0,
        ShoutCast,
    }

    [DataContract]
    public class RadioStation
    {
        [DataMember(Order = 0)]
        public string Url { get; set; }

        [DataMember(Order = 1)]
        public string Title { get; set; }

        [DataMember(Order = 2)]
        public string Genre { get; set; }

        [DataMember(Order = 3)]
        public string Type { get; set; }

        [DataMember(Order = 4)]
        public RadioStationSource Source { get; set; }

        [DataMember(Order = 5)]
        public int Bitrate { get; set; }

        [DataMember(Order = 5)]
        public string Content { get; set; }

        private bool _isFake = false;

        public bool IsFake
        {
            get
            {
                return _isFake;
            }
        }

        public static RadioStation Empty
        {
            get
            {
                RadioStation rs = new RadioStation();
                rs.Title = "TXT_NO_STATIONS_LOADED";
                return rs;
            }
        }

        public static RadioStation NotFound
        {
            get
            {
                RadioStation rs = new RadioStation();
                rs.Title = "TXT_NO_STATIONS_FOUND";
                return rs;
            }
        }

        public RadioStation()
        {
            this.Source = RadioStationSource.Internal;
            _isFake = true;
        }

        public RadioStation(RadioStationSource source)
        {
            this.Source = source;
            _isFake = true;
        }
    }

    [DataContract]
    public class RadioStationsData
    {
        [DataMember(Order = 0)]
        public List<RadioStation> RadioStations { get; set; }

        public RadioStationsData()
        {
            RadioStations = new List<RadioStation>();
        }

        public static RadioStationsData GetDefault()
        {
            RadioStationsData rsd = new RadioStationsData();

            rsd.RadioStations.Add(RadioStation.Empty);

            return rsd;
        }

        public static RadioStationsData Search(ref ManualResetEvent abortEvent, string searchKeyword, int maxCount = 20, RadioStation additionalParams = null)
        {
            RadioStationsData rsd = new RadioStationsData();

            try
            {
                if (abortEvent.WaitOne(5))
                    goto Exit_Search;

                string shoutCastdevId = ProTONEConfig.ShoutCastApiDevID;
                string shoutCastSearchBaseUrl = ProTONEConfig.ShoutCastSearchBaseURL;
                string shoutCastTuneInBaseUrl = ProTONEConfig.ShoutCastTuneInBaseURL;
                bool doShoutcastLookup = false;

                if (string.IsNullOrEmpty(shoutCastdevId) == false &&
                    string.IsNullOrEmpty(shoutCastSearchBaseUrl) == false &&
                    string.IsNullOrEmpty(shoutCastTuneInBaseUrl) == false)
                {
                    doShoutcastLookup = true;

                    // ShoutCast API DevID available, try to get stations list from ShoutCast Directory
                    string searchUrl = string.Format("{0}/advancedsearch?mt=audio/mpeg&f=json&limit={1}&k={2}",
                            shoutCastSearchBaseUrl, maxCount, shoutCastdevId);

                    if (string.IsNullOrEmpty(searchKeyword) == false)
                    {
                        if (searchKeyword.ToLowerInvariant().StartsWith("now:"))
                        {
                            // search by "now playing"
                            searchKeyword = searchKeyword.ToLowerInvariant().Replace("now:", "").Trim();
                            searchUrl = string.Format("{0}/nowplaying?mt=audio/mpeg&f=json&limit={1}&k={2}&ct={3}",
                            shoutCastSearchBaseUrl, maxCount, shoutCastdevId,
                            StringUtils.UrlEncode(searchKeyword));
                        }
                        else
                        {
                            // generic search
                            searchUrl += string.Format("&search={0}", StringUtils.UrlEncode(searchKeyword));
                        }
                    }

                    if (abortEvent.WaitOne(5))
                        goto Exit_Search;

                    using (WebClient wc = new WebClient())
                    {
                        string jsonReply = wc.DownloadString(searchUrl);

                        if (abortEvent.WaitOne(5))
                            goto Exit_Search;

                        dynamic obj2 = JObject.Parse(jsonReply);

                        if (abortEvent.WaitOne(5))
                            goto Exit_Search;

                        var response = obj2.response;
                        if (response != null && response.statusCode == "200")
                        {
                            if (abortEvent.WaitOne(5))
                                goto Exit_Search;

                            if (response.data != null &&
                                response.data.stationlist != null)
                            {
                                if (abortEvent.WaitOne(5))
                                    goto Exit_Search;

                                var stations = response.data.stationlist.station;
                                if (stations != null)
                                {
                                    if (abortEvent.WaitOne(5))
                                        goto Exit_Search;

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
                                        goto Exit_Search;

                                    if (string.IsNullOrEmpty(tuneInBase))
                                        tuneInBase = "/sbin/tunein-station.pls";

                                    for (int i = 0; i < stations.Count; i++)
                                    {
                                        if (abortEvent.WaitOne(5))
                                            goto Exit_Search;

                                        var station = stations[i];

                                        try
                                        {
                                            RadioStation rs = new RadioStation(RadioStationSource.ShoutCast);
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

                                            rsd.RadioStations.Add(rs);
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

                if (abortEvent.WaitOne(5))
                    goto Exit_Search;

                if (doShoutcastLookup == false)
                {
                    RadioStationsData internalDatabase = null;

                    // Fill with Internal stations list from Persistence Service
                    string xml = PersistenceProxy.ReadObject("RadioStationsData", string.Empty, false);
                    if (!string.IsNullOrEmpty(xml))
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        using (StringReader sr = new StringReader(xml))
                        using (XmlReader xr = XmlReader.Create(sr))
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(RadioStationsData));
                            internalDatabase = xs.Deserialize(xr) as RadioStationsData;
                        }
                    }

                    if (abortEvent.WaitOne(5))
                        goto Exit_Search;

                    if (internalDatabase != null &&
                        internalDatabase.RadioStations != null &&
                        internalDatabase.RadioStations.Count > 0)
                    {
                        rsd.RadioStations.AddRange(internalDatabase.RadioStations);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

Exit_Search:
            if (rsd.RadioStations.Count < 1)
                rsd.RadioStations.Add(RadioStation.NotFound);

            return rsd;
        }

        private static string ParseReply(string reply)
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

        /*
        public void SavePersistentList()
        {
            StringBuilder xml = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineHandling = NewLineHandling.Entitize;
            settings.OmitXmlDeclaration = true;
            settings.NewLineChars = "\r\n";
            settings.Indent = true;
            settings.IndentChars = " ";
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.CloseOutput = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

            using (XmlWriter xw = XmlWriter.Create(xml, settings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer xs = new XmlSerializer(typeof(RadioStationsData));
                xs.Serialize(xw, this, ns);
            }

            PersistenceProxy.SaveObject("RadioStationsData", xml.ToValidXml());
        }
        */
    }
}
