using Microsoft.Win32;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Utilities;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OPMedia.Runtime.ProTONE.Configuration
{
    public enum CddaInfoSource
    {
        // Don't read audio cd info
        None = 0,
        // Read CD-text only
        CdText,
        // Read CDDB only
        Cddb,
        // Try CD-text first then CDDB [Default]
        CdText_Cddb,
        // Try CDDB first then CD-text
        Cddb_CdText
    }

    [Flags]
    public enum SignalAnalisysFunction
    {
        None = 0x00,

        VUMeter = 0x01,
        Waveform = 0x02,
        Spectrogram = 0x04,

        ExportInterface = 0x10,

        All = 0xFF
    }


    public static class ProTONEConfig
    {
        const string _Fallback_DefaultSubtitleURIs =
            "OpenSubtitles;https://api.opensubtitles.com/api/v1;1\\" +
            "BSP_V1;http://api.bsplayer-subtitles.com/v1.php;1\\" +
            "NuSoap;http://api.getsubtitle.com/server.php;1\\" +
            "";

        const string _Fallback_DefaultLinkedFiles =
            @"AU;AIF;AIFF;CDA;FLAC;MID;MIDI;MP1;MP2;MP3;MPA;RAW;RMI;SND;WAV;WMA/BMK\AVI;DIVX;QT;M1V;M2V;MOD;MOV;MPG;MPEG;VOB;WM;WMV;MKV;MP4/SUB;SRT;USF;ASS;SSA;BMK";


        #region Calculated Level 2 settings

        public static string PlayerInstallationPath
        {
            get
            {
                return Path.Combine(AppConfig.InstallationPath, Core.Constants.PlayerBinary);
            }
        }

        public static string LibraryInstallationPath
        {
            get
            {
                return Path.Combine(AppConfig.InstallationPath, Core.Constants.LibraryBinary);
            }
        }

        public static bool IsPlayer
        {
            get
            {
                return (string.Compare(ApplicationInfo.ApplicationName, Core.Constants.PlayerName) == 0);
            }
        }

        public static bool IsMediaLibrary
        {
            get
            {
                return (string.Compare(ApplicationInfo.ApplicationName, Core.Constants.LibraryName) == 0);
            }
        }
        #endregion

        #region Level 2 Settings using Persistence Service (Per-suite settings)

        public static int OsdbKeepAliveInterval
        {
            get
            {
                return PersistenceProxy.ReadNode("OsdbKeepAliveInterval", 5 * 60 * 1000);
            }
            set
            {
                PersistenceProxy.SaveNode("OsdbKeepAliveInterval", value);
            }
        }


        public static bool UseMetadata
        {
            get { return PersistenceProxy.ReadNode("UseMetadata", true); }
            set { PersistenceProxy.SaveNode("UseMetadata", value); }
        }

        public static bool UseFileNameFormat
        {
            get { return PersistenceProxy.ReadNode("UseFileNameFormat", true); }
            set { PersistenceProxy.SaveNode("UseFileNameFormat", value); }
        }

        public static string PlaylistEntryFormat
        {
            get { return PersistenceProxy.ReadNode("PlaylistEntryFormat", "<A> - <T>"); }
            set { PersistenceProxy.SaveNode("PlaylistEntryFormat", value); }
        }

        public static string FileNameFormat
        {
            get { return PersistenceProxy.ReadNode("FileNameFormat", "<A> - <T>"); }
            set { PersistenceProxy.SaveNode("FileNameFormat", value); }
        }

        public static string CustomPlaylistEntryFormats
        {
            get { return PersistenceProxy.ReadNode("CustomPlaylistEntryFormats", string.Empty); }
            set { PersistenceProxy.SaveNode("CustomPlaylistEntryFormats", value); }
        }

        public static string CustomFileNameFormats
        {
            get { return PersistenceProxy.ReadNode("CustomFileNameFormats", string.Empty); }
            set { PersistenceProxy.SaveNode("CustomFileNameFormats", value); }
        }


        public static bool DisableDVDMenu
        {
            get { return PersistenceProxy.ReadNode(true, "DisableDVDMenu", false); }
            set { PersistenceProxy.SaveNode(true, "DisableDVDMenu", value); }
        }

        public static int PrefferedSubtitleLang
        {
            get { return PersistenceProxy.ReadNode("PrefferedSubtitleLang", 1033); }
            set { PersistenceProxy.SaveNode("PrefferedSubtitleLang", value); }
        }

        public static bool SubEnabled
        {
            get { return PersistenceProxy.ReadNode("SubEnabled", false); }
            set { PersistenceProxy.SaveNode("SubEnabled", value); }
        }

        public static bool OsdEnabled
        {
            get { return PersistenceProxy.ReadNode("OsdEnabled", false); }
            set { PersistenceProxy.SaveNode("OsdEnabled", value); }
        }

        public static Color OsdColor
        {
            get
            {
                int argb = PersistenceProxy.ReadNode("OsdColor", Color.White.ToArgb());
                return Color.FromArgb(argb);
            }

            set
            {
                PersistenceProxy.SaveNode("OsdColor", value.ToArgb());
            }
        }

        public static Color SubColor
        {
            get
            {
                int argb = PersistenceProxy.ReadNode("SubColor", Color.White.ToArgb());
                return Color.FromArgb(argb);
            }

            set
            {
                PersistenceProxy.SaveNode("SubColor", value.ToArgb());
            }
        }

        static Font DefSubAndOsdFont = new Font("Segoe UI", 12f, FontStyle.Bold, GraphicsUnit.Point);

        public static Font OsdFont
        {
            get
            {
                string _f = PersistenceProxy.ReadNode("OsdFont", new FontConverter().ConvertToInvariantString(DefSubAndOsdFont));
                Font f = (Font)new FontConverter().ConvertFromInvariantString(_f);
                byte charSet = (byte)PersistenceProxy.ReadNode("OsdFontCharSet", DefSubAndOsdFont.GdiCharSet);
                return new Font(f.FontFamily, f.Size, f.Style, f.Unit, charSet);
            }

            set
            {
                PersistenceProxy.SaveNode("OsdFont", new FontConverter().ConvertToInvariantString(value));
                PersistenceProxy.SaveNode("OsdFontCharSet", value.GdiCharSet);
            }
        }

        public static Font SubFont
        {
            get
            {
                string _f = PersistenceProxy.ReadNode("SubFont", new FontConverter().ConvertToInvariantString(DefSubAndOsdFont));
                Font f = (Font)new FontConverter().ConvertFromInvariantString(_f);
                byte charSet = (byte)PersistenceProxy.ReadNode("SubFontCharSet", DefSubAndOsdFont.GdiCharSet);
                return new Font(f.FontFamily, f.Size, f.Style, f.Unit, charSet);
            }

            set
            {
                PersistenceProxy.SaveNode("SubFont", new FontConverter().ConvertToInvariantString(value));
                PersistenceProxy.SaveNode("SubFontCharSet", value.GdiCharSet);
            }
        }

        public static int OsdPersistTimer
        {
            get
            {
                return PersistenceProxy.ReadNode("OsdPersistTimer", 4000);
            }

            set
            {
                PersistenceProxy.SaveNode("OsdPersistTimer", value);
            }
        }

        public static bool SubtitleDownloadEnabled
        {
            get
            {
                return PersistenceProxy.ReadNode("SubtitleDownloadEnabled", true);
            }

            set
            {
                PersistenceProxy.SaveNode("SubtitleDownloadEnabled", value);
            }
        }

        public static int SubtitleMinimumMovieDuration
        {
            get
            {
                return PersistenceProxy.ReadNode("SubtitleMinimumMovieDuration", 20 /* 20 minutes */);
            }

            set
            {
                PersistenceProxy.SaveNode("SubtitleMinimumMovieDuration", value);
            }
        }

        public static string DefaultSubtitleURIs
        {
            get { return PersistenceProxy.ReadNode("DefaultSubtitleURIs", _Fallback_DefaultSubtitleURIs, false); }
        }

        public static string DefaultLinkedFiles
        {
            get { return PersistenceProxy.ReadNode("DefaultLinkedFiles", _Fallback_DefaultLinkedFiles, false); }
        }


        public static CddaInfoSource AudioCdInfoSource
        {
            get { return PersistenceProxy.ReadNode("AudioCdInfoSource", CddaInfoSource.CdText_Cddb, false); }
            set { PersistenceProxy.SaveNode("AudioCdInfoSource", value, false); }
        }

        public static string CddbServerName
        {
            get { return PersistenceProxy.ReadNode("CddbServerName", "freedb.freedb.org", false); }
            set { PersistenceProxy.SaveNode("CddbServerName", value, false); }
        }

        public static int CddbServerPort
        {
            get { return PersistenceProxy.ReadNode("CddbServerPort", 8880, false); }
            set { PersistenceProxy.SaveNode("CddbServerPort", value, false); }
        }

        #endregion

        #region Level 2 Settings using Persistence Service (Per-user settings)

        public static string SubtitleDownloadURIs
        {
            get { return PersistenceProxy.ReadNode("SubtitleDownloadURIs", DefaultSubtitleURIs); }
            set { PersistenceProxy.SaveNode("SubtitleDownloadURIs", value); }
        }

        public static List<string> GetFavoriteFolders(string favFoldersHiveName)
        {
            List<string> favoriteFolders = new List<string>();

            string str = PersistenceProxy.ReadNode(favFoldersHiveName, string.Empty);
            if (!string.IsNullOrEmpty(str))
            {
                string[] favFolders = StringUtils.ToStringArray(str, '?');
                favoriteFolders.AddRange(favFolders);
            }

            return favoriteFolders;
        }

        public static void SetFavoriteFolders(List<string> folders, string favFoldersHiveName)
        {
            List<string> favoriteFolders = new List<string>();
            favoriteFolders.AddRange(folders);

            string favFolders = StringUtils.FromStringArray(favoriteFolders.ToArray(), '?');
            if (favFolders == null)
                favFolders = string.Empty;

            PersistenceProxy.SaveNode(favFoldersHiveName, favFolders);
        }

        public static bool AddToFavoriteFolders(string path)
        {
            List<string> favorites = new List<string>(ProTONEConfig.GetFavoriteFolders("FavoriteFolders"));
            if (favorites.Contains(path))
                return false;

            favorites.Add(path);
            ProTONEConfig.SetFavoriteFolders(favorites, "FavoriteFolders");
            return true;
        }

        static bool? _useLinkedFiles = null;

        public static bool UseLinkedFiles
        {
            get
            {
                if (_useLinkedFiles.HasValue)
                    return _useLinkedFiles.Value;

                _useLinkedFiles = (PersistenceProxy.ReadNode("UseLinkedFiles", 1) != 0);

                return _useLinkedFiles.Value;
            }
            set
            {
                _useLinkedFiles = value;
                PersistenceProxy.SaveNode("UseLinkedFiles", value ? 1 : 0);
            }
        }

        static Dictionary<string, string> _table = null;
        public static Dictionary<string, string> LinkedFilesTable
        {
            get
            {
                if (_table == null)
                {
                    _table = new Dictionary<string, string>();

                    try
                    {
                        string st = PersistenceProxy.ReadNode("LinkedFiles", DefaultLinkedFiles);
                        string[] pairs = StringUtils.ToStringArray(st, '\\');
                        if (pairs != null && pairs.Length > 0)
                        {
                            foreach (string pair in pairs)
                            {
                                string[] nameValue = StringUtils.ToStringArray(pair, '/');
                                if (nameValue != null && nameValue.Length > 0)
                                {
                                    string name = nameValue[0];
                                    string value = nameValue.Length > 1 ? nameValue[1] : string.Empty;

                                    try
                                    {
                                        _table.Add(name, value);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                return _table;
            }

            set
            {
                try
                {
                    if (value != null)
                    {
                        string str = string.Empty;
                        foreach (KeyValuePair<string, string> kvp in value)
                        {
                            str += kvp.Key;
                            str += "/";
                            str += kvp.Value;
                            str += "\\";
                        }

                        str = str.Trim('\\').Trim('/');

                        PersistenceProxy.SaveNode("LinkedFiles", str);
                    }

                    _table = value;
                }
                catch
                {
                }
            }
        }

        public static string[] GetChildFileTypes(string fileType)
        {
            if (_table == null)
                _table = LinkedFilesTable;

            foreach (KeyValuePair<string, string> kvp in _table)
            {
                List<string> types = new List<string>(StringUtils.ToStringArray(kvp.Key, ';'));
                if (types.Contains(fileType.ToUpperInvariant()))
                {
                    return StringUtils.ToStringArray(kvp.Value, ';');
                }
            }

            return null;
        }

        public static string[] GetParentFileTypes(string fileType)
        {
            foreach (KeyValuePair<string, string> kvp in _table)
            {
                List<string> types = new List<string>(StringUtils.ToStringArray(kvp.Value, ';'));
                if (types.Contains(fileType.ToUpperInvariant()))
                {
                    return StringUtils.ToStringArray(kvp.Key, ';');
                }
            }

            return null;
        }
        #endregion

        #region Level 2 Settings using Settings File (Combined per-app and per-user settings)

        public static string ExplorerLaunchType
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "ExplorerLaunchType", "EnqueueFiles");
            }

            set
            {
                PersistenceProxy.SaveNode(true, "ExplorerLaunchType", value);
            }
        }

        public static int LastBalance
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LastBalance", 0);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LastBalance", value);
            }
        }

        public static int LastVolume
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LastVolume", 5000);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LastVolume", value);
            }
        }

        public static int LastFilterIndex
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LastFilterIndex", 0);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LastFilterIndex", value);
            }
        }

        public static string LastOpenedFolder
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LastOpenedFolder", PathUtils.CurrentDir);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LastOpenedFolder", value);
            }
        }

        public static int PL_LastFilterIndex
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "PL_LastFilterIndex", 0);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "PL_LastFilterIndex", value);
            }
        }

        public static string PL_LastOpenedFolder
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "PL_LastOpenedFolder", PathUtils.CurrentDir);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "PL_LastOpenedFolder", value);
            }
        }



        public static bool LoopPlay
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "LoopPlay", false);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "LoopPlay", value);
            }
        }

        public static bool XFade
        {
            get
            {
                return PersistenceProxy.ReadNode(false, "XFade", false);
            }

            set
            {
                PersistenceProxy.SaveNode(false, "XFade", value);
            }
        }

        public static int XFadeLength
        {
            get
            {
                return PersistenceProxy.ReadNode(false, "XFadeLength", 10);
            }

            set
            {
                PersistenceProxy.SaveNode(false, "XFadeLength", value);
            }
        }

        public static int XFadeAnticipationPercentage
        {
            get
            {
                double val = PersistenceProxy.ReadNode(false, "XFadeAnticipationPercentage", 85);
                return (int)Math.Min(99, Math.Max(val, 1));
            }

            set
            {
                PersistenceProxy.SaveNode(false, "XFadeAnticipationPercentage", value);
            }
        }

        public static int XFadeProfile
        {
            get
            {
                return PersistenceProxy.ReadNode(false, "XFadeProfile", 0);
            }

            set
            {
                PersistenceProxy.SaveNode(false, "XFadeProfile", value);
            }
        }

        public static int PlaylistEventHandler
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "PlaylistEventHandler", 0);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "PlaylistEventHandler", value);
            }
        }

        public static string PlaylistEventData
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "PlaylistEventData", string.Empty);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "PlaylistEventData", value);
            }
        }


        public static int ScheduledEventHandler
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "ScheduledEventHandler", 0);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "ScheduledEventHandler", value);
            }
        }

        public static TimeSpan ScheduledEventTime
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "ScheduledEventTime", new TimeSpan(0, 0, 0));
            }
            set
            {
                PersistenceProxy.SaveNode(true, "ScheduledEventTime", value);
            }
        }

        public static int ScheduledEventDays
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "ScheduledEventDays", 0);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "ScheduledEventDays", value);
            }
        }

        public static bool EnableScheduledEvent
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "EnableScheduledEvent", false);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "EnableScheduledEvent", value);
            }
        }

        public static int SchedulerWaitTimerProceed
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SchedulerWaitTimerProceed", 2);
            }
            set
            {
                PersistenceProxy.SaveNode(true, "SchedulerWaitTimerProceed", value);
            }
        }



        public static bool MediaStateNotificationsEnabled
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "MediaStateNotificationsEnabled", true);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "MediaStateNotificationsEnabled", value);
            }
        }

        public static bool SubDownloadedNotificationsEnabled
        {
            get
            {
                return PersistenceProxy.ReadNode(true, "SubDownloadedNotificationsEnabled", true);
            }

            set
            {
                PersistenceProxy.SaveNode(true, "SubDownloadedNotificationsEnabled", value);
            }
        }


        public static bool FullScreenOn
        {
            get
            {
                if (IsPlayer)
                    return PersistenceProxy.ReadNode(true, "FullScreenOn", false);

                return false;
            }

            set
            {
                if (IsPlayer)
                    PersistenceProxy.SaveNode(true, "FullScreenOn", value);
            }
        }

        public static Point OnlineContentBrowser_WindowLocation
        {
            get
            {
                try
                {
                    string str = PersistenceProxy.ReadNode(true, "OnlineContentBrowser_WindowLocation", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return (Point)new PointConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }

                Point ptFallback = new Point(100, 100);

                PersistenceProxy.SaveNode(true, "OnlineContentBrowser_WindowLocation", new PointConverter().ConvertToInvariantString(ptFallback));

                return ptFallback;
            }
            set
            {
                if ((value.X >= 0) && (value.Y >= 0))
                {
                    PersistenceProxy.SaveNode(true, "OnlineContentBrowser_WindowLocation", new PointConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static Size OnlineContentBrowser_WindowSize
        {
            get
            {
                Size size = new Size(800, 600);
                try
                {
                    string str = PersistenceProxy.ReadNode(true, "OnlineContentBrowser_WindowSize", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        size = (Size)new SizeConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }
                return size;
            }
            set
            {
                if ((value.Width >= 0) && (value.Height >= 0))
                {
                    PersistenceProxy.SaveNode(true, "OnlineContentBrowser_WindowSize", new SizeConverter().ConvertToInvariantString(value));
                }
            }
        }


        public static Point DetachedWindowLocation
        {
            get
            {
                try
                {
                    string str = PersistenceProxy.ReadNode(true, "DetachedWindowLocation", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return (Point)new PointConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }

                Point ptFallback = new Point(100, 100);

                PersistenceProxy.SaveNode(true, "DetachedWindowLocation", new PointConverter().ConvertToInvariantString(ptFallback));

                return ptFallback;
            }
            set
            {
                if ((value.X >= 0) && (value.Y >= 0))
                {
                    PersistenceProxy.SaveNode(true, "DetachedWindowLocation", new PointConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static Size DetachedWindowSize
        {
            get
            {
                Size size = new Size(800, 600);
                try
                {
                    string str = PersistenceProxy.ReadNode(true, "DetachedWindowSize", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        size = (Size)new SizeConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }
                return size;
            }
            set
            {
                if ((value.Width >= 0) && (value.Height >= 0))
                {
                    PersistenceProxy.SaveNode(true, "DetachedWindowSize", new SizeConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static FormWindowState DetachedWindowState
        {
            get
            {
                FormWindowState normal = FormWindowState.Normal;
                try
                {
                    normal = (FormWindowState)PersistenceProxy.ReadNode(true, "DetachedWindowState", 0);
                }
                catch
                {
                }
                return normal;
            }
            set
            {
                PersistenceProxy.SaveNode(true, "DetachedWindowState", (int)value);
            }
        }


        public static Point SA_WindowLocation
        {
            get
            {
                try
                {
                    string str = PersistenceProxy.ReadNode(true, "SA_WindowLocation", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return (Point)new PointConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }

                Point ptFallback = new Point(100, 100);

                PersistenceProxy.SaveNode(true, "SA_WindowLocation", new PointConverter().ConvertToInvariantString(ptFallback));

                return ptFallback;
            }

            set
            {
                if ((value.X >= 0) && (value.Y >= 0))
                {
                    PersistenceProxy.SaveNode(true, "SA_WindowLocation", new PointConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static Size SA_WindowSize
        {
            get
            {
                Size size = new Size(800, 600);
                try
                {
                    string str = PersistenceProxy.ReadNode(true, "SA_WindowSize", string.Empty);
                    if (!string.IsNullOrEmpty(str))
                    {
                        size = (Size)new SizeConverter().ConvertFromInvariantString(str);
                    }
                }
                catch
                {
                }
                return size;
            }
            set
            {
                if ((value.Width >= 0) && (value.Height >= 0))
                {
                    PersistenceProxy.SaveNode(true, "SA_WindowSize", new SizeConverter().ConvertToInvariantString(value));
                }
            }
        }

        public static FormWindowState SA_WindowState
        {
            get
            {
                FormWindowState normal = FormWindowState.Normal;
                try
                {
                    normal = (FormWindowState)PersistenceProxy.ReadNode(true, "SA_WindowState", 0);
                }
                catch
                {
                }
                return normal;
            }
            set
            {
                PersistenceProxy.SaveNode(true, "SA_WindowState", (int)value);
            }
        }

        #endregion

        #region SHoutCast settings

        public static string ShoutCastApiDevID
        {
            get { return PersistenceProxy.ReadNode("ShoutCastApiDevID", string.Empty); }
            set { PersistenceProxy.SaveNode("ShoutCastApiDevID", value); }
        }

        public static string ShoutCastSearchBaseURL
        {
            get { return PersistenceProxy.ReadNode("ShoutCastSearchBaseURL", "http://api.shoutcast.com/station"); }
            set { PersistenceProxy.SaveNode("ShoutCastSearchBaseURL", value); }
        }

        public static string ShoutCastTuneInBaseURL
        {
            get { return PersistenceProxy.ReadNode("ShoutCastTuneInBaseURL", "http://yp.shoutcast.com"); }
            set { PersistenceProxy.SaveNode("ShoutCastTuneInBaseURL", value); }
        }

        #endregion

        #region Deezer settings

        public static dz_track_quality_t DeezerTrackQuality
        {
            get { return PersistenceProxy.ReadNode("DeezerTrackQuality", dz_track_quality_t.DZ_TRACK_QUALITY_CDQUALITY); }
            set { PersistenceProxy.SaveNode("DeezerTrackQuality", value); }
        }

        public static string DeezerUserAccessToken
        {
            get { return PersistenceProxy.ReadNode("DeezerUserAccessToken", string.Empty); }
            set { PersistenceProxy.SaveNode("DeezerUserAccessToken", value); }
        }

        public static bool DeezerHasValidConfig
        {
            get
            {
                string userAccessToken = ProTONEConfig.DeezerUserAccessToken;
                return (string.IsNullOrEmpty(userAccessToken) == false);
            }
        }

        public static bool DeezerUseServicesForFileMetadata
        {
            get { return PersistenceProxy.ReadNode("DeezerUseServicesForFileMetadata", false); }
            set { PersistenceProxy.SaveNode("DeezerUseServicesForFileMetadata", value); }
        }

        #endregion

        #region Media Browser History
        public static List<string> Media_Browser_History_Local
        {
            get
            {
                string str = PersistenceProxy.ReadNode(true, "Media_Browser_History_Local", string.Empty);
                return StringUtils.ToStringList(str, '|');
            }

            set
            {
                string str = StringUtils.FromStringList(value, '|');
                PersistenceProxy.SaveNode(true, "Media_Browser_History_Local", str);
            }
        }

        public static List<string> Media_Browser_History_Shoutcast
        {
            get
            {
                string str = PersistenceProxy.ReadNode(true, "Media_Browser_History_Shoutcast", string.Empty);
                return StringUtils.ToStringList(str, '|');
            }

            set
            {
                string str = StringUtils.FromStringList(value, '|');
                PersistenceProxy.SaveNode(true, "Media_Browser_History_Shoutcast", str);
            }
        }

        public static List<string> Media_Browser_History_Deezer
        {
            get
            {
                string str = PersistenceProxy.ReadNode(true, "Media_Browser_History_Deezer", string.Empty);
                return StringUtils.ToStringList(str, '|');
            }

            set
            {
                string str = StringUtils.FromStringList(value, '|');
                PersistenceProxy.SaveNode(true, "Media_Browser_History_Deezer", str);
            }
        }
        #endregion

        static Guid _filterGraphGuid = Guid.Empty;
        static object _filterGraphGuidLock = new object();

        public static Guid FilterGraphGuid
        {
            get
            {
                if (_filterGraphGuid == Guid.Empty)
                {
                    Guid defaultGuid = Filters.FilterGraphNoThread;

                    try
                    {
                        lock (_filterGraphGuidLock)
                        {
                            if (_filterGraphGuid == Guid.Empty)
                            {
                                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OPMedia Research\ProTONE Suite");
                                string str = key.GetValue("SelectedFilterGraphGuid", defaultGuid.ToString()) as string;
                                _filterGraphGuid = Guid.Parse(str);
                            }
                        }
                    }
                    catch
                    {
                        _filterGraphGuid = defaultGuid;
                    }
                }

                return _filterGraphGuid;
            }
        }


        private static SignalAnalisysFunction? _signalAnalisysFunctions = null;
        private static object _signalAnalisysFunctionsLock = new object();

        public static SignalAnalisysFunction SignalAnalisysFunctions
        {
            get
            {
                lock (_signalAnalisysFunctionsLock)
                {
                    if (_signalAnalisysFunctions == null)
                        _signalAnalisysFunctions = (SignalAnalisysFunction)PersistenceProxy.ReadNode(true, "SignalAnalisysFunctions",
                            (int)SignalAnalisysFunction.All);

                    if (_signalAnalisysFunctions == null)
                        return SignalAnalisysFunction.All;

                    return _signalAnalisysFunctions.Value;
                }
            }

            set
            {
                lock (_signalAnalisysFunctionsLock)
                {
                    if (_signalAnalisysFunctions == null || _signalAnalisysFunctions.Value != value)
                    {
                        _signalAnalisysFunctions = value;
                        PersistenceProxy.SaveNode(true, "SignalAnalisysFunctions", (int)value);
                    }
                }
            }
        }

        public static bool SignalAnalisysFunctionActive(SignalAnalisysFunction function)
        {
            return ((ProTONEConfig.SignalAnalisysFunctions & function) == function);
        }

        public static bool IsSignalAnalisysActive()
        {
            return (ProTONEConfig.SignalAnalisysFunctions != SignalAnalisysFunction.None);
        }
    }
}
