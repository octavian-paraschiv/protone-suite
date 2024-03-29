﻿using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.SubtitleDownload;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using OPMedia.UI.Themes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.SubtitleDownload
{
    public delegate void SubtitleDownloadNotifyHandler(string movieFile, string subtitleFile);

    public class SubtitleDownloaderInfo
    {
        public SubtitleDownloader Downloader { get; set; }
        public SubtitleInfo Info { get; set; }
    }

    public partial class SubtitleDownloadNotifyForm : ToolForm
    {
        Dictionary<SubtitleDownloader, List<SubtitleInfo>> _subtitleDownloadInfo = null;
        string _movieFilePath = string.Empty;

        public event SubtitleDownloadNotifyHandler SubtitleDownloadNotify = null;

        public SubtitleDownloadNotifyForm(string movieFilePath,
            Dictionary<SubtitleDownloader, List<SubtitleInfo>> subtitleDownloadInfo) :
            base("TXT_SUBTITLEDOWNLOADNOTIFY")
        {
            InitializeComponent();

            ThemeManager.SetFont(lvSubtitles, FontSizes.Smallest);

            _subtitleDownloadInfo = subtitleDownloadInfo;
            _movieFilePath = movieFilePath;

            SetTitle(Translator.Translate("TXT_SUBTITLEDOWNLOADNOTIFY", 0, Path.GetFileName(_movieFilePath)));

            this.Width = Math.Min(1024, SystemInformation.PrimaryMonitorSize.Width - 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Load += new EventHandler(SubtitleDownloadNotifyForm_Load);
            lvSubtitles.Resize += new EventHandler(lvSubtitles_Resize);
        }

        void lvSubtitles_Resize(object sender, EventArgs e)
        {
            colServer.Width = 190;
            colPrio.Width = 50;
            colLanguage.Width = 70;
            colFPS.Width = 55;

            colFileName.Width = lvSubtitles.EffectiveWidth -
                (colServer.Width + colPrio.Width + colLanguage.Width + colFPS.Width);
        }

        int _highestPrio = int.MaxValue;
        string none = Translator.Translate("TXT_NA").ToLowerInvariant();

        void SubtitleDownloadNotifyForm_Load(object sender, EventArgs e)
        {
            foreach (SubtitleDownloader sd in _subtitleDownloadInfo.Keys)
            {
                if (sd.Priority < _highestPrio)
                    _highestPrio = sd.Priority;
            }

            foreach (KeyValuePair<SubtitleDownloader, List<SubtitleInfo>> kvp in _subtitleDownloadInfo)
            {
                SubtitleDownloader sd = kvp.Key;
                List<SubtitleInfo> list = kvp.Value;

                if (sd != null && list != null)
                {
                    foreach (SubtitleInfo si in list)
                    {
                        string[] data = new string[]
                        {
                            string.IsNullOrEmpty(si.SubFileName) ? none : si.SubFileName,
                            string.IsNullOrEmpty(sd.DisplayName) ? none : sd.DisplayName,
                            sd.Priority.ToString(),
                            string.IsNullOrEmpty(si.LanguageName) ? none : si.LanguageName,
                            string.IsNullOrEmpty(si.FrameRate) ? none : si.FrameRate
                        };

                        ListViewItem item = new ListViewItem(data);

                        item.Tag = new SubtitleDownloaderInfo
                        {
                            Info = si,
                            Downloader = sd
                        };

                        lvSubtitles.Items.Add(item);

                        item.Font = ThemeManager.SmallestFont;
                        item.UseItemStyleForSubItems = true;
                        item.ForeColor = ThemeManager.ForeColor;

                        if (LanguageHelper.IsSameLanguage(ProTONEConfig.PrefferedSubtitleLang, si.ISO639))
                        {
                            item.ForeColor = ThemeManager.ListActiveItemColor;
                            item.UseItemStyleForSubItems = false;
                        }

                        if (_highestPrio == sd.Priority)
                        {
                            item.UseItemStyleForSubItems = false;
                            item.Font = ThemeManager.SmallFont;
                            foreach (ListViewItem.ListViewSubItem lvsi in item.SubItems)
                                lvsi.Font = ThemeManager.SmallFont;
                        }
                    }
                }
            }

            int count = lvSubtitles.Items.Count;
            string movieName = string.Format("'{0}'", Path.GetFileName(_movieFilePath));

            lvSubtitles.ListViewItemSorter = new SubtitleFormComparer(Columns.Language, _sortOrderParam);
            lvSubtitles.Sort();

            SetTitle(Translator.Translate("TXT_SUBTITLEDOWNLOADNOTIFY", count, movieName));
        }


        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (lvSubtitles.SelectedItems != null && lvSubtitles.SelectedItems.Count > 0)
            {
                ListViewItem item = lvSubtitles.SelectedItems[0];
                if (item.Tag is SubtitleDownloaderInfo sdi)
                {
                    string downloadedSubtitleFile = sdi.Downloader.DownloadCompressedSubtitle(_movieFilePath, sdi.Info);
                    if (!string.IsNullOrEmpty(downloadedSubtitleFile))
                    {
                        string movieFileName = Path.GetFileName(_movieFilePath);

                        if (SubtitleDownloadNotify != null)
                        {
                            SubtitleDownloadNotify(_movieFilePath, downloadedSubtitleFile);
                        }
                    }
                }
            }
        }

        int _oldSortColumn = Columns.Language;
        int _sortOrderParam = 1;

        private void lvSubtitles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == _oldSortColumn)
            {
                _sortOrderParam *= -1;
            }

            _oldSortColumn = e.Column;

            lvSubtitles.ListViewItemSorter = new SubtitleFormComparer(e.Column, _sortOrderParam);
            lvSubtitles.Sort();

        }

        private void lvSubtitles_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Show preffered language with bold text
            if (e.ColumnIndex == Columns.Language)
            {
                if (e.Value != null && LanguageHelper.IsSameLanguage(ProTONEConfig.PrefferedSubtitleLang, e.Value.ToString()))
                {
                    e.CellStyle.Font = new Font(ThemeManager.NormalFont, FontStyle.Bold);
                }

                e.Handled = true;
                return;
            }

            // Mark higherst prio items with red
            if (e.ColumnIndex == Columns.ServerOrder)
            {
                if (e.Value != null && e.Value.ToString() == _highestPrio.ToString())
                {
                    e.CellStyle.ForeColor = ThemeManager.SelectedColor;
                    e.CellStyle.BackColor = ThemeManager.BackColor;

                    e.CellStyle.SelectionForeColor = ThemeManager.BackColor;
                    e.CellStyle.SelectionBackColor = ThemeManager.SelectedColor;
                }

                e.Handled = true;
                return;
            }

            if (e.Value != null && e.Value.ToString() == none)
            {
                e.CellStyle.ForeColor = ThemeManager.BorderColor;
                e.Handled = true;
                return;
            }
        }
    }

    internal class Columns
    {
        public const int FileName = 0;
        public const int ServerName = 1;
        public const int ServerOrder = 2;
        public const int Language = 3;
        public const int Size = 4;
    }

    /// <summary>
    /// Class used to compare two list view items.
    /// Needed to sort the list view items.
    /// </summary>
    internal class SubtitleFormComparer : IComparer
    {
        private int _col;
        private int _sortOrderParam = 1;

        public SubtitleFormComparer(int column, int sortOrderParam)
        {
            _col = column;
            _sortOrderParam = (sortOrderParam > 0) ? 1 : -1;
        }

        public int Compare(object x, object y)
        {
            ListViewItem row1 = x as ListViewItem;
            ListViewItem row2 = y as ListViewItem;

            if (row1 == null && row2 == null)
                return 0;
            else if (row1 == null)
                return 1;
            else if (row2 == null)
                return -1;

            switch (_col)
            {
                case Columns.FileName:
                case Columns.ServerName:
                case Columns.ServerOrder:
                case Columns.Size:
                    {
                        int res = CompareByCriterion(row1, row2, _col);
                        if (res != 0)
                            return _sortOrderParam * res;
                        else
                            goto default;
                    }

                case Columns.Language:
                default:
                    {
                        int localSortParam = 1;
                        if (_col == Columns.Language)
                        {
                            localSortParam *= _sortOrderParam;
                        }

                        int res = CompareLanguages(row1, row2);
                        if (res != 0)
                            return localSortParam * res;
                        else
                            return localSortParam * CompareByCriterion(row1, row2, Columns.ServerOrder);
                    }
            }
        }

        private int CompareByCriterion(ListViewItem row1, ListViewItem row2, int criterion)
        {
            string none = Translator.Translate("TXT_NA").ToLowerInvariant();
            string str1 = row1.SubItems[criterion].Text;
            string str2 = row2.SubItems[criterion].Text;

            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
                return 0;
            else if (string.IsNullOrEmpty(str1))
                return _sortOrderParam;
            else if (string.IsNullOrEmpty(str2))
                return -1 * _sortOrderParam;

            if (str1 == none && str2 == none)
                return 0;
            else if (str1 == none)
                return _sortOrderParam;
            else if (str2 == none)
                return -1 * _sortOrderParam;

            return string.Compare(str1, str2);
        }

        private int CompareLanguages(ListViewItem row1, ListViewItem row2)
        {
            string lang1 = (row1.Tag as SubtitleDownloaderInfo)?.Info.ISO639;
            string lang2 = (row2.Tag as SubtitleDownloaderInfo)?.Info.ISO639;

            bool pref1 = LanguageHelper.IsSameLanguage(ProTONEConfig.PrefferedSubtitleLang, lang1);
            bool pref2 = LanguageHelper.IsSameLanguage(ProTONEConfig.PrefferedSubtitleLang, lang2);

            // Preferred language should go on top
            int res = (pref2 ? 1 : 0) - (pref1 ? 1 : 0);
            if (res != 0)
                return res;

            bool sameLanguage = LanguageHelper.IsSameLanguage(lang1, lang2);

            return sameLanguage ? 0 : string.Compare(lang1, lang2);
        }
    }
}
