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
using OPMedia.Runtime.ProTONE.AudioMetering;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class SignalAnalisysFrame : ThemeFrame
    {
        public SignalAnalisysFrame()
        {
            InitializeComponent();
            SetTitle(Translator.TranslateTaggedString("TXT_APP_NAME: TXT_SIGNALANALYSIS"));

            ProTONEConfig.SignalAnalisysFunctions = SignalAnalisysFunction.All;

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = true;
            this.TopMost = true;

            this.HandleDestroyed += SignalAnalisysFrame_HandleDestroyed;
            this.FormClosing += new FormClosingEventHandler(OnFormClosing);
            this.Shown += OnShown;
        }

        private void SignalAnalisysFrame_HandleDestroyed(object sender, EventArgs e)
        {
            ProTONEConfig.SignalAnalisysFunctions = SignalAnalisysFunction.None;
        }

        private void OnShown(object sender, EventArgs e)
        {
            if (WasapiMeter.Instance.Start())
            {
                this.Location = ProTONEConfig.SA_WindowLocation;
                this.Size = ProTONEConfig.SA_WindowSize;
                this.WindowState = ProTONEConfig.SA_WindowState;
            }
            else
            {
                Close();
            }
        }

        protected override bool AutoCenterEnabled
        {
            get
            {
                return false;
            }
        }

        void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            WasapiMeter.Instance.Stop();
            ProTONEConfig.SA_WindowLocation = this.Location;
            ProTONEConfig.SA_WindowSize = this.Size;
            ProTONEConfig.SA_WindowState = this.WindowState;
        }


        protected override bool IsShortcutAllowed(OPMShortcut cmd)
        {
            // Any command in range is valid.
            return (ShortcutMapper.CmdFirst <= cmd && cmd <= ShortcutMapper.CmdLast);
        }
    }
}