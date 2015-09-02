using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core.TranslationSupport;

namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public partial class EncoderOptionsCtl : UserControl
    {
        private int selectedPanel = -1;

        public EncoderSettingsContainer EncoderSettings { get; set; }

        List<EncoderConfiguratorCtl> panels = new List<EncoderConfiguratorCtl>();

        public void DisplaySettings(bool usedForCdRipper)
        {
            InternalDisplaySettings(usedForCdRipper);
        }

        public EncoderOptionsCtl()
        {
            this.EncoderSettings = new EncoderSettingsContainer();
            InitializeComponent();

            cmbOutputFormat.Items.Clear();
            AddPanel(new Mp3EncoderOptionsCtl());
            AddPanel(new WavEncoderOptionsCtl());
        }

        private void InternalDisplaySettings(bool usedForCdRipper)
        {
            foreach (Control ctl in pnlEncoderOptions.Controls)
            {
                Mp3EncoderOptionsCtl mp3Ctl = ctl as Mp3EncoderOptionsCtl;
                if (mp3Ctl != null)
                {
                    mp3Ctl.UsedForCdRipper = usedForCdRipper;
                    mp3Ctl.Mp3EncoderSettings = EncoderSettings.Mp3EncoderSettings;
                    mp3Ctl.Reload();
                }
            }

            cmbOutputFormat.SelectedIndex = (int)EncoderSettings.AudioMediaFormatType;
            ShowPanel(cmbOutputFormat.SelectedIndex);
        }

        private void AddPanel(EncoderConfiguratorCtl panel)
        {
            cmbOutputFormat.Items.Add(panel.OutputFormat);
            panels.Add(panel);
            panel.Visible = false;
            panel.Dock = DockStyle.Fill;
        }

        private void OnSelectOutputFormat(object sender, EventArgs e)
        {
            ShowPanel(cmbOutputFormat.SelectedIndex);
            EncoderSettings.AudioMediaFormatType = (AudioMediaFormatType)cmbOutputFormat.SelectedIndex;
        }

        private void ShowPanel(int index)
        {
            try
            {
                this.SuspendLayout();
                pnlEncoderOptions.SuspendLayout();
                pnlEncoderOptions.Controls.Clear();

                EncoderConfiguratorCtl panel = panels[index];
                if (panel != null)
                {
                    Translator.TranslateControl(panel, false);
                    panel.Visible = true;

                    pnlEncoderOptions.Controls.Add(panel);
                }

                selectedPanel = index;
            }
            finally
            {
                pnlEncoderOptions.ResumeLayout();
                this.ResumeLayout();
            }
        }
    }
}
