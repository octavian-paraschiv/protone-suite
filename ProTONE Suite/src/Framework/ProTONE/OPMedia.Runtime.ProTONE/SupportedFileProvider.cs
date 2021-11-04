using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OPMedia.Runtime.ProTONE
{
    public class SupportedFileProvider
    {
        #region Supported file types
        static List<string> __supportedAudioMediaTypes = new List<string>(new string[]
        { 
            // 17 supported audio file types
            "au",
            "aif", "aiff",
            "cda", // Audio CD track
            "flac",
            "mid", "midi",

            "mod", // audio "module" file type

            // Audio MPEG
            "mp1", "mp2",  "mp3", "mpa",

            "raw",
            "rmi",
            "snd",
            "wav",
            "wma",
        });

        static List<string> __supportedVideoMediaTypes = new List<string>(new string[]
        {
            // 14 supported video file types

            "avi", "divx", "qt",  "m1v", "m2v",

            "mod", // video format for use in digital tapeless camcorders (JVC / Panasonic / Canon)

            "mov",  "mpg", "mpeg", "vob",
            "wm", "wmv",

            "mkv", "mp4",
        });

        static List<string> __supportedHDVideoMediaTypes = new List<string>(new string[]
        {
            "mkv", "mp4",
        });

        static List<string> __supportedPlaylists = new List<string>(new string[]
        {
            "m3u", "pls", "asx", "wpl"
        });

        static List<string> __supportedSubtitles = new List<string>(new string[]
        {
            // MicroDVD
            "sub", 
                
            // SubRip
            "srt", 
                
            // Universal Subtitle Format
            "usf", 
                
            // SubStation Alpha
            "ass", "ssa", 

            //"utf", "idx", "smi", "rt", "aqt", "mpl", 
        });

        #endregion

        #region Other stuff
        public List<string> SupportedAudioTypes { get => __supportedAudioMediaTypes; }
        public List<string> SupportedHDVideoTypes { get => __supportedHDVideoMediaTypes; }
        public List<string> SupportedVideoTypes { get => __supportedVideoMediaTypes; }
        public List<string> SupportedPlaylists { get => __supportedPlaylists; }
        public List<string> SupportedSubtitles { get => __supportedSubtitles; }

        public string AllMediaTypesMultiFilter
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string type in AllMediaTypes)
                    sb.Append(string.Format("{0};", type));

                return sb.ToString().ToLowerInvariant().Trim(';');
            }
        }

        public List<string> AllMediaTypes
        {
            get
            {
                List<string> allTypes = new List<string>();
                allTypes.AddRange(__supportedAudioMediaTypes);
                allTypes.AddRange(__supportedVideoMediaTypes);
                allTypes.AddRange(__supportedPlaylists); // supported playlists
                return allTypes;
            }
        }


        static SupportedFileProvider _instance = new SupportedFileProvider();
        public static SupportedFileProvider Instance { get { return _instance; } }

        private SupportedFileProvider()
        {
        }

        public bool IsSupportedPlaylist(string path)
        {
            string ext = Path.GetExtension(path);
            return SupportedPlaylists.Contains(ext);
        }


        public bool IsSupportedMedia(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    return FolderContainsMediaFiles(path);
            }
            catch
            {
            }

            string ext = Path.GetExtension(path);
            return AllMediaTypes.Contains(ext);
        }

        public bool FolderContainsMediaFiles(string path)
        {
            return FolderContainsMediaFiles(path, 0);
        }

        const int MaxRecursionLevel = 3;

        public bool FolderContainsMediaFiles(string path, int level)
        {
            var files = Directory.GetFiles(path);
            if (files != null)
            {
                foreach (string file in files)
                {
                    if (IsSupportedMedia(file))
                    {
                        return true;
                    }
                }
            }

            var subfolders = Directory.GetDirectories(path);
            if (subfolders != null)
            {
                foreach (string subfolder in subfolders)
                {
                    if (level < (MaxRecursionLevel - 1) &&
                        FolderContainsMediaFiles(subfolder, level + 1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
    #endregion
}