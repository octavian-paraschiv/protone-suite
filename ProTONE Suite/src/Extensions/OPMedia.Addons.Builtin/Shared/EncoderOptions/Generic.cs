﻿using OPMedia.Addons.Builtin.Shared.Compression;


namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public enum AudioMediaFormatType
    {
        MP3 = 0,
        WAV,
        WMA,
        OGG
    }


    public abstract class EncoderSettings
    {
        public AudioMediaFormatType FormatType { get; private set; }

        public EncoderSettings(AudioMediaFormatType fmtType)
        {
            this.FormatType = fmtType;
        }
    }

    public class WavEncoderSettings : EncoderSettings
    {
        public WavEncoderSettings()
            : base(AudioMediaFormatType.WAV)
        {
        }
    }

    public class Mp3EncoderSettings : EncoderSettings
    {
        public bool CopyInputFileMetadata { get; set; }

        public Mp3ConversionOptions Options { get; set; }

        public override string ToString()
        {
            return this.Options.ToString();
        }

        public Mp3EncoderSettings()
            : base(AudioMediaFormatType.MP3)
        {
            this.CopyInputFileMetadata = false;
            this.Options = new Mp3ConversionOptions();
        }
    }

    public class OggEncoderSettings : EncoderSettings
    {
        public OggEncoderSettings()
            : base(AudioMediaFormatType.OGG)
        {
        }
    }

    public class WmaEncoderSettings : EncoderSettings
    {
        public WmaEncoderSettings()
            : base(AudioMediaFormatType.WMA)
        {
        }
    }

}
