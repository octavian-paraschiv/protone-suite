namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    partial class Mp3EncoderOptionsCtl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbChannelMode = new OPMedia.UI.Controls.OPMComboBox();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.chkGenerateTag = new OPMedia.UI.Controls.OPMCheckBox();
            this.cmbBitrateMode = new OPMedia.UI.Controls.OPMComboBox();
            this.lblBitrate = new OPMedia.UI.Controls.OPMLabel();
            this.lblVbrQuality = new OPMedia.UI.Controls.OPMLabel();
            this.lblPreset = new OPMedia.UI.Controls.OPMLabel();
            this.cmbBitrateCBR = new OPMedia.UI.Controls.OPMComboBox();
            this.cmbBitrateABR = new OPMedia.UI.Controls.OPMComboBox();
            this.cmbPreset = new OPMedia.UI.Controls.OPMComboBox();
            this.cmbVbrQuality = new OPMedia.UI.Controls.OPMComboBox();
            this.lblOutputBitrateHint = new OPMedia.UI.Controls.OPMLabel();
            this.lblFrequency = new OPMedia.UI.Controls.OPMLabel();
            this.cmbFrequency = new OPMedia.UI.Controls.OPMComboBox();
            this.opmTableLayoutPanel4 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmTableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.Location = new System.Drawing.Point(0, 0);
            this.opmLabel3.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.opmLabel3.MaximumSize = new System.Drawing.Size(130, 24);
            this.opmLabel3.MinimumSize = new System.Drawing.Size(130, 24);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(130, 24);
            this.opmLabel3.TabIndex = 5;
            this.opmLabel3.Text = "TXT_CHANNEL_MODE";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbChannelMode
            // 
            this.cmbChannelMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbChannelMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbChannelMode.FormattingEnabled = true;
            this.cmbChannelMode.Location = new System.Drawing.Point(135, 0);
            this.cmbChannelMode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbChannelMode.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbChannelMode.Name = "cmbChannelMode";
            this.cmbChannelMode.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbChannelMode.Size = new System.Drawing.Size(97, 24);
            this.cmbChannelMode.TabIndex = 6;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.Location = new System.Drawing.Point(0, 29);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(130, 30);
            this.opmLabel1.TabIndex = 0;
            this.opmLabel1.Text = "TXT_BITRATE_MODE";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkGenerateTag
            // 
            this.chkGenerateTag.AutoSize = true;
            this.opmTableLayoutPanel4.SetColumnSpan(this.chkGenerateTag, 5);
            this.chkGenerateTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkGenerateTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkGenerateTag.Location = new System.Drawing.Point(0, 109);
            this.chkGenerateTag.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.chkGenerateTag.Name = "chkGenerateTag";
            this.chkGenerateTag.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkGenerateTag.Size = new System.Drawing.Size(711, 19);
            this.chkGenerateTag.TabIndex = 10;
            this.chkGenerateTag.Text = "TXT_GENERATE_TAG";
            this.chkGenerateTag.UseVisualStyleBackColor = true;
            // 
            // cmbBitrateMode
            // 
            this.cmbBitrateMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBitrateMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbBitrateMode.FormattingEnabled = true;
            this.cmbBitrateMode.Items.AddRange(new object[] {
            "CBR",
            "VBR"});
            this.cmbBitrateMode.Location = new System.Drawing.Point(135, 29);
            this.cmbBitrateMode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbBitrateMode.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbBitrateMode.Name = "cmbBitrateMode";
            this.cmbBitrateMode.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbBitrateMode.Size = new System.Drawing.Size(97, 24);
            this.cmbBitrateMode.TabIndex = 1;
            // 
            // lblBitrate
            // 
            this.lblBitrate.AutoSize = true;
            this.lblBitrate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBitrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblBitrate.Location = new System.Drawing.Point(578, 29);
            this.lblBitrate.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.lblBitrate.Name = "lblBitrate";
            this.lblBitrate.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblBitrate.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblBitrate.Size = new System.Drawing.Size(128, 30);
            this.lblBitrate.TabIndex = 8;
            this.lblBitrate.Text = "TXT_BITRATE";
            this.lblBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVbrQuality
            // 
            this.lblVbrQuality.AutoSize = true;
            this.lblVbrQuality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVbrQuality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblVbrQuality.Location = new System.Drawing.Point(481, 29);
            this.lblVbrQuality.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.lblVbrQuality.Name = "lblVbrQuality";
            this.lblVbrQuality.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblVbrQuality.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblVbrQuality.Size = new System.Drawing.Size(92, 30);
            this.lblVbrQuality.TabIndex = 6;
            this.lblVbrQuality.Text = "TXT_VBRQUALITY";
            this.lblVbrQuality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPreset.Location = new System.Drawing.Point(232, 29);
            this.lblPreset.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblPreset.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblPreset.Size = new System.Drawing.Size(244, 30);
            this.lblPreset.TabIndex = 10;
            this.lblPreset.Text = "TXT_PRESET";
            this.lblPreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBitrateCBR
            // 
            this.cmbBitrateCBR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBitrateCBR.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbBitrateCBR.FormattingEnabled = true;
            this.cmbBitrateCBR.Items.AddRange(new object[] {
            "32",
            "40",
            "48",
            "56",
            "64",
            "80",
            "96",
            "112",
            "128",
            "160",
            "192",
            "224",
            "256",
            "320"});
            this.cmbBitrateCBR.Location = new System.Drawing.Point(232, 64);
            this.cmbBitrateCBR.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbBitrateCBR.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbBitrateCBR.Name = "cmbBitrateCBR";
            this.cmbBitrateCBR.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbBitrateCBR.Size = new System.Drawing.Size(249, 24);
            this.cmbBitrateCBR.TabIndex = 7;
            // 
            // cmbBitrateABR
            // 
            this.cmbBitrateABR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBitrateABR.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbBitrateABR.FormattingEnabled = true;
            this.cmbBitrateABR.Location = new System.Drawing.Point(232, 64);
            this.cmbBitrateABR.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbBitrateABR.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbBitrateABR.Name = "cmbBitrateABR";
            this.cmbBitrateABR.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbBitrateABR.Size = new System.Drawing.Size(249, 24);
            this.cmbBitrateABR.TabIndex = 7;
            // 
            // cmbPreset
            // 
            this.cmbPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPreset.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPreset.FormattingEnabled = true;
            this.cmbPreset.Location = new System.Drawing.Point(135, 64);
            this.cmbPreset.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbPreset.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbPreset.Name = "cmbPreset";
            this.cmbPreset.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbPreset.Size = new System.Drawing.Size(97, 24);
            this.cmbPreset.TabIndex = 4;
            // 
            // cmbVbrQuality
            // 
            this.cmbVbrQuality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVbrQuality.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbVbrQuality.FormattingEnabled = true;
            this.cmbVbrQuality.Location = new System.Drawing.Point(135, 64);
            this.cmbVbrQuality.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbVbrQuality.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbVbrQuality.Name = "cmbVbrQuality";
            this.cmbVbrQuality.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbVbrQuality.Size = new System.Drawing.Size(97, 24);
            this.cmbVbrQuality.TabIndex = 4;
            // 
            // lblOutputBitrateHint
            // 
            this.opmTableLayoutPanel4.SetColumnSpan(this.lblOutputBitrateHint, 5);
            this.lblOutputBitrateHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutputBitrateHint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOutputBitrateHint.Location = new System.Drawing.Point(0, 128);
            this.lblOutputBitrateHint.Margin = new System.Windows.Forms.Padding(0);
            this.lblOutputBitrateHint.Name = "lblOutputBitrateHint";
            this.lblOutputBitrateHint.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblOutputBitrateHint.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblOutputBitrateHint.Size = new System.Drawing.Size(711, 206);
            this.lblOutputBitrateHint.TabIndex = 12;
            this.lblOutputBitrateHint.Text = "dfsdsfdsfsdfdsf";
            this.lblOutputBitrateHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFrequency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFrequency.Location = new System.Drawing.Point(232, 0);
            this.lblFrequency.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblFrequency.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblFrequency.Size = new System.Drawing.Size(244, 24);
            this.lblFrequency.TabIndex = 8;
            this.lblFrequency.Text = "Freq:";
            this.lblFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFrequency
            // 
            this.cmbFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFrequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbFrequency.FormattingEnabled = true;
            this.cmbFrequency.Items.AddRange(new object[] {
            "8000",
            "11025",
            "12000",
            "16000",
            "22050",
            "24000",
            "32000",
            "44100",
            "48000"});
            this.cmbFrequency.Location = new System.Drawing.Point(481, 0);
            this.cmbFrequency.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbFrequency.MinimumSize = new System.Drawing.Size(85, 0);
            this.cmbFrequency.Name = "cmbFrequency";
            this.cmbFrequency.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbFrequency.Size = new System.Drawing.Size(97, 24);
            this.cmbFrequency.TabIndex = 7;
            // 
            // opmTableLayoutPanel4
            // 
            this.opmTableLayoutPanel4.AutoSize = true;
            this.opmTableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmTableLayoutPanel4.ColumnCount = 5;
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel4.Controls.Add(this.chkGenerateTag, 0, 4);
            this.opmTableLayoutPanel4.Controls.Add(this.lblOutputBitrateHint, 0, 5);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbVbrQuality, 3, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbPreset, 3, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbBitrateCBR, 3, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbBitrateABR, 3, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.lblVbrQuality, 2, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.lblBitrate, 2, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.lblPreset, 2, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbFrequency, 3, 0);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbChannelMode, 1, 0);
            this.opmTableLayoutPanel4.Controls.Add(this.cmbBitrateMode, 1, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.lblFrequency, 2, 0);
            this.opmTableLayoutPanel4.Controls.Add(this.opmLabel3, 0, 0);
            this.opmTableLayoutPanel4.Controls.Add(this.opmLabel1, 0, 1);
            this.opmTableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.opmTableLayoutPanel4.Name = "opmTableLayoutPanel4";
            this.opmTableLayoutPanel4.RowCount = 7;
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel4.Size = new System.Drawing.Size(711, 354);
            this.opmTableLayoutPanel4.TabIndex = 1;
            // 
            // Mp3EncoderOptionsCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel4);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Mp3EncoderOptionsCtl";
            this.Size = new System.Drawing.Size(711, 354);
            this.opmTableLayoutPanel4.ResumeLayout(false);
            this.opmTableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMComboBox cmbBitrateMode;
        private UI.Controls.OPMLabel opmLabel3;
        private UI.Controls.OPMComboBox cmbChannelMode;
        private UI.Controls.OPMComboBox cmbBitrateCBR;
        private UI.Controls.OPMComboBox cmbBitrateABR;
        private UI.Controls.OPMLabel lblBitrate;
        private UI.Controls.OPMLabel lblVbrQuality;
        private UI.Controls.OPMComboBox cmbPreset;
        private UI.Controls.OPMCheckBox chkGenerateTag;
        private UI.Controls.OPMLabel lblPreset;
        private UI.Controls.OPMLabel lblOutputBitrateHint;
        private UI.Controls.OPMComboBox cmbVbrQuality;
        private UI.Controls.OPMLabel lblFrequency;
        private UI.Controls.OPMComboBox cmbFrequency;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel4;

    }
}
