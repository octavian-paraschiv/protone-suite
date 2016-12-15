using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;

using System.Configuration;

using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Rendering.Base;

using OPMedia.UI.Themes;
using OPMedia.Runtime.Shortcuts;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.FfdShowApi;
using OPMedia.Core.GlobalEvents;
using OPMedia.Runtime.ProTONE.Configuration;


namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class RenderingFrame : ThemeFrame
    {
        private bool _fullScreen = false;

        private Timer _osdShowTimer = new Timer();
        private Timer _checkVisibilityTimer = new Timer();

        public RenderingFrame()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(RenderingFrame_FormClosing);
            this.VisibleChanged += new EventHandler(RenderingFrame_VisibleChanged);
            this.Resize += new EventHandler(RenderingFrame_Resize);

            this.FormButtons = Themes.FormButtons.Close;
            this.ShowInTaskbar = true;
            this.TopMost = (ProTONEConfig.IsMediaLibrary);

            _osdShowTimer.Interval = 500;
            _osdShowTimer.Tick += new EventHandler(_osdShowTimer_Tick);
        }

        protected override bool AutoCenterEnabled
        {
            get
            {
                return false;
            }
        }

        void _osdShowTimer_Tick(object sender, EventArgs e)
        {
            _osdShowTimer.Stop();

            string text = Translator.Translate("TXT_OSD_FULLSCREEN", (string)(_fullScreen ?
                    Translator.Translate("TXT_ON") : Translator.Translate("TXT_OFF")));

            MediaRenderer.DefaultInstance.DisplayOsdMessage(text);
        }

        void RenderingFrame_Resize(object sender, EventArgs e)
        {
            if (_settingFullScreenState)
            {
                _settingFullScreenState = false;
                _fullScreen = (WindowState == FormWindowState.Maximized);

                ProTONEConfig.FullScreenOn = _fullScreen;

                _osdShowTimer.Stop();
                _osdShowTimer.Start();
            }
        }

        protected override bool CanUpdateInterfaceStyle()
        {
            return !_fullScreen;
        }

        void RenderingFrame_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                // Need to apply windows position in order to preserve monitor affinity
                this.Location = ProTONEConfig.DetachedWindowLocation;
                this.Size = ProTONEConfig.DetachedWindowSize;

                if (ProTONEConfig.FullScreenOn)
                {
                    if (!_fullScreen)
                        SetFullScreen(true, false);
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                ProTONEConfig.DetachedWindowLocation = this.Location;
                ProTONEConfig.DetachedWindowSize = this.Size;
                ProTONEConfig.DetachedWindowState = this.WindowState;
            }
        }

        void RenderingFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProTONEConfig.FullScreenOn = _fullScreen;
            if (!_fullScreen)
            {
                ProTONEConfig.DetachedWindowLocation = this.Location;
                ProTONEConfig.DetachedWindowSize = this.Size;
                ProTONEConfig.DetachedWindowState = this.WindowState;
            }
        }

        bool _settingFullScreenState = false;
        public void SetFullScreen(bool fullScreen, bool persistWindowState)
        {
            try
            {
                _fullScreen = fullScreen && ProTONEConfig.IsPlayer;
                _settingFullScreenState = true;

                this.TitleBarVisible = !fullScreen;

                if (_fullScreen)
                {
                    // Save position before entring full screen if told so
                    if (persistWindowState)
                    {
                        ProTONEConfig.DetachedWindowLocation = this.Location;
                        ProTONEConfig.DetachedWindowSize = this.Size;
                        ProTONEConfig.DetachedWindowState = FormWindowState.Normal;
                        
                    }

                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Location = ProTONEConfig.DetachedWindowLocation;
                    this.Size = ProTONEConfig.DetachedWindowSize;
                }
            }
            finally
            {
                this.TopMost = (ProTONEConfig.IsMediaLibrary && !_fullScreen);
            }
        }

        protected override bool OnChangeWindowState(FormWindowState requestedState)
        {
            SetFullScreen(requestedState == FormWindowState.Maximized, true);
            return true;
        }

        internal void ToggleFullScreen()
        {
            SetFullScreen(!_fullScreen, true);
        }

        public void SetContextMenuStrip(ContextMenuStrip cms)
        {
            renderingZone.ContextMenuStrip = cms;
        }

        public Control RenderingZone
        {
            get { return renderingZone; }
        }

        protected override bool IsShortcutAllowed(OPMShortcut cmd)
        {
            // Any command in range is valid.
            return (ShortcutMapper.CmdFirst <= cmd && cmd <= ShortcutMapper.CmdLast);
        }

        [EventSink(GlobalEvents.EventNames.RestoreRenderingRegionPosition)]
        public void OnRestoreRenderingRegionPosition()
        {
            this.WindowState = FormWindowState.Normal;
            this.Location = ProTONEConfig.DetachedWindowLocation;
            this.Size = ProTONEConfig.DetachedWindowSize;
            SetFullScreen(false, false);
        }

        //public override void OnExecuteShortcut(OPMShortcutEventArgs args)
        //{
        //    if (this == Form.ActiveForm && !args.Handled && ExecuteShortcutEscalation != null)
        //    {
        //        // Escalate everything to whomever has created the rendering form.
        //        ExecuteShortcutEscalation(args);
        //    }
        //}
    }
}