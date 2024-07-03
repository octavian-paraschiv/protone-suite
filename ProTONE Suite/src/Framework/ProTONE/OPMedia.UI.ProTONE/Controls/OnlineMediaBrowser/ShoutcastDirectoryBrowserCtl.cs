using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.Runtime.ProTONE.Playlists;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public partial class ShoutcastDirectoryBrowserCtl : MediaBrowserPage
    {
        public ShoutcastDirectoryBrowserCtl()
        {
            InitializeComponent();
            _searchType = OnlineMediaSource.ShoutCast;

            lvRadioStations.MultiSelect = true;
            lvRadioStations.Resize += lvRadioStations_Resize;
            lvRadioStations.SelectedIndexChanged += LvRadioStations_SelectedIndexChanged;
            lvRadioStations.ContextMenuStrip = BuildCommonMenuStrip(true);
        }


        private void LvRadioStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.SelectedItems.Clear();

            foreach (ListViewItem lvi in lvRadioStations.SelectedItems)
            {
                RadioStation rs = lvi.Tag as RadioStation;
                if (rs != null)
                    base.SelectedItems.Add(rs);
            }
        }

        private void lvRadioStations_Resize(object sender, EventArgs e)
        {
            AdjustColumns();
        }

        private void AdjustColumns()
        {

            colSource.Width = colGenre.Width = colMediaType.Width = 70;
            //colContent.Width = 100;
            colBitrate.Width = 50;

            int w = colSource.Width + colGenre.Width + colMediaType.Width + colBitrate.Width;
            colContent.Width = colURL.Width = colName.Width = (lvRadioStations.EffectiveWidth - w) / 3;
        }


        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            PerformValidation();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            StartCancellableSearch();
        }

        private void cmbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None &&
                e.KeyCode == Keys.Enter)
            {
                StartCancellableSearch();
            }
        }

        protected override void DisplaySearchResultsInternal()
        {
            lvRadioStations.Items.Clear();

            if (this.Items != null)
            {
                foreach (var item in this.Items)
                {
                    RadioStation rs = item as RadioStation;
                    if (rs != null)
                    {
                        string title = rs.Title ?? string.Empty;

                        ListViewItem lvi = new ListViewItem(new string[] { "",

                        title.ToUpperInvariant().StartsWith("TXT_") ?
                            Translator.Translate(title) : title,

                        rs.IsFake ? "" : rs.Source.ToString(),
                        rs.Url, rs.Content, rs.Genre,
                        rs.IsFake ? "" : rs.Bitrate.ToString(),
                        rs.Type });

                        lvi.Tag = rs;
                        lvRadioStations.Items.Add(lvi);
                    }
                }

                if (lvRadioStations.Items.Count > 0)
                {
                    lvRadioStations.Items[0].Selected = true;
                    lvRadioStations.Items[0].Focused = true;
                }
            }
        }

        public override bool PreValidateSearch()
        {
            bool valid = base.PreValidateSearch();
            if (valid)
            {
                string search = this.GetSearchText();
                search = search.ToLowerInvariant();

                if (search.StartsWith("now:"))
                {
                    // We're trying to make use of the "now playing" advanced search filter.
                    // If the search string after "now:" is non empty then we're OK.
                    search = search.Replace("now:", "");
                    valid = string.IsNullOrEmpty(search) == false;
                }
            }

            return valid;
        }

        protected override void LoadSearchHistory()
        {
            cmbSearch.Items.Clear();
            var history = ProTONEConfig.Media_Browser_History_Shoutcast;
            if (history != null && history.Count > 0)
                cmbSearch.Items.AddRange(history.ToArray());
        }

        protected override bool UpdateSearchHistoryInternal(string lastSearchText)
        {
            try
            {
                if (string.IsNullOrEmpty(lastSearchText) == false)
                {
                    List<string> history = new List<string>(ProTONEConfig.Media_Browser_History_Shoutcast);

                    // Check whether it is already in the search history.
                    // Ignore letter case.
                    foreach (string s in history)
                    {
                        if (string.Compare(s, lastSearchText, true) == 0)
                            return false;
                    }

                    if (history.Count >= 20)
                        history.RemoveAt(0);

                    history.Add(lastSearchText);

                    ProTONEConfig.Media_Browser_History_Shoutcast = history;

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return false;
        }


        public override string GetSearchText()
        {
            return cmbSearch.Text;
        }

        public override void PerformValidation()
        {
            btnSearch.Enabled = base.PreValidateSearch();
        }

    }
}
