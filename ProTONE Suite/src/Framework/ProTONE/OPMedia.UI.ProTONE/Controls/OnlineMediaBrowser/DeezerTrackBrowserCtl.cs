using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.Controls;
using OPMedia.UI.Menus;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Themes;

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public partial class DeezerTrackBrowserCtl : MediaBrowserPage
    {
        public DeezerTrackBrowserCtl()
        {
            InitializeComponent();
            _searchType = OnlineMediaSource.Deezer;

            lvTracks.MultiSelect = true;
            lvTracks.Resize += OnListResize;
            lvTracks.SelectedIndexChanged += OnListSelectedIndexChanged;
            lvTracks.ContextMenuStrip = BuildMenuStrip(true);
            OnThemeUpdatedInternal();

            this.Load += new EventHandler(OnLoad);
        }

        void OnLoad(object sender, EventArgs e)
        {
            if (this.DesignMode == false)
            {
                // Setting RTF should always be done inside OnLoad ... not on constructor ...
                lblFilterHint.Rtf = Translator.Translate("TXT_DEEZERFILTER_HINT");
            }
        }

        protected override void OnThemeUpdatedInternal()
        {
            lblFilterHint.BackColor = ThemeManager.BackColor;
            lblFilterHint.ForeColor = ThemeManager.ForeColor;
        }

        private void OnListSelectedIndexChanged(object sender, EventArgs e)
        {
            base.SelectedItems.Clear();

            foreach (ListViewItem lvi in lvTracks.SelectedItems)
            {
                DeezerTrackItem dti = lvi.Tag as DeezerTrackItem;
                if (dti != null)
                    base.SelectedItems.Add(dti);
            }
        }

        private void OnListResize(object sender, EventArgs e)
        {
            AdjustColumns();
        }

        private void AdjustColumns()
        {
            colURL.Width = 150;
            int w = (lvTracks.EffectiveWidth - colURL.Width) / 3;
            colAlbum.Width = colArtist.Width = colName.Width = w;
        }

        protected override void DisplaySearchResultsInternal()
        {
            lvTracks.Items.Clear();

            if (this.Items != null)
            {
                foreach (var item in this.Items)
                {
                    DeezerTrackItem dzi = item as DeezerTrackItem;
                    if (dzi != null)
                    {
                        ListViewItem lvi = new ListViewItem(new string[]
                        {
                            string.Empty,
                            dzi.Title,
                            dzi.Artist,
                            dzi.Album,
                            dzi.Url
                        });

                        lvi.Tag = dzi;
                        lvTracks.Items.Add(lvi);
                    }
                }

                if (lvTracks.Items.Count > 0)
                {
                    lvTracks.Items[0].Selected = true;
                    lvTracks.Items[0].Focused = true;
                }
            }
        }

        public override bool PreValidateSearch(string search)
        {
            bool valid = base.PreValidateSearch(search);

            if (valid)
            {
                if (search.Contains(":") || search.Contains("\""))
                {
                    // We're trying to make use of advanced search filters.
                    // If the Deezer Searcher can build a non-empty filter from the user input, then we're OK.
                    DeezerJsonFilter filter = DeezerTrackSearcher.BuildFilterFromQuery(search);
                    valid = DeezerJsonFilter.IsNullOrEmpty(filter) == false;
                }
            }

            return valid;
        }
    }
}
