using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.ExtendedInfo;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Controls;
using OPMedia.UI.Generic;
using OPMedia.UI.Menus;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.UI.ProTONE.SubtitleDownload;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public enum MenuType
    {
        Playlist = 0,
        SingleItem,
        MultipleItems
    }

    public class MenuBuilder<T>
    {
        PlaylistPanel _pnlPlaylist = null;

        public MenuBuilder(PlaylistPanel pnlPlaylist)
        {
            _pnlPlaylist = pnlPlaylist;
        }

        public int BuildCommandsMenu(int index, MenuWrapper<T> menu, EventHandler clickHandler)
        {
            for (OPMShortcut cmd = OPMShortcut.CmdPlayPause; cmd <= OPMShortcut.CmdFullScreen; cmd++)
            {
                BuildMenuEntry(cmd, menu, clickHandler, index);
                index++;
            }

            menu.InsertSingleEntry(index, new OPMMenuStripSeparator());
            index++;

            for (OPMShortcut cmd = OPMShortcut.CmdFwd; cmd <= OPMShortcut.CmdVolDn; cmd++)
            {
                BuildMenuEntry(cmd, menu, clickHandler, index);
                index++;
            }

            menu.InsertSingleEntry(index, new OPMMenuStripSeparator());
            index++;

            for (OPMShortcut cmd = OPMShortcut.CmdLoopPlay; cmd <= OPMShortcut.CmdToggleShuffle; cmd++)
            {
                BuildMenuEntry(cmd, menu, clickHandler, index);
                index++;
            }

            return index;
        }

        public void AttachToolsMenu(MenuWrapper<T> menu, EventHandler clickHandler)
        {
            BuildMenuEntry(OPMShortcut.CmdCfgVideo, menu, clickHandler);
            BuildMenuEntry(OPMShortcut.CmdCfgAudio, menu, clickHandler);
            menu.AddSingleEntry(new OPMMenuStripSeparator());
            BuildMenuEntry(OPMShortcut.CmdCfgSubtitles, menu, clickHandler);
            BuildMenuEntry(OPMShortcut.CmdCfgTimer, menu, clickHandler);
            menu.AddSingleEntry(new OPMMenuStripSeparator());
            BuildMenuEntry(OPMShortcut.CmdOpenSettings, menu, clickHandler);
        }

        public void AttachPlaylistItemMenu(PlaylistItem plItem, MenuWrapper<T> menu, MenuType menuType, EventHandler clickHandler)
        {
            if (menuType != MenuType.Playlist)
            {
                AttachCommonPlaylistToolsMenu(menu, menuType, clickHandler, plItem);
            }

            MenuWrapper<T> menuToAlter = menu;

            if (menuType == MenuType.Playlist)
            {
                OPMToolStripMenuItem item = new OPMToolStripMenuItem(plItem.DisplayName);
                item.Tag = plItem;
                item.Click += clickHandler;

                int idx = menu.AddSingleEntry(item);
                item.Checked = (idx == _pnlPlaylist.PlayIndex);

                menuToAlter = new MenuWrapper<OPMToolStripMenuItem>(item) as MenuWrapper<T>;
            }

            if (ProTONEConfig.DeezerHasValidConfig)
            {
                OPMToolStripMenuItem tsmi = new OPMToolStripMenuItem();
                tsmi.Click += clickHandler;
                tsmi.Text = Translator.Translate("TXT_ADD_TO_DEEZER_PLAYLIST");
                tsmi.Tag = "AddToDeezerPlaylist";
                tsmi.Image = Resources.deezer16;
                menuToAlter.AddSingleEntry(tsmi);
            }

            if (plItem != null && menuType != MenuType.MultipleItems)
            {
                // It may have subitems:
                // * a DVD item will have titles, chapters, etc ...
                // * other media files may have bookmarks ...
                Dictionary<PlaylistSubItem, List<PlaylistSubItem>> submenu = plItem.GetSubmenu();

                if (submenu != null && submenu.Count >= 1)
                {
                    if (menuType == MenuType.SingleItem)
                    {
                        menuToAlter.AddSingleEntry(new OPMMenuStripSeparator());
                    }

                    foreach (KeyValuePair<PlaylistSubItem, List<PlaylistSubItem>> subitems in submenu)
                    {
                        OPMToolStripMenuItem subItem = new OPMToolStripMenuItem(subitems.Key.Name);
                        subItem.Click += clickHandler;
                        subItem.Tag = subitems.Key;

                        if (subitems.Value != null)
                        {
                            foreach (PlaylistSubItem ssitem in subitems.Value)
                            {
                                OPMToolStripMenuItem subSubItem = null;

                                if (ssitem is DvdSubItem)
                                {
                                    subSubItem = new OPMToolStripMenuItem(ssitem.Name);
                                    subSubItem.Click += clickHandler;
                                    subSubItem.Tag = ssitem;

                                    DvdSubItem si = ssitem as DvdSubItem;
                                    DvdRenderingStartHint hint = (si != null) ?
                                        si.StartHint as DvdRenderingStartHint : null;

                                    if (hint != null && hint.IsSubtitleHint)
                                    {
                                        subSubItem.Checked = (hint.SID == RenderingEngine.DefaultInstance.SubtitleStream);
                                    }
                                }
                                else if (ssitem is BookmarkSubItem)
                                {
                                    BookmarkSubItem bsi = (ssitem as BookmarkSubItem);
                                    BookmarkStartHint hint = (bsi != null) ?
                                        bsi.StartHint as BookmarkStartHint : new BookmarkStartHint(Bookmark.Empty);

                                    string name = string.Format("{0} - '{1}'", hint.Bookmark.PlaybackTime, bsi.Name);
                                    subSubItem = new OPMToolStripMenuItem(name);
                                    subSubItem.Click += clickHandler;
                                    subSubItem.Tag = ssitem;
                                }

                                if (subSubItem != null)
                                {
                                    subItem.DropDownItems.Add(subSubItem);
                                }
                            }
                        }

                        menuToAlter.AddSingleEntry(subItem);
                    }
                }

                if (menuType == MenuType.SingleItem)
                {
                    if (plItem is DvdPlaylistItem)
                    {
                        AttachDvdMenuItems(plItem as DvdPlaylistItem, menuToAlter, clickHandler);
                    }
                    else
                    {
                        AttachFileMenuItems(plItem as PlaylistItem, menuToAlter, clickHandler);
                    }
                }
            }
        }

        public void AttachCommonPlaylistToolsMenu(MenuWrapper<T> menu, MenuType menuType, EventHandler clickHandler, PlaylistItem plItem)
        {
            //OPMShortcut cmdStart = (menuType == MenuType.SingleItem) ? 
            //  OPMShortcut.CmdMoveUp : OPMShortcut.CmdClear;

            for (OPMShortcut cmd = OPMShortcut.CmdMoveUp; cmd <= OPMShortcut.CmdSavePlaylist; cmd++)
            {
                switch (cmd)
                {
                    case OPMShortcut.CmdMoveUp:
                    case OPMShortcut.CmdMoveDown:
                    case OPMShortcut.CmdDelete:
                        BuildMenuEntry(cmd, menu, clickHandler, -1, plItem != null);
                        break;

                    default:
                        BuildMenuEntry(cmd, menu, clickHandler);
                        break;
                }
            }
        }

        private void BuildMenuEntry(OPMShortcut cmd, MenuWrapper<T> menu, EventHandler clickHandler, int index = -1, bool enabled = true)
        {
            if (cmd == OPMShortcut.CmdOpenDisk)
            {
                if (VideoDVDHelpers.IsOSSupported == false)
                    return;
            }

            string shortcuts = ShortcutMapper.GetShortcutString(cmd);
            string menuName = cmd.ToString().ToUpperInvariant().Replace("CMD", "MNU");
            string imageName = "btn" + cmd.ToString().Replace("Cmd", "");

            string desc = Translator.Translate("TXT_" + menuName);

            OPMToolStripMenuItem tsmi = new OPMToolStripMenuItem(desc);
            tsmi.Click += clickHandler;
            tsmi.Tag = cmd;
            tsmi.ShortcutKeyDisplayString = shortcuts;

            switch (cmd)
            {
                case OPMShortcut.CmdMoveUp:
                    tsmi.Image = OPMedia.UI.Properties.Resources.Up16;
                    break;

                case OPMShortcut.CmdMoveDown:
                    tsmi.Image = OPMedia.UI.Properties.Resources.Down16;
                    break;

                case OPMShortcut.CmdDelete:
                    tsmi.Image = OPMedia.UI.Properties.Resources.Delete16;
                    break;

                case OPMShortcut.CmdLoadPlaylist:
                    tsmi.Image = OPMedia.UI.Properties.Resources.Open16;
                    break;

                case OPMShortcut.CmdSavePlaylist:
                    tsmi.Image = OPMedia.UI.Properties.Resources.Save16;
                    break;

                case OPMShortcut.CmdOpenDisk:
                    tsmi.Image = ImageProcessing.DVD;
                    break;

                case OPMShortcut.CmdOpenURL:
                    tsmi.Image = OPMedia.Core.Properties.Resources.Internet;
                    break;

                case OPMShortcut.CmdPlayPause:
                    {
                        Bitmap img = null;

                        switch (RenderingEngine.DefaultInstance.FilterState)
                        {
                            case Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused:
                                img = Resources.btnPlay;
                                break;

                            case Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running:
                                img = Resources.btnPause;
                                break;

                            default:
                                img = Resources.btnPlay;
                                break;
                        }

                        img.MakeTransparent(ThemeManager.TransparentColor);
                        tsmi.Image = img;
                    }
                    break;

                case OPMShortcut.CmdCfgAudio:
                    tsmi.Image = ImageProcessing.AudioFile16;
                    break;

                case OPMShortcut.CmdCfgVideo:
                    tsmi.Image = ImageProcessing.VideoFile16;
                    break;

                case OPMShortcut.CmdCfgSubtitles:
                    tsmi.Image = ImageProcessing.Subtitle16;
                    break;

                case OPMShortcut.CmdOpenSettings:
                    tsmi.Image = OPMedia.UI.Properties.Resources.Settings16;
                    break;

                case OPMShortcut.CmdCfgTimer:
                    tsmi.Image = Resources.IconTime;
                    break;

                default:
                    tsmi.Image = Resources.ResourceManager.GetImage(imageName);
                    break;
            }

            tsmi.Enabled = enabled;

            if (index >= 0)
            {
                menu.InsertSingleEntry(index, tsmi);
            }
            else
            {
                menu.AddSingleEntry(tsmi);
            }
        }

        private void AttachFileMenuItems(PlaylistItem playlistItem, MenuWrapper<T> menu, EventHandler clickHandler)
        {
            if (EnableSubtitleEntry(playlistItem))
            {
                if (menu.MenuItemsCount > 0)
                {
                    menu.AddSingleEntry(new OPMMenuStripSeparator());
                }

                string str = Translator.Translate("TXT_SEARCH_SUBTITLES");
                OPMToolStripMenuItem tsmi = new OPMToolStripMenuItem(str);
                tsmi.Click += clickHandler;
                tsmi.Tag = OPMShortcut.CmdSearchSubtitles;
                tsmi.ShortcutKeyDisplayString = ShortcutMapper.GetShortcutString(OPMShortcut.CmdSearchSubtitles);

                menu.AddSingleEntry(tsmi);
            }
        }

        private void AttachDvdMenuItems(DvdPlaylistItem dvdPlaylistItem, MenuWrapper<T> menu, EventHandler clickHandler)
        {
            //cmsInsert.DropDownItems.Add("DVD menu item");
            // Nothing to add specifically for DVD's right now ...
        }

        private bool EnableSubtitleEntry(PlaylistItem plItem)
        {
            return
                plItem != null &&
                plItem.IsVideo &&
                SubtitleDownloadProcessor.CanPerformSubtitleDownload(plItem.Path,
                (int)plItem.Duration.TotalSeconds);
        }

    }
}
