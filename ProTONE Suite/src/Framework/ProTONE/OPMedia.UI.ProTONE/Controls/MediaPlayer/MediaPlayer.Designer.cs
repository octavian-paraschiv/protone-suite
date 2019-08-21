using OPMedia.UI.Controls;
using OPMedia.UI.ProTONE.Controls.MediaPlayer;
using OPMedia.UI.ProTONE.Controls;
using System.Windows.Forms;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Themes;
namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    partial class MediaPlayer
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
            this.layoutPanel = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.playlist = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistPanel();
            this.pnlPlayback = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaybackPanel();
            this.cmsOpenFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.layoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 1;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.playlist, 0, 0);
            this.layoutPanel.Controls.Add(this.pnlPlayback, 0, 1);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 2;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.Size = new System.Drawing.Size(573, 385);
            this.layoutPanel.TabIndex = 0;
            // 
            // playlist
            // 
            this.playlist.AllowDrop = true;
            this.playlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlist.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.playlist.Location = new System.Drawing.Point(0, 0);
            this.playlist.Margin = new System.Windows.Forms.Padding(0);
            this.playlist.MinimumSize = new System.Drawing.Size(160, 160);
            this.playlist.Name = "playlist";
            this.playlist.OverrideBackColor = System.Drawing.Color.Empty;
            this.playlist.Size = new System.Drawing.Size(573, 304);
            this.playlist.TabIndex = 0;
            this.playlist.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlPlaylist_DragDrop);
            this.playlist.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlPlaylist_DragEnter);
            this.playlist.DragOver += new System.Windows.Forms.DragEventHandler(this.pnlPlaylist_DragOver);
            this.playlist.DragLeave += new System.EventHandler(this.pnlPlaylist_DragLeave);
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.AutoSize = true;
            this.pnlPlayback.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPlayback.CompactView = false;
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlayback.EffectiveSeconds = 0D;
            this.pnlPlayback.ElapsedSeconds = 0D;
            this.pnlPlayback.FilterState = OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped;
            this.pnlPlayback.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.pnlPlayback.Location = new System.Drawing.Point(3, 307);
            this.pnlPlayback.MaximumSize = new System.Drawing.Size(3500, 75);
            this.pnlPlayback.MinimumSize = new System.Drawing.Size(500, 75);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.OverrideBackColor = System.Drawing.Color.Transparent;
            this.pnlPlayback.ProjectedVolume = 5000;
            this.pnlPlayback.Size = new System.Drawing.Size(567, 75);
            this.pnlPlayback.TabIndex = 1;
            this.pnlPlayback.TimeScaleEnabled = true;
            this.pnlPlayback.TotalSeconds = 0D;
            this.pnlPlayback.VolumeScaleEnabled = true;
            // 
            // cmsOpenFiles
            // 
            this.cmsOpenFiles.Name = "cmsOpenFiles";
            this.cmsOpenFiles.Size = new System.Drawing.Size(61, 4);
            // 
            // MediaPlayer
            // 
            this.Controls.Add(this.layoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(365, 255);
            this.Name = "MediaPlayer";
            this.Size = new System.Drawing.Size(573, 385);
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private OPMTableLayoutPanel layoutPanel;
        private PlaylistPanel playlist;
        private ContextMenuStrip cmsOpenFiles;
        private PlaybackPanel pnlPlayback;
    }
}
