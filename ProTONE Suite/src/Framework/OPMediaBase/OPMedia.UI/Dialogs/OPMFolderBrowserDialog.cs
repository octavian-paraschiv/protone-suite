using OPMedia.Core;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.UI.Themes;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace OPMedia.UI.Dialogs
{
    public delegate bool PerformPathValidationHandler(string path);

    public partial class OPMFolderBrowserDialog : ToolForm
    {
        public bool ShowSpecialFolders { get; set; }
        public bool ShowNewFolderButton { get; set; }
        public string SelectedPath { get; set; }
        public string Description { get; set; }

        public event PerformPathValidationHandler PerformPathValidation = null;

        string _title = null;

        public OPMFolderBrowserDialog(string title = null)
        {
            InitializeComponent();

            _title = title;

            this.InheritAppIcon = false;

            this.Description = Translator.Translate("TXT_SELECT_FOLDER");


            this.ShowNewFolderButton = true;
            this.SelectedPath = PathUtils.CurrentDir;

            btnOK.Enabled = false;
            tvExplorer.LabelEdit = true;

            tvExplorer.AfterSelect += new TreeViewEventHandler(tvExplorer_AfterSelect);
            this.Load += new EventHandler(OPMFolderBrowserDialog_Load);
        }

        void tvExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnOK.Enabled = false;
            btnNewFolder.Enabled = false;

            if (Directory.Exists(tvExplorer.SelectedNodePath))
            {
                btnOK.Enabled = (PerformPathValidation == null || PerformPathValidation(tvExplorer.SelectedNodePath));

                try
                {
                    DriveInfo drvInvo = new DriveInfo(Path.GetPathRoot(tvExplorer.SelectedNodePath));
                    btnNewFolder.Enabled = (drvInvo.AvailableFreeSpace > 0 && drvInvo.IsReady);
                }
                catch { }
            }

            if (btnOK.Enabled)
            {
                this.SelectedPath = tvExplorer.SelectedNodePath;
            }
            else
            {
                this.SelectedPath = string.Empty;
            }
        }

        void OPMFolderBrowserDialog_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_title))
                SetTitle("TXT_SELECT_FOLDER");
            else
                SetTitle(_title);

            btnNewFolder.Visible = this.ShowNewFolderButton;
            lblDescription.Text = this.Description;

            tvExplorer.ShowSpecialFolders = this.ShowSpecialFolders;
            tvExplorer.InitOPMShellTreeView();

            tvExplorer.DrillToFolder(this.SelectedPath);
        }

        void OPMFolderBrowserDialog_Shown(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.SelectedPath))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(SelectedPath))
            {
                string newName = string.Format("NewFolder_{0}", StringUtils.GenerateRandomToken(4));
                string fullName = Path.Combine(SelectedPath, newName);

                Directory.CreateDirectory(fullName);
                Thread.Sleep(700);
                if (Directory.Exists(fullName))
                {
                    TreeNode node = tvExplorer.CreateTreeNode(fullName);
                    tvExplorer.SelectedNode.Nodes.Add(node);
                    tvExplorer.SelectedNode = node;

                    tvExplorer.SelectedNode.BeginEdit();
                }
            }
        }
    }
}
