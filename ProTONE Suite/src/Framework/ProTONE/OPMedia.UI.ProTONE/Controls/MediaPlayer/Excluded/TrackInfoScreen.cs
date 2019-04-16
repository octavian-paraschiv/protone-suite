﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.UI.Controls;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Core.Logging;
using TagLib.Riff;
using OPMedia.Runtime.FileInformation;
using OPMedia.UI.Themes;
using OPMedia.Core;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens
{
    public partial class TrackInfoScreen : OPMBaseControl
    {
        #region CanSaveData
        bool _canSaveData = false;
        object _canSaveDataLock = new object();

        private bool CanSaveData
        {
            get
            {
                lock (_canSaveDataLock)
                {
                    return _canSaveData;
                }
            }

            set
            {
                lock (_canSaveDataLock)
                {
                    _canSaveData = value;
                }
            }
        }
        #endregion

        public PlaylistItem PlaylistItem 
        {
            set
            {
                ShowPlaylistItem(value, true);
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

        public void SaveData()
        {
            PlaylistItem plItem = pgProperties.Tag as PlaylistItem;

            if (plItem != null && plItem.IsTrackInfoEditable)
            {
                try
                {
                    MediaFileInfoSlim mfis = pgProperties.SelectedObject as MediaFileInfoSlim;
                    if (mfis != null)
                    {
                        mfis.Save();
                    }
                }
                catch (Exception ex)
                {
                    ErrorDispatcher.DispatchError(ex, false);
                }
            }

        }

        public TrackInfoScreen()
        {
            InitializeComponent();

            ThemeManager.SetFont(lblItem, FontSizes.Small);
            ShowPlaylist();

            playlistScreen.SelectedItemChanged += new SelectedItemChangedHandler(playlistScreen_SelectedItemChanged);

            pgProperties.Enter += new EventHandler(pgProperties_Enter);
            pgProperties.Leave += new EventHandler(pgProperties_Leave);
        }

        void pgProperties_Leave(object sender, EventArgs e)
        {
            ThemeForm frm = FindForm() as ThemeForm;
            if (frm != null)
                frm.SuppressKeyPress = false;
        }

        void pgProperties_Enter(object sender, EventArgs e)
        {
            ThemeForm frm = FindForm() as ThemeForm;
            if (frm != null)
                frm.SuppressKeyPress = true;
        }
        
        public void ShowPlaylistItem(PlaylistItem plItem, bool callByProperty)
        {
            try
            {
                this.CanSaveData = false;

                if (plItem == null && callByProperty)
                {
                    List<PlaylistItem> plItems = playlistScreen.GetPlaylistItems();
                    if (plItems != null && plItems.Count > 0)
                    {
                        plItem = plItems[0];
                    }
                }

                pgProperties.Tag = null;
                pgProperties.SelectedObject = null;
                pgProperties.Visible = false;
                lblNoInfo.Visible = true;
                lblItem.Text = string.Empty;

                if (plItem != null)
                {
                    plItem.Rebuild();

                    lblItem.Text = plItem.MediaFileInfo.Path;

                    if (plItem.SupportsTrackInfo)
                    {
                        pgProperties.Tag = plItem;

                        MediaFileInfoSlim slim = plItem.MediaFileInfo.Slim();
                        FileAttributesBrowser.ProcessSingleObjectAttributes(slim);
                        pgProperties.SelectedObject = slim;

                        pgProperties.Visible = true;
                        lblNoInfo.Visible = false;
                    }
                }

                if (callByProperty)
                    playlistScreen.SetFirstSelectedPlaylistItem(plItem);
            }
            finally
            {
                this.CanSaveData = true;
            }
        }

        public void CopyPlaylist(PlaylistScreen source)
        {
            this.playlistScreen.CopyPlaylist(source);
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

        void playlistScreen_SelectedItemChanged(PlaylistItem newSelectedItem)
        {
            if (this.CanSaveData)
                SaveData();

            ShowPlaylistItem(newSelectedItem, false);
        }
    }
}
