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
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblSep5 = new System.Windows.Forms.Label();
            this.lblSep4 = new System.Windows.Forms.Label();
            this.lblSep3 = new System.Windows.Forms.Label();
            this.lblSep2 = new System.Windows.Forms.Label();
            this.lvPlaylist = new OPMedia.UI.Controls.HeaderlessListView();
            this.colDummy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMisc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSep1 = new System.Windows.Forms.Label();
            this.pnlAdditional = new OPMedia.UI.Controls.OPMPanel();
            this.piTotal = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piPrev = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piCurrent = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piNext = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.cmsPlaylist.SuspendLayout();
            this.pnlLayout.SuspendLayout();
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
            // pnlLayout
            // 
            this.pnlLayout.AutoSize = true;
            this.pnlLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.ColumnCount = 2;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.Controls.Add(this.lblSep5, 0, 9);
            this.pnlLayout.Controls.Add(this.lblSep4, 0, 7);
            this.pnlLayout.Controls.Add(this.lblSep3, 0, 5);
            this.pnlLayout.Controls.Add(this.lblSep2, 0, 3);
            this.pnlLayout.Controls.Add(this.lvPlaylist, 0, 0);
            this.pnlLayout.Controls.Add(this.piTotal, 0, 2);
            this.pnlLayout.Controls.Add(this.piPrev, 0, 8);
            this.pnlLayout.Controls.Add(this.piCurrent, 0, 4);
            this.pnlLayout.Controls.Add(this.piNext, 0, 6);
            this.pnlLayout.Controls.Add(this.lblSep1, 0, 1);
            this.pnlLayout.Controls.Add(this.pnlAdditional, 1, 0);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlLayout.RowCount = 10;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.pnlLayout.Size = new System.Drawing.Size(533, 626);
            this.pnlLayout.TabIndex = 4;
            // 
            // lblSep5
            // 
            this.lblSep5.AutoSize = true;
            this.lblSep5.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.lblSep5, 2);
            this.lblSep5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep5.Location = new System.Drawing.Point(3, 624);
            this.lblSep5.Name = "lblSep5";
            this.lblSep5.Size = new System.Drawing.Size(527, 2);
            this.lblSep5.TabIndex = 13;
            // 
            // lblSep4
            // 
            this.lblSep4.AutoSize = true;
            this.lblSep4.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.lblSep4, 2);
            this.lblSep4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep4.Location = new System.Drawing.Point(3, 598);
            this.lblSep4.Name = "lblSep4";
            this.lblSep4.Size = new System.Drawing.Size(527, 2);
            this.lblSep4.TabIndex = 12;
            // 
            // lblSep3
            // 
            this.lblSep3.AutoSize = true;
            this.lblSep3.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.lblSep3, 2);
            this.lblSep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep3.Location = new System.Drawing.Point(3, 572);
            this.lblSep3.Name = "lblSep3";
            this.lblSep3.Size = new System.Drawing.Size(527, 2);
            this.lblSep3.TabIndex = 11;
            // 
            // lblSep2
            // 
            this.lblSep2.AutoSize = true;
            this.lblSep2.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.lblSep2, 2);
            this.lblSep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep2.Location = new System.Drawing.Point(3, 546);
            this.lblSep2.Name = "lblSep2";
            this.lblSep2.Size = new System.Drawing.Size(527, 2);
            this.lblSep2.TabIndex = 10;
            // 
            // lvPlaylist
            // 
            this.lvPlaylist.AllowEditing = true;
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
            this.lvPlaylist.Location = new System.Drawing.Point(3, 3);
            this.lvPlaylist.MultiSelect = false;
            this.lvPlaylist.Name = "lvPlaylist";
            this.lvPlaylist.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvPlaylist.Size = new System.Drawing.Size(526, 515);
            this.lvPlaylist.TabIndex = 3;
            this.lvPlaylist.UseCompatibleStateImageBehavior = false;
            this.lvPlaylist.View = System.Windows.Forms.View.Details;
            this.lvPlaylist.SelectedIndexChanged += new System.EventHandler(this.lvPlaylist_SelectedIndexChanged);
            this.lvPlaylist.DragLeave += new System.EventHandler(this.lvPlaylist_DragLeave);
            this.lvPlaylist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvPlaylist_MouseClick);
            this.lvPlaylist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPlaylist_MouseDoubleClick);
            this.lvPlaylist.Resize += new System.EventHandler(this.lvPlaylist_Resize);
            // 
            // lblSep1
            // 
            this.lblSep1.AutoSize = true;
            this.lblSep1.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.lblSep1, 2);
            this.lblSep1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSep1.Location = new System.Drawing.Point(3, 521);
            this.lblSep1.Name = "lblSep1";
            this.lblSep1.Size = new System.Drawing.Size(527, 2);
            this.lblSep1.TabIndex = 9;
            // 
            // pnlAdditional
            // 
            this.pnlAdditional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAdditional.Location = new System.Drawing.Point(532, 0);
            this.pnlAdditional.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAdditional.Name = "pnlAdditional";
            this.pnlAdditional.Size = new System.Drawing.Size(1, 521);
            this.pnlAdditional.TabIndex = 14;
            // 
            // piTotal
            // 
            this.piTotal.AltDisplay = "";
            this.piTotal.AutoSize = true;
            this.piTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.SetColumnSpan(this.piTotal, 2);
            this.piTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piTotal.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piTotal.Item = null;
            this.piTotal.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.None;
            this.piTotal.Location = new System.Drawing.Point(0, 524);
            this.piTotal.Margin = new System.Windows.Forms.Padding(0, 1, 0, 2);
            this.piTotal.Name = "piTotal";
            this.piTotal.OverrideBackColor = System.Drawing.Color.Empty;
            this.piTotal.Size = new System.Drawing.Size(533, 20);
            this.piTotal.TabIndex = 4;
            // 
            // piPrev
            // 
            this.piPrev.AltDisplay = "";
            this.piPrev.AutoSize = true;
            this.piPrev.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.SetColumnSpan(this.piPrev, 2);
            this.piPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piPrev.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piPrev.Item = null;
            this.piPrev.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.Previous;
            this.piPrev.Location = new System.Drawing.Point(0, 602);
            this.piPrev.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.piPrev.Name = "piPrev";
            this.piPrev.OverrideBackColor = System.Drawing.Color.Empty;
            this.piPrev.Size = new System.Drawing.Size(533, 20);
            this.piPrev.TabIndex = 6;
            // 
            // piCurrent
            // 
            this.piCurrent.AltDisplay = "";
            this.piCurrent.AutoSize = true;
            this.piCurrent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.SetColumnSpan(this.piCurrent, 2);
            this.piCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piCurrent.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piCurrent.Item = null;
            this.piCurrent.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.Current;
            this.piCurrent.Location = new System.Drawing.Point(0, 550);
            this.piCurrent.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.piCurrent.Name = "piCurrent";
            this.piCurrent.OverrideBackColor = System.Drawing.Color.Empty;
            this.piCurrent.Size = new System.Drawing.Size(533, 20);
            this.piCurrent.TabIndex = 7;
            // 
            // piNext
            // 
            this.piNext.AltDisplay = "";
            this.piNext.AutoSize = true;
            this.piNext.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.SetColumnSpan(this.piNext, 2);
            this.piNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piNext.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.piNext.Item = null;
            this.piNext.ItemType = OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemType.Next;
            this.piNext.Location = new System.Drawing.Point(0, 576);
            this.piNext.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.piNext.Name = "piNext";
            this.piNext.OverrideBackColor = System.Drawing.Color.Empty;
            this.piNext.Size = new System.Drawing.Size(533, 20);
            this.piNext.TabIndex = 8;
            // 
            // PlaylistPanel
            // 
            this.Controls.Add(this.pnlLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlaylistPanel";
            this.Size = new System.Drawing.Size(533, 626);
            this.cmsPlaylist.ResumeLayout(false);
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OPMToolStripMenuItem dummyToolStripMenuItem;
        private OPMContextMenuStrip cmsPlaylist;
        private OPMTableLayoutPanel pnlLayout;
        private PlaylistItemInfo piTotal;
        private PlaylistItemInfo piPrev;
        private PlaylistItemInfo piCurrent;
        private PlaylistItemInfo piNext;
        private Label lblSep5;
        private Label lblSep4;
        private Label lblSep3;
        private Label lblSep2;
        private HeaderlessListView lvPlaylist;
        private ColumnHeader colDummy;
        private ColumnHeader colMisc;
        private ColumnHeader colIcon;
        private ColumnHeader colTime;
        private ColumnHeader colFile;
        private Label lblSep1;
        private OPMPanel pnlAdditional;
    }
}
