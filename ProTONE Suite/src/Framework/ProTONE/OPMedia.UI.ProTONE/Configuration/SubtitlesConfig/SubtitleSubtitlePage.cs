﻿using Iso639;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.Configuration
{
    public partial class SubtitleSubtitlePage : BaseCfgPanel
    {
        bool _subtitleDownloadEnabled = ProTONEConfig.SubtitleDownloadEnabled;

        OPMComboBox _cmbEditServerType = new OPMComboBox();
        YesNoComboBox _cmbEditActive = new YesNoComboBox();
        TextBox _txtEditServer = new TextBox();
        TextBox _txtEditUser = new TextBox();
        TextBox _txtEditPswd = new TextBox();

        readonly int[] widths = new int[] { 0, 80, 100, 45, 50, 50 };

        private string _subtitleDownloadURIs = string.Empty;

        private ToolTip _tip = new ToolTip();

        protected override void SaveInternal()
        {
            ProTONEConfig.SubtitleDownloadEnabled = _subtitleDownloadEnabled;
            ProTONEConfig.SubtitleDownloadURIs = _subtitleDownloadURIs;

            if (cmbLanguages.SelectedItem is SubtitleLanguage lang)
                ProTONEConfig.PrefferedSubtitleLang = lang.LCID;

            ProTONEConfig.SubtitleMinimumMovieDuration = (int)nudMinMovieDuration.Value;
            ProTONEConfig.SubDownloadedNotificationsEnabled = chkNotifySubDownloaded.Checked;
        }

        public SubtitleSubtitlePage()
        {
            InitializeComponent();

            _cmbEditServerType.Items.Add(SubtitleServerType.OpenSubtitles);
            _cmbEditServerType.Items.Add(SubtitleServerType.BSP_V1);
            _cmbEditServerType.Items.Add(SubtitleServerType.NuSoap);
            _cmbEditServerType.DropDownHeight = 30;

            btnAdd.Image = OPMedia.UI.Properties.Resources.Add;
            btnDelete.Image = OPMedia.UI.Properties.Resources.Del;
            btnMoveUp.Image = OPMedia.UI.Properties.Resources.Up16;
            btnMoveDown.Image = OPMedia.UI.Properties.Resources.Down16;

            _tip.SetToolTip(btnAdd, Translator.Translate("TXT_ADD"));
            _tip.SetToolTip(btnDelete, Translator.Translate("TXT_DELETE"));
            _tip.SetToolTip(btnMoveUp, Translator.Translate("TXT_MOVEUP"));
            _tip.SetToolTip(btnMoveDown, Translator.Translate("TXT_MOVEDOWN"));

            btnDelete.Visible = false;

            PopulateLanguages();

            ThemeManager.SetFont(lvDownloadAddresses, FontSizes.Small);
            lblMinDuration.Text = Translator.Translate("TXT_MINMOVIEDURATION");

            this.HandleCreated += new EventHandler(OnLoad);

            lvDownloadAddresses.Resize += new EventHandler(lvDownloadAddresses_Resize);
            lvDownloadAddresses.SubItemEdited += new OPMListView.EditableListViewEventHandler(OnListEdited);
            lvDownloadAddresses.ColumnWidthChanging += new ColumnWidthChangingEventHandler(lvDownloadAddresses_ColumnWidthChanging);

            lvDownloadAddresses.RegisterEditControl(_txtEditServer);
            lvDownloadAddresses.RegisterEditControl(_cmbEditServerType);
            lvDownloadAddresses.RegisterEditControl(_cmbEditActive);
            lvDownloadAddresses.RegisterEditControl(_txtEditUser);
            lvDownloadAddresses.RegisterEditControl(_txtEditPswd);

        }

        void lvDownloadAddresses_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
        }

        void lvDownloadAddresses_Resize(object sender, EventArgs e)
        {
            colEmpty.Width = 0;
            colServerType.Width = 80;
            colServerUrl.Width = 250;
            colActive.Width = 60;

            int w = lvDownloadAddresses.EffectiveWidth - 15 -
                (colServerType.Width + colServerUrl.Width + colActive.Width);

            colUserName.Width = colPassword.Width = w / 2;

        }

        void OnLoad(object sender, EventArgs e)
        {
            nudMinMovieDuration.Value = ProTONEConfig.SubtitleMinimumMovieDuration;
            nudMinMovieDuration.ValueChanged += new EventHandler(nudMinMovieDuration_ValueChanged);

            chkSubtitleDownload.Checked = _subtitleDownloadEnabled;
            chkSubtitleDownload.CheckedChanged += new EventHandler(chkSubtitleDownload_CheckedChanged);

            _subtitleDownloadURIs = ProTONEConfig.SubtitleDownloadURIs;

            BuildListFromSubtitleDownloadURIs();

            ThemeManager.SetFont(lblClickHint, FontSizes.Small);

            pnlOnlineSubtitles.Enabled = _subtitleDownloadEnabled;

            chkNotifySubDownloaded.Checked = ProTONEConfig.SubDownloadedNotificationsEnabled;
            this.chkNotifySubDownloaded.CheckedChanged += new System.EventHandler(this.chkNotifySubDownloaded_CheckedChanged);

            this.cmbLanguages.SelectedIndexChanged += new System.EventHandler(this.cmbLanguages_SelectedIndexChanged);
        }

        private void OnAdd(object sender, EventArgs e)
        {
            string[] data = new string[]
            { string.Empty, "Osdb", "[ URL ]", Translator.Translate("TXT_NO"), string.Empty, string.Empty };

            lvDownloadAddresses.EndEditing(false);

            int i = 0;

            ListViewItem item = new ListViewItem(data[i++]);

            OPMListViewSubItem subItem = new OPMListViewSubItem(_cmbEditServerType, item, data[i++]);
            subItem.ReadOnly = false;
            item.SubItems.Add(subItem);

            subItem = new OPMListViewSubItem(_txtEditServer, item, data[i++]);
            subItem.ReadOnly = false;
            item.SubItems.Add(subItem);

            subItem = new OPMListViewSubItem(_cmbEditActive, item, data[i++]);
            subItem.ReadOnly = false;
            item.SubItems.Add(subItem);

            subItem = new OPMListViewSubItem(_txtEditServer, item, data[i++]);
            subItem.ReadOnly = false;
            item.SubItems.Add(subItem);

            subItem = new OPMListViewSubItem(_txtEditUser, item, data[i++]);
            subItem.ReadOnly = false;
            item.SubItems.Add(subItem);

            lvDownloadAddresses.Items.Add(item);

            item.Selected = true;

            BuildSubtitleDownloadURIsFromList();
            Modified = true;
        }

        private void OnDelete(object sender, EventArgs e)
        {
            lvDownloadAddresses.EndEditing(false);

            if (lvDownloadAddresses.SelectedItems != null &&
                lvDownloadAddresses.SelectedItems.Count > 0)
            {
                int selItem = lvDownloadAddresses.SelectedItems[0].Index;
                lvDownloadAddresses.Items.RemoveAt(selItem);

                if (lvDownloadAddresses.SelectedItems.Count > 0)
                {
                    selItem = Math.Min(selItem, lvDownloadAddresses.Items.Count - 1);
                    lvDownloadAddresses.Items[selItem].Selected = true;
                }

                BuildSubtitleDownloadURIsFromList();
                Modified = true;
            }
        }

        private void OnMoveUp(object sender, EventArgs e)
        {
            if (lvDownloadAddresses.SelectedItems != null &&
                lvDownloadAddresses.SelectedItems.Count > 0)
            {
                int x = lvDownloadAddresses.SelectedItems[0].Index;
                if (x > 0)
                {
                    if (SwapItems(x, x - 1))
                    {
                        lvDownloadAddresses.Items[x - 1].Selected = true;

                        Modified = true;
                    }
                }
            }
        }

        private void OnMoveDown(object sender, EventArgs e)
        {
            if (lvDownloadAddresses.SelectedItems != null &&
                lvDownloadAddresses.SelectedItems.Count > 0)
            {
                int x = lvDownloadAddresses.SelectedItems[0].Index;
                if (x < lvDownloadAddresses.Items.Count - 1)
                {
                    if (SwapItems(x, x + 1))
                    {
                        lvDownloadAddresses.Items[x + 1].Selected = true;

                        Modified = true;
                    }
                }
            }
        }

        private void cmbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void OnEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Modified = true;
        }

        private void chkSubtitleDownload_CheckedChanged(object sender, EventArgs e)
        {
            Modified = true;
            _subtitleDownloadEnabled = chkSubtitleDownload.Checked;
            pnlOnlineSubtitles.Enabled = _subtitleDownloadEnabled;
        }

        private void nudMinMovieDuration_ValueChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        void OnListEdited(object sender, ListViewSubItemEventArgs args)
        {
            BuildSubtitleDownloadURIsFromList();
            Modified = true;
        }

        private bool SwapItems(int item1, int item2)
        {
            try
            {
                lvDownloadAddresses.EndEditing(false);

                if (item1 >= 0 && item2 >= 0 &&
                    item1 != item2 && item1 < lvDownloadAddresses.Items.Count &&
                    item2 < lvDownloadAddresses.Items.Count)
                {
                    ListViewItem lvi1 = lvDownloadAddresses.Items[item1].Clone() as ListViewItem;
                    ListViewItem lvi2 = lvDownloadAddresses.Items[item2].Clone() as ListViewItem;

                    if (lvi1 != null && lvi2 != null)
                    {
                        lvDownloadAddresses.Items[item1] = lvi2;
                        lvDownloadAddresses.Items[item2] = lvi1;

                        return true;
                    }
                }

                return false;
            }
            finally
            {
                BuildSubtitleDownloadURIsFromList();
                BuildListFromSubtitleDownloadURIs();
            }
        }

        private void PopulateLanguages()
        {
            var languages = Language.Database
                .Where(l => l.Culture.LCID != SubtitleLanguage.LOCALE_CUSTOM_UNSPECIFIED)
                .OrderBy(l => l.DisplayName())
                .Select(l => new SubtitleLanguage(l))
                .Prepend(new SubtitleLanguage(null))
                .ToArray();

            cmbLanguages.DataSource = languages;
            cmbLanguages.SelectedItem = languages.Where(l => l.LCID == ProTONEConfig.PrefferedSubtitleLang).FirstOrDefault();
            cmbLanguages.SelectedIndexChanged += new EventHandler(cmbLanguages_SelectedIndexChanged);
        }

        private void chkNotifySubDownloaded_CheckedChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void BuildSubtitleDownloadURIsFromList()
        {
            List<string> downloadURIs = new List<string>();
            foreach (ListViewItem row in lvDownloadAddresses.Items)
            {
                List<string> data = new List<string>();
                for (int i = 1; i < row.SubItems.Count; i++)
                {
                    ListViewItem.ListViewSubItem subItem = row.SubItems[i];

                    string str = subItem.Text;
                    if (i == colActive.Index)
                    {
                        if (str == Translator.Translate("TXT_YES"))
                        {
                            str = "1";
                        }
                        else
                        {
                            str = "0";
                        }
                    }

                    data.Add(str);
                }

                downloadURIs.Add(StringUtils.FromStringArray(data.ToArray(), ';'));
            }

            _subtitleDownloadURIs = StringUtils.FromStringArray(downloadURIs.ToArray(), '\\');
        }

        private void BuildListFromSubtitleDownloadURIs()
        {
            lvDownloadAddresses.Items.Clear();

            string[] subtitleDownloadURIs = StringUtils.ToStringArray(_subtitleDownloadURIs, '\\');
            if (subtitleDownloadURIs != null)
            {
                foreach (string url in subtitleDownloadURIs)
                {
                    string[] fields = StringUtils.ToStringArray(url, ';');
                    List<String> lFields = new List<string>(fields);

                    lFields.Insert(0, string.Empty);
                    while (lFields.Count < lvDownloadAddresses.Columns.Count)
                    {
                        lFields.Add(string.Empty);
                    }
                    while (lFields.Count > lvDownloadAddresses.Columns.Count)
                    {
                        lFields.RemoveAt(lFields.Count - 1);
                    }

                    if (lFields[colActive.Index] == "1")
                        lFields[colActive.Index] = Translator.Translate("TXT_YES");
                    else
                        lFields[colActive.Index] = Translator.Translate("TXT_NO");

                    ListViewItem item = new ListViewItem(lFields[0]);

                    bool isDefaultServer = ProTONEConfig.DefaultSubtitleURIs.ToUpperInvariant().Contains(
                        lFields[colServerUrl.Index].ToUpperInvariant());

                    for (int i = 1; i < lFields.Count; i++)
                    {
                        OPMListViewSubItem subItem = null;
                        string text = lFields[i];

                        if (i == colServerType.Index)
                        {
                            subItem = new OPMListViewSubItem(_cmbEditServerType, item, text);
                            subItem.ReadOnly = isDefaultServer;
                        }
                        else if (i == colServerUrl.Index)
                        {
                            subItem = new OPMListViewSubItem(_txtEditServer, item, text);
                            subItem.ReadOnly = isDefaultServer;
                        }
                        else if (i == colActive.Index)
                        {
                            subItem = new OPMListViewSubItem(_cmbEditActive, item, text);
                            subItem.ReadOnly = false;
                        }
                        else if (i == colUserName.Index)
                        {
                            subItem = new OPMListViewSubItem(_txtEditUser, item, text);
                            subItem.ReadOnly = false;
                        }
                        else if (i == colPassword.Index)
                        {
                            subItem = new OPMListViewSubItem(_txtEditPswd, item, text);
                            subItem.ReadOnly = false;
                        }

                        item.SubItems.Add(subItem);
                    }

                    lvDownloadAddresses.Items.Add(item);
                }
            }
        }

        private void lvDownloadAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enable = false;

            try
            {
                if (lvDownloadAddresses.SelectedItems != null && lvDownloadAddresses.SelectedItems.Count == 1)
                {
                    ListViewItem item = lvDownloadAddresses.SelectedItems[0];
                    string serverUrl = item.SubItems[colServerUrl.Index].Text;

                    bool isDefaultServer = ProTONEConfig.DefaultSubtitleURIs.ToUpperInvariant().Contains(
                        serverUrl.ToUpperInvariant());

                    enable = !isDefaultServer;
                }
            }
            finally
            {
                btnDelete.Visible = enable;
            }
        }

        void btnRestoreDefaults_Click(object sender, System.EventArgs e)
        {
            if (MessageDisplay.Query("TXT_CONFIRM_RESTORE", "TXT_RESTORE_DEFAULTSERVERS",
               MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _subtitleDownloadURIs = ProTONEConfig.DefaultSubtitleURIs;
                BuildListFromSubtitleDownloadURIs();
                Modified = true;
            }
        }

    }

    #region SubtitleLanguage class

    public class SubtitleLanguage
    {
        public const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;

        private Language _lang;

        public int LCID => _lang?.Culture?.LCID ?? -1;

        public SubtitleLanguage(Language lang)
        {
            _lang = lang;
        }

        public override string ToString()
        {
            try
            {
                if (_lang != null)
                {
                    return string.Format("{0} | {1} | {2}",
                        StringUtils.CapitalizeWords(_lang.DisplayName()),
                        StringUtils.CapitalizeWords(_lang.Culture.NativeName),
                        _lang.Culture.TwoLetterISOLanguageName.ToUpperInvariant());
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return "[ " + Translator.Translate("TXT_NO_LANG") + " ]";
        }

    }


    #endregion
}

