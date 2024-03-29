using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.FileInformation;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.ExtendedInfo;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TagLib;
using TagLib.Mpeg;

namespace OPMedia.Runtime.ProTONE.FileInformation
{
    public class ID3FileInfo : MediaFileInfo, ITaggedMediaFileInfo
    {
        TagLib.Mpeg.AudioFile af = null;
        ID3ArtworkInfo artworkInfo = null;

        Tag _tag = null;
        Properties _prop = null;

        static string[] _audioGenres = null;
        static object _genresLock = new object();

        bool _tagModified = false;

        public static new ID3FileInfo Empty
        {
            get
            {
                ID3FileInfo empty = new ID3FileInfo(null, false);
                empty._tag = new TagLib.Id3v2.Tag();
                return empty;
            }
        }

        public static string[] AudioGenres
        {
            get
            {
                if (_audioGenres == null)
                {
                    lock (_genresLock)
                    {
                        if (_audioGenres == null)
                        {
                            List<string> genres = new List<string>();
                            genres.AddRange(Genres.Audio);
                            genres.Sort();

                            _audioGenres = genres.ToArray();
                        }
                    }
                }

                return _audioGenres;
            }
        }

        [Browsable(false)]
        public bool HasTag
        {
            get
            {
                //return (af != null && 
                //    ((af.TagTypesOnDisk & TagTypes.Id3v1) == TagTypes.Id3v1 ||
                //    (af.TagTypesOnDisk & TagTypes.Id3v2) == TagTypes.Id3v2));

                //return (af != null && _tag != null);

                return (_tag != null);
            }
        }

        [TranslatableDisplayName("TXT_ARTIST")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        public override string Artist
        {
            get { return HasTag ? BuildCleanString(_tag.FirstPerformer) : null; }
            set
            {
                try
                {
                    _tag.Performers = new string[] { value };
                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [TranslatableDisplayName("TXT_ALBUM")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        public override string Album
        {
            get { return HasTag ? BuildCleanString(_tag.Album) : null; }
            set
            {
                try
                {
                    _tag.Album = value;
                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [TranslatableDisplayName("TXT_TITLE")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        public override string Title
        {
            get { return HasTag ? BuildCleanString(_tag.Title) : null; }
            set
            {
                try
                {
                    _tag.Title = value;
                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [TranslatableDisplayName("TXT_COMMENTS")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor)), Localizable(true)]
        public override string Comments
        {
            get { return HasTag ? BuildCleanString(_tag.Comment) : null; }
            set
            {
                try
                {
                    _tag.Comment = value;
                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [TranslatableDisplayName("TXT_GENRE")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        [Editor("OPMedia.Runtime.ProTONE.GenrePropertyBrowser, OPMedia.Runtime.ProTONE", typeof(UITypeEditor))]
        public override string Genre
        {
            get { return HasTag ? BuildCleanString(_tag.FirstGenre) : null; }
            set
            {
                try
                {
                    _tag.Genres = new string[] { value };
                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [TranslatableDisplayName("TXT_TRACK")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        [DefaultValue((short)1)]
        public override short? Track
        {
            get
            {
                short? retVal = new Nullable<short>();
                if (HasTag && _tag.Track > 0 && _tag.Track < 255)
                {
                    retVal = (short)_tag.Track;
                }

                return retVal;
            }

            set
            {
                try
                {
                    if (value.HasValue && value.Value > 0 && value.Value < 255)
                    {
                        _tag.Track = (uint)value.GetValueOrDefault();
                    }
                    else
                    {
                        _tag.Track = 0;
                    }

                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [TranslatableDisplayName("TXT_YEAR")]
        [TranslatableCategory("TXT_TAGINFO")]
        [Browsable(true)]
        public override short? Year
        {
            get
            {
                short? retVal = new Nullable<short>();
                if (HasTag && _tag.Year > 1000 && _tag.Year < 9999)
                {
                    retVal = (short)_tag.Year;
                }

                return retVal;
            }

            set
            {
                try
                {
                    if (value.HasValue && value.Value > 1000 && value.Value < 9999)
                    {
                        _tag.Year = (uint)value.GetValueOrDefault();
                    }
                    else
                    {
                        _tag.Year = 0;
                    }

                    _tagModified = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        [Browsable(false)]
        public override Dictionary<string, string> ExtendedInfo
        {
            get
            {
                Dictionary<string, string> info = new Dictionary<string, string>();

                info.Add("TXT_DURATION:", Duration.GetValueOrDefault().ToString());
                info.Add("TXT_BITRATE", Bitrate.GetValueOrDefault().ToString());
                info.Add("TXT_CHANNELS:", Channels.GetValueOrDefault().ToString());
                info.Add("TXT_FREQUENCY:", Frequency.GetValueOrDefault().ToString());

                if (HasTag)
                {
                    info.Add(string.Empty, null); // separator
                    bool removeSep = true;

                    if (!string.IsNullOrEmpty(Album))
                    {
                        removeSep = false;
                        info.Add("TXT_ALBUM:", Album);
                    }
                    if (!string.IsNullOrEmpty(Artist))
                    {
                        removeSep = false;
                        info.Add("TXT_ARTIST:", Artist);
                    }
                    if (!string.IsNullOrEmpty(Title))
                    {
                        removeSep = false;
                        info.Add("TXT_TITLE:", Title);
                    }
                    if (!string.IsNullOrEmpty(Genre))
                    {
                        removeSep = false;
                        info.Add("TXT_GENRE:", Genre);
                    }
                    if (!string.IsNullOrEmpty(Comments))
                    {
                        removeSep = false;
                        info.Add("TXT_COMMENTS:", Comments);
                    }
                    if (Track.HasValue)
                    {
                        removeSep = false;
                        info.Add("TXT_TRACK:", Track.GetValueOrDefault().ToString());
                    }
                    if (Year.HasValue)
                    {
                        removeSep = false;
                        info.Add("TXT_YEAR:", Year.GetValueOrDefault().ToString());
                    }

                    if (removeSep)
                    {
                        info.Remove(string.Empty);
                    }
                }

                return info;
            }
        }

        [TranslatableDisplayName("TXT_BITRATE")]
        [TranslatableCategory("TXT_MEDIAINFO")]
        [ReadOnly(true)]
        [Browsable(true)]
        public override Bitrate? Bitrate
        {
            get
            {
                if (AudioHeader != null)
                    return new Bitrate((short)AudioHeader.Value.AudioBitrate,
                        AudioHeader.Value.VBRIHeader.Present || AudioHeader.Value.XingHeader.Present);
                else if (_prop != null)
                    return new Bitrate((short)_prop.AudioBitrate, false);
                else
                    return null;
            }
        }

        [TranslatableDisplayName("TXT_CHANNELS")]
        [TranslatableCategory("TXT_MEDIAINFO")]
        [ReadOnly(true)]
        [Browsable(true)]
        public override ChannelMode? Channels
        {
            get
            {
                if (AudioHeader != null)
                {
                    try
                    {
                        // This is an ugly hack but we do this to avoid making changes in the code of TagLib#.
                        // Refer to http://www.mp3-tech.org/programmer/frame_header.html
                        // The bitwise structure of the frame header is: AAAAAAAAAAABBCCDEEEEFFGHIIJJKLMM
                        // Channel mode is represented by the II bits so we need to right-shift with 6 bits.
                        // The original source code of the library is right-shifting with 14 bits which is not correct.

                        FieldInfo fi = typeof(AudioHeader).GetField("flags", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (fi != null)
                        {
                            // Get the flags bits exactly as read from the file
                            uint flags = (uint)fi.GetValue(AudioHeader.Value);

                            // Right-shifty with 6 bits and bitwise mask with 0x03 = (11) to get the two II bits for channel mode.
                            return (ChannelMode)((flags >> 6) & 0x03);
                        }
                    }
                    catch { }

                    return AudioHeader.Value.ChannelMode;
                }

                if (_prop != null)
                    return (_prop.AudioChannels == 1) ? ChannelMode.SingleChannel : ChannelMode.Stereo;

                return ChannelMode.SingleChannel;
            }
        }

        [TranslatableDisplayName("TXT_DURATION")]
        [TranslatableCategory("TXT_MEDIAINFO")]
        [ReadOnly(true)]
        [Browsable(true)]
        public override TimeSpan? Duration
        {
            get
            {
                if (AudioHeader != null)
                    return TimeSpan.FromSeconds((int)AudioHeader.Value.Duration.TotalSeconds);
                else if (_prop != null)
                    return TimeSpan.FromSeconds((int)_prop.Duration.TotalSeconds);
                else
                    return null;
            }
        }

        [TranslatableDisplayName("TXT_FREQUENCY")]
        [TranslatableCategory("TXT_MEDIAINFO")]
        [ReadOnly(true)]
        [Browsable(true)]
        public override int? Frequency
        {
            get
            {
                if (AudioHeader != null)
                    return AudioHeader.Value.AudioSampleRate;
                else if (_prop != null)
                    return _prop.AudioSampleRate;
                else
                    return null;
            }
        }


        [SingleSelectionBrowsable]
        [Editor("OPMedia.UI.ProTONE.Dialogs.ID3ArtworkPropertyBrowser, OPMedia.UI.ProTONE", typeof(UITypeEditor))]
        [TranslatableDisplayName("TXT_ARTWORK")]
        [TranslatableCategory("TXT_EXTRAINFO")]
        public ID3ArtworkInfo ArtworkInfo
        {
            get
            {
                if (artworkInfo == null)
                {
                    artworkInfo = new ID3ArtworkInfo(af);
                }

                return artworkInfo;
            }

            set
            {
                artworkInfo = value;
                artworkInfo.Save();

                _tagModified = true;
            }
        }

        public override string ImageURL
        {
            get
            {
                Bitmap bmp = null;

                try
                {
                    if (ArtworkInfo != null &&
                        ArtworkInfo.ArtworkImages != null &&
                        ArtworkInfo.ArtworkImages.Count > 0)
                    {
                        bmp = ArtworkInfo.ArtworkImages[0].Picture;
                        if (bmp != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bmp.Save(ms, ImageFormat.Png);
                                byte[] imgBytes = ms.ToArray();
                                string imgBase64 = Convert.ToBase64String(imgBytes);
                                return $"base64:{imgBase64}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

                return null;
            }
        }

        object _saveLock = new object();

        public override void Save()
        {
            lock (_saveLock)
            {
                if (_tag != null && _tagModified)
                {
                    double resumePosition = -1;
                    if (this.Equals(RenderingEngine.DefaultInstance.RenderedMediaInfo))
                    {
                        RenderingEngine.DefaultInstance.PauseRenderer();
                        resumePosition = RenderingEngine.DefaultInstance.MediaPosition;
                        RenderingEngine.DefaultInstance.StopRenderer(true);
                        Thread.Sleep(100);
                    }

                    if (IsEmpty)
                    {
                        af.RemoveTags(TagTypes.AllTags);
                    }


                    try
                    {
                        af.Save();
                        _tagModified = false;
                    }
                    finally
                    {
                        if (resumePosition > 0)
                        {
                            BookmarkStartHint hint = new BookmarkStartHint(new Bookmark("default", (int)resumePosition));
                            RenderingEngine.DefaultInstance.StartRendererWithHint(hint);
                            RenderingEngine.DefaultInstance.AudioVolume = ProTONEConfig.LastVolume;
                        }
                    }
                }
            }
        }

        public override void Clear()
        {
            if (af != null)
            {
                af.RemoveTags(TagTypes.AllTags);
                _tagModified = true;
            }
        }

        public override void DeepLoad()
        {
            // Read the audio header if not already done
            if (!_deepLoad)
            {
                if (af != null && af.Properties != null)
                {
                    // This will actually toggle reading the audio header
                    TimeSpan duration = af.Properties.Duration;
                    _deepLoad = true;
                }
            }
        }

        public override void Rebuild(bool deepLoad)
        {
            _tag = null;

            if (IsValid)
            {
                try
                {
                    af = new TagLib.Mpeg.AudioFile(base.Path, ReadStyle.Average);

                    if (deepLoad)
                    {
                        // This will actually toggle reading the audio header
                        TimeSpan duration = af.Properties.Duration;
                        _deepLoad = true;
                    }

                    _tag = af.Tag;
                    _prop = af.Properties;
                }
                catch
                {
                }

                _tagModified = false;
            }
        }

        private bool _deepLoad = false;
        public ID3FileInfo(string path, bool deepLoad)
            : base(path, false)
        {
            Rebuild(deepLoad);
        }

        private string BuildCleanString(string input)
        {
            if (input != null)
            {
                return input.Trim().Trim("\0".ToCharArray());
            }

            return string.Empty;
        }

        private AudioHeader? AudioHeader
        {
            get
            {
                try
                {
                    if (_prop != null)
                    {
                        List<ICodec> codecs = new List<ICodec>(_prop.Codecs);
                        return (AudioHeader)codecs[0];
                    }
                }
                catch
                {
                }

                return null;
            }
        }

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return (string.IsNullOrEmpty(Album) &&
                        string.IsNullOrEmpty(Artist) &&
                        string.IsNullOrEmpty(Comments) &&
                        string.IsNullOrEmpty(Genre) &&
                        string.IsNullOrEmpty(Title) &&
                        !(Track.HasValue && Track.Value > 0 && Track.Value < 255) &&
                        !(Year.HasValue && Year.Value > 1000 && Track.Value < 9999));

            }
        }
    }

    public struct Bitrate
    {
        short _b;
        bool _isVbr;

        public Bitrate(short b, bool isVbr)
        {
            this = new Bitrate();
            _b = b;
            _isVbr = isVbr;
        }

        public override string ToString()
        {
            return _b.ToString() + " Kb/s, " + (string)(_isVbr ? "VBR" : "CBR");
        }
    }
}

namespace OPMedia.Runtime.ProTONE
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class GenrePropertyBrowser : UITypeEditor
    {
        public GenrePropertyBrowser()
            : base()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                // Display an angle selection control and retrieve the value.
                ListBox lb = new ListBox();
                lb.BorderStyle = BorderStyle.None;
                lb.Dock = DockStyle.Fill;
                lb.Height = 250;
                lb.Tag = edSvc;

                lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);

                lb.Items.Add(string.Empty); // no genre
                foreach (string gi in ID3FileInfo.AudioGenres)
                {
                    lb.Items.Add(gi);
                }

                lb.SelectedIndex = lb.FindString(value as string);

                edSvc.DropDownControl(lb);

                return (string)lb.SelectedItem;

            }

            return value;

        }

        void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                IWindowsFormsEditorService edSvc = (sender as Control).Tag as IWindowsFormsEditorService;
                if (edSvc != null)
                {
                    edSvc.CloseDropDown();
                }
            }
        }
    }
}
