using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.NetworkAccess;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.BSP_V1;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;


namespace OPMedia.Runtime.ProTONE.SubtitleDownload.BSP_V1
{
    public class BspV1Session : SubtitleServerSession
    {
        BSPSubtitlesService _wsdl = null;

        #region Construction / Cleanup
        public BspV1Session(string serverUrl, string username, string password, CultureInfo culture)
            : base(serverUrl, username, password, culture)
        {
        }
        #endregion

        protected override void InitializeSession()
        {
            _wsdl = new BSPSubtitlesService(_serverUrl);
            _wsdl.Proxy = AppConfig.GetWebProxy();
            _wsdl.UserAgent = string.Format("{0} v{1}", Constants.PlayerName, SuiteVersion.Version);
            _wsdl.Timeout = (int)Timeout;
        }

        protected override bool AuthenticationRequired => true;

        protected override void TestConnection()
        {
            string hello = _wsdl.helloWorld();
        }

        protected override void Authenticate()
        {
            SubtitlesResult res = _wsdl.logIn(_username, _password, _wsdl.UserAgent);
            if (res.result != "200")
                // not OK
                throw new SubtitleDownloadException("Login to BSP_V1 server has failed", res.status);

            _sessionToken = res.data;
        }

        protected override void CleanupSession()
        {
            _wsdl.Abort();
            _wsdl.Dispose();
            _wsdl = null;
        }

        protected override List<SubtitleInfo> GetSubtitles(VideoInfo vi)
        {
            List<SubtitleInfo> retVal = new List<SubtitleInfo>();

            SearchResult res = _wsdl.searchSubtitles(_sessionToken, vi.moviehash, (long)vi.moviebytesize, vi.sublanguageid, vi.imdbid);
            if (res.data != null && res.data.Length > 0)
            {
                var infos = from sd in res.data
                            where
                            (
                                //sd?.movieName?.Length > 0 &&
                                //sd?.movieHash?.Length > 0 &&
                                (sd.movieFPS <= 0 || vi.framerate == sd.movieFPS.ToString("##.###")) &&
                                sd?.subHash?.Length > 0 &&
                                sd?.subLang?.Length > 0 &&
                                sd?.subFormat?.Length > 0 &&
                                sd?.subDownloadLink?.Length > 0
                            )
                            select new SubtitleInfo
                            {
                                IDSubtitleFile = sd.subID,
                                SubFileName = sd.subName,
                                MovieName = sd.movieName,

                                LanguageName = LanguageHelper.Lookup(sd.subLang)?.DisplayName() ?? sd.subLang,
                                ISO639 = LanguageHelper.Lookup(sd.subLang)?.Part3 ?? sd.subLang,

                                MovieHash = sd.movieHash ?? vi.moviehash,

                                SubDownloadLink = sd.subDownloadLink,
                                SubHash = sd.subHash,
                                SubFormat = sd.subFormat,

                                FrameRate = sd.movieFPS > 0 ? sd.movieFPS.ToString(CultureInfo.InvariantCulture) : null,
                            };

                retVal.AddRange(infos);
            }

            return retVal;
        }

        protected override string DownloadSubtitleInternal(string fileName, SubtitleInfo subtitle)
        {
            string destPath = Path.ChangeExtension(fileName, subtitle.SubFormat);
            string downloadPath = Path.ChangeExtension(fileName, "gz");

            try
            {
                using (WebFileRetriever wfr = new WebFileRetriever(AppConfig.ProxySettings, subtitle.SubDownloadLink, downloadPath))
                {
                    wfr.PerformDownload(false);
                }

                using (FileStream compressedSubtitle = new FileStream(downloadPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    using (GZipStream str = new GZipStream(compressedSubtitle, CompressionMode.Decompress, false))
                    {
                        using (FileStream outputSubtitle = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            byte[] buffer = new byte[65536];
                            int read = 0;
                            do
                            {
                                read = str.Read(buffer, 0, buffer.Length);
                                if (read > 0)
                                {
                                    outputSubtitle.Write(buffer, 0, read);
                                }
                            }
                            while (read > 0);
                        }
                    }
                }

                File.Delete(downloadPath);

                return destPath;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }
    }
}
