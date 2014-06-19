﻿using OPMedia.UI.Controls;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.Controls.BookmarkManagement
{
    partial class BookmarkScreen
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
            this.lblItem = new OPMedia.UI.Controls.OPMLabel();
            this.lvBookmarks = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblDesc = new OPMedia.UI.Controls.OPMLabel();
            this.pbAddCurrent = new System.Windows.Forms.PictureBox();
            this.pbAdd = new System.Windows.Forms.PictureBox();
            this.pbDelete = new System.Windows.Forms.PictureBox();
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.pnlButtons = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.playlistScreen = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistScreen();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).BeginInit();
            this.pnlLayout.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.pnlLayout.SetColumnSpan(this.lblItem, 3);
            this.lblItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblItem.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.lblItem.Location = new System.Drawing.Point(0, 3);
            this.lblItem.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.lblItem.Name = "lblItem";
            this.lblItem.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblItem.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblItem.Size = new System.Drawing.Size(327, 13);
            this.lblItem.TabIndex = 1;
            this.lblItem.Text = "[ item ]";
            this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvBookmarks
            // 
            this.lvBookmarks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colTime,
            this.colText});
            this.lvBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvBookmarks.Font = new System.Drawing.Font("Segoe UI", 6.75F);
            this.lvBookmarks.Location = new System.Drawing.Point(153, 19);
            this.lvBookmarks.Margin = new System.Windows.Forms.Padding(0);
            this.lvBookmarks.MultiSelect = false;
            this.lvBookmarks.Name = "lvBookmarks";
            this.lvBookmarks.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvBookmarks.Size = new System.Drawing.Size(153, 236);
            this.lvBookmarks.TabIndex = 2;
            this.lvBookmarks.UseCompatibleStateImageBehavior = false;
            this.lvBookmarks.View = System.Windows.Forms.View.Details;
            this.lvBookmarks.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvBookmarks_MouseDoubleClick);
            // 
            // colEmpty
            // 
            this.colEmpty.Name = "colEmpty";
            this.colEmpty.Text = "";
            this.colEmpty.Width = 0;
            // 
            // colTime
            // 
            this.colTime.Name = "colTime";
            this.colTime.Text = "TXT_BOOKMARK_TIME";
            this.colTime.Width = 106;
            // 
            // colText
            // 
            this.colText.Name = "colText";
            this.colText.Text = "TXT_BOOKMARK_DESC";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDesc.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.lblDesc.Location = new System.Drawing.Point(153, 255);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblDesc.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblDesc.Size = new System.Drawing.Size(153, 12);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "TXT_CLICK_LIST_TO_EDIT";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbAddCurrent
            // 
            this.pbAddCurrent.Location = new System.Drawing.Point(2, 22);
            this.pbAddCurrent.Margin = new System.Windows.Forms.Padding(2);
            this.pbAddCurrent.Name = "pbAddCurrent";
            this.pbAddCurrent.Size = new System.Drawing.Size(16, 16);
            this.pbAddCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddCurrent.TabIndex = 0;
            this.pbAddCurrent.TabStop = false;
            this.pbAddCurrent.Tag = "TXT_NEWNOW_BMDESC";
            this.pbAddCurrent.Text = "...";
            this.pbAddCurrent.Click += new System.EventHandler(this.btnAddCurrent_Click);
            // 
            // pbAdd
            // 
            this.pbAdd.Location = new System.Drawing.Point(2, 2);
            this.pbAdd.Margin = new System.Windows.Forms.Padding(2);
            this.pbAdd.Name = "pbAdd";
            this.pbAdd.Size = new System.Drawing.Size(16, 16);
            this.pbAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAdd.TabIndex = 1;
            this.pbAdd.TabStop = false;
            this.pbAdd.Tag = "TXT_NEW_BMDESC";
            this.pbAdd.Text = "...";
            this.pbAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pbDelete
            // 
            this.pbDelete.Location = new System.Drawing.Point(2, 42);
            this.pbDelete.Margin = new System.Windows.Forms.Padding(2);
            this.pbDelete.Name = "pbDelete";
            this.pbDelete.Size = new System.Drawing.Size(16, 16);
            this.pbDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDelete.TabIndex = 2;
            this.pbDelete.TabStop = false;
            this.pbDelete.Tag = "TXT_DELETE_BMDESC";
            this.pbDelete.Text = "...";
            this.pbDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // opmTableLayoutPanel1
            // 
            this.pnlLayout.ColumnCount = 3;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.Controls.Add(this.lblItem, 0, 0);
            this.pnlLayout.Controls.Add(this.lvBookmarks, 1, 1);
            this.pnlLayout.Controls.Add(this.pnlButtons, 2, 1);
            this.pnlLayout.Controls.Add(this.lblDesc, 1, 2);
            this.pnlLayout.Controls.Add(this.playlistScreen, 0, 1);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Name = "opmTableLayoutPanel1";
            this.pnlLayout.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlLayout.RowCount = 3;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.Size = new System.Drawing.Size(327, 267);
            this.pnlLayout.TabIndex = 1;
            // 
            // opmFlowLayoutPanel1
            // 
            this.pnlButtons.AutoSize = true;
            this.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlButtons.Controls.Add(this.pbAdd);
            this.pnlButtons.Controls.Add(this.pbAddCurrent);
            this.pnlButtons.Controls.Add(this.pbDelete);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlButtons.Location = new System.Drawing.Point(306, 39);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.pnlButtons.Name = "opmFlowLayoutPanel1";
            this.pnlButtons.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlButtons.Size = new System.Drawing.Size(21, 216);
            this.pnlButtons.TabIndex = 3;
            // 
            // playlistScreen
            // 
            this.playlistScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playlistScreen.CompactMode = true;
            this.playlistScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistScreen.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.playlistScreen.Location = new System.Drawing.Point(0, 19);
            this.playlistScreen.Margin = new System.Windows.Forms.Padding(0);
            this.playlistScreen.Name = "playlistScreen";
            this.playlistScreen.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlLayout.SetRowSpan(this.playlistScreen, 2);
            this.playlistScreen.Size = new System.Drawing.Size(153, 248);
            this.playlistScreen.TabIndex = 5;
            // 
            // BookmarkScreen
            // 
            this.Controls.Add(this.pnlLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BookmarkScreen";
            this.Size = new System.Drawing.Size(327, 267);
            ((System.ComponentModel.ISupportInitialize)(this.pbAddCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).EndInit();
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OPMLabel lblItem;
        private OPMListView lvBookmarks;
        private OPMLabel lblDesc;
        private PictureBox pbAddCurrent;
        private PictureBox pbAdd;
        private PictureBox pbDelete;
        private ColumnHeader colIcon;
        private ColumnHeader colTime;
        private ColumnHeader colText;
        private OPMTableLayoutPanel pnlLayout;
        private OPMFlowLayoutPanel pnlButtons;
        private ColumnHeader colEmpty;
        private MediaPlayer.PlaylistScreen playlistScreen;
    }
}