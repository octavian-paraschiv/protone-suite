using MovieCollection.OpenSubtitles;
using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.SubtitleDownload.BSP_V1;
using OPMedia.Runtime.ProTONE.SubtitleDownload.NuSoap;
using OPMedia.Runtime.ProTONE.SubtitleDownload.OpenSubtitles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.SubtitleDownload.Base
{
    public enum SubtitleServerType
    {
        NuSoap = 1,
        BSP_V1 = 2,
        OpenSubtitles = 3
    }

    public abstract class SubtitleServerSession : IDisposable
    {
        private System.Timers.Timer _tmrKeepAlive = null;

        protected string _serverUrl = string.Empty;
        protected string _username = string.Empty;
        protected string _password = string.Empty;
        protected CultureInfo _culture = null;

        protected string _sessionToken = string.Empty;

        #region Factory
        public static SubtitleServerSession Create(SubtitleServerType serverType, string serverUrl, string userName, string password, CultureInfo culture = null)
        {
            SubtitleServerSession session = null;

            try
            {
                switch (serverType)
                {
                    case SubtitleServerType.BSP_V1:
                        session = new BspV1Session(serverUrl, userName, password, culture);
                        break;

                    case SubtitleServerType.NuSoap:
                        session = new NuSoapSession(serverUrl, userName, password, culture);
                        break;

                    case SubtitleServerType.OpenSubtitles:
                        session = new OpenSubtitlesSession(serverUrl, userName, password, culture);
                        break;

                    default:
                        throw new ArgumentException($"Unsupported server type: {serverType}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return session;
        }
        #endregion

        #region Server session operations

        public List<SubtitleInfo> GetSubtitles(string fileName)
        {
            List<SubtitleInfo> retVal = new List<SubtitleInfo>();

            VideoFileInfo vfi = RenderingEngine.DefaultInstance.QueryVideoMediaInfo(fileName);
            if (vfi?.IsValid ?? false)
            {
                string hashCode = OpenSubtitlesHasher.GetFileHash(fileName);

                VideoInfo ovi = new VideoInfo
                {
                    framerate = (vfi.FrameRate?.Value ?? -1).ToString("##.###"),
                    imdbid = string.Empty,
                    moviehash = hashCode,
                    moviebytesize = vfi.Size.GetValueOrDefault(),
                    sublanguageid = "all"
                };

                #region GetSubtitles Commented code - DEBUG PURPOSE ONLY
                // Name the movie "fringe 4x03.avi"                
                //ovi.moviehash = "18379ac9af039390";
                //ovi.moviebytesize = 366876694;
                #endregion

                List<SubtitleInfo> response = GetSubtitles(ovi);

                string[] fileNameParts = vfi.Name.ToLowerInvariant().Split(" -.][(){}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fileNameParts.Length > 0)
                {
                    List<string> fileNamePartsList = new List<string>(fileNameParts);
                    foreach (SubtitleInfo osf in response)
                    {
                        if (CheckMatch(osf.MovieName, fileNamePartsList) ||
                            CheckMatch(osf.MovieNameEng, fileNamePartsList) ||
                            CheckMatch(osf.MovieReleaseName, fileNamePartsList) ||
                            CheckMatch(osf.SubFileName, fileNamePartsList))
                            retVal.Add(osf);

                    }
                }
            }

            return retVal;
        }

        public string DownloadSubtitle(string fileName, SubtitleInfo subtitle)
        {
            if (string.IsNullOrEmpty(subtitle.SubFormat))
                subtitle.SubFormat = "srt";

            return DownloadSubtitleInternal(fileName, subtitle);
        }

        #endregion

        #region Constructor
        public SubtitleServerSession(string serverUrl, string username, string password, CultureInfo culture)
        {
            _serverUrl = serverUrl;
            _username = username;
            _password = password;
            _culture = culture ?? Thread.CurrentThread.CurrentUICulture;

            InitializeSession();

            TestConnection();

            if (AuthenticationRequired)
                Authenticate();

            // Create keepalive timer
            // keepAliveInterval == 0 means that no keep alive is required.
            double keepAliveInterval = KeepAliveInterval;
            if (keepAliveInterval > 0)
            {
                _tmrKeepAlive = new System.Timers.Timer();
                _tmrKeepAlive.AutoReset = true;
                _tmrKeepAlive.Interval = keepAliveInterval;
                _tmrKeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(_tmrKeepAlive_Elapsed);
                _tmrKeepAlive.Start();
            }

            Logger.LogTrace($"{GetType().Name}: object created");
        }
        #endregion

        #region Implementation
        void _tmrKeepAlive_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_tmrKeepAlive != null)
                _tmrKeepAlive.Stop();

            try
            {
                KeepAliveSession();
            }
            finally
            {
                if (_tmrKeepAlive != null)
                    _tmrKeepAlive.Start();
            }
        }

        protected void CheckAuthentication(string operationName)
        {
            if (AuthenticationRequired && string.IsNullOrEmpty(_sessionToken))
            {
                throw new SubtitleDownloadException("Cannot perform " + operationName, "Not yet logged in");
            }
        }

        public void Dispose()
        {
            if (_tmrKeepAlive != null)
            {
                _tmrKeepAlive.Stop();
                _tmrKeepAlive = null;
            }

            CleanupSession();

            _sessionToken = null;

            Logger.LogTrace($"{GetType().Name}: object disposed");
        }

        protected bool CheckMatch(string movieName, List<string> fileNamePartsList)
        {
            if (string.IsNullOrEmpty(movieName) ||
                fileNamePartsList == null || fileNamePartsList.Count == 0)
                return false;


            string[] movieNameParts = movieName
                .ToLowerInvariant()
                .Trim(Path.GetInvalidFileNameChars())
                .Split(" -.][(){}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (movieNameParts.Length > 0)
            {
                List<string> movieNamePartsList = new List<string>(movieNameParts);
                int matchCounter = 0;

                foreach (string movieNamePart in movieNameParts)
                {
                    // Do we have a partial name match ?
                    if (fileNamePartsList.Contains(movieNamePart))
                        matchCounter++;

                    // Would current movie name part indicate it's a TV series ?
                    // I.e. S01E04 or 01x04
                    else if (CheckSeriesMovieName(movieNamePart, fileNamePartsList))
                        return true;

                    if (matchCounter >= (fileNamePartsList.Count - 2))
                        return true;
                }
            }

            return false;
        }

        protected bool CheckSeriesMovieName(string movieNamePart, List<string> fileNamePartsList)
        {
            int movieEpisode = 0, movieSeason = 0;

            if (GetSeriesMovieCounters(movieNamePart, out movieSeason, out movieEpisode))
            {
                foreach (string fileNamePart in fileNamePartsList)
                {
                    int e = 0, s = 0;
                    if (GetSeriesMovieCounters(fileNamePart, out s, out e))
                    {
                        if (s == movieSeason && e == movieEpisode)
                            return true;
                    }
                }
            }

            return false;
        }

        protected bool GetSeriesMovieCounters(string fields, out int season, out int episode)
        {
            string[] seriesFields = fields.ToLowerInvariant().Split(
                "sex".ToCharArray() /* no obscene meaning ... these letters REALLY are our delimters ! :D */,
                StringSplitOptions.RemoveEmptyEntries);

            if (seriesFields.Length > 1)
            {
                bool s = int.TryParse(seriesFields[0], out season);
                bool e = int.TryParse(seriesFields[1], out episode);
                return s && e;
            }
            else if (seriesFields.Length == 1)
            {
                bool e = int.TryParse(seriesFields[0], out episode);
                season = 0;
                return e;
            }

            season = episode = -1;
            return false;
        }
        #endregion

        protected virtual string UserAgent => $"{ProTONEConstants.PlayerUserAgent} v{SuiteVersion.Version}";
        protected virtual bool AuthenticationRequired => true;
        protected virtual double KeepAliveInterval => 0;
        protected virtual double Timeout => 1000 * 20;

        protected virtual void Authenticate() { }
        protected virtual void KeepAliveSession() { }

        protected abstract void InitializeSession();
        protected abstract void TestConnection();
        protected abstract void CleanupSession();

        protected abstract List<SubtitleInfo> GetSubtitles(VideoInfo vi);

        protected abstract string DownloadSubtitleInternal(string fileName, SubtitleInfo subtitle);
    }
}
