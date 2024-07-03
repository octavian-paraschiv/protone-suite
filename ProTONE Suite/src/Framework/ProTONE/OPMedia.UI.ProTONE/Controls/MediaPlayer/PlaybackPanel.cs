using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Controls;
using OPMedia.UI.Generic;
using OPMedia.UI.ProTONE.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EventNames = OPMedia.Core.EventNames;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class PlaybackPanel : OPMBaseControl
    {
        public event ValueChangedEventHandler PositionChanged = null;
        public event ValueChangedEventHandler VolumeChanged = null;


        private FilterState _filterState = FilterState.Stopped;

        OPMToolTipManager _tip = null;

        Dictionary<Control, OPMToolTip> _tips = new Dictionary<Control, OPMToolTip>();

        #region Properties

        // TODO: implement
        public bool CompactView { get; set; }

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
                timeScale.ElapsedSeconds = value;
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

            lblTime.Text = sElapsed + sTotal;
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
                timeScale.TotalSeconds = value;
                UpdateDisplayedTime();
            }
        }

        public bool VolumeScaleEnabled
        {
            get
            {
                return volumeScale.Enabled;
            }

            set
            {
                volumeScale.Enabled = value;
            }
        }

        public bool TimeScaleEnabled
        {
            get
            {
                return timeScale.Enabled;
            }

            set
            {
                timeScale.Enabled = value;
            }
        }

        public int ProjectedVolume
        {
            get
            {
                return volumeScale.Position;
            }

            set
            {
                volumeScale.Position = value;
            }
        }

        public double EffectiveSeconds
        {
            get
            {
                return timeScale.EffectiveSeconds;
            }

            set
            {
                timeScale.EffectiveSeconds = value;
            }
        }
        #endregion


        public PlaybackPanel()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            _tip = new OPMToolTipManager(this);

            btnOpenDisk.Image = ImageProcessing.DVD;
            btnOpenURL.Image = OPMedia.Core.Properties.Resources.Internet;
            btnSettings.Image = OPMedia.UI.Properties.Resources.Settings;
            btnSignalAnalisys.Image = Resources.btnSignalAnalisys;

            this.HandleCreated += PlaybackPanel_HandleCreated;

            timeScale.PositionChanged += new ValueChangedEventHandler(timeScale_PositionChanged);
            volumeScale.PositionChanged += new ValueChangedEventHandler(volumeScale_PositionChanged);
        }

        internal void FilterStateChanged(FilterState filterState, PlaylistItem pli, MediaTypes mediaType)
        {
            this.FilterState = filterState;
        }

        void volumeScale_PositionChanged(double newVal)
        {
            if (VolumeChanged != null)
            {
                VolumeChanged(newVal);
            }
        }

        void timeScale_PositionChanged(double newVal)
        {
            if (PositionChanged != null)
            {
                PositionChanged(newVal);
            }
        }


        [EventSink(EventNames.PerformTranslation)]
        public void OnPerformTranslation()
        {
            BindEventHandlers(this);
            UpdateStateButtons();
        }

        private void PlaybackPanel_HandleCreated(object sender, EventArgs e)
        {
            OnPerformTranslation();
        }

        private void BindEventHandlers(Control parent)
        {
            if (parent != null && parent.Controls != null)
            {
                foreach (Control ctl in parent.Controls)
                {
                    ImageButton btn = (ctl as ImageButton);
                    if (btn != null)
                    {
                        btn.Click -= OnButtonPressed;
                        btn.Click += OnButtonPressed;
                        //ctl.MouseHover += OnMouseHover;

                        if (_tips.ContainsKey(btn))
                        {
                            OPMToolTip oldTip = _tips[btn];
                            oldTip.RemoveAll();
                            _tips.Remove(btn);
                        }

                        var tip = new OPMToolTip();

                        OPMShortcut cmd = OPMShortcut.CmdOutOfRange;
                        if (Enum.TryParse<OPMShortcut>(btn.Tag as string, out cmd))
                        {
                            string resourceTag = string.Format("TXT_{0}", cmd.ToString().ToUpperInvariant()).Replace("CMD", "BTN");
                            string tipText = Translator.Translate(resourceTag, ShortcutMapper.GetShortcutString(cmd));

                            Image img = null;
                            if (btn.TipImage != null)
                                img = btn.TipImage.Resize(true);
                            else if (btn.Image != null)
                                img = btn.Image.Resize(true);

                            tip.SetSimpleToolTip(btn, tipText, img);
                        }

                        _tips.Add(btn, tip);
                    }
                    else
                    {
                        BindEventHandlers(ctl);
                    }
                }
            }
        }

        [EventSink(EventNames.ThemeUpdated)]
        [EventSink(LocalEventNames.UpdateStateButtons)]
        public void UpdateStateButtons()
        {
            btnToggleXFade.Checked = ProTONEConfig.XFade;
            btnLoopPlay.Checked = ProTONEConfig.LoopPlay;
            btnPlaylistEnd.Checked = SystemScheduler.PlaylistEventEnabled;
            UpdatePlayPauseButton();
        }

        private void UpdatePlayPauseButton()
        {
            Image img = null;

            switch (_filterState)
            {
                case Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running:
                    img = Resources.btnLargePause;
                    break;

                default:
                    img = Resources.btnLargePlay;
                    break;
            }


            btnPlayPause.Image = img;
        }

        private void OnButtonPressed(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            if (btn != null)
            {
                try
                {
                    OPMShortcut cmd = OPMShortcut.CmdOutOfRange;
                    if (Enum.TryParse<OPMShortcut>(btn.Tag as string, out cmd))
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

        private void OnMouseHover(object sender, EventArgs e)
        {
            //ImageButton btn = sender as ImageButton;
            //if (btn != null)
            //{
            //    OPMShortcut cmd = OPMShortcut.CmdOutOfRange;
            //    if (Enum.TryParse<OPMShortcut>(btn.Tag as string, out cmd))
            //    {
            //        string resourceTag = string.Format("TXT_{0}", cmd.ToString().ToUpperInvariant()).Replace("CMD", "BTN");
            //        string tipText = Translator.Translate(resourceTag, ShortcutMapper.GetShortcutString(cmd));
            //        _tip.ShowSimpleToolTip(tipText, btn.Image);
            //    }
            //}
        }
    }
}
