﻿namespace OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Forms
{
    partial class WizCdRipperStep1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizCdRipperStep1));
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbAudioCDDrives = new OPMedia.UI.Controls.OPMComboBox();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.lvTracks = new OPMedia.UI.Controls.OPMListView();
            this.colTrackNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSizeBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefresh = new OPMedia.UI.Controls.OPMButton();
            this.opmFlowLayoutPanel1 = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.llCheckAll = new OPMedia.UI.Controls.OPMLinkLabel();
            this.llUncheckAll = new OPMedia.UI.Controls.OPMLinkLabel();
            this.pbWaiting = new OPMedia.UI.Controls.WaitingPictureBox();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.opmFlowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWaiting)).BeginInit();
            this.SuspendLayout();
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 3;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel3, 0, 4);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbAudioCDDrives, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel2, 0, 2);
            this.opmTableLayoutPanel1.Controls.Add(this.lvTracks, 0, 3);
            this.opmTableLayoutPanel1.Controls.Add(this.btnRefresh, 1, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.opmFlowLayoutPanel1, 2, 4);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 5;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(700, 462);
            this.opmTableLayoutPanel1.TabIndex = 0;
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmTableLayoutPanel1.SetColumnSpan(this.opmLabel3, 2);
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel3.Location = new System.Drawing.Point(6, 443);
            this.opmLabel3.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(480, 19);
            this.opmLabel3.TabIndex = 5;
            this.opmLabel3.Text = "TXT_GRABBERHINT_STEP1";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmTableLayoutPanel1.SetColumnSpan(this.opmLabel1, 3);
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.Location = new System.Drawing.Point(0, 3);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(697, 15);
            this.opmLabel1.TabIndex = 0;
            this.opmLabel1.Text = "TXT_SELECTDRIVE";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbAudioCDDrives
            // 
            this.cmbAudioCDDrives.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAudioCDDrives.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbAudioCDDrives.FormattingEnabled = true;
            this.cmbAudioCDDrives.Location = new System.Drawing.Point(0, 27);
            this.cmbAudioCDDrives.Margin = new System.Windows.Forms.Padding(0, 6, 0, 3);
            this.cmbAudioCDDrives.Name = "cmbAudioCDDrives";
            this.cmbAudioCDDrives.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbAudioCDDrives.Size = new System.Drawing.Size(458, 24);
            this.cmbAudioCDDrives.TabIndex = 1;
            this.cmbAudioCDDrives.SelectedIndexChanged += new System.EventHandler(this.OnDriveSelected);
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmTableLayoutPanel1.SetColumnSpan(this.opmLabel2, 3);
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.Location = new System.Drawing.Point(0, 57);
            this.opmLabel2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(697, 15);
            this.opmLabel2.TabIndex = 2;
            this.opmLabel2.Text = "TXT_SELECTTRACKS";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvTracks
            // 
            this.lvTracks.AllowEditing = true;
            this.lvTracks.AlternateRowColors = true;
            this.lvTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTrackNo,
            this.colDuration,
            this.colSizeBytes,
            this.colAlbum,
            this.colArtist,
            this.colTitle,
            this.colGenre});
            this.opmTableLayoutPanel1.SetColumnSpan(this.lvTracks, 3);
            this.lvTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTracks.Location = new System.Drawing.Point(0, 75);
            this.lvTracks.Margin = new System.Windows.Forms.Padding(0);
            this.lvTracks.MultiSelect = false;
            this.lvTracks.Name = "lvTracks";
            this.lvTracks.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvTracks.Size = new System.Drawing.Size(700, 368);
            this.lvTracks.TabIndex = 3;
            this.lvTracks.UseCompatibleStateImageBehavior = false;
            this.lvTracks.View = System.Windows.Forms.View.Details;
            this.lvTracks.DoubleClick += new System.EventHandler(this.lvTracks_DoubleClick);
            // 
            // colTrackNo
            // 
            this.colTrackNo.Text = "#";
            // 
            // colDuration
            // 
            this.colDuration.Text = "TXT_DURATION";
            // 
            // colSizeBytes
            // 
            this.colSizeBytes.Text = "TXT_SIZE";
            // 
            // colAlbum
            // 
            this.colAlbum.Text = "TXT_ALBUM";
            // 
            // colArtist
            // 
            this.colArtist.Text = "TXT_ARTIST";
            // 
            // colTitle
            // 
            this.colTitle.Text = "TXT_TITLE";
            // 
            // colGenre
            // 
            this.colGenre.Text = "TXT_GENRE";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(459, 26);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(1, 5, 0, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnRefresh.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnRefresh.ShowDropDown = false;
            this.btnRefresh.Size = new System.Drawing.Size(27, 25);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // opmFlowLayoutPanel1
            // 
            this.opmFlowLayoutPanel1.AutoSize = true;
            this.opmFlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmFlowLayoutPanel1.Controls.Add(this.llCheckAll);
            this.opmFlowLayoutPanel1.Controls.Add(this.llUncheckAll);
            this.opmFlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmFlowLayoutPanel1.Location = new System.Drawing.Point(489, 446);
            this.opmFlowLayoutPanel1.Name = "opmFlowLayoutPanel1";
            this.opmFlowLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmFlowLayoutPanel1.Size = new System.Drawing.Size(208, 13);
            this.opmFlowLayoutPanel1.TabIndex = 6;
            this.opmFlowLayoutPanel1.WrapContents = false;
            // 
            // llCheckAll
            // 
            this.llCheckAll.AutoSize = true;
            this.llCheckAll.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.llCheckAll.Location = new System.Drawing.Point(3, 0);
            this.llCheckAll.Margin = new System.Windows.Forms.Padding(3, 0, 15, 0);
            this.llCheckAll.Name = "llCheckAll";
            this.llCheckAll.Size = new System.Drawing.Size(84, 13);
            this.llCheckAll.TabIndex = 1;
            this.llCheckAll.TabStop = true;
            this.llCheckAll.Text = "TXT_CHECK_ALL";
            this.llCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llCheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnCheckAll);
            // 
            // llUncheckAll
            // 
            this.llUncheckAll.AutoSize = true;
            this.llUncheckAll.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.llUncheckAll.Location = new System.Drawing.Point(105, 0);
            this.llUncheckAll.Name = "llUncheckAll";
            this.llUncheckAll.Size = new System.Drawing.Size(100, 13);
            this.llUncheckAll.TabIndex = 0;
            this.llUncheckAll.TabStop = true;
            this.llUncheckAll.Text = "TXT_UNCHECK_ALL";
            this.llUncheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llUncheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnUncheckAll);
            // 
            // pbWaiting
            // 
            this.pbWaiting.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbWaiting.Location = new System.Drawing.Point(411, 24);
            this.pbWaiting.Margin = new System.Windows.Forms.Padding(3, 5, 0, 3);
            this.pbWaiting.Name = "pbWaiting";
            this.pbWaiting.Size = new System.Drawing.Size(24, 24);
            this.pbWaiting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbWaiting.TabIndex = 6;
            this.pbWaiting.TabStop = false;
            // 
            // WizCdRipperStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "WizCdRipperStep1";
            this.Size = new System.Drawing.Size(700, 462);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.opmFlowLayoutPanel1.ResumeLayout(false);
            this.opmFlowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWaiting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMComboBox cmbAudioCDDrives;
        private UI.Controls.OPMLabel opmLabel2;
        private UI.Controls.OPMListView lvTracks;
        private System.Windows.Forms.ColumnHeader colTrackNo;
        private System.Windows.Forms.ColumnHeader colDuration;
        private System.Windows.Forms.ColumnHeader colSizeBytes;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colArtist;
        private System.Windows.Forms.ColumnHeader colGenre;
        private UI.Controls.OPMButton btnRefresh;
        private System.Windows.Forms.ColumnHeader colAlbum;
        private UI.Controls.OPMLabel opmLabel3;
        private UI.Controls.WaitingPictureBox pbWaiting;
        private UI.Controls.OPMLinkLabel llUncheckAll;
        private UI.Controls.OPMFlowLayoutPanel opmFlowLayoutPanel1;
        private UI.Controls.OPMLinkLabel llCheckAll;
    }
}
