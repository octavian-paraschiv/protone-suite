using OPMedia.Addons.Builtin.CatalogExplorer.ImportWizard.Controls;
using OPMedia.Addons.Builtin.CatalogExplorer.ImportWizard.Tasks;
using OPMedia.Addons.Builtin.CatalogExplorer.SearchWizard.Controls;
using OPMedia.Addons.Builtin.Configuration;
using OPMedia.Addons.Builtin.Navigation.CatalogExplorer.DataLayer;
using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.Addons;
using OPMedia.Runtime.Addons.AddonsBase.Navigation;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.RemoteControl;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Controls.Dialogs;
using OPMedia.UI.Dialogs;
using OPMedia.UI.Generic;
using OPMedia.UI.Wizards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EventNames = OPMedia.Core.EventNames;

/*
 * IMPORTANT NOTE: In Media Catalog, instead of paths are used VPaths. 
 * These are in fact catalog item ID's that are converted to strings, for navigation actions.
 */
namespace OPMedia.Addons.Builtin.CatalogExplorer
{
    public partial class AddonPanel : NavBaseCtl
    {
        Catalog _cat = null;

        private System.Windows.Forms.Timer updateUiTimer;

        private Stack<string> bckPaths = new Stack<string>();
        private Stack<string> fwdPaths = new Stack<string>();

        GenericWaitDialog _waitDialog = null;

        bool _busyWithDisplay = false;

        List<string> _recentFiles = new List<string>();

        BackgroundWorker _bwOpen = new BackgroundWorker();
        BackgroundWorker _bwSave = new BackgroundWorker();
        BackgroundWorker _bwMerge = new BackgroundWorker();

        public override List<string> HandledFileTypes
        {
            get
            {
                return new List<string>(new string[] { "ctx" });
            }
        }

        public override string GetHelpTopic()
        {
            return "CatalogExplorer";
        }

        public AddonPanel()
        {
            InitializeComponent();

            tsmiProTONEPlay.Image = ImageProcessing.Player16;
            tsmiProTONEEnqueue.Image = ImageProcessing.Player16;
            tsmiCatalog.Image = ImageProcessing.Library16;

            tsbCatalog.Image = ImageProcessing.Library;

            this.AddonImage = ImageProcessing.Library;
            this.SmallAddonImage = ImageProcessing.Library16;

            tvCatalog.Cursor = lvCatalogFolder.Cursor = Cursors.Default;

            updateUiTimer = new System.Windows.Forms.Timer();
            updateUiTimer.Enabled = true;
            updateUiTimer.Interval = (int)(BuiltinAddonConfig.FEPreviewTimer * 1000);
            updateUiTimer.Start();
            updateUiTimer.Tick += new EventHandler(updateUiTimer_Tick);

            tvCatalog.AfterSelect += new TreeViewEventHandler(tvCatalog_AfterSelect);

            lvCatalogFolder.NavigateUp += new OPMedia.Addons.Builtin.CatalogExplorer.Controls.NavigateUpHandler(NavigateUp);

            this.HandleCreated += new EventHandler(AddonPanel_HandleCreated);
            this.HandleDestroyed += new EventHandler(AddonPanel_HandleDestroyed);

            string[] list = StringUtils.ToStringArray(BuiltinAddonConfig.MCRecentFiles, '?');
            if (list != null)
            {
                foreach (string file in list)
                {
                    string lowercaseFile = file.ToLowerInvariant();
                    if (File.Exists(lowercaseFile) && !_recentFiles.Contains(lowercaseFile)
                        && lowercaseFile.EndsWith(".ctx"))
                    {
                        _recentFiles.Add(lowercaseFile);
                    }
                }
            }

            _bwOpen.WorkerSupportsCancellation =
            _bwSave.WorkerSupportsCancellation =
            _bwMerge.WorkerSupportsCancellation = false;

            _bwOpen.WorkerReportsProgress =
            _bwSave.WorkerReportsProgress =
            _bwMerge.WorkerReportsProgress = false;

            _bwOpen.DoWork += new DoWorkEventHandler(BackgroundOpen);
            _bwSave.DoWork += new DoWorkEventHandler(BackgroundSave);
            _bwMerge.DoWork += new DoWorkEventHandler(BackgroundMerge);

            _bwOpen.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundWorkCompleted);
            _bwSave.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundWorkCompleted);
            _bwMerge.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundWorkCompleted);

        }

        [EventSink(EventNames.PerformTranslation)]
        public void OnPerformTranslation()
        {
            lvCatalogFolder.Translateheaders();
            Translator.TranslateControl(this, DesignMode);
            Translator.TranslateToolStrip(toolStripMain, DesignMode);
            Translator.TranslateToolStrip(contextMenuStrip, DesignMode);
        }

        [EventSink(EventNames.ExecuteShortcut)]
        public void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (FindForm() != null && !args.Handled && ContainsFocus)
            {
                switch (args.cmd)
                {
                    case OPMShortcut.CmdGenericRename:
                        HandleAction(ToolAction.ToolActionRename);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericNew:
                        CreateNewCatalog();
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericOpen:
                        OpenCatalog();
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericSave:
                        HandleAction(ToolAction.ToolActionSave);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdCatalogMerge:
                        HandleAction(ToolAction.ToolActionMerge);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdCatalogWizard:
                        HandleAction(ToolAction.ToolActionCatalog);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericDelete:
                        HandleAction(ToolAction.ToolActionDelete);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericRefresh:
                        HandleAction(ToolAction.ToolActionReload);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericSearch:
                        HandleAction(ToolAction.ToolActionSearch);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdNavigateBack:
                        HandleAction(ToolAction.ToolActionBack);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdNavigateForward:
                        HandleAction(ToolAction.ToolActionFwd);
                        args.Handled = true;
                        break;
                }
            }
        }

        void AddonPanel_HandleDestroyed(object sender, EventArgs e)
        {
            BuiltinAddonConfig.SplitterDistanceMC = pnlSplitter.SplitterDistance;
        }

        void AddonPanel_HandleCreated(object sender, EventArgs e)
        {
            pnlSplitter.SplitterDistance = BuiltinAddonConfig.SplitterDistanceMC;
            pnlSplitter.SplitterWidth = 3;

            OnPerformTranslation();

            string launchCatalogPath = this.Tag as string;
            if (string.IsNullOrEmpty(launchCatalogPath) && BuiltinAddonConfig.MCOpenLastCatalog)
            {
                if (_recentFiles.Count > 0)
                {
                    // Open newest file (the one at the end of the list).
                    launchCatalogPath = _recentFiles[_recentFiles.Count - 1];
                }
            }

            DisplayCurrentPath();

            OpenFileWithCheck(launchCatalogPath, true);
        }

        void tvCatalog_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                AddCurrentFolderToHistory();
                BrowseCatalogFolder(e.Node.Tag as CatalogItem);
            }
        }

        CatalogItem _curFolder = null;

        public void ClearHistory()
        {
            bckPaths.Clear();
            fwdPaths.Clear();
        }

        public void ExploreBack()
        {
            fwdPaths.Push(_curFolder.VPath);
            CatalogItem target = _cat.GetByVPath(bckPaths.Pop());
            BrowseCatalogFolder(target);
        }

        public void ExploreForward()
        {
            bckPaths.Push(_curFolder.VPath);
            CatalogItem target = _cat.GetByVPath(fwdPaths.Pop());
            BrowseCatalogFolder(target);
        }

        private void NavigateUp()
        {
            AddCurrentFolderToHistory();
            BrowseCatalogFolder(ParentFolderTarget, _curFolder);
        }

        public CatalogItem ExploreBackTarget
        {
            get
            {
                return (bckPaths.Count > 0) ? _cat.GetByVPath(bckPaths.Peek()) : null;
            }
        }

        public CatalogItem ExploreForwardTarget
        {
            get
            {
                return (fwdPaths.Count > 0) ? _cat.GetByVPath(fwdPaths.Peek()) : null;
            }
        }

        public CatalogItem ParentFolderTarget
        {
            get
            {
                if (_curFolder != null && _curFolder.VPath != Catalog.CatalogVPath && !_curFolder.IsRoot)
                {
                    return _cat.GetByItemID(_curFolder.ParentItemID);
                }

                return null;
            }
        }

        private void AddCurrentFolderToHistory()
        {
            if (_curFolder != null)
            {
                bckPaths.Push(_curFolder.VPath);
            }
        }

        private void BrowseCatalogFolder(CatalogItem folder, CatalogItem prevFolder = null)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (folder != null)
                {
                    lvCatalogFolder.DisplayCatalogFolder(_cat, folder, prevFolder);
                    _curFolder = folder;

                    List<string> paths = new List<string>();
                    paths.Add(folder.VPath);
                    RaiseNavigationAction(NavActionType.ActionSelectDirectory, paths, _cat);

                    if (tvCatalog.SelectedNode == null ||
                        tvCatalog.SelectedNode.Tag as CatalogItem != folder)
                    {
                        tvCatalog.AfterSelect -= new TreeViewEventHandler(tvCatalog_AfterSelect);

                        tvCatalog.SelectedNode = null;

                        TreeNode[] matchingNodes = tvCatalog.Nodes.Find(folder.VPath, true);
                        if (matchingNodes != null && matchingNodes.Length > 0)
                        {
                            tvCatalog.SelectedNode = matchingNodes[0];
                        }

                        tvCatalog.AfterSelect += new TreeViewEventHandler(tvCatalog_AfterSelect);
                    }
                }
                else
                {
                    lvCatalogFolder.DisplayCatalogRoots(_cat);
                    _curFolder = null;

                    List<string> paths = new List<string>();
                    paths.Add(Catalog.CatalogVPath);
                    RaiseNavigationAction(NavActionType.ActionSelectDirectory, paths, _cat);
                }
            }
            finally
            {
                Cursor = Cursors.Default;


            }
        }

        private void OnDoubleClickDirectory(object sender, DoubleClickDirectoryEventArgs args)
        {
            if (args != null)
            {
                CatalogItem folder = _cat.GetByVPath(args.m_strPath);
                AddCurrentFolderToHistory();
                BrowseCatalogFolder(folder);
            }
        }

        private void OnDoubleClickFile(object sender, DoubleClickFileEventArgs args)
        {
            if (args != null)
            {
                CatalogItem ci = _cat.GetByVPath(args.m_strPath);

                string launchPath = BuildLaunchPath(ci, true);
                if (!string.IsNullOrEmpty(launchPath))
                {
                    List<string> paths = new List<string>();
                    paths.Add(launchPath);

                    RaiseNavigationAction(NavActionType.ActionDoubleClickFile, paths);
                }
            }
        }

        string _prevSerialNumber = string.Empty;
        string _prevDriveLetter = string.Empty;

        private string BuildLaunchPath(CatalogItem ci, bool singleFile)
        {
            string launchPath = string.Empty;

            if (ci != null && ci.OrigItemPath.StartsWith("$:"))
            {
                if (!singleFile && _prevSerialNumber == ci.RootSerialNumber)
                {
                    launchPath = ci.OrigItemPath.Replace("$", _prevDriveLetter);
                }
                else
                {
                    CatalogItem root = _cat.GetRootBySerialNumber(ci.RootSerialNumber);
                    if (root != null)
                    {
                        InsertDriveNotifyDialog dlg = new InsertDriveNotifyDialog(
                            ci.RootItemLabel,
                            root.Description,
                            ci.RootSerialNumber,
                            (DriveType)root.ItemType);

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            string driveLetter = dlg.ActualDriveLetter.Substring(0, 1);
                            launchPath = ci.OrigItemPath.Replace("$", driveLetter);

                            _prevSerialNumber = ci.RootSerialNumber;
                            _prevDriveLetter = driveLetter;
                        }
                    }
                }
            }
            else
            {
                launchPath = ci.OrigItemPath;
            }

            return launchPath;
        }

        private void OnSelectDirectory(object sender, SelectDirectoryEventArgs args)
        {
            if (args != null)
            {
                List<string> paths = new List<string>();
                paths.Add(args.m_strPath);
                RaiseNavigationAction(NavActionType.ActionSelectDirectory, paths, _cat);
            }
        }

        private void OnSelectFile(object sender, SelectFileEventArgs args)
        {
            if (args != null)
            {
                List<string> paths = new List<string>();
                paths.Add(args.m_strPath);
                RaiseNavigationAction(NavActionType.ActionSelectFile, paths, _cat);
            }
        }

        private void OnSelectMultipleItems(object sender, SelectMultipleItemsEventArgs args)
        {
            if (args != null)
            {
                RaiseNavigationAction(NavActionType.ActionSelectMultipleItems, args.m_strPaths, _cat);
            }
        }

        public override void OnActiveStateChanged(bool isActive)
        {
            updateUiTimer.Enabled = isActive;
        }

        void updateUiTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                updateUiTimer.Stop();

                if (BuiltinAddonConfig.MCRememberRecentFiles)
                {
                    tsbOpen.DropDownButtonWidth = 15;
                }
                else
                {
                    tsbOpen.DropDownButtonWidth = 1;
                    tsbOpen.DropDownItems.Clear();
                }

                SuspendLayout();
                OnUpdateUi(toolStripMain.Items);
                OnUpdateUi(contextMenuStrip.Items);
                ResumeLayout();

                DisplayCurrentPath();
            }
            finally
            {
                updateUiTimer.Start();
            }
        }

        string _prevPath = string.Empty;
        private void DisplayCurrentPath()
        {
            Image img = null;
            string text = null;

            try
            {
                if (_curFolder != null)
                {
                    _prevPath = _curFolder.OrigItemPath;

                    img = lvCatalogFolder.GetImageOfItemType(_curFolder.ItemType);
                    text = Translator.Translate("TXT_CURRENT_PATH", _curFolder.OrigItemPath);
                }
                else if (_cat != null)
                {
                    img = ImageProvider.GetIconOfFileType("ctx");
                    text = Translator.Translate("TXT_CURRENT_PATH", _cat.Path);
                }
            }
            catch
            {
                img = null;
                text = null;
            }

            EventDispatch.DispatchEvent(EventNames.SetMainStatusBar, text, img);
        }

        private void OnToolAction(object sender, EventArgs e)
        {
            HandleToolAction(sender as ToolStripItem);
        }

        private void HandleToolAction(ToolStripItem tsi)
        {
            if (tsi == null || string.IsNullOrEmpty(tsi.Tag as string))
                return;

            ToolAction action = (ToolAction)Enum.Parse(typeof(ToolAction),
                   tsi.Tag as string);

            HandleAction(action);
        }

        private void HandleAction(ToolAction action)
        {
            if (!IsToolActionEnabled(action))
                return;

            switch (action)
            {
                case ToolAction.ToolActionReload:
                    GlobalReload();
                    break;

                case ToolAction.ToolActionNew:
                    CreateNewCatalog();
                    break;
                case ToolAction.ToolActionOpen:
                    OpenCatalog();
                    break;

                case ToolAction.ToolActionSave:
                    if (_cat != null && _cat.IsInDefaultLocation)
                    {
                        goto case ToolAction.ToolActionSaveAs;
                    }
                    else
                    {
                        SaveCatalogNoDialog();
                    }
                    break;

                case ToolAction.ToolActionSaveAs:
                    SaveCatalogWithDialog();
                    break;

                case ToolAction.ToolActionDelete:
                    List<string> sel = GetSelectedVPaths();
                    foreach (string vpath in sel)
                    {
                        CatalogItem item = _cat.GetByVPath(vpath);
                        if (item != null)
                        {
                            if (item.IsFolder)
                            {
                                // Only folders in tree view
                                tvCatalog.RemoveItem(item);
                            }

                            lvCatalogFolder.RemoveItem(item);

                            item.Delete();
                        }
                    }
                    break;

                case ToolAction.ToolActionMerge:
                    MergeCatalog();
                    break;

                case ToolAction.ToolActionCatalog:
                    {
                        Task task = null;

                        if (_cat != null)
                        {
                            task = new Task();
                            task.CatalogPath = _cat.Path;
                        }

                        if (ImportWizardMain.Execute(ref task) == DialogResult.OK)
                        {
                            _cat = new Catalog((task as Task).CatalogPath);
                            DisplayCatalog();
                        }
                    }
                    break;

                case ToolAction.ToolActionBack:
                    ExploreBack();
                    break;

                case ToolAction.ToolActionFwd:
                    ExploreForward();
                    break;

                case ToolAction.ToolActionUp:
                    NavigateUp();
                    break;

                case ToolAction.ToolActionSearch:
                    SearchWizard.Tasks.Task taskSearch = new SearchWizard.Tasks.Task();
                    taskSearch.Catalog = _cat;
                    taskSearch.SearchPath = (_curFolder != null) ? _curFolder.VPath : null;
                    if (SearchWizardMain.Execute(taskSearch) == DialogResult.OK)
                    {
                        switch (taskSearch.Action)
                        {
                            case ToolAction.ToolActionProTONEEnqueue:
                                RunProTONEActionOnVPaths(taskSearch.MatchingItems,
                                    CommandType.EnqueueFiles);
                                break;

                            case ToolAction.ToolActionProTONEPlay:
                                RunProTONEActionOnVPaths(taskSearch.MatchingItems,
                                    CommandType.PlayFiles);
                                break;

                            case ToolAction.ToolActionJumpToItem:
                                if (taskSearch.MatchingItems.Count > 0)
                                {
                                    JumpToItem(taskSearch.MatchingItems[0]);
                                }
                                break;

                            case ToolAction.ToolActionDelete:
                                foreach (string vpath in taskSearch.MatchingItems)
                                {
                                    CatalogItem item = _cat.GetByVPath(vpath);
                                    if (item != null)
                                    {
                                        if (item.IsFolder)
                                        {
                                            // Only folders in tree view
                                            tvCatalog.RemoveItem(item);
                                        }

                                        lvCatalogFolder.RemoveItem(item);

                                        item.Delete();
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case ToolAction.ToolActionProTONEEnqueue:
                    RunProTONEActionOnVPaths(GetSelectedVPaths(),
                        CommandType.EnqueueFiles);
                    break;

                case ToolAction.ToolActionProTONEPlay:
                    RunProTONEActionOnVPaths(GetSelectedVPaths(),
                        CommandType.PlayFiles);
                    break;

                case ToolAction.ToolActionRename:
                    lvCatalogFolder.Rename();
                    break;
            }
        }

        private void RunProTONEActionOnVPaths(List<string> vpaths,
            CommandType commandType)
        {
            List<string> launchPaths = new List<string>();
            foreach (string vPath in vpaths)
            {
                CatalogItem ci = _cat.GetByVPath(vPath);
                string launchPath = BuildLaunchPath(ci, false);
                if (!string.IsNullOrEmpty(launchPath))
                {
                    launchPaths.Add(launchPath);
                }
            }

            _prevSerialNumber = string.Empty;
            _prevDriveLetter = string.Empty;

            if (launchPaths.Count > 0)
            {
                PlayerRemoteControl.SendPlayerCommand(commandType, launchPaths.ToArray());
            }
        }



        private void CreateNewCatalog()
        {
            _cat = new Catalog();
            DisplayCatalog();
        }

        private void ShowWaitDialog(string message)
        {
            CloseWaitDialog();
            _waitDialog = new GenericWaitDialog();
            _waitDialog.ShowDialog(message);
        }

        private void CloseWaitDialog()
        {
            if (_waitDialog != null)
            {
                _waitDialog.Close();
                _waitDialog = null;
            }
        }

        private void DisplayCatalog()
        {
            _busyWithDisplay = true;
            CursorHelper.ShowWaitCursor(this, true);

            try
            {
                if (_cat != null && _cat.IsValid)
                {
                    tvCatalog.DisplayCatalog(_cat);
                    AddRecentFile(_cat.Path);
                }

                ClearHistory();
                DisplayCurrentPath();
            }
            finally
            {
                CursorHelper.ShowWaitCursor(this, false);
                _busyWithDisplay = false;
            }
        }

        private void OnUpdateUi(ToolStripItemCollection tsic)
        {
            if (tsic == null)
                return;

            bool playerInstalled = File.Exists(ProTONEConfig.PlayerInstallationPath);
            tsmiSepProTONE.Visible = tsmiProTONEEnqueue.Visible = tsmiProTONEPlay.Visible =
                playerInstalled;

            for (int i = 0; i < tsic.Count; i++)
            {
                ToolStripItem btn = tsic[i] as ToolStripItem;

                if (btn == null)
                    continue;

                btn.Enabled = true;

                string tag = btn.Tag as string;
                if (string.IsNullOrEmpty(tag))
                {
                    continue;
                }

                ToolAction action = ToolAction.ToolActionNothing;
                try
                {
                    action = (ToolAction)Enum.Parse(typeof(ToolAction), tag);
                }
                catch
                {
                    action = ToolAction.ToolActionNothing;
                }

                if (action == ToolAction.ToolActionNothing)
                {
                    continue;
                }

                List<string> selItems = GetSelectedVPaths();
                switch (action)
                {
                    case ToolAction.ToolActionNew:
                        BuildMenuText(btn, "TXT_NEW", string.Empty, OPMShortcut.CmdGenericNew);
                        break;

                    case ToolAction.ToolActionOpen:
                        BuildMenuText(btn, "TXT_OPEN", string.Empty, OPMShortcut.CmdGenericOpen);
                        break;

                    case ToolAction.ToolActionSave:
                        BuildMenuText(btn, "TXT_SAVE", string.Empty, OPMShortcut.CmdGenericSave);
                        btn.Enabled = (_cat != null);
                        break;

                    case ToolAction.ToolActionSaveAs:
                        BuildMenuText(btn, "TXT_SAVE_AS", string.Empty, OPMShortcut.CmdOutOfRange);
                        btn.Enabled = (_cat != null);
                        break;

                    case ToolAction.ToolActionBack:
                        btn.Enabled = ExploreBackTarget != null;
                        BuildMenuText(btn, "TXT_BACK", ExploreBackTarget, OPMShortcut.CmdNavigateBack);
                        break;

                    case ToolAction.ToolActionFwd:
                        btn.Enabled = ExploreForwardTarget != null;
                        BuildMenuText(btn, "TXT_FORWARD", ExploreForwardTarget, OPMShortcut.CmdNavigateForward);
                        break;

                    case ToolAction.ToolActionUp:
                        btn.Enabled = _cat != null;

                        string parentName = (ParentFolderTarget == null) ? Translator.Translate("TXT_ROOT") :
                            ParentFolderTarget.OrigItemPath.Replace("$:", ParentFolderTarget.RootItemLabel);

                        BuildMenuText(btn, "TXT_UP", parentName, OPMShortcut.CmdNavigateUp);
                        break;

                    case ToolAction.ToolActionSearch:
                        BuildMenuText(btn, "TXT_SEARCH", string.Empty, OPMShortcut.CmdGenericSearch);
                        btn.Enabled = (_cat != null);
                        break;

                    case ToolAction.ToolActionReload:
                        BuildMenuText(btn, "TXT_REFRESH", string.Empty, OPMShortcut.CmdGenericRefresh);
                        btn.Enabled = (_curFolder != null);
                        break;

                    case ToolAction.ToolActionDelete:
                        BuildMenuText(btn, "TXT_DELETE", string.Empty, OPMShortcut.CmdGenericDelete);
                        btn.Enabled = GetSelectedVPaths().Count > 0;
                        break;

                    case ToolAction.ToolActionCopy:
                    case ToolAction.ToolActionCut:
#if MC_COPY_PASTE
                        btn.Visible = true;
                        btn.Enabled = GetSelectedVPaths().Count > 0;
#else
                        btn.Visible = false;
                        btn.Enabled = false;
#endif
                        break;

                    case ToolAction.ToolActionPaste:
#if MC_COPY_PASTE
                        btn.Enabled = (_fileTask != null &&
                            (_fileTask.FileTaskType == FileTaskType.Copy || _fileTask.FileTaskType == FileTaskType.Move));
                        btn.Visible = true;
#else
                        btn.Visible = false;
                        btn.Enabled = false;
#endif

                        break;

                    case ToolAction.ToolActionMerge:
                        BuildMenuText(btn, "TXT_MERGECATALOGS", string.Empty, OPMShortcut.CmdCatalogMerge);
                        btn.Enabled = (_cat != null && _cat.IsValid);
                        break;

                    case ToolAction.ToolActionCatalog:
                        BuildMenuText(btn, "TXT_CATALOG", string.Empty, OPMShortcut.CmdCatalogWizard);
                        btn.Enabled = (_cat == null || !_cat.IsInDefaultLocation);
                        break;

                    case ToolAction.ToolActionRename:
                        BuildMenuText(btn, "TXT_RENAME", string.Empty, OPMShortcut.CmdGenericRename);
                        btn.Enabled = GetSelectedVPaths().Count == 1;
                        break;

                    case ToolAction.ToolActionProTONEEnqueue:
                    case ToolAction.ToolActionProTONEPlay:
                        if (btn.Visible)
                        {
                            string text = (action == ToolAction.ToolActionProTONEEnqueue) ?
                                "TXT_PROTONE_ENQUEUE" : "TXT_PROTONE_PLAY";

                            BuildMenuText(btn, text, string.Empty, OPMShortcut.CmdOutOfRange);

                            bool enable = false;
                            foreach (string path in GetSelectedOrigPaths())
                            {
                                if (SupportedFileProvider.Instance.IsSupportedMedia(path))
                                {
                                    enable = true;
                                    break;
                                }
                            }
                            btn.Enabled = enable;
                        }
                        break;
                }
            }
        }

        private bool IsToolActionEnabled(ToolAction action)
        {
            if (IsOperationInProgress)
                return false;

            for (int i = 0; i < contextMenuStrip.Items.Count; i++)
            {
                ToolStripItem btn = contextMenuStrip.Items[i] as ToolStripItem;

                if (btn == null ||
                    string.IsNullOrEmpty(btn.Tag as string))
                {
                    // Not an action button, continue.
                    continue;
                }

                if ((btn.Tag as string).ToLowerInvariant() == action.ToString().ToLowerInvariant())
                {
                    return btn.Enabled;
                }
            }

            for (int i = 0; i < toolStripMain.Items.Count; i++)
            {
                ToolStripItem btn = toolStripMain.Items[i] as ToolStripItem;

                if (btn == null ||
                    string.IsNullOrEmpty(btn.Tag as string))
                {
                    // Not an action button, continue.
                    continue;
                }

                if ((btn.Tag as string).ToLowerInvariant() == action.ToString().ToLowerInvariant())
                {
                    return btn.Enabled;
                }
            }

            return true;
        }

        private void BuildMenuText(ToolStripItem tsm, string tag, CatalogItem targetItem, OPMShortcut command)
        {
            BuildMenuText(tsm, tag, (targetItem != null) ? targetItem.OrigItemPath : string.Empty, command);
        }

        private void BuildMenuText(ToolStripItem tsm, string tag, string param, OPMShortcut command)
        {
            tsm.ToolTipText =
                (tsm.Enabled && !string.IsNullOrEmpty(param)) ?
                Translator.Translate(tag) + ": " + param :
                Translator.Translate(tag);
            tsm.Text = Translator.Translate(tag);

            if (tsm is OPMToolStripMenuItem)
            {
                string text = tsm.ToolTipText;
                if (text.Length > 45)
                {
                    tsm.Text = text.Substring(0, 45) + "...";
                }
                else
                {
                    tsm.ToolTipText = string.Empty;
                    tsm.Text = text;
                }

                if (command != OPMShortcut.CmdOutOfRange)
                {
                    (tsm as OPMToolStripMenuItem).ShortcutKeyDisplayString =
                        ShortcutMapper.GetShortcutString(command);
                }
            }
            else
            {
                if (command != OPMShortcut.CmdOutOfRange)
                {
                    tsm.ToolTipText +=
                        string.Format(" ({0})", ShortcutMapper.GetShortcutString(command));
                }

                if (command == OPMShortcut.CmdGenericOpen && _recentFiles.Count > 0 && BuiltinAddonConfig.MCRememberRecentFiles)
                {
                    tsm.ToolTipText += "\r\n" + Translator.Translate("TXT_OPENRECENTFILEDROPDOWN");
                }
            }
        }

        private List<string> GetSelectedVPaths()
        {
            List<string> selectedItems = new List<string>();
            foreach (ListViewItem item in lvCatalogFolder.SelectedItems)
            {
                if (item.Tag as CatalogItem == null)
                    continue;

                string itemPath = (item.Tag as CatalogItem).VPath;

                selectedItems.Add(itemPath);
            }

            return selectedItems;
        }

        private List<string> GetSelectedOrigPaths()
        {
            List<string> selectedItems = new List<string>();
            foreach (ListViewItem item in lvCatalogFolder.SelectedItems)
            {
                if (item.Tag as CatalogItem == null)
                    continue;

                string itemPath = (item.Tag as CatalogItem).OrigItemPath;

                selectedItems.Add(itemPath);
            }

            return selectedItems;
        }

        public override void Reload(object target)
        {
            _busyWithDisplay = true;
            CursorHelper.ShowWaitCursor(this, true);

            try
            {
                tvCatalog.Reload(target as CatalogItem);
                lvCatalogFolder.Reload(target as CatalogItem);
                ReloadProperties();
            }
            finally
            {
                _busyWithDisplay = false;
                CursorHelper.ShowWaitCursor(this, false);
            }
        }

        private void JumpToItem(string vPath)
        {
            CatalogItem ci = _cat.GetByVPath(vPath);
            if (ci != null)
            {
                CatalogItem parent = _cat.GetByItemID(ci.ParentItemID);
                if (parent != null)
                {
                    TreeNode tn = tvCatalog.FindNode(parent);
                    if (tn != null)
                    {
                        tvCatalog.SelectedNode = tn;
                        tn.Expand();

                        lvCatalogFolder.FindNode(ci);
                    }
                }
            }
        }

        protected override BaseCfgPanel GetBaseCfgPanel()
        {
            return new CatalogExplorerCfgPanel();
        }

        #region Open files

        private void OpenCatalog()
        {
            tsbOpen.HideDropDown();

            OPMOpenFileDialog dlg = new OPMOpenFileDialog();
            dlg.Title = Translator.Translate("TXT_OPENCATALOG");
            dlg.Filter = Translator.Translate("TXT_CATALOG_FILTER");
            dlg.InitialDirectory = BuiltinAddonConfig.MCLastOpenedFolder;

            dlg.FillFavoriteFoldersEvt += () => { return ProTONEConfig.GetFavoriteFolders("FavoriteFolders"); };
            dlg.AddToFavoriteFolders += (s) => { return ProTONEConfig.AddToFavoriteFolders(s); };
            dlg.ShowAddToFavorites = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                BuiltinAddonConfig.MCLastOpenedFolder = Path.GetDirectoryName(dlg.FileName);
                OpenFileWithCheck(dlg.FileName, false);
            }
        }

        private void OpenFileWithCheck(string fileName, bool openRecent)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            fileName = fileName.ToLowerInvariant();

            if (File.Exists(fileName))
            {
                _bwOpen.RunWorkerAsync(fileName);
                ShowWaitDialog("TXT_WAIT_LOADING_CATALOG");
            }
            else
            {
                if (_recentFiles.Contains(fileName))
                {
                    _recentFiles.Remove(fileName);
                }

                string mainMessage = Translator.Translate("TXT_FILE_NOT_FOUND", fileName);
                if (openRecent)
                {
                    mainMessage += "\r\n";
                    mainMessage += Translator.Translate("TXT_RECENT_FILE_REMOVED");
                }

                ErrorDispatcher.DispatchError(mainMessage, false);
            }
        }

        private void OnOpenRecentFile(object sender, ToolStripItemClickedEventArgs e)
        {
            OpenFileWithCheck(e.ClickedItem.Text, true);
        }

        void BackgroundOpen(object sender, DoWorkEventArgs e)
        {
            _cat = new Catalog(e.Argument as string);
        }

        #endregion

        #region Save files

        private void SaveCatalogWithDialog()
        {
            OPMSaveFileDialog dlg = new OPMSaveFileDialog();
            dlg.Title = Translator.Translate("TXT_SAVECATALOG");
            dlg.Filter = Translator.Translate("TXT_CATALOG_FILTER");
            dlg.DefaultExt = "ctx";
            dlg.InitialDirectory = BuiltinAddonConfig.MCLastOpenedFolder;

            dlg.FillFavoriteFoldersEvt += () => { return ProTONEConfig.GetFavoriteFolders("FavoriteFolders"); };
            dlg.AddToFavoriteFolders += (s) => { return ProTONEConfig.AddToFavoriteFolders(s); };
            dlg.ShowAddToFavorites = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                BuiltinAddonConfig.MCLastOpenedFolder = Path.GetDirectoryName(dlg.FileName);
                _bwSave.RunWorkerAsync(dlg.FileName);
                ShowWaitDialog("TXT_WAIT_SAVING_CATALOG");
            }
        }

        private void SaveCatalogNoDialog()
        {
            _bwSave.RunWorkerAsync(null);
        }


        void BackgroundSave(object sender, DoWorkEventArgs e)
        {
            string path = e.Argument as string;
            if (path == null)
                _cat.Save();
            else
                _cat.Save(path);
        }

        #endregion

        #region Merge files

        private void MergeCatalog()
        {
            if (_cat != null && _cat.IsValid)
            {
                OPMOpenFileDialog dlg = new OPMOpenFileDialog();
                dlg.Title = Translator.Translate("TXT_MERGECATALOG");
                dlg.Filter = Translator.Translate("TXT_CATALOG_FILTER");
                dlg.InitialDirectory = BuiltinAddonConfig.MCLastOpenedFolder;

                dlg.FillFavoriteFoldersEvt += () => { return ProTONEConfig.GetFavoriteFolders("FavoriteFolders"); };
                dlg.AddToFavoriteFolders += (s) => { return ProTONEConfig.AddToFavoriteFolders(s); };
                dlg.ShowAddToFavorites = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    BuiltinAddonConfig.MCLastOpenedFolder = Path.GetDirectoryName(dlg.FileName);
                    _bwMerge.RunWorkerAsync(dlg.FileName);
                    ShowWaitDialog("TXT_WAIT_MERGING_CATALOG");
                }
            }
        }

        void BackgroundMerge(object sender, DoWorkEventArgs e)
        {
            _cat.MergeCatalog(e.Argument as string);
        }

        #endregion

        #region Common open/save/merge

        void OnBackgroundWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                CatalogException cex = e.Error as CatalogException;
                if (cex != null)
                {
                    Logger.LogException(cex);
                    string msg = Translator.Translate(cex.Message, cex.Detail);
                    EventDispatch.DispatchEvent(EventNames.ShowMessageBox, msg, Translator.Translate("TXT_APP_NAME"), MessageBoxIcon.Exclamation);
                }
                else
                    ErrorDispatcher.DispatchError(e.Error, false);
            }

            DisplayCatalog();
            CloseWaitDialog();
        }

        bool IsOperationInProgress
        {
            get
            {
                return (_bwOpen.IsBusy || _bwSave.IsBusy || _bwMerge.IsBusy || _busyWithDisplay);
            }
        }

        #endregion

        private void OnPrepareRecentFileList(object sender, EventArgs e)
        {
            tsbOpen.DropDownItems.Clear();

            if (_recentFiles != null && _recentFiles.Count > 0)
            {
                foreach (string file in _recentFiles)
                {
                    OPMToolStripMenuItem tsmi = new OPMToolStripMenuItem(file);
                    tsbOpen.DropDownItems.Add(tsmi);
                }
            }
            else
            {
                HandleToolAction(tsbOpen);
            }
        }

        private void AddRecentFile(string p)
        {
            if (BuiltinAddonConfig.MCRememberRecentFiles || BuiltinAddonConfig.MCOpenLastCatalog)
            {
                if (!_cat.IsInDefaultLocation)
                {
                    while (_recentFiles.Contains(_cat.Path.ToLowerInvariant()))
                    {
                        _recentFiles.Remove(_cat.Path.ToLowerInvariant());
                    }

                    while (_recentFiles.Count >= BuiltinAddonConfig.MCRecentFilesCount)
                    {
                        _recentFiles.RemoveAt(0); // Remove oldest file in recent file list
                    }

                    _recentFiles.Add(_cat.Path.ToLowerInvariant());

                    BuiltinAddonConfig.MCRecentFiles =
                        StringUtils.FromStringArray(_recentFiles.ToArray(), '?');

                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }

    internal enum ToolAction : int
    {
        ToolActionNothing = -1,

        ToolActionNew = 0,
        ToolActionOpen,
        ToolActionSave,
        ToolActionSaveAs,

        ToolActionBack,
        ToolActionFwd,
        ToolActionUp,

        ToolActionSearch,
        ToolActionReload,

        ToolActionCopy,
        ToolActionCut,
        ToolActionPaste,
        ToolActionLink,

        ToolActionRename,
        ToolActionDelete,

        ToolActionMerge,
        ToolActionCatalog,

        ToolActionJumpToItem,

        ToolActionProTONEPlay,
        ToolActionProTONEEnqueue,

        ToolActionLaunch,
    }

    #region Wizard
    public static class ImportWizardMain
    {
        public static DialogResult Execute(ref Task initTask)
        {
            if (initTask == null)
            {
                initTask = new Task();
            }

            Type[] pages = new Type[]
                {
                    typeof(WizImportStep1Ctl),
                    typeof(WizImportStep2Ctl)
                };

            return WizardHostForm.CreateWizard("TXT_IMPORTWIZARD", pages, true, initTask);
        }
    }

    public static class SearchWizardMain
    {
        public static DialogResult Execute(BackgroundTask initTask)
        {
            Type[] pages = new Type[]
                {
                    typeof(WizMCSearchStep1Ctl),
                };

            return WizardHostForm.CreateWizard("TXT_SEARCHWIZARD_MC", pages, true, initTask,
                OPMedia.UI.Properties.Resources.Search.ToIcon());
        }

        public static DialogResult Execute()
        {
            return Execute(null);
        }
    }
    #endregion
}
