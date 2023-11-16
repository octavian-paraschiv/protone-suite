using System;

namespace OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard
{
    public partial class ChangeEncodingPanel : EditPanelBase
    {
        public override bool ShowPreview
        {
            get
            {
                return false;
            }
        }

        public override bool ShowWordCasing
        {
            get
            {
                return false;
            }
        }

        public ChangeEncodingPanel()
        {
            this.title = "TXT_CHANGEENCODINGPANEL";
            InitializeComponent();

            encoderOptionsCtl.SettingsChanged += new EventHandler(encoderOptionsCtl_SettingsChanged);
        }

        void encoderOptionsCtl_SettingsChanged(object sender, EventArgs e)
        {
            _task.EncoderSettings = encoderOptionsCtl.EncoderSettings;
        }

        protected override void DisplayTask()
        {
            _task.TaskType = TaskType.ChangeEncoding;

            //if (_task.EncoderSettings != null)
            //  encoderOptionsCtl.EncoderSettings = _task.EncoderSettings;

            encoderOptionsCtl.DisplaySettings(false);
        }
    }
}
