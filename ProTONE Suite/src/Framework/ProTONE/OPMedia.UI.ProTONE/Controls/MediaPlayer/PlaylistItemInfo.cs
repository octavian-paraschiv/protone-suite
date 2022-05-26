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
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.Runtime.ProTONE;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public partial class PlaylistItemInfo : OPMBaseControl
    {
        private PlaylistItem _item = null;

        private readonly int ImageSize = 300;

        public PlaylistItemType ItemType { get; set; }

        public PlaylistItem Item
        {
            get
            {
                return _item;
            }

            set
            {
                if (_item == null || _item != value)
                {
                    _item = value;
                    PerformTranslation();
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
                    PerformTranslation();
                }
            }
        }

        public PlaylistItemInfo()
        {
            this.ItemType = PlaylistItemType.None;

            InitializeComponent();

            pbInfo.Image = Resources.UnknownImage;

            OnThemeUpdatedInternal();
            PerformTranslation();

            RenderingEngine.DefaultInstance.RenderedStreamPropertyChanged += DefaultInstance_RenderedStreamPropertyChanged;
        }

        private void DefaultInstance_RenderedStreamPropertyChanged(Dictionary<string, string> newData)
        {
            var data = this.Item?.MediaInfo;

            if (data != null)
            {
                foreach (var kvp in newData)
                {
                    var key = $"{kvp.Key}:";

                    if (data.ContainsKey(key))
                        data[key] = kvp.Value;
                    else
                        data.Add(key, kvp.Value);
                }

                propDisplay.SetInfo(null, data);
            }
        }

        private string _oldUrl = null;


        protected override void OnThemeUpdatedInternal()
        {
            propDisplay.ForeColor = ThemeManager.ForeColor;
            propDisplay.BackColor = ThemeManager.GradientNormalColor1;
            PerformTranslation();
        }

        [EventSink(EventNames.PerformTranslation)]
        public void PerformTranslation()
        {
            try
            {
                string desc = string.Empty;
                string prefix = string.Empty;
                string name = Translator.Translate("TXT_UNKNOWN");

                propDisplay.SetInfo(null, null);

                if (this.ItemType != PlaylistItemType.Current)
                {
                    pnlDisplay.Visible = false;
                    pbInfo.Image = null;
                    pbInfo.Visible = false;
                }
                else
                {
                    pnlDisplay.Visible = true;
                    pbInfo.Visible = true;
                }

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
                        propDisplay.SetInfo(null, values);

                        Image image = null;
                        bool isInfoRebuilt = false;
                        bool isImageChanged = (string.Compare(_oldUrl, url, false) != 0);

                        Task.Factory.StartNew(() =>
                        {
                            if (string.IsNullOrEmpty(url))
                            {
                                // this call is blocking
                                this.Item.Rebuild();

                                url = this.Item.ImageURL ?? "";

                                int len = Math.Min(url.Length, 100);
                                Logger.LogToConsole($"ImageURL: {url.Substring(0, len)}");

                                isInfoRebuilt = true;
                            }

                            // this call is blocking
                            Size sz = new Size(ImageSize, ImageSize);
                            image = ImageProvider.GetImageFromURL(url, 10000, sz);

                        }).ContinueWith(_ =>
                        {
                            if (isImageChanged || isInfoRebuilt)
                            {
                                _oldUrl = url;

                                if (image == null)
                                {
                                    string itemType = (this.Item.Type ?? "").ToLowerInvariant();
                                    if (SupportedFileProvider.Instance.SupportedVideoTypes.Contains(itemType))
                                        image = Resources.VideoDefaultImage;
                                    else if (itemType == "URL" || SupportedFileProvider.Instance.SupportedAudioTypes.Contains(itemType))
                                        image = Resources.AudioDefaultImage;
                                    else
                                        image = Resources.UnknownImage;
                                }

                                pbInfo.Image = image;
                            }

                            if (isInfoRebuilt)
                            {
                                // Media info has been rebuilt so reload it
                                values = this.Item.MediaInfo;
                                propDisplay.SetInfo(null, values);


                            }

                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                }
                else
                {
                    pbInfo.Image = Resources.UnknownImage;
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    values.Add("TXT_UNKNOWN", "");
                    propDisplay.SetInfo(null, values);
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
