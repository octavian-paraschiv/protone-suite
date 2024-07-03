using OPMedia.Runtime.Addons.AddonsBase.Preview;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.ShellSupport;
using System;
using System.Collections.Generic;

namespace OPMedia.Addons.Builtin.Player
{
    public partial class AddonPanel : PreviewBaseCtl
    {
        public override string GetHelpTopic()
        {
            return "PlayerPreviewPanel";
        }

        public AddonPanel()
            : base()
        {
            InitializeComponent();
            mediaPlayer.CompactView = true;
        }

        public override List<string> HandledFileTypes
        {
            get
            {
                List<String> fileTypes = new List<string>();
                fileTypes.AddRange(SupportedFileProvider.Instance.SupportedAudioTypes);
                fileTypes.AddRange(SupportedFileProvider.Instance.SupportedVideoTypes);
                return fileTypes;
            }
        }

        protected override void DoBeginPreview(string item, object additionalData)
        {
            mediaPlayer.PlayFiles(new string[] { item });
            ProTONEConfig.SignalAnalisysFunctions = SignalAnalisysFunction.VUMeter | SignalAnalisysFunction.Spectrogram;

            if (additionalData != null)
            {
                mediaPlayer.StopPlayback();
            }
        }

        protected override void DoEndPreview()
        {
            mediaPlayer.StopPlayback();
            ProTONEConfig.SignalAnalisysFunctions = SignalAnalisysFunction.None;
        }
    }
}
