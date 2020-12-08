using OPMedia.Core;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.Dialogs;
using OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Controls;
using OPMedia.UI.Controls.Dialogs;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Core.TranslationSupport;
using System.IO;
using OPMedia.Runtime.ProTONE.Playlists;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.Dialogs
{
    public partial class OnlineContentBrowser : ThemeForm
    {
        ImageList _ilImages = null;

        OPMToolTip _tt = null;

        public OnlineContentBrowser() : base("TXT_SELECT_ONLINE_MEDIA")
        {
            InitializeComponent();

            this.ShowInTaskbar = true;

            _ilImages = new ImageList();
            
            _ilImages.TransparentColor = Color.White;

            _ilImages.ColorDepth = ColorDepth.Depth32Bit;
            _ilImages.ImageSize = new Size(32, 32);

            _ilImages.Images.Add(OPMedia.UI.Properties.Resources.Favorites);
            _ilImages.Images.Add(Resources.Shoutcast);
            _ilImages.Images.Add(Resources.deezer);

            tabContentBrowser.ImageList = _ilImages;

            if (OnlineContentSearcher.IsSearchConfigValid(OnlineMediaSource.Internal) == false)
                tabContentBrowser.TabPages.Remove(tpLocalDatabase);
            else
                tpLocalDatabase.ImageIndex = 0;

            if (OnlineContentSearcher.IsSearchConfigValid(OnlineMediaSource.ShoutCast) == false)
                tabContentBrowser.TabPages.Remove(tpShoutcastDir);
            else
                tpShoutcastDir.ImageIndex = 1;

            if (OnlineContentSearcher.IsSearchConfigValid(OnlineMediaSource.Deezer) == false)
                tabContentBrowser.TabPages.Remove(tpDeezerContent);
            else
                tpDeezerContent.ImageIndex = 2;

            tabContentBrowser.SelectedIndexChanged += new EventHandler(tabContentBrowser_SelectedIndexChanged);

            _tt = new OPMToolTip();
        }

        [EventSink(GlobalEvents.EventNames.RestoreOnlineBrowserPosition)]
        public void OnRestoreOnlineBrowserPosition()
        {
            this.WindowState = FormWindowState.Normal;
            this.Location = ProTONEConfig.OnlineContentBrowser_WindowLocation;
            this.Size = ProTONEConfig.OnlineContentBrowser_WindowSize;
            BringToFront();
        }

        protected override bool AllowCloseOnKeyDown(Keys keyDown)
        {
            return (keyDown == Keys.Escape);
        }

        void tabContentBrowser_SelectedIndexChanged(object sender, EventArgs e)
        {
            MediaBrowserPage selectedPage = GetSelectedPage();
            if (selectedPage != null)
                selectedPage.Activate();
        }

        private MediaBrowserPage GetSelectedPage()
        {
            MediaBrowserPage selectedPage = null;

            if (tabContentBrowser.SelectedTab != null &&
                tabContentBrowser.SelectedTab.Controls != null &&
                tabContentBrowser.SelectedTab.Controls.Count > 0)
            {
                selectedPage = tabContentBrowser.SelectedTab.Controls[0] as MediaBrowserPage;
            }

            return selectedPage;
        }

        internal void RestorePosition()
        {
            this.Size = ProTONEConfig.OnlineContentBrowser_WindowSize;
            this.Location = ProTONEConfig.OnlineContentBrowser_WindowLocation;
        }

        internal void SavePosition()
        {
            ProTONEConfig.OnlineContentBrowser_WindowLocation = this.Location;
            ProTONEConfig.OnlineContentBrowser_WindowSize = this.Size;
        }

        [EventSink(LocalEventNames.AddToPlaylist)]
        public void AddToPlaylist(List<OnlineMediaItem> onlineContent)
        {
            string filter = string.Empty;

            filter += RenderingEngine.DefaultInstance.PlaylistsFilter;
            filter += Translator.Translate("TXT_ALL_FILES_FILTER");
            filter = filter.Replace("TXT_PLAYLISTS", Translator.Translate("TXT_PLAYLISTS"));

            OPMSaveFileDialog dlg = new OPMSaveFileDialog();
            dlg.Title = Translator.Translate("TXT_ADD_TO_LOCAL_PLAYLIST");
            dlg.Filter = filter;
            dlg.DefaultExt = "m3u";
            dlg.FilterIndex = ProTONEConfig.PL_LastFilterIndex;
            dlg.InitialDirectory = ProTONEConfig.PL_LastOpenedFolder;

            dlg.InheritAppIcon = false;
            dlg.Icon = OPMedia.UI.Properties.Resources.Save16.ToIcon();

            dlg.FillFavoriteFoldersEvt += () => { return ProTONEConfig.GetFavoriteFolders("FavoriteFolders"); };
            dlg.AddToFavoriteFolders += (s) => { return ProTONEConfig.AddToFavoriteFolders(s); };
            dlg.ShowAddToFavorites = true;

            dlg.ShowNewFolder = true;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                ProTONEConfig.PL_LastFilterIndex = dlg.FilterIndex;

                bool addToExisting = File.Exists(dlg.FileName);

                SaveToPlaylist(dlg.FileName, onlineContent, addToExisting);

                try
                {
                    string file = dlg.FileNames[0];
                    ProTONEConfig.PL_LastOpenedFolder = Path.GetDirectoryName(file);
                }
                catch
                {
                    ProTONEConfig.PL_LastOpenedFolder = dlg.InitialDirectory;
                }
            }
        }

        private void SaveToPlaylist(string fileName, List<OnlineMediaItem> onlineContent, bool addToExisting)
        {
            Playlist pl = new Playlist();

            if (addToExisting)
                pl.LoadPlaylist(fileName);

            onlineContent.ForEach((omi) =>
            {
                if (omi is DeezerTrackItem)
                    pl.Add(new DeezerTrackPlaylistItem(omi as DeezerTrackItem));
                else if (omi is RadioStation)
                    pl.Add(new RadioStationPlaylistItem(omi as RadioStation));
            });

            pl.SavePlaylist(fileName);
        }
    }
}
