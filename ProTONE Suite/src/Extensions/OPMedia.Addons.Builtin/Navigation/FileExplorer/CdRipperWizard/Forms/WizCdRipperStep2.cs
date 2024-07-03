using OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Tasks;
using OPMedia.UI.Dialogs;
using OPMedia.UI.Wizards;
using System;
using System.IO;
using System.Windows.Forms;

namespace OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Forms
{
    public partial class WizCdRipperStep2 : WizardBaseCtl
    {
        //public override Size DesiredSize
        //{
        //    get
        //    {
        //        return new Size(660, 455);
        //    }
        //}

        public WizCdRipperStep2()
        {
            InitializeComponent();
        }

        protected override void OnPageLeave_Finishing()
        {
            (BkgTask as Task).EncoderSettings = encoderOptionsCtl.EncoderSettings;
        }

        protected override void OnPageEnter_MovingNext()
        {
            cmbFilePattern.SelectedIndex = 0;

            Wizard.CanFinish = false;

            if (string.IsNullOrEmpty((BkgTask as Task).OutputFolder))
            {
                (BkgTask as Task).OutputFolder = Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic);
            }

            txtDestFolder.Text = (BkgTask as Task).OutputFolder;

            encoderOptionsCtl.DisplaySettings(true);

            CheckFinishButton();
        }

        private void opmButton1_Click(object sender, EventArgs e)
        {
            OPMFolderBrowserDialog dlg = new OPMFolderBrowserDialog();
            dlg.SelectedPath = txtDestFolder.Text;
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtDestFolder.Text = dlg.SelectedPath;
            }
        }

        private void OnOutputFolderChanged(object sender, EventArgs e)
        {
            (BkgTask as Task).OutputFolder = txtDestFolder.Text;
            CheckFinishButton();
        }

        private void OnFilePatternChanged(object sender, EventArgs e)
        {
            (BkgTask as Task).OutputFilePattern = cmbFilePattern.Text;
            CheckFinishButton();
        }

        private void CheckFinishButton()
        {
            Wizard.CanFinish = Directory.Exists((BkgTask as Task).OutputFolder) &&
                !string.IsNullOrEmpty((BkgTask as Task).OutputFilePattern);
        }


    }
}
