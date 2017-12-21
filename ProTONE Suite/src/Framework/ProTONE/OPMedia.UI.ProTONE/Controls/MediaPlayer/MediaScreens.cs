﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens;
using OPMedia.UI.ProTONE.Controls.BookmarkManagement;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.UI.Themes;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.ProTONE.GlobalEvents;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Generic;
using OPMedia.UI.ProTONE.Properties;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public class MediaScreens : OPMTabControl
    {
        public PlaylistScreen PlaylistScreen { get; private set; }
        public TrackInfoScreen TrackInfoScreen { get; private set; }
        public SignalAnalysisScreen SignalAnalysisScreen { get; private set; }
        public BookmarkScreen BookmarkScreen { get; private set; }

        public Control _activeScreen = null;
        public Control _oldActiveScreen = null;

        public MediaScreens() : base()
        {
            this.InnerPadding = new Padding(0);

            this.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ItemSize = new Size(100, 55);
            this.SizeMode = TabSizeMode.Fixed;

            this.PlaylistScreen = new PlaylistScreen();
            this.PlaylistScreen.Dock = DockStyle.Fill;

            this.TrackInfoScreen = new TrackInfoScreen();
            this.TrackInfoScreen.Dock = DockStyle.Fill;

            this.SignalAnalysisScreen = new SignalAnalysisScreen();
            this.SignalAnalysisScreen.Dock = DockStyle.Fill;

            this.BookmarkScreen = new BookmarkScreen();
            this.BookmarkScreen.Dock = DockStyle.Fill;

            base.SelectedIndexChanged += new EventHandler(MediaScreens_SelectedIndexChanged);

            _oldActiveScreen = null;
            _activeScreen = this.PlaylistScreen;

            this.HandleCreated += new EventHandler(MediaScreens_HandleCreated);
            this.HandleDestroyed += new EventHandler(MediaScreens_HandleDestroyed);

            this.ImageList = new ImageList();
            this.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.ImageList.ImageSize = new Size(32, 32);
            this.ImageList.TransparentColor = Color.Magenta;

            this.ImageList.Images.Add(ImageProcessing.Playlist);
            this.ImageList.Images.Add(Resources.TrackInfo);
            this.ImageList.Images.Add(Resources.SignalAnalisys);
            this.ImageList.Images.Add(ImageProcessing.Bookmark);
            
            OnUpdateMediaScreens();
        }

        [EventSink(LocalEventNames.UpdateMediaScreens)]
        public void OnUpdateMediaScreens()
        {
            // playlist -------------------
            MediaScreen mediaScreen = MediaScreen.Playlist;
            bool show = ((ProTONEConfig.ShowMediaScreens & mediaScreen) == mediaScreen);
            TabPage tp = GetPageContainingControl(PlaylistScreen);

            if (show == false && tp != null)
                TabPages.Remove(tp);
            else if (show == true && tp == null)
            {
                int idx = FindIndexForInsert(PlaylistScreen);

                OPMTabPage otp = new OPMTabPage("TXT_PLAYLIST", this.PlaylistScreen);
                otp.BackColor = Color.Red;
                otp.ImageIndex = 0;

                if (idx >= TabPages.Count)
                    TabPages.Add(otp);
                else
                    TabPages.Insert(idx, otp);
            }
            
            // track info -------------------
            mediaScreen = MediaScreen.TrackInfo;
            show = ((ProTONEConfig.ShowMediaScreens & mediaScreen) == mediaScreen);
            tp = GetPageContainingControl(TrackInfoScreen);

            if (show == false && tp != null)
                TabPages.Remove(tp);
            else if (show == true && tp == null)
            {
                int idx = FindIndexForInsert(TrackInfoScreen);

                OPMTabPage otp = new OPMTabPage("TXT_TRACKINFO", this.TrackInfoScreen);
                otp.ImageIndex = 1;

                if (idx >= TabPages.Count)
                    TabPages.Add(otp);
                else
                    base.TabPages.Insert(idx, otp);
            }

            // signal analisys -------------------
            mediaScreen = MediaScreen.SignalAnalisys;
            show = ((ProTONEConfig.ShowMediaScreens & mediaScreen) == mediaScreen);
            tp = GetPageContainingControl(SignalAnalysisScreen);

            if (show == false && tp != null)
                TabPages.Remove(tp);
            else if (show == true && tp == null)
            {
                int idx = FindIndexForInsert(SignalAnalysisScreen);

                OPMTabPage otp = new OPMTabPage("TXT_SIGNALANALYSIS", this.SignalAnalysisScreen);
                otp.ImageIndex = 2;

                if (idx >= TabPages.Count)
                    TabPages.Add(otp);
                else
                    base.TabPages.Insert(idx, otp);
            }

            // bookmarks -------------------
            mediaScreen = MediaScreen.BookmarkInfo;
            show = ((ProTONEConfig.ShowMediaScreens & mediaScreen) == mediaScreen);
            tp = GetPageContainingControl(BookmarkScreen);

            if (show == false && tp != null)
                TabPages.Remove(tp);
            else if (show == true && tp == null)
            {
                int idx = FindIndexForInsert(BookmarkScreen);

                OPMTabPage otp = new OPMTabPage("TXT_BOOKMARKS", this.BookmarkScreen);
                otp.ImageIndex = 3;

                if (idx >= TabPages.Count)
                    TabPages.Add(otp);
                else
                    base.TabPages.Insert(idx, otp);
            }

            ResizeTabPages();
            Translator.TranslateControl(this, DesignMode);

        }

        internal void DoLayout()
        {
            ResizeTabPages();
        }

        private void ResizeTabPages()
        {
            int w = (this.Width - 3) / this.TabCount;
            this.ItemSize = new Size(w, 55);
        }

        private int FindIndexForInsert(OPMBaseControl screen)
        {
            int idx = 0;

            if (screen == PlaylistScreen)
            {
                idx = 0; // Playlist always goes first
            }
            else
            {
                TabPage tpPlaylist = GetPageContainingControl(PlaylistScreen);
                TabPage tpTrackInfo = GetPageContainingControl(TrackInfoScreen);

                if (screen == TrackInfoScreen)
                {
                    if (tpPlaylist != null)
                        idx = 1; // If playlist is shown, Track Info goes in second place
                    else
                        idx = 0; // Otherwise it goes first
                }
                else if (screen == SignalAnalysisScreen)
                {
                    if (tpTrackInfo != null &&
                        tpPlaylist != null)
                        idx = 2; // If both playlist and track info are shown, Signal Analisys goes in third place

                    else if (tpTrackInfo != null || tpPlaylist != null)
                        idx = 1; // If one of playlist and track info is shown (bit no both), Signal Analisys goes in second place

                    else
                        idx = 0; // Otherwise it goes first
                }
                else if (screen == BookmarkScreen)
                    idx = (TabPages.Count); // Bookmark Info always goes last
            }

            return idx;
        }

        void MediaScreens_HandleDestroyed(object sender, EventArgs e)
        {
            EventDispatch.UnregisterHandler(this);
        }
        void MediaScreens_HandleCreated(object sender, EventArgs e)
        {
            EventDispatch.RegisterHandler(this);
            ThemeManager.SetDoubleBuffer(this);
        }

        void MediaScreens_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control currentScreen = null;
            if (base.SelectedTab != null)
            {
                currentScreen = base.SelectedTab.Controls[0];
                DoScreenSelectionTasks(currentScreen);
            }
        }

        private void DoScreenSelectionTasks(Control c)
        {
            string desc = string.Empty;

            _oldActiveScreen = _activeScreen;
            _activeScreen = c;

            if (_activeScreen == this.PlaylistScreen)
            {
            }
            else if (_activeScreen == this.BookmarkScreen)
            {
                this.BookmarkScreen.CopyPlaylist(this.PlaylistScreen);

                PlaylistItem plItem = null;
                if (MediaRenderer.DefaultInstance.FilterState == FilterState.Running ||
                    MediaRenderer.DefaultInstance.FilterState == FilterState.Paused)
                {
                    plItem = this.PlaylistScreen.GetActivePlaylistItem();
                }

                if (plItem == null)
                {
                    plItem = this.PlaylistScreen.GetFirstSelectedPlaylistItem();
                }

                this.BookmarkScreen.PlaylistItem = plItem;

            }
            else if (_activeScreen == this.TrackInfoScreen)
            {
                this.TrackInfoScreen.CopyPlaylist(this.PlaylistScreen);

                PlaylistItem plItem = null;
                if (MediaRenderer.DefaultInstance.FilterState == FilterState.Running ||
                    MediaRenderer.DefaultInstance.FilterState == FilterState.Paused)
                {
                    plItem = this.PlaylistScreen.GetActivePlaylistItem();
                }

                if (plItem == null)
                {
                    plItem = this.PlaylistScreen.GetFirstSelectedPlaylistItem();
                }

                this.TrackInfoScreen.PlaylistItem = plItem;
            }
            else if (_activeScreen == this.SignalAnalysisScreen)
            {
            }

            if (_oldActiveScreen == this.BookmarkScreen)
            {
                this.BookmarkScreen.SaveBookmarks(false);
            }
            else if (_oldActiveScreen == this.TrackInfoScreen)
            {
                this.TrackInfoScreen.SaveData();
            }

        }

        [EventSink(OPMedia.UI.ProTONE.GlobalEvents.EventNames.PerformTranslation)]
        public void OnPerformTranslation()
        {
            Translator.TranslateControl(this, DesignMode);

            TabPages[0].Text = Translator.Translate("TXT_PLAYLIST");
            TabPages[1].Text = Translator.Translate("TXT_TRACKINFO");
            TabPages[2].Text = Translator.Translate("TXT_SIGNALANALYSIS");
            TabPages[3].Text = Translator.Translate("TXT_BOOKMARKS");
        }

        [EventSink(OPMedia.UI.ProTONE.GlobalEvents.EventNames.ExecuteShortcut)]
        public void OnExecuteShortcut(Runtime.Shortcuts.OPMShortcutEventArgs args)
        {
            if (this.PlaylistScreen != null)
            {
                bool dispatchCmd = false;

                switch(args.cmd)
                {
                    case OPMShortcut.CmdPlaylistEnd:
                    case OPMShortcut.CmdToggleShuffle:
                    case OPMShortcut.CmdLoopPlay:
                        dispatchCmd = true;
                        break;

                    default:
                        dispatchCmd = (_activeScreen == this.PlaylistScreen);
                        break;
                }

                // Dispatch shortcut
                if (dispatchCmd)
                    this.PlaylistScreen.OnExecuteShortcut(args);
            }
        }
    }
}
