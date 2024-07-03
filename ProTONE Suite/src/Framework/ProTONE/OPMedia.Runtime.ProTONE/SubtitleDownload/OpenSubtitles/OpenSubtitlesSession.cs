using MovieCollection.OpenSubtitles;
using MovieCollection.OpenSubtitles.Models;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.NetworkAccess;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OPMedia.Runtime.ProTONE.SubtitleDownload.OpenSubtitles
{
    internal class OpenSubtitlesSession : SubtitleServerSession
    {
        const string ApiKey = "BoBk2S2HpzOShTMn7SASl7NQOXG5U3Vd";

        private HttpClient _cl;
        private OpenSubtitlesService _svc;
        private Login _login;


        protected override bool AuthenticationRequired => true;

        public OpenSubtitlesSession(string serverUrl, string username, string password, CultureInfo culture)
           : base(serverUrl, username, password, culture)
        {

        }

        protected override void InitializeSession()
        {
            _cl = new HttpClient();
            _cl.Timeout = TimeSpan.FromMilliseconds(Timeout);

            _svc = new OpenSubtitlesService(_cl, new OpenSubtitlesOptions
            {
                ApiKey = ApiKey,
                ProductInformation = new ProductHeaderValue("ProTONE", SuiteVersion.Version)
            });
        }

        protected override void Authenticate()
        {
            if (_username?.Length > 0)
            {
                var login = _svc.LoginAsync(new MovieCollection.OpenSubtitles.Models.NewLogin
                {
                    Username = _username,
                    Password = _password,

                }).GetAwaiter().GetResult();

                if (login?.Status == 200 && _login?.Token?.Length > 0)
                    _login = login;
            }
        }

        protected override void CleanupSession()
        {
            if (_login?.Token?.Length > 0)
            {
                var rsp = _svc.LogoutAsync(_login.Token).GetAwaiter().GetResult();
                if (rsp?.Status == 200)
                    _login = null;
            }

            _svc = null;

            _cl?.Dispose();
            _cl = null;
        }

        protected override void TestConnection()
        {
            _ = _svc.GetSubtitleFormatsAsync().GetAwaiter().GetResult();
        }

        protected override List<SubtitleInfo> GetSubtitles(VideoInfo vi)
        {
            List<SubtitleInfo> retVal = new List<SubtitleInfo>();

            List<AttributeResult<Subtitle>> retSubs = new List<AttributeResult<Subtitle>>();

            int totalPages = int.MaxValue;

            for (int page = 0; page < totalPages; page++)
            {
                try
                {
                    var rsp = _svc.SearchSubtitlesAsync(new NewSubtitleSearch
                    {
                        Query = vi.moviename,
                        MovieHash = vi.moviehash,
                        Page = page,

                    }).GetAwaiter().GetResult();

                    if (rsp == null)
                        break;

                    if (rsp?.TotalPages <= 0)
                        break;

                    totalPages = rsp.TotalPages;

                    if (rsp?.Data?.Count > 0)
                    {
                        retSubs.AddRange(rsp.Data);
                        continue;
                    }

                    break;
                }
                catch
                {
                    break;
                }
            }


            if (retSubs.Count > 0)
            {
                var infos = from sd in retSubs
                            where
                            (
                                //sd?.movieName?.Length > 0 &&
                                //sd?.movieHash?.Length > 0 &&

                                (string.IsNullOrEmpty(sd.Attributes?.Fps) || vi.framerate == StringUtils.CastAs<double>(sd.Attributes?.Fps ?? "0", 0).ToString("##.###")) &&
                                sd?.Attributes?.MovieHashMatch == true &&
                                sd?.Attributes?.Language?.Length > 0 &&
                                sd?.Attributes?.Url?.ToString()?.Length > 0
                            )
                            select new SubtitleInfo
                            {
                                IDSubtitle = (sd.Attributes.LegacySubtitleId ?? sd.Attributes.SubtitleId),
                                IDSubtitleFile = (sd.Attributes.Files?.FirstOrDefault()?.FileId).GetValueOrDefault(),
                                SubFileName = sd.Attributes.Files?.FirstOrDefault()?.FileName,
                                MovieName = sd.Attributes.FeatureDetails?.MovieName,

                                LanguageName = LanguageHelper.Lookup(sd.Attributes.Language)?.DisplayName() ?? sd.Attributes.Language,
                                ISO639 = LanguageHelper.Lookup(sd.Attributes.Language)?.Part3 ?? sd.Attributes.Language,

                                MovieHash = vi.moviehash, // Ideally we should get this from the response ...

                                SubDownloadLink = sd.Attributes.Url.ToString(),
                                // SubHash = sd.Attributes.,
                                SubFormat = StringUtils.FirstNonEmpty(sd.Attributes.Format,
                                    PathUtils.GetExtension(sd.Attributes.Files?.FirstOrDefault()?.FileName), "srt"),

                                FrameRate = sd.Attributes?.Fps,
                            };

                if (infos.Count() > 0)
                    retVal.AddRange(infos);
            }

            return retVal;
        }

        protected override string DownloadSubtitleInternal(string fileName, SubtitleInfo subtitle)
        {
            try
            {
                var download = _svc.GetSubtitleForDownloadAsync(new NewDownload
                {
                    FileId = subtitle.IDSubtitleFile,
                    FileName = subtitle.SubFileName,
                    SubFormat = subtitle.SubFormat,

                }, _login?.Token).GetAwaiter().GetResult();

                if (download?.Link != null)
                {
                    string destPath = Path.ChangeExtension(fileName, subtitle.SubFormat);

                    using (WebFileRetriever wfr = new WebFileRetriever(AppConfig.ProxySettings, download.Link.ToString(), destPath))
                    {
                        wfr.PerformDownload(false);
                    }
                    return destPath;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return string.Empty;
        }
    }
}
