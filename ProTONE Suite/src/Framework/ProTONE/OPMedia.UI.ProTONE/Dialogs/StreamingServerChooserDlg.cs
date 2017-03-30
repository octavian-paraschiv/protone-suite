using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Themes;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.UI.Controls;
using TagLib;
using OPMedia.UI.Dialogs;
using OPMedia.Core;
using LocalEvents = OPMedia.UI.ProTONE.GlobalEvents;
using OPMedia.Core.TranslationSupport;

namespace OPMedia.UI.ProTONE.Dialogs
{
    public partial class StreamingServerChooserDlg : ThemeForm
    {
        public string Uri 
        {
            get { return txtSelectedURL.Text; }
        }

        public RadioStation RadioStation
        {
            get 
            { 
                RadioStation rs = null;

                if (lvServers.SelectedItems != null && lvServers.SelectedItems.Count > 0)
                {
                    ListViewItem lvi = lvServers.SelectedItems[0];
                    rs = lvi.Tag as RadioStation;
                }

                return rs;
            }
        }

        Timer _tmrSearch = null;

        RadioStationsData _allData = null;
        List<RadioStation> _displayData = null;

        GenericWaitDialog _waitDialog = null;
        BackgroundWorker _bwSearch = new BackgroundWorker();

        public StreamingServerChooserDlg() : base("TXT_SELECT_RADIO_STATION")
        {
            InitializeComponent();

            _bwSearch.WorkerSupportsCancellation = false;
            _bwSearch.WorkerReportsProgress = false;
            _bwSearch.DoWork += new DoWorkEventHandler(OnBackgroundSearch);
            _bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundSearchCompleted);

            AdjustColumns();
            lvServers.Resize += new EventHandler(lvServers_Resize);

            this.Load += new EventHandler(StreamingServerChooserDlg_Load);
            this.Shown += new EventHandler(StreamingServerChooserDlg_Shown);

            //lvServers.ColumnClick += new ColumnClickEventHandler(lvServers_ColumnClick);

            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            lvServers.DoubleClick += new EventHandler(lvServers_DoubleClick);
        }

        void StreamingServerChooserDlg_Shown(object sender, EventArgs e)
        {
            _allData = RadioStationsData.GetDefault();
            DisplayData();
        }

        int _oldColumnIndex = 1;
        SortOrder _sortOrder = SortOrder.Ascending;

        void lvServers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != _oldColumnIndex)
            {
            }
            else
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }

            _oldColumnIndex = e.Column;

            lvServers.ShowSortGlyph(e.Column, _sortOrder);
        }

        void StreamingServerChooserDlg_Load(object sender, EventArgs e)
        {
            lvServers.Items.Clear();
            
            // Move focus in the Search box
            txtSearch.Select();
            txtSearch.Focus();
        }

        private void DisplayData()
        {
            if (_allData != null)
            {
                foreach (RadioStation rs in _allData.RadioStations)
                {
                    string title = rs.Title ?? string.Empty;

                    ListViewItem lvi = new ListViewItem(new string[] { "", 
                        
                        title.ToUpperInvariant().StartsWith("TXT_") ? 
                            Translator.Translate(title) : title, 

                        rs.IsFake ? "" : rs.Source.ToString(), 
                        rs.Url, rs.Content, rs.Genre, 
                        rs.IsFake ? "" : rs.Bitrate.ToString(), 
                        rs.Type });

                    lvi.Tag = rs;
                    lvServers.Items.Add(lvi);
                }

                if (lvServers.Items.Count > 0)
                {
                    lvServers.Items[0].Selected = true;
                    lvServers.Items[0].Focused = true;
                }
            }

            // Move focus in the Search box
            txtSearch.Select();
            txtSearch.Focus();
        }

        void lvServers_Resize(object sender, EventArgs e)
        {
            AdjustColumns();
        }

        private void AdjustColumns()
        {
            colSource.Width = colGenre.Width = colMediaType.Width = 70;
            //colContent.Width = 100;
            colBitrate.Width = 50;

            int w = colSource.Width + colGenre.Width + colMediaType.Width + colBitrate.Width;
            colContent.Width = colURL.Width = colName.Width = (lvServers.EffectiveWidth - w) / 3;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            StartSearchTimer();
        }

        private void StartSearchTimer()
        {
            if (_tmrSearch == null)
            {
                _tmrSearch = new Timer();
                _tmrSearch.Interval = 1000;
                _tmrSearch.Tick += new EventHandler(_tmrSearch_Tick);
            }

            _tmrSearch.Stop();
            _tmrSearch.Start();
        }

        void _tmrSearch_Tick(object sender, EventArgs e)
        {
            if (_tmrSearch != null)
                _tmrSearch.Stop();

            _bwSearch.RunWorkerAsync(txtSearch.Text);
            ShowWaitDialog("Search in progress, please wait ...");
        }

        private void lvServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = string.Empty;
            try
            {
                url = lvServers.SelectedItems[0].SubItems[colURL.Index].Text;
            }
            catch { }

            txtSelectedURL.Text = url;
        }

        void lvServers_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.RadioStation != null)
                EventDispatch.DispatchEvent(LocalEvents.EventNames.LoadRadioStation, this.RadioStation);
        }


        private void ShowWaitDialog(string message)
        {
            CloseWaitDialog();
            _waitDialog = new GenericWaitDialog();
            _waitDialog.ShowDialog(message, this);
        }

        private void CloseWaitDialog()
        {
            if (_waitDialog != null)
            {
                _waitDialog.Close();
                _waitDialog = null;
            }
        }

        void OnBackgroundSearch(object sender, DoWorkEventArgs e)
        {
            string keyword = e.Argument as string;
            _allData = RadioStationsData.Search(keyword);
        }

        void OnBackgroundSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseWaitDialog();
            lvServers.Items.Clear();

            if (e.Cancelled == false && e.Error == null)
                DisplayData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSelectedURL.Text) == false)
            {
                RadioStation rs = new RadioStation(RadioStationSource.Internal);
                rs.Url = txtSelectedURL.Text;
                EventDispatch.DispatchEvent(LocalEvents.EventNames.LoadRadioStation, rs);
                return;
            }

            if (this.RadioStation != null)
            {
                EventDispatch.DispatchEvent(LocalEvents.EventNames.LoadRadioStation, this.RadioStation);
                return;
            }
        }
    }
}
