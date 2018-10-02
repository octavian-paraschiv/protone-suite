using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Lame;
using OPMedia.Core.TranslationSupport;

using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;

namespace OPMedia.Addons.Builtin.Shared.Compression
{
    public enum BitrateMode
    {
        CBR = 0,
        ABR,
        VBR,
        Preset
    }

    public enum ChannelMode
    {
        Stereo = 0,
        JointStereo,
        DualChannel,
        SingleChannel,
    }

    public enum VBRQuality
    {
        VBR_10 = 410,
        VBR_20 = 420,
        VBR_30 = 430,
        VBR_40 = 440,
        VBR_50 = 450,
        VBR_60 = 460,
        VBR_70 = 470,
        VBR_80 = 480,
        VBR_90 = 490,
        VBR_100 = 500,
    }

    public class Mp3ConversionOptions
    {
        public BitrateMode BitrateMode { get; set; }
        public ChannelMode ChannelMode { get; set; }

        public int BitrateCBR { get; set; }

        public int BitrateABR { get; set; }

        public LAMEPreset Preset { get; set; }

        public VBRQuality VBRQuality { get; set; }
            
        public WaveFormatEx WaveFormat { get; set; }

        public int ResampleFrequency { get; set; }

        public Mp3ConversionOptions()
        {
            this.BitrateMode = BitrateMode.CBR;
            this.ChannelMode = ChannelMode.Stereo;
            this.BitrateCBR = 192;
            this.BitrateABR = (int)LAMEPreset.ABR_128;
            this.Preset = LAMEPreset.STANDARD;
            this.VBRQuality = VBRQuality.VBR_60;
            this.WaveFormat = WaveFormatEx.Cdda;
            this.ResampleFrequency = WaveFormatEx.Cdda.nSamplesPerSec;
        }

        public string GetSummary()
        {
            string summary = string.Empty;

            switch (BitrateMode)
            {
                case BitrateMode.CBR:
                    summary = Translator.Translate("TXT_CBR_SUMMARY", this.ChannelMode, this.BitrateCBR, this.ResampleFrequency);
                    break;

                case BitrateMode.ABR:
                    // ABR == VBR bitrate-based
                    summary = Translator.Translate("TXT_ABR_SUMMARY", this.ChannelMode, this.BitrateABR, this.ResampleFrequency);
                    break;

                case BitrateMode.VBR:
                    // VBR quality-based
                    summary = Translator.Translate("TXT_VBR_SUMMARY", this.ChannelMode, this.VBRQuality, this.ResampleFrequency);
                    break;

                case BitrateMode.Preset:
                    // Preset mode is VBR or CBR/320 in case that VBRPreset == INSANE
                    if (this.Preset != LAMEPreset.INSANE)
                        summary = Translator.Translate("TXT_PRESET_SUMMARY", this.ChannelMode, this.Preset, this.ResampleFrequency);
                    else
                        summary = Translator.Translate("TXT_PRESET_INSANE", this.ChannelMode, this.Preset, this.ResampleFrequency);
                    break;
            }

            return summary;
        }

    }
}
