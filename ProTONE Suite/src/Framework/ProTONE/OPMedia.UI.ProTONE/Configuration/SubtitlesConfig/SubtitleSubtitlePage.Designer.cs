﻿using OPMedia.UI.Controls;
using System.Windows.Forms;
namespace OPMedia.UI.ProTONE.Configuration
{
    partial class SubtitleSubtitlePage
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
            this.lvDownloadAddresses = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPassword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbLanguages = new OPMedia.UI.Controls.OPMComboBox();
            this.chkSubtitleDownload = new OPMedia.UI.Controls.OPMCheckBox();
            this.pnlOnlineSubtitles = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.flowLayoutPanel2 = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.kryptonLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.btnAdd = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.PictureBox();
            this.btnMoveUp = new System.Windows.Forms.PictureBox();
            this.btnMoveDown = new System.Windows.Forms.PictureBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblClickHint = new OPMedia.UI.Controls.OPMLabel();
            this.btnRestoreDefaults = new OPMedia.UI.Controls.OPMButton();
            this.tableLayoutPanel3 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblMinDuration = new OPMedia.UI.Controls.OPMLabel();
            this.nudMinMovieDuration = new OPMedia.UI.Controls.OPMNumericUpDown();
            this.kryptonLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.chkNotifySubDownloaded = new OPMedia.UI.Controls.OPMCheckBox();
            this.tableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.tableLayoutPanel2 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.pnlOnlineSubtitles.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMoveUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMoveDown)).BeginInit();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinMovieDuration)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDownloadAddresses
            // 
            this.lvDownloadAddresses.AccessibleName = "lvDownloadAddresses";
            this.lvDownloadAddresses.AllowEditing = true;
            this.lvDownloadAddresses.AlternateRowColors = true;
            this.lvDownloadAddresses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvDownloadAddresses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colServerType,
            this.colServerUrl,
            this.colActive,
            this.colUserName,
            this.colPassword});
            this.lvDownloadAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDownloadAddresses.Location = new System.Drawing.Point(0, 0);
            this.lvDownloadAddresses.Margin = new System.Windows.Forms.Padding(0);
            this.lvDownloadAddresses.MultiSelect = false;
            this.lvDownloadAddresses.Name = "lvDownloadAddresses";
            this.lvDownloadAddresses.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvDownloadAddresses.Size = new System.Drawing.Size(521, 96);
            this.lvDownloadAddresses.TabIndex = 0;
            this.lvDownloadAddresses.UseCompatibleStateImageBehavior = false;
            this.lvDownloadAddresses.View = System.Windows.Forms.View.Details;
            this.lvDownloadAddresses.SelectedIndexChanged += new System.EventHandler(this.lvDownloadAddresses_SelectedIndexChanged);
            // 
            // colEmpty
            // 
            this.colEmpty.Name = "colEmpty";
            this.colEmpty.Text = "TXT_SERVERTYPE";
            this.colEmpty.Width = 0;
            // 
            // colServerType
            // 
            this.colServerType.Name = "colServerType";
            this.colServerType.Text = "TXT_SERVERTYPE";
            this.colServerType.Width = 119;
            // 
            // colServerUrl
            // 
            this.colServerUrl.Name = "colServerUrl";
            this.colServerUrl.Text = "TXT_SERVERURL";
            this.colServerUrl.Width = 128;
            // 
            // colActive
            // 
            this.colActive.Name = "colActive";
            this.colActive.Text = "TXT_ACTIVE";
            this.colActive.Width = 100;
            // 
            // colUserName
            // 
            this.colUserName.Name = "colUserName";
            this.colUserName.Text = "TXT_USERNAME";
            // 
            // colPassword
            // 
            this.colPassword.Name = "colPassword";
            this.colPassword.Text = "TXT_PASSWORD";
            // 
            // label3
            // 
            this.label3.AccessibleName = "label3";
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(0, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.label3.Name = "label3";
            this.label3.OverrideBackColor = System.Drawing.Color.Empty;
            this.label3.OverrideForeColor = System.Drawing.Color.Empty;
            this.label3.Size = new System.Drawing.Size(185, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "TXT_PREFFEREDSUBTITLELANG";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbLanguages
            // 
            this.cmbLanguages.AccessibleName = "cmbLanguages";
            this.cmbLanguages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbLanguages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbLanguages.FormattingEnabled = true;
            this.cmbLanguages.Location = new System.Drawing.Point(191, 0);
            this.cmbLanguages.Margin = new System.Windows.Forms.Padding(0);
            this.cmbLanguages.Name = "cmbLanguages";
            this.cmbLanguages.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbLanguages.Size = new System.Drawing.Size(355, 24);
            this.cmbLanguages.TabIndex = 1;
            // 
            // chkSubtitleDownload
            // 
            this.chkSubtitleDownload.AccessibleName = "chkSubtitleDownload";
            this.chkSubtitleDownload.AutoSize = true;
            this.chkSubtitleDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkSubtitleDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSubtitleDownload.Location = new System.Drawing.Point(0, 37);
            this.chkSubtitleDownload.Margin = new System.Windows.Forms.Padding(0, 12, 0, 6);
            this.chkSubtitleDownload.Name = "chkSubtitleDownload";
            this.chkSubtitleDownload.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkSubtitleDownload.Size = new System.Drawing.Size(546, 19);
            this.chkSubtitleDownload.TabIndex = 1;
            this.chkSubtitleDownload.Text = "TXT_SUBTITLE_DOWNLOAD";
            // 
            // pnlOnlineSubtitles
            // 
            this.pnlOnlineSubtitles.AutoSize = true;
            this.pnlOnlineSubtitles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlOnlineSubtitles.ColumnCount = 2;
            this.pnlOnlineSubtitles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlOnlineSubtitles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlOnlineSubtitles.Controls.Add(this.lvDownloadAddresses, 0, 0);
            this.pnlOnlineSubtitles.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.pnlOnlineSubtitles.Controls.Add(this.opmTableLayoutPanel1, 0, 1);
            this.pnlOnlineSubtitles.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.pnlOnlineSubtitles.Controls.Add(this.chkNotifySubDownloaded, 0, 4);
            this.pnlOnlineSubtitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOnlineSubtitles.Location = new System.Drawing.Point(0, 62);
            this.pnlOnlineSubtitles.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOnlineSubtitles.Name = "pnlOnlineSubtitles";
            this.pnlOnlineSubtitles.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlOnlineSubtitles.RowCount = 6;
            this.pnlOnlineSubtitles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.pnlOnlineSubtitles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlOnlineSubtitles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlOnlineSubtitles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlOnlineSubtitles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlOnlineSubtitles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlOnlineSubtitles.Size = new System.Drawing.Size(546, 275);
            this.pnlOnlineSubtitles.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AccessibleName = "flowLayoutPanel2";
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.kryptonLabel1);
            this.flowLayoutPanel2.Controls.Add(this.btnAdd);
            this.flowLayoutPanel2.Controls.Add(this.btnDelete);
            this.flowLayoutPanel2.Controls.Add(this.btnMoveUp);
            this.flowLayoutPanel2.Controls.Add(this.btnMoveDown);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(521, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.flowLayoutPanel2.Size = new System.Drawing.Size(25, 96);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.AccessibleName = "kryptonLabel1";
            this.kryptonLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kryptonLabel1.Location = new System.Drawing.Point(3, 0);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.kryptonLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.kryptonLabel1.Size = new System.Drawing.Size(7, 2);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(19, 18);
            this.btnAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAdd.TabIndex = 5;
            this.btnAdd.TabStop = false;
            this.btnAdd.Click += new System.EventHandler(this.OnAdd);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(3, 24);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(19, 18);
            this.btnDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnDelete.TabIndex = 6;
            this.btnDelete.TabStop = false;
            this.btnDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(3, 44);
            this.btnMoveUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(19, 18);
            this.btnMoveUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMoveUp.TabIndex = 7;
            this.btnMoveUp.TabStop = false;
            this.btnMoveUp.Click += new System.EventHandler(this.OnMoveUp);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(3, 64);
            this.btnMoveDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(19, 18);
            this.btnMoveDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMoveDown.TabIndex = 8;
            this.btnMoveDown.TabStop = false;
            this.btnMoveDown.Click += new System.EventHandler(this.OnMoveDown);
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.AutoSize = true;
            this.opmTableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmTableLayoutPanel1.ColumnCount = 2;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.Controls.Add(this.lblClickHint, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.btnRestoreDefaults, 1, 0);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 99);
            this.opmTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 1;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(521, 25);
            this.opmTableLayoutPanel1.TabIndex = 30;
            // 
            // lblClickHint
            // 
            this.lblClickHint.AccessibleName = "lblClickHint";
            this.lblClickHint.AutoSize = true;
            this.lblClickHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClickHint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblClickHint.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.lblClickHint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblClickHint.Location = new System.Drawing.Point(0, 0);
            this.lblClickHint.Margin = new System.Windows.Forms.Padding(0);
            this.lblClickHint.Name = "lblClickHint";
            this.lblClickHint.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblClickHint.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblClickHint.Size = new System.Drawing.Size(343, 25);
            this.lblClickHint.TabIndex = 0;
            this.lblClickHint.Text = "TXT_CLICK_LIST_TO_EDIT";
            this.lblClickHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRestoreDefaults
            // 
            this.btnRestoreDefaults.AutoSize = true;
            this.btnRestoreDefaults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreDefaults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRestoreDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreDefaults.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.btnRestoreDefaults.Location = new System.Drawing.Point(343, 0);
            this.btnRestoreDefaults.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestoreDefaults.Name = "btnRestoreDefaults";
            this.btnRestoreDefaults.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnRestoreDefaults.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnRestoreDefaults.ShowDropDown = false;
            this.btnRestoreDefaults.Size = new System.Drawing.Size(178, 25);
            this.btnRestoreDefaults.TabIndex = 1;
            this.btnRestoreDefaults.Text = "TXT_RESTORE_DEFAULTSERVERS";
            this.btnRestoreDefaults.UseVisualStyleBackColor = true;
            this.btnRestoreDefaults.Click += new System.EventHandler(this.btnRestoreDefaults_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AccessibleName = "flowLayoutPanel3";
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.pnlOnlineSubtitles.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.lblMinDuration);
            this.tableLayoutPanel3.Controls.Add(this.nudMinMovieDuration);
            this.tableLayoutPanel3.Controls.Add(this.kryptonLabel2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 141);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 17, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(546, 23);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lblMinDuration
            // 
            this.lblMinDuration.AccessibleName = "lblMinDuration";
            this.lblMinDuration.AutoSize = true;
            this.lblMinDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMinDuration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMinDuration.Location = new System.Drawing.Point(0, 0);
            this.lblMinDuration.Margin = new System.Windows.Forms.Padding(0);
            this.lblMinDuration.Name = "lblMinDuration";
            this.lblMinDuration.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblMinDuration.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblMinDuration.Size = new System.Drawing.Size(150, 23);
            this.lblMinDuration.TabIndex = 0;
            this.lblMinDuration.Text = "TXT_MINMOVIEDURATION";
            this.lblMinDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMinMovieDuration
            // 
            this.nudMinMovieDuration.AccessibleName = "nudMinMovieDuration";
            this.nudMinMovieDuration.AutoSize = true;
            this.nudMinMovieDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMinMovieDuration.Location = new System.Drawing.Point(150, 0);
            this.nudMinMovieDuration.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.nudMinMovieDuration.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudMinMovieDuration.Name = "nudMinMovieDuration";
            this.nudMinMovieDuration.Size = new System.Drawing.Size(35, 23);
            this.nudMinMovieDuration.TabIndex = 1;
            this.nudMinMovieDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMinMovieDuration.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.AccessibleName = "kryptonLabel2";
            this.kryptonLabel2.AutoSize = true;
            this.kryptonLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kryptonLabel2.Location = new System.Drawing.Point(191, 0);
            this.kryptonLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.kryptonLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.kryptonLabel2.Size = new System.Drawing.Size(355, 23);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Text = "TXT_MINUTES";
            this.kryptonLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkNotifySubDownloaded
            // 
            this.chkNotifySubDownloaded.AutoSize = true;
            this.pnlOnlineSubtitles.SetColumnSpan(this.chkNotifySubDownloaded, 2);
            this.chkNotifySubDownloaded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkNotifySubDownloaded.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNotifySubDownloaded.Location = new System.Drawing.Point(0, 181);
            this.chkNotifySubDownloaded.Margin = new System.Windows.Forms.Padding(0, 17, 3, 3);
            this.chkNotifySubDownloaded.Name = "chkNotifySubDownloaded";
            this.chkNotifySubDownloaded.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkNotifySubDownloaded.Size = new System.Drawing.Size(543, 19);
            this.chkNotifySubDownloaded.TabIndex = 2;
            this.chkNotifySubDownloaded.Text = "TXT_NOTIFYSUBDOWNLOADED";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkSubtitleDownload, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlOnlineSubtitles, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(546, 337);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.cmbLanguages, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(546, 25);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // SubtitleSubtitlePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SubtitleSubtitlePage";
            this.Size = new System.Drawing.Size(546, 337);
            this.pnlOnlineSubtitles.ResumeLayout(false);
            this.pnlOnlineSubtitles.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMoveUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMoveDown)).EndInit();
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinMovieDuration)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OPMTableLayoutPanel tableLayoutPanel1;
        private OPMTableLayoutPanel pnlOnlineSubtitles;
        private OPMTableLayoutPanel tableLayoutPanel3;
        private OPMLabel lblMinDuration;
        private OPMNumericUpDown nudMinMovieDuration;
        private OPMLabel kryptonLabel2;
        private OPMListView lvDownloadAddresses;
        private System.Windows.Forms.ColumnHeader colEmpty;
        private System.Windows.Forms.ColumnHeader colActive;
        private System.Windows.Forms.ColumnHeader colServerUrl;
        private System.Windows.Forms.ColumnHeader colServerType;
        private System.Windows.Forms.ColumnHeader colUserName;
        private System.Windows.Forms.ColumnHeader colPassword;
        private OPMFlowLayoutPanel flowLayoutPanel2;
        private OPMLabel kryptonLabel1;
        private OPMCheckBox chkSubtitleDownload;
        private OPMLabel label3;
        private OPMComboBox cmbLanguages;
        private OPMTableLayoutPanel tableLayoutPanel2;
        private OPMCheckBox chkNotifySubDownloaded;
        private PictureBox btnAdd;
        private PictureBox btnDelete;
        private PictureBox btnMoveUp;
        private PictureBox btnMoveDown;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
        private OPMLabel lblClickHint;
        private OPMButton btnRestoreDefaults;
    }
}
