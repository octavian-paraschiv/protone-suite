using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

// Ported from original header file deezer-player.h

namespace OPMedia.DeezerInterop.PlayerApi
{
    #region Enums
    // Player events.
    public enum dz_player_event_t
    {
        DZ_PLAYER_EVENT_UNKNOWN,                             /* Player event has not been set yet, not a valid value. */
    
        // Data access related event.
        DZ_PLAYER_EVENT_LIMITATION_FORCED_PAUSE,             /* Another deezer player session was created elsewhere, the player has entered pause mode. */

        // Track selection related event.
        DZ_PLAYER_EVENT_QUEUELIST_LOADED,                    /* Content has been loaded. */
        DZ_PLAYER_EVENT_QUEUELIST_NO_RIGHT,                  /* You don't have the right to play this content: track, album or playlist */
        DZ_PLAYER_EVENT_QUEUELIST_TRACK_NOT_AVAILABLE_OFFLINE,/* You're offline, the track is not available. */
        DZ_PLAYER_EVENT_QUEUELIST_TRACK_RIGHTS_AFTER_AUDIOADS,/* You have right to play it, but you should render an ads first :
                                                                  - Use dz_player_event_get_advertisement_infos_json().
                                                                  - Play an ad with dz_player_play_audioads().
                                                                  - Wait for #DZ_PLAYER_EVENT_RENDER_TRACK_END.
                                                                  - Use dz_player_play() with previous track or DZ_PLAYER_PLAY_CMD_RESUMED_AFTER_ADS (to be done even on mixes for now).
                                                              */
        DZ_PLAYER_EVENT_QUEUELIST_SKIP_NO_RIGHT,              /* You're on a mix, and you had no right to do skip. */
    
        DZ_PLAYER_EVENT_QUEUELIST_TRACK_SELECTED,             /* A track is selected among the ones available on the server, and will be fetched and read. */
    
        DZ_PLAYER_EVENT_QUEUELIST_NEED_NATURAL_NEXT,          /* We need a new natural_next action. */
    
        // Data loading related event.
        DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY,              /* Data is ready to be introduced into audio output (first data after a play). */
        DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY_AFTER_SEEK,   /* Data is ready to be introduced into audio output (first data after a seek). */

        // Play (audio rendering on output) related event.
        DZ_PLAYER_EVENT_RENDER_TRACK_START_FAILURE,       /* Error, track is unable to play. */
        DZ_PLAYER_EVENT_RENDER_TRACK_START,               /* A track has started to play. */
        DZ_PLAYER_EVENT_RENDER_TRACK_END,                 /* A track has stopped because the stream has ended. */
        DZ_PLAYER_EVENT_RENDER_TRACK_PAUSED,              /* Currently on paused. */
        DZ_PLAYER_EVENT_RENDER_TRACK_SEEKING,             /* Waiting for new data on seek. */
        DZ_PLAYER_EVENT_RENDER_TRACK_UNDERFLOW,           /* Underflow happened whilst playing a track. */
        DZ_PLAYER_EVENT_RENDER_TRACK_RESUMED,             /* Player resumed play after a underflow or a pause. */
        DZ_PLAYER_EVENT_RENDER_TRACK_REMOVED,             /* Player stopped playing a track. */
}

    // Add an hint about why a new play is done.
    public enum dz_player_play_command_t
    {
        DZ_PLAYER_PLAY_CMD_UNKNOWN,           /* Player command has not been set yet, not a valid value. */
        DZ_PLAYER_PLAY_CMD_START_TRACKLIST,   /* A new tracklist was loaded and a track played. */
        DZ_PLAYER_PLAY_CMD_JUMP_IN_TRACKLIST, /* The user jump into a new song in the current tracklist. */
        DZ_PLAYER_PLAY_CMD_NEXT,              /* Next button. */
        DZ_PLAYER_PLAY_CMD_PREV,              /* Prev button. */
        DZ_PLAYER_PLAY_CMD_DISLIKE,           /* Dislike button. Dislike and skip the current track (Only available for radios/mixes) */
        DZ_PLAYER_PLAY_CMD_NATURAL_END,       /* Natural end. */
        DZ_PLAYER_PLAY_CMD_RESUMED_AFTER_ADS, /* Reload after playing an ads. */
    }


    // Player tracklist repeat modes.
    // Defines the behavior of the player when it finishes playing a track.
    public enum dz_queuelist_repeat_mode_t
    {
        DZ_QUEUELIST_REPEAT_MODE_OFF,          /* Play the loaded content starting from the given track index in the queuelist. */
        DZ_QUEUELIST_REPEAT_MODE_ONE,          /* Automatically play the current track forever. */
        DZ_QUEUELIST_REPEAT_MODE_ALL,          /* Automatically play the entire queuelist forever with a natural order. */
    }
    #endregion

    #region Delegates
    /**
     // Prototype of the on metadata available callback.
     *
     * You have to assume the #dz_track_metadata_t object is invalid
     * after the callback returns,\n but you may retain it, then release it
     * by calling dz_object_retain() or dz_object_release().
     * @param handle             Deezer player handle.
     * @param metadata           The handle of the metadata information.
     * @param userdata           A reference to the user’s data.
     */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_player_onmetadata_cb(IntPtr handle, IntPtr metadata, IntPtr userdata);

    /**
     // Prototype of the cache progress callback.
     * @param handle             Deezer player handle.
     * @param progress           The cache progress information in microseconds.
     * @param userdata           A reference to the user’s data.
     */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_player_onindexprogress_cb(IntPtr handle, UInt64 progress, IntPtr userdata);

    /**
     // Prototype of the render progress callback.
     * @param handle             Deezer player handle.
     * @param progress           The playback progress information in microseconds.
     * @param userdata           A reference to the user’s data.
     */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_player_onrenderprogress_cb(IntPtr handle, UInt64 progress, IntPtr userdata);

    /**
     // Prototype of the on event callback.
     * @param handle             Deezer player handle.
     * @param event              The event information.
     * @param userdata           A reference to the user’s data.
     */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_player_onevent_cb(IntPtr handle, IntPtr evtHandle, IntPtr userdata);

        /**
     // Prototype of the on renderer event callback
     *
     * @param self     Deezer player handle.
     * @param event    The handle of the renderer event that has just occured.
     * @param userdata A reference to the user’s data.
     */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_player_onrendererevent_cb(IntPtr handle, IntPtr evtHandle, IntPtr userdata);
    #endregion

    public partial class DeezerApi
    {
        /**
         * @brief Get the event type of a player event instance.
         * @param self A player event instance.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_player_event_t dz_player_event_get_type(IntPtr self);

        /**
         * @brief Get the queuelist context for a player event.
         *
         * For most player events, you can get the streaming mode #dz_streaming_mode_t and the index in the queuelist.
         *
         * @param self               The player event that the context will be retrieved.
         * @param out_streaming_mode The streaming mode from which the event has occurred.
         * @param out_idx            The corresponding index in the queuelist of the event.
         * @return true if the context has been returned, false otherwise.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dz_player_event_get_queuelist_context(IntPtr self,
            out dz_streaming_mode_t out_streaming_mode,
            out UInt32 out_idx);

        /**
         * @brief Get the Deezer API information for the player event instance.
         * @note Only available for #DZ_PLAYER_EVENT_QUEUELIST_TRACK_SELECTED event.
         * @param self A player event instance.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_player_event_track_selected_dzapiinfo(IntPtr self);

        /**
         * @brief Get the Deezer API information for the next track of a player event instance.
         * @note Only available for #DZ_PLAYER_EVENT_QUEUELIST_TRACK_SELECTED event.
         * @param self A player event instance.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_player_event_track_selected_next_track_dzapiinfo(IntPtr self);

        /**
         * @brief Get the playback rights for a track of a player event instance.
         * @note Only available for #DZ_PLAYER_EVENT_QUEUELIST_TRACK_SELECTED event.
         * @param self A player event instance.
         * @param out_can_pause_unpause Pause/unpause availability for the track.
         * @param out_can_seek          Seek availability for the track.
         * @param out_nb_skip_allowed   Number of skips available for the track.
         * @return true if the output information is valid.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dz_player_event_track_selected_rights(IntPtr self,
                                                                 out bool out_can_pause_unpause,
                                                                 out bool out_can_seek,
                                                                 out int  out_nb_skip_allowed);
        /**
         * @brief Find out if the current track is a preview (30s).
         * 
         * @note Only available for #DZ_PLAYER_EVENT_QUEUELIST_TRACK_SELECTED event.
         * @param self A player event instance.
         * @return true if the selected track event is for a preview, false in all other cases.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dz_player_event_track_selected_is_preview(IntPtr self);

        /**
         * @brief Get information about displayed advertisements.
         *
         * @note Only available for #DZ_PLAYER_EVENT_QUEUELIST_TRACK_RIGHTS_AFTER_AUDIOADS event.
         * @param self A player event instance.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_player_event_get_advertisement_infos_json(IntPtr self);

        /**
         * @brief Constructor for Deezer player.
         *
         * @param connect A deezer connect handle.
         * @returns A player handle.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr dz_player_new(IntPtr connect);

        /**
         * @brief Register the event callback.
         *
         * Only one callback can be registered per player instance.
         * Should be done after dz_player_new() and before dz_player_load().
         *
         * @param self Deezer player handle
         * @param cb   The dz_player_onerror_cb() callback.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_event_cb(IntPtr self,
                                                        dz_player_onevent_cb cb);

        /**
         * @brief Register a on data callback.
         *
         * Only one callback can be registered per player instance.
         * Should be done after dz_player_new() and before dz_player_load().
         *
         * @param self Deezer player handle.
         * @param cb   The dz_player_ondata_cb() callback.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_metadata_cb(IntPtr self,
                                                           dz_player_onmetadata_cb cb);


        /**
         * @brief Register a callback called when more data are available locally.
         *
         * Only one callback can be registered per player instance.
         * Should be done after dz_player_new() and before dz_player_load()
         *
         * @param self       Deezer player handle.
         * @param cb         The dz_player_onprogress_cb() callback.
         * @param refresh_us Approximate refresh time self callback is called.
         * @return DZ_ERROR_NO_ERROR if OK
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_index_progress_cb(IntPtr self,
                                                                 dz_player_onindexprogress_cb cb,
                                                                 UInt64 refresh_us);

        /**
         * @brief Register a callback that will be called when there is more audio
         * data rendered (played on an audio output).
         *
         * Only one callback can be registered per player instance.
         * Should be done after dz_player_new and before dz_player_load
         *
         * @param self       Deezer player handle.
         * @param cb         The dz_player_onprogress_cb() callback.
         * @param refresh_us Approximate refresh time self callback is called.
         * @return DZ_ERROR_NO_ERROR if OK
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_render_progress_cb(IntPtr self,
                                                                  dz_player_onrenderprogress_cb cb,
                                                                  UInt64 refresh_us);


        /**
         * @brief Activate the player.
         *
         * @note Activate the player after the callback has been set.
         *
         * @param self       Deezer player handle you want to activate
         * @param supervisor Application context that will be returned when events occur.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_activate(IntPtr self, IntPtr supervisor);


        /**
         * @brief Deactivate the player.
         *
         * @param self               Deezer player handle you want to deactivate.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_deactivate(IntPtr self,
                                                      dz_activity_operation_callback cb,
                                                      IntPtr operation_userdata);

        /**
         * @brief Set the track quality.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param quality            Audio track quality.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_track_quality(IntPtr self,
                                                             dz_activity_operation_callback cb,
                                                             IntPtr operation_userdata,
                                                             dz_track_quality_t quality);

        /**
         * @brief Get track quality.
         *
         * Get the currently configured track quality.
         *
         * @param self               Deezer player handle.
         * @return The audio track quality.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_track_quality_t dz_player_get_track_quality(IntPtr self);


        /**
         * @brief Set the repeat mode.
         * @note This function must be called after dz_player_activate() has been called.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param repeat_mode        Repeat mode. It can be disabled, repeat one or repeat all.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_repeat_mode(IntPtr self,
                                                           dz_activity_operation_callback cb,
                                                           IntPtr operation_userdata,
                                                           dz_queuelist_repeat_mode_t repeat_mode);
        /**
         * @brief Set the shuffle mode.
         * @note The shuffle mode is only applied for album or playlist playback.
         * @note This function must be called after dz_player_activate() has been called.
         *
         * @param self                Deezer player handle.
         * @param cb                  The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata  A reference to the user’s data.
         * @param enable_shuffle_mode Enable shuffle playback mode of the queuelist if set to true.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_enable_shuffle_mode(IntPtr self,
                                                               dz_activity_operation_callback cb,
                                                               IntPtr operation_userdata,
                                                               bool enable_shuffle_mode);
        /**
         * @brief Set the tracks/radio to play.
         *
         * @note A #DZ_PLAYER_EVENT_QUEUELIST_LOADED is sent when the content is ready to be played
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param tracklist_data: 
         *        - On demand: a deezer url, such as dzmedia:///track/[track_id] for single track content.
         *        - On demand: a deezer url, such as dzmedia:///album/[album_id] or dzmedia:///playlist/[playlist_id] for an album or a playlist.
         *        - (Not available) On demand: a json with a list of deezer url
         *        - Radio: a theme/artist/user/playlist radio dzradio:///[radio_id]
         *          "radio_id" is like "radio-???", "artist-???", "user-???", "playlist-???" respectively
         *          for a radio, a mix based on an artist, a user or a playlist.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_load(IntPtr self,
                                                dz_activity_operation_callback cb,
                                                IntPtr operation_userdata,
                                               string tracklist_data);

        /**
         * @brief Set the radio to play by type and id.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param radio_type         "radio", "artist", "user" or "playlist".
         * @param radio_id           A string representing the integer id of the radio id.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_load_radio(IntPtr self,
                                                      dz_activity_operation_callback cb,
                                                      IntPtr operation_userdata,
                                                      string radio_type,
                                                      string radio_id);


        /**
         * @brief Cache a track for playback purposes.
         *
         * Use this function to prefetch the next track to be played.\n
         * This function can be called multiple times if need be (i.e. each time the next track changes).
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param track_url          A track url, like dzmedia:///track/[track_id]
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_cache_next(IntPtr self,
                                                      dz_activity_operation_callback cb,
                                                      IntPtr operation_userdata,
                                                      string track_url);

        /**
         * @brief Begin playing the already loaded tracklist.
         *        The player gets data and renders it.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param command            See function comments.
         * @param idx                Index of the track in the queuelist to play.\n
         *                           * Radios (aka mixes) also support #Int32_CURRENT or #Int32_NEXT.\n
         *                           * Albums & Playlists support #Int32_PREVIOUS, #Int32_CURRENT
         *                           or #Int32_NEXT.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_play(IntPtr self,
                                                dz_activity_operation_callback cb,
                                                IntPtr operation_userdata,
                                                dz_player_play_command_t command,
                                                Int32 idx);

        /**
         * @brief Play an audio ad when it is requested.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_play_audioads(IntPtr self,
                                                         dz_activity_operation_callback cb,
                                                         IntPtr operation_userdata);

        /**
         * @brief Put the player in stop state.
         *
         * Cached buffers have been dropped, and the data callback has stopped being triggered by the player.
         * The player can start streaming again from the begin if you call dz_player_play() again.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_stop(IntPtr self,
                                                dz_activity_operation_callback cb,
                                                IntPtr operation_userdata);

        /**
         * @brief Put the player on pause.
         *
         * No cache buffer is dropped, and the data callback stops being triggered by the player.
         * @note You need to call dz_player_resume() to start data callback again.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_pause(IntPtr self,
                                                 dz_activity_operation_callback cb,
                                                 IntPtr operation_userdata);

        /**
         * @brief Resumes playback after a pause.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_resume(IntPtr self,
                                                   dz_activity_operation_callback cb,
                                                   IntPtr operation_userdata);

        /**
         * @brief Seek at the specified time position.
         *
         * Cached buffers have been dropped, and the player will prepare data
         * from the closest position.
         *
         * If the player is in stop state, it goes into the pause state.
         * Otherwise the player stays in play / pause state.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data..
         * @param pos_usecond        Position in microseconds from the begin of track.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_seek(IntPtr self,
                                                dz_activity_operation_callback cb,
                                                IntPtr operation_userdata,
                                                UInt64 pos_usecond);


                /**
         * @brief Set the player output volume.
         *
         * Changing this volume won't impact the volume of the other applications
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param volume             Volume to set in percent (0 to 100).
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_output_volume(IntPtr self,
                                                             dz_activity_operation_callback cb,
                                                             IntPtr operation_userdata,
                                                             Int32 volume);


        /**
         * @brief Mute the stream.
         *
         * Cached buffers have been dropped, and the player will prepare data
         * from the closest position.
         *
         * If the player was in stop state, it goes into the pause state.
         * Otherwise the player keeps its play/pause state.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param muted              Mute value.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_output_mute(IntPtr self,
                                                           dz_activity_operation_callback cb,
                                                           IntPtr operation_userdata,
                                                           bool muted);

        /**
         * @brief Setup things to be able to do cross fadings.
         *
         * "things" are :
         * - Event #DZ_PLAYER_EVENT_QUEUELIST_NEED_NATURAL_NEXT fade_duration_ms before the end of current track.
         * - Cross fading between two tracks when a natural_next happens.
         * @note Cross fading won't be applied for free account users.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param fade_duration_ms   Duration in microseconds of cross-fading (0 to remove, typically less than 10s).
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_crossfading_duration(IntPtr self,
                                                                    dz_activity_operation_callback cb,
                                                                    IntPtr operation_userdata,
                                                                    UInt32 fade_duration_ms);

        /**
         * @brief Set the UI context.
         *
         * Used by deezer for statistical purposes to improve client experience.
         *
         * Example of ui_app_context:
         * * Feed/hear this:
         *   - flow: {"t":"feed_user_radio", "id":USER_ID_VALUE}
         *   - card_album: {"t":"feed_album", "id":ALBUM_ID_VALUE,"c":CARD_ID_VALUE}
         *   - card_playlist: {"t":"feed_playlist", "id":PLAYLIST_VALUE,"c":CARD_ID_VALUE}
         *   - card_track: {"t":"feed_playlist", "id":TRACK_ID_VALUE,"c":CARD_ID_VALUE}
         *   - card_smartradio: {"t":"feed_smartradio", "id":ARTIST_ID_VALUE,"c":CARD_ID_VALUE}
         * * Search:
         *   - track: {"t":"search_page", "id":QUERY}
         * * Playlist pages:
         *   - playlist: {"t":"playlist_page", "id":PLAYLIST_VALUE}
         *   - loved tracks: {"t":"loved_page", "id":PLAYLIST_VALUE}
         * * Album page:
         *   - album: {"t":"album_page", "id":ALBUM_ID_VALUE}
         *
         *
         * Example of ui_app_context:
         * - track_page:[trackid]
         * - album_page:[albumid]
         * - playlist_page:[playlistid]
         * - downloads_page
         * - personalsong_page
         * - folder_page
         * - radio_page
         * - smartradio_page
         * - search_page
         * - collection_track
         * - collection_playlist
         * - collection_album
         * - collection_radio
         * - artist_top
         * - artist_discography
         * - artist_radio
         * - artist_exclu
         * - artist_playlist
         * - album_discography
         * - tops_album
         * - tops_track
         * - recommendations_playlist
         * - recommendations_release
         * - recommendations_lastfm
         * - recommendations_album
         * - recommendations_friend_top
         * - recommendations_friend_share_track
         * - recommendations_friend_share_album
         * - recommendations_friend_share_playlist
         * - ticker_track
         * - ticker_playlist
         * - ticker_album
         * - profile_top
         * - profile_album
         * - profile_playlist
         * - selection_billboard
         * - selection_album
         * - selection_playlist
         * - selection_top
         * - selection_radio
         * - player_default_playlist
         * - contextmenu_playlist
         * - contextmenu_album
         * - facebook_track
         * - facebook_artist
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param sz_json            A JSON string representing the context.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_log_context(IntPtr self,
                                                           dz_activity_operation_callback cb,
                                                           IntPtr operation_userdata,
                                                           string sz_json);
        /**
         * @brief Set the UI context.
         *
         * Used by deezer for statistics purpose to improve client experience.
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param sz_id              Application context ID   as described for dz_player_set_log_context().
         * @param sz_type            Application context Type as described for dz_player_set_log_context().
         * @param sz_card            Application context Card as described for dz_player_set_log_context().
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_log_context3(IntPtr self,
                                                            dz_activity_operation_callback cb,
                                                            IntPtr operation_userdata,
                                                            string sz_id,
                                                            string sz_type,
                                                            string sz_card);

        /**
         * @brief Get the information for the renderer event.
         * @param self The event from which the information will be extracted.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_renderer_event_get_infos(IntPtr self);

                /**
         * @brief Register a on renderer event callback.
         *
         * Only one callback can be registered per player instance.
         * Should be done after dz_player_new() and before dz_player_load().
         *
         * @param self Deezer player handle.
         * @param cb   The dz_player_onerror_cb() callback.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_set_renderer_event_cb(IntPtr self,
                                                                 dz_player_onrendererevent_cb cb);

        /**
         * @brief Get information about a renderer state handle.
         *
         * @param self A renderer state handle that the infos need to be extracted.
         * @return A JSON string describing the state of the renderer.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_renderer_state_get_infos(IntPtr self);

        /**
         * @brief Get the current state of all renderers.
         *
         * Asynchronously return a IntPtr object.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_renderer_get_state(IntPtr self,
                                                              dz_activity_operation_callback cb,
                                                              IntPtr operation_userdata);

        /**
         * @brief Change/add/remove a renderer to the playback.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param json_list_of_renderer_id JSON list the renderer IDs like "[\"id1\",\"id2\"]".
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_renderer_change_selection(IntPtr self,
                                                                     dz_activity_operation_callback cb,
                                                                     IntPtr operation_userdata,
                                                                     string json_list_of_renderer_id);

        /**
         * @brief Change/add/remove a renderer to the playback.
         *
         * @param self               Deezer player handle.
         * @param cb                 The dz_activity_operation_callback() callback called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param renderer_id        The renderer id impacted.
         * @param volume             The volume to be applied to the renderer.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_player_renderer_set_volume(IntPtr self,
                                                               dz_activity_operation_callback cb,
                                                               IntPtr operation_userdata,
                                                               string renderer_id,
                                                               Int32 volume);
    }
}
