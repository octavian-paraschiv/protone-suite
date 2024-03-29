#region Copyright � opmedia research 2006
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	AddonPanel.cs
#endregion

#region Using directives
using OPMedia.Addons.Builtin.Configuration;
using OPMedia.Addons.Builtin.FileExplorer.FileOperations.Forms;
using OPMedia.Addons.Builtin.FileExplorer.SearchWizard.Controls;
using OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Forms;
using OPMedia.Addons.Builtin.Navigation.FileExplorer.FileOperations.Tasks;
using OPMedia.Addons.Builtin.Properties;
using OPMedia.Addons.Builtin.TaggedFileProp;
using OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.NetworkAccess;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.Addons;
using OPMedia.Runtime.Addons.ActionManagement;
using OPMedia.Runtime.Addons.AddonsBase.Navigation;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.RemoteControl;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Controls.Dialogs;
using OPMedia.UI.FileTasks;
using OPMedia.UI.Generic;
using OPMedia.UI.Themes;
using OPMedia.UI.Wizards;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EventNames = OPMedia.Core.EventNames;


#endregion

namespace OPMedia.Addons.Builtin.FileExplorer
{
    /// <summary>
    /// The panel for the File Explorer addon.
    /// </summary>
    public partial class AddonPanel : NavBaseCtl
    {
        private bool disableEvents = false;

        //private bool drivesDisplayed = false;

        private ImageList ilDrives = null;

        private FileTaskForm _pasteFileTask = null;
        private FileTaskForm _deleteFileTask = null;

        private ImageList ilFavorites = null;


        private Timer updateUiTimer;
        private Timer previewTimer;

        public static bool IsRequired { get { return true; } }

        public override string GetHelpTopic()
        {
            return "FileExplorer";
        }

        public AddonPanel() : base()
        {
            InitializeComponent();

            tsmiProTONEPlay.Image = ImageProcessing.Player16;
            tsmiProTONEEnqueue.Image = ImageProcessing.Player16;

            opmShellList.BorderStyle = BorderStyle.None;

            opmShellList.MultiSelect = true;

            opmShellList.Clear();

            ilAddon.Images.Add(Resources.FileExplorer);

            this.tsbFavorites.Image = OPMedia.UI.Properties.Resources.Favorites;
            this.tsmiFavorites.Image = OPMedia.UI.Properties.Resources.Favorites16;
            this.tsbNewFolder.Image = OPMedia.UI.Properties.Resources.New_Folder_Command;
            this.tsmiNewFolder.Image = OPMedia.UI.Properties.Resources.New_Folder_Command16;

            this.AddonImage = Resources.FileExplorer;
            this.SmallAddonImage = Resources.FileExplorer16;

            updateUiTimer = new Timer();
            updateUiTimer.Enabled = true;
            updateUiTimer.Interval = 500;
            updateUiTimer.Start();
            updateUiTimer.Tick += new EventHandler(updateUiTimer_Tick);


            previewTimer = new Timer();
            previewTimer.Stop();
            previewTimer.Tick += new EventHandler(previewTimer_Tick);

            ilDrives = new ImageList();
            ilDrives.ImageSize = new Size(16, 16);
            ilDrives.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;

            ilFavorites = new ImageList();
            ilFavorites.ImageSize = new Size(16, 16);
            ilFavorites.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            ilFavorites.Images.Add(Resources.none);

            toolStripMain.ImageList = ilDrives;

            opmShellList.KeyDown += new KeyEventHandler(opmShellList_KeyDown);
            opmShellList.QueryLinkedFiles += new QueryLinkedFilesHandler(OnQueryLinkedFiles);
            opmShellList.ItemRenamed += new ItemRenameHandler(OnRenamed);

            this.HandleCreated += new EventHandler(AddonPanel_HandleCreated);
            this.Leave += new EventHandler(AddonPanel_Leave);

            opmShellList.QueryDisplayName += opmShellList_QueryDisplayName;
        }

        string opmShellList_QueryDisplayName(string fsi)
        {
            if (fsi != null)
            {
                if (fsi.ToUpperInvariant().EndsWith("CDA"))
                {
                    CDAFileInfo cdfi = MediaFileInfo.FromPath(fsi) as CDAFileInfo;
                    if (cdfi != null)
                        return cdfi.DisplayName;
                }

                return Path.GetFileName(fsi);
            }

            return string.Empty;
        }

        List<string> OnQueryLinkedFiles(string path, FileTaskType taskType)
        {
            FEFileTaskSupport support = new FEFileTaskSupport(null);
            if (File.Exists(path))
            {
                return support.GetChildFiles(path, taskType);
            }

            return null;
        }

        void OnRenamed(string newPath)
        {
            AddonHostForm masterForm = FindForm() as AddonHostForm;
            if (masterForm != null)
            {
                SelectFileEventArgs args = new SelectFileEventArgs();
                args.m_strPath = newPath;
                OnSelectFile(this, args);
            }
        }

        [EventSink(EventNames.ExecuteShortcut)]
        public void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (FindForm() != null && !args.Handled && ContainsFocus)
            {
                switch (args.cmd)
                {
                    case OPMShortcut.CmdGenericNew:
                        HandleAction(ToolAction.ToolActionNewFolder);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericRename:
                        HandleAction(ToolAction.ToolActionRename);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericPaste:
                        HandleAction(ToolAction.ToolActionPaste);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericCut:
                        HandleAction(ToolAction.ToolActionCut);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericCopy:
                        HandleAction(ToolAction.ToolActionCopy);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericDelete:
                        HandleAction(ToolAction.ToolActionDelete);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdSwitchWindows:
                        CancelAutoPreview();
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdChangeDisk:
                        tsbDrives.ShowDropDown();
                        tsbDrives.Select();

                        if (tsbDrives.DropDownItems.Count > 0)
                        {
                            tsbDrives.DropDownItems[0].Select();
                        }

                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdFavManager:
                        HandleAction(ToolAction.ToolActionFavoritesManage);
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

                    // Already implemented inside the list
                    //case OPMShortcut.CmdNavigateUp:
                    //    HandleAction(ToolAction.ToolActionUp);
                    //    args.Handled = true;
                    //    break;

                    case OPMShortcut.CmdGenericSearch:
                        HandleAction(ToolAction.ToolActionSearch);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdGenericRefresh:
                        HandleAction(ToolAction.ToolActionReload);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdTaggingWizard:
                        HandleAction(ToolAction.ToolActionTaggingWizard);
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdCdRipperWizard:
                        HandleAction(ToolAction.ToolActionCdRipper);
                        args.Handled = true;
                        break;
                }
            }

            //Application.DoEvents();

        }

        void AddonPanel_Leave(object sender, EventArgs e)
        {
            if (previewTimer.Enabled)
            {
                CancelAutoPreview();
            }
        }

        void CancelAutoPreview()
        {
            previewTimer.Stop();
            RaiseNavigationAction(NavActionType.ActionCancelAutoPreview, null, new object());
        }

        [EventSink(EventNames.PerformTranslation)]
        public void OnPerformTranslation()
        {
            Translator.TranslateControl(this, DesignMode);
            Translator.TranslateToolStrip(toolStripMain, DesignMode);
            Translator.TranslateToolStrip(contextMenuStrip, DesignMode);
            Translator.TranslateToolStripItem(tsbFavorites, DesignMode);
        }

        void AddonPanel_HandleCreated(object sender, EventArgs e)
        {
            OnPerformTranslation();
        }


        void opmShellList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Modifiers)
            {
                case Keys.None:
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Escape:
                                previewTimer.Stop();
                                if (BuiltinAddonConfig.FEPreviewTimer > 0)
                                {
                                    RaiseNavigationAction(NavActionType.ActionCancelAutoPreview, null, null);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        #region Navigation events
        public override void OnActiveStateChanged(bool isActive)
        {
            updateUiTimer.Enabled = isActive;
        }

        void updateUiTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                updateUiTimer.Stop();

                SuspendLayout();

                OnUpdateUi(toolStripMain.Items);
                OnUpdateUi(contextMenuStrip.Items);

                OnUpdateUi(tsbFavorites.DropDownItems);
                OnUpdateUi(tsmiFavorites.DropDownItems);

                DisplayCurrentPath();

                ResumeLayout();
            }
            finally
            {
                updateUiTimer.Start();
            }
        }

        void previewTimer_Tick(object sender, EventArgs e)
        {
            previewTimer.Stop();

            List<string> paths = opmShellList.SelectedPaths;
            if (paths != null && paths.Count > 0)
            {
                RaiseNavigationAction(NavActionType.ActionDoubleClickFile, paths);
            }
        }


        private void OnLoad(object sender, EventArgs e)
        {
            ChangePath(AppConfig.LastExploredFolder);
        }


        private void OnDoubleClickDirectory(object sender, DoubleClickDirectoryEventArgs args)
        {
            Application.UseWaitCursor = (true);

            if (!disableEvents)
            {
                disableEvents = true;
                string folderPath = args.m_strPath;
                DisplayCurrentPath();
                disableEvents = false;
            }

            Application.UseWaitCursor = (false);
        }

        private void OnDoubleClickFile(object sender, DoubleClickFileEventArgs args)
        {
            CancelAutoPreview();

            if (args != null)
            {
                List<string> paths = new List<string>();
                paths.Add(args.m_strPath);

                RaiseNavigationAction(NavActionType.ActionDoubleClickFile, paths);
            }
        }

        private void OnSelectDirectory(object sender, SelectDirectoryEventArgs args)
        {
            CancelAutoPreview();

            if (args != null)
            {
                List<string> paths = new List<string>();
                paths.Add(args.m_strPath);
                RaiseNavigationAction(NavActionType.ActionSelectDirectory, paths);
            }
        }

        private void OnSelectFile(object sender, SelectFileEventArgs args)
        {
            CancelAutoPreview();

            if (args != null)
            {
                List<string> paths = new List<string>();
                paths.Add(args.m_strPath);
                RaiseNavigationAction(NavActionType.ActionSelectFile, paths);

                if (paths != null &&
                    paths.Count == 1 &&
                    !string.IsNullOrEmpty(paths[0]))
                {
                    ActionRequest req = new ActionRequest();
                    req.ActionType = ActionType.ActionBeginPreview;
                    req.Items = paths;

                    bool autoPreviewAvailable = false;
                    if (AddonsCore.Instance.CanDispatchAction(req, ref autoPreviewAvailable))
                    {
                        if (autoPreviewAvailable && BuiltinAddonConfig.FEPreviewTimer > 0)
                        {
                            previewTimer.Interval = (int)(BuiltinAddonConfig.FEPreviewTimer * 1000);
                            previewTimer.Start();
                            RaiseNavigationAction(NavActionType.ActionPrepareAutoPreview, null, null);
                        }
                        else
                        {
                            RaiseNavigationAction(NavActionType.ActionNotifyPreviewableItem, null, null);
                        }
                    }
                    else
                    {
                        RaiseNavigationAction(NavActionType.ActionNotifyNonPreviewableItem, null, null);
                    }
                }
            }
        }

        private void OnSelectMultipleItems(object sender, SelectMultipleItemsEventArgs args)
        {
            CancelAutoPreview();

            if (args != null)
            {
                RaiseNavigationAction(NavActionType.ActionSelectMultipleItems, args.m_strPaths);
            }
        }
        #endregion

        #region Helpers


        private void ChangePath(string path)
        {
            Application.UseWaitCursor = (true);
            if (!disableEvents)
            {
                disableEvents = true;
                opmShellList.Path = path;
                DisplayCurrentPath();

                disableEvents = false;
            }
            Application.UseWaitCursor = (false);
        }

        string _prevPath = string.Empty;
        private void DisplayCurrentPath()
        {
            _prevPath = opmShellList.Path;

            Image img = null;
            string text = null;

            try
            {
                img = ImageProvider.GetIcon(opmShellList.Path, false);
                text = Translator.Translate("TXT_CURRENT_PATH", opmShellList.Path);
            }
            catch
            {
                img = null;
                text = null;
            }

            EventDispatch.DispatchEvent(EventNames.SetMainStatusBar, text, img);
        }

        private void NavigateUp()
        {
            try
            {
                if (!opmShellList.IsInDriveRoot)
                {
                    opmShellList.Path = PathUtils.ParentDir;
                    DisplayCurrentPath();
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Handle tool actions (toolbar/menu)
        private void OnToolAction(object sender, EventArgs e)
        {
            HandleToolAction(sender as ToolStripItem);
        }

        private void HandleToolAction(ToolStripItem tsi)
        {
            try
            {
                if (tsi == null || string.IsNullOrEmpty(tsi.Tag as string))
                    return;

                ToolAction action = (ToolAction)Enum.Parse(typeof(ToolAction),
                       tsi.Tag as string);

                HandleAction(action);
            }
            catch
            {
            }
        }


        private void HandleAction(ToolAction action)
        {
            try
            {
                if (!IsToolActionEnabled(action))
                    return;

                FileTaskForm activeFileTask = null;

                updateUiTimer.Stop();

                List<string> selItems = opmShellList.SelectedPaths;
                switch (action)
                {
                    case ToolAction.ToolActionNewFolder:
                        opmShellList.CreateNewFolder();
                        return;

                    case ToolAction.ToolActionBack:
                        opmShellList.ExploreBack();
                        return;

                    case ToolAction.ToolActionFwd:
                        opmShellList.ExploreForward();
                        return;

                    case ToolAction.ToolActionUp:
                        NavigateUp();
                        return;

                    case ToolAction.ToolActionSearch:
                        SearchWizard.Tasks.Task taskSearch = new SearchWizard.Tasks.Task();
                        taskSearch.SearchPath = opmShellList.Path;
                        if (SearchWizardMain.Execute(taskSearch) == DialogResult.OK)
                        {
                            switch (taskSearch.Action)
                            {
                                case ToolAction.ToolActionProTONEEnqueue:
                                    {
                                        if (taskSearch.MatchingItems.Count > 0)
                                        {
                                            PlayerRemoteControl.SendPlayerCommand(
                                                CommandType.EnqueueFiles,
                                                taskSearch.MatchingItems.ToArray());
                                        }
                                    }
                                    break;

                                case ToolAction.ToolActionProTONEPlay:
                                    {
                                        if (taskSearch.MatchingItems.Count > 0)
                                        {
                                            PlayerRemoteControl.SendPlayerCommand(
                                                CommandType.PlayFiles,
                                                taskSearch.MatchingItems.ToArray());
                                        }
                                    }
                                    break;

                                case ToolAction.ToolActionJumpToItem:
                                    if (taskSearch.MatchingItems.Count > 0)
                                    {
                                        opmShellList.JumpToItem(taskSearch.MatchingItems[0], false);
                                    }
                                    break;

                                case ToolAction.ToolActionTaggingWizard:
                                    {
                                        TaggedFileProp.TaggingWizard.Task taskTagging = new TaggedFileProp.TaggingWizard.Task();
                                        foreach (string item in taskSearch.MatchingItems)
                                        {
                                            if (Directory.Exists(item))
                                            {
                                                taskTagging.Files.AddRange(PathUtils.EnumFiles(item, "*.mp?", SearchOption.AllDirectories));
                                            }
                                            else if (File.Exists(item))
                                            {
                                                taskTagging.Files.Add(item);
                                            }
                                        }

                                        TaggingWizardMain.Execute(FindForm(), taskTagging);
                                        ReloadProperties();
                                    }
                                    break;

                                case ToolAction.ToolActionCopy:
                                    _pasteFileTask = new FEFileTaskForm(FileTaskType.Copy, taskSearch.MatchingItems, opmShellList.Path);
                                    break;

                                case ToolAction.ToolActionCut:
                                    _pasteFileTask = new FEFileTaskForm(FileTaskType.Move, taskSearch.MatchingItems, opmShellList.Path);
                                    break;

                                case ToolAction.ToolActionDelete:
                                    _deleteFileTask = new FEFileTaskForm(FileTaskType.Delete, taskSearch.MatchingItems, opmShellList.Path);
                                    activeFileTask = _deleteFileTask;
                                    break;

                                case ToolAction.ToolActionLaunch:
                                    if (taskSearch.MatchingItems.Count > 0)
                                    {
                                        opmShellList.OpenItem(taskSearch.MatchingItems[0]);
                                    }
                                    break;
                            }
                        }
                        return;

                    case ToolAction.ToolActionReload:
                        GlobalReload();
                        return;

                    case ToolAction.ToolActionTaggingWizard:
                        {
                            TaggedFileProp.TaggingWizard.Task taskTagging = new TaggedFileProp.TaggingWizard.Task();
                            foreach (string item in opmShellList.SelectedPaths)
                            {
                                if (Directory.Exists(item))
                                {
                                    taskTagging.Files.AddRange(PathUtils.EnumFiles(item, "*.mp?", SearchOption.AllDirectories));
                                }
                                else if (File.Exists(item))
                                {
                                    taskTagging.Files.Add(item);
                                }
                            }

                            TaggingWizardMain.Execute(FindForm(), taskTagging);

                            if (taskTagging.TaskType != TaskType.MultiRename)
                                ReloadProperties();
                            else
                                RaiseNavigationAction(NavActionType.ActionSelectMultipleItems, opmShellList.SelectedPaths);
                        }
                        return;

                    case ToolAction.ToolActionCdRipper:
                        {
                            OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Tasks.Task task =
                                new Navigation.FileExplorer.CdRipperWizard.Tasks.Task();

                            task.OutputFolder = opmShellList.Path;

                            CdRipperWizardMain.Execute(task);
                            ReloadNavigation();
                        }
                        break;

                    case ToolAction.ToolActionCopy:
                        _pasteFileTask = new FEFileTaskForm(FileTaskType.Copy, opmShellList.SelectedPaths, opmShellList.Path);
                        return;

                    case ToolAction.ToolActionCut:
                        _pasteFileTask = new FEFileTaskForm(FileTaskType.Move, opmShellList.SelectedPaths, opmShellList.Path);
                        return;

                    case ToolAction.ToolActionPaste:
                        if (_pasteFileTask != null)
                        {
                            _pasteFileTask.DestFolder = opmShellList.Path;
                            activeFileTask = _pasteFileTask;
                        }
                        break;

                    case ToolAction.ToolActionDelete:
                        if (!opmShellList.IsInEditMode)
                        {
                            _deleteFileTask = new FEFileTaskForm(FileTaskType.Delete, opmShellList.SelectedPaths, opmShellList.Path);
                            activeFileTask = _deleteFileTask;
                        }
                        break;

                    case ToolAction.ToolActionRename:
                        Rename();
                        return;

                    case ToolAction.ToolActionFavoritesAdd:
                        {
                            List<string> favorites = new List<string>(ProTONEConfig.GetFavoriteFolders("FavoriteFolders"));
                            if (favorites.Contains(opmShellList.Path))
                                return;

                            favorites.Add(opmShellList.Path);
                            ProTONEConfig.SetFavoriteFolders(favorites, "FavoriteFolders");
                        }
                        return;

                    case ToolAction.ToolActionFavoritesManage:
                        new FavoriteFoldersManager("FavoriteFolders").ShowDialog();
                        return;

                    case ToolAction.ToolActionProTONEEnqueue:
                        {
                            List<String> items = opmShellList.SelectedPaths;
                            if (items.Count > 0)
                            {
                                PlayerRemoteControl.SendPlayerCommand(
                                    CommandType.EnqueueFiles,
                                    items.ToArray());
                            }
                        }
                        break;

                    case ToolAction.ToolActionProTONEPlay:
                        {
                            List<String> items = opmShellList.SelectedPaths;
                            if (items.Count > 0)
                            {
                                PlayerRemoteControl.SendPlayerCommand(
                                    CommandType.PlayFiles,
                                    items.ToArray());
                            }
                        }
                        break;

                }

                if (activeFileTask != null)
                {
                    RaiseNavigationAction(NavActionType.ActionCancelAutoPreview, null, null);

                    try
                    {
                        opmShellList.EnableAutoRefresh(false);
                        DialogResult dlg = activeFileTask.ShowDialog();
                    }
                    finally
                    {
                        if (activeFileTask.RequiresRefresh)
                        {
                            opmShellList.RefreshList(true);
                        }

                        opmShellList.EnableAutoRefresh(true);

                        if (activeFileTask.FileTaskType == FileTaskType.Delete)
                            _deleteFileTask = null;
                        else
                            _pasteFileTask = null;

                        activeFileTask = null;
                    }
                }
            }
            finally
            {
                updateUiTimer.Start();
            }
        }

        private void Rename()
        {
            opmShellList.Rename();
            ReloadProperties();
        }

        public override void Reload(object target)
        {
            opmShellList.RefreshList(true);
        }

        #endregion

        #region Update UI (toolbar/menu)
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

                List<string> selItems = opmShellList.SelectedPaths;
                switch (action)
                {
                    case ToolAction.ToolActionNewFolder:
                        btn.Enabled = true;
                        btn.Visible = true;
                        BuildMenuText(btn, "TXT_NEWFOLDER", string.Empty, OPMShortcut.CmdGenericNew);
                        break;

                    case ToolAction.ToolActionBack:
                        btn.Enabled = opmShellList.ExploreBackTarget.Length > 0;
                        BuildMenuText(btn, "TXT_BACK", opmShellList.ExploreBackTarget, OPMShortcut.CmdNavigateBack);
                        break;

                    case ToolAction.ToolActionFwd:
                        btn.Enabled = opmShellList.ExploreForwardTarget.Length > 0;
                        BuildMenuText(btn, "TXT_FORWARD", opmShellList.ExploreForwardTarget, OPMShortcut.CmdNavigateForward);
                        break;

                    case ToolAction.ToolActionUp:
                        btn.Enabled = !opmShellList.IsInDriveRoot;
                        BuildMenuText(btn, "TXT_UP", opmShellList.ParentFolderTarget, OPMShortcut.CmdNavigateUp);
                        break;

                    case ToolAction.ToolActionSearch:
                        BuildMenuText(btn, "TXT_SEARCH", string.Empty, OPMShortcut.CmdGenericSearch);
                        btn.Enabled = !string.IsNullOrEmpty(opmShellList.Path);
                        break;

                    case ToolAction.ToolActionReload:
                        BuildMenuText(btn, "TXT_REFRESH", string.Empty, OPMShortcut.CmdGenericRefresh);
                        btn.Enabled = !string.IsNullOrEmpty(opmShellList.Path);
                        break;

                    case ToolAction.ToolActionCopy:
                        BuildMenuText(btn, "TXT_COPY", string.Empty, OPMShortcut.CmdGenericCopy);
                        btn.Enabled = opmShellList.SelectedPaths.Count > 0;
                        break;
                    case ToolAction.ToolActionCut:
                        BuildMenuText(btn, "TXT_CUT", string.Empty, OPMShortcut.CmdGenericCut);
                        btn.Enabled = opmShellList.SelectedPaths.Count > 0;
                        break;
                    case ToolAction.ToolActionDelete:
                        BuildMenuText(btn, "TXT_DELETE", string.Empty, OPMShortcut.CmdGenericDelete);
                        btn.Enabled = opmShellList.SelectedPaths.Count > 0;
                        break;

                    case ToolAction.ToolActionPaste:
                        BuildMenuText(btn, "TXT_PASTE", string.Empty, OPMShortcut.CmdGenericPaste);
                        btn.Enabled = (_pasteFileTask != null &&
                            (_pasteFileTask.FileTaskType == FileTaskType.Copy || _pasteFileTask.FileTaskType == FileTaskType.Move));
                        break;

                    case ToolAction.ToolActionRename:
                        BuildMenuText(btn, "TXT_RENAME", string.Empty, OPMShortcut.CmdGenericRename);
                        btn.Enabled = opmShellList.SelectedPaths.Count == 1;
                        break;

                    case ToolAction.ToolActionProTONEEnqueue:
                    case ToolAction.ToolActionProTONEPlay:
                        if (btn.Visible)
                        {
                            string text = (action == ToolAction.ToolActionProTONEEnqueue) ?
                                "TXT_PROTONE_ENQUEUE" : "TXT_PROTONE_PLAY";

                            BuildMenuText(btn, text, string.Empty, OPMShortcut.CmdOutOfRange);

                            bool enable = false;
                            foreach (string path in opmShellList.SelectedPaths)
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

                    case ToolAction.ToolActionTaggingWizard:
                        BuildMenuText(btn, "TXT_TAGGINGWIZARD", string.Empty, OPMShortcut.CmdTaggingWizard);
                        break;

                    case ToolAction.ToolActionCdRipper:
                        BuildMenuText(btn, "TXT_CDRIPPERWIZARD", string.Empty, OPMShortcut.CmdCdRipperWizard);
                        break;

                    case ToolAction.ToolActionListDrives:
                        BuildMenuText(btn, "TXT_DRIVES", string.Empty, OPMShortcut.CmdChangeDisk);
                        break;

                    case ToolAction.ToolActionFavoritesAdd:
                        btn.Visible = true;
                        BuildMenuText(btn, "TXT_FAVORITES_ADD", string.Empty, OPMShortcut.CmdOutOfRange);
                        break;

                    case ToolAction.ToolActionFavoritesManage:
                        btn.Visible = true;
                        BuildMenuText(btn, "TXT_FAVORITES_MANAGE", string.Empty, OPMShortcut.CmdFavManager);
                        break;

                    case ToolAction.ToolActionFavoriteFolders:
                        btn.Visible = true;
                        BuildMenuText(btn, "TXT_FAVORITES", string.Empty, OPMShortcut.CmdOutOfRange);
                        break;
                }
            }
        }

        private bool IsToolActionEnabled(ToolAction action)
        {
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
        #endregion

        #region Drive bar / Favorites bar
        private void OnFavoriteChosen(object sender, EventArgs e)
        {
            if (sender as OPMToolStripMenuItem != null)
            {
                ChangePath((sender as OPMToolStripMenuItem).Text);
            }
        }

        private void OnDriveChosen(object sender, ToolStripItemClickedEventArgs e)
        {
            ChangePath(e.ClickedItem.Tag as String);
        }

        private void OnBuildDriveButtonMenu(object sender, EventArgs e)
        {
            try
            {
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

                tsbDrives.DropDownItems.Clear();
                ilDrives.Images.Clear();

                tsbDrives.DropDown.BackColor = ThemeManager.WndValidColor;

                foreach (System.IO.DriveInfo di in drives)
                {
                    DriveInfoItem dii = new DriveInfoItem(di);
                    OPMToolStripDropDownMenuItem tsi = new OPMToolStripDropDownMenuItem(tsbDrives);
                    tsi.ImageScaling = ToolStripItemImageScaling.None;

                    tsi.Text = dii.ToString();
                    tsi.Tag = dii.Path;
                    tsi.Image = dii.Image;

                    tsbDrives.DropDownItems.Add(tsi);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        private void BuildMenuText(ToolStripItem tsm, string tag, string param, OPMShortcut command)
        {
            tsm.ToolTipText =
                (tsm.Enabled && !string.IsNullOrEmpty(param)) ? Translator.Translate(tag) + ": " + param :
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
            }
        }

        private void OnBuildFavoritesMenu(object sender, EventArgs e)
        {
            try
            {
                ToolStripDropDownItem tsmi = sender as ToolStripDropDownItem;
                if (tsmi == null || tsmi.DropDownItems == null || tsmi.DropDownItems.Count < 2)
                    return;

                tsbFavorites.DropDown.BackColor = ThemeManager.WndValidColor;

                ilFavorites.Images.Clear();

                // Clear favorites items
                List<ToolStripItem> itemsToClear = new List<ToolStripItem>();
                foreach (ToolStripItem child in tsmi.DropDownItems)
                {
                    if ((child as ToolStripSeparator) != null || child.Tag != null)
                        continue;

                    itemsToClear.Add(child);
                }

                foreach (ToolStripItem itemToClear in itemsToClear)
                {
                    tsmi.DropDownItems.Remove(itemToClear);
                }

                List<string> favPaths = ProTONEConfig.GetFavoriteFolders("FavoriteFolders");
                if (favPaths != null && favPaths.Count > 0)
                {
                    foreach (string path in favPaths)
                    {
                        if (Directory.Exists(path))
                        {
                            OPMToolStripDropDownMenuItem tsi = new OPMToolStripDropDownMenuItem(tsbFavorites);
                            tsi.Text = path;
                            tsi.Click += new EventHandler(OnFavoriteChosen);
                            tsi.Image = ilFavorites.Images[GetIcon(path)];
                            tsi.ImageScaling = ToolStripItemImageScaling.None;

                            tsmi.DropDownItems.Add(tsi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private int GetIcon(string path)
        {
            System.Drawing.Image icon = ImageProvider.GetIcon(path, false);
            if (icon != null)
            {
                ilFavorites.Images.Add(icon);
                return ilFavorites.Images.Count - 1;
            }

            return 0;
        }

        protected override BaseCfgPanel GetBaseCfgPanel()
        {
            return new FileExplorerCfgPanel();
        }

        public override bool EditDisplayedPathAllowed
        {
            get
            {
                return true;
            }
        }

        public override void TryCommitNewPath(string newPath)
        {
            if (ValidatePath(newPath))
            {
                opmShellList.Path = newPath;
            }

        }

        private bool ValidatePath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    return true;

                if (path.StartsWith("\\\\"))
                {
                    ShareCollection shares = ShareCollection.GetShares(path);
                    return (shares != null && shares.Count > 0);
                }
            }
            catch { }

            return false;
        }

    }

    #region Tool actions
    internal enum ToolAction : int
    {
        ToolActionNothing = -1,

        ToolActionNewFolder = 0,

        ToolActionBack,
        ToolActionFwd,
        ToolActionUp,

        ToolActionFavoriteFolders,
        ToolActionFavoritesAdd,
        ToolActionFavoritesManage,

        ToolActionSearch,
        ToolActionReload,

        ToolActionCopy,
        ToolActionCut,
        ToolActionPaste,

        ToolActionRename,
        ToolActionDelete,

        ToolActionTaggingWizard,
        ToolActionCdRipper,

        ToolActionJumpToItem,

        ToolActionProTONEPlay,
        ToolActionProTONEEnqueue,

        ToolActionLaunch,

        ToolActionListDrives,
    }
    #endregion

    #region Wizard
    public static class SearchWizardMain
    {
        public static DialogResult Execute(BackgroundTask initTask)
        {
            Type[] pages = new Type[]
                {
                    typeof(WizFESearchStep1Ctl),
                };

            return WizardHostForm.CreateWizard("TXT_SEARCHWIZARD_FE", pages, true, initTask,
                OPMedia.UI.Properties.Resources.Search.ToIcon());
        }

        public static DialogResult Execute()
        {
            return Execute(null);
        }
    }

    public static class CdRipperWizardMain
    {
        public static DialogResult Execute(BackgroundTask initTask)
        {
            Type[] pages = new Type[]
                {
                    typeof(WizCdRipperStep1),
                    typeof(WizCdRipperStep2),
                };

            return WizardHostForm.CreateWizard("TXT_CDRIPPERWIZARD", pages, true, initTask,
                OPMedia.Core.Properties.Resources.CDA.ToIcon());
        }

        public static DialogResult Execute()
        {
            return Execute(null);
        }
    }
    #endregion
}


