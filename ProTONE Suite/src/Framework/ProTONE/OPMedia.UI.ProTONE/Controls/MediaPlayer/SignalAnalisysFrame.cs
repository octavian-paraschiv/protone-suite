using OPMedia.Core.Shortcuts;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Themes;
using System;
using System.Windows.Forms;

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
            this.Location = ProTONEConfig.SA_WindowLocation;
            this.Size = ProTONEConfig.SA_WindowSize;
            this.WindowState = ProTONEConfig.SA_WindowState;
        }

        protected override bool AutoCenterEnabled => false;

        void OnFormClosing(object sender, FormClosingEventArgs e)
        {
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