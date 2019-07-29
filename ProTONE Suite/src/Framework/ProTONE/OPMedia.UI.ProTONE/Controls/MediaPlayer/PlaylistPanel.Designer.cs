using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Themes;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    partial class PlaylistPanel
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
            this.cmsPlaylist = new OPMedia.UI.Controls.OPMContextMenuStrip();
            this.dummyToolStripMenuItem = new OPMedia.UI.Controls.OPMToolStripMenuItem();
            this.lblSep2 = new System.Windows.Forms.Label();
            this.lblSep3 = new System.Windows.Forms.Label();
            this.lblSep5 = new System.Windows.Forms.Label();
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lvPlaylist = new OPMedia.UI.Controls.HeaderlessListView();
            this.colDummy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMisc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblEmptyPlaylist = new OPMedia.UI.Controls.OPMLabel();
            this.lblOpenPlaylist = new OPMedia.UI.Controls.OPMLinkLabel();
            this.lblOpenFiles = new OPMedia.UI.Controls.OPMLinkLabel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel4 = new OPMedia.UI.Controls.OPMLabel();
            this.piTotal = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piCurrent = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piPrev = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piNext = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.vuLeft = new OPMedia.UI.ProTONE.Controls.VuMeterGauge();
            this.vuRight = new OPMedia.UI.ProTONE.Controls.VuMeterGauge();
            this.cmsPlaylist.SuspendLayout();
            this.pnlLayout.SuspendLayout();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsPlaylist
            // 
            this.cmsPlaylist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.cmsPlaylist.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmsPlaylist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.cmsPlaylist.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyToolStripMenuItem});
            this.cmsPlaylist.Name = "cmsPlaylist";
            this.cmsPlaylist.Size = new System.Drawing.Size(117, 26);
            this.cmsPlaylist.Opening += new System.ComponentModel.CancelEventHandler(this.cmsPlaylist_Opening);
            // 
            // dummyToolStripMenuItem
            // 
            this.dummyToolStripMenuItem.Name = "dummyToolStripMenuItem";
            this.dummyToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.dummyToolStripMenuItem.Text = "dummy";
            // 
            // lblSep2
            // 
            this.lblSep2.AutoSize = true;
            this.lblSep2.BackColor = System.Drawing.Color.Gray;
            this.lblSep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep2.Location = new System.Drawing.Point(398, 439);
            this.lblSep2.Name = "lblSep2";
            this.lblSep2.Size = new System.Drawing.Size(274, 2);
            this.lblSep2.TabIndex = 10;
            this.lblSep2.Visible = false;
            // 
            // lblSep3
            // 
            this.lblSep3.AutoSize = true;
            this.lblSep3.BackColor = System.Drawing.Color.Gray;
            this.lblSep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep3.Location = new System.Drawing.Point(398, 461);
            this.lblSep3.Name = "lblSep3";
            this.lblSep3.Size = new System.Drawing.Size(274, 2);
            this.lblSep3.TabIndex = 11;
            // 
            // lblSep5
            // 
            this.lblSep5.AutoSize = true;
            this.lblSep5.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.lblSep5, 6);
            this.lblSep5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep5.Location = new System.Drawing.Point(3, 485);
            this.lblSep5.Name = "lblSep5";
            this.lblSep5.Size = new System.Drawing.Size(689, 2);
            this.lblSep5.TabIndex = 13;
            // 
            // pnlLayout
            // 
            this.pnlLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.ColumnCount = 6;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Controls.Add(this.lblSep5, 0, 7);
            this.pnlLayout.Controls.Add(this.lblSep3, 4, 3);
            this.pnlLayout.Controls.Add(this.lblSep2, 4, 1);
            this.pnlLayout.Controls.Add(this.piTotal, 0, 9);
            this.pnlLayout.Controls.Add(this.piCurrent, 4, 0);
            this.pnlLayout.Controls.Add(this.piPrev, 4, 4);
            this.pnlLayout.Controls.Add(this.piNext, 4, 2);
            this.pnlLayout.Controls.Add(this.label1, 3, 0);
            this.pnlLayout.Controls.Add(this.opmTableLayoutPanel1, 0, 0);
            this.pnlLayout.Controls.Add(this.vuLeft, 1, 2);
            this.pnlLayout.Controls.Add(this.vuRight, 1, 4);
            this.pnlLayout.Controls.Add(this.opmLabel1, 0, 2);
            this.pnlLayout.Controls.Add(this.opmLabel2, 0, 4);
            this.pnlLayout.Controls.Add(this.opmLabel3, 2, 2);
            this.pnlLayout.Controls.Add(this.opmLabel4, 2, 4);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlLayout.RowCount = 10;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Size = new System.Drawing.Size(695, 507);
            this.pnlLayout.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(388, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.pnlLayout.SetRowSpan(this.label1, 5);
            this.label1.Size = new System.Drawing.Size(2, 473);
            this.label1.TabIndex = 14;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 1;
            this.pnlLayout.SetColumnSpan(this.opmTableLayoutPanel1, 3);
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.lvPlaylist, 0, 3);
            this.opmTableLayoutPanel1.Controls.Add(this.lblEmptyPlaylist, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.lblOpenPlaylist, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.lblOpenFiles, 0, 2);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 4;
            this.pnlLayout.SetRowSpan(this.opmTableLayoutPanel1, 2);
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(383, 441);
            this.opmTableLayoutPanel1.TabIndex = 15;
            // 
            // lvPlaylist
            // 
            this.lvPlaylist.AllowEditing = false;
            this.lvPlaylist.AlternateRowColors = true;
            this.lvPlaylist.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvPlaylist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDummy,
            this.colMisc,
            this.colIcon,
            this.colTime,
            this.colFile});
            this.lvPlaylist.ContextMenuStrip = this.cmsPlaylist;
            this.lvPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPlaylist.Location = new System.Drawing.Point(10, 89);
            this.lvPlaylist.Margin = new System.Windows.Forms.Padding(10, 5, 0, 0);
            this.lvPlaylist.MultiSelect = false;
            this.lvPlaylist.Name = "lvPlaylist";
            this.lvPlaylist.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvPlaylist.Size = new System.Drawing.Size(373, 352);
            this.lvPlaylist.TabIndex = 5;
            this.lvPlaylist.UseCompatibleStateImageBehavior = false;
            this.lvPlaylist.View = System.Windows.Forms.View.Details;
            this.lvPlaylist.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.lvPlaylist_ItemMouseHover);
            this.lvPlaylist.SelectedIndexChanged += new System.EventHandler(this.lvPlaylist_SelectedIndexChanged);
            this.lvPlaylist.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvPlaylist_DragDrop);
            this.lvPlaylist.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvPlaylist_DragEnter);
            this.lvPlaylist.DragOver += new System.Windows.Forms.DragEventHandler(this.lvPlaylist_DragOver);
            this.lvPlaylist.DragLeave += new System.EventHandler(this.lvPlaylist_DragLeave);
            this.lvPlaylist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvPlaylist_MouseClick);
            this.lvPlaylist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPlaylist_MouseDoubleClick);
            // 
            // lblEmptyPlaylist
            // 
            this.lblEmptyPlaylist.AutoSize = true;
            this.lblEmptyPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmptyPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblEmptyPlaylist.FontSize = OPMedia.UI.Themes.FontSizes.Large;
            this.lblEmptyPlaylist.Location = new System.Drawing.Point(10, 5);
            this.lblEmptyPlaylist.Margin = new System.Windows.Forms.Padding(10, 5, 3, 10);
            this.lblEmptyPlaylist.Name = "lblEmptyPlaylist";
            this.lblEmptyPlaylist.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblEmptyPlaylist.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblEmptyPlaylist.SingleLine = true;
            this.lblEmptyPlaylist.Size = new System.Drawing.Size(370, 19);
            this.lblEmptyPlaylist.TabIndex = 6;
            this.lblEmptyPlaylist.Text = "TXT_EMPTY_PLAYLIST";
            this.lblEmptyPlaylist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOpenPlaylist
            // 
            this.lblOpenPlaylist.AutoSize = true;
            this.lblOpenPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOpenPlaylist.Location = new System.Drawing.Point(10, 34);
            this.lblOpenPlaylist.Margin = new System.Windows.Forms.Padding(10, 0, 3, 10);
            this.lblOpenPlaylist.Name = "lblOpenPlaylist";
            this.lblOpenPlaylist.Size = new System.Drawing.Size(370, 15);
            this.lblOpenPlaylist.TabIndex = 7;
            this.lblOpenPlaylist.TabStop = true;
            this.lblOpenPlaylist.Text = "TXT_LOADPLAYLIST";
            this.lblOpenPlaylist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOpenPlaylist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblOpenPlaylist_LinkClicked);
            // 
            // lblOpenFiles
            // 
            this.lblOpenFiles.AutoSize = true;
            this.lblOpenFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOpenFiles.Location = new System.Drawing.Point(10, 59);
            this.lblOpenFiles.Margin = new System.Windows.Forms.Padding(10, 0, 3, 10);
            this.lblOpenFiles.Name = "lblOpenFiles";
            this.lblOpenFiles.Size = new System.Drawing.Size(370, 15);
            this.lblOpenFiles.TabIndex = 8;
            this.lblOpenFiles.TabStop = true;
            this.lblOpenFiles.Text = "TXT_LOADMEDIAFILES";
            this.lblOpenFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOpenFiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblOpenFiles_LinkClicked);
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.opmLabel1.Location = new System.Drawing.Point(0, 448);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(60, 13);
            this.opmLabel1.TabIndex = 18;
            this.opmLabel1.Text = "LVOL:  0%";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.opmLabel2.Location = new System.Drawing.Point(0, 463);
            this.opmLabel2.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(60, 13);
            this.opmLabel2.TabIndex = 19;
            this.opmLabel2.Text = "RVOL:  0%";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.opmLabel3.Location = new System.Drawing.Point(348, 448);
            this.opmLabel3.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(35, 13);
            this.opmLabel3.TabIndex = 20;
            this.opmLabel3.Text = "100%";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLabel4
            // 
            this.opmLabel4.AutoSize = true;
            this.opmLabel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.opmLabel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel4.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.opmLabel4.Location = new System.Drawing.Point(348, 463);
            this.opmLabel4.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.opmLabel4.Name = "opmLabel4";
            this.opmLabel4.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel4.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel4.Size = new System.Drawing.Size(35, 13);
            this.opmLabel4.TabIndex = 21;
            this.opmLabel4.Text = "100%";
            this.opmLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // piTotal
            // 
            this.piTotal.AltDisplay = "";
            this.piTotal.AutoSize = true;
            this.piTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.SetColumnSpan(this.piTotal, 6);
            this.piTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piTotal.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piTotal.Item = null;
            this.piTotal.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.None;
            this.piTotal.Location = new System.Drawing.Point(0, 488);
            this.piTotal.Margin = new System.Windows.Forms.Padding(0, 1, 0, 2);
            this.piTotal.Name = "piTotal";
            this.piTotal.OverrideBackColor = System.Drawing.Color.Empty;
            this.piTotal.Size = new System.Drawing.Size(695, 17);
            this.piTotal.TabIndex = 4;
            // 
            // piCurrent
            // 
            this.piCurrent.AltDisplay = "";
            this.piCurrent.AutoSize = true;
            this.piCurrent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.piCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piCurrent.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piCurrent.Item = null;
            this.piCurrent.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.Current;
            this.piCurrent.Location = new System.Drawing.Point(395, 2);
            this.piCurrent.Margin = new System.Windows.Forms.Padding(0, 2, 0, 5);
            this.piCurrent.Name = "piCurrent";
            this.piCurrent.OverrideBackColor = System.Drawing.Color.Empty;
            this.piCurrent.Size = new System.Drawing.Size(280, 432);
            this.piCurrent.TabIndex = 7;
            // 
            // piPrev
            // 
            this.piPrev.AltDisplay = "";
            this.piPrev.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.piPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piPrev.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piPrev.Item = null;
            this.piPrev.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.Previous;
            this.piPrev.Location = new System.Drawing.Point(395, 463);
            this.piPrev.Margin = new System.Windows.Forms.Padding(0);
            this.piPrev.Name = "piPrev";
            this.piPrev.OverrideBackColor = System.Drawing.Color.Empty;
            this.piPrev.Size = new System.Drawing.Size(280, 20);
            this.piPrev.TabIndex = 6;
            // 
            // piNext
            // 
            this.piNext.AltDisplay = "";
            this.piNext.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.piNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piNext.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piNext.Item = null;
            this.piNext.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.Next;
            this.piNext.Location = new System.Drawing.Point(395, 441);
            this.piNext.Margin = new System.Windows.Forms.Padding(0);
            this.piNext.Name = "piNext";
            this.piNext.OverrideBackColor = System.Drawing.Color.Empty;
            this.piNext.Size = new System.Drawing.Size(280, 20);
            this.piNext.TabIndex = 8;
            // 
            // vuLeft
            // 
            this.vuLeft.AllowDragging = false;
            this.vuLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vuLeft.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vuLeft.EffectiveMaximum = 0D;
            this.vuLeft.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.vuLeft.Location = new System.Drawing.Point(62, 449);
            this.vuLeft.Margin = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.vuLeft.Maximum = 10000D;
            this.vuLeft.MaximumSize = new System.Drawing.Size(10000, 9);
            this.vuLeft.MinimumSize = new System.Drawing.Size(10, 9);
            this.vuLeft.Name = "vuLeft";
            this.vuLeft.NrTicks = 20;
            this.vuLeft.OverrideBackColor = System.Drawing.Color.Empty;
            this.vuLeft.OverrideElapsedBackColor = System.Drawing.Color.Empty;
            this.vuLeft.ShowTicks = true;
            this.vuLeft.Size = new System.Drawing.Size(284, 9);
            this.vuLeft.TabIndex = 16;
            this.vuLeft.Value = 0D;
            this.vuLeft.Vertical = false;
            // 
            // vuRight
            // 
            this.vuRight.AllowDragging = false;
            this.vuRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vuRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.vuRight.EffectiveMaximum = 0D;
            this.vuRight.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.vuRight.Location = new System.Drawing.Point(62, 466);
            this.vuRight.Margin = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.vuRight.Maximum = 10000D;
            this.vuRight.MaximumSize = new System.Drawing.Size(10000, 9);
            this.vuRight.MinimumSize = new System.Drawing.Size(10, 9);
            this.vuRight.Name = "vuRight";
            this.vuRight.NrTicks = 20;
            this.vuRight.OverrideBackColor = System.Drawing.Color.Empty;
            this.vuRight.OverrideElapsedBackColor = System.Drawing.Color.Empty;
            this.vuRight.ShowTicks = true;
            this.vuRight.Size = new System.Drawing.Size(284, 9);
            this.vuRight.TabIndex = 17;
            this.vuRight.Value = 0D;
            this.vuRight.Vertical = false;
            // 
            // PlaylistPanel
            // 
            this.Controls.Add(this.pnlLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlaylistPanel";
            this.Size = new System.Drawing.Size(695, 507);
            this.cmsPlaylist.ResumeLayout(false);
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OPMToolStripMenuItem dummyToolStripMenuItem;
        private OPMContextMenuStrip cmsPlaylist;
        private PlaylistItemInfo piNext;
        private PlaylistItemInfo piCurrent;
        private PlaylistItemInfo piPrev;
        private PlaylistItemInfo piTotal;
        private OPMTableLayoutPanel pnlLayout;
        private Label lblSep5;
        private Label lblSep3;
        private Label lblSep2;
        private Label label1;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
        private HeaderlessListView lvPlaylist;
        private ColumnHeader colDummy;
        private ColumnHeader colMisc;
        private ColumnHeader colIcon;
        private ColumnHeader colTime;
        private ColumnHeader colFile;
        private OPMLabel lblEmptyPlaylist;
        private OPMLinkLabel lblOpenPlaylist;
        private OPMLinkLabel lblOpenFiles;
        private VuMeterGauge vuLeft;
        private VuMeterGauge vuRight;
        private OPMLabel opmLabel1;
        private OPMLabel opmLabel2;
        private OPMLabel opmLabel3;
        private OPMLabel opmLabel4;
    }
}
