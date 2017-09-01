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

namespace OPMedia.UI.ProTONE.Dialogs
{
    public partial class OnlineContentBrowser : ThemeForm
    {
        CancellableWaitDialog _waitDialog = null;
        ManualResetEvent _searchCancelled = new ManualResetEvent(false);

        ImageList _ilImages = null;

        public OnlineContentBrowser() : base("TXT_SELECT_ONLINE_MEDIA")
        {
            InitializeComponent();

            this.ShowInTaskbar = true;

            _ilImages = new ImageList();
            _ilImages.TransparentColor = Color.White;
            _ilImages.ColorDepth = ColorDepth.Depth32Bit;
            _ilImages.ImageSize = new Size(32, 32);

            _ilImages.Images.Add(Resources.Favorites);
            _ilImages.Images.Add(Resources.Shoutcast);
            _ilImages.Images.Add(Resources.Deezer);

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

            this.Shown += OnlineContentBrowser_Shown;

            tabContentBrowser.SelectedIndexChanged += new EventHandler(tabContentBrowser_SelectedIndexChanged);

            txtSearch.TextChanged += new EventHandler(OnSearchTextChanged);

            OnSearchTextChanged(null, null);
        }

        void OnSearchTextChanged(object sender, EventArgs e)
        {
            bool validText = false;

            MediaBrowserPage selectedPage = GetSelectedPage();
            if (selectedPage != null)
                validText = selectedPage.PreValidateSearch(txtSearch.Text);

            btnSearch.Enabled = validText;
            txtSearch.BackColor = validText ? ThemeManager.WndValidColor : ThemeManager.ColorValidationFailed;
        }

        void tabContentBrowser_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Select();
            txtSearch.Focus();

            OnSearchTextChanged(null, null);
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

                if (selectedPage.PreValidateSearch(search))
                {
                    selectedPage.SearchCompleted -= SelectedPage_SearchCompleted;
                    selectedPage.SearchCompleted += SelectedPage_SearchCompleted;

                    _searchCancelled.Reset();

                    selectedPage.StartCancellableSearch(txtSearch.Text, _searchCancelled);

                    ShowWaitDialog("Please wait for the search task to finish.\r\nYou can press ESC to cancel the search.");
                }
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
    }

    

}
