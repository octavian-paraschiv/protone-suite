using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Themes;
using System.Threading.Tasks;
using OPMedia.Core;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class PlaylistItemInfo : OPMBaseControl
    {
        private PlaylistItem _item = null;

        public PlaylistItemType ItemType { get; set; } 

        public PlaylistItem Item
        {
            get
            {
                return _item;
            }

            set
            {
                if (_item != value)
                {
                    _item = value;
                    UpdateItem();
                }
            }
        }

        private string _altDisplay = "";

        public string AltDisplay
        {
            get
            {
                return _altDisplay;
            }

            set
            {
                if (_altDisplay != value)
                {
                    _altDisplay = value;
                    UpdateItem();
                }
            }
        }

        public PlaylistItemInfo()
        {
            this.ItemType = PlaylistItemType.None;

            InitializeComponent();
            OnThemeUpdatedInternal();

            UpdateItem();
        }

        private void UpdateItem()
        {
            try
            {
                string desc = string.Empty;
                string prefix = string.Empty;
                string name = Translator.Translate("TXT_NA");

                propDisplay.ClearData();
                propDisplay.Visible = false;

                pbInfo.Image = null;
                pbInfo.Visible = false;

                lblDesc.FontSize = (this.ItemType == PlaylistItemType.None) ?
                    FontSizes.NormalBold : FontSizes.Large;

                switch (this.ItemType)
                {
                    case PlaylistItemType.Previous:
                        prefix = "TXT_PREV";
                        break;

                    case PlaylistItemType.Current:
                        prefix = "TXT_NOW";
                        break;

                    case PlaylistItemType.Next:
                        prefix = "TXT_NEXT";
                        break;
                }

                if (this.Item != null &&
                    string.IsNullOrEmpty(this.Item.DisplayName) == false)
                {
                    name = this.Item.DisplayName;

                    if (this.ItemType == PlaylistItemType.Current)
                    {
                        var values = this.Item.MediaInfo;
                        propDisplay.AssignData(null, values, null);

                        propDisplay.Visible = true;
                        propDisplay.Visible = true;
                        pbInfo.Visible = true;

                        Image image = null;

                        Task.Factory.StartNew(() =>
                        {
                            image = ImageProvider.GetImageFromURL(this.Item.ImageURL, 10000);

                        }).ContinueWith(_ =>
                        {
                            pbInfo.Image = image;
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                    }
                }

                if (this.ItemType == PlaylistItemType.None)
                    desc = this.AltDisplay;
                else
                    desc = $"{Translator.Translate(prefix)}: {name}";

                lblDesc.Text = desc;
            }
            finally
            {
            }
        }
    }

    public enum PlaylistItemType
    {
        None,
        Previous,
        Current,
        Next,
    }
}
