using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

using OPMedia.UI.Controls;
using OPMedia.UI.ProTONE.Controls.MediaPlayer;
using OPMedia.Runtime.ProTONE;
using System.Diagnostics;
using OPMedia.Core.Logging;

using System.IO;
using OPMedia.Core.TranslationSupport;
using System.Threading;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Core;
using OPMedia.Runtime.Shortcuts;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.UI.Themes;
using OPMedia.Core.Configuration;

using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.UI.ProTONE.Dialogs;
using OPMedia.UI.ProTONE.Configuration;

using OPMedia.UI.Configuration;
using OPMedia.Runtime.ProTONE.ExtendedInfo;

using OPMedia.UI.ProTONE.SubtitleDownload;
using OPMedia.Runtime;
using System.Net.NetworkInformation;
using OPMedia.Runtime.ProTONE.SubtitleDownload;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Menus;
using OPMedia.Runtime.ProTONE.FfdShowApi;
using OPMedia.UI.Controls.Dialogs;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System.Net;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.RemoteControl;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.Dialogs;


namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public delegate void NotifyMediaStateChangedHandler(bool isVideoFile);

    public partial class MediaPlayer : OPMBaseControl
    {
        public event NotifyMediaStateChangedHandler NotifyMediaStateChanged = null;

        private ContextMenuStrip _menuRendering = null;

        private OnlineContentBrowser _onlineContentBrowser = null;

        #region Members
        const int PanelOffset = 20;

        public bool compactView = false;
        
        //string playedFileTitle = string.Empty;
        int playlistWidth = 0;
        #endregion

        public bool CompactView
        {
            set 
            { 
                compactView = value; 
                DoLayout();  
            }
            get { return compactView; }
        }

        public string PlayedFileTitle
        {
            get 
            {
                if (RenderingEngine.DefaultInstance.IsStreamedMedia)
                {
                    if (RenderingEngine.DefaultInstance.StreamData != null &&
                        RenderingEngine.DefaultInstance.StreamData.ContainsKey("TXT_TITLE"))
                    {
                        return RenderingEngine.DefaultInstance.StreamData["TXT_TITLE"] ?? string.Empty;
                    }
                }

                return playlist.GetActiveFileTitle();
            }
        }

        #region Public methods
        

        public void StopPlayback()
        {
            Stop(true);
        }

        public void PlayFiles(string[] fileNames)
        {
            LoadFiles(fileNames);
        }

        public void EnqueueFiles(string[] fileNames)
        {
            int initialCount = playlist.GetFileCount();
            playlist.AddFiles(fileNames);

            if (initialCount < 1)
                PlayFile(playlist.GetActiveItem(), null);
        }

        public void ClearPlaylist()
        {
            playlist.Clear();
        }

        public new void Dispose()
        {
        }

        protected void DoLayout()
        {
            playlist.Visible = !compactView;

            layoutPanel.Controls.Clear();
            if (!compactView)
            {
                layoutPanel.Controls.Add(playlist, 0, 0);
            }
            layoutPanel.Controls.Add(pnlPlayback, 0, layoutPanel.Controls.Count);

            pnlPlayback.CompactView = compactView;
            //canResize = true;

        }
        #endregion

        #region Constructor
        public MediaPlayer() : base()
        {
            InitializeComponent();

            pnlPlayback.TimeScaleEnabled = false;

            pnlPlayback.PositionChanged += 
                new ValueChangedEventHandler(pnlRendering_PositionChanged);
            pnlPlayback.VolumeChanged += 
                new ValueChangedEventHandler(pnlRendering_VolumeChanged);

            this.MouseWheel += new MouseEventHandler(MediaPlayer_MouseWheel);
            this.HandleCreated += new EventHandler(MediaPlayer_HandleCreated);
            this.HandleDestroyed += new EventHandler(MediaPlayer_HandleDestroyed);

            playlist.LaunchFile += new LaunchFileEventHandler(pnlPlaylist_LaunchFile);
            playlist.PlaylistItemMenuClick += new EventHandler(HandlePlaylistItemMenuClick);

            if (!DesignMode)
            {
                RenderingEngine.DefaultInstance.FilterStateChanged += new FilterStateChangedHandler(OnMediaStateChanged);
                RenderingEngine.DefaultInstance.MediaRendererHeartbeat += new MediaRendererEventHandler(OnMediaRendererHeartbeat);
                RenderingEngine.DefaultInstance.MediaRenderingException += new MediaRenderingExceptionHandler(OnMediaRenderingException);
            }
        }

        [EventSink(LocalEventNames.JumpToBookmark)]
        public void OnJumpToBookmark(BookmarkSubItem subItem)
        {
            JumpToPlaylistSubItem(subItem);
        }
        #endregion

        #region Event handlers

        #region Drag-and-drop events and related

        #region DragEnter
        private void pnlPlaylist_DragEnter(object sender, DragEventArgs e)
        {
            pnlRendering_DragEnter(sender, e);
        }

        private void pnlRendering_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!CompactView)
            {
                string[] droppedFiles = GetRelevantDragDropData(e);
                if (droppedFiles != null)
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }
        #endregion

        #region DragLeave
        private void pnlPlaylist_DragLeave(object sender, EventArgs e)
        {
            pnlRendering_DragLeave(sender, e);
        }

        private void pnlRendering_DragLeave(object sender, EventArgs e)
        {
            // Do nothing
        }
        #endregion

        #region DragOver
        private void pnlPlaylist_DragOver(object sender, DragEventArgs e)
        {
            pnlRendering_DragOver(sender, e);
        }

        private void pnlRendering_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!CompactView)
            {
                string[] droppedFiles = GetRelevantDragDropData(e);
                if (droppedFiles != null)
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }
        #endregion

        #region DragDrop
        private void pnlPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!CompactView)
            {
                string[] droppedFiles = GetRelevantDragDropData(e);
                if (droppedFiles != null)
                {
                    int initialCount = playlist.GetFileCount();

                    e.Effect = DragDropEffects.Move;

                    playlist.AddFiles(droppedFiles);

                    if (initialCount < 1)
                        PlayFile(playlist.GetFirstItem(), null);
                }
            }
        }

        private void pnlRendering_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!CompactView)
            {
                string[] droppedFiles = GetRelevantDragDropData(e);
                if (droppedFiles != null)
                {
                    e.Effect = DragDropEffects.Move;
                    LoadFiles(droppedFiles);
                }
            }
        }
        #endregion

        #endregion

        void MediaPlayer_HandleDestroyed(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RenderingEngine.DefaultInstance.FilterStateChanged -= new FilterStateChangedHandler(OnMediaStateChanged);
                RenderingEngine.DefaultInstance.MediaRendererHeartbeat -= new MediaRendererEventHandler(OnMediaRendererHeartbeat);
                RenderingEngine.DefaultInstance.Dispose();
            }
        }

        void MediaPlayer_HandleCreated(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                pnlPlayback.ProjectedVolume = ProTONEConfig.LastVolume;
                SetVolume(pnlPlayback.ProjectedVolume);
            }
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            playlistWidth = playlist.Width;
        }

        void pnlPlaylist_LaunchFile(PlaylistItem path)
        {
            PlayFile(path, null);
        }

        void MediaPlayer_MouseWheel(object sender, MouseEventArgs e)
        {
            pnlPlayback.ProjectedVolume += e.Delta;
            SetVolume(pnlPlayback.ProjectedVolume);
        }

        void pnlRendering_PositionChanged(double newVal)
        {
            if (RenderingEngine.DefaultInstance.FilterState != FilterState.Stopped)
            {
                if (RenderingEngine.DefaultInstance.FilterState != FilterState.Paused)
                {
                    RenderingEngine.DefaultInstance.PauseRenderer();
                }
                
                RenderingEngine.DefaultInstance.ResumeRenderer(newVal);

                NotifyGUI("TXT_OSD_SEEKTO", TimeSpan.FromSeconds((int)newVal));
            }
        }

        public void NotifyGUI(string format, params object[] args)
        {
            if (RenderingEngine.DefaultInstance.HasRenderingErrors == false)
            {
               string text = Translator.Translate(format, args);
   
               RenderingEngine.DefaultInstance.DisplayOsdMessage(text);
   
               if (ProTONEConfig.MediaStateNotificationsEnabled)
               {
                   EventDispatch.DispatchEvent(EventNames.ShowTrayMessage, text, Translator.Translate("TXT_APP_NAME"), 0);
               }
           }
        }

        void pnlRendering_VolumeChanged(double newVal)
        {
            SetVolume(newVal);
        }

        void OnMediaRenderingException(RenderingExceptionEventArgs args)
        {
            string msg = args.RenderingException.ToString();
            string desc = this.playlist.GetActiveItem().ToString();
            ErrorDispatcher.DispatchError(msg.Replace("__DESC__", desc), false);
            args.Handled = true;
        }

        private void OnMediaStateChanged(FilterState oldState, string oldMedia, 
            FilterState newState, string newMedia)
        {
            OnMediaRendererHeartbeat();

            PlaylistItem pli = playlist.GetActivePlaylistItem();
            pnlPlayback.FilterStateChanged(newState, pli, RenderingEngine.DefaultInstance.RenderedMediaType);

            bool playerNotActive = (newState == FilterState.NotOpened || newState == FilterState.Stopped);
            if (playerNotActive && playlist.GetFileCount() >= 1)
            {
                RenderingEngine.DefaultInstance.PlaylistAtEnd = playlist.IsPlaylistAtEnd;

                if (RenderingEngine.DefaultInstance.IsStopFromGui == false)
                    MoveNext();
            }
            else
            {
                RenderingEngine.DefaultInstance.PlaylistAtEnd = false;
            }
        }

        private void OnMediaRendererHeartbeat()
        {
            if (pnlPlayback.ProjectedVolume != ProTONEConfig.LastVolume)
            {
                pnlPlayback.ProjectedVolume = ProTONEConfig.LastVolume;
            }

            pnlPlayback.ElapsedSeconds = (int)(RenderingEngine.DefaultInstance.MediaPosition);
            pnlPlayback.TotalSeconds = (int)(RenderingEngine.DefaultInstance.MediaLength);
            pnlPlayback.EffectiveSeconds = (int)(RenderingEngine.DefaultInstance.EffectiveMediaLength);

            pnlPlayback.TimeScaleEnabled = RenderingEngine.DefaultInstance.CanSeekMedia &&
                (RenderingEngine.DefaultInstance.FilterState == FilterState.Running || 
                RenderingEngine.DefaultInstance.FilterState == FilterState.Paused);

            pnlPlayback.VolumeScaleEnabled = (RenderingEngine.DefaultInstance.RenderedMediaType != MediaTypes.Video);
            
            if (_renderingFrame != null)
            {
                _renderingFrame.SetTitle(BuildTitle());
            }

            try
            {
                if (RenderingEngine.DefaultInstance.FilterState == FilterState.Running)
                {
                    Bookmark bmk = RenderingEngine.DefaultInstance.RenderedMediaInfo.GetNearestBookmarkInRange(
                        (int)RenderingEngine.DefaultInstance.MediaPosition, 1);

                    if (bmk != null)
                    {
                        Logger.LogTrace("Display Bookmark: " + bmk.ToString());
                        RenderingEngine.DefaultInstance.DisplayOsdMessage(bmk.Title.Replace(";", "\r\n"));
                    }
                }
            }
            catch
            {
            }
        }

        public string BuildTitle()
        {
            string title = Translator.Translate("TXT_APP_NAME");
            try
            {
                if (this.PlayedFileTitle?.Length > 0)
                {
                    title = string.Format("{1} - [{2}] - {0}",
                        Translator.Translate("TXT_APP_NAME"),
                        this.PlayedFileTitle, RenderingEngine.DefaultInstance.TranslatedFilterState);
                }
                else
                {
                    title = Translator.Translate("TXT_APP_NAME");
                }
            }
            catch
            {
            }

            return title;
        }
        #endregion

        #region Implementation
        private void Stop(bool isStopFromGui)
        {
            if (RenderingEngine.DefaultInstance.FilterState != FilterState.Stopped)
            {
                RenderingEngine.DefaultInstance.StopRenderer(isStopFromGui);

                if (isStopFromGui)
                {
                    HideRenderingRegion();
                }
            }
        }

        private void Play()
        {
            var fileToPlay = playlist.GetActiveItem();
            PlayFile(fileToPlay, null);
        }

        private void Pause()
        {
            switch (RenderingEngine.DefaultInstance.FilterState)
            {
                case FilterState.Running:
                    {
                        NotifyGUI("TXT_OSD_PAUSED");

                        // OSD text can be updated via FFDShow only while playing, never while paused.
                        // So we need a small time before pausing, to allow OSD to show "paused"
                        // Then we can effectively pause
                        Thread.Sleep(200);

                        RenderingEngine.DefaultInstance.PauseRenderer();
                    }
                    break;

                case FilterState.Paused:
                    {
                        RenderingEngine.DefaultInstance.ResumeRenderer(RenderingEngine.DefaultInstance.MediaPosition);

                        // Keep this action to execute AFTER resuming playback
                        // OSD text can be updated via FFDShow only while playing, never while paused.
                        NotifyGUI("TXT_OSD_SEEKTO",
                            TimeSpan.FromSeconds((int)RenderingEngine.DefaultInstance.MediaPosition));
                    }
                    break;
            }
        }

        private void MoveNext()
        {
            var strFile = playlist.GetNextItem();
            PlayFile(strFile, null);
        }

        private void MovePrevious()
        {
            var strFile = playlist.GetPreviousItem();
            PlayFile(strFile, null);
        }

        private void LoadDisc()
        {
            if (VideoDVDHelpers.IsOSSupported)
            {
                OPMFolderBrowserDialog dlg = new OPMFolderBrowserDialog("TXT_SELECT_DVD");
                dlg.ShowNewFolderButton = false;
                dlg.Description = Translator.Translate("TXT_LOAD_DVD_FOLDER");

                dlg.PerformPathValidation += (p) => 
                {
                    var dvdInfo = DvdMedia.FromPath(p);
                    return (dvdInfo != null);
                };

                dlg.InheritAppIcon = false;
                dlg.Icon = OPMedia.Core.Properties.Resources.DVD.ToIcon();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var dvd = DvdMedia.FromPath(dlg.SelectedPath);
                    if (dvd != null)
                    {
                        playlist.Clear();
                        playlist.AddFiles(new string[] { dvd.DvdPath });

                        PlayFile(playlist.GetFirstItem(), null);
                    }
                    else
                    {
                        MessageDisplay.Show(Translator.Translate("TXT_INVALIDDVDVOLUME"),
                            Translator.Translate("TXT_ERROR"), MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void LoadFiles()
        {
            OPMOpenFileDialog dlg = new OPMOpenFileDialog();
            dlg.Title = Translator.Translate("TXT_LOADMEDIAFILES");
            dlg.Multiselect = true;

            dlg.InheritAppIcon = false;
            dlg.Icon = Resources.btnLoad.ToIcon((uint)Color.White.ToArgb());

            string filter = string.Empty;

            filter += RenderingEngine.DefaultInstance.AvailableFileTypesFilter;
            filter += Translator.Translate("TXT_ALL_FILES_FILTER");

            filter = filter.Replace("TXT_AUDIO_FILES", Translator.Translate("TXT_AUDIO_FILES"));
            filter = filter.Replace("TXT_VIDEO_FILES", Translator.Translate("TXT_VIDEO_FILES"));
            filter = filter.Replace("TXT_VIDEO_HD_FILES", Translator.Translate("TXT_VIDEO_HD_FILES"));
            filter = filter.Replace("TXT_PLAYLISTS", Translator.Translate("TXT_PLAYLISTS"));
            
            dlg.Filter = filter;

            dlg.FilterIndex = ProTONEConfig.LastFilterIndex;
            dlg.InitialDirectory = ProTONEConfig.LastOpenedFolder;

            dlg.FillFavoriteFoldersEvt += () => { return ProTONEConfig.GetFavoriteFolders("FavoriteFolders"); };
            dlg.AddToFavoriteFolders += (s) => { return ProTONEConfig.AddToFavoriteFolders(s); };

            dlg.QueryDisplayName += (fsi) =>
                {
                    if (fsi != null)
                    {
                        if (fsi.ToUpperInvariant().EndsWith("CDA"))
                        {
                            CDAFileInfo cdfi = MediaFileInfo.FromPath(fsi) as CDAFileInfo;
                            if (cdfi != null)
                                return cdfi.DisplayName;
                        }

                        return Path.GetFileName(fsi);
                    }

                    return string.Empty;
                };

            dlg.ShowAddToFavorites = true;

            dlg.OpenDropDownOptions = new List<OpenOption>(new OpenOption[]
            {
                new MediaPlayerOpenOption(CommandType.PlayFiles),
                new MediaPlayerOpenOption(CommandType.EnqueueFiles)
            });

            if (dlg.ShowDialog() == DialogResult.OK && dlg.FileNames.Length > 0)
            {
                CommandType openOption = CommandType.PlayFiles;
                try
                {
                    openOption = (CommandType)dlg.OpenOption;
                }
                catch
                {
                    openOption = CommandType.PlayFiles;
                }

                if (openOption == CommandType.EnqueueFiles)
                    EnqueueFiles(dlg.FileNames);
                else
                    LoadFiles(dlg.FileNames);

                ProTONEConfig.LastFilterIndex = dlg.FilterIndex;

                try
                {
                    string file = dlg.FileNames[0];
                    ProTONEConfig.LastOpenedFolder = Path.GetDirectoryName(file);
                }
                catch
                {
                    ProTONEConfig.LastOpenedFolder = dlg.InitialDirectory;
                }
            }
        }

        private void LoadFiles(string[] fileNames)
        {
            playlist.Clear();
            playlist.AddFiles(fileNames);

            PlayFile(playlist.GetFirstItem(), null);
        }

        internal void PlayFile(PlaylistItem pli, PlaylistSubItem subItem)
        {
            if (pli == null)
                return;

            string strFile = pli.Path;
            int delayStart = pli.DelayStart;

            if (!string.IsNullOrEmpty(strFile))
            {
                if (subItem == null && RenderingEngine.DefaultInstance.FilterState != FilterState.Stopped)
                {
                    Stop(false);
                }

                bool isVideoFile = false;
                bool isDVDVolume = false;

                string name = string.Empty;

                DvdMedia dvdDrive = DvdMedia.FromPath(strFile);
                if (dvdDrive != null)
                {
                    isDVDVolume = true;
                    name = dvdDrive.ToString();

                    if (subItem != null && subItem.StartHint != null)
                    {
                        name += Translator.Translate("TXT_PLAY_FROM", subItem.StartHint);
                    }
                }
                else
                {
                    MediaFileInfo mfi = MediaFileInfo.FromPath(strFile);
                    isVideoFile = SupportedFileProvider.Instance.SupportedVideoTypes.Contains(mfi.MediaType);
                    
                    if (pli != null)
                        name = pli.DisplayName;
                    else
                        name = mfi.Name;
                }

                if (isDVDVolume)
                    Logger.LogTrace("Played media appears to be a DVD volume ...");
                else if (isVideoFile)
                    Logger.LogTrace("Played media appears to be a video file ...");
                else
                    Logger.LogTrace("Played media appears to be an audio file...");

                if (isVideoFile || isDVDVolume)
                {
                    ShowRenderingRegion();
                }
                else
                {
                    HideRenderingRegion();
                }

                if (NotifyMediaStateChanged != null)
                {
                    NotifyMediaStateChanged(isVideoFile || isDVDVolume);
                }

                RenderingEngine.DefaultInstance.SetRenderFile(strFile);

                bool rendererStarted = false;
                if (subItem != null && subItem.StartHint != null)
                {
                    RenderingEngine.DefaultInstance.StartRendererWithHint(subItem.StartHint);
                    rendererStarted = true;
                }

                if (!rendererStarted)
                {
                    if (delayStart > 0)
                    {
                        Bookmark mark = new Bookmark("DelayStart", delayStart);
                        BookmarkStartHint hint = new BookmarkStartHint(mark);
                        RenderingEngine.DefaultInstance.StartRendererWithHint(hint);
                    }
                    else
                    {
                        RenderingEngine.DefaultInstance.StartRenderer();
                    }
                }

                if (RenderingEngine.DefaultInstance.HasRenderingErrors == false)
                {
                   if (_renderingFrame != null && (isVideoFile || isDVDVolume))
                   {
                       if (!_renderingFrame.Visible)
                       {
                           _renderingFrame.Show();
                       }
                   }
   
                   SetVolume(pnlPlayback.ProjectedVolume);
   
                   if (subItem != null && subItem.StartHint != null)
                   {
                       NotifyGUI("TXT_OSD_PLAY", subItem);
                   }
                   else
                   {
                       NotifyGUI("TXT_OSD_PLAY", name);
                   }
   
                   if (isVideoFile)
                   {
                       if (_delayedSubtitleLookupTimer == null)
                       {
                           _delayedSubtitleLookupTimer = new System.Windows.Forms.Timer();
                           _delayedSubtitleLookupTimer.Interval = 1000;
                           _delayedSubtitleLookupTimer.Tick += new EventHandler(_delayedSubtitleLookupTimer_Tick);
                       }
   
                       _delayedSubtitleLookupTimer.Start();
                   }
                }
                else
                {
                    HideRenderingRegion();
                }
            }
        }

        void _delayedSubtitleLookupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                SubtitleDownloadProcessor.TryFindSubtitle(RenderingEngine.DefaultInstance.GetRenderFile(),
                            (int)RenderingEngine.DefaultInstance.MediaLength, false);
            }
            catch { }
            finally
            {
                _delayedSubtitleLookupTimer.Stop();
            }
        }

        public System.Windows.Forms.Timer _delayedSubtitleLookupTimer = null;

        public void SetVolume(double volume)
        {
            ProTONEConfig.LastVolume = (int)volume;
            if (RenderingEngine.DefaultInstance.RenderedMediaType != MediaTypes.Video &&
                RenderingEngine.DefaultInstance.FilterState != FilterState.Stopped)
            {
                RenderingEngine.DefaultInstance.AudioVolume = (int)(volume / 100);
                RenderingEngine.DefaultInstance.DisplayOsdMessage(Translator.Translate("TXT_OSD_VOL", (int)(volume / 100)));

                RenderingEngine.DefaultInstance.AudioBalance = ProTONEConfig.LastBalance;
            }

            if (pnlPlayback.ProjectedVolume != ProTONEConfig.LastVolume)
            {
                pnlPlayback.ProjectedVolume = ProTONEConfig.LastVolume;
            }
        }

        [EventSink(EventNames.ExecuteShortcut)]
        public void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (args.Handled)
                return;

            Logger.LogInfo("OnExecuteShortcut enter: " + args);

            if (_renderingFrame == null || !_renderingFrame.Visible)
            {
                playlist.Focus();
            }

            switch (args.cmd)
            {
                case OPMShortcut.CmdPlayPause:
                    if (RenderingEngine.DefaultInstance.FilterState == FilterState.Paused || 
                        RenderingEngine.DefaultInstance.FilterState == FilterState.Running)
                    {
                        Pause();
                        args.Handled = true;
                    }
                    else
                    {
                        Play();
                        args.Handled = true;
                    }
                    break;

                case OPMShortcut.CmdStop:
                    Stop(true);
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdPrev:
                    MovePrevious();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdNext:
                    MoveNext();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdLoad:
                    LoadFiles();
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdFwd:
                    MoveToPosition(pnlPlayback.ElapsedSeconds + 5);
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdRew:
                    MoveToPosition(pnlPlayback.ElapsedSeconds - 5);
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdVolUp:
                    pnlPlayback.ProjectedVolume += 500;
                    SetVolume(pnlPlayback.ProjectedVolume);
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdVolDn:
                    pnlPlayback.ProjectedVolume -= 500;
                    SetVolume(pnlPlayback.ProjectedVolume);
                    args.Handled = true;
                    break;
                case OPMShortcut.CmdFullScreen:
                    ToggleFullScreen();
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdOpenDisk:
                    LoadDisc();
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdCfgAudio:
                    FfdShowConfig.DoConfigureAudio(this.Handle);
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdCfgVideo:
                    FfdShowConfig.DoConfigureVideo(this.Handle);
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdCfgSubtitles:
                    ProTONESettingsForm.Show("TXT_S_SUBTITLESETTINGS", "TXT_S_SUBTITLESETTINGS");
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdCfgTimer:
                    ProTONESettingsForm.Show("TXT_S_MISC_SETTINGS", "TXT_S_SCHEDULERSETTINGS");
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdCfgRemote:
                    ProTONESettingsForm.Show("TXT_S_CONTROL", "TXT_REMOTECONTROLCFG");
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdOpenSettings:
                    DialogResult dlgResult = ProTONESettingsForm.Show();
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdCfgKeyboard:
                    ProTONESettingsForm.Show("TXT_S_CONTROL", "TXT_S_KEYMAP");
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdOpenURL:
                    {
                        args.Handled = true;

                        //StreamingServerChooserDlg dlg2 = new StreamingServerChooserDlg();
                        //dlg2.Show();

                        if (_onlineContentBrowser == null)
                        {
                            _onlineContentBrowser = new OnlineContentBrowser();
                            _onlineContentBrowser.FormClosing += new FormClosingEventHandler(_onlineContentBrowser_FormClosing);
                            _onlineContentBrowser.FormClosed += _onlineContentBrowser_FormClosed;
                            _onlineContentBrowser.Shown += new EventHandler(_onlineContentBrowser_Shown);
                        }

                        _onlineContentBrowser.Show();

                        /*
                        if (dlg2.ShowDialog() == DialogResult.OK)
                        {
                            string textUrl = dlg2.Uri;
                            string radioStationUrl = dlg2.RadioStation != null ? dlg2.RadioStation.Url : string.Empty;

                            if (string.Compare(textUrl, radioStationUrl, true) != 0)
                            {
                                // Load the specified url (manually entered by the user)
                                string[] urls = new string[] { textUrl };
                                LoadFiles(urls);
                            }
                            else if (dlg2.RadioStation != null)
                            {
                                LoadRadioStation(dlg2.RadioStation);
                            }
                        }*/
                    }
                    break;

                case OPMShortcut.CmdSearchSubtitles:
                    {
                        args.Handled = true;

                        PlaylistItem plItem = playlist.GetPlaylistItemForEditing();
                        if (plItem != null &&
                            plItem.MediaFileInfo is VideoFileInfo)
                        {
                            SubtitleDownloadProcessor.TryFindSubtitle(plItem.Path, (int)plItem.Duration.TotalSeconds, true);
                        }
                    }
                    break;

                case OPMShortcut.CmdSignalAnalisys:
                    ShowSignalAnalisysFrame();
                    break;
                    
                default:
                    playlist.OnExecuteShortcut(args);
                    break;
            }

            Logger.LogInfo("OnExecuteShortcut leave");
        }

        void _onlineContentBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            _onlineContentBrowser.SavePosition();
        }

        void _onlineContentBrowser_Shown(object sender, EventArgs e)
        {
            _onlineContentBrowser.RestorePosition();
        }

        private void _onlineContentBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            _onlineContentBrowser = null;
        }

        [EventSink(GlobalEvents.EventNames.LoadOnlineContent)]
        public void LoadOnlineContent(List<OnlineMediaItem> onlineContent, bool doEnqueue)
        {
            if (doEnqueue == false)
                playlist.Clear();

            playlist.AddOnlineContent(onlineContent);

            if (doEnqueue == false)
                PlayFile(playlist.GetFirstItem(), null);
        }

        private void ToggleFullScreen()
        {
            if (RenderingEngine.DefaultInstance.RenderedMediaType == MediaTypes.Video ||
                RenderingEngine.DefaultInstance.RenderedMediaType == MediaTypes.Both &&
                _renderingFrame != null)
            {
                _renderingFrame.ToggleFullScreen();
            }
        }


        public void MoveToPosition(double pos)
        {
            if (RenderingEngine.DefaultInstance.FilterState != FilterState.Stopped && 
                RenderingEngine.DefaultInstance.CanSeekMedia)
            {
                RenderingEngine.DefaultInstance.PauseRenderer();
                RenderingEngine.DefaultInstance.ResumeRenderer(pos);

                NotifyGUI("TXT_OSD_SEEKTO", TimeSpan.FromSeconds((int)pos));
            }
        }

        private string[] GetRelevantDragDropData(DragEventArgs e)
        {
            string[] retVal = null;
            if (e != null)
            {
                e.Effect = DragDropEffects.None;
                if (e.Data != null &&
                    e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] data = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                    List<string> droppedFiles = new List<string>(data);

                    if (droppedFiles.Count > 0)
                    {
                        retVal = droppedFiles.ToArray();
                    }
                }
            }

            return retVal;
        }

        #region Rendering frame

        private RenderingFrame _renderingFrame = null;

        private void ShowRenderingRegion()
        {
            if (_renderingFrame == null)
            {
                CreateRenderingFrame();
            }

            _renderingFrame.SetContextMenuStrip(_menuRendering);

            RenderingEngine.DefaultInstance.RenderPanel = _renderingFrame.RenderingZone;
        }

        private void HideRenderingRegion()
        {
            if (_renderingFrame != null)
            {
                _renderingFrame.Hide();
            }
        }

        private void CreateRenderingFrame()
        {
            GC.Collect();

            _renderingFrame = new RenderingFrame();
            _renderingFrame.HandleDestroyed += new EventHandler(renderFrame_HandleDestroyed);
        }

        void renderFrame_HandleDestroyed(object sender, EventArgs e)
        {
            RenderingEngine.DefaultInstance.StopRenderer(true);
            RenderingEngine.DefaultInstance.RenderPanel = null;
            RenderingEngine.DefaultInstance.MessageDrain = null;
        
            _renderingFrame = null;
            GC.Collect();
        }
        #endregion

        #region Signal Analysis frame

        private SignalAnalisysFrame _signalAnalysisFrame = null;

        private void ShowSignalAnalisysFrame()
        {
            if (_signalAnalysisFrame == null)
            {
                GC.Collect();
                _signalAnalysisFrame = new SignalAnalisysFrame();
                _signalAnalysisFrame.HandleDestroyed += new EventHandler(signalAnalysisFrame_HandleDestroyed);
            }

            _signalAnalysisFrame.Show();
            _signalAnalysisFrame.BringToFront();

        }

        void signalAnalysisFrame_HandleDestroyed(object sender, EventArgs e)
        {
            _signalAnalysisFrame = null;
            GC.Collect();
        }
        #endregion

        #endregion

        public void SetRenderingMenu(ContextMenuStrip renderingMenu)
        {
            _menuRendering = renderingMenu;
            playlist.ContextMenuStrip = renderingMenu;
            pnlPlayback.ContextMenuStrip = renderingMenu;
        }

        public void JumpToPlaylistItem(PlaylistItem plItem)
        {
            if (plItem != null)
            {
                if (playlist.JumpToPlaylistItem(plItem))
                {
                    var strFile = playlist.GetActiveItem();
                    PlayFile(strFile, null);
                }
            }
        }

        public void JumpToPlaylistSubItem(PlaylistSubItem subItem)
        {
            bool doJump = false;

            if (subItem != null && subItem.Parent != null)
            {
                if (subItem.Parent != playlist.GetActivePlaylistItem())
                {
                    if (playlist.JumpToPlaylistItem(subItem.Parent))
                    {
                        Stop(true);
                        doJump = true;
                    }
                } 
                else
                {
                    doJump = true;
                }
                
                if (doJump)
                {
                    var strFile = playlist.GetActiveItem();
                    if (strFile != null)
                    {
                        PlayFile(strFile, subItem);
                    }
                    else
                    {
                        PlayFile(subItem.Parent, subItem);
                    }
                }
            }
        }

        public void HandlePlaylistItemMenuClick(object sender, EventArgs e)
        {
            OPMToolStripMenuItem senderMenu = (sender as OPMToolStripMenuItem);
            if (senderMenu != null)
            {
                try
                {
                    senderMenu.Enabled = false;

                    if (senderMenu.Tag != null)
                    {
                        if (senderMenu.Tag is PlaylistSubItem)
                        {
                            if (senderMenu.Tag is DvdSubItem)
                            {
                                DvdSubItem si = senderMenu.Tag as DvdSubItem;
                                DvdRenderingStartHint hint =
                                    (si != null) ?
                                    si.StartHint as DvdRenderingStartHint : null;

                                if (hint != null && hint.IsSubtitleHint)
                                {
                                    RenderingEngine.DefaultInstance.SubtitleStream = hint.SID;
                                    return;
                                }
                            }

                            if (senderMenu.Tag is AudioCdSubItem)
                            {
                                CDAFileInfo cdfi = (senderMenu.Tag as AudioCdSubItem).Parent.MediaFileInfo as CDAFileInfo;
                                if (cdfi != null)
                                {
                                    cdfi.RefreshDisk();
                                    EventDispatch.DispatchEvent(LocalEventNames.UpdatePlaylistNames, true);
                                }
                            }
                            else
                            {
                                PlaylistSubItem psi = senderMenu.Tag as PlaylistSubItem;
                                if (psi != null && psi.StartHint != null)
                                {
                                    JumpToPlaylistSubItem(senderMenu.Tag as PlaylistSubItem);
                                }
                            }
                        }
                        else if (senderMenu.Tag is PlaylistItem)
                        {
                            JumpToPlaylistItem(senderMenu.Tag as PlaylistItem);
                        }
                        else
                        {
                            switch (senderMenu.Tag as string)
                            {
                                case "AddToDeezerPlaylist":
                                    playlist.AddToDeezerPlaylist();
                                    break;

                                default:
                                    ShortcutMapper.DispatchCommand((OPMShortcut)senderMenu.Tag);
                                    break;

                            }

                        }
                    }
                }
                finally
                {
                    senderMenu.Enabled = true;
                }
            }
        }

        public void BuildPlaylistMenu(OPMToolStripMenuItem tsmiPlaceholder, EventHandler eventHandler)
        {
            foreach (PlaylistItem plItem in playlist.GetPlaylistItems())
            {
                new MenuBuilder<OPMToolStripMenuItem>(playlist).AttachPlaylistItemMenu(plItem,
                       new MenuWrapper<OPMToolStripMenuItem>(tsmiPlaceholder),
                       MenuType.Playlist, eventHandler);
            }
            

        }
    }

    public class MediaPlayerOpenOption : OpenOption
    {
        public MediaPlayerOpenOption(CommandType cmd)
            : base(string.Empty, null)
        {
            OPMedia.UI.ProTONE.Configuration.FileTypesPanel.ExplorerLaunchType elt = new FileTypesPanel.ExplorerLaunchType(cmd);
            base.OptionTag = cmd;
            base.OptionTitle = elt.ToString();
        }
    }
}
