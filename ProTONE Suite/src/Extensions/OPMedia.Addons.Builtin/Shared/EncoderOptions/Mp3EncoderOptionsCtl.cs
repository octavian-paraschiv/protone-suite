using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.Runtime.ProTONE.Compression.Lame;
using OPMedia.UI.Dialogs;
using OPMedia.Addons.Builtin.Translations;
using OPMedia.Addons.Builtin.Shared.Compression.OPMedia.Runtime.ProTONE.Compression.LameWrapper;
using OPMedia.Core.TranslationSupport;

namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public partial class Mp3EncoderOptionsCtl : EncoderConfiguratorCtl
    {
        public bool UsedForCdRipper { get; set; }
        
        public override AudioMediaFormatType OutputFormat
        {
            get
            {
                return AudioMediaFormatType.MP3;
            }
        }

        public Mp3EncoderSettings Mp3EncoderSettings { get; set; }

        private Mp3ConversionOptions Options
        {
            get
            {
                return Mp3EncoderSettings.Options;
            }

            set
            {
                Mp3EncoderSettings.Options = value;
            }
        }

        private bool CopyInputFileMetadata
        {
            get { return Mp3EncoderSettings.CopyInputFileMetadata; }
            set { Mp3EncoderSettings.CopyInputFileMetadata = value; }
        }

        public Mp3EncoderOptionsCtl() 
        {
            InitializeComponent();

            if (Mp3EncoderSettings == null)
                Mp3EncoderSettings = new Mp3EncoderSettings();

            this.Load += new EventHandler(Mp3EncoderOptionsCtl_Load);
        }

        void Mp3EncoderOptionsCtl_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void UpdateSummary()
        {
            string summary = "";
            this.Options.BE_CONFIG(ref summary);
            lblOutputBitrateHint.Text = summary;
        }

        private void ChangeBitrateModeFieldsVisibility()
        {
            switch (this.Options.BitrateMode)
            {
                case BitrateMode.CBR:
                    lblBitrate.Visible = cmbBitrate.Visible = true;
                    lblPreset.Visible = cmbPreset.Visible = false;
                    lblVbrQuality.Visible = cgVbrQuality.Visible = pnlVbrHints.Visible = false;
                    break;

                case BitrateMode.ABR:
                    lblBitrate.Visible = cmbBitrate.Visible = true;
                    lblPreset.Visible = cmbPreset.Visible = false;
                    lblVbrQuality.Visible = cgVbrQuality.Visible = pnlVbrHints.Visible = false;
                    break;

                case BitrateMode.Preset:
                    lblBitrate.Visible = cmbBitrate.Visible = false;
                    lblPreset.Visible = cmbPreset.Visible = true;
                    lblVbrQuality.Visible = cgVbrQuality.Visible = pnlVbrHints.Visible = false;
                    break;

                case BitrateMode.VBR:
                    lblBitrate.Visible = cmbBitrate.Visible = false;
                    lblPreset.Visible = cmbPreset.Visible = false;
                    lblVbrQuality.Visible = cgVbrQuality.Visible = pnlVbrHints.Visible = true;
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.Options != null)
            {
                string summary = "";
                string conversionFlags = this.Options.BE_CONFIG(ref summary).ToString();
                LogFileConsoleDetail dlg = new LogFileConsoleDetail(conversionFlags);
                dlg.Text = "MP3 Conversion Flags";
                dlg.ShowDialog();
            }
        }

        internal void Reload()
        {
            if (Mp3EncoderSettings == null)
                Mp3EncoderSettings = new Mp3EncoderSettings();

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
            // CBR and ABR bit rate
            cmbBitrate.SelectedIndex = cmbBitrate.FindStringExact(this.Options.Bitrate.ToString());
            cmbBitrate.SelectedIndexChanged += (s, a) =>
            {
                this.Options.Bitrate = int.Parse(cmbBitrate.Text);
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // VBR "preset-based"
            cmbPreset.Items.Clear();
            foreach (var x in Enum.GetValues(typeof(Preset)))
            {
                cmbPreset.Items.Add(x);
            }
            cmbPreset.SelectedIndex = cmbPreset.FindStringExact(this.Options.VBRPreset.ToString());
            cmbPreset.SelectedIndexChanged += (s, a) =>
            {
                this.Options.VBRPreset =
                    (Preset)Enum.Parse(typeof(Preset), cmbPreset.Text);
                UpdateSummary();
            };
            // ------------------------

            // ------------------------
            // VBR "quality-based"
            cgVbrQuality.Value = this.Options.VBRQuality;
            cgVbrQuality.PositionChanged += (pos) =>
            {
                this.Options.VBRQuality = (int)cgVbrQuality.Value;
                UpdateSummary();
            };
            // ------------------------


            // ------------------------
            // CopyInputFileMetadata
            chkGenerateTag.Checked = this.CopyInputFileMetadata;
            chkGenerateTag.CheckedChanged += (s, a) =>
            {
                this.CopyInputFileMetadata = chkGenerateTag.Checked;
            };
            // ------------------------

            UpdateSummary();
        }
    }
}
