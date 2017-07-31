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
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.Dialogs
{
    public partial class OnlineContentBrowser : ThemeForm
    {
        CancellableWaitDialog _waitDialog = null;
        ManualResetEvent _searchCancelled = new ManualResetEvent(false);


        public OnlineContentBrowser() : base("TXT_SELECT_ONLINE_MEDIA")
        {
            InitializeComponent();

            this.ShowInTaskbar = true;

            if (OnlineContentSearcher.IsSearchConfigValid(OnlineMediaSource.Internal) == false)
                tabContentBrowser.TabPages.Remove(tpLocalDatabase);

            if (OnlineContentSearcher.IsSearchConfigValid(OnlineMediaSource.ShoutCast) == false)
                tabContentBrowser.TabPages.Remove(tpShoutcastDir);

            if (OnlineContentSearcher.IsSearchConfigValid(OnlineMediaSource.Deezer) == false)
                tabContentBrowser.TabPages.Remove(tpDeezerContent);

            this.Shown += OnlineContentBrowser_Shown;
        }

        private void OnlineContentBrowser_Shown(object sender, EventArgs e)
        {
            txtSearch.Select();
            txtSearch.Focus();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MediaBrowserPage selectedPage = GetSelectedPage();
            if (selectedPage != null)
            {
                string search = txtSearch.Text;

                selectedPage.SearchCompleted -= SelectedPage_SearchCompleted;
                selectedPage.SearchCompleted += SelectedPage_SearchCompleted;

                selectedPage.StartCancellableSearch(txtSearch.Text, _searchCancelled);
                ShowWaitDialog("Please wait for the search task to finish.\r\nYou can press ESC to cancel the search.");
            }
        }

        private void SelectedPage_SearchCompleted(object sender, EventArgs e)
        {
            MediaBrowserPage senderPage = sender as MediaBrowserPage;
            MediaBrowserPage selectedPage = GetSelectedPage();

            if (senderPage != null && senderPage == selectedPage)
                senderPage.DisplaySearchResults();

            CloseWaitDialog();
        }

        private void ShowWaitDialog(string message)
        {
            CloseWaitDialog();

            _waitDialog = new CancellableWaitDialog();
            _waitDialog.FormClosed += new FormClosedEventHandler(_waitDialog_FormClosed);
            _waitDialog.ShowDialog(message, this);
        }

        void _waitDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_waitDialog != null && _waitDialog.EscapePressed)
            {
                _waitDialog.SetText("Please wait while cancelling the search task ...");
                _searchCancelled.Set();
            }
        }

        private void CloseWaitDialog()
        {
            if (_waitDialog != null)
            {
                _waitDialog.Close();
                _waitDialog = null;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            LaunchSelectedItems(false);
        }

        private void btnEnqueue_Click(object sender, EventArgs e)
        {
            LaunchSelectedItems(true);
        }

        private void LaunchSelectedItems(bool doEnqueue)
        {
            MediaBrowserPage selectedPage = GetSelectedPage();
            if (selectedPage != null)
            {
                var selItems = selectedPage.SelectedItems;
                if (selItems != null && selItems.Count > 0)
                    EventDispatch.DispatchEvent(LocalEventNames.LoadOnlineContent, selItems, doEnqueue);
            }
        }
    }

    

}
