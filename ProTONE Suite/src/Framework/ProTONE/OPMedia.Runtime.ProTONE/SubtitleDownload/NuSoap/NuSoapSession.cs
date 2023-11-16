using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.NuSoap;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OPMedia.Runtime.ProTONE.SubtitleDownload.NuSoap
{
    public class NuSoapSession : SubtitleServerSession
    {
        NuSoapWsdl _wsdl = null;

        #region Construction / Cleanup
        public NuSoapSession(string serverUrl, CultureInfo culture)
            : this(serverUrl, string.Empty, string.Empty, culture)
        {
        }

        public NuSoapSession(string serverUrl, string username, string password, CultureInfo culture)
            : base(serverUrl, username, password, culture)
        {
        }
        #endregion

        protected override void DoInitializeSession()
        {
            _wsdl = new NuSoapWsdl(_serverUrl);
            _wsdl.Proxy = AppConfig.GetWebProxy();
            _wsdl.UserAgent = string.Format("{0} v{1}", Constants.PlayerName, SuiteVersion.Version);
        }

        protected override bool IsAuthenticationRequired()
        {
            return false;
        }

        protected override void DoTestConnection()
        {
            OPMedia.Runtime.ProTONE.NuSoap.Language[] langs = _wsdl.getLanguages();
        }

        protected override void DoAuthenticate()
        {
            // No authentication
        }

        protected override double GetKeepAliveInterval()
        {
            return 0; // No keepalive mechanism
        }

        protected override void DoKeepAliveSession()
        {
            // No keepalive mechanism
        }

        protected override void DoCleanup()
        {
            _wsdl.Abort();
            _wsdl.Dispose();
            _wsdl = null;
        }

        protected override List<SubtitleInfo> DoGetSubtitles(VideoInfo vi)
        {
            List<SubtitleInfo> retVal = new List<SubtitleInfo>();

            SubtitleFile[] subtitles = _wsdl.searchSubtitlesByHash(vi.moviehash, vi.sublanguageid, 0, 100);
            if (subtitles != null && subtitles.Length > 0)
            {
                var infos = from sf in subtitles
                            where
                            (
                                sf?.file_name?.Length > 0 &&
                                sf?.sub_hash?.Length > 0 &&
                                sf?.language?.Length > 0
                            )
                            select new SubtitleInfo
                            {
                                IDSubtitleFile = sf.cod_subtitle_file.ToString(),
                                SubFileName = sf.file_name,
                                SubHash = sf.sub_hash,
                                LanguageName = sf.language,
                                MovieHash = vi.moviehash,
                                SubFormat = PathUtils.GetExtension(sf.file_name)
                            };

                retVal.AddRange(infos);
            }

            return retVal;
        }

        protected override Dictionary<string, object> DoGetServerStatistics()
        {
            return new Dictionary<string, object>();
        }

        protected override string DoDownloadSubtitle(string fileName, SubtitleInfo si)
        {
            OPMedia.Runtime.ProTONE.NuSoap.SubtitleDownload sd = new OPMedia.Runtime.ProTONE.NuSoap.SubtitleDownload();
            int x = 0;
            int.TryParse(si.IDSubtitleFile, out x);

            sd.cod_subtitle_file = x;
            sd.movie_hash = si.MovieHash;

            string destPath = Path.ChangeExtension(fileName, si.SubFormat);

            SubtitleArchive[] archives = _wsdl.downloadSubtitles(new OPMedia.Runtime.ProTONE.NuSoap.SubtitleDownload[] { sd });

            if (archives != null && archives.Length > 0)
            {
                byte[] decodedBytes = Convert.FromBase64String(archives[0].data);

                using (MemoryStream compressedSubtitle = new MemoryStream(decodedBytes))
                {
                    using (InflaterInputStream str = new InflaterInputStream(compressedSubtitle))
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

                return destPath;
            }

            return string.Empty;
        }
    }
}
