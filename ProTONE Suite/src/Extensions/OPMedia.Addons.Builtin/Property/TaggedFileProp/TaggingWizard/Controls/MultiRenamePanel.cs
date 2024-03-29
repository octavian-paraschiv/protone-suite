using System;

namespace OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard
{
    public partial class MultiRenamePanel : EditPanelBase
    {
        public MultiRenamePanel()
        {
            this.title = "TXT_MULTIRENAME";
            InitializeComponent();
            txtRenamePattern.TextChanged += new EventHandler(txtRenamePattern_TextChanged);
        }

        void txtRenamePattern_TextChanged(object sender, EventArgs e)
        {
            _task.RemamePattern = txtRenamePattern.Text;
        }

        protected override void DisplayTask()
        {
            _task.TaskType = TaskType.MultiRename;
            txtRenamePattern.Text = _task.RemamePattern;
        }
    }
}
