using NAudio.Lame;
using NAudio.Wave;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering.Cdda;
using OPMedia.Runtime.ProTONE.Rendering.Cdda.Freedb;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;

namespace OPMedia.Addons.Builtin.Shared.Compression
{
    class GrabberToMP3 : CdRipper
    {
        public Mp3ConversionOptions Options { get; set; }

        public override void Grab(CDDrive cd, Track track, string destFile, bool generateTags)
        {
            if (MustCancel())
                return;

            byte[] buff = base.GetTrackData(cd, track);

            if (MustCancel())
                return;

            ID3FileInfoSlim ifiSlim = new ID3FileInfoSlim();
            ifiSlim.Album = track.Album;
            ifiSlim.Artist = track.Artist;
            ifiSlim.Genre = track.Genre;
            ifiSlim.Title = track.Title;
            ifiSlim.Track = (short)track.Index;

            short year = 1900;
            if (short.TryParse(track.Year, out year))
                ifiSlim.Year = year;

            ifiSlim.Comments = $"Grabbed with OPMedia Library from CDDA ID {cd.GetCDDBDiskID()}";

            this.Options.WaveFormat = WaveFormatEx.Cdda;

            EncodeBuffer(buff, destFile, generateTags, ifiSlim);
        }

        public void EncodeBuffer(byte[] buff, string destFile, bool generateTags, ID3FileInfoSlim ifiSlim)
        {
            LAMEPreset preset = LAMEPreset.STANDARD;
            LameMP3FileWriter w = null;

            WaveFormat fmt = new WaveFormat(this.Options.ResampleFrequency,
                this.Options.ChannelMode == ChannelMode.SingleChannel ? 1 : 2);

            switch (this.Options.BitrateMode)
            {
                case BitrateMode.CBR:
                    w = new LameMP3FileWriter(destFile, fmt, this.Options.BitrateCBR, null);
                    break;

                case BitrateMode.ABR:
                    // ABR == VBR bitrate-based
                    preset = (LAMEPreset)this.Options.BitrateABR;
                    w = new LameMP3FileWriter(destFile, fmt, preset, null);
                    break;

                case BitrateMode.VBR:
                    // VBR quality-based
                    w = new LameMP3FileWriter(destFile, fmt, (LAMEPreset)this.Options.VBRQuality, null);
                    break;

                case BitrateMode.Preset:
                    w = new LameMP3FileWriter(destFile, fmt, this.Options.Preset, null);
                    break;
            }

            if (MustCancel())
                return;

            try
            {
                int buffPos = 0;
                while (buffPos < buff.Length)
                {
                    if (MustCancel())
                        return;

                    int bytesToCopy = (int)Math.Min(2 * fmt.AverageBytesPerSecond, buff.Length - buffPos);

                    w.Write(buff, buffPos, bytesToCopy);

                    buffPos += (int)bytesToCopy;
                }
            }
            finally
            {
                w.Close();
                w = null;
            }

            if (!MustCancel() && generateTags && ifiSlim != null)
            {
                ID3FileInfo ifi = new ID3FileInfo(destFile, false);
                ifi.Album = StringUtils.Capitalize(ifiSlim.Album, WordCasing.CapitalizeWords);
                ifi.Artist = StringUtils.Capitalize(ifiSlim.Artist, WordCasing.CapitalizeWords);
                ifi.Genre = StringUtils.Capitalize(ifiSlim.Genre, WordCasing.CapitalizeWords);
                ifi.Title = StringUtils.Capitalize(ifiSlim.Title, WordCasing.CapitalizeWords);
                ifi.Track = ifiSlim.Track.GetValueOrDefault();
                ifi.Year = ifiSlim.Year.GetValueOrDefault();
                ifi.Comments = ifiSlim.Comments;
                ifi.Save();
            }
        }
    }
}
