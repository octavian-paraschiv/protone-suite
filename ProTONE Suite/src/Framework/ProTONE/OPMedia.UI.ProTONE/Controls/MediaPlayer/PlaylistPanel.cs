using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Logging;

using System.IO;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Runtime;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using OPMedia.Core.Configuration;
using System.Diagnostics;

using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.ExtendedInfo;
using OPMedia.UI.Generic;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Menus;
using System.Threading;
using OPMedia.UI.ProTONE.Dialogs;
using OPMedia.UI.ProTONE.SubtitleDownload;
using OPMedia.UI.Controls.Dialogs;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.Runtime.Processors;
using System.Net;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;

using System.Linq;
using OPMedia.Runtime.ProTONE.RemoteControl;
using System.Threading.Tasks;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public delegate void LaunchFileEventHandler(PlaylistItem path);
    public delegate void StopRequestEventHandler();
    public delegate void TotalTimeChangedHandler(TimeSpan tsTotalTime);
    public delegate void SelectedItemChangedHandler(PlaylistItem newSelectedItem);

    public partial class PlaylistPanel : OPMBaseControl
    {
        //OPMToolTipManager _ttm = null;
        //OPMToolTip _tip = new OPMToolTip();

        Playlist playlist = new Playlist();
        
        public event LaunchFileEventHandler LaunchFile = null;
        public event EventHandler PlaylistItemMenuClick = null;
        public event TotalTimeChangedHandler TotalTimeChanged = null;
        public event SelectedItemChangedHandler SelectedItemChanged = null;

        public System.Windows.Forms.Timer _tmrSavePlaylist = null;
        private System.Windows.Forms.Timer _tmrUpdateItemsDesc = null;

        public bool IsPlaylistAtEnd
        { get { return playlist.IsAtEnd; } }

        public int PlayIndex
        {
            get
            {
                return playlist.PlayIndex;
            }
        }

        /*
        private bool _compactMode = false;
        public bool CompactMode
        {
            get { return _compactMode; }
            set 
            { 
                _compactMode = value;

                if (value)
                {
                    pnlLayout.Controls.Remove(piNext);
                    pnlLayout.Controls.Remove(piCurrent);
                    pnlLayout.Controls.Remove(piNext);
                }

                lvPlaylist.MultiSelect = !_compactMode;
                lvPlaylist.ContextMenuStrip = _compactMode ? null : cmsPlaylist;
                lvPlaylist_Resize(this, null); 
            }
        }*/

        public void FocusOnList()
        {
            this.Focus();
            lvPlaylist.Focus();
        }

        public void CopyPlaylist(PlaylistPanel source)
        {
            this.Clear();
            foreach (PlaylistItem pi in source.playlist)
            {
                this.playlist.Add(pi);
                int i = this.playlist.Count - 1;

                playlist_PlaylistUpdated(i, 0, UpdateType.Added);
            }
        }

        public PlaylistPanel() : base()
        {
            InitializeComponent();

            _tmrSavePlaylist = new System.Windows.Forms.Timer();
            _tmrSavePlaylist.Enabled = false;
            _tmrSavePlaylist.Interval = 500;
            _tmrSavePlaylist.Tick += _tmrSavePlaylist_Tick;

           
            _tmrUpdateItemsDesc = new System.Windows.Forms.Timer();
            _tmrUpdateItemsDesc.Interval = 300;
            _tmrUpdateItemsDesc.Tick += OnTimerUpdateItemsDesc;

            ThemeManager.SetFont(lvPlaylist, FontSizes.Normal);
            lvPlaylist.MultiSelect = true;
            lvPlaylist.Items.Clear();

            playlist.PlaylistUpdated += new PlaylistUpdatedEventHandler(playlist_PlaylistUpdated);

            this.HandleDestroyed += new EventHandler(PlaylistPanel_HandleDestroyed);

            if (MainThread.MainWindow != null)
            {
                MainThread.MainWindow.Shown += new EventHandler(MainWindow_Shown);
            }

            UpdateTotalTime(0);

            if (!DesignMode)
            {
                //MediaRenderer.DefaultInstance.MediaRendererHeartbeat += new MediaRendererEventHandler(OnMediaRendererHeartbeat);
                MediaRenderer.DefaultInstance.FilterStateChanged += new FilterStateChangedHandler(OnFilterStateChanged);
            }

            OnThemeUpdatedInternal();

            OnTimerUpdateItemsDesc(null, null);
        }

        void OnFilterStateChanged(FilterState oldState, string oldMedia, FilterState newState, string newMedia)
        {
            try
            {
                ListViewItem lvi = lvPlaylist.Items[playlist.PlayIndex];
                PlaylistItem pli = lvi.Tag as PlaylistItem;
                SetItemRelation(lvi, pli);

                UpdateTotalTime(playlist.TotalPlaylistTime);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                UpdatePlaylistNames(false);
            }
        }

        void MainWindow_Shown(object sender, EventArgs e)
        {
            if (!DesignMode && initialState)
            {
                initialState = false;
                PersistentPlaylist.Load(ref playlist);
                UpdateItemsDesc();
            }
        }

        [EventSink(LocalEventNames.PerformTranslation)]
        public void UpdateLanguage()
        {
            UpdatePlaylistNames(false);
        }

        [EventSink(LocalEventNames.UpdatePlaylistNames)]
        public void UpdatePlaylistNames(bool rebuildFileInfos)
        {
            int idxVisible = -1;

            foreach (ListViewItem lvi in lvPlaylist.Items)
            {
                PlaylistItem plItem = lvi.Tag as PlaylistItem;

                if (rebuildFileInfos)
                {
                    plItem.MediaFileInfo.Rebuild(true);
                }

                if (plItem != null)
                {
                    lvi.SubItems[colFile.Index].Text = plItem.DisplayName;
                }

                if (UpdateMiscIcon(lvi))
                {
                    idxVisible = lvi.Index;
                }

                foreach (ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
                {
                    bool isActive = (lvi.Index == playlist.PlayIndex);
                    lvsi.Font = isActive ? ThemeManager.NormalBoldFont : ThemeManager.NormalFont;
                    lvsi.ForeColor = isActive ? ThemeManager.ListActiveItemColor : ThemeManager.ForeColor;
                }
            }

            if (idxVisible >= 0)
            {
            	// THis generates issue #264
                // TODO: fix #264 while still keep the capability to auto-scroll to the active item...
                //lvPlaylist.EnsureVisible(idxVisible);
            }

            UpdateItemsDesc();
        }

        private void UpdateItemsDesc()
        {
            _tmrUpdateItemsDesc.Stop();
            _tmrUpdateItemsDesc.Start();
        }

        private void OnTimerUpdateItemsDesc(object sender, EventArgs e)
        {
            _tmrUpdateItemsDesc.Stop();
            piPrev.Item = playlist.GetPrevious();
            piNext.Item = playlist.GetNext();
            piCurrent.Item = playlist.ActivePlaylistItem;
        }


        private string _prevActiveItem = null;
        private bool UpdateMiscIcon(ListViewItem lvi)
        {
            PlaylistItem plItem = lvi.Tag as PlaylistItem;

            Image imgMisc = null;
            string txtMisc = string.Empty;

            bool retVal = false;

            if (plItem != null)
            {
                bool isActive = (lvi.Index == playlist.PlayIndex);

                if (plItem.IsVideo)
                {
                    if (SubtitleDownloadProcessor.IsFileOnDownloadList(plItem.Path))
                    {
                        // Currently downloading a subtitle
                        Bitmap bmp = OPMedia.UI.Properties.Resources.hourglass;
                        bmp.MakeTransparent(ThemeManager.TransparentColor);
                        imgMisc = ImageProvider.ScaleImage(bmp, new Size(16, 16), true);
                        txtMisc = Translator.Translate("TXT_SUBTITLE_DOWNLOADING");

                    }
                    else if (SubtitleDownloadProcessor.TestForExistingSubtitle(plItem.Path))
                    {
                        // Already having a subtitle
                        imgMisc = ImageProcessing.Subtitle16;
                        txtMisc = Translator.Translate("TXT_SUBTITLE_AVAILABLE");
                    }
                }
                else if (isActive)
                {
                    Image img = null;
                    switch (MediaRenderer.DefaultInstance.FilterState)
                    {
                        case FilterState.Running:
                            img = Resources.btnPlay;
                            break;

                        case FilterState.Paused:
                            img = Resources.btnPause;
                            break;

                        default:
                            img = null;
                            break;
                    }

                    imgMisc = img.Resize(false);
                }
                else
                {
                    imgMisc = null;
                }

                if (isActive)
                {
                    _prevActiveItem = plItem.Path;
                    retVal = string.Compare(_prevActiveItem, plItem.Path, true) == 0;
                }
            }

            lvi.SubItems[colMisc.Index].Tag = new ExtendedSubItemDetail(imgMisc, txtMisc);
            return retVal;
        }

        internal void Reselect()
        {
            this.Invalidate(true);
            lvPlaylist.Invalidate(true);
        }

        bool _abortLoad = false;

        void PlaylistPanel_HandleDestroyed(object sender, EventArgs e)
        {
            try
            {
                _abortLoad = true;
                PersistentPlaylist.Save(playlist);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        bool initialState = true;
        

        internal List<PlaylistItem> GetPlaylistItems()
        {
            return playlist.AllItems;
        }

        internal void Clear()
        {
            lvPlaylist.Items.Clear();
            playlist.ClearAll();
        }

        internal void AddOnlineContent(List<OnlineMediaItem> onlineContent)
        {
            if (_abortLoad)
                return;

            if (onlineContent != null)
            {
                foreach (OnlineMediaItem omi in onlineContent)
                {
                    if (_abortLoad)
                        break;

                    playlist.AddOnlineMediaItem(omi);
                }
            }
        }

        internal void AddFiles(IEnumerable<string> files)
        {
            if (_abortLoad)
                return;

            if (files != null)
            {
                foreach (string file in files)
                {
                    if (_abortLoad)
                        break;

                    if (DvdMedia.FromPath(file) != null)
                    {
                        playlist.AddItem(file);
                    }
                    else if (File.Exists(file))
                    {
                        if (IsPlaylist(file))
                        {
                            LoadPlaylist(file, true);
                        }
                        else
                        {
                            playlist.AddItem(file);
                        }
                    }
                    else if (Directory.Exists(file))
                    {
                        AddFiles(PathUtils.EnumDirectories(file));
                        AddFiles(PathUtils.EnumFiles(file));
                    }
                    else
                    {
                        playlist.AddItem(file);
                    }
                }
            }
        }

        private bool IsPlaylist(string file)
        {
            try
            {
                Uri uri = new Uri(file);
                string ext = PathUtils.GetExtension(uri.LocalPath);
                return MediaRenderer.SupportedPlaylists.Contains(ext);
            }
            catch
            {
                return false;
            }
        }

        internal PlaylistItem GetFirstItem()
        {
            if (playlist.Count > 0)
            {
                return playlist[0];
            }

            return null;
        }

        internal PlaylistItem GetNextItem()
        {
            PlaylistItem retVal = null;

            if (playlist.MoveNext())
            {
                retVal = GetActiveItem();
            }

            return retVal;
        }

        internal PlaylistItem GetPreviousItem()
        {
            PlaylistItem retVal = null;

            if (playlist.MovePrevious())
            {
                retVal = GetActiveItem();
            }

            return retVal;
        }

        internal PlaylistItem GetActiveItem()
        {
            try
            {
                if (playlist.Count <= 0)
                    return null;
    
                return GetActivePlaylistItem();
            }
            catch { }

            return null;
        }

        internal string GetActiveFileTitle()
        {
            try
            {
                if (playlist.Count <= 0)
                    return null;
    
                return GetActivePlaylistItem().DisplayName;
            }
            catch { }

            return null;
        }

        internal PlaylistItem GetActivePlaylistItem()
        {
            if (playlist.Count <= 0)
                return null;

            return playlist.ActivePlaylistItem;
        }

        internal void SetFirstSelectedPlaylistItem(PlaylistItem plItem)
        {
            if (lvPlaylist.Items.Count > 0)
            {
                lvPlaylist.SelectedItems.Clear();
                foreach (ListViewItem lvItem in lvPlaylist.Items)
                {
                    if (lvItem.Tag == plItem)
                    {
                        lvItem.Selected = true;
                        lvItem.Focused = true;
                        lvPlaylist.EnsureVisible(lvItem.Index);
                        break;
                    }
                }
            }
        }

        internal PlaylistItem GetFirstSelectedPlaylistItem()
        {
            if (lvPlaylist.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvItem in lvPlaylist.SelectedItems)
                {
                    return lvItem.Tag as PlaylistItem;
                }
            }

            return null;
        }

        internal void Delete()
        {
            List<PlaylistItem> itemsToDelete = new List<PlaylistItem>();
            if (lvPlaylist.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvItem in lvPlaylist.SelectedItems)
                {
                    PlaylistItem itemToDelete = lvItem.Tag as PlaylistItem;
                    if (itemToDelete != null)
                    {
                        Logger.LogTrace("Requested to Delete an item from the playlist, index="
                            + lvItem.Index);
                        itemsToDelete.Add(itemToDelete);
                    }
                }
            }

            if (itemsToDelete.Count > 0)
            {
                playlist.RemoveItems(itemsToDelete);
            }

            Logger.LogTrace("Requested to Delete but no item selected -- requested igonred.");
        }

        internal void MoveDown()
        {
            List<int> selectedIndexes = new List<int>();
            List<int> selectedIndexesAfter = new List<int>();

            foreach (ListViewItem row in lvPlaylist.SelectedItems)
            {
                int i = row.Index;
                selectedIndexes.Add(i);
                selectedIndexesAfter.Add(i + 1);
            }

            if (playlist.ShiftItems(selectedIndexes, false))
            {
                foreach (ListViewItem item in lvPlaylist.Items)
                {
                    item.Selected = selectedIndexesAfter.Contains(item.Index);
                }
            }
        }

        internal void MoveUp()
        {
            List<int> selectedIndexes = new List<int>();
            List<int> selectedIndexesAfter = new List<int>();

            foreach (ListViewItem row in lvPlaylist.SelectedItems)
            {
                int i = row.Index;
                selectedIndexes.Add(i);
                selectedIndexesAfter.Add(i - 1);
            }

            if (playlist.ShiftItems(selectedIndexes, true))
            {
                foreach (ListViewItem item in lvPlaylist.Items)
                {
                    item.Selected = selectedIndexesAfter.Contains(item.Index);
                }
            }
        }

        internal void SavePlaylist()
        {
            string filter = string.Empty;

            filter += MediaRenderer.DefaultInstance.PlaylistsFilter;
            filter += Translator.Translate("TXT_ALL_FILES_FILTER");
            filter = filter.Replace("TXT_PLAYLISTS", Translator.Translate("TXT_PLAYLISTS"));

            OPMSaveFileDialog dlg = new OPMSaveFileDialog();
            dlg.Title = Translator.Translate("TXT_SAVEPLAYLIST");
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

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProTONEConfig.PL_LastFilterIndex = dlg.FilterIndex;

                playlist.SavePlaylist(dlg.FileName);

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

        public void LoadPlaylist(string file, bool enqueue)
        {
            if (!enqueue)
            {
                Clear();
            }

            Uri uri = new Uri(file);
            if (uri.Scheme == "file")
            {
                playlist.LoadPlaylist(file);
            }
            else
            {
                string fileName = Path.GetFileName(uri.LocalPath);
                string tempFile = Path.Combine(Path.GetTempPath(), fileName);

                using (WebClient wc = new WebClient())
                {
                    wc.Proxy = AppConfig.GetWebProxy();
                    wc.DownloadFile(uri, tempFile);
                    playlist.LoadPlaylist(tempFile);
                }
            }
        }

        internal void LoadPlaylist()
        {
            string filter = string.Empty;

            filter += MediaRenderer.DefaultInstance.PlaylistsFilter;
            filter += Translator.Translate("TXT_ALL_FILES_FILTER");
            filter = filter.Replace("TXT_PLAYLISTS", Translator.Translate("TXT_PLAYLISTS"));

            OPMOpenFileDialog dlg = new OPMOpenFileDialog();
            dlg.Multiselect = true;
            dlg.Title = Translator.Translate("TXT_LOADPLAYLIST");
            dlg.Filter = filter;
            dlg.FilterIndex = ProTONEConfig.PL_LastFilterIndex;
            dlg.InitialDirectory = ProTONEConfig.PL_LastOpenedFolder;

            dlg.InheritAppIcon = false;
            dlg.Icon = OPMedia.UI.Properties.Resources.Open16.ToIcon();

            dlg.FillFavoriteFoldersEvt += () => { return ProTONEConfig.GetFavoriteFolders("FavoriteFolders"); };
            dlg.AddToFavoriteFolders += (s) => { return ProTONEConfig.AddToFavoriteFolders(s); };
            dlg.ShowAddToFavorites = true;

            dlg.OpenDropDownOptions = new List<OpenOption>(new OpenOption[]
            {
                new MediaPlayerOpenOption(CommandType.PlayFiles),
                new MediaPlayerOpenOption(CommandType.EnqueueFiles)
            });

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProTONEConfig.PL_LastFilterIndex = dlg.FilterIndex;

                CommandType openOption = CommandType.PlayFiles;
                try
                {
                    openOption = (CommandType)dlg.OpenOption;
                }
                catch
                {
                    openOption = CommandType.PlayFiles;
                }

                bool doEnqueue = (openOption == CommandType.EnqueueFiles);

                if (doEnqueue)
                {
                    Playlist pl = new Playlist();
                    pl.LoadPlaylist(dlg.FileName);

                    if (pl.AllItems != null)
                        pl.AllItems.ForEach(pli => playlist.AddItem(pli));
                }
                else
                {
                    Clear();
                    playlist.LoadPlaylist(dlg.FileName);
                }
                
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

        void playlist_PlaylistUpdated(int item1, int item2, UpdateType updateType)
        {
            try
            {
                switch (updateType)
                {
                    case UpdateType.Added:
                        {
                            PlaylistItem plItem = playlist[item1];

                            ListViewItem item = new ListViewItem(new string[] { "", "", "", "", "" });
                            lvPlaylist.Items.Add(item);
                            SetItemRelation(item, plItem);
                        }
                        break;

                    case UpdateType.Removed:
                        if (item1 >= 0 && item1 < lvPlaylist.Items.Count)
                        {
                            lvPlaylist.Items.RemoveAt(item1);
                            if (item1 < lvPlaylist.Items.Count)
                            {
                                lvPlaylist.Items[item1].Selected = true;
                            }
                        }
                        break;

                    case UpdateType.Swapped:
                        if (item1 >= 0 && item1 < lvPlaylist.Items.Count &&
                            item2 >= 0 && item2 < lvPlaylist.Items.Count)
                        {
                            ListViewItem lvItem1 = lvPlaylist.Items[item1];
                            ListViewItem lvItem2 = lvPlaylist.Items[item2];

                            PlaylistItem plItem1 = lvItem1.Tag as PlaylistItem;
                            PlaylistItem plItem2 = lvItem2.Tag as PlaylistItem;

                            SetItemRelation(lvItem1, plItem2);
                            SetItemRelation(lvItem2, plItem1);

                            lvItem1.Selected = false;
                            lvItem2.Selected = true;
                        }
                        break;

                    case UpdateType.Shuffled:
                        lvPlaylist.Items.Clear();
                        foreach (PlaylistItem plItem in playlist)
                        {
                            ListViewItem item = new ListViewItem(new string[] { "", "", "", "", "" });
                            lvPlaylist.Items.Add(item);
                            SetItemRelation(item, plItem);
                        }
                        break;
                }

                UpdateTotalTime(playlist.TotalPlaylistTime);
                UpdateItemsDesc();
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void _tmrSavePlaylist_Tick(object sender, EventArgs e)
        {
            _tmrSavePlaylist.Stop();
            PersistentPlaylist.Save(playlist);
        }

        private void SetItemRelation(ListViewItem lvItem, PlaylistItem plItem)
        {
            if (lvItem != null && plItem != null)
            {
                lvItem.Tag = plItem;
                lvItem.SubItems[colFile.Index].Text = plItem.DisplayName;

                TimeSpan duration = plItem.Duration;
                bool isActive = (lvItem.Index == playlist.PlayIndex);

                if (duration.TotalMilliseconds == 0 && isActive)
                {
                    try
                    {
                        duration = TimeSpan.FromSeconds((int)MediaRenderer.DefaultInstance.MediaLength);
                    }
                    catch
                    {
                        duration = TimeSpan.FromMilliseconds(0);
                    }

                    plItem.Duration = duration;
                }

                int totalSeconds = (int)(Math.Max(0, duration.TotalSeconds));
                TimeSpan representableDuration = TimeSpan.FromSeconds(totalSeconds);

                if (representableDuration.TotalMilliseconds == 0)
                {
                    lvItem.SubItems[colTime.Index].Text = "";
                }
                else
                {
                    lvItem.SubItems[colTime.Index].Text = duration.ToString();
                }

                lvItem.SubItems[colIcon.Index].Tag = new ExtendedSubItemDetail(plItem.GetImageEx(false), null);

                UpdateMiscIcon(lvItem);
                foreach (ListViewItem.ListViewSubItem lvsi in lvItem.SubItems)
                {
                    lvsi.Font = isActive ? ThemeManager.NormalBoldFont : ThemeManager.NormalFont;
                    lvsi.ForeColor = isActive ? ThemeManager.ListActiveItemColor : ThemeManager.ForeColor;
                }
            }
        }

        void lvPlaylist_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (SelectedItemChanged != null)
            {
                if (lvPlaylist.SelectedItems.Count > 0)
                    SelectedItemChanged(lvPlaylist.SelectedItems[0].Tag as PlaylistItem);
                else
                    SelectedItemChanged(null);
            }
        }

        private void lvPlaylist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&
                lvPlaylist.SelectedItems.Count > 0)
            {
                int sel = lvPlaylist.SelectedItems[0].Index;
                if (playlist.MoveToItem(sel) && LaunchFile != null)
                {
                    LaunchFile(GetActiveItem());
                }
            }
        }

        internal int GetFileCount()
        {
            return playlist.Count;
        }

        private void lvPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            OnDragDrop(e);
        }

        private void lvPlaylist_DragEnter(object sender, DragEventArgs e)
        {
            OnDragEnter(e);
        }

        private void lvPlaylist_DragLeave(object sender, EventArgs e)
        {
            OnDragLeave(e);
        }

        private void lvPlaylist_DragOver(object sender, DragEventArgs e)
        {
            OnDragOver(e);
        }

        protected override void OnThemeUpdatedInternal()
        {
            var c = ThemeManager.BorderColor;
            lblSep1.BackColor = c;
            lblSep2.BackColor = c;
            lblSep3.BackColor = c;
            lblSep4.BackColor = c;
            lblSep5.BackColor = c;

            base.OnThemeUpdatedInternal();
        }

        public void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (args.Handled)
                return;

            bool refreshButtonState = false;

            switch (args.cmd)
            {
                case OPMShortcut.CmdClear:
                    Clear();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdDelete:
                    Delete();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdLoadPlaylist:
                    LoadPlaylist();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdSavePlaylist:
                    SavePlaylist();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdMoveDown:
                    MoveDown();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdMoveUp:
                    MoveUp();
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdJumpToItem:
                    HandleJumpToItem();
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdToggleShuffle:
                    playlist.ShufflePlaylist();
                    args.Handled = true;
                    refreshButtonState = true;
                    break;

                case OPMShortcut.CmdLoopPlay:
                    ProTONEConfig.LoopPlay ^= true;
                    args.Handled = true;
                    refreshButtonState = true;
                    break;

                case OPMShortcut.CmdXFade:
                    ProTONEConfig.XFade ^= true;
                    args.Handled = true;
                    refreshButtonState = true;
                    break;

                case OPMShortcut.CmdPlaylistEnd:
                    SystemScheduler.PlaylistEventEnabled ^= true;
                    refreshButtonState = true;
                    args.Handled = true;
                    break;
            }

            if (refreshButtonState)
            {
                EventDispatch.DispatchEvent(LocalEventNames.UpdateStateButtons);
            }
        }

        private void HandleJumpToItem()
        {
            JumpToItemDlg dlg = new JumpToItemDlg(playlist);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.SelectedItem != null)
                {
                    JumpToPlaylistItem(dlg.SelectedItem);
                    LaunchFile(GetActiveItem());
                }
            }
        }

        //private void UpdateButtonsState()
        //{
        //    bool singleItemSelected = (lvPlaylist.SelectedItems.Count == 1);
        //    bool itemsSelected = (lvPlaylist.SelectedItems.Count > 0);
        //    bool itemsPresent = (lvPlaylist.Items.Count > 0);

        //    if (singleItemSelected)
        //    {
        //        PlaylistItem plItem = lvPlaylist.SelectedItems[0].Tag as PlaylistItem;
        //        if (plItem != null)
        //        {
        //            bookmarkManagerCtl.PlaylistItem = plItem;
        //        }
        //    }
        //}

        //private void lvPlaylist_SelectionChanged(object sender, EventArgs e)
        //{
        //    bookmarkManagerCtl.PlaylistItem = null;
        //    UpdateButtonsState();
        //}

        internal bool JumpToPlaylistItem(PlaylistItem plItem)
        {
            return playlist.MoveToItem(plItem);
        }

        //internal bool IsActiveItem(PlaylistItem plItem)
        //{
        //    string renderFile = MediaRenderer.DefaultInstance.GetRenderFile();
        //    bool isActive = string.Compare(renderFile, plItem.Path, true) == 0;

        //    return isActive;
        //}

        private void lvPlaylist_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
            }
        }

        public PlaylistItem GetPlaylistItemForEditing()
        {
            if (lvPlaylist.SelectedItems != null && lvPlaylist.SelectedItems.Count > 0)
            {
                ListViewItem selItem = lvPlaylist.SelectedItems[0];
                if (selItem != null)
                {
                    return selItem.Tag as PlaylistItem;
                }
            }

            return null;
        }

        private void cmsPlaylist_Opening(object sender, CancelEventArgs e)
        {
            cmsPlaylist.Items.Clear();

            PlaylistItem plItem = GetPlaylistItemForEditing();

            new MenuBuilder<ContextMenuStrip>(this).AttachPlaylistItemMenu(plItem,
                    new MenuWrapper<ContextMenuStrip>(cmsPlaylist),
                    (lvPlaylist.SelectedItems.Count == 1) ? MenuType.SingleItem : MenuType.MultipleItems,
                    PlaylistItemMenuClick);
        }

        private void lvPlaylist_Resize(object sender, EventArgs e)
        {
            colDummy.Width = 0;
            colIcon.Width = 20;
            colMisc.Width = 20;
            colTime.Width = 55;
            colFile.Width = lvPlaylist.EffectiveWidth - colIcon.Width - colMisc.Width - colTime.Width;
        }

        private void UpdateTotalTime(double totalSeconds)
        {
            TimeSpan ts = TimeSpan.FromSeconds((int)totalSeconds);
            int h = ts.Days * 24 + ts.Hours;

            piTotal.AltDisplay = string.Format("Total: {0}:{1:d2}:{2:d2}",
                h, ts.Minutes, ts.Seconds);

            if (TotalTimeChanged != null)
            {
                TotalTimeChanged(TimeSpan.FromSeconds((int)totalSeconds));
            }

        }

        internal void AddToDeezerPlaylist(bool addToExisting)
        {
            var deezerPlaylistItems = from pli in playlist.AllItems
                                      where pli is DeezerTrackPlaylistItem
                                      select (pli as DeezerTrackPlaylistItem);

            if (deezerPlaylistItems != null)
            {
                var list = deezerPlaylistItems.ToList();
                if (list.Count > 0)
                    AddToDeezerPlaylist(list, FindForm(), addToExisting);
            }

        }

        [EventSink(LocalEventNames.AddToDeezerPlaylist)]
        public void AddToDeezerPlaylist(List<DeezerTrackPlaylistItem> plItems, Form parent, bool addToExisting)
        {
            if (ProTONEConfig.DeezerHasValidConfig)
            {
                string userAccessToken = ProTONEConfig.DeezerUserAccessToken;
                string applicationId = ProTONEConfig.DeezerApplicationId;
                string deezerApiEndpoint = ProTONEConfig.DeezerApiEndpoint;

                DeezerInterop.RestApi.DeezerRuntime dzr = new DeezerInterop.RestApi.DeezerRuntime(deezerApiEndpoint, userAccessToken, applicationId);
                if (dzr != null)
                {
                    ManualResetEvent abortEvent = new ManualResetEvent(false);

                    var playlists = OnlineContentSearcher.GetMyPlaylists(OnlineMediaSource.Deezer, abortEvent);

                    string playlistName = ChooseDeeezerPlaylistName(playlists, addToExisting);
                    if (string.IsNullOrEmpty(playlistName) == false)
                    {
                        if (addToExisting)
                        {
                        }
                        else
                        {
                            // Create the new playlist
                            UInt64 playlistId = dzr.CreatePlaylist(userAccessToken, playlistName, abortEvent);
                            if (playlistId > 0)
                                dzr.AddToPlaylist(userAccessToken, playlistId, null, abortEvent);
                        }
                    }
                }
            }
        }

        private string ChooseDeeezerPlaylistName(List<OnlinePlaylist> playlist, bool addToExisting)
        {
            return "";
        }
    }
}
