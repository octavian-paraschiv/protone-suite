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
            this.lblSep2 = new OPMedia.UI.Controls.OPMLabel();
            this.lblSep3 = new OPMedia.UI.Controls.OPMLabel();
            this.lblSep5 = new OPMedia.UI.Controls.OPMLabel();
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblSep1 = new OPMedia.UI.Controls.OPMLabel();
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
            this.piTotal = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piCurrent = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piPrev = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piNext = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
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
            this.lblSep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSep2.Location = new System.Drawing.Point(398, 550);
            this.lblSep2.Name = "lblSep2";
            this.lblSep2.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSep2.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblSep2.Size = new System.Drawing.Size(274, 2);
            this.lblSep2.TabIndex = 10;
            this.lblSep2.Visible = false;
            // 
            // lblSep3
            // 
            this.lblSep3.AutoSize = true;
            this.lblSep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSep3.Location = new System.Drawing.Point(398, 572);
            this.lblSep3.Name = "lblSep3";
            this.lblSep3.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSep3.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblSep3.Size = new System.Drawing.Size(274, 2);
            this.lblSep3.TabIndex = 11;
            // 
            // lblSep5
            // 
            this.lblSep5.AutoSize = true;
            this.pnlLayout.SetColumnSpan(this.lblSep5, 6);
            this.lblSep5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSep5.Location = new System.Drawing.Point(3, 596);
            this.lblSep5.Name = "lblSep5";
            this.lblSep5.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSep5.OverrideForeColor = System.Drawing.Color.Empty;
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
            this.pnlLayout.Controls.Add(this.lblSep1, 3, 0);
            this.pnlLayout.Controls.Add(this.opmTableLayoutPanel1, 0, 0);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayout.Name = "pnlLayout";
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
            this.pnlLayout.Size = new System.Drawing.Size(695, 618);
            this.pnlLayout.TabIndex = 4;
            // 
            // lblSep1
            // 
            this.lblSep1.AutoSize = true;
            this.lblSep1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSep1.Location = new System.Drawing.Point(388, 5);
            this.lblSep1.Margin = new System.Windows.Forms.Padding(5);
            this.lblSep1.Name = "lblSep1";
            this.lblSep1.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblSep1.OverrideForeColor = System.Drawing.Color.Empty;
            this.pnlLayout.SetRowSpan(this.lblSep1, 5);
            this.lblSep1.Size = new System.Drawing.Size(2, 584);
            this.lblSep1.TabIndex = 14;
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
            this.opmTableLayoutPanel1.RowCount = 4;
            this.pnlLayout.SetRowSpan(this.opmTableLayoutPanel1, 5);
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(383, 594);
            this.opmTableLayoutPanel1.TabIndex = 15;
            // 
            // lvPlaylist
            // 
            this.lvPlaylist.AllowEditing = false;
            this.lvPlaylist.AlternateRowColors = true;
            this.lvPlaylist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.lvPlaylist.Size = new System.Drawing.Size(373, 505);
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
            this.lblOpenPlaylist.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
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
            this.lblOpenFiles.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
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
            this.piTotal.Location = new System.Drawing.Point(0, 599);
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
            this.piCurrent.Size = new System.Drawing.Size(280, 543);
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
            this.piPrev.Location = new System.Drawing.Point(395, 574);
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
            this.piNext.Location = new System.Drawing.Point(395, 552);
            this.piNext.Margin = new System.Windows.Forms.Padding(0);
            this.piNext.Name = "piNext";
            this.piNext.OverrideBackColor = System.Drawing.Color.Empty;
            this.piNext.Size = new System.Drawing.Size(280, 20);
            this.piNext.TabIndex = 8;
            // 
            // PlaylistPanel
            // 
            this.Controls.Add(this.pnlLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlaylistPanel";
            this.Size = new System.Drawing.Size(695, 618);
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
        private PlaylistItemInfo piCurrent;
        private PlaylistItemInfo piPrev;
        private PlaylistItemInfo piTotal;
        private OPMTableLayoutPanel pnlLayout;
        private OPMLabel lblSep5;
        private OPMLabel lblSep3;
        private OPMLabel lblSep2;
        private OPMLabel lblSep1;
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
        private PlaylistItemInfo piNext;
    }
}
