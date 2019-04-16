using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Controls;
using OPMedia.Core.Logging;
using System.Resources;

using OPMedia.UI.Themes;
using OPMedia.Core;

using OPMedia.Runtime.Shortcuts;

using OPMedia.UI.Generic;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE;

using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using System.Threading.Tasks;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class PlaybackControlPanel : OPMBaseControl
    {
        OPMToolTipManager _tip = null;
        ToolStripItem _hoveredItem = null;

        private FilterState _filterState = FilterState.Stopped;

        private bool _compactView = false;
        public bool CompactView
        {
            get
            {
                return _compactView;
            }

            set
            {
                _compactView = value;
                UpdateButtonSize();
            }
        }

        private void UpdateButtonSize()
        {
            foreach (ToolStripItem tsi in opmToolStrip1.Items)
            {
                if (tsi is ToolStripSeparator)
                    tsi.Visible = !_compactView;
                else if (tsi is ToolStripButton)
                {
                    ToolStripButton btn = tsi as ToolStripButton;
                    if (btn != null)
                    {
                        OPMShortcut cmd = (OPMShortcut)btn.Tag;
                        btn.Visible = (!_compactView || cmd <= OPMShortcut.CmdStop);

                        if (btn.Image != null)
                        {
                            int h = Math.Min(40, btn.Image.Height);
                            btn.Size = new Size(h, h);
                            btn.ImageAlign = ContentAlignment.MiddleCenter;
                        }

                        
                        btn.AutoSize = false;
                    }
                }
            }
        }

        #region Properties

        public FilterState FilterState
        { get { return _filterState; } set { _filterState = value; UpdatePlayPauseButton(); } }

        double _elapsedSeconds = 0, _totalSeconds = 0;

        public double ElapsedSeconds
        {
            get
            {
                return _elapsedSeconds;
            }

            set
            {
                _elapsedSeconds = value;
                UpdateDisplayedTime();
            }
        }

        private void UpdateDisplayedTime()
        {
            string sElapsed = "", sTotal = "";

            if (_elapsedSeconds > 0)
                sElapsed = string.Format("{0}", TimeSpan.FromSeconds((int)_elapsedSeconds));
            else
                sElapsed = "00:00:00";

            if (_totalSeconds >= 0)
            {
                if (_totalSeconds > 0)
                    sTotal = string.Format(" / {0}", TimeSpan.FromSeconds((int)_totalSeconds));
                else
                    sTotal = " / 00:00:00";
            }

            tslTime.Text = sElapsed + sTotal;
            tslTime.ForeColor = ThemeManager.WndTextColor;
        }

        public double TotalSeconds
        {
            get
            {
                return _totalSeconds;
            }

            set
            {
                _totalSeconds = value;
                UpdateDisplayedTime();
            }
        }
        #endregion

        public PlaybackControlPanel()
            : base()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            
            _tip = new OPMToolTipManager(opmToolStrip1);
            
            this.HandleCreated += new EventHandler(PlaybackControlPanel_HandleCreated);
        }

        void PlaybackControlPanel_HandleCreated(object sender, EventArgs e)
        {
            UpdateStateButtons();
        }

        [EventSink(EventNames.ThemeUpdated)]
        [EventSink(LocalEventNames.UpdateStateButtons)]
        public void UpdateStateButtons()
        {
            tsmToggleXFade.Checked = ProTONEConfig.XFade;
            tsmLoopPlay.Checked = ProTONEConfig.LoopPlay;
            tsmPlaylistEnd.Checked = SystemScheduler.PlaylistEventEnabled;

            UpdatePlayPauseButton();

            tsmStop.InactiveImage = Resources.btnStop;
            tsmNext.InactiveImage = Resources.btnNext;
            tsmPrev.InactiveImage = Resources.btnPrev;
            
            if (VideoDVDHelpers.IsOSSupported)
                tsmOpenDisk.InactiveImage = ImageProcessing.DVD;
            else
            {
                tsmOpenDisk.Visible = false;
                if (opmToolStrip1.Items.Contains(tsmOpenDisk))
                    opmToolStrip1.Items.Remove(tsmOpenDisk);
            }

            tsmOpenURL.InactiveImage = OPMedia.Core.Properties.Resources.Internet;
            tsmLoad.InactiveImage = Resources.btnLoad;
            tsmOpenSettings.InactiveImage = OPMedia.UI.Properties.Resources.Settings;
            tsmLoopPlay.InactiveImage = Resources.btnLoopPlay;
            tsmToggleShuffle.InactiveImage = Resources.btnToggleShuffle;
            tsmPlaylistEnd.InactiveImage = Resources.btnPlaylistEnd;
            tsmToggleXFade.InactiveImage = Resources.XFade;

            tslTime.Font = ThemeManager.ExtremeLargeFont;
            tslTime.ForeColor = ThemeManager.WndTextColor;

            UpdateButtonSize();
        }

        private void OnButtonPressed(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn != null)
            {
                try
                {
                    OPMShortcut cmd = (OPMShortcut)btn.Tag;
                    ShortcutMapper.DispatchCommand(cmd);
                }
                catch (Exception ex)
                {
                    ErrorDispatcher.DispatchError(ex, false);
                }
                finally
                {
                    UpdateStateButtons();
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            pnlButtons.Focus();
        }

        private void OnMouseHover(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn != null)
            {
                _hoveredItem = btn;

                OPMShortcut cmd = (OPMShortcut)btn.Tag;
                string resourceTag = string.Format("TXT_{0}", cmd.ToString().ToUpperInvariant()).Replace("CMD", "BTN");
                string tipText = Translator.Translate(resourceTag, ShortcutMapper.GetShortcutString(cmd));
                _tip.ShowSimpleToolTip(tipText, btn.Image);
            }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            //if (sender == _hoveredItem)
            //{
            //    _hoveredItem = null;
            //    _tip.RemoveAll();
            //}
        }

        private void UpdatePlayPauseButton()
        {
            Image img = null;

            switch(_filterState)
            {
                case Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running:
                    img = Resources.btnPause;
                    break;

                default:
                    img = Resources.btnPlay;
                    break;
            }


            tsmPlayPause.InactiveImage = img;
        }
   }
}
