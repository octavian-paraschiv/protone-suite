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
using OPMedia.Core.Logging;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class PlaylistItemInfo : OPMBaseControl
    {
        private PlaylistItem _item = null;

        private readonly int ImageSize = 250;

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

                lblDesc.FontSize = (this.ItemType == PlaylistItemType.Current) ?
                    FontSizes.Large : FontSizes.NormalBold;

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
                    string url = this.Item.ImageURL;

                    if (this.ItemType == PlaylistItemType.Current)
                    {
                        var values = this.Item.MediaInfo;
                        propDisplay.AssignData(null, values, null);

                        propDisplay.Visible = true;
                        propDisplay.Visible = true;
                        pbInfo.Visible = true;
                        pbInfo.Width = 1;
                        pbInfo.Margin = new System.Windows.Forms.Padding(0);

                        Image image = null;

                        Task.Factory.StartNew(() =>
                        {
                            if (string.IsNullOrEmpty(url))
                            {
                                // this call is blocking
                                this.Item.Rebuild();

                                url = this.Item.ImageURL ?? "";

                                int len = Math.Min(url.Length, 100);
                                Logger.LogToConsole($"ImageURL: {url.Substring(0, 100)}");
                            }

                            // this call is blocking
                            image = ImageProvider.GetImageFromURL(url, 10000);

                        }).ContinueWith(_ =>
                        {
                            pbInfo.Image = image;
                            if (image != null)
                            {
                                pbInfo.Margin = new System.Windows.Forms.Padding(4, 0, 3, 3);
                                pbInfo.Width = ImageSize;
                            }

                            values = this.Item.MediaInfo;
                            propDisplay.AssignData(null, values, null);

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
