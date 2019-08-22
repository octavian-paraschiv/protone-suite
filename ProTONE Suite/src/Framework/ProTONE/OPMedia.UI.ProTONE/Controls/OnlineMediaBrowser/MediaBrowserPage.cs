using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.Core;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.UI.Generic;
using OPMedia.UI.ProTONE.Dialogs;
using System.Windows.Forms;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.UI.Dialogs;

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public class MediaBrowserPage : OPMBaseControl
    {
        class MediaBrowserSearchParams
        {
            public OnlineContentSearchParameters SearchParams { get; set; }
            public ManualResetEvent AbortEvent { get; set; }
        }

        CancellableWaitDialog _waitDialog = null;
        ManualResetEvent _searchCancelled = new ManualResetEvent(false);

        public List<OnlineMediaItem> SelectedItems { get; protected set; }
        public List<OnlineMediaItem> Items { get; protected set; }

        private BackgroundWorker _bwSearch = new BackgroundWorker();
        protected OnlineMediaSource _searchType = OnlineMediaSource.Internal;

        protected OnlineContentSearchFilter SearchFilter { get; set; }

        public new OPMContextMenuStrip ContextMenuStrip { get; set; }

        public MediaBrowserPage() : base()
        {
            this.SelectedItems = new List<OnlineMediaItem>();
            this.Items = new List<OnlineMediaItem>();

            this.SearchFilter = OnlineContentSearchFilter.Any;

            _bwSearch.WorkerSupportsCancellation = false;
            _bwSearch.WorkerReportsProgress = false;
            _bwSearch.DoWork += new DoWorkEventHandler(OnBackgroundSearch);
            _bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundSearchCompleted);
        }

        public void DisplaySearchResults()
        {
            DisplaySearchResultsInternal();
        }

        public bool UpdateSearchHistory(string lastSearchText)
        {
            if (Items == null || Items.Count < 1)
                // Unsuccesful search
                return false;

            return UpdateSearchHistoryInternal(lastSearchText);
        }

        public void Activate()
        {
            LoadSearchHistory();
            PerformValidation();
        }

        public void StartCancellableSearch()
        {
            _searchCancelled.Reset();

            MediaBrowserSearchParams sp = new MediaBrowserSearchParams
            {
                AbortEvent = _searchCancelled,
                SearchParams = new OnlineContentSearchParameters
                {
                    Filter = this.SearchFilter,
                    SearchText = this.GetSearchText()
                }
            };

            _bwSearch.RunWorkerAsync(sp);

            ShowWaitDialog("TXT_WAIT_SEARCH");
        }

        void OnBackgroundSearch(object sender, DoWorkEventArgs e)
        {
            MediaBrowserSearchParams sp = e.Argument as MediaBrowserSearchParams;
            if (sp != null)
                SearchInternal(sp.SearchParams, sp.AbortEvent);
        }

        void OnBackgroundSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplaySearchResults();

            string s = GetSearchText();

            // Don't add "LookupMyPlaylists" in the search history 
            if (string.Compare(s, "LookupMyPlaylists", true) != 0)
            {
                if (UpdateSearchHistory(s))
                    LoadSearchHistory();
            }

            CloseWaitDialog();
        }

        private void SearchInternal(OnlineContentSearchParameters searchParams, ManualResetEvent abortEvent)
        {
            this.Items.Clear();
            this.SelectedItems.Clear();

            if (searchParams.SearchText == "LookupMyPlaylists")
            {
                var playlists = OnlineContentSearcher.GetMyPlaylists(_searchType, abortEvent);
                if (playlists != null && playlists.Count > 0)
                {
                    OnlinePlaylist p = null;

                    MainThread.Send((c) =>
                        {
                            SelectOnlinePlaylistDlg dlg = new SelectOnlinePlaylistDlg(playlists);
                            if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
                                p = dlg.SelectedItem;
                        });

                    if (p != null)
                    {
                        var results = OnlineContentSearcher.ExpandOnlinePlaylist(_searchType, p, abortEvent);
                        if (results != null && results.Count > 0)
                            this.Items.AddRange(results);
                    }
                }
            }
            else
            {
                var results = OnlineContentSearcher.Search(_searchType, searchParams, abortEvent);
                if (results != null && results.Count > 0)
                    this.Items.AddRange(results);
            }
        }

        protected OPMContextMenuStrip BuildCommonMenuStrip(bool addToFav)
        {
            OPMContextMenuStrip cms = new OPMContextMenuStrip();

            OPMToolStripMenuItem tsmi = new OPMToolStripMenuItem();

            OPMMenuStripSeparator sep = new OPMMenuStripSeparator();

            tsmi.Click += new EventHandler(OnMenuClick);
            tsmi.Text = Translator.Translate("TXT_PLAY");
            tsmi.Tag = MediaBrowserAction.Play;
            tsmi.Image = ImageProcessing.Player16;
            cms.Items.Add(tsmi);

            tsmi = new OPMToolStripMenuItem();
            tsmi.Click += new EventHandler(OnMenuClick);
            tsmi.Text = Translator.Translate("TXT_ENQUEUE");
            tsmi.Tag = MediaBrowserAction.Enqueue;
            tsmi.Image = ImageProcessing.Player16;
            cms.Items.Add(tsmi);

            cms.Items.Add(sep);

            if (addToFav)
            {
                tsmi = new OPMToolStripMenuItem();
                tsmi.Click += new EventHandler(OnMenuClick);
                tsmi.Text = Translator.Translate("TXT_ADD_FAV_LIST");
                tsmi.Tag = MediaBrowserAction.AddFav;
                tsmi.Image = OPMedia.UI.Properties.Resources.Favorites16;
                cms.Items.Add(tsmi);
            }
            else
            {
                tsmi = new OPMToolStripMenuItem();
                tsmi.Click += new EventHandler(OnMenuClick);
                tsmi.Text = Translator.Translate("TXT_DEL_FAV_LIST");
                tsmi.Tag = MediaBrowserAction.DelFav;
                tsmi.Image = OPMedia.UI.Properties.Resources.Delete16;
                cms.Items.Add(tsmi);
            }

            cms.Items.Add(sep);

            tsmi = new OPMToolStripMenuItem();
            tsmi.Click += new EventHandler(OnMenuClick);
            tsmi.Text = Translator.Translate("TXT_ADD_TO_LOCAL_PLAYLIST");
            tsmi.Tag = MediaBrowserAction.AddToLocalPlaylist;
            tsmi.Image = ImageProcessing.Playlist16;
            cms.Items.Add(tsmi);

            return cms;
        }

        protected void OnMenuClick(object sender, EventArgs e)
        {
            OPMToolStripMenuItem tsmi = sender as OPMToolStripMenuItem;
            if (tsmi != null)
            {
                string act = tsmi.Tag as string;
                if (string.IsNullOrEmpty(act) == false)
                {
                    var selItems = this.SelectedItems;
                    if (selItems != null && selItems.Count > 0)
                    {
                        switch (act)
                        {
                            case MediaBrowserAction.Play:
                                EventDispatch.DispatchEvent(LocalEventNames.LoadOnlineContent, selItems, false);
                                return;

                            case MediaBrowserAction.Enqueue:
                                EventDispatch.DispatchEvent(LocalEventNames.LoadOnlineContent, selItems, true);
                                return;

                            case MediaBrowserAction.AddFav:
                                EventDispatch.DispatchEvent(LocalEventNames.ManageOnlineContent, selItems, true);
                                return;

                            case MediaBrowserAction.DelFav:
                                EventDispatch.DispatchEvent(LocalEventNames.ManageOnlineContent, selItems, false);
                                return;

                            case MediaBrowserAction.AddToLocalPlaylist:
                                EventDispatch.DispatchEvent(LocalEventNames.AddToPlaylist, selItems);
                                return;

                            case MediaBrowserAction.AddToDeezerPlaylist:
                                {
                                    List<DeezerTrackPlaylistItem> deezerItems = new List<DeezerTrackPlaylistItem>();

                                    selItems.ForEach((omi) =>
                                    {
                                        if (omi is DeezerTrackItem)
                                            deezerItems.Add(new DeezerTrackPlaylistItem(omi as DeezerTrackItem));
                                    });

                                    EventDispatch.DispatchEvent(LocalEventNames.AddToDeezerPlaylist, deezerItems, FindForm());
                                }
                                return;

                        }
                    }

                    HandleAction(act);
                }
            }
        }

        protected void ShowWaitDialog(string message)
        {
            CloseWaitDialog();

            _waitDialog = new CancellableWaitDialog();
            _waitDialog.FormClosed += new FormClosedEventHandler(_waitDialog_FormClosed);
            _waitDialog.ShowDialog(message, FindForm());
        }

        protected void CloseWaitDialog()
        {
            if (_waitDialog != null)
            {
                _waitDialog.Close();
                _waitDialog = null;
            }
        }

        void _waitDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_waitDialog != null && _waitDialog.EscapePressed)
            {
                _waitDialog.SetText("Please wait while cancelling the search task ...");
                _searchCancelled.Set();
            }
        }


        protected virtual void DisplaySearchResultsInternal()
        {
        }

        protected virtual bool UpdateSearchHistoryInternal(string lastSearchText)
        {
            return false;
        }

        protected virtual void HandleAction(string act)
        {
        }

        protected virtual void LoadSearchHistory()
        {
        }

        public virtual string GetSearchText() { return string.Empty; }

        public virtual bool PreValidateSearch()
        {
            string search = GetSearchText();
            return string.IsNullOrEmpty(search) == false;
        }

        public virtual void PerformValidation() { }
    }

    public class MediaBrowserAction
    {
        public const string Play = "Play";
        public const string Enqueue = "Enqueue";
        public const string AddFav = "AddFav";
        public const string DelFav = "DelFav";

        public const string AddToLocalPlaylist = "AddToLocalPlaylist";
        public const string AddToDeezerPlaylist = "AddToDeezerPlaylist";
    }
}
