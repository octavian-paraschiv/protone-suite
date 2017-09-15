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

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public class MediaBrowserPage : OPMBaseControl
    {
        class MediaBrowserSearchParams
        {
            public OnlineContentSearchParameters SearchParams { get; set; }
            public ManualResetEvent AbortEvent { get; set; }
        }

        public List<IOnlineMediaItem> SelectedItems { get; protected set; }
        public List<IOnlineMediaItem> Items { get; protected set; }

        public event EventHandler SearchCompleted;

        private BackgroundWorker _bwSearch = new BackgroundWorker();

        protected OnlineMediaSource _searchType = OnlineMediaSource.Internal;

        protected OnlineContentSearchFilter SearchFilter { get; set; }

        public new OPMContextMenuStrip ContextMenuStrip { get; set; }

        public MediaBrowserPage() : base()
        {
            this.SelectedItems = new List<IOnlineMediaItem>();
            this.Items = new List<IOnlineMediaItem>();

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

        public virtual bool PreValidateSearch(string search)
        {
            return string.IsNullOrEmpty(search) == false;
        }

        public void StartCancellableSearch(string search, ManualResetEvent abortEvent)
        {
            MediaBrowserSearchParams sp = new MediaBrowserSearchParams
            {
                AbortEvent = abortEvent,
                SearchParams = new OnlineContentSearchParameters
                {
                    Filter = this.SearchFilter,
                    SearchText = search
                }
            };
            _bwSearch.RunWorkerAsync(sp);
        }

        void OnBackgroundSearch(object sender, DoWorkEventArgs e)
        {
            MediaBrowserSearchParams sp = e.Argument as MediaBrowserSearchParams;
            if (sp != null)
                SearchInternal(sp.SearchParams, sp.AbortEvent);
        }

        void OnBackgroundSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (SearchCompleted != null)
                SearchCompleted(this, EventArgs.Empty);
        }

        protected virtual void SearchInternal(OnlineContentSearchParameters searchParams, ManualResetEvent abortEvent)
        {
            this.Items.Clear();
            this.SelectedItems.Clear();

            var results = OnlineContentSearcher.Search(_searchType, searchParams, abortEvent);
            if (results != null && results.Count > 0)
                this.Items.AddRange(results);
        }

        protected OPMContextMenuStrip BuildCommonMenuStrip(bool addToFav)
        {
            OPMContextMenuStrip cms = new OPMContextMenuStrip();

            OPMToolStripMenuItem tsmi = new OPMToolStripMenuItem();

            OPMMenuStripSeparator sep = new OPMMenuStripSeparator();

            tsmi.Click += new EventHandler(OnMenuClick);
            tsmi.Text = Translator.Translate("TXT_PLAY");
            tsmi.Tag = "Play";
            tsmi.Image = Resources.player;
            cms.Items.Add(tsmi);

            tsmi = new OPMToolStripMenuItem();
            tsmi.Click += new EventHandler(OnMenuClick);
            tsmi.Text = Translator.Translate("TXT_ENQUEUE");
            tsmi.Tag = "Enqueue";
            tsmi.Image = Resources.player;
            cms.Items.Add(tsmi);

            cms.Items.Add(sep);

            if (addToFav)
            {
                tsmi = new OPMToolStripMenuItem();
                tsmi.Click += new EventHandler(OnMenuClick);
                tsmi.Text = Translator.Translate("TXT_ADD_FAV_LIST");
                tsmi.Tag = "AddFav";
                tsmi.Image = Resources.Favorites;
                cms.Items.Add(tsmi);
            }
            else
            {
                tsmi = new OPMToolStripMenuItem();
                tsmi.Click += new EventHandler(OnMenuClick);
                tsmi.Text = Translator.Translate("TXT_DEL_FAV_LIST");
                tsmi.Tag = "DelFav";
                tsmi.Image = Resources.btnDelete;
                cms.Items.Add(tsmi);
            }



            return cms;
        }

        protected virtual void DisplaySearchResultsInternal()
        {
        }

        protected void OnMenuClick(object sender, EventArgs e)
        {
            OPMToolStripMenuItem tsmi = sender as OPMToolStripMenuItem;
            if (tsmi != null)
            {
                string act = tsmi.Tag as string;
                switch (act)
                {
                    case "Play":
                        LaunchSelectedItems(false);
                        break;

                    case "Enqueue":
                        LaunchSelectedItems(true);
                        break;

                    default:
                        HandleAction(act);
                        break;

                }
            }
        }

        protected virtual void HandleAction(string act)
        {
        }

        private void LaunchSelectedItems(bool doEnqueue)
        {
            var selItems = this.SelectedItems;
            if (selItems != null && selItems.Count > 0)
                EventDispatch.DispatchEvent(LocalEventNames.LoadOnlineContent, selItems, doEnqueue);
        }

        public virtual string GetSearchBoxTip()
        {
            return null;
        }
    }
}
