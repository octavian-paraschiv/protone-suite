using Newtonsoft.Json.Linq;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Runtime.Deezer;
using OPMedia.Runtime.ProTONE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Core.Logging;
using System.Diagnostics;

namespace OPMedia.Runtime.ProTONE.Rendering.DS
{
    public class DeezerRenderer : DsCustomRenderer
    {
        dz_connect_configuration _config = null;
        DeezerAppContext _ctx = null;

        const string USER_CACHE_PATH = "c:\\dzr\\dzrcache_NDK_SAMPLE";

        protected override void DoStartRendererWithHint(RenderingStartHint startHint)
        {
            if (renderMediaName == null || renderMediaName.Length <= 0)
                return;

            Logger.LogToConsole("DeezerRenderer::DoStartRendererWithHint startHint={0}", startHint);

            ResetAllEvents();

            if (_config == null)
            {
                _config = new dz_connect_configuration();

                _config.app_id = ProTONEConfig.DeezerApplicationId;
                _config.product_id = ApplicationInfo.ApplicationName;
                _config.product_build_id = SuiteVersion.Version;
                _config.user_profile_path = USER_CACHE_PATH;          // TODO SET THE USER CACHE PATH
                _config.connect_event_cb = new dz_connect_onevent_cb(OnApplicationConnectEvent);
                _config.app_has_crashed_delegate = new dz_connect_crash_reporting_delegate(OnApplicationError);
            }

            if (_ctx == null)
            {
                _ctx = new DeezerAppContext();
                _ctx.dzconnect = DeezerApi.dz_connect_new(_config);

                _ctx.renderProgressCB = new dz_player_onrenderprogress_cb(OnRenderProgress);
                _ctx.playerEventCB = new dz_player_onevent_cb(OnPlayerEvent);
                _ctx.rendererEventCB = new dz_player_onrendererevent_cb(OnRendererEvent);
            }

            dz_error_t err;

            err = DeezerApi.dz_connect_debug_log_disable(_ctx.dzconnect);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_connect_activate(_ctx.dzconnect, IntPtr.Zero);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            _ctx.activation_count++;
            
            err = DeezerApi.dz_connect_cache_path_set(_ctx.dzconnect, null, IntPtr.Zero, USER_CACHE_PATH);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_connect_set_access_token(_ctx.dzconnect, null, IntPtr.Zero, ProTONEConfig.DeezerUserAccessToken);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            // --------------------------------------------------------------------
            // Phase 1: Connect offline mode using dz_connect_offline_mode
            // This will trigger DZ_CONNECT_EVENT_USER_OFFLINE_AVAILABLE.
            // Upon completion, _evtAppUserOfflineAvailable will be set.
            _evtAppUserOfflineAvailable.Reset();
            err = DeezerApi.dz_connect_offline_mode(_ctx.dzconnect, null, IntPtr.Zero, false);

            if (_evtAppUserOfflineAvailable.WaitOne(10000) == false)
                DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_CONNECT_SESSION_NOT_ONLINE);
            else
                Logger.LogToConsole("DeezerRenderer::DoStartRendererWithHint dz_connect_offline_mode => DZ_CONNECT_EVENT_USER_OFFLINE_AVAILABLE");
            // --------------------------------------------------------------------
                        
            // --------------------------------------------------------------------
            // Phase 2: Create a new player object using dz_player_new
            // This will trigger DZ_CONNECT_EVENT_USER_LOGIN_OK.
            // Upon completion, _evtAppUserLoginOK will be set.
            _evtAppUserLoginOK.Reset();

            _ctx.dzplayer = DeezerApi.dz_player_new(_ctx.dzconnect);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            if (_evtAppUserLoginOK.WaitOne(10000) == false)
                DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_CONNECT_SESSION_LOGIN_FAILED);
            else
                Logger.LogToConsole("DeezerRenderer::DoStartRendererWithHint dz_player_new => DZ_CONNECT_EVENT_USER_LOGIN_OK");
            // --------------------------------------------------------------------


            // --------------------------------------------------------------------
            // Phase 3: Activate the player using dz_player_activate and set the callback delegates
            IntPtr pCtx = Marshal.AllocHGlobal(Marshal.SizeOf(_ctx));
            Marshal.StructureToPtr(_ctx, pCtx, false);

            err = DeezerApi.dz_player_activate(_ctx.dzplayer, pCtx);
            DeezerApi.ThrowExceptionForDzErrorCode(err);
            
            err = DeezerApi.dz_player_set_event_cb(_ctx.dzplayer, _ctx.playerEventCB);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_player_set_render_progress_cb(_ctx.dzplayer, _ctx.renderProgressCB, (UInt64)5e5);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_player_set_renderer_event_cb(_ctx.dzplayer, _ctx.rendererEventCB);
            DeezerApi.ThrowExceptionForDzErrorCode(err);
            // --------------------------------------------------------------------

            // --------------------------------------------------------------------
            // Phase 4: Load the playable content using dz_player_load
            // This will trigger DZ_PLAYER_EVENT_QUEUELIST_LOADED
            // Upon completion, _evtQueueListLoaded will be set.

            _ctx.sz_content_url = renderMediaName;

            _evtQueueListLoaded.Reset();
            err = DeezerApi.dz_player_load(_ctx.dzplayer, null, IntPtr.Zero, _ctx.sz_content_url);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            if (_evtQueueListLoaded.WaitOne(10000) == false)
                DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_CONNECT_SESSION_LOGIN_FAILED);
            else
                Logger.LogToConsole("DeezerRenderer::DoStartRendererWithHint dz_connect_offline_mode => DZ_CONNECT_EVENT_USER_LOGIN_OK");
            // --------------------------------------------------------------------

            // --------------------------------------------------------------------
            // Phase 5: Start playback using dz_player_play
            // This will trigger DZ_PLAYER_EVENT_RENDER_TRACK_START
            // Upon completion, _evtPlayerPlaybackStarted will be set.
            err = DeezerApi.dz_player_play(_ctx.dzplayer, null, IntPtr.Zero,
                       dz_player_play_command_t.DZ_PLAYER_PLAY_CMD_START_TRACKLIST,
                       DeezerInterop.PlayerApi.Constants.DZ_INDEX_IN_QUEUELIST_CURRENT);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            if (_evtPlayerPlaybackStarted.WaitOne(10000) == false)
                DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_RUNNABLE_NOT_STARTED);
            else
                Logger.LogToConsole("DeezerRenderer::DoStartRendererWithHint dz_player_play => DZ_PLAYER_EVENT_RENDER_TRACK_START");
            // --------------------------------------------------------------------
        }

        private void ResetAllEvents()
        {
            _evtAppUserLoginOK.Reset();
            _evtAppUserOfflineAvailable.Reset();
            _evtPlayerPaused.Reset();
            _evtPlayerStreamReadyAfterSeek.Reset();
            _evtQueueListLoaded.Reset();
            _evtPlayerPlaybackStarted.Reset();
        }

        protected override void DoStopRenderer()
        {
            StackTrace st = new StackTrace();
            Logger.LogToConsole("DeezerRenderer::DoStopRenderer call Stack = {0}", st.ToString());

            if (FilterState != FilterState.Stopped)
            {
                if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    err = DeezerApi.dz_player_stop(_ctx.dzplayer, null, IntPtr.Zero);
                    DeezerApi.ThrowExceptionForDzErrorCode(err);
                }
            }
        }

        protected override void DoPauseRenderer()
        {
            Logger.LogToConsole("DeezerRenderer::DoPauseRenderer");

            if (FilterState == FilterState.Running)
            {
                if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    _evtPlayerPaused.Reset();

                    err = DeezerApi.dz_player_pause(_ctx.dzplayer, null, IntPtr.Zero);
                    DeezerApi.ThrowExceptionForDzErrorCode(err);

                    if (_evtPlayerPaused.WaitOne(10000) == false)
                        DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_PLAYER_PAUSE_NOT_STARTED);
                    else
                        Logger.LogToConsole("DeezerRenderer::DoResumeRenderer player is now paused.");
                }
            }
        }

        protected override void DoResumeRenderer(double fromPosition)
        {
            Logger.LogToConsole("DeezerRenderer::DoResumeRenderer fromPosition={0}", fromPosition);

            dz_error_t err;

            if (_evtPlayerPaused.WaitOne(10000) == false)
                DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_PLAYER_PAUSE_NOT_STARTED);
            else
                Logger.LogToConsole("DeezerRenderer::DoResumeRenderer player is now paused.");

            if (FilterState == FilterState.Paused)
            {
                if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
                {
                    int resumePos = (int)fromPosition;
                    if (resumePos != RenderPosition)
                    {
                        // dz_player_seek will trigger DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY_AFTER_SEEK
                        // Upon completion, _evtPlayerStreamReadyAfterSeek will be set.
                        _evtPlayerStreamReadyAfterSeek.Reset();
                        err = DeezerApi.dz_player_seek(_ctx.dzplayer, null, IntPtr.Zero, (UInt64)(resumePos * 1e6));
                        DeezerApi.ThrowExceptionForDzErrorCode(err);

                        if (_evtPlayerStreamReadyAfterSeek.WaitOne(10000) == false)
                            DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_MEDIASTREAMER_SEEK_NOT_SEEKABLE);
                        else
                            Logger.LogToConsole("DeezerRenderer::DoResumeRenderer dz_player_seek completed with success, ready for dz_player_resume");
                    }

                    err = DeezerApi.dz_player_resume(_ctx.dzplayer, null, IntPtr.Zero);
                    DeezerApi.ThrowExceptionForDzErrorCode(err);
                }
            }
        }

        protected override void SetMediaPosition(double pos)
        {
            Logger.LogToConsole("DeezerRenderer::SetMediaPosition pos={0}", pos);

            if (FilterState != FilterState.Stopped)
            {
                if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    //if (FilterState == BaseClasses.FilterState.Running)
                    //{
                    //    err = DeezerApi.dz_player_pause(_ctx.dzplayer, null, IntPtr.Zero);
                    //    DeezerApi.ThrowExceptionForDzErrorCode(err);
                    //}

                    int resumePos = (int)pos;
                    if (resumePos != RenderPosition)
                    {
                        err = DeezerApi.dz_player_seek(_ctx.dzplayer, null, IntPtr.Zero, (UInt64)(resumePos * 1e6));
                        DeezerApi.ThrowExceptionForDzErrorCode(err);
                    }

                    if (_evtPlayerPaused.WaitOne(10000) == false)
                        DeezerApi.ThrowExceptionForDzErrorCode(dz_error_t.DZ_ERROR_PLAYER_PAUSE_NOT_STARTED);
                    else
                        Logger.LogToConsole("DeezerRenderer::SetMediaPosition player is now paused, OK to resume rendering");

                   
                }
            }
        }

        protected override bool IsMediaSeekable()
        {
            return true;
        }

        int _volume = -5000;


        protected override int GetAudioVolume()
        {
            return _volume;
        }

        protected override void SetAudioVolume(int vol)
        {
            if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
            {
                dz_error_t err;

                Logger.LogToConsole("DeezerRenderer::SetAudioVolume vol={0}", vol);

                err = DeezerApi.dz_player_set_output_volume(_ctx.dzplayer, null, IntPtr.Zero, vol);

                DeezerApi.ThrowExceptionForDzErrorCode(err);
            }
        }


        private bool OnApplicationError()
        {
            return true;
        }

        ManualResetEvent _evtAppUserOfflineAvailable = new ManualResetEvent(false);
        ManualResetEvent _evtAppUserLoginOK = new ManualResetEvent(false);

        private void OnApplicationConnectEvent(IntPtr handle, IntPtr evtHandle, IntPtr userData)
        {
            dz_connect_event_t evtType = DeezerApi.dz_connect_event_get_type(evtHandle);
            Logger.LogToConsole("DeezerRenderer::OnApplicationConnectEvent evtType={0}", evtType);

            switch (evtType)
            {
                case dz_connect_event_t.DZ_CONNECT_EVENT_USER_OFFLINE_AVAILABLE:
                    _evtAppUserOfflineAvailable.Set();
                    break;

                case dz_connect_event_t.DZ_CONNECT_EVENT_USER_LOGIN_OK:
                    _evtAppUserLoginOK.Set();
                    break;
            }
        }

        #region Duration and related

        int _duration = 0;

        ManualResetEvent _evtQueueListLoaded = new ManualResetEvent(false);

        ManualResetEvent _evtPlayerPaused= new ManualResetEvent(false);
        ManualResetEvent _evtPlayerStreamReadyAfterSeek = new ManualResetEvent(false);
        ManualResetEvent _evtPlayerPlaybackStarted = new ManualResetEvent(false);

        private void OnPlayerEvent(IntPtr handle, IntPtr evtHandle, IntPtr userdata)
        {
            dz_player_event_t evtType = DeezerApi.dz_player_event_get_type(evtHandle);
            Logger.LogToConsole("DeezerRenderer::OnPlayerEvent evtType={0}", evtType);

            switch (evtType)
            {
                case dz_player_event_t.DZ_PLAYER_EVENT_QUEUELIST_TRACK_SELECTED:
                    {
                        string selectedInfo = DeezerApi.dz_player_event_track_selected_dzapiinfo(evtHandle);
                        dynamic obj2 = JObject.Parse(selectedInfo);
                        _duration = obj2.duration;
                    }
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_START:
                    _evtPlayerPlaybackStarted.Set();
                    FilterState = FilterState.Running;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_RESUMED:
                    FilterState = FilterState.Running;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_PAUSED:
                    _evtPlayerPaused.Set();
                    FilterState = FilterState.Paused;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_END:
                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_REMOVED:
                    FilterState = FilterState.Stopped;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_QUEUELIST_LOADED:
                    _evtQueueListLoaded.Set();
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY_AFTER_SEEK:
                    _evtPlayerStreamReadyAfterSeek.Set();
                    break;

            }
        }

        protected override double GetMediaLength()
        {
            return _duration;
        }

        protected override bool IsEndOfMedia()
        {

            long nMediaPos = (long)GetMediaPosition();
            long nMediaLen = (long)GetMediaLength();
            long nElapsed = base._elapsedSeconds;

            return (nElapsed >= nMediaLen || nMediaPos >= nMediaLen);
        }
        #endregion

        #region Media position and related

        #region RenderPosition
        int _renderPosition = 0;
        object _renderPosLock = new object();

        private int RenderPosition
        {
            get
            {
                lock (_renderPosLock)
                {
                    return _renderPosition;
                }
            }

            set
            {
                lock (_renderPosLock)
                {
                    _renderPosition = value;
                };
            }
        }
        #endregion

        #region FilterState

        FilterState _filterState = FilterState.Stopped;
        object _fsLock = new object();

        private FilterState FilterState
        {
            get
            {
                lock (_fsLock)
                {
                    return _filterState;
                }
            }

            set
            {
                lock (_fsLock)
                {
                    _filterState = value;
                };
            }
        }
        #endregion

        private void OnRendererEvent(IntPtr handle, IntPtr evtHandle, IntPtr userdata)
        {
            string s = DeezerApi.dz_renderer_event_get_infos(evtHandle);
            Logger.LogToConsole("DeezerRenderer::OnRendererEvent info={0}", s ?? "<null>");
        }

        private void OnRenderProgress(IntPtr handle, UInt64 progress, IntPtr userdata)
        {
            int curRenderPos = (int)(progress / 1e6);

            if (curRenderPos != RenderPosition && curRenderPos > 0)
            {
                Logger.LogToConsole("DeezerRenderer::OnRenderProgress curRenderPos={0}", curRenderPos);
                RenderPosition = curRenderPos;
                FilterState = FilterState.Running;
            }
        }

        protected override double GetMediaPosition()
        {
            return RenderPosition;
        }

        protected override double DoGetMediaPosition()
        {
            return RenderPosition;
        }

        #endregion

        protected override bool IsAudioMediaAvailable()
        {
            return true;
        }

        protected override FilterState GetFilterState()
        {
            return FilterState;
        }

        protected override int GetScaledVolume(int rawVolume)
        {
            int dz_vol = (int)(0.01 * rawVolume);
            return dz_vol;
        }
    }
}
