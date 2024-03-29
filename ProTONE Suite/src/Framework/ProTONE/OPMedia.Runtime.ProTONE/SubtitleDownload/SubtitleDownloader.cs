﻿using OPMedia.Core.Logging;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.FileInformation;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using System;
using System.Collections.Generic;

namespace OPMedia.Runtime.ProTONE.SubtitleDownload
{
    public class SubtitleDownloader : IDisposable
    {
        public static readonly List<string> AllowedSubtitleExtensions = new List<string>(
            new string[] { "sub", "srt", "txt", "utf", "idx", "smi", "rt", "ass", "ssa", "aqt", "mpl", "usf" });

        SubtitleServerType _serverType = SubtitleServerType.OpenSubtitles;
        string _serverUrl = string.Empty;
        string _userName = string.Empty;
        string _password = string.Empty;

        public string ServerUrl
        { get { return _serverUrl; } }

        public int Priority { get; set; }

        public bool IsActive { get; set; }

        public string DisplayName
        {
            get
            {
                string dn = _serverType.ToString();
                try
                {
                    dn = new Uri(_serverUrl).Host;
                }
                catch
                {
                }

                return dn;
            }
        }

        SubtitleServerSession _session = null;

        public static SubtitleDownloader FromDownloadURI(string downloadURI)
        {
            string serverUrl = string.Empty, serverTypeStr = string.Empty,
                userName = string.Empty, password = string.Empty;

            bool active = false;

            string[] fields = StringUtils.ToStringArray(downloadURI, ';');
            if (fields != null && fields.Length > 0)
            {
                int i = 0;
                serverTypeStr = fields[i++];
                if (fields.Length > i)
                    serverUrl = fields[i++];
                if (fields.Length > i)
                    active = (fields[i++] == "1");
                if (fields.Length > i)
                    userName = fields[i++];
                if (fields.Length > i)
                    password = fields[i++];
            }

            SubtitleServerType serverType = SubtitleServerType.OpenSubtitles;
            try
            {
                serverType = (SubtitleServerType)Enum.Parse(typeof(SubtitleServerType), serverTypeStr);
            }
            catch
            {
                serverType = SubtitleServerType.OpenSubtitles;
            }

            SubtitleDownloader sd = new SubtitleDownloader(serverType, serverUrl, userName, password);
            sd.IsActive = active;

            return sd;
        }

        #region Constructors
        public SubtitleDownloader(string serverUrl, string userName, string password)
            : this(SubtitleServerType.OpenSubtitles, serverUrl, userName, password)
        {
        }

        public SubtitleDownloader(SubtitleServerType serverType, string serverUrl, string userName, string password)
        {
            _serverType = serverType;
            _serverUrl = serverUrl;
            _userName = userName;
            _password = password;
        }
        #endregion

        #region Generic purpose methods
        public List<SubtitleInfo> GetSubtitles(string fileName)
        {
            try
            {
                NativeFileInfo nfi = new NativeFileInfo(fileName, true);
                if (nfi.IsValid)
                {
                    if (_session == null)
                        _session = SubtitleServerSession.Create(_serverType, _serverUrl, _userName, _password);

                    return _session?.GetSubtitles(fileName);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public string DownloadCompressedSubtitle(string fileName, SubtitleInfo subtitle)
        {
            try
            {
                if (_session == null)
                    _session = SubtitleServerSession.Create(_serverType, _serverUrl, _userName, _password);

                return _session?.DownloadSubtitle(fileName, subtitle);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return string.Empty;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _session?.Dispose();
            _session = null;
        }

        #endregion
    }
}
