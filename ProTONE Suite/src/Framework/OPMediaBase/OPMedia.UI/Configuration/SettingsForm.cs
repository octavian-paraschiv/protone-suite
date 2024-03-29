using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.HelpSupport;
using OPMedia.UI.Properties;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI
{
    public partial class SettingsForm : ToolForm
    {
        public static bool InternetConfig = false;

        protected static bool _restart = false;

        protected BaseCfgPanel selectedPanel = null;

        private string _titleToOpen = "";
        private string _subTitleToOpen = "";

        public new static DialogResult Show()
        {
            SettingsForm _instance = new SettingsForm();
            return _instance.ShowDialog();
        }

        public static void RequestRestart()
        {
            _restart = true;
        }

        protected SettingsForm(string titleToOpen, string subtitleToOpen)
            : this()
        {
            _titleToOpen = Translator.Translate(titleToOpen);
            _subTitleToOpen = Translator.Translate(subtitleToOpen);
        }

        public SettingsForm() : base("TXT_CONFIGUREAPP")
        {
            InitializeComponent();

            tabOptions.ImageList = new ImageList();
            tabOptions.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            tabOptions.ImageList.ImageSize = new Size(32, 32);
            tabOptions.ImageList.TransparentColor = Color.Magenta;

            this.InheritAppIcon = false;
            this.Icon = Resources.Settings.ToIcon();
            this.FormClosing += new FormClosingEventHandler(SettingsForm_FormClosing);
            this.HandleDestroyed += new EventHandler(SettingsForm_HandleDestroyed);
            this.Load += new EventHandler(SettingsForm_Load);
        }

        Timer _delayedPanelSelector;

        void SettingsForm_Load(object sender, EventArgs e)
        {
            tabOptions.TabPages.Clear();

            AddPanel(typeof(GeneralSettingsPanel));
            AddAditionalPanels();
            AddPanel(typeof(InternetSettingsPanel), RequiresInternetConfig());
            AddPanel(typeof(TroubleshootingPanel));

            RemoveUnneededPanels();

            _delayedPanelSelector = new Timer();
            _delayedPanelSelector.Interval = 300;
            _delayedPanelSelector.Tick += (ss, ee) =>
            {
                _delayedPanelSelector.Stop();
                SelectTitleToOpen();
            };
            _delayedPanelSelector.Start();
        }

        private void SelectTitleToOpen()
        {
            foreach (TabPage tp in tabOptions.TabPages)
            {
                if (tp.Text == _titleToOpen)
                {
                    ShowPanel(tp.Controls[0] as BaseCfgPanel, _subTitleToOpen);
                    break;
                }
            }
        }

        public virtual void RemoveUnneededPanels()
        {
        }

        public virtual void AddAditionalPanels()
        {
        }

        public virtual bool RequiresInternetConfig()
        {
            return InternetConfig;
        }


        void SettingsForm_HandleDestroyed(object sender, EventArgs e)
        {
            if (_restart)
            {
                LoggedApplication.Restart();
            }
        }

        bool _closedOnce = false;
        void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_closedOnce)
                {
                    foreach (TabPage tp in tabOptions.TabPages)
                    {
                        BaseCfgPanel panel = tp.Controls[0] as BaseCfgPanel;
                        if (panel != null)
                        {
                            if (DialogResult == DialogResult.OK)
                            {
                                panel.Save();
                            }
                            else
                            {
                                panel.Discard();
                            }
                        }
                    }

                    _closedOnce = true;
                    return;
                }
            }
            catch (SettingsSaveException ex)
            {
                ErrorDispatcher.DispatchError(ex.Message, false);
            }

            e.Cancel = true;
        }

        protected void RemovePanel(Type panelType)
        {
            List<TabPage> pagesToRemove = new List<TabPage>();

            foreach (TabPage tp in tabOptions.TabPages)
            {
                Control ctl = tp.Controls[0];
                if (ctl.GetType() == panelType)
                {
                    pagesToRemove.Add(tp);
                }
            }

            foreach (TabPage tp in pagesToRemove)
            {
                tabOptions.TabPages.Remove(tp);
            }
        }

        protected void KeepPanels(List<Type> panelsToKeep)
        {
            List<TabPage> pagesToRemove = new List<TabPage>();

            foreach (TabPage tp in tabOptions.TabPages)
            {
                Control ctl = tp.Controls[0];
                if (!panelsToKeep.Contains(ctl.GetType()))
                {
                    pagesToRemove.Add(tp);
                }
            }

            foreach (TabPage tp in pagesToRemove)
            {
                tabOptions.TabPages.Remove(tp);
            }
        }

        protected void AddPanel(Type panelType)
        {
            AddPanel(panelType, true);
        }

        protected void AddPanel(Type panelType, bool condition)
        {
            AddPanel(panelType, condition, false);
        }

        protected void AddPanel(Type panelType, bool condition, bool alignRight)
        {
            if (condition)
            {
                BaseCfgPanel panel = null;

                try
                {
                    panel = Activator.CreateInstance(panelType) as BaseCfgPanel;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

                if (panel != null)
                {
                    if (AddPanel(panel, condition, alignRight))
                    {
                        if (panel is MultiPageCfgPanel)
                        {
                            List<BaseCfgPanel> subPagesToAdd = new List<BaseCfgPanel>();

                            if (panel is TroubleshootingPanel)
                            {
                                List<BaseCfgPanel> pages = GetTroubleshootingSubPages();
                                if (pages != null)
                                {
                                    subPagesToAdd.AddRange(pages);
                                }
                            }
                            else if (panel is ControlAppPanel)
                            {
                                subPagesToAdd.Add(new KeyMapCfgPanel());
                                List<BaseCfgPanel> pages = GetControlSubPages();
                                if (pages != null)
                                {
                                    subPagesToAdd.AddRange(pages);
                                }
                            }
                            else if (panel is InternetSettingsPanel)
                            {
                                subPagesToAdd.Add(new NetworkCfgPanel());
                                List<BaseCfgPanel> pages = GetInternetSubPages();
                                if (pages != null)
                                {
                                    subPagesToAdd.AddRange(pages);
                                }
                            }

                            foreach (var page in subPagesToAdd)
                            {
                                (panel as MultiPageCfgPanel).AddSubPage(page);
                            }
                        }
                    }
                }
            }
        }

        protected void AddPanel(BaseCfgPanel panel, bool condition)
        {
            AddPanel(panel, condition, false);
        }

        protected bool AddPanel(BaseCfgPanel panel, bool condition, bool alignRight)
        {
            if (condition)
            {
                if (panel != null)
                {
                    string title = Translator.Translate(panel.Title);
                    if (!tabOptions.TabPages.ContainsKey(title))
                    {
                        panel.Dock = DockStyle.Fill;
                        OPMTabPage tp = new OPMTabPage(title, panel);
                        tp.ImageIndex = tabOptions.ImageList.Images.Count;
                        tabOptions.ImageList.Images.Add(panel.Image);

                        tabOptions.TabPages.Add(tp);
                        return true;
                    }
                }
            }

            return false;
        }

        private void ShowPanel(BaseCfgPanel panel, string subTitleToOpen = "")
        {
            if (selectedPanel != panel)
            {
                foreach (TabPage tp in tabOptions.TabPages)
                {
                    BaseCfgPanel crtPanel = tp.Controls[0] as BaseCfgPanel;
                    if (panel == crtPanel)
                    {
                        tabOptions.SelectedTab = tp;

                        IMultiPageCfgPanel multiPagePanel = panel as IMultiPageCfgPanel;
                        if (multiPagePanel != null && string.IsNullOrEmpty(subTitleToOpen) == false)
                            multiPagePanel.SelectSubPage(subTitleToOpen);

                        selectedPanel = panel;

                        break;
                    }
                }
            }
        }

        public override void FireHelpRequest()
        {
            BaseCfgPanel panel = null;
            try
            {
                panel = tabOptions.SelectedTab.Controls[0] as BaseCfgPanel;
            }
            catch
            {
                panel = null;
            }

            if (panel != null)
                HelpTarget.HelpRequest(this.Name, panel.GetHelpTopic());
            else
                base.FireHelpRequest();
        }

        public virtual List<BaseCfgPanel> GetTroubleshootingSubPages()
        {
            return null;
        }

        public virtual List<BaseCfgPanel> GetControlSubPages()
        {
            return null;
        }


        public virtual List<BaseCfgPanel> GetInternetSubPages()
        {
            return null;
        }

    }

    public class SettingsSaveException : Exception
    {
        public SettingsSaveException(string msg)
            : base(msg)
        {
        }
    }
}