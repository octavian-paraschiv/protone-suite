using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.ProTONE.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public partial class LocalDatabaseBrowserCtl : MediaBrowserPage
    {
        ImageList _ilImages = null;

        public LocalDatabaseBrowserCtl()
        {
            InitializeComponent();
            _searchType = OnlineMediaSource.Internal;

            lvRadioStations.MultiSelect = true;
            lvRadioStations.Resize += lvRadioStations_Resize;
            lvRadioStations.SelectedIndexChanged += LvRadioStations_SelectedIndexChanged;
            lvRadioStations.ContextMenuStrip = BuildCommonMenuStrip(false);

            _ilImages = new ImageList();
            _ilImages.TransparentColor = Color.White;
            _ilImages.ColorDepth = ColorDepth.Depth32Bit;
            _ilImages.ImageSize = new Size(16, 16);

            _ilImages.Images.Add(Resources.Shoutcast);
            _ilImages.Images.Add(Resources.deezer);

            lvRadioStations.SmallImageList = _ilImages;
        }

        [EventSink(LocalEventNames.ManageOnlineContent)]
        public void ManageOnlineContent(List<OnlineMediaItem> onlineContent, bool doAdd)
        {
            var data = LocalDatabaseSearcher.LoadOnlineMediaData();

            foreach (OnlineMediaItem item in onlineContent)
            {
                if (doAdd)
                    data.SafeAddItem(item);
                else
                    data.SafeRemoveItem(item);
            }

            bool isSaved = LocalDatabaseSearcher.SaveOnlineMediaData(data);
            if (isSaved && !doAdd)
            {
                StartCancellableSearch();
            }
        }


        private void LvRadioStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.SelectedItems.Clear();

            foreach (ListViewItem lvi in lvRadioStations.SelectedItems)
            {
                OnlineMediaItem rs = lvi.Tag as OnlineMediaItem;
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
            colBitrate.Width = 50;
            colGenre.Width = colType.Width = 100;
            colIcon.Width = 20;

            int w = lvRadioStations.EffectiveWidth - (colGenre.Width + colType.Width + colBitrate.Width + colIcon.Width);

            colName.Width = colAlbum.Width = colArtist.Width = colURL.Width = w / 4;
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
                    string title = item.Title ?? string.Empty;

                    int ilIndex = -1;
                    if (item.Source == OnlineMediaSource.ShoutCast)
                        ilIndex = 0;
                    else if (item.Source == OnlineMediaSource.Deezer)
                        ilIndex = 1;

                    ListViewItem lvi = new ListViewItem(new string[]
                    {
                        // Icon
                        string.Empty,

                        // Name
                        title.ToUpperInvariant().StartsWith("TXT_") ?
                            Translator.Translate(title) : title,

                        // Artist
                        item.Artist ?? string.Empty,

                        // Album
                        item.Album ?? string.Empty,

                        // URL
                        item.Url ?? string.Empty,

                        // Genre
                        item.Genre ?? string.Empty,

                        // Bitrate
                        (item.Bitrate > 0) ? item.Bitrate.ToString() : string.Empty,

                        // Type
                        item.Type ?? string.Empty,
                    });

                    lvi.ImageIndex = ilIndex;

                    lvi.Tag = item;
                    lvRadioStations.Items.Add(lvi);
                }
            }

            if (lvRadioStations.Items.Count > 0)
            {
                lvRadioStations.Items[0].Selected = true;
                lvRadioStations.Items[0].Focused = true;
            }
        }

        protected override void LoadSearchHistory()
        {
            cmbSearch.Items.Clear();
            var history = ProTONEConfig.Media_Browser_History_Local;
            if (history != null && history.Count > 0)
                cmbSearch.Items.AddRange(history.ToArray());
        }

        protected override bool UpdateSearchHistoryInternal(string lastSearchText)
        {
            try
            {
                if (string.IsNullOrEmpty(lastSearchText) == false)
                {
                    List<string> history = new List<string>(ProTONEConfig.Media_Browser_History_Local);

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

                    ProTONEConfig.Media_Browser_History_Local = history;

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
