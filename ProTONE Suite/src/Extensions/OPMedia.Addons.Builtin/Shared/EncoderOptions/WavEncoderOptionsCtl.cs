using OPMedia.Core.TranslationSupport;

namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public partial class WavEncoderOptionsCtl : EncoderConfiguratorCtl
    {
        public WavEncoderOptionsCtl()
            : base(new WavEncoderSettings())
        {
            InitializeComponent();
        }

        internal override void Reload()
        {
            string labelText =
                    string.Format("{0}\r\n{1}",
                        Translator.Translate("TXT_CONFIG_NOT_REQUIRED"),
                        Translator.Translate(base.UsedForCdRipper ?
                        "TXT_SAMPLERATE_HINT_SRCCD" :
                        "TXT_SAMPLERATE_HINT_SRCFILE"));

            this.opmLabel1.Text = labelText;
        }
    }
}
