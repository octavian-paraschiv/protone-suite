using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.Addons.ActionManagement;
using OPMedia.Runtime.Addons.AddonsBase.Navigation;
using OPMedia.Runtime.Addons.AddonsBase.Preview;
using OPMedia.Runtime.Addons.AddonsBase.Prop;
using OPMedia.Runtime.Addons.Configuration;
using OPMedia.Runtime.Addons.Controls;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI;
using OPMedia.UI.Controls;
using OPMedia.UI.HelpSupport;
using OPMedia.UI.Properties;
using OPMedia.UI.Themes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.Runtime.Addons
{
    public partial class AddonHostForm : MainFrame
    {
        public static bool networkConfig = false;
        string _launchPath = string.Empty;

        #region Reload methods

        public void ReloadNavigation(object target)
        {
            OnNavigationAction(null,
                new NavigationActionEventArgs(null, NavActionType.ActionReloadNavigation, null, target));
        }

        public void ReloadProperties()
        {
            OnNavigationAction(null,
                new NavigationActionEventArgs(null, NavActionType.ActionReloadProperties, null));
        }

        public void ReloadPreview()
        {
            OnNavigationAction(null,
                new NavigationActionEventArgs(null, NavActionType.ActionReloadPreview, null));
        }

        public void GlobalReload()
        {
            ReloadNavigation(null);
            ReloadProperties();
            ReloadPreview();
        }
        #endregion

        public AddonHostForm(string launchPath)
            : this()
        {
            _launchPath = launchPath;
        }

        private int VSplitterDistance
        {
            get { return pnlOpMedia.SplitterDistance; }
            set { pnlOpMedia.SplitterDistance = value; }
        }

        private int HSplitterDistance
        {
            get { return pnlLocalContent.SplitterDistance; }
            set { pnlLocalContent.SplitterDistance = value; }
        }

        protected void SetNavPanelOrientation(Orientation o)
        {
            if (o == Orientation.Vertical)
            {
                pnlOpMedia.Orientation = Orientation.Vertical;
                pnlLocalContent.Orientation = Orientation.Horizontal;
            }
            else
            {
                pnlOpMedia.Orientation = Orientation.Horizontal;
                pnlLocalContent.Orientation = Orientation.Vertical;
            }

            pnlNavContainer.Cursor = pnlPreview.Cursor = pnlProperties.Cursor = Cursors.Default;
        }

        protected AddonHostForm()
            : base("TXT_APP_NAME")
        {
            InitializeComponent();
            SetNavPanelOrientation(Orientation.Vertical);

            this.Shown += new EventHandler(MainForm_Load);
            //this.Load += new EventHandler(MainForm_Load);

            this.HandleDestroyed += new EventHandler(MainForm_HandleDestroyed);

            this.ShowInTaskbar = true;
            lblNoItems.Text = Translator.Translate("TXT_THEREARENOITEMS");
            lblNoProperties.Text = Translator.Translate("TXT_THEREARENOITEMS");
            lblNoPreview.Text = Translator.Translate("TXT_THEREARENOITEMS");

            this.Location = AppConfig.WindowLocation;
            this.Size = AppConfig.WindowSize;
            this.WindowState = AppConfig.WindowState;

            tsmiSettings.ShortcutKeyDisplayString =
                ShortcutMapper.GetShortcutString(OPMShortcut.CmdOpenSettings);

            tsmiSettings.Image = Resources.Settings.Transparent();

            SetColors();

            ExitEditPathMode(false);
        }

        protected override void OnThemeUpdatedInternal()
        {
            base.OnThemeUpdatedInternal();
            SetColors();
        }

        public void SetColors()
        {
            lblStatusMain.ForeColor = ThemeManager.ForeColor;
            statusBar.ForeColor = ThemeManager.ForeColor;
            lblStatusMain.BackColor = ThemeManager.BackColor;
            statusBar.BackColor = ThemeManager.BackColor;
            lblStatusBarSep.OverrideBackColor = ThemeManager.SeparatorColor;
        }

        [EventSink(EventNames.SetMainStatusBar)]
        public void OnSetMainStatusBar(string text, Image img)
        {
            if (text == null && img == null)
            {
                statusBar.Visible = false;
            }
            else
            {
                statusBar.Visible = true;
                lblStatusMain.Image = img;
                lblStatusMain.Text = text;
                lblStatusMain.Font = ThemeManager.NormalBoldFont;
            }
        }

        [EventSink(EventNames.PerformTranslation)]
        public void OnPerformTranslation()
        {
            lblNoItems.Text = Translator.Translate("TXT_THEREARENOITEMS");
            lblNoProperties.Text = Translator.Translate("TXT_THEREARENOITEMS");
            lblNoPreview.Text = Translator.Translate("TXT_THEREARENOITEMS");

            tsmiAbout.Text = Translator.Translate("TXT_ABOUT",
                Translator.Translate("TXT_APP_NAME"));

            tsmiFile.Text = Translator.Translate("TXT_FILE");
            tsmiSettings.Text = Translator.Translate("TXT_SETTINGS");
            tXTAPPHELPToolStripMenuItem.Text = Translator.Translate("TXT_APPHELP");
            tXTHELPToolStripMenuItem.Text = Translator.Translate("TXT_HELP");
            tsmiExit.Text = Translator.Translate("TXT_EXIT");

            foreach (OPMToolStripMenuItem tsmi in msMain.Items)
            {
                if (tsmi.Tag is NavigationAddon)
                {
                    NavigationAddon addon = (tsmi.Tag as NavigationAddon);

                    string name = Translator.Translate(addon.Name);
                    tsmi.Text = name.Replace("*", "");
                }
            }

            for (int j = 0; j < msMain.Items.Count; j++)
            {
                if (msMain.Items[j] != null &&
                    (msMain.Items[j].Tag is NavigationAddon) &&
                    (msMain.Items[j] as OPMToolStripMenuItem).Checked)
                {
                    SetTitle(string.Format("{0} - {1}",
                        Translator.Translate("TXT_APP_NAME"), msMain.Items[j].Text));
                    break;
                }
            }

            Translator.TranslateToolStrip(msMain, DesignMode);
        }

        [EventSink(EventNames.KeymapChanged)]
        public void OnKeymapChanged()
        {
            tsmiSettings.ShortcutKeyDisplayString =
                ShortcutMapper.GetShortcutString(OPMShortcut.CmdOpenSettings);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            tsmiAbout.ToolTipText = string.Empty;
            tsmiAbout.AutoToolTip = false;

            tsmiAbout.Text = Translator.Translate("TXT_ABOUT",
                Translator.Translate("TXT_APP_NAME"));

            // First chance ...
            if (!GetNavigationAddons())
            {
                if (AddonAppSettingsForm.Show("TXT_S_ADDONSETTINGS", string.Empty) == DialogResult.Cancel)
                {
                    EventDispatch.DispatchEvent(EventNames.ShowMessageBox,
                        Translator.Translate("TXT_NO_NAV_ADDONS"),
                        Translator.Translate("TXT_APP_NAME"),
                        MessageBoxIcon.Error);

                    Logger.LogWarning("No navigation addons are currently configured. Application is exiting.");

                    // Exit application by closing the main form.
                    Close();
                    return;
                }
            }

            this.VSplitterDistance = AddonAppConfig.VSplitterDistance;
            this.HSplitterDistance = AddonAppConfig.HSplitterDistance;
        }

        protected override bool IsShortcutAllowed(OPMShortcut cmd)
        {
            // Any command in range is valid.
            return (ShortcutMapper.CmdFirst <= cmd && cmd <= ShortcutMapper.CmdLast);
        }

        public override void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (args.Handled)
                return;

            AddonAppSettingsForm.InternetConfig = networkConfig;

            switch (args.cmd)
            {
                case OPMShortcut.CmdOpenSettings:
                    if (ApplicationInfo.IsSuiteApplication == false)
                    {
                        AddonAppSettingsForm.Show();
                        args.Handled = true;
                    }
                    break;

                case OPMShortcut.CmdCfgKeyboard:
                    AddonAppSettingsForm.Show("TXT_S_CONTROL", "TXT_S_KEYMAP");
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdSwitchWindows:
                    SelectNextAddon();
                    Focus();
                    args.Handled = true;
                    break;
            }
        }

        void MainForm_HandleDestroyed(object sender, EventArgs e)
        {
            AddonAppConfig.VSplitterDistance = this.VSplitterDistance;
            AddonAppConfig.HSplitterDistance = this.HSplitterDistance;

        }

        /// <summary>
        /// Gets all the navigation addons and displays them in the
        /// navigation bar.
        /// </summary>
        private bool GetNavigationAddons()
        {
            Logger.LogTrace("MainForm.GetNavigationAddons called ...");

            AddonsCore.Instance.PreviewEnded += new PreviewEndedEventHandler(Instance_PreviewEnded);

            if (AddonsCore.Instance.NavAddonsLoader == null ||
                AddonsCore.Instance.NavAddonsLoader.Addons == null ||
                AddonsCore.Instance.NavAddonsLoader.Addons.Count < 1)
                return false;

            IDictionaryEnumerator enumerator =
               AddonsCore.Instance.NavAddonsLoader.Addons.GetEnumerator();

            Translator.TranslateToolStrip(msMain, DesignMode);

            OPMAddonMenuItem catalogButton = null;
            OPMAddonMenuItem previousAddonButton = null;

            while (enumerator.MoveNext())
            {
                NavigationAddon addon = enumerator.Value as NavigationAddon;

                if (addon != null)
                {
                    string name = Translator.Translate(addon.Name);
                    OPMAddonMenuItem button = new OPMAddonMenuItem(name.Replace("*", ""));

                    button.Name = addon.AddonTypeName;
                    button.Click += new EventHandler(AddonButtonClick);
                    //button.AutoSize = true;

                    NavBaseCtl panel = addon.AddonPanel;
                    button.Image = panel.SmallAddonImage;
                    button.Tag = addon;
                    button.ImageTransparentColor = Color.Magenta;

                    msMain.Items.Add(button);

                    if (addon.AddonTypeName == AddonAppConfig.LastNavAddon)
                    {
                        previousAddonButton = button;
                    }
                    if (addon.AddonTypeName.ToLowerInvariant().Contains("catalog"))
                    {
                        catalogButton = button;
                    }
                }
            }

            msMain.ResumeLayout();

            if (!string.IsNullOrEmpty(_launchPath))
            {
                // Need to figure out which navigation addon must be used to open this file
                if (msMain.Items.Count >= 1)
                {
                    string ext = PathUtils.GetExtension(_launchPath);

                    foreach (OPMToolStripMenuItem button in msMain.Items)
                    {
                        NavigationAddon addon = button.Tag as NavigationAddon;
                        if (addon != null &&
                            addon.AddonPanel != null &&
                            addon.AddonPanel.HandledFileTypes != null &&
                            addon.AddonPanel.HandledFileTypes.Count > 0 &&
                            addon.AddonPanel.HandledFileTypes.Contains(ext))
                        {
                            AddonButtonClick(button, null);
                            return true;
                        }
                    }

                    ErrorDispatcher.DispatchError("ProTONE Media Library cannot handle this file: " + _launchPath, false);
                    Process.GetCurrentProcess().Kill();
                    return true;
                }
            }

            if (previousAddonButton != null)
            {
                AddonButtonClick(previousAddonButton, null);
            }
            else if (msMain.Items.Count >= 1)
            {
                foreach (OPMToolStripMenuItem button in msMain.Items)
                {
                    if (button is OPMAddonMenuItem && button.Tag is NavigationAddon)
                    {
                        AddonButtonClick(button, null);
                        break;
                    }
                }
            }

            Logger.LogTrace("MainForm.GetNavigationAddons done.");

            return true;
        }

        void Instance_PreviewEnded()
        {
            Logger.LogTrace("Preview has terminated.");
            pnlPreview.Controls.Clear();
            pnlPreview.Controls.Add(lblNoPreview);
        }

        private void SelectNextAddon()
        {
            int i = -1;

            List<OPMToolStripMenuItem> buttons = new List<OPMToolStripMenuItem>();
            for (int j = 0; j < msMain.Items.Count; j++)
            {
                if (msMain.Items[j] != null && (msMain.Items[j].Tag is NavigationAddon))
                    buttons.Add(msMain.Items[j] as OPMToolStripMenuItem);
            }

            if (buttons.Count > 1)
            {
                for (int j = 0; j < buttons.Count; j++)
                {
                    if (buttons[j].Checked)
                    {
                        i = j;
                        break;
                    }
                }

                i = (i + 1) % (buttons.Count);

                if (buttons[i] != null)
                {
                    AddonButtonClick(buttons[i], null);
                }
            }
        }

        /// <summary>
        /// Handles the Click event for the buttons on the navigation bar.
        /// </summary>
        string selectedAddonTitle = string.Empty;
        private void AddonButtonClick(object sender, EventArgs e)
        {
            Logger.LogTrace("MainForm.AddonButtonClick called ...");

            NavigationAddon addon = null;

            OPMAddonMenuItem button = sender as OPMAddonMenuItem;
            if (button != null)
            {
                addon = button.Tag as NavigationAddon;
                if (addon == null)
                    return;

                if (!string.IsNullOrEmpty(_launchPath))
                {
                    addon.AddonPanel.Tag = _launchPath;
                    _launchPath = string.Empty;
                }

                if (addon != null)
                {
                    pnlNavContainer.SuspendLayout();

                    addon.AddonPanel.Dock = DockStyle.Fill;
                    addon.AddonPanel.Visible = true;
                    addon.AddonPanel.PanelTitle = button.Text.Replace("\r", "").Replace("\n", "");

                    addon.AddonPanel.NavigationAction -=
                        new NavBaseCtl.NavigationActionEventHandler(OnNavigationAction);
                    addon.AddonPanel.NavigationAction +=
                        new NavBaseCtl.NavigationActionEventHandler(OnNavigationAction);

                    pnlNavContainer.Controls.Clear();
                    pnlNavContainer.Controls.Add(addon.AddonPanel);

                    pnlNavContainer.ResumeLayout();

                    foreach (OPMToolStripMenuItem tsButton in msMain.Items)
                    {
                        if (tsButton is OPMAddonMenuItem)
                        {
                            if (button.Tag != tsButton.Tag)
                            {
                                NavigationAddon na = tsButton.Tag as NavigationAddon;
                                if (na != null && na.AddonPanel != null)
                                {
                                    na.AddonPanel.OnActiveStateChanged(false);
                                }

                                tsButton.Checked = false;
                            }
                        }
                    }

                    addon.AddonPanel.OnActiveStateChanged(true);

                    ThemeManager.SetDoubleBuffer(addon.AddonPanel);

                    //button.Image = null;
                    button.Checked = true;
                    //button.BackColor = Color.FromArgb(150, ThemeManager.GradientNormalColor2);
                    AddonAppConfig.LastNavAddon = button.Name;

                    SetTitle(string.Format("{0} - {1}",
                        Translator.Translate("TXT_APP_NAME"), button.Text));

                }
            }

            Logger.LogTrace("MainForm.AddonButtonClick done.");
        }

        bool prepareAutoPreview = false;

        /// <summary>
        /// Handles the NavigationAction event of the navigation addon.
        /// </summary>
        void OnNavigationAction(object sender, NavigationActionEventArgs args)
        {
            try
            {
                Logger.LogTrace("MainForm handling OnNavigationAction ...");

                Application.UseWaitCursor = true;
                this.SuspendLayoutEx();

                bool isVoidAction = false;
                bool isPreviewAction = false;
                ActionRequest request = new ActionRequest();
                ActionResponse response = null;

                Logger.LogTrace("Action is of type: " + args.ActionType.ToString());

                pnlPreview.SuspendLayoutEx();
                pnlProperties.SuspendLayoutEx();

                lblNoPreview.Text = Translator.Translate("TXT_THEREARENOITEMS");

                prepareAutoPreview =
                    (args.ActionType == NavActionType.ActionPrepareAutoPreview &&
                     args.ActionType != NavActionType.ActionCancelAutoPreview);

                switch (args.ActionType)
                {
                    case NavActionType.ActionSaveProperties:
                        // It is a save property request.
                        request.ActionType = ActionManagement.ActionType.ActionSaveProperties;
                        request.Items = args.Paths; // don't matter any way
                        break;

                    case NavActionType.ActionSelectDirectory:
                    case NavActionType.ActionSelectFile:
                    case NavActionType.ActionSelectMultipleItems:
                        // It is a property request.
                        request.ActionType = ActionManagement.ActionType.ActionBeginEdit;
                        request.Items = args.Paths;
                        break;

                    case NavActionType.ActionDoubleClickFile:
                        // It is a regular preview request.
                        request.ActionType = ActionManagement.ActionType.ActionBeginPreview;
                        request.Items = args.Paths;
                        isPreviewAction = true;
                        break;

                    case NavActionType.ActionNotifyNonPreviewableItem:
                        lblNoPreview.Text = Translator.Translate("TXT_NOTIFY_NON_PREVIEW");
                        isPreviewAction = true;
                        break;

                    case NavActionType.ActionNotifyPreviewableItem:
                        lblNoPreview.Text = Translator.Translate("TXT_NOTIFY_PREVIEW");
                        isPreviewAction = true;
                        break;

                    case NavActionType.ActionPrepareAutoPreview:
                        lblNoPreview.Text = Translator.Translate("TXT_PREPARE_PREVIEW");
                        prepareAutoPreview = true;
                        isPreviewAction = true;
                        break;

                    case NavActionType.ActionCancelAutoPreview:
                        if (args.AdditionalData == null && prepareAutoPreview)
                        {
                            lblNoPreview.Text = Translator.Translate("TXT_PREVIEW_CANCELED");
                        }
                        else
                        {
                            lblNoPreview.Text = Translator.Translate("TXT_NOTIFY_PREVIEW");
                        }
                        isPreviewAction = true;
                        break;

                    case NavActionType.ActionReloadNavigation:
                        {
                            // Don't reselct addons, send command to the ones already selected.
                            isVoidAction = true;

                            NavBaseCtl ctl = GetNavigationControl();
                            if (ctl != null)
                            {
                                ctl.Reload(args.AdditionalData);
                            }
                        }
                        break;

                    case NavActionType.ActionReloadPreview:
                        {
                            // Don't reselct addons, send command to the ones already selected.
                            isVoidAction = true;

                            PreviewBaseCtl ctl = GetPreviewControl();
                            if (ctl != null)
                            {
                                ctl.Reload(args.AdditionalData);
                            }
                        }
                        break;

                    case NavActionType.ActionReloadProperties:
                        {
                            // Don't reselct addons, send command to the ones already selected.
                            isVoidAction = true;

                            PropBaseCtl ctl = GetPropertiesControl();
                            if (ctl != null)
                            {
                                ctl.Reload(args.AdditionalData);
                            }
                        }
                        break;

                    case NavActionType.ActionDoubleClickDirectory:
                    default:
                        // No actions currently taken in this situation.
                        isVoidAction = true;
                        break;
                }

                if (!isVoidAction)
                {
                    response = AddonsCore.Instance.DispatchAction(request);
                    bool failed = ActionResponse.IsFailedAction(response);

                    if (isPreviewAction)
                    {
                        PreviewBaseCtl previewCtl = GetPreviewControl();
                        if (previewCtl != null)
                        {
                            previewCtl.EndPreview();
                            previewCtl.NavigationAction -=
                                new BaseAddonCtl.NavigationActionEventHandler(OnNavigationAction);
                        }

                        if (failed)
                        {
                            Logger.LogTrace("Preview action has failed.");
                            if (!pnlPreview.Controls.Contains(lblNoPreview))
                            {
                                pnlPreview.Controls.Clear();
                                pnlPreview.Controls.Add(lblNoPreview);
                            }
                        }
                        else
                        {
                            // Note that only a single item can be previewed.
                            PreviewAddon addon = response.TargetAddon as PreviewAddon;
                            if (!pnlPreview.Controls.Contains(addon.AddonPanel))
                            {
                                pnlPreview.Controls.Clear();
                                addon.AddonPanel.Dock = DockStyle.Fill;
                                addon.AddonPanel.Location = new System.Drawing.Point(10, 10);
                                addon.AddonPanel.TabIndex = 0;
                                pnlPreview.Controls.Add(addon.AddonPanel);
                            }

                            previewCtl = GetPreviewControl();
                            if (previewCtl != null)
                            {
                                previewCtl.NavigationAction +=
                                    new BaseAddonCtl.NavigationActionEventHandler(OnNavigationAction);

                                previewCtl.BeginPreview(args.Paths[0], args.AdditionalData);
                            }
                        }
                    }
                    else
                    {
                        PropBaseCtl propCtl = GetPropertiesControl();
                        if (propCtl != null)
                        {
                            propCtl.NavigationAction -=
                                new BaseAddonCtl.NavigationActionEventHandler(OnNavigationAction);
                        }

                        if (failed)
                        {
                            Logger.LogTrace("View Properties Action has failed.");
                            pnlProperties.Controls.Clear();
                            pnlProperties.Controls.Add(lblNoProperties);
                        }
                        else
                        {
                            PropertyAddon addon = response.TargetAddon as PropertyAddon;
                            if (!pnlProperties.Controls.Contains(addon.AddonPanel))
                            {
                                pnlProperties.Controls.Clear();
                                addon.AddonPanel.Dock = DockStyle.Fill;
                                pnlProperties.Controls.Add(addon.AddonPanel);
                            }

                            propCtl = GetPropertiesControl();
                            if (propCtl != null)
                            {
                                propCtl.NavigationAction +=
                                    new BaseAddonCtl.NavigationActionEventHandler(OnNavigationAction);

                                propCtl.BeginEdit(args.Paths, args.AdditionalData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
            finally
            {
                pnlPreview.ResumeLayoutEx();
                pnlProperties.ResumeLayoutEx();
                this.ResumeLayoutEx();
                Application.UseWaitCursor = false;

                Logger.LogTrace("MainForm handled OnNavigationAction.");
            }
        }

        private PreviewBaseCtl GetPreviewControl()
        {
            if (pnlPreview.Controls.Count > 0)
            {
                return pnlPreview.Controls[0] as PreviewBaseCtl;
            }

            return null;
        }

        private PropBaseCtl GetPropertiesControl()
        {
            if (pnlProperties.Controls.Count > 0)
            {
                return pnlProperties.Controls[0] as PropBaseCtl;
            }

            return null;
        }

        private NavBaseCtl GetNavigationControl()
        {
            if (pnlNavContainer.Controls.Count > 0)
            {
                return pnlNavContainer.Controls[0] as NavBaseCtl;
            }

            return null;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShortcutMapper.DispatchCommand(OPMShortcut.CmdOpenSettings);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tXTABOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageDisplay.ShowAboutBox();
        }

        private void tXTAPPHELPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShortcutMapper.DispatchCommand(OPMShortcut.CmdOpenHelp);
        }

        public override void FireHelpRequest()
        {
            if (this.IsActive)
            {
                string sectionName = AddonAppConfig.LastNavAddon.Replace(".", "");
                string topicName = "";

                BaseAddonCtl addon = null;
                try
                {
                    if (pnlNavContainer.ContainsFocus)
                    {
                        addon = pnlNavContainer.Controls[0] as BaseAddonCtl;
                    }
                    else if (pnlProperties.ContainsFocus)
                    {
                        addon = pnlProperties.Controls[0] as BaseAddonCtl;
                    }
                    else if (pnlPreview.ContainsFocus)
                    {
                        addon = pnlPreview.Controls[0] as BaseAddonCtl;
                    }
                }
                catch { }

                if (addon != null)
                {
                    topicName = addon.GetHelpTopic();
                }

                HelpTarget.HelpRequest(sectionName, topicName);
            }
            else
            {
                base.FireHelpRequest();
            }
        }

        protected void EnterEditPathMode()
        {
            NavBaseCtl ctl = GetNavigationControl();
            if (ctl != null && ctl.EditDisplayedPathAllowed)
            {
                lblStatusMain.Visible = false;
                txtStatusMain.Visible = true;
                txtStatusMain.Width = this.Width - 5;

                string realPath = lblStatusMain.Text.Replace(Translator.Translate("TXT_CURRENT_PATH", string.Empty), string.Empty);
                txtStatusMain.Text = realPath;

                txtStatusMain.Select();
                txtStatusMain.Focus();
            }
        }

        protected void ExitEditPathMode(bool commitPath)
        {
            lblStatusMain.Visible = true;
            txtStatusMain.Visible = false;

            Application.DoEvents();

            if (commitPath)
            {
                string previousPath = lblStatusMain.Text.Replace(Translator.Translate("TXT_CURRENT_PATH", string.Empty), string.Empty);
                string newPath = txtStatusMain.Text;

                lblStatusMain.Text = "Looking up " + newPath;
                Application.DoEvents();

                if (previousPath != newPath)
                {
                    TryCommitNewPath(newPath);
                }
            }
        }

        private void TryCommitNewPath(string newPath)
        {
            NavBaseCtl ctl = GetNavigationControl();
            if (ctl != null && ctl.EditDisplayedPathAllowed)
            {
                ctl.TryCommitNewPath(newPath);
            }
        }

        private void statusBar_Click(object sender, EventArgs e)
        {
            ExitEditPathMode(false);
        }

        private void lblStatusMain_Click(object sender, EventArgs e)
        {
            EnterEditPathMode();
        }

        private void txtStatusMain_Leave(object sender, EventArgs e)
        {
            ExitEditPathMode(false);
        }

        void txtStatusMain_LostFocus(object sender, System.EventArgs e)
        {
            ExitEditPathMode(false);
        }

        void txtStatusMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        ExitEditPathMode(true);
                        break;

                    case Keys.Escape:
                        ExitEditPathMode(false);
                        break;
                }
            }
        }

    }

}