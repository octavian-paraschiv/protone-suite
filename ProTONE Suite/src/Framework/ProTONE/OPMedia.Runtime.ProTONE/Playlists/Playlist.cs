#region Copyright � 2008 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	Playlist.cs
#endregion

#region Using directives
using Newtonsoft.Json;
using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.Runtime.ProTONE.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#endregion

namespace OPMedia.Runtime.ProTONE.Playlists
{
    public enum UpdateType
    {
        None = 0,
        Added,
        Removed,
        Swapped,
        ActiveChanged,
        Shuffled,
        Cleared,
    }

    public delegate void PlaylistUpdatedEventHandler(int item1, int item2, UpdateType updateType);

    public class Playlist : List<PlaylistItem>
    {
        #region Members
        private int playIndex = 0;
        public event PlaylistUpdatedEventHandler PlaylistUpdated = null;
        #endregion

        #region Properties
        public virtual int PlayIndex
        { get { return playIndex; } }

        public virtual double TotalPlaylistTime
        {
            get
            {
                double retVal = 0;
                foreach (PlaylistItem item in this.AsReadOnly())
                {
                    retVal += item.Duration.TotalSeconds;
                }

                return retVal;
            }
        }

        public virtual PlaylistItem ActivePlaylistItem
        {
            get
            {
                if (Count > 0 && playIndex >= 0 && playIndex < Count)
                {
                    return this[playIndex];
                }

                return null;
            }
        }

        public bool IsAtEnd
        {
            get
            {
                // When playing in loop, the playlist is never at the end
                if (ProTONEConfig.LoopPlay)
                    return false; // If playing in loop, 

                return (Count > 0 && playIndex == Count - 1);
            }
        }

        public List<PlaylistItem> AllItems
        {
            get
            {
                return new List<PlaylistItem>(this.AsReadOnly());
            }
        }
        #endregion

        #region Methods

        #region Shuffle playlist routine

        public void ShufflePlaylist()
        {
            Random rnd = new Random();

            for (int i = 0; i < this.Count; i++)
            {
                int idx1 = i;
                int idx2 = rnd.Next(i, this.Count);
                SwapItems(idx1, idx2, false);
            }

            EventRaiser(-1, -1, UpdateType.Shuffled);
        }
        #endregion


        public bool MoveToItem(PlaylistItem plItem)
        {
            if (plItem == ActivePlaylistItem)
            {
                return true;
            }

            int i = 0;
            for (i = 0; i < Count; i++)
            {
                if (this[i] == plItem)
                {
                    break;
                }
            }

            if (i < Count && playIndex != i)
            {
                playIndex = i;
            }

            return (playIndex == i);
        }

        public void ClearAll()
        {
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }

            EventRaiser(-1, -1, UpdateType.Cleared);

            playIndex = 0;

            RenderingEngine.DefaultInstance.PlaylistAtEnd = false;


        }

        public virtual PlaylistItem GetNext()
        {
            if (Count > 0 && playIndex < Count - 1)
                return this[playIndex + 1];

            if (Count > 0 && playIndex == Count - 1 && ProTONEConfig.LoopPlay)
                return this[0];

            return null;
        }

        public virtual bool MoveNext()
        {
            if (Count > 0 && playIndex < Count - 1)
            {
                playIndex++;
                return true;
            }

            if (Count > 0 && playIndex == Count - 1 && ProTONEConfig.LoopPlay)
            {
                playIndex = 0;
                return true;
            }

            return false;
        }

        public virtual bool MovePrevious()
        {
            if (Count > 0 && playIndex > 0)
            {
                playIndex--;
                return true;
            }

            if (Count > 0 && playIndex == 0 && ProTONEConfig.LoopPlay)
            {
                playIndex = Count - 1;
                return true;
            }

            return false;
        }

        public virtual PlaylistItem GetPrevious()
        {
            if (Count > 0 && playIndex > 0)
                return this[playIndex - 1];

            if (Count > 0 && playIndex == 0 && ProTONEConfig.LoopPlay)
                return this[Count - 1];

            return null;
        }

        public virtual bool MoveToItem(int item)
        {
            if (Count > 0 && item < Count && item >= 0)
            {
                playIndex = item;
                return true;
            }

            return false;
        }

        public virtual bool SwapItems(int item1, int item2, bool raiseSwapEvent = true)
        {
            if (item1 >= 0 && item2 >= 0 &&
                item1 != item2 && item1 < Count &&
                item2 < Count)
            {
                PlaylistItem playedItem = null;
                if (playIndex >= 0)
                {
                    playedItem = this[playIndex];
                }

                PlaylistItem item = this[item1];
                this[item1] = this[item2];
                this[item2] = item;

                for (int i = 0; i < Count; i++)
                {
                    if (this[i] == playedItem)
                    {
                        playIndex = i;
                        break;
                    }
                }

                if (raiseSwapEvent)
                    EventRaiser(item1, item2, UpdateType.Swapped);

                return true;
            }

            return false;
        }

        public virtual bool ShiftItems(List<int> itemIndexes, bool shiftUp)
        {
            int offset = shiftUp ? -1 : 1;

            // Make sure all indexes are in ascending order
            itemIndexes.Sort();

            // Check if all items can be moved
            foreach (int i in itemIndexes)
            {
                if (i + offset < 0) return false; // Can't move 
                if (i + offset >= Count) return false; // Can't move 
            }

            if (shiftUp)
            {
                for (int j = 0; j < itemIndexes.Count; j++)
                {
                    SwapItems(itemIndexes[j], itemIndexes[j] - 1);
                }
            }
            else
            {
                for (int j = itemIndexes.Count - 1; j >= 0; j--)
                {
                    SwapItems(itemIndexes[j], itemIndexes[j] + 1);
                }
            }

            return true;
        }

        public void AddOnlineMediaItem(OnlineMediaItem omi, int delayStart = 0)
        {
            try
            {
                PlaylistItem pli = null;

                if (omi is RadioStation)
                    pli = new RadioStationPlaylistItem(omi as RadioStation);
                else if (omi is DeezerTrackItem)
                    pli = new DeezerTrackPlaylistItem(omi as DeezerTrackItem);

                pli.DelayStart = delayStart;

                Add(pli);

                EventRaiser(Count - 1, -1, UpdateType.Added);
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, true);
            }
            finally
            {
                Application.DoEvents();
            }
        }

        public void AddItem(PlaylistItem pli)
        {
            if (pli != null)
            {
                base.Add(pli);
                EventRaiser(Count - 1, -1, UpdateType.Added);
            }
        }

        public virtual void AddItem(string itemPath, int delayStart = 0)
        {
            try
            {
                bool added = false;
                Uri uri = null;
                try
                {
                    uri = new Uri(itemPath, UriKind.Absolute);
                }
                catch
                {
                    uri = null;
                }

                if (uri != null)
                {
                    if (uri.IsFile)
                    {
                        itemPath = uri.LocalPath;
                    }
                    else
                    {
                        var pli = new UrlPlaylistItem(itemPath);
                        pli.DelayStart = delayStart;
                        Add(pli);

                        added = true;
                    }
                }

                if (!added)
                {
                    PlaylistItem pli = null;
                    if (DvdMedia.FromPath(itemPath) != null)
                        pli = new DvdPlaylistItem(itemPath);
                    else
                        pli = new PlaylistItem(itemPath, false);

                    pli.DelayStart = delayStart;
                    Add(pli);
                }

                EventRaiser(Count - 1, -1, UpdateType.Added);
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, true);
            }
            finally
            {
                Application.DoEvents();
            }
        }

        public virtual void RemoveItem(int item)
        {
            RemoveAt(item);

            if (item <= playIndex)
                playIndex--;

            if (playIndex < 0)
                playIndex = 0;
            if (playIndex >= Count)
                playIndex = Count - 1;

            EventRaiser(item, -1, UpdateType.Removed);
        }

        public virtual void RemoveItems(IEnumerable<PlaylistItem> items)
        {
            foreach (PlaylistItem item in items)
            {
                int index = this.IndexOf(item);
                if (index >= 0 && index < this.Count)
                {
                    RemoveItem(index);
                }
            }
        }

        public virtual void AddItems(IEnumerable<string> itemPaths)
        {
            foreach (string itemPath in itemPaths)
            {
                if (_abortLoad)
                    break;

                AddItem(itemPath);
            }
        }

        public virtual void SavePlaylist(string fileName)
        {
            string ext = PathUtils.GetExtension(fileName);
            switch (ext)
            {
                case "m3u":
                    SaveM3UPlaylist(fileName);
                    break;
                case "pls":
                    SavePLSPlaylist(fileName);
                    break;
                case "asx":
                    SaveASXPlaylist(fileName);
                    break;
                case "wpl":
                    SaveWPLPlaylist(fileName);
                    break;

                default:
                    throw new ArgumentException("Invalid playlist type: " + ext);
            }
        }

        public virtual void LoadPlaylist(string fileName)
        {
            _abortLoad = false;

            try
            {
                string ext = PathUtils.GetExtension(fileName);
                switch (ext)
                {
                    case "m3u":
                        LoadM3UPlaylist(fileName);
                        break;
                    case "pls":
                        LoadPLSPlaylist(fileName);
                        break;
                    case "asx":
                        LoadASXPlaylist(fileName);
                        break;
                    case "wpl":
                        LoadWPLPlaylist(fileName);
                        break;

                    default:
                        throw new ArgumentException("Invalid playlist type: " + ext);
                }
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, true);
            }
            finally
            {
                RenderingEngine.DefaultInstance.PlaylistAtEnd = false;
            }
        }

        #endregion

        #region Construction
        public Playlist()
        {
            playIndex = 0;
        }
        #endregion

        #region Implementation

        private void EventRaiser(int item1, int item2, UpdateType updateType)
        {
            if (PlaylistUpdated != null)
            {
                PlaylistUpdated(item1, item2, updateType);
            }
        }

        private string GetAbsoluteItemPath(string itemLine, string playlistFileName)
        {
            string itemPath = Path.GetDirectoryName(itemLine);
            if (string.IsNullOrEmpty(itemPath))
            {
                try
                {
                    string playlistPath = Path.GetDirectoryName(playlistFileName);
                    itemLine = Path.Combine(playlistPath, itemLine);
                }
                catch { }
            }
            else
            {
                try
                {
                    itemLine = Path.GetFullPath(itemLine);
                }
                catch { }
            }

            return itemLine;
        }

        private void WriteXmlElement(XmlWriter writer, string elementName, string[] attributeNames, string[] attributeValues)
        {
            writer.WriteStartElement(elementName);
            {
                int count = Math.Min(attributeNames.Length, attributeValues.Length);
                for (int i = 0; i < count; i++)
                {
                    writer.WriteStartAttribute(attributeNames[i]);
                    writer.WriteValue(attributeValues[i]);
                    writer.WriteEndAttribute();
                }
            }
            writer.WriteEndElement();
        }

        #region M3U Playlists

        private void SaveM3UPlaylist(string fileName)
        {
            StreamWriter sw = null;

            try
            {
                string s = SaveM3UPlaylistAsString();
                File.WriteAllText(fileName, s);
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public string SaveM3UPlaylistAsString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#EXTM3U");
            foreach (PlaylistItem item in this.AsReadOnly())
            {
                if (item is RadioStationPlaylistItem)
                    sb.AppendLine($"#RADIO:JSON:{item.PersistentPlaylistName}");
                else if (item is DeezerTrackPlaylistItem)
                    sb.AppendLine($"#DZMEDIA:JSON:{item.PersistentPlaylistName}");
                else
                    sb.AppendLine($"#EXTINF:{item.Duration.TotalSeconds},{item.PersistentPlaylistName}");

                var path = item.Path;

                //if (item.DelayStart > 0)
                //  path += $">{item.DelayStart}";

                sb.AppendLine(path);
            }

            return sb.ToString();
        }

        private void LoadM3UPlaylist(string fileName)
        {
            try
            {
                string text = File.ReadAllText(fileName);
                if (string.IsNullOrEmpty(text) == false)
                    LoadM3UPlaylistFromString(text, fileName);
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
        }

        public void LoadM3UPlaylistFromString(string content, string playlistFileName = "")
        {
            try
            {
                OnlineMediaItem omi = null;

                string[] lines = content.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (lines != null)
                {
                    foreach (string line in lines)
                    {
                        if (_abortLoad)
                            break;

                        if (line.StartsWith("#"))
                        {
                            try
                            {
                                if (line.StartsWith("#RADIO:"))
                                {
                                    if (line.StartsWith("#RADIO:JSON:"))
                                    {
                                        string s = line.Replace("#RADIO:JSON:", "").Trim();
                                        omi = JsonConvert.DeserializeObject<RadioStation>(s);
                                    }
                                    else
                                    {
                                        string s = line.Replace("#RADIO:", "").Trim();
                                        string[] fields = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (fields != null && fields.Length > 1)
                                        {
                                            omi = new RadioStation(OnlineMediaSource.ShoutCast);
                                            omi.Title = fields[1];
                                        }
                                    }
                                }
                                else if (line.StartsWith("#DZMEDIA:"))
                                {
                                    if (line.StartsWith("#DZMEDIA:JSON:"))
                                    {
                                        string s = line.Replace("#DZMEDIA:JSON:", "").Trim();
                                        omi = JsonConvert.DeserializeObject<DeezerTrackItem>(s);
                                    }
                                    else
                                    {
                                        string s = line.Replace("#DZMEDIA:", "").Trim();
                                        int idx = s.IndexOf(',');
                                        if (idx > 0)
                                        {
                                            string durStr = s.Substring(0, idx);

                                            DeezerTrackItem dti = new DeezerTrackItem();

                                            int sec = 0;
                                            int.TryParse(durStr, out sec);
                                            dti.Duration = TimeSpan.FromSeconds(sec);

                                            if (idx < (s.Length - 2))
                                            {
                                                string[] fields2 = s.Substring(idx + 1).Split("`".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                int i = 0;

                                                dti.Artist = fields2.Field(i++);
                                                dti.Title = fields2.Field(i++);
                                                dti.Album = fields2.Field(i++);
                                                dti.ReleaseDate = fields2.Field(i++);
                                                dti.AlbumUriImageSmall = fields2.Field(i++);
                                                dti.AlbumUriImageLarge = fields2.Field(i++);
                                                dti.ArtistUriImageSmall = fields2.Field(i++);
                                                dti.ArtistUriImageLarge = fields2.Field(i++);
                                            }

                                            omi = dti;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex);
                            }
                        }
                        else
                        {
                            // This is a path line
                            var path = line;
                            int delayStart = 0;

                            if (line.Contains(">"))
                            {
                                // The line contains a delay start indication
                                string[] fields = line.Split('>');
                                path = fields[0];
                                int.TryParse(fields[1], out delayStart);
                            }

                            if (omi != null)
                            {
                                omi.Url = path;
                                AddOnlineMediaItem(omi, delayStart);
                            }
                            else
                                AddItem(GetAbsoluteItemPath(path, playlistFileName), delayStart);

                            omi = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PLS playlists

        private void SavePLSPlaylist(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            int actualCount = 0;
            for (int i = 1; i < this.Count; i++)
            {
                PlaylistItem item = this.AsReadOnly()[i - 1];
                if (item != null)
                {
                    string fileKey = string.Format("File{0}", i);
                    string titleKey = string.Format("Title{0}", i);
                    string lengthKey = string.Format("Length{0}", i);

                    Kernel32.WritePrivateProfileString("playlist", fileKey, item.Path, fileName);
                    Kernel32.WritePrivateProfileString("playlist", titleKey, item.DisplayName, fileName);
                    Kernel32.WritePrivateProfileString("playlist", lengthKey,
                        item.Duration.TotalSeconds.ToString(), fileName);

                    actualCount++;
                }
            }

            Kernel32.WritePrivateProfileString("playlist", "NumberOfEntries", actualCount.ToString(), fileName);
            Kernel32.WritePrivateProfileString("playlist", "Version", "2", fileName);
        }

        private void LoadPLSPlaylist(string fileName)
        {
            int count = (int)Kernel32.GetPrivateProfileInt("playlist", "NumberOfEntries", 0, fileName);
            for (int i = 1; i <= count; i++)
            {
                if (_abortLoad)
                    break;

                string key = string.Format("File{0}", i);
                StringBuilder sb = new StringBuilder(Kernel32.MAX_PATH);
                if (Kernel32.GetPrivateProfileString("playlist", key, string.Empty, sb, Kernel32.MAX_PATH, fileName) > 0)
                {
                    string line = sb.ToString();
                    AddItem(GetAbsoluteItemPath(line, fileName));
                }

            }
        }

        #endregion

        #region WPL playlists

        private void SaveWPLPlaylist(string fileName)
        {
            XmlWriter xw = null;
            StreamWriter sw = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.NewLineHandling = NewLineHandling.Entitize;
                settings.OmitXmlDeclaration = true;
                settings.NewLineChars = "\r\n";
                settings.Indent = true;
                settings.IndentChars = "    ";
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.CloseOutput = true;

                StringBuilder sb = new StringBuilder();
                xw = XmlWriter.Create(sb, settings);
                {
                    xw.WriteStartElement("smil");
                    {
                        xw.WriteStartElement("head");
                        {
                            string[] names = new string[] { "name", "content" };
                            string[] values = new string[] { "Generator", string.Format("{0} v.{1}", Constants.PlayerName, SuiteVersion.Version) };
                            WriteXmlElement(xw, "meta", names, values);

                            values = new string[] { "AverageRating", "0" };
                            WriteXmlElement(xw, "meta", names, values);

                            int totalDuration = 0;
                            foreach (PlaylistItem item in this.AsReadOnly())
                            {
                                totalDuration += (int)item.Duration.TotalSeconds;
                            }

                            values = new string[] { "TotalDuration", totalDuration.ToString() };
                            WriteXmlElement(xw, "meta", names, values);

                            values = new string[] { "ItemCount", this.Count.ToString() };
                            WriteXmlElement(xw, "meta", names, values);
                        }
                        xw.WriteEndElement();

                        xw.WriteStartElement("body");
                        {
                            xw.WriteStartElement("seq");
                            {
                                foreach (PlaylistItem item in this.AsReadOnly())
                                {
                                    string[] names = new string[] { "src" };
                                    string[] values = new string[] { item.Path };
                                    WriteXmlElement(xw, "media", names, values);
                                }
                            }
                            xw.WriteEndElement();
                        }
                        xw.WriteEndElement();

                    }
                    xw.WriteEndElement();
                }

                xw.Flush();

                sw = new StreamWriter(fileName);
                sw.WriteLine("<?wpl version=\"1.0\"?>");
                sw.Write(sb.ToValidXml());
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }

                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        private void LoadWPLPlaylist(string fileName)
        {
            string docText = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                docText = sr.ReadToEnd();
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(docText.ToLowerInvariant());

            XmlNodeList nodes = doc.SelectNodes("/smil/body/seq/media");
            foreach (XmlNode node in nodes)
            {
                if (_abortLoad)
                    break;

                string line = node.Attributes["src"].Value;
                AddItem(GetAbsoluteItemPath(line, fileName));
            }
        }

        #endregion

        #region ASX playlists

        private void SaveASXPlaylist(string fileName)
        {
            XmlWriter xw = null;
            StreamWriter sw = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.NewLineHandling = NewLineHandling.Entitize;
                settings.OmitXmlDeclaration = true;
                settings.NewLineChars = "\r\n";
                settings.Indent = true;
                settings.IndentChars = "    ";
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.CloseOutput = true;

                StringBuilder sb = new StringBuilder();
                xw = XmlWriter.Create(sb, settings);
                {
                    xw.WriteStartElement("asx");
                    xw.WriteStartAttribute("version");
                    xw.WriteValue("3.0");
                    xw.WriteEndAttribute();
                    {
                        foreach (PlaylistItem item in this.AsReadOnly())
                        {
                            xw.WriteStartElement("entry");
                            {
                                xw.WriteStartElement("Duration");
                                xw.WriteStartAttribute("value");
                                xw.WriteValue(item.Duration.ToString());
                                xw.WriteEndAttribute();
                                xw.WriteEndElement();

                                string[] names = new string[] { "Name", "Value" };
                                string[] values = new string[] { "FileSize", item.MediaFileInfo.Size.ToString() };
                                WriteXmlElement(xw, "Param", names, values);

                                values = new string[] { "FileType", item.MediaFileInfo.Extension.ToLowerInvariant() };
                                WriteXmlElement(xw, "Param", names, values);

                                names = new string[] { "href" };
                                values = new string[] { item.Path };
                                WriteXmlElement(xw, "ref", names, values);

                            }
                            xw.WriteEndElement();
                        }
                    }
                    xw.WriteEndElement();
                }

                xw.Flush();

                sw = new StreamWriter(fileName);
                sw.Write(sb.ToValidXml());
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }

                if (sw != null)
                {
                    sw.Close();
                }
            }

        }

        private void LoadASXPlaylist(string fileName)
        {
            string docText = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                docText = sr.ReadToEnd();
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(docText.ToLowerInvariant());

            XmlNodeList nodes = doc.SelectNodes("/asx/entry/ref");
            foreach (XmlNode node in nodes)
            {
                if (_abortLoad)
                    break;

                string line = node.Attributes["href"].Value;
                AddItem(GetAbsoluteItemPath(line, fileName));
            }
        }

        #endregion

        #endregion

        bool _abortLoad = false;
        internal void AbortLoad()
        {
            _abortLoad = true;
        }
    }
}
