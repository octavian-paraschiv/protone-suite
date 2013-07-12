﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.Rendering.Cdda;
using OPMedia.Core;
using System.IO;
using System.Runtime.InteropServices;

namespace OPMedia.Runtime.ProTONE.Rendering.DS.DsFilters
{
    public class AudioCdFileParser : FileParser
    {
        [StructLayout(LayoutKind.Sequential)]
        private class OUTPUT_DATA_HEADER
        {
            public uint dwData = 0;
            public uint dwDataLength = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class OUTPUT_FILE_HEADER
        {
            public uint dwRiff = 0;
            public uint dwFileSize = 0;
            public uint dwWave = 0;
            public uint dwFormat = 0;
            public uint dwFormatLength = 0;
        }

        private const uint RIFF_TAG = 0x46464952;
        private const uint CDDA_TAG = 0x41444443;
        private const uint WAVE_TAG = 0x45564157;
        private const uint FMT__TAG = 0x20746D66;
        private const uint DATA_TAG = 0x61746164;
        private const uint WAVE_FORMAT_PCM = 0x01;

        CDDrive _cdrom = new CDDrive();
        WaveFormatEx _wfex = null;

        int _track = -1;

        public AudioCdFileParser()
            : base(false)
        {
        }

        protected long m_llDataOffset = 0;
        public long DataOffset
        {
            get { return m_llDataOffset; }
        }

        protected override HRESULT CheckFile()
        {
            try
            {
                string rootPath = Path.GetPathRoot(m_sFileName);
                if (!string.IsNullOrEmpty(rootPath))
                {
                    if (_cdrom.Open(rootPath.ToUpperInvariant()[0]))
                    {
                        if (_cdrom.Refresh()) /* verifies if CD is ready and also reads TOC */
                        {
                            string trackStr = m_sFileName.Replace(rootPath, "").ToLowerInvariant().Replace("track", "").Replace(".cda", "");
                            _track = -1;
                            if (int.TryParse(trackStr, out _track) && _track > 0)
                            {
                                if (_cdrom.IsAudioTrack(_track))
                                {
                                    return S_OK;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return S_FALSE;
        }

        byte[] _buffer = null;

        protected override HRESULT LoadTracks()
        {
            uint size = 0;
            int trackSize = _cdrom.ReadTrack(_track, null, ref size, null);

            _buffer = new byte[size];

            int bytesRead = _cdrom.ReadTrack(_track, _buffer, ref size, null);
            if (bytesRead > 0)
            {
                _cdrom.Close();

                // The following info is STANDARD to all audio CD's according to standard IEC 60908
                WaveFormatEx _wfex = new WaveFormatEx();
                _wfex.cbSize = 0;
                _wfex.wFormatTag = 1; // PCM
                _wfex.nChannels = 2; // stereo
                _wfex.wBitsPerSample = 16; // 16-bit samples
                _wfex.nSamplesPerSec = 44100; // sampling rate 44.1 khz
                _wfex.nBlockAlign = (ushort)(_wfex.nChannels * _wfex.wBitsPerSample / 8);
                _wfex.nAvgBytesPerSec = _wfex.nSamplesPerSec * _wfex.nBlockAlign;

                AMMediaType mt = new AMMediaType();
                mt.majorType = MediaType.Audio;
                mt.subType = MediaSubType.PCM;
                mt.sampleSize = _wfex.nBlockAlign;
                mt.fixedSizeSamples = true;
                mt.SetFormat(_wfex);
                m_Tracks.Add(new CdTrack(this, mt));

                m_llDataOffset = 0;
                m_rtDuration = (UNITS * (_buffer.Length - m_llDataOffset)) / _wfex.nAvgBytesPerSec;

                m_Stream = new BitStreamReader(new MemoryStream(_buffer));

                return S_OK;
            }

            return S_FALSE;
        }
    }

    public class AudioCdSourceFilter : BaseSourceFilterTemplate<AudioCdFileParser>
    {
        public AudioCdSourceFilter()
            : base("OPM Audio CD Source Filter")
        {
        }
    }

    public class CdTrack : DemuxTrack
    {
        #region Variables

        protected AMMediaType m_mt = null;
        protected long m_ullReadPosition = 0;
        protected int m_lSampleSize = 0;
        protected long m_rtMediaPosition = 0;

        #endregion

        #region Constructor

        public CdTrack(AudioCdFileParser _parser, AMMediaType mt)
            : base(_parser, TrackType.Audio)
        {
            m_mt = mt;
            WaveFormatEx _wfx = m_mt;
            m_lSampleSize = _wfx.nAvgBytesPerSec / 2;
        }

        #endregion

        #region Overridden Methods

        public override HRESULT SetMediaType(AMMediaType pmt)
        {
            if (pmt.majorType != m_mt.majorType) 
                return VFW_E_TYPE_NOT_ACCEPTED;
            
            if (pmt.formatPtr == IntPtr.Zero) 
                return VFW_E_INVALIDMEDIATYPE;
            
            if (pmt.subType != m_mt.subType)
                return VFW_E_TYPE_NOT_ACCEPTED;
            
            if (pmt.formatType != m_mt.formatType) 
                return VFW_E_TYPE_NOT_ACCEPTED;

            return NOERROR;
        }

        public override HRESULT GetMediaType(int iPosition, ref AMMediaType pmt)
        {
            if (iPosition < 0) return E_INVALIDARG;
            if (iPosition > 0) return VFW_S_NO_MORE_ITEMS;
            pmt.Set(m_mt);
            return NOERROR;
        }

        public override HRESULT SeekTrack(long _time)
        {
            AudioCdFileParser pParser = (AudioCdFileParser)m_pParser;
            if (_time <= 0 || _time > pParser.Duration)
            {
                m_ullReadPosition = pParser.DataOffset;
            }
            else
            {
                WaveFormatEx _wfx = m_mt;
                if (pParser.Duration > 0)
                {
                    m_ullReadPosition = (pParser.Stream.TotalSize - pParser.DataOffset) * _time / pParser.Duration;
                    if (_wfx.nBlockAlign != 0)
                    {
                        m_ullReadPosition -= m_ullReadPosition % _wfx.nBlockAlign;
                    }
                }
            }
            m_rtMediaPosition = _time;
            return base.SeekTrack(_time);
        }

        public override PacketData GetNextPacket()
        {
            if (m_ullReadPosition < m_pParser.Stream.TotalSize)
            {
                PacketData _data = new PacketData();
                _data.Position = m_ullReadPosition;
                _data.Size = m_lSampleSize;
                _data.SyncPoint = true;
                _data.Start = m_rtMediaPosition;
                _data.Stop = _data.Start + UNITS / 2;
                m_ullReadPosition += m_lSampleSize;
                m_rtMediaPosition = _data.Stop;
                return _data;
            }
            return null;
        }

        #endregion
    }

    public class AudioCdPlayback : DSFilePlayback
    {
        protected override HRESULT OnInitInterfaces()
        {
            // Create Filter
            DSBaseSourceFilter _source = new DSBaseSourceFilter(new AudioCdSourceFilter());
            // load the file
            _source.FileName = m_sFileName;
            // Add to the filter Graph
            _source.FilterGraph = m_GraphBuilder;
            // Render the output pin
            return _source.OutputPin.Render();
        }
    }
}
