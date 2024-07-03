using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPMedia.Core.Shortcuts
{
    public enum OPMShortcut
    {
        // Playback control
        CmdPlayPause,
        CmdStop,
        CmdPrev,
        CmdNext,
        CmdLoad,
        CmdOpenDisk,
        CmdOpenURL,

        // Full Screen
        CmdFullScreen,

        // Media seek control
        CmdFwd,
        CmdRew,

        // Volume control
        CmdVolUp,
        CmdVolDn,

        // Playlist control
        CmdMoveUp,
        CmdMoveDown,
        CmdDelete,

        CmdClear,
        CmdLoadPlaylist,
        CmdSavePlaylist,

        CmdLoopPlay,
        CmdXFade,
        CmdPlaylistEnd,
        CmdToggleShuffle,
        CmdJumpToItem,

        // Player configuration
        CmdCfgVideo,
        CmdCfgAudio,
        CmdCfgTimer,
        CmdCfgSubtitles,
        CmdCfgRemote,

        // Subtitles
        CmdSearchSubtitles,

        // Common commands (player-non player)
        CmdOpenHelp,
        CmdOpenSettings,
        CmdCfgKeyboard,
        CmdDumpDebugStats,

        // Commands not related to player
        CmdGenericOpen,
        CmdGenericNew,
        CmdGenericSave,
        CmdGenericUndo,
        CmdGenericApply,

        // 
        CmdSwitchWindows,

        // 
        CmdGenericCopy,
        CmdGenericCut,
        CmdGenericPaste,

        CmdGenericRefresh,
        CmdGenericDelete,
        CmdGenericRename,
        CmdGenericSearch,

        CmdNavigateUp,
        CmdNavigateBack,
        CmdNavigateForward,
        CmdChangeDisk,
        CmdFavManager,
        CmdTaggingWizard,
        CmdCatalogWizard,
        CmdCatalogMerge,
        CmdCdRipperWizard,

        CmdEditPath,

        CmdSignalAnalisys,

        CmdOutOfRange,
    };
}
