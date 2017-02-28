﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Tasks;
using OPMedia.Runtime.ProTONE.Rendering.Cdda;
using OPMedia.Runtime.ProTONE.Rendering.Cdda.Freedb;
using OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Forms;
using System.IO;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard;
using OPMedia.Core.Utilities;
using System.Threading;
using OPMedia.Addons.Builtin.Shared.EncoderOptions;

namespace OPMedia.Addons.Builtin.Shared.Compression
{
    public abstract class CdRipper
    {
        private bool _cancel = false;
        private object _cancelLock = new object();

        public void RequestCancel()
        {
            lock (_cancelLock)
            {
                _cancel = true;
            }
        }

        public bool MustCancel()
        {
            lock (_cancelLock)
            {
                return _cancel;
            }
        }

        public static CdRipper CreateGrabber(AudioMediaFormatType outputType)
        {
            switch (outputType)
            {
                case AudioMediaFormatType.WAV:
                    return new GrabberToWave();
                
                case AudioMediaFormatType.MP3:
                    return new GrabberToMP3();
                
                //case AudioMediaFormatType.WMA:
                //    return new GrabberToWMA();
                
                //case AudioMediaFormatType.OGG:
                //    return new GrabberToOGG();
            }

            return null;
        }

        public virtual void Grab(CDDrive cd, Track track, string destFile, bool generateTags)
        {
            throw new NotImplementedException(string.Format("{0} cannot be used to grab an audio CD.", this.GetType()));
        }

        protected byte[] GetTrackData(CDDrive cd, Track track)
        {
            uint size = cd.TrackSize(track.Index);
            byte[] buff = new byte[size];
            int x = cd.ReadTrack(track.Index, buff, ref size, null);

            if (buff == null || buff.Length < 1)
                throw new InvalidDataException("TXT_INVALID_TRACK_DATA");

            return buff;
        }

        public static string GetFileName(WordCasing wordCasing, Track track, string renamePattern)
        {
            string newName = renamePattern;
            
            StringUtils.ReplaceToken(ref newName, "<A", track.Artist);
            StringUtils.ReplaceToken(ref newName, "<B", track.Album);
            StringUtils.ReplaceToken(ref newName, "<T", track.Title);
            StringUtils.ReplaceToken(ref newName, "<G", track.Genre);
            StringUtils.ReplaceToken(ref newName, "<#", track.Index.ToString("d2"));
            StringUtils.ReplaceToken(ref newName, "<Y", track.Year);

            newName = StringUtils.StripInvalidPathChars(newName);
            if (!string.IsNullOrEmpty(newName))
            {
                return newName;
            }

            return string.Format("track{0:d2}", track.Index);;
        }
    }
}
