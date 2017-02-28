﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Runtime.ProTONE.ExtendedInfo;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.UI.Generic;
using OPMedia.Core;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.ProTONE.Dialogs;
using OPMedia.UI.Themes;

using OPMedia.Core.GlobalEvents;

using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using System.Drawing.Design;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.UI.ProTONE.Controls.MediaPlayer;

namespace OPMedia.UI.ProTONE.Controls.BookmarkManagement
{
    public partial class BookmarkScreen : OPMBaseControl
    {
        IWindowsFormsEditorService _wfes = null;

        PlaylistItem _plItem = null;
        ToolTip _tip = new ToolTip();
        Timer _timer = new Timer();

        DateTimePicker _dtpEditTime = new DateTimePicker();
        TextBox _txtEditComment = new TextBox();
        bool _showFilePath;
        bool _canAddToCurrent;

        ImageList _il = null;

        public void CopyPlaylist(PlaylistScreen source)
        {
            this.playlistScreen.CopyPlaylist(source);
        }

        public PlaylistItem PlaylistItem
        { 
            get 
            { 
                return _plItem; 
            } 
            
            set 
            {
                LoadnewPlaylistItem(value, true);
            } 
        }

        bool _showEmbeddedPlaylist = true;
        public bool ShowEmbeddedPlaylist
        {
            get
            {
                return _showEmbeddedPlaylist;
            }

            set
            {
                _showEmbeddedPlaylist = value;
                ShowPlaylist();
            }
        }

        public BookmarkScreen(IWindowsFormsEditorService wfes, PlaylistItem plItem) : this()
        {
            _wfes = wfes;
            _showFilePath = false;
            _canAddToCurrent = false;
            this.PlaylistItem = plItem;
        }

        public BookmarkScreen()
        {
            InitializeComponent();

            _showFilePath = true;
            _canAddToCurrent = true;

            ThemeManager.SetFont(lblItem, FontSizes.Small);

            Translator.TranslateControl(this, DesignMode);

            lblItem.Text = string.Empty;

            pbAdd.Text = pbAddCurrent.Text = pbDelete.Text = string.Empty;

            pbAdd.Image = OPMedia.UI.Properties.Resources.Add;
            pbAddCurrent.Image = OPMedia.UI.Properties.Resources.Add2;
            pbDelete.Image = OPMedia.UI.Properties.Resources.Del;

            _tip.SetToolTip(pbAdd, Translator.Translate("TXT_NEW_BMDESC"));
            _tip.SetToolTip(pbAddCurrent, Translator.Translate("TXT_NEWNOW_BMDESC"));
            _tip.SetToolTip(pbDelete, Translator.Translate("TXT_DELETE_BMDESC"));

            _timer = new Timer();
            _timer.Interval = 300;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Enabled = true;

            _il = new ImageList();
            _il.ColorDepth = ColorDepth.Depth32Bit;
            _il.ImageSize = new System.Drawing.Size(16, 16);
            _il.Images.Add(OPMedia.Core.Properties.Resources.bookmark);
            lvBookmarks.SmallImageList = _il;

            ManageAddCurrentButtonState();

            _dtpEditTime.ShowUpDown = true;
            _dtpEditTime.Format = DateTimePickerFormat.Custom;
            _dtpEditTime.CustomFormat = "HH:mm:ss";
            _dtpEditTime.Enabled = true;
            lvBookmarks.RegisterEditControl(_dtpEditTime);

            lvBookmarks.RegisterEditControl(_txtEditComment);

            lvBookmarks.ColumnWidthChanging += new ColumnWidthChangingEventHandler(lvBookmarks_ColumnWidthChanging);
            lvBookmarks.Resize += new EventHandler(lvBookmarks_Resize);
            lvBookmarks.SubItemEdited += new OPMListView.EditableListViewEventHandler(lvBookmarks_SubItemEdited);

            this.Load += new EventHandler(BookmarkManagerCtl_Load);

            playlistScreen.SelectedItemChanged += new SelectedItemChangedHandler(playlistScreen_SelectedItemChanged);

            ShowPlaylist();
        }

        bool _selectEventDisabled = false;

        void playlistScreen_SelectedItemChanged(PlaylistItem newSelectedItem)
        {
            if (_selectEventDisabled)
                return;

            LoadnewPlaylistItem(newSelectedItem, false);
        }

        void BookmarkManagerCtl_Load(object sender, EventArgs e)
        {
            lblItem.Visible = _showFilePath;
        }

        void lvBookmarks_SubItemEdited(object sender, ListViewSubItemEventArgs args)
        {
            SaveBookmarks(true);
        }

        void lvBookmarks_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            ApplyColumnWidths();
        }

        void lvBookmarks_Resize(object sender, EventArgs e)
        {
            ApplyColumnWidths();
        }

        void ApplyColumnWidths()
        {
            colIcon.Width = 20;
            int w = lvBookmarks.EffectiveWidth - colIcon.Width;
            colTime.Width = colText.Width = w / 2;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            ManageAddCurrentButtonState();
        }
        
        public void ManageAddCurrentButtonState()
        {
            try
            {
                // Add to current possible only if we are currently playing this item.
                pbAddCurrent.Visible = _canAddToCurrent &&
                    _plItem != null && _plItem.IsBookmarkInfoEditable &&
                    MediaRenderer.DefaultInstance.RenderedMediaInfo != null &&
                    MediaRenderer.DefaultInstance.RenderedMediaInfo.Equals(_plItem.MediaFileInfo) &&
                    MediaRenderer.DefaultInstance.FilterState != FilterState.NotOpened &&
                    MediaRenderer.DefaultInstance.FilterState != FilterState.Stopped;
            }
            catch
            {
            }
        }

        public void LoadnewPlaylistItem(PlaylistItem plItem, bool callFromProperty)
        {
            try
            {
                if (_plItem != null && _plItem.MediaFileInfo != null)
                {
                    _plItem.MediaFileInfo.BookmarkCollectionChanged -=
                        new EventHandler(MediaFileInfo_BookmarkCollectionChanged);
                }

                if (plItem == null && callFromProperty)
                {
                    List<PlaylistItem> plItems = playlistScreen.GetPlaylistItems();
                    if (plItems != null && plItems.Count > 0)
                    {
                        plItem = plItems[0];
                    }
                }

                _plItem = plItem;

                if (_plItem != null && callFromProperty)
                {
                    playlistScreen.SetFirstSelectedPlaylistItem(_plItem);
                }

                LoadBookmarks();

                if (_plItem != null && _plItem.MediaFileInfo != null)
                {
                    _plItem.MediaFileInfo.BookmarkCollectionChanged +=
                        new EventHandler(MediaFileInfo_BookmarkCollectionChanged);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void LoadBookmarks()
        {
            this.SuspendLayoutEx();

            pnlBookmarkEdit.Visible = false;
            lblNoInfo.Visible = true;

            try
            {
                lvBookmarks.Items.Clear();
                lblItem.Text = string.Empty;

                if (_plItem == null)
                    return;

                try
                {
                    lblItem.Text = _plItem.MediaFileInfo.Path;

                    if (_plItem.SupportsBookmarkInfo &&
                        _plItem.MediaFileInfo.Bookmarks != null)
                    {
                        pnlBookmarkEdit.Visible = true;
                        lblNoInfo.Visible = false;

                        pbAdd.Visible = pbDelete.Visible = _plItem.IsBookmarkInfoEditable;
                        lvBookmarks.AllowEditing = _plItem.IsBookmarkInfoEditable;

                        List<Bookmark> bmkList =
                            new List<Bookmark>(_plItem.MediaFileInfo.Bookmarks.Values);

                        bmkList.Sort(Bookmark.CompareByTime);

                        foreach (Bookmark bmk in bmkList)
                        {
                            string[] subItems = new string[]
                            {
                                string.Empty,
                                bmk.PlaybackTime.ToString(),
                                bmk.Title
                            };

                            int i = 0;
                            ListViewItem item = new ListViewItem(subItems[i++]);
                            item.Tag = bmk;
                            item.ImageIndex = 0;

                            OPMListViewSubItem si = new OPMListViewSubItem(_dtpEditTime, item, subItems[i++]);
                            si.ReadOnly = false;
                            item.SubItems.Add(si);

                            si = new OPMListViewSubItem(_txtEditComment, item, subItems[i++]);
                            si.ReadOnly = false;
                            item.SubItems.Add(si);

                            lvBookmarks.Items.Add(item);
                        }

                        if (lvBookmarks.Items.Count > 0)
                        {
                            lvBookmarks.Select();
                            lvBookmarks.Focus();
                            lvBookmarks.Items[0].Selected = true;
                        }
                    }
                    else
                    {
                        pbAdd.Visible = pbDelete.Visible = pbAddCurrent.Visible = false;
                        lvBookmarks.Enabled = false;
                        lvBookmarks.AllowEditing = false;
                    }
                }
                catch (Exception ex)
                {
                    pbAdd.Visible = pbDelete.Visible = pbAddCurrent.Visible = false;
                    lvBookmarks.Enabled = false;
                    lvBookmarks.AllowEditing = false;

                    ErrorDispatcher.DispatchError(ex, false);
                }
            }
            finally
            {
                this.ResumeLayoutEx();
            }
        }

        void MediaFileInfo_BookmarkCollectionChanged(object sender, EventArgs e)
        {
            LoadBookmarks();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string[] subItems = new string[]
            {
                string.Empty,
                CalculateNewBookmarkTime(0),
                Translator.Translate("TXT_NEW_BOOKMARK")
            };

            int i = 0;
            ListViewItem item = new ListViewItem(subItems[i++]);
            item.ImageIndex = 0;
            //item.Tag = bmk;

            OPMListViewSubItem si = new OPMListViewSubItem(_dtpEditTime, item, subItems[i++]);
            si.ReadOnly = false;
            item.SubItems.Add(si);

            si = new OPMListViewSubItem(_txtEditComment, item, subItems[i++]);
            si.ReadOnly = false;
            item.SubItems.Add(si);

            item = lvBookmarks.Items.Add(item);

            SaveBookmarks(true);

            item.Selected = true;

            lvBookmarks.StartEditing(item, si);
        }

        private void btnAddCurrent_Click(object sender, EventArgs e)
        {
            MediaRenderer.DefaultInstance.PauseRenderer();

            string[] subItems = new string[]
            {
                string.Empty,
                CalculateNewBookmarkTime((int)MediaRenderer.DefaultInstance.MediaPosition),
                Translator.Translate("TXT_NEW_BOOKMARK")
            };

            int i = 0;
            ListViewItem item = new ListViewItem(subItems[i++]);
            item.ImageIndex = 0;
            //item.Tag = bmk;

            OPMListViewSubItem si = new OPMListViewSubItem(_dtpEditTime, item, subItems[i++]);
            si.ReadOnly = false;
            item.SubItems.Add(si);

            si = new OPMListViewSubItem(_txtEditComment, item, subItems[i++]);
            si.ReadOnly = false;
            item.SubItems.Add(si);

            item = lvBookmarks.Items.Add(item);

            SaveBookmarks(true);

            item.Selected = true;

            lvBookmarks.StartEditing(item, si);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvBookmarks.SelectedItems.Count > 0)
            {
                lvBookmarks.Items.Remove(lvBookmarks.SelectedItems[0]);

                SaveBookmarks(true);

                lvBookmarks.Select();
                lvBookmarks.Focus();

                if (lvBookmarks.Items.Count > 0)
                {
                    lvBookmarks.Items[0].Selected = true;
                }

            }
        }

        public void SaveBookmarks(bool reloadAfterSave)
        {
            if (_plItem != null &&
                _plItem.MediaFileInfo != null &&
                _plItem.MediaFileInfo.Bookmarks != null && 
                _plItem.IsBookmarkInfoEditable)
            {
                _plItem.MediaFileInfo.Bookmarks.Clear();

                foreach (ListViewItem row in lvBookmarks.Items)
                {
                    string time = row.SubItems[colTime.Index].Text;
                    string desc = row.SubItems[colText.Index].Text;
                    
                    if (!string.IsNullOrEmpty(time))
                    {
                        TimeSpan ts = (TimeSpan)new TimeSpanConverter().ConvertFromInvariantString(time);
                            //- Bookmark.MinimumDate;
                        
                        Bookmark bmk = new Bookmark(desc, ts);
                        if (_plItem.MediaFileInfo.Bookmarks.ContainsKey(ts))
                        {
                            _plItem.MediaFileInfo.Bookmarks[ts] = bmk;
                        }
                        else
                        {
                            _plItem.MediaFileInfo.Bookmarks.Add(ts, bmk);
                        }
                    }
                }

                _plItem.MediaFileInfo.SaveBookmarks(reloadAfterSave);
                if (reloadAfterSave)
                {
                    LoadBookmarks();
                }
            }
        }

        private void lvBookmarks_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvBookmarks.SelectedItems.Count > 0)
            {
                Bookmark bmk = lvBookmarks.SelectedItems[0].Tag as Bookmark;
                if (bmk != null)
                {
                    BookmarkSubItem subItem = new BookmarkSubItem(_plItem, bmk);
                    EventDispatch.DispatchEvent(LocalEventNames.JumpToBookmark, subItem);
                }
            }
        }

        private string CalculateNewBookmarkTime(int secondsStart)
        {
            if (_plItem != null &&
               _plItem.MediaFileInfo != null &&
               _plItem.MediaFileInfo.Bookmarks != null)
            {
                int sec = secondsStart;
                for(;;)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(sec);

                    if (!_plItem.MediaFileInfo.Bookmarks.ContainsKey(ts))
                        return ts.ToString();

                    sec++;
                }
            }

            TimeSpan timePart = DateTime.Now.Subtract(DateTime.Today);
            return new TimeSpan((int)timePart.TotalSeconds).ToString();
        }

        private void ShowPlaylist()
        {
            this.pnlLayout.ColumnStyles.Clear();
            if (_showEmbeddedPlaylist)
            {
                this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }
            else
            {
                this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
                this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            }

            playlistScreen.Visible = _showEmbeddedPlaylist;
        }

    }



    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class BookmarkPropertyBrowser : UITypeEditor
    {
        public BookmarkPropertyBrowser()
            : base()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null)
            {
                if (context.Instance != null &&
                    (context.Instance is MediaFileInfo || context.Instance is BookmarkFileInfo))
                {
                    return UITypeEditorEditStyle.DropDown;
                }
            }

            return base.GetEditStyle(context);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            PlaylistItem plItem = value as PlaylistItem;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                BookmarkScreen ctl = new BookmarkScreen(edSvc, plItem);
                ctl.ShowEmbeddedPlaylist = false;
                ctl.BorderStyle = BorderStyle.Fixed3D;
                edSvc.DropDownControl(ctl);
            }

            return plItem;
        }
    }
}
