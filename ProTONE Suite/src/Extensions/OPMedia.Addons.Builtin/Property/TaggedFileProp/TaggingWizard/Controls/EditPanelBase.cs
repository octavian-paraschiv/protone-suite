using OPMedia.UI.Controls;

namespace OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard
{
    public partial class EditPanelBase : OPMBaseControl
    {
        protected Task _task = new Task();
        protected string title = "EditPanelBase";

        public virtual bool ShowWordCasing
        {
            get
            {
                return true;
            }
        }
        public virtual bool ShowPreview
        {
            get
            {
                return true;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public EditPanelBase()
        {
            InitializeComponent();
        }

        public void SetTask(Task task)
        {
            if (task == null)
            {
                task = new Task();
            }

            _task = task;
            DisplayTask();
        }

        public Task GetTask()
        {
            return _task;
        }

        protected virtual void DisplayTask()
        {
        }


    }
}
