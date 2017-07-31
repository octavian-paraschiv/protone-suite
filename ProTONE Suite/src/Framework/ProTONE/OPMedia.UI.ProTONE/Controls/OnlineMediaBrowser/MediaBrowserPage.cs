using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    public class MediaBrowserPage : OPMBaseControl
    {
        class SearchParams
        {
            public string SearchText { get; set; }
            public ManualResetEvent AbortEvent { get; set; }
        }

        public List<IOnlineMediaItem> SelectedItems { get; protected set; }
        public List<IOnlineMediaItem> Items { get; protected set; }

        public event EventHandler SearchCompleted;

        private BackgroundWorker _bwSearch = new BackgroundWorker();

        protected OnlineMediaSource _searchType = OnlineMediaSource.Internal;

        public MediaBrowserPage() : base()
        {
            this.SelectedItems = new List<IOnlineMediaItem>();
            this.Items = new List<IOnlineMediaItem>();

            _bwSearch.WorkerSupportsCancellation = false;
            _bwSearch.WorkerReportsProgress = false;
            _bwSearch.DoWork += new DoWorkEventHandler(OnBackgroundSearch);
            _bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundSearchCompleted);
        }

        public void DisplaySearchResults()
        {
            DisplaySearchResultsInternal();
        }

        public void StartCancellableSearch(string search, ManualResetEvent abortEvent)
        {
            SearchParams sp = new SearchParams { SearchText = search, AbortEvent = abortEvent };
            _bwSearch.RunWorkerAsync(sp);
        }

        void OnBackgroundSearch(object sender, DoWorkEventArgs e)
        {
            SearchParams sp = e.Argument as SearchParams;
            if (sp != null)
                SearchInternal(sp.SearchText, sp.AbortEvent);
        }

        void OnBackgroundSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (SearchCompleted != null)
                SearchCompleted(this, EventArgs.Empty);
        }

        protected virtual void SearchInternal(string search, ManualResetEvent abortEvent)
        {
            this.Items.Clear();
            this.SelectedItems.Clear();

            var results = OnlineContentSearcher.Search(_searchType, search, abortEvent);
            if (results != null && results.Count > 0)
                this.Items.AddRange(results);
        }

        protected virtual void DisplaySearchResultsInternal()
        {
        }
    }
}
