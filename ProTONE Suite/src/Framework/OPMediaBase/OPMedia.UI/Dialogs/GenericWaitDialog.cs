using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Themes;
using System;
using System.Windows.Forms;

namespace OPMedia.UI.Dialogs
{
    public partial class GenericWaitDialog : ToolForm, IDisposable
    {
        protected bool ShowProgress { get; set; }

        public void SetText(string msg)
        {
            this.lblNotifyText.Text = Translator.Translate(msg);
        }

        public DialogResult ShowDialog(string message, Form parent = null)
        {
            SetText(message);

            if (parent == null)
                return ShowDialog();

            return ShowDialog(parent);
        }

        public GenericWaitDialog()
        {
            InitializeComponent();

            // TODO: fixme
            // base.TitleBarVisible = false;

            this.ShowProgress = false;
            this.Shown += OnShown;
        }

        private void OnShown(object sender, EventArgs e)
        {
            SetProgress(0);
        }

        protected override bool AllowCloseOnKeyDown(Keys key)
        {
            return false;
        }

        protected void SetProgress(double percentage)
        {
            bool progress = this.ShowProgress;
            pbProgress.Visible = progress;
            pictureBox1.Visible = !progress;

            if (progress)
            {
                pbProgress.Value = percentage;
            }
        }
    }

    public class CancellableWaitDialog : GenericWaitDialog
    {
        public bool EscapePressed { get; private set; }

        protected override bool AllowCloseOnKeyDown(Keys key)
        {
            this.EscapePressed = (key == Keys.Escape);
            return this.EscapePressed;
        }
    }
}