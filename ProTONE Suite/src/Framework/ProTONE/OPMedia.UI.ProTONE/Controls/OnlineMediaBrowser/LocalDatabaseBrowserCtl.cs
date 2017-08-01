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
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Controls;
using OPMedia.UI.Menus;
using System.Linq;

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public partial class LocalDatabaseBrowserCtl : MediaBrowserPage
    {
        public LocalDatabaseBrowserCtl()
        {
            InitializeComponent();
            _searchType = OnlineMediaSource.Internal;

            lvRadioStations.MultiSelect = true;
            lvRadioStations.Resize += lvRadioStations_Resize;
            lvRadioStations.SelectedIndexChanged += LvRadioStations_SelectedIndexChanged;
            lvRadioStations.ContextMenuStrip = BuildMenuStrip(false);
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
            colGenre.Width = 120;

            int w = lvRadioStations.EffectiveWidth - colGenre.Width;
            colURL.Width = colName.Width = w / 2;
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

                        ListViewItem lvi = new ListViewItem(new string[] 
                        {
                            // Dummy entry
                            string.Empty,

                            // Name
                            title.ToUpperInvariant().StartsWith("TXT_") ?
                                Translator.Translate(title) : title,

                            // URL
                            rs.Url,

                            // Genre
                            rs.Genre
                        });

                        lvi.Tag = rs;
                        lvRadioStations.Items.Add(lvi);
                    }
                }
            }

            if (lvRadioStations.Items.Count > 0)
            {
                lvRadioStations.Items[0].Selected = true;
                lvRadioStations.Items[0].Focused = true;
            }
        }
    }
}
