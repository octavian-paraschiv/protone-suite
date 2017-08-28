using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OPMedia.DeezerInterop.PlayerApi;

namespace OPMedia.Runtime.Deezer
{
    [StructLayout(LayoutKind.Sequential)]
    public class DeezerAppContext
    {
        public int nb_track_played;
        public bool is_playing;
        public string sz_content_url;
        public int activation_count;
        public IntPtr dzconnect;
        public IntPtr dzplayer;
        public dz_queuelist_repeat_mode_t repeat_mode;
        public bool is_shuffle_mode;

        public dz_player_onrenderprogress_cb renderProgressCB;
        public dz_player_onevent_cb playerEventCB;
        public dz_player_onrendererevent_cb rendererEventCB;
        
        public dz_activity_operation_callback activityCB;
    }
}
