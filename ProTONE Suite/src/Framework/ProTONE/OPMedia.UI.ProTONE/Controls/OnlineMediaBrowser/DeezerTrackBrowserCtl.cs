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

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public partial class DeezerTrackBrowserCtl : MediaBrowserPage
    {
        public DeezerTrackBrowserCtl()
        {
            InitializeComponent();
            _searchType = OnlineMediaSource.Deezer;

            lvTracks.Resize += OnListResize;
            lvTracks.SelectedIndexChanged += OnListSelectedIndexChanged;
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
            int w = lvTracks.EffectiveWidth / 4;
            colAlbum.Width = colArtist.Width = colName.Width = colURL.Width = w;
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
                            dzi.Url,
                            dzi.Artist,
                            dzi.Album
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
    }
}
