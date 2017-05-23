﻿namespace OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens
{
    partial class SignalAnalysisScreen
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
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.pnlSpectrogram = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel10 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel11 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel5 = new OPMedia.UI.Controls.OPMLabel();
            this.lblSignalSpectrum = new OPMedia.UI.Controls.OPMLabel();
            this.pnlVuMeter = new System.Windows.Forms.TableLayoutPanel();
            this.opmLabel4 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.lblSignalLevel = new OPMedia.UI.Controls.OPMLabel();
            this.pnlWaveform = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.gpWaveform = new OPMedia.UI.Controls.GraphPlotter();
            this.opmLabel7 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel8 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel9 = new OPMedia.UI.Controls.OPMLabel();
            this.lblSignalWaveform = new OPMedia.UI.Controls.OPMLabel();
            this.opmContextMenuStrip1 = new OPMedia.UI.Controls.OPMContextMenuStrip();
            this.spSpectrogram = new OPMedia.UI.ProTONE.Controls.SpectrogramPlotter();
            this.vuRight = new OPMedia.UI.ProTONE.Controls.VuMeterGauge();
            this.vuLeft = new OPMedia.UI.ProTONE.Controls.VuMeterGauge();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.pnlSpectrogram.SuspendLayout();
            this.pnlVuMeter.SuspendLayout();
            this.pnlWaveform.SuspendLayout();
            this.SuspendLayout();
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 1;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.pnlSpectrogram, 0, 2);
            this.opmTableLayoutPanel1.Controls.Add(this.pnlVuMeter, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.pnlWaveform, 0, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 3;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(444, 447);
            this.opmTableLayoutPanel1.TabIndex = 2;
            // 
            // pnlSpectrogram
            // 
            this.pnlSpectrogram.ColumnCount = 4;
            this.pnlSpectrogram.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlSpectrogram.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlSpectrogram.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlSpectrogram.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlSpectrogram.Controls.Add(this.opmLabel10, 0, 1);
            this.pnlSpectrogram.Controls.Add(this.spSpectrogram, 1, 1);
            this.pnlSpectrogram.Controls.Add(this.opmLabel11, 0, 3);
            this.pnlSpectrogram.Controls.Add(this.opmLabel5, 0, 2);
            this.pnlSpectrogram.Controls.Add(this.lblSignalSpectrum, 1, 0);
            this.pnlSpectrogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSpectrogram.Location = new System.Drawing.Point(2, 269);
            this.pnlSpectrogram.Margin = new System.Windows.Forms.Padding(2, 6, 3, 3);
            this.pnlSpectrogram.Name = "pnlSpectrogram";
            this.pnlSpectrogram.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlSpectrogram.RowCount = 5;
            this.pnlSpectrogram.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSpectrogram.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSpectrogram.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlSpectrogram.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSpectrogram.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSpectrogram.Size = new System.Drawing.Size(439, 175);
            this.pnlSpectrogram.TabIndex = 6;
            // 
            // opmLabel10
            // 
            this.opmLabel10.AutoSize = true;
            this.opmLabel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel10.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel10.Location = new System.Drawing.Point(0, 12);
            this.opmLabel10.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel10.Name = "opmLabel10";
            this.opmLabel10.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel10.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel10.Size = new System.Drawing.Size(34, 12);
            this.opmLabel10.TabIndex = 6;
            this.opmLabel10.Text = "0 dBM";
            this.opmLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opmLabel11
            // 
            this.opmLabel11.AutoSize = true;
            this.opmLabel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel11.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel11.Location = new System.Drawing.Point(0, 148);
            this.opmLabel11.Margin = new System.Windows.Forms.Padding(0, 5, 2, 15);
            this.opmLabel11.Name = "opmLabel11";
            this.opmLabel11.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel11.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel11.Size = new System.Drawing.Size(34, 12);
            this.opmLabel11.TabIndex = 7;
            this.opmLabel11.Text = "-6 dBM";
            this.opmLabel11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // opmLabel5
            // 
            this.opmLabel5.AutoSize = true;
            this.opmLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel5.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel5.Location = new System.Drawing.Point(0, 24);
            this.opmLabel5.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel5.Name = "opmLabel5";
            this.opmLabel5.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel5.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel5.Size = new System.Drawing.Size(34, 119);
            this.opmLabel5.TabIndex = 10;
            this.opmLabel5.Text = "-3 dBM";
            this.opmLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSignalSpectrum
            // 
            this.lblSignalSpectrum.AutoSize = true;
            this.pnlSpectrogram.SetColumnSpan(this.lblSignalSpectrum, 3);
            this.lblSignalSpectrum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSignalSpectrum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSignalSpectrum.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.lblSignalSpectrum.Location = new System.Drawing.Point(39, 0);
            this.lblSignalSpectrum.Name = "lblSignalSpectrum";
            this.lblSignalSpectrum.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSignalSpectrum.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblSignalSpectrum.Size = new System.Drawing.Size(397, 12);
            this.lblSignalSpectrum.TabIndex = 11;
            this.lblSignalSpectrum.Text = "TXT_SIGNALSPECTRUM";
            // 
            // pnlVuMeter
            // 
            this.pnlVuMeter.AutoSize = true;
            this.pnlVuMeter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlVuMeter.ColumnCount = 3;
            this.pnlVuMeter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlVuMeter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlVuMeter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlVuMeter.Controls.Add(this.opmLabel4, 2, 2);
            this.pnlVuMeter.Controls.Add(this.opmLabel3, 2, 1);
            this.pnlVuMeter.Controls.Add(this.vuRight, 1, 2);
            this.pnlVuMeter.Controls.Add(this.vuLeft, 1, 1);
            this.pnlVuMeter.Controls.Add(this.opmLabel1, 0, 1);
            this.pnlVuMeter.Controls.Add(this.opmLabel2, 0, 2);
            this.pnlVuMeter.Controls.Add(this.lblSignalLevel, 1, 0);
            this.pnlVuMeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVuMeter.Location = new System.Drawing.Point(17, 5);
            this.pnlVuMeter.Margin = new System.Windows.Forms.Padding(17, 5, 0, 3);
            this.pnlVuMeter.Name = "pnlVuMeter";
            this.pnlVuMeter.RowCount = 3;
            this.pnlVuMeter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlVuMeter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlVuMeter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlVuMeter.Size = new System.Drawing.Size(427, 72);
            this.pnlVuMeter.TabIndex = 3;
            // 
            // opmLabel4
            // 
            this.opmLabel4.AutoSize = true;
            this.opmLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel4.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel4.Location = new System.Drawing.Point(400, 42);
            this.opmLabel4.Margin = new System.Windows.Forms.Padding(0);
            this.opmLabel4.Name = "opmLabel4";
            this.opmLabel4.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel4.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel4.Size = new System.Drawing.Size(27, 30);
            this.opmLabel4.TabIndex = 5;
            this.opmLabel4.Text = "100%";
            this.opmLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel3.Location = new System.Drawing.Point(400, 12);
            this.opmLabel3.Margin = new System.Windows.Forms.Padding(0);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(27, 30);
            this.opmLabel3.TabIndex = 4;
            this.opmLabel3.Text = "100%";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel1.Location = new System.Drawing.Point(0, 12);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(19, 30);
            this.opmLabel1.TabIndex = 2;
            this.opmLabel1.Text = "L: 0";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel2.Location = new System.Drawing.Point(0, 42);
            this.opmLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(19, 30);
            this.opmLabel2.TabIndex = 3;
            this.opmLabel2.Text = "R: 0";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSignalLevel
            // 
            this.lblSignalLevel.AutoSize = true;
            this.pnlVuMeter.SetColumnSpan(this.lblSignalLevel, 2);
            this.lblSignalLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSignalLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSignalLevel.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.lblSignalLevel.Location = new System.Drawing.Point(22, 0);
            this.lblSignalLevel.Name = "lblSignalLevel";
            this.lblSignalLevel.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSignalLevel.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblSignalLevel.Size = new System.Drawing.Size(402, 12);
            this.lblSignalLevel.TabIndex = 6;
            this.lblSignalLevel.Text = "TXT_SIGNALLEVEL";
            // 
            // pnlWaveform
            // 
            this.pnlWaveform.ColumnCount = 2;
            this.pnlWaveform.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlWaveform.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlWaveform.Controls.Add(this.gpWaveform, 1, 1);
            this.pnlWaveform.Controls.Add(this.opmLabel7, 0, 1);
            this.pnlWaveform.Controls.Add(this.opmLabel8, 0, 2);
            this.pnlWaveform.Controls.Add(this.opmLabel9, 0, 3);
            this.pnlWaveform.Controls.Add(this.lblSignalWaveform, 1, 0);
            this.pnlWaveform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWaveform.Location = new System.Drawing.Point(6, 86);
            this.pnlWaveform.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
            this.pnlWaveform.Name = "pnlWaveform";
            this.pnlWaveform.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlWaveform.RowCount = 4;
            this.pnlWaveform.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlWaveform.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlWaveform.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlWaveform.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlWaveform.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlWaveform.Size = new System.Drawing.Size(435, 174);
            this.pnlWaveform.TabIndex = 5;
            // 
            // gpWaveform
            // 
            this.gpWaveform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpWaveform.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.gpWaveform.IsHistogram = false;
            this.gpWaveform.Location = new System.Drawing.Point(32, 12);
            this.gpWaveform.LogarithmicXAxis = false;
            this.gpWaveform.LogarithmicYAxis = false;
            this.gpWaveform.Margin = new System.Windows.Forms.Padding(0);
            this.gpWaveform.MaxVal = null;
            this.gpWaveform.MinVal = null;
            this.gpWaveform.Name = "gpWaveform";
            this.gpWaveform.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlWaveform.SetRowSpan(this.gpWaveform, 3);
            this.gpWaveform.ShowDecadicLines = false;
            this.gpWaveform.ShowXAxis = true;
            this.gpWaveform.ShowYAxis = false;
            this.gpWaveform.Size = new System.Drawing.Size(403, 162);
            this.gpWaveform.TabIndex = 2;
            // 
            // opmLabel7
            // 
            this.opmLabel7.AutoSize = true;
            this.opmLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel7.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel7.Location = new System.Drawing.Point(0, 12);
            this.opmLabel7.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel7.Name = "opmLabel7";
            this.opmLabel7.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel7.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel7.Size = new System.Drawing.Size(30, 12);
            this.opmLabel7.TabIndex = 3;
            this.opmLabel7.Text = "+MAX";
            this.opmLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opmLabel8
            // 
            this.opmLabel8.AutoSize = true;
            this.opmLabel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel8.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel8.Location = new System.Drawing.Point(0, 24);
            this.opmLabel8.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel8.Name = "opmLabel8";
            this.opmLabel8.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel8.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel8.Size = new System.Drawing.Size(30, 138);
            this.opmLabel8.TabIndex = 4;
            this.opmLabel8.Text = "0";
            this.opmLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opmLabel9
            // 
            this.opmLabel9.AutoSize = true;
            this.opmLabel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel9.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel9.Location = new System.Drawing.Point(0, 162);
            this.opmLabel9.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel9.Name = "opmLabel9";
            this.opmLabel9.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel9.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel9.Size = new System.Drawing.Size(30, 12);
            this.opmLabel9.TabIndex = 5;
            this.opmLabel9.Text = "-MAX";
            this.opmLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSignalWaveform
            // 
            this.lblSignalWaveform.AutoSize = true;
            this.lblSignalWaveform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSignalWaveform.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSignalWaveform.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.lblSignalWaveform.Location = new System.Drawing.Point(35, 0);
            this.lblSignalWaveform.Name = "lblSignalWaveform";
            this.lblSignalWaveform.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSignalWaveform.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblSignalWaveform.Size = new System.Drawing.Size(397, 12);
            this.lblSignalWaveform.TabIndex = 6;
            this.lblSignalWaveform.Text = "TXT_SIGNALWAVEFORM";
            // 
            // opmContextMenuStrip1
            // 
            this.opmContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.opmContextMenuStrip1.ForeColor = System.Drawing.Color.Black;
            this.opmContextMenuStrip1.Name = "opmContextMenuStrip1";
            this.opmContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // spSpectrogram
            // 
            this.pnlSpectrogram.SetColumnSpan(this.spSpectrogram, 3);
            this.spSpectrogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spSpectrogram.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.spSpectrogram.IsHistogram = true;
            this.spSpectrogram.Location = new System.Drawing.Point(36, 12);
            this.spSpectrogram.LogarithmicXAxis = false;
            this.spSpectrogram.LogarithmicYAxis = false;
            this.spSpectrogram.Margin = new System.Windows.Forms.Padding(0);
            this.spSpectrogram.MaxVal = null;
            this.spSpectrogram.MinVal = null;
            this.spSpectrogram.Name = "spSpectrogram";
            this.spSpectrogram.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlSpectrogram.SetRowSpan(this.spSpectrogram, 3);
            this.spSpectrogram.ShowDecadicLines = true;
            this.spSpectrogram.ShowXAxis = true;
            this.spSpectrogram.ShowYAxis = false;
            this.spSpectrogram.Size = new System.Drawing.Size(403, 163);
            this.spSpectrogram.TabIndex = 3;
            // 
            // vuRight
            // 
            this.vuRight.AllowDragging = false;
            this.vuRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vuRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vuRight.Enabled = false;
            this.vuRight.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.vuRight.Location = new System.Drawing.Point(22, 45);
            this.vuRight.Margin = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.vuRight.Maximum = 10000D;
            this.vuRight.Name = "vuRight";
            this.vuRight.NrTicks = 10;
            this.vuRight.OverrideBackColor = System.Drawing.Color.Empty;
            this.vuRight.OverrideElapsedBackColor = System.Drawing.Color.Empty;
            this.vuRight.ShowTicks = true;
            this.vuRight.Size = new System.Drawing.Size(375, 23);
            this.vuRight.TabIndex = 1;
            this.vuRight.Value = 0D;
            this.vuRight.Vertical = false;
            // 
            // vuLeft
            // 
            this.vuLeft.AllowDragging = false;
            this.vuLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vuLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vuLeft.Enabled = false;
            this.vuLeft.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.vuLeft.Location = new System.Drawing.Point(22, 15);
            this.vuLeft.Maximum = 10000D;
            this.vuLeft.Name = "vuLeft";
            this.vuLeft.NrTicks = 10;
            this.vuLeft.OverrideBackColor = System.Drawing.Color.Empty;
            this.vuLeft.OverrideElapsedBackColor = System.Drawing.Color.Empty;
            this.vuLeft.ShowTicks = true;
            this.vuLeft.Size = new System.Drawing.Size(375, 24);
            this.vuLeft.TabIndex = 0;
            this.vuLeft.Value = 0D;
            this.vuLeft.Vertical = false;
            // 
            // SignalAnalysisScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SignalAnalysisScreen";
            this.Size = new System.Drawing.Size(444, 447);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.pnlSpectrogram.ResumeLayout(false);
            this.pnlSpectrogram.PerformLayout();
            this.pnlVuMeter.ResumeLayout(false);
            this.pnlVuMeter.PerformLayout();
            this.pnlWaveform.ResumeLayout(false);
            this.pnlWaveform.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VuMeterGauge vuLeft;
        private VuMeterGauge vuRight;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.GraphPlotter gpWaveform;
        private System.Windows.Forms.TableLayoutPanel pnlVuMeter;
        private UI.Controls.OPMLabel opmLabel4;
        private UI.Controls.OPMLabel opmLabel3;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMLabel opmLabel2;
        private UI.Controls.OPMContextMenuStrip opmContextMenuStrip1;
        private UI.Controls.OPMTableLayoutPanel pnlWaveform;
        private UI.Controls.OPMLabel opmLabel7;
        private UI.Controls.OPMLabel opmLabel8;
        private UI.Controls.OPMLabel opmLabel9;
        private UI.Controls.OPMLabel lblSignalLevel;
        private UI.Controls.OPMLabel lblSignalWaveform;
        private UI.Controls.OPMTableLayoutPanel pnlSpectrogram;
        private UI.Controls.OPMLabel opmLabel10;
        private SpectrogramPlotter spSpectrogram;
        private UI.Controls.OPMLabel opmLabel11;
        private UI.Controls.OPMLabel opmLabel5;
        private UI.Controls.OPMLabel lblSignalSpectrum;


    }
}
