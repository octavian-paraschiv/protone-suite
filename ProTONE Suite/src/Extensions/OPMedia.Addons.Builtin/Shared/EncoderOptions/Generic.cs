using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Addons.Builtin.Shared.Compression.OPMedia.Runtime.ProTONE.Compression.LameWrapper;
using OPMedia.Runtime.ProTONE.Compression.Lame;

namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public enum AudioMediaFormatType
    {
        MP3 = 0,
        WAV,
        WMA,
        OGG
    }

    public sealed class EncoderSettingsContainer
    {
        public AudioMediaFormatType AudioMediaFormatType { get; set; }

        public WavEncoderSettings WavEncoderSettings { get; set; }
        public Mp3EncoderSettings Mp3EncoderSettings { get; set; }
        public OggEncoderSettings OggEncoderSettings { get; set; }
        public WmaEncoderSettings WmaEncoderSettings { get; set; }

        public EncoderSettingsContainer()
        {
            this.AudioMediaFormatType = EncoderOptions.AudioMediaFormatType.MP3;
            this.WavEncoderSettings = new WavEncoderSettings();
            this.Mp3EncoderSettings = new Mp3EncoderSettings();
            this.OggEncoderSettings = new OggEncoderSettings();
            this.WmaEncoderSettings = new WmaEncoderSettings();
        }
    }

    public class WavEncoderSettings
    {
    }

    public class Mp3EncoderSettings
    {
        public bool CopyInputFileMetadata { get; set; }

        public Mp3ConversionOptions Options { get; set; }

        public override string ToString()
        {
            string summary = "";
            return this.Options.BE_CONFIG(ref summary).ToString();
        }

        public Mp3EncoderSettings()
        {
            this.CopyInputFileMetadata = false;
            this.Options = new Mp3ConversionOptions();
        }
    }

    public class OggEncoderSettings
    {
    }

    public class WmaEncoderSettings
    {
    }

}
