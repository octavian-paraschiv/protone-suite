using System;
using OPMedia.Core.TranslationSupport;
using OPMedia.Addons.Builtin.Shared.Compression;
using NAudio.Lame;

namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public partial class Mp3EncoderOptionsCtl : EncoderConfiguratorCtl
    {
        public Mp3EncoderSettings Settings
        {
            get
            {
                return (base.EncoderSettings as Mp3EncoderSettings);
            }
        }

        public Mp3ConversionOptions Options
        {
            get
            {
                return this.Settings.Options;
            }
        }

        public Mp3EncoderOptionsCtl() 
            : base(new Mp3EncoderSettings())
        {
            InitializeComponent();
            this.Load += new EventHandler(Mp3EncoderOptionsCtl_Load);
        }

        void Mp3EncoderOptionsCtl_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void UpdateSummary(bool fireSettingsChanged = true)
        {
            string summary = this.Options.GetSummary();
            lblOutputBitrateHint.Text = summary;

            if (fireSettingsChanged)
                FireSettingsChanged();
        }

        private void ChangeBitrateModeFieldsVisibility()
        {
            switch (this.Options.BitrateMode)
            {
                case BitrateMode.CBR:
                    lblBitrate.Visible = cmbBitrateCBR.Visible = true;
                    lblPreset.Visible = cmbPreset.Visible = false;
                    lblVbrQuality.Visible = cmbVbrQuality.Visible = false;
                    cmbBitrateABR.Visible = false;
                    break;

                case BitrateMode.ABR:
                    lblBitrate.Visible = cmbBitrateABR.Visible = true;
                    lblPreset.Visible = cmbPreset.Visible = false;
                    lblVbrQuality.Visible = cmbVbrQuality.Visible = false;
                    cmbBitrateCBR.Visible = false;
                    break;

                case BitrateMode.Preset:
                    lblBitrate.Visible = cmbBitrateCBR.Visible = false;
                    lblPreset.Visible = cmbPreset.Visible = true;
                    lblVbrQuality.Visible = cmbVbrQuality.Visible = false;
                    cmbBitrateABR.Visible = false;
                    break;

                case BitrateMode.VBR:
                    lblBitrate.Visible = cmbBitrateCBR.Visible = false;
                    lblPreset.Visible = cmbPreset.Visible = false;
                    lblVbrQuality.Visible = cmbVbrQuality.Visible = true;
                    cmbBitrateABR.Visible = false;
                    break;
            }
        }

        internal override void  Reload()
        {
            chkGenerateTag.Text = Translator.Translate(UsedForCdRipper ?
                "TXT_GENERATE_TAG" : "TXT_REUSE_TAG");

            // ------------------------------
            // Channel mode options
            cmbChannelMode.Items.Clear();
            foreach (var x in Enum.GetValues(typeof(ChannelMode)))
            {
                switch ((ChannelMode)x)
                {
                    case ChannelMode.JointStereo:
                    case ChannelMode.Stereo:
                        cmbChannelMode.Items.Add(x);
                        break;

                    case ChannelMode.SingleChannel:
                        {
                            // Mono not available if ripping CD tracks.
                            if (UsedForCdRipper == false)
                                cmbChannelMode.Items.Add(x);
                        }
                        break;
                }

            }
            cmbChannelMode.SelectedIndex = cmbChannelMode.FindStringExact(this.Options.ChannelMode.ToString());
            cmbChannelMode.SelectedIndexChanged += (s, a) =>
            {
                this.Options.ChannelMode =
                    (ChannelMode)Enum.Parse(typeof(ChannelMode), cmbChannelMode.Text);
                UpdateSummary();
            };
            //-------------------------

            // ------------------------
            // Bitrate Mode options
            cmbBitrateMode.Items.Clear();
            foreach (var x in Enum.GetValues(typeof(BitrateMode)))
            {
                cmbBitrateMode.Items.Add(x);
            }
            cmbBitrateMode.SelectedIndex = cmbBitrateMode.FindStringExact(this.Options.BitrateMode.ToString());
            ChangeBitrateModeFieldsVisibility();
            cmbBitrateMode.SelectedIndexChanged += (s, a) =>
            {
                this.Options.BitrateMode = (BitrateMode)cmbBitrateMode.SelectedIndex;
                ChangeBitrateModeFieldsVisibility();
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // CBR bit rate
            cmbBitrateCBR.SelectedIndex = cmbBitrateCBR.FindStringExact(this.Options.BitrateCBR.ToString());
            cmbBitrateCBR.SelectedIndexChanged += (s, a) =>
            {
                this.Options.BitrateCBR = int.Parse(cmbBitrateCBR.Text);
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // ABR bit rate
            cmbBitrateABR.Items.Clear();
            foreach (LAMEPreset x in Enum.GetValues(typeof(LAMEPreset)))
            {
                if (x > LAMEPreset.ABR_320)
                    break;

                cmbBitrateABR.Items.Add((int)x);
            }

            cmbBitrateABR.SelectedIndex = cmbBitrateABR.FindStringExact(this.Options.BitrateABR.ToString());
            cmbBitrateABR.SelectedIndexChanged += (s, a) =>
            {
                this.Options.BitrateABR = int.Parse(cmbBitrateABR.Text);
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // VBR "preset-based"
            cmbPreset.Items.Clear();
            foreach (LAMEPreset x in Enum.GetValues(typeof(LAMEPreset)))
            {
                if (x < LAMEPreset.R3MIX)
                    continue;

                cmbPreset.Items.Add(x);
            }
            cmbPreset.SelectedIndex = cmbPreset.FindStringExact(this.Options.Preset.ToString());
            cmbPreset.SelectedIndexChanged += (s, a) =>
            {
                this.Options.Preset =
                    (LAMEPreset)Enum.Parse(typeof(LAMEPreset), cmbPreset.Text);
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // VBR "quality-based"
            cmbVbrQuality.Items.Clear();
            foreach (VBRQuality x in Enum.GetValues(typeof(VBRQuality)))
                cmbVbrQuality.Items.Add(x);

            cmbVbrQuality.SelectedIndex = cmbVbrQuality.FindStringExact(this.Options.VBRQuality.ToString());
            cmbVbrQuality.SelectedIndexChanged += (s, a) =>
            {
                this.Options.VBRQuality = (VBRQuality)Enum.Parse(typeof(VBRQuality), cmbVbrQuality.Text);
                UpdateSummary();
            };
            // ------------------------


            // ------------------------
            // CopyInputFileMetadata
            chkGenerateTag.Checked = this.Settings.CopyInputFileMetadata;
            chkGenerateTag.CheckedChanged += (s, a) =>
            {
                this.Settings.CopyInputFileMetadata = chkGenerateTag.Checked;
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // resample frequency
            cmbFrequency.SelectedIndex = cmbFrequency.FindStringExact(this.Options.ResampleFrequency.ToString());
            cmbFrequency.SelectedIndexChanged += (s, a) =>
            {
                this.Options.ResampleFrequency = int.Parse(cmbFrequency.Text);
                UpdateSummary();
            };
            // ------------------------

            UpdateSummary(false);
        }
    }
}
