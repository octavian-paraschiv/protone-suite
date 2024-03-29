#region Copyright � 2008 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	PlaylistItem.cs
#endregion

#region Using directives
using OPMedia.Core;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.DeezerInterop.RestApi;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.ExtendedInfo;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.Runtime.ProTONE.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
#endregion

namespace OPMedia.Runtime.ProTONE.Playlists
{
    // This item appears in main playlist and in Playlist menu item of main menu
    public class PlaylistItem
    {
        protected MediaFileInfo mi = MediaFileInfo.Empty;

        public int DelayStart { get; set; }

        public bool IsVideo
        {
            get
            {
                return (mi is VideoFileInfo);
            }
        }

        [Browsable(false)]
        public MediaFileInfo MediaFileInfo
        {
            get { return mi; }
        }



        public bool SupportsTrackInfo
        {
            get { return _SupportsTrackInfo(); }
        }
        public bool IsTrackInfoEditable
        {
            get { return _IsTrackInfoEditable(); }
        }

        public bool SupportsBookmarkInfo
        {
            get { return _SupportsBookmarkInfo(); }
        }
        public bool IsBookmarkInfoEditable
        {
            get { return _IsBookmarkInfoEditable(); }
        }



        public virtual string Path
        {
            get
            {
                return mi.Path;
            }
        }

        public override bool Equals(object obj)
        {
            PlaylistItem pi = obj as PlaylistItem;
            if (pi != null)
            {
                return (string.Compare(pi.Path, this.Path, true) == 0);
            }

            return false;
        }

        public virtual string PersistentPlaylistName
        {
            get
            {
                return DisplayName;
            }
        }

        public virtual string DisplayName
        {
            get
            {
                string retVal = string.Empty;

                string artist = string.Empty;
                string album = string.Empty;
                string title = string.Empty;
                string genre = string.Empty;
                string comments = string.Empty;
                string track = string.Empty;
                string year = string.Empty;

                if (ProTONEConfig.UseMetadata)
                {
                    // Format using metadata
                    artist = mi.Artist;
                    album = mi.Album;
                    title = mi.Title;
                    genre = mi.Genre;
                    comments = mi.Comments;
                    track = (mi.Track.HasValue) ? mi.Track.GetValueOrDefault().ToString("d2") : string.Empty;
                    year = (mi.Year.HasValue) ? mi.Year.GetValueOrDefault().ToString("d4") : string.Empty;
                }

                if (ProTONEConfig.UseFileNameFormat)
                {
                    // First - parse the file name
                    string name = mi.Name;
                    if (!string.IsNullOrEmpty(mi.Extension))
                    {
                        name = name.Replace(mi.Extension, string.Empty);
                    }

                    Dictionary<string, string> fileTokens = StringUtils.Tokenize(name, ProTONEConfig.FileNameFormat);

                    // Second - replace formatting fields with data from file name where available
                    if (fileTokens != null && fileTokens.Count > 0)
                    {
                        artist = GetFieldValue(artist, GetTokenValue(fileTokens, "<A>"));
                        album = GetFieldValue(album, GetTokenValue(fileTokens, "<B>"));
                        title = GetFieldValue(title, GetTokenValue(fileTokens, "<T>"));
                        genre = GetFieldValue(genre, GetTokenValue(fileTokens, "<G>"));
                        comments = GetFieldValue(comments, GetTokenValue(fileTokens, "<C>"));
                        track = GetFieldValue(track, GetTokenValue(fileTokens, "<#>"));
                        year = GetFieldValue(year, GetTokenValue(fileTokens, "<Y>"));
                    }
                }

                if (ProTONEConfig.UseMetadata || ProTONEConfig.UseFileNameFormat)
                {
                    // Format entries if any formatting rules are applied

                    retVal = ProTONEConfig.PlaylistEntryFormat;
                    StringUtils.ReplaceToken(ref retVal, "<A", artist ?? string.Empty);
                    StringUtils.ReplaceToken(ref retVal, "<B", album ?? string.Empty);
                    StringUtils.ReplaceToken(ref retVal, "<T", title ?? string.Empty);
                    StringUtils.ReplaceToken(ref retVal, "<G", genre ?? string.Empty);
                    StringUtils.ReplaceToken(ref retVal, "<C", comments ?? string.Empty);
                    StringUtils.ReplaceToken(ref retVal, "<#", track ?? string.Empty);
                    StringUtils.ReplaceToken(ref retVal, "<Y", year ?? string.Empty);
                }

                retVal = retVal.Trim();
                retVal = retVal.Trim(new char[] { '-' });
                retVal = retVal.Trim();

                if (string.IsNullOrEmpty(retVal))
                {
                    // Use file name
                    retVal = mi.Name;
                }

                return retVal;
            }
        }

        public virtual string Type
        {
            get
            {
                return mi.MediaType;
            }
        }

        public virtual string SubType
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual int TrackNumber
        {
            get
            {
                return (int)(mi.Track.GetValueOrDefault());
            }
        }

        public virtual TimeSpan Duration
        {
            get
            {
                TimeSpan ts = mi.Duration.GetValueOrDefault();
                return new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds);
            }

            set
            {
                mi.Duration = value;
            }
        }

        public virtual string Details
        {
            get
            {
                string retVal = string.Empty;
                if (MediaInfo != null)
                {
                    retVal = mi.Details;
                }

                return retVal;
            }
        }

        public virtual Dictionary<string, string> MediaInfo
        {
            get
            {
                Dictionary<string, string> info = new Dictionary<string, string>();

                const int maxLen = 90;

                if (!string.IsNullOrEmpty(mi.Name))
                {
                    info.Add("TXT_FILENAME:", StringUtils.Limit(mi.Name, maxLen));
                }

                if (!string.IsNullOrEmpty(mi.MediaType))
                {
                    info.Add("TXT_MEDIATYPE:", this.GetMediaTypeEx());
                }

                if (mi.ExtendedInfo != null && mi.ExtendedInfo.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in mi.ExtendedInfo)
                    {
                        try
                        {
                            info.Add(kvp.Key, StringUtils.Limit(kvp.Value, maxLen));
                        }
                        catch { }
                    }
                }

                return info;
            }
        }

        public virtual string ImageURL
        {
            get
            {
                string url = null;

                if (MediaFileInfo != null)
                    url = MediaFileInfo.ImageURL;

                if (string.IsNullOrEmpty(url))
                    url = _altImageUrl;

                return url;
            }
        }

        string _altImageUrl = null;

        public virtual void Rebuild()
        {
            // Query the Deezer API with params extracted from the media file info
            if (ProTONEConfig.DeezerHasValidConfig && ProTONEConfig.DeezerUseServicesForFileMetadata)
            {
                // First - parse the file name
                string name = mi.Name;
                if (!string.IsNullOrEmpty(mi.Extension))
                {
                    name = name.Replace(mi.Extension, string.Empty);
                }

                Dictionary<string, string> fileTokens = StringUtils.Tokenize(name, ProTONEConfig.FileNameFormat);

                DeezerJsonFilter filter = new DeezerJsonFilter
                {
                    Album = MediaFileInfo.Album,
                    Artist = MediaFileInfo.Artist,
                    Title = MediaFileInfo.Title
                };

                bool needsMediaFileInfoCorrection = false;

                // Second - replace formatting fields with data from file name where available
                if (fileTokens != null && fileTokens.Count > 0)
                {
                    if (string.IsNullOrEmpty(filter.Album))
                    {
                        filter.Album = GetTokenValue(fileTokens, "<A>");
                        needsMediaFileInfoCorrection = true;
                    }

                    if (string.IsNullOrEmpty(filter.Artist))
                    {
                        filter.Artist = GetTokenValue(fileTokens, "<B>");
                        needsMediaFileInfoCorrection = true;
                    }

                    if (string.IsNullOrEmpty(filter.Title))
                    {
                        filter.Title = GetTokenValue(fileTokens, "<T>");
                        needsMediaFileInfoCorrection = true;
                    }
                }

                if (string.IsNullOrEmpty(filter.Artist) &&
                    string.IsNullOrEmpty(filter.Album) == false)
                {
                    // Could be a file where these two fields are reversed
                    filter.Artist = filter.Album;
                }

                filter.Album = string.Empty;

                DeezerRuntime dzr = DeezerRuntimeFactory.GetRuntime();

                if (DeezerJsonFilter.IsNullOrEmpty(filter) == false)
                {
                start_search:
                    int i = 0;
                    var evt = new ManualResetEvent(false);

                    List<Track> tracks = dzr.ExecuteSearch(filter.SearchText, 1, evt);
                    if (tracks != null && tracks.Count > 0)
                    {
                        DeezerTrackItem dti = new DeezerTrackItem();
                        dti.LoadTrackData(tracks[0]);

                        _altImageUrl = dti.AlbumUriImageSmall;
                        if (string.IsNullOrEmpty(_altImageUrl))
                            _altImageUrl = dti.ArtistUriImageSmall;

                        if (needsMediaFileInfoCorrection)
                        {
                            mi.Album = dti.Album;
                            mi.Artist = dti.Artist;
                            mi.Title = dti.Title;

                            try
                            {
                                mi.Save();
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        // Lookup by artist only
                        filter.Album = string.Empty;
                        filter.Title = string.Empty;

                        tracks = dzr.ExecuteSearch(filter.SearchText, 1, evt);
                        if (tracks != null && tracks.Count > 0)
                        {
                            DeezerTrackItem dti = new DeezerTrackItem();
                            dti.LoadTrackData(tracks[0]);
                            _altImageUrl = dti.ArtistUriImageSmall;
                        }
                    }
                }
            }

        }

        public Image GetImage(bool large)
        {
            Image img = null;

            if (mi != null)
            {
                if (mi.IsDVDVolume)
                {
                    img = ImageProvider.GetShell32Icon(Shell32Icon.DvdDisk, large);
                }
                else switch (mi.MediaType.ToUpperInvariant())
                    {
                        case "URL":
                            img = ImageProvider.GetShell32Icon(Shell32Icon.URL, large);
                            break;

                        case "CDA":
                            img = ImageProvider.GetShell32Icon(Shell32Icon.CompactDisk, large);
                            break;

                        default:
                            img = ImageProvider.GetIcon(mi.Path, large);
                            break;
                    }
            }

            if (img == null)
                img = ImageProvider.GetShell32Icon(Shell32Icon.BlankFile, large);

            return img;
        }

        public virtual Dictionary<PlaylistSubItem, List<PlaylistSubItem>> GetSubmenu()
        {
            if (mi.Bookmarks != null && mi.Bookmarks.Count > 0)
            {
                return CreateBookmarksSubmenu();
            }
            else
            {
                if (mi is CDAFileInfo)
                {
                    return CreateAudioCdSubmenu();
                }

                // Don't insert anything.
                // This is a regular file item.
                return null;
            }
        }

        private Dictionary<PlaylistSubItem, List<PlaylistSubItem>> CreateAudioCdSubmenu()
        {
            CddaInfoSource src = ProTONEConfig.AudioCdInfoSource;
            if (src == CddaInfoSource.None)
                return null;

            Dictionary<PlaylistSubItem, List<PlaylistSubItem>> submenu =
                            new Dictionary<PlaylistSubItem, List<PlaylistSubItem>>();

            submenu.Add(new AudioCdSubItem(this, Translator.Translate("TXT_OPT_RELOAD_CDINFO")), null);

            return submenu;
        }

        private Dictionary<PlaylistSubItem, List<PlaylistSubItem>> CreateBookmarksSubmenu()
        {
            Dictionary<PlaylistSubItem, List<PlaylistSubItem>> submenu =
                            new Dictionary<PlaylistSubItem, List<PlaylistSubItem>>();

            PlaylistSubItem title = new BookmarkSubItem(this,
                Translator.Translate("TXT_BOOKMARKS"));

            List<PlaylistSubItem> bookmarks = new List<PlaylistSubItem>();

            if (mi.Bookmarks != null && mi.Bookmarks.Count > 0)
            {
                foreach (Bookmark bmk in mi.Bookmarks.Values)
                {
                    BookmarkSubItem bmkSubItem = new BookmarkSubItem(this, bmk);
                    bookmarks.Add(bmkSubItem);
                }
            }

            submenu.Add(title, bookmarks);

            return submenu;
        }

        public virtual void DeepLoad()
        {
            if (mi != null)
            {
                mi.DeepLoad();
            }
        }

        public virtual bool MoveToSubitem(PlaylistSubItem subItem)
        {
            // No subitems to move to ...
            return false;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }


        public PlaylistItem(string itemPath, bool deepLoad) : this(itemPath, false, deepLoad)
        {
        }

        public PlaylistItem(string itemPath, bool isDvd, bool deepLoad)
        {
            if (isDvd)
                mi = new VideoDvdInformation(itemPath);
            else
                mi = MediaFileInfo.FromPath(itemPath, deepLoad);
        }

        private string GetTokenValue(Dictionary<string, string> tokens, string token)
        {
            if (tokens != null && tokens.Count > 0 && tokens.ContainsKey(token))
                return tokens[token];

            return string.Empty;
        }

        private string GetFieldValue(string field1, string field2)
        {
            string retVal = field1;

            if (string.IsNullOrEmpty(retVal))
            {
                retVal = field2;
            }
            if (string.IsNullOrEmpty(retVal))
            {
                retVal = string.Empty;
            }

            return retVal;
        }

        protected virtual bool _SupportsTrackInfo()
        {
            return true;
        }
        protected virtual bool _SupportsBookmarkInfo()
        {
            return true;
        }

        protected virtual bool _IsTrackInfoEditable()
        {
            return IsOnAWritableDisk();
        }

        protected virtual bool _IsBookmarkInfoEditable()
        {
            return IsOnAWritableDisk();
        }

        protected bool IsOnAWritableDisk()
        {
            try
            {
                string rootPath = System.IO.Path.GetPathRoot(this.Path);
                DriveInfo di = new DriveInfo(rootPath);

                switch (di.DriveType)
                {
                    case DriveType.Fixed:
                    case DriveType.Ram:
                    case DriveType.Removable:
                        return true;
                }
            }
            catch { }
            return false;
        }
    }

    public class BoormarkEditablePlaylistItem : PlaylistItem
    {
        public BoormarkEditablePlaylistItem(string itemPath) : base(itemPath, false)
        {
        }

        public override string ToString()
        {
            int count = 0;
            if (base.MediaFileInfo.Bookmarks != null)
            {
                count = base.MediaFileInfo.Bookmarks.Count;
            }

            return Translator.Translate("TXT_BOOKMARK_COUNT", count);
        }
    }

    // This item appears in the drop down menu
    // that may be attached to a playlist item
    public abstract class PlaylistSubItem
    {
        protected string _name = string.Empty;
        public string Name { get { return _name; } }

        protected PlaylistItem _parent = null;
        public PlaylistItem Parent
        { get { return _parent; } }

        protected RenderingStartHint _hint = null;
        public RenderingStartHint StartHint
        { get { return _hint; } }

        public virtual string ParentMediaPath
        {
            get
            {
                return _parent.Path;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public PlaylistSubItem(string name, PlaylistItem parent)
        {
            _name = name;
            _parent = parent;
        }
    }

    public static class Extensions
    {
        public static string GetMediaTypeEx(this PlaylistItem pli)
        {
            string str = string.Empty;

            if (pli != null)
            {
                string type = pli.Type.ToUpperInvariant();
                string subType = pli.SubType;

                str = type;
                if (string.IsNullOrEmpty(subType) == false)
                    str += string.Format(" ({0})", subType);
            }

            return str;
        }
    }
}

