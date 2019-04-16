using System.Windows.Forms;
using OPMedia.UI.Themes;
using OPMedia.UI.Controls;
using OPMedia.Runtime.Shortcuts;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    partial class PlaybackControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaybackControlPanel));
            this.pnlButtons = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.opmToolStrip1 = new OPMedia.UI.Controls.OPMToolStrip();
            this.tsmPlaylistEnd = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmLoopPlay = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmPrev = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmPlayPause = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmStop = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmNext = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmToggleXFade = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmToggleShuffle = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.opmToolStripSeparator1 = new OPMedia.UI.Controls.OPMToolStripSeparator();
            this.tslTime = new System.Windows.Forms.ToolStripLabel();
            this.tsmLoad = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmOpenDisk = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmOpenURL = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsmOpenSettings = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.playbackPanel1 = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaybackPanel();
            this.opmToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoSize = true;
            this.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlButtons.Size = new System.Drawing.Size(0, 0);
            this.pnlButtons.TabIndex = 10;
            // 
            // opmToolStrip1
            // 
            this.opmToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.opmToolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.opmToolStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.opmToolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.opmToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.opmToolStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.opmToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmPlaylistEnd,
            this.tsmLoopPlay,
            this.tsmPrev,
            this.tsmPlayPause,
            this.tsmStop,
            this.tsmNext,
            this.tsmToggleXFade,
            this.tsmToggleShuffle,
            this.opmToolStripSeparator1,
            this.tslTime,
            this.tsmLoad,
            this.tsmOpenDisk,
            this.tsmOpenURL,
            this.tsmOpenSettings});
            this.opmToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.opmToolStrip1.MinimumSize = new System.Drawing.Size(100, 45);
            this.opmToolStrip1.Name = "opmToolStrip1";
            this.opmToolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.opmToolStrip1.ShowBorder = false;
            this.opmToolStrip1.Size = new System.Drawing.Size(559, 45);
            this.opmToolStrip1.TabIndex = 11;
            this.opmToolStrip1.Text = "opmToolStrip1";
            this.opmToolStrip1.VerticalGradient = true;
            // 
            // tsmPlaylistEnd
            // 
            this.tsmPlaylistEnd.ActiveImage = null;
            this.tsmPlaylistEnd.CheckedImage = null;
            this.tsmPlaylistEnd.DisabledImage = null;
            this.tsmPlaylistEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmPlaylistEnd.Image = ((System.Drawing.Image)(resources.GetObject("tsmPlaylistEnd.Image")));
            this.tsmPlaylistEnd.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmPlaylistEnd.InactiveImage = ((System.Drawing.Image)(resources.GetObject("tsmPlaylistEnd.InactiveImage")));
            this.tsmPlaylistEnd.Margin = new System.Windows.Forms.Padding(0);
            this.tsmPlaylistEnd.Name = "tsmPlaylistEnd";
            this.tsmPlaylistEnd.Size = new System.Drawing.Size(23, 45);
            this.tsmPlaylistEnd.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdPlaylistEnd;
            this.tsmPlaylistEnd.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmPlaylistEnd.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmPlaylistEnd.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmPlaylistEnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmLoopPlay
            // 
            this.tsmLoopPlay.ActiveImage = null;
            this.tsmLoopPlay.CheckedImage = null;
            this.tsmLoopPlay.DisabledImage = null;
            this.tsmLoopPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmLoopPlay.Image = ((System.Drawing.Image)(resources.GetObject("tsmLoopPlay.Image")));
            this.tsmLoopPlay.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmLoopPlay.InactiveImage = ((System.Drawing.Image)(resources.GetObject("tsmLoopPlay.InactiveImage")));
            this.tsmLoopPlay.Margin = new System.Windows.Forms.Padding(0);
            this.tsmLoopPlay.Name = "tsmLoopPlay";
            this.tsmLoopPlay.Size = new System.Drawing.Size(23, 45);
            this.tsmLoopPlay.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdLoopPlay;
            this.tsmLoopPlay.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmLoopPlay.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmLoopPlay.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmLoopPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmPrev
            // 
            this.tsmPrev.ActiveImage = null;
            this.tsmPrev.CheckedImage = null;
            this.tsmPrev.DisabledImage = null;
            this.tsmPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmPrev.Image = null;
            this.tsmPrev.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmPrev.InactiveImage = null;
            this.tsmPrev.Margin = new System.Windows.Forms.Padding(0);
            this.tsmPrev.Name = "tsmPrev";
            this.tsmPrev.Size = new System.Drawing.Size(23, 45);
            this.tsmPrev.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdPrev;
            this.tsmPrev.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmPrev.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmPrev.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmPlayPause
            // 
            this.tsmPlayPause.ActiveImage = null;
            this.tsmPlayPause.CheckedImage = null;
            this.tsmPlayPause.DisabledImage = null;
            this.tsmPlayPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmPlayPause.Image = global::OPMedia.UI.ProTONE.Properties.Resources.btnPlay;
            this.tsmPlayPause.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsmPlayPause.InactiveImage = global::OPMedia.UI.ProTONE.Properties.Resources.btnPlay;
            this.tsmPlayPause.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.tsmPlayPause.Name = "tsmPlayPause";
            this.tsmPlayPause.Size = new System.Drawing.Size(23, 42);
            this.tsmPlayPause.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdPlayPause;
            this.tsmPlayPause.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmPlayPause.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmPlayPause.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmPlayPause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmStop
            // 
            this.tsmStop.ActiveImage = null;
            this.tsmStop.CheckedImage = null;
            this.tsmStop.DisabledImage = null;
            this.tsmStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmStop.Image = null;
            this.tsmStop.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmStop.InactiveImage = null;
            this.tsmStop.Margin = new System.Windows.Forms.Padding(0);
            this.tsmStop.Name = "tsmStop";
            this.tsmStop.Size = new System.Drawing.Size(23, 45);
            this.tsmStop.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdStop;
            this.tsmStop.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmStop.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmStop.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmStop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmNext
            // 
            this.tsmNext.ActiveImage = null;
            this.tsmNext.CheckedImage = null;
            this.tsmNext.DisabledImage = null;
            this.tsmNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmNext.Image = null;
            this.tsmNext.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmNext.InactiveImage = null;
            this.tsmNext.Margin = new System.Windows.Forms.Padding(0);
            this.tsmNext.Name = "tsmNext";
            this.tsmNext.Size = new System.Drawing.Size(23, 45);
            this.tsmNext.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdNext;
            this.tsmNext.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmNext.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmNext.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmToggleXFade
            // 
            this.tsmToggleXFade.ActiveImage = null;
            this.tsmToggleXFade.CheckedImage = null;
            this.tsmToggleXFade.DisabledImage = null;
            this.tsmToggleXFade.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmToggleXFade.Image = global::OPMedia.UI.ProTONE.Properties.Resources.XFade;
            this.tsmToggleXFade.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmToggleXFade.InactiveImage = global::OPMedia.UI.ProTONE.Properties.Resources.XFade;
            this.tsmToggleXFade.Name = "tsmToggleXFade";
            this.tsmToggleXFade.Size = new System.Drawing.Size(23, 42);
            this.tsmToggleXFade.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdXFade;
            this.tsmToggleXFade.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmToggleXFade.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmToggleXFade.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmToggleXFade.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmToggleShuffle
            // 
            this.tsmToggleShuffle.ActiveImage = null;
            this.tsmToggleShuffle.CheckedImage = null;
            this.tsmToggleShuffle.DisabledImage = null;
            this.tsmToggleShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmToggleShuffle.Image = ((System.Drawing.Image)(resources.GetObject("tsmToggleShuffle.Image")));
            this.tsmToggleShuffle.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmToggleShuffle.InactiveImage = ((System.Drawing.Image)(resources.GetObject("tsmToggleShuffle.InactiveImage")));
            this.tsmToggleShuffle.Margin = new System.Windows.Forms.Padding(0);
            this.tsmToggleShuffle.Name = "tsmToggleShuffle";
            this.tsmToggleShuffle.Size = new System.Drawing.Size(23, 45);
            this.tsmToggleShuffle.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdToggleShuffle;
            this.tsmToggleShuffle.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmToggleShuffle.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmToggleShuffle.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmToggleShuffle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // opmToolStripSeparator1
            // 
            this.opmToolStripSeparator1.Name = "opmToolStripSeparator1";
            this.opmToolStripSeparator1.Size = new System.Drawing.Size(6, 45);
            // 
            // tslTime
            // 
            this.tslTime.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslTime.Margin = new System.Windows.Forms.Padding(0);
            this.tslTime.Name = "tslTime";
            this.tslTime.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tslTime.Size = new System.Drawing.Size(218, 45);
            this.tslTime.Text = "00:00:00 / 00:00/00";
            // 
            // tsmLoad
            // 
            this.tsmLoad.ActiveImage = null;
            this.tsmLoad.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmLoad.CheckedImage = null;
            this.tsmLoad.DisabledImage = null;
            this.tsmLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmLoad.Image = null;
            this.tsmLoad.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmLoad.InactiveImage = null;
            this.tsmLoad.Margin = new System.Windows.Forms.Padding(0);
            this.tsmLoad.Name = "tsmLoad";
            this.tsmLoad.Size = new System.Drawing.Size(23, 45);
            this.tsmLoad.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdLoad;
            this.tsmLoad.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmLoad.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmLoad.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmLoad.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmOpenDisk
            // 
            this.tsmOpenDisk.ActiveImage = null;
            this.tsmOpenDisk.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmOpenDisk.CheckedImage = null;
            this.tsmOpenDisk.DisabledImage = null;
            this.tsmOpenDisk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmOpenDisk.Image = null;
            this.tsmOpenDisk.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmOpenDisk.InactiveImage = null;
            this.tsmOpenDisk.Margin = new System.Windows.Forms.Padding(0);
            this.tsmOpenDisk.Name = "tsmOpenDisk";
            this.tsmOpenDisk.Size = new System.Drawing.Size(23, 45);
            this.tsmOpenDisk.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdOpenDisk;
            this.tsmOpenDisk.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmOpenDisk.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmOpenDisk.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmOpenDisk.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmOpenURL
            // 
            this.tsmOpenURL.ActiveImage = null;
            this.tsmOpenURL.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmOpenURL.CheckedImage = null;
            this.tsmOpenURL.DisabledImage = null;
            this.tsmOpenURL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmOpenURL.Image = null;
            this.tsmOpenURL.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmOpenURL.InactiveImage = null;
            this.tsmOpenURL.Margin = new System.Windows.Forms.Padding(0);
            this.tsmOpenURL.Name = "tsmOpenURL";
            this.tsmOpenURL.Size = new System.Drawing.Size(23, 45);
            this.tsmOpenURL.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdOpenURL;
            this.tsmOpenURL.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmOpenURL.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmOpenURL.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmOpenURL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // tsmOpenSettings
            // 
            this.tsmOpenSettings.ActiveImage = null;
            this.tsmOpenSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmOpenSettings.CheckedImage = null;
            this.tsmOpenSettings.DisabledImage = null;
            this.tsmOpenSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmOpenSettings.Image = null;
            this.tsmOpenSettings.ImageTransparentColor = System.Drawing.Color.White;
            this.tsmOpenSettings.InactiveImage = null;
            this.tsmOpenSettings.Margin = new System.Windows.Forms.Padding(0, 1, 5, 2);
            this.tsmOpenSettings.Name = "tsmOpenSettings";
            this.tsmOpenSettings.Size = new System.Drawing.Size(23, 42);
            this.tsmOpenSettings.Tag = OPMedia.Runtime.Shortcuts.OPMShortcut.CmdOpenSettings;
            this.tsmOpenSettings.Click += new System.EventHandler(this.OnButtonPressed);
            this.tsmOpenSettings.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.tsmOpenSettings.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.tsmOpenSettings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // playbackPanel1
            // 
            this.playbackPanel1.AutoSize = true;
            this.playbackPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.playbackPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.playbackPanel1.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.playbackPanel1.Location = new System.Drawing.Point(0, 41);
            this.playbackPanel1.MaximumSize = new System.Drawing.Size(3500, 75);
            this.playbackPanel1.MinimumSize = new System.Drawing.Size(500, 75);
            this.playbackPanel1.Name = "playbackPanel1";
            this.playbackPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.playbackPanel1.Size = new System.Drawing.Size(559, 75);
            this.playbackPanel1.TabIndex = 12;
            // 
            // PlaybackControlPanel
            // 
            this.Controls.Add(this.playbackPanel1);
            this.Controls.Add(this.opmToolStrip1);
            this.Controls.Add(this.pnlButtons);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlaybackControlPanel";
            this.Size = new System.Drawing.Size(559, 116);
            this.opmToolStrip1.ResumeLayout(false);
            this.opmToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OPMFlowLayoutPanel pnlButtons;
        private OPMToolStrip opmToolStrip1;
        private OPMTriStateToolStripButton tsmPlayPause;
        private OPMTriStateToolStripButton tsmStop;
        private OPMToolStripSeparator opmToolStripSeparator1;
        private OPMTriStateToolStripButton tsmPrev;
        private OPMTriStateToolStripButton tsmNext;
        private OPMTriStateToolStripButton tsmLoad;
        private OPMTriStateToolStripButton tsmOpenURL;
        private OPMTriStateToolStripButton tsmLoopPlay;
        private OPMTriStateToolStripButton tsmPlaylistEnd;
        private OPMTriStateToolStripButton tsmToggleShuffle;
        private OPMTriStateToolStripButton tsmOpenSettings;
        private OPMTriStateToolStripButton tsmOpenDisk;
        private ToolStripLabel tslTime;
        private OPMTriStateToolStripButton tsmToggleXFade;
        private PlaybackPanel playbackPanel1;
    }
}
