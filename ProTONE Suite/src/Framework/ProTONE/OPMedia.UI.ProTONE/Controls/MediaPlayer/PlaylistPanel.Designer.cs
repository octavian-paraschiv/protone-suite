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
            this.lvPlaylist = new OPMedia.UI.Controls.HeaderlessListView();
            this.colDummy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMisc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.piTotal = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piPrev = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piCurrent = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.piNext = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistItemInfo();
            this.label1 = new System.Windows.Forms.Label();
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
            this.pnlLayout.SetColumnSpan(this.lvPlaylist, 4);
            this.lvPlaylist.ContextMenuStrip = this.cmsPlaylist;
            this.lvPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPlaylist.Location = new System.Drawing.Point(3, 3);
            this.lvPlaylist.MultiSelect = false;
            this.lvPlaylist.Name = "lvPlaylist";
            this.lvPlaylist.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvPlaylist.Size = new System.Drawing.Size(527, 477);
            this.lvPlaylist.TabIndex = 3;
            this.lvPlaylist.UseCompatibleStateImageBehavior = false;
            this.lvPlaylist.View = System.Windows.Forms.View.Details;
            this.lvPlaylist.SelectedIndexChanged += new System.EventHandler(this.lvPlaylist_SelectedIndexChanged);
            this.lvPlaylist.DragLeave += new System.EventHandler(this.lvPlaylist_DragLeave);
            this.lvPlaylist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvPlaylist_MouseClick);
            this.lvPlaylist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPlaylist_MouseDoubleClick);
            this.lvPlaylist.Resize += new System.EventHandler(this.lvPlaylist_Resize);
            // 
            // pnlLayout
            // 
            this.pnlLayout.AutoSize = true;
            this.pnlLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.ColumnCount = 4;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Controls.Add(this.label5, 0, 9);
            this.pnlLayout.Controls.Add(this.label4, 0, 7);
            this.pnlLayout.Controls.Add(this.label3, 0, 5);
            this.pnlLayout.Controls.Add(this.label2, 0, 3);
            this.pnlLayout.Controls.Add(this.lvPlaylist, 0, 0);
            this.pnlLayout.Controls.Add(this.piTotal, 1, 2);
            this.pnlLayout.Controls.Add(this.piPrev, 1, 8);
            this.pnlLayout.Controls.Add(this.piCurrent, 1, 4);
            this.pnlLayout.Controls.Add(this.piNext, 1, 6);
            this.pnlLayout.Controls.Add(this.label1, 0, 1);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlLayout.RowCount = 10;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlLayout.Size = new System.Drawing.Size(533, 585);
            this.pnlLayout.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.label5, 4);
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 575);
            this.label5.MaximumSize = new System.Drawing.Size(3500, 3);
            this.label5.MinimumSize = new System.Drawing.Size(35, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(527, 3);
            this.label5.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.label4, 4);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 552);
            this.label4.MaximumSize = new System.Drawing.Size(3500, 3);
            this.label4.MinimumSize = new System.Drawing.Size(35, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(527, 3);
            this.label4.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.label3, 4);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 529);
            this.label3.MaximumSize = new System.Drawing.Size(3500, 3);
            this.label3.MinimumSize = new System.Drawing.Size(35, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(527, 3);
            this.label3.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.label2, 4);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 506);
            this.label2.MaximumSize = new System.Drawing.Size(3500, 3);
            this.label2.MinimumSize = new System.Drawing.Size(35, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(527, 3);
            this.label2.TabIndex = 10;
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
            this.piTotal.Location = new System.Drawing.Point(20, 486);
            this.piTotal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.piTotal.Name = "piTotal";
            this.piTotal.OverrideBackColor = System.Drawing.Color.Empty;
            this.piTotal.Size = new System.Drawing.Size(492, 15);
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
            this.piPrev.Location = new System.Drawing.Point(20, 555);
            this.piPrev.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.piPrev.Name = "piPrev";
            this.piPrev.OverrideBackColor = System.Drawing.Color.Empty;
            this.piPrev.Size = new System.Drawing.Size(492, 15);
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
            this.piCurrent.Location = new System.Drawing.Point(20, 509);
            this.piCurrent.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.piCurrent.Name = "piCurrent";
            this.piCurrent.OverrideBackColor = System.Drawing.Color.Empty;
            this.piCurrent.Size = new System.Drawing.Size(492, 15);
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
            this.piNext.Location = new System.Drawing.Point(20, 532);
            this.piNext.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.piNext.Name = "piNext";
            this.piNext.OverrideBackColor = System.Drawing.Color.Empty;
            this.piNext.Size = new System.Drawing.Size(492, 15);
            this.piNext.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.pnlLayout.SetColumnSpan(this.label1, 4);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 483);
            this.label1.MaximumSize = new System.Drawing.Size(3500, 3);
            this.label1.MinimumSize = new System.Drawing.Size(35, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(527, 3);
            this.label1.TabIndex = 9;
            // 
            // PlaylistScreen
            // 
            this.Controls.Add(this.pnlLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlaylistScreen";
            this.Size = new System.Drawing.Size(533, 585);
            this.cmsPlaylist.ResumeLayout(false);
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OPMToolStripMenuItem dummyToolStripMenuItem;
        private HeaderlessListView lvPlaylist;
        private ColumnHeader colDummy;
        private ColumnHeader colIcon;
        private ColumnHeader colMisc;
        private ColumnHeader colTime;
        private ColumnHeader colFile;
        private OPMContextMenuStrip cmsPlaylist;
        private OPMTableLayoutPanel pnlLayout;
        private PlaylistItemInfo piTotal;
        private PlaylistItemInfo piPrev;
        private PlaylistItemInfo piCurrent;
        private PlaylistItemInfo piNext;
        private Label label1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
    }
}
