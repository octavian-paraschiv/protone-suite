using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Playlists;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    [Flags]
    public enum OnlineMediaSource
    {
        None = 0x00,

        Internal = 0x01,
        ShoutCast = 0x02,
        Deezer = 0x04,

        All = 0xFF,
    }

    [DataContract]
    [Serializable]
    [XmlInclude(typeof(RadioStation))]
    [XmlInclude(typeof(DeezerTrackItem))]
    public class OnlineMediaItem
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
        public OnlineMediaSource Source { get; set; }

        [DataMember(Order = 5)]
        public int Bitrate { get; set; }

        [DataMember(Order = 6)]
        public string Content { get; set; }

        [DataMember(Order = 7)]
        public string Artist { get; set; }

        [DataMember(Order = 8)]
        public string Album { get; set; }

        [DataMember(Order = 9)]
        public TimeSpan Duration { get; set; }

        [DataMember(Order = 10)]
        public string ReleaseDate { get; set; }

        [DataMember(Order = 11)]
        public string AlbumUriImageSmall { get; set; }

        [DataMember(Order = 12)]
        public string AlbumUriImageLarge { get; set; }

        [DataMember(Order = 13)]
        public string ArtistUriImageSmall { get; set; }

        [DataMember(Order = 14)]
        public string ArtistUriImageLarge { get; set; }

        public override bool Equals(object obj)
        {
            OnlineMediaItem item = obj as OnlineMediaItem;
            if (item != null)
            {
                return (string.Compare(this.Url, item.Url, true) == 0);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (Url ?? "").GetHashCode();
        }
    }

    [DataContract]
    [Serializable]
    public class OnlinePlaylist
    {
        [DataMember(Order = 0)]
        public UInt64 Id { get; set; }

        [DataMember(Order = 1)]
        public string Title { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("[Id={0}, Title={1}, Desc={2}]", Id, Title, Description);
        }

        public static OnlinePlaylist AddNew()
        {
            return new OnlinePlaylist
            {
                Id = 0,
                Description = "",
                Title = $"( {Translator.Translate("TXT_NEW")} )"
            };
        }
    }

}
