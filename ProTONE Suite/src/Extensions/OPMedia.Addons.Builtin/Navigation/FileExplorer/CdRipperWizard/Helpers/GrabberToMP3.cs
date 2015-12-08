﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Runtime.ProTONE.Rendering.Cdda;
using OPMedia.Runtime.ProTONE.Rendering.Cdda.Freedb;
using System.IO;

using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Core.Utilities;
using OPMedia.Addons.Builtin.Shared.Compression.OPMedia.Runtime.ProTONE.Compression.LameWrapper;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.Compression.Lame;

namespace OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Helpers
{
    class GrabberToMP3 : CdRipper
    {
        public Mp3ConversionOptions Options { get; set; }

        private uint m_hLameStream = 0;
        private uint m_InputSamples = 0;
        private uint m_OutBufferSize = 0;
        private byte[] m_OutBuffer = null;

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

            this.Options.WaveFormat = WaveFormatEx.Cdda;

            EncodeBuffer(buff, destFile, generateTags, ifiSlim);
        }

        public void EncodeBuffer(byte[] buff, string destFile, bool generateTags, ID3FileInfoSlim ifiSlim)
        {
            string summary = "";
            LameEncConfig cfg = this.Options.GetConfig(ref summary);

            uint LameResult = Lame_encDll.Init(cfg, ref m_InputSamples, ref m_OutBufferSize, ref m_hLameStream);
            if (LameResult != Lame_encDll.ERR_SUCCESSFUL)
            {
                throw new ApplicationException(string.Format("Lame_encDll.Init failed with the error code {0}", LameResult));
            } 
            
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                uint EncodedSize = 0;
                m_OutBuffer = new byte[m_OutBufferSize];

                if (MustCancel())
                    return;

                try
                {
                    int buffPos = 0;
                    while (buffPos < buff.Length)
                    {
                        if (MustCancel())
                            return;

                        m_OutBuffer = new byte[m_OutBufferSize];

                        uint bytesToCopy = Math.Min(2 * m_InputSamples, (uint)buff.Length - (uint)buffPos);

                        if ((LameResult = Lame_encDll.Encode(m_hLameStream, buff, buffPos, (uint)bytesToCopy, m_OutBuffer, ref EncodedSize)) == Lame_encDll.ERR_SUCCESSFUL)
                        {
                            if (EncodedSize > 0)
                            {
                                bw.Write(m_OutBuffer, 0, (int)EncodedSize);
                            }
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("Lame_encDll.Encode failed with the error code {0}", LameResult));
                        }

                        buffPos += (int)bytesToCopy;
                    }
                }
                finally
                {
                    EncodedSize = 0;
                    if (Lame_encDll.Release(m_hLameStream, m_OutBuffer, ref EncodedSize) == Lame_encDll.ERR_SUCCESSFUL)
                    {
                        if (EncodedSize > 0)
                        {
                            bw.Write(m_OutBuffer, 0, (int)EncodedSize);
                        }
                    }
                    Lame_encDll.Close(m_hLameStream);
                }
            }

            if (!MustCancel() && cfg.format.bWriteVBRHeader != 0)
            {
                uint err = Lame_encDll.WriteVBRHeader(destFile);
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
                ifi.Save();
            }
        }
    }
}
