using Newtonsoft.Json.Linq;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.DeezerInterop.PlayerApi;
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
using OPMedia.Runtime.Shortcuts;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using System.IO;
using OPMedia.Runtime.ProTONE;

namespace OPMedia.DeezerWorker
{
    public partial class DeezerPlayer : IWorkerPlayer
    {
        dz_connect_configuration _dzConfig = null;

        IntPtr _dzConnect = IntPtr.Zero;
        IntPtr _dzPlayer = IntPtr.Zero;

        dz_player_onrenderprogress_cb _dzRenderProgressCB = null;
        dz_player_onevent_cb _dzPlayerEventCB = null;
        dz_player_onrendererevent_cb _dzRendererEventCB = null;

        dz_activity_operation_callback _dzPlayerDeactivatedCB = null;
        dz_activity_operation_callback _dzConnectDeactivatedCB = null;

        CommandProcessor _proc = null;

        bool _connected = false;

        string _userCachePath = null;

        public DeezerPlayer()
        {
            _userCachePath = PathUtils.GetCacheFolderPath("dzrcache");
        }

        private void CheckIfInitialized(string userId)
        {
            if (_connected == false)
            {
                Logger.LogTrace("DeezerPlayer::CheckIfInitialized => Initializing ...");
                SetupConfig(userId);
                SetupAppContext(userId);
            }
        }

        private void SetupConfig(string userId)
        {
            if (_dzConfig == null)
            {
                Logger.LogTrace("DeezerPlayer::SetupConfig => Creating config object ...");

                _dzConfig = new dz_connect_configuration();
                _dzConfig.app_id = userId;
                _dzConfig.product_id = ApplicationInfo.ApplicationName;
                _dzConfig.product_build_id = SuiteVersion.Version;
                _dzConfig.user_profile_path = _userCachePath;
                _dzConfig.connect_event_cb = new dz_connect_onevent_cb(OnApplicationConnectEvent);
                _dzConfig.app_has_crashed_delegate = new dz_connect_crash_reporting_delegate(OnAppCrashed);
            }
        }

        private bool OnAppCrashed()
        {
            Logger.LogTrace("DeezerPlayer::OnAppCrashed called ...");
            return false;
        }

        private void SetupAppContext(string userId)
        {
            if (_connected == false)
            {
                _evtAppUserOfflineAvailable.Reset();
                _evtAppUserLoginOK.Reset();

                Logger.LogTrace("DeezerPlayer::SetupAppContext => Creating app context ...");

                dz_error_t err;

                _dzRenderProgressCB = new dz_player_onrenderprogress_cb(OnRenderProgress);
                _dzPlayerEventCB = new dz_player_onevent_cb(OnPlayerEvent);
                _dzRendererEventCB = new dz_player_onrendererevent_cb(OnRendererEvent);

                _dzConnect = DeezerApi.dz_connect_new(_dzConfig);
                if (_dzConnect == IntPtr.Zero)
                    HandleDzErrorCode("dz_connect_new", WorkerError.CannotConnectToMedia, dz_error_t.DZ_ERROR_CLASS_INSTANTIATION);

                err = DeezerApi.dz_connect_activate(_dzConnect, IntPtr.Zero);
                HandleDzErrorCode("dz_connect_activate", WorkerError.CannotConnectToMedia, err);

                err = DeezerApi.dz_connect_cache_path_set(_dzConnect, null, IntPtr.Zero, _userCachePath);
                HandleDzErrorCode("dz_connect_cache_path_set", WorkerError.CannotConnectToMedia, err);

                _dzPlayer = DeezerApi.dz_player_new(_dzConnect);
                if (_dzPlayer == IntPtr.Zero)
                    HandleDzErrorCode("dz_player_new", WorkerError.CannotConnectToMedia, dz_error_t.DZ_ERROR_CLASS_INSTANTIATION);

                err = DeezerApi.dz_player_activate(_dzPlayer, IntPtr.Zero);
                HandleDzErrorCode("dz_player_activate", WorkerError.CannotConnectToMedia, err);

                err = DeezerApi.dz_player_set_event_cb(_dzPlayer, _dzPlayerEventCB);
                HandleDzErrorCode("dz_player_set_event_cb", WorkerError.CannotConnectToMedia, err);

                // dz_player_set_metadata_cb

                err = DeezerApi.dz_player_set_render_progress_cb(_dzPlayer, _dzRenderProgressCB, (UInt64)5e5);
                HandleDzErrorCode("dz_player_set_render_progress_cb", WorkerError.CannotConnectToMedia, err);

                err = DeezerApi.dz_player_set_renderer_event_cb(_dzPlayer, _dzRendererEventCB);
                HandleDzErrorCode("dz_player_set_renderer_event_cb", WorkerError.CannotConnectToMedia, err);

                string token = ProTONEConfig.DeezerUserAccessToken;

                err = DeezerApi.dz_connect_set_access_token(_dzConnect, null, IntPtr.Zero, token);
                HandleDzErrorCode("dz_connect_set_access_token", WorkerError.CannotConnectToMedia, err);

                err = DeezerApi.dz_player_set_crossfading_duration(_dzPlayer, null, IntPtr.Zero, 3000);
                HandleDzErrorCode("dz_player_set_crossfading_duration", WorkerError.CannotConnectToMedia, err);

                err = DeezerApi.dz_connect_offline_mode(_dzConnect, null, IntPtr.Zero, false);
                HandleDzErrorCode("dz_connect_offline_mode", WorkerError.CannotConnectToMedia, err);

                err = DeezerApi.dz_player_set_track_quality(_dzPlayer, null, IntPtr.Zero, 
                    dz_track_quality_t.DZ_TRACK_QUALITY_CDQUALITY);
                HandleDzErrorCode("dz_player_set_track_quality", WorkerError.CannotConnectToMedia, err);

                if (_evtAppUserLoginOK.WaitOne(Worker.OperationTimeout) == false)
                    HandleDzErrorCode("DeezerPlayer::SetupConfig", WorkerError.CannotConnectToMedia, dz_error_t.DZ_ERROR_CONNECT_SESSION_LOGIN_FAILED);

                _connected = true;
            }
        }


        public void OnConnectDeactivated(IntPtr userData, IntPtr operation_userdata, dz_error_t status, IntPtr result)
        {
            Logger.LogTrace("DeezerPlayer::OnConnectDeactivated ...");
            _evtConnectDeactivated.Set();
        }

        public void OnPlayerDeactivated(IntPtr userData, IntPtr operation_userdata, dz_error_t status, IntPtr result)
        {
            Logger.LogTrace("DeezerPlayer::OnPlayerDeactivated ...");
            _evtPlayerDeactivated.Set();
        }


        public void Play(string url, string userId, int delayStart, long renderHwnd, long notifyHwnd)
        {
            dz_error_t err;

            _needNaturalNext = false;

            Logger.LogTrace("DeezerPlayer::Play url={0}", url);

            CheckIfInitialized(userId);

            if (_dzConnect == IntPtr.Zero || _dzPlayer == IntPtr.Zero)
                HandleDzErrorCode("DeezerPlayer::Play", WorkerError.CannotConnectToMedia, dz_error_t.DZ_ERROR_CLASS_INSTANTIATION);

            // --------------------------------------------------------------------
            _evtQueueListLoaded.Reset();
            err = DeezerApi.dz_player_load(_dzPlayer, null, IntPtr.Zero, url);
            HandleDzErrorCode("dz_player_load", WorkerError.MediaReadError, err);

            if (_evtQueueListLoaded.WaitOne(Worker.OperationTimeout) == false)
                HandleDzErrorCode("dz_player_load", WorkerError.MediaReadError, dz_error_t.DZ_ERROR_PLAYER_LOAD_TIMEOUT);

            Logger.LogTrace("dz_player_load => Success");
            // --------------------------------------------------------------------


            // --------------------------------------------------------------------
            // Start playback using dz_player_play
            // This will trigger DZ_PLAYER_EVENT_RENDER_TRACK_START
            // Upon completion, _evtPlayerPlaybackStarted will be set.
            _evtPlayerPlaybackStarted.Reset();

            // for playback from offset: dz_player_resume

            err = DeezerApi.dz_player_play(_dzPlayer, null, IntPtr.Zero,
                       dz_player_play_command_t.DZ_PLAYER_PLAY_CMD_START_TRACKLIST,
                       DeezerInterop.PlayerApi.Constants.DZ_INDEX_IN_QUEUELIST_CURRENT);

            err = DeezerApi.dz_player_set_output_mute(_dzPlayer, null, IntPtr.Zero, true);
            HandleDzErrorCode("dz_player_set_output_mute", WorkerError.RenderingError, err);

            Logger.LogTrace("dz_player_set_output_mute => Success");

            HandleDzErrorCode("dz_player_play", WorkerError.RenderingError, err);

            if (_evtPlayerPlaybackStarted.WaitOne(Worker.OperationTimeout) == false)
                HandleDzErrorCode("dz_player_play", WorkerError.RenderingError, dz_error_t.DZ_ERROR_PLAYER_PLAY_TIMEOUT);

            Logger.LogTrace("dz_player_play => Success");
            // --------------------------------------------------------------------

            //if (delayStart > 0)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        Thread.Sleep(500);
            //        Pause();
            //        Thread.Sleep(500);
            //        Resume(delayStart);
            //    });
            //}
        }

        ManualResetEvent _evtAppUserOfflineAvailable = new ManualResetEvent(false);
        ManualResetEvent _evtAppUserLoginOK = new ManualResetEvent(false);

        ManualResetEvent _evtQueueListLoaded = new ManualResetEvent(false);
        ManualResetEvent _evtPlayerPaused = new ManualResetEvent(false);
        ManualResetEvent _evtPlayerStreamReadyAfterSeek = new ManualResetEvent(false);

        ManualResetEvent _evtPlayerPlaybackStarted = new ManualResetEvent(false);
        ManualResetEvent _evtPlayerPlaybackStopped = new ManualResetEvent(false);

        ManualResetEvent _evtPlayerDeactivated = new ManualResetEvent(false);
        ManualResetEvent _evtConnectDeactivated = new ManualResetEvent(false);

        public void Stop()
        {
            Logger.LogTrace("DeezerPlayer::Stop");

            if (FilterState != FilterState.Stopped)
            {
                if (_dzPlayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    _evtPlayerPlaybackStopped.Reset();

                    err = DeezerApi.dz_player_stop(_dzPlayer, null, IntPtr.Zero);
                    HandleDzErrorCode("dz_player_stop", WorkerError.RenderingError, err);

                    if (_evtPlayerPlaybackStopped.WaitOne(Worker.OperationTimeout) == false)
                        HandleDzErrorCode("dz_player_pause", WorkerError.RenderingError, dz_error_t.DZ_ERROR_PLAYER_STOP_TIMEOUT);

                    RenderPosition = 0;

                    Logger.LogTrace("dz_player_stop => Success");

                    //// Cleanup cache
                    //if (Directory.Exists(_userCachePath))
                    //{
                    //    Directory.Delete(_userCachePath, true);
                    //}
                }
            }
        }

        public void Pause()
        {
            Logger.LogTrace("DeezerPlayer::Pause");

            if (FilterState == FilterState.Running)
            {
                if (_dzPlayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    _evtPlayerPaused.Reset();

                    err = DeezerApi.dz_player_pause(_dzPlayer, null, IntPtr.Zero);
                    HandleDzErrorCode("dz_player_pause", WorkerError.RenderingError, err);

                    if (_evtPlayerPaused.WaitOne(Worker.OperationTimeout) == false)
                        HandleDzErrorCode("dz_player_pause", WorkerError.RenderingError, dz_error_t.DZ_ERROR_PLAYER_PAUSE_TIMEOUT);

                    Logger.LogTrace("dz_player_pause => Success");
                }
            }
        }

        public void Resume(int pos)
        {
            Logger.LogTrace("DeezerPlayer::Resume fromPosition={0}", pos);

            dz_error_t err;

            if (_evtPlayerPaused.WaitOne(Worker.OperationTimeout) == false)
                HandleDzErrorCode("DeezerPlayer::Resume", WorkerError.RenderingError, dz_error_t.DZ_ERROR_PLAYER_PAUSE_NOT_STARTED);

            Logger.LogTrace("DeezerPlayer::Resume player is now paused.");

            if (FilterState == FilterState.Paused)
            {
                if (_dzPlayer != IntPtr.Zero)
                {
                    int resumePos = (int)pos;
                    if (resumePos != RenderPosition)
                    {
                        // dz_player_seek will trigger DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY_AFTER_SEEK
                        // Upon completion, _evtPlayerStreamReadyAfterSeek will be set.
                        _evtPlayerStreamReadyAfterSeek.Reset();
                        err = DeezerApi.dz_player_seek(_dzPlayer, null, IntPtr.Zero, (UInt64)(resumePos * 1e6));
                        HandleDzErrorCode("dz_player_seek", WorkerError.RenderingError, err);

                        if (_evtPlayerStreamReadyAfterSeek.WaitOne(Worker.OperationTimeout) == false)
                            HandleDzErrorCode("dz_player_seek", WorkerError.RenderingError, dz_error_t.DZ_ERROR_PLAYER_SEEK_TIMEOUT);

                        Logger.LogTrace("dz_player_seek => DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY_AFTER_SEEK");
                    }

                    err = DeezerApi.dz_player_resume(_dzPlayer, null, IntPtr.Zero);
                    HandleDzErrorCode("dz_player_resume", WorkerError.RenderingError, err);
                }
            }
        }

        public void SetMediaPosition(int pos)
        {
            Logger.LogTrace("DeezerPlayer::SetMediaPosition pos={0}", pos);

            if (FilterState != FilterState.Stopped)
            {
                if (_dzPlayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    int resumePos = (int)pos;
                    if (resumePos != RenderPosition)
                    {
                        err = DeezerApi.dz_player_seek(_dzPlayer, null, IntPtr.Zero, (UInt64)(resumePos * 1e6));
                        HandleDzErrorCode("dz_player_seek", WorkerError.RenderingError, err);
                    }

                    if (_evtPlayerPaused.WaitOne(Worker.OperationTimeout) == false)
                        HandleDzErrorCode("[Event toggled by dz_player_seek]", WorkerError.RenderingError, dz_error_t.DZ_ERROR_PLAYER_SEEK_TIMEOUT);

                    Logger.LogTrace("[Event toggled by dz_player_seek] => DZ_PLAYER_EVENT_RENDER_TRACK_PAUSED");
                }
            }
        }

        volatile int _volume = -5000;

        public int GetVolume()
        {
            return _volume;
        }

        public void SetVolume(int vol)
        {
            if (_dzPlayer != IntPtr.Zero)
            {
                dz_error_t err;

                Logger.LogTrace("DeezerPlayer::SetVolume vol={0}", vol);

                err = DeezerApi.dz_player_set_output_volume(_dzPlayer, null, IntPtr.Zero, vol);

                HandleDzErrorCode("dz_player_set_output_volume", WorkerError.RenderingError, err);

                if (vol > 0)
                {
                    err = DeezerApi.dz_player_set_output_mute(_dzPlayer, null, IntPtr.Zero, false);
                }

                _volume = vol;
            }
        }


        private bool OnApplicationError()
        {
            return true;
        }

        private void OnApplicationConnectEvent(IntPtr handle, IntPtr evtHandle, IntPtr userData)
        {
            dz_connect_event_t evtType = DeezerApi.dz_connect_event_get_type(evtHandle);
            Logger.LogTrace("DeezerPlayer::OnApplicationConnectEvent evtType={0}", evtType);

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
        bool _needNaturalNext = false;
        bool _isForcedPause = false;

        private void OnPlayerEvent(IntPtr handle, IntPtr evtHandle, IntPtr userdata)
        {
            dz_player_event_t evtType = DeezerApi.dz_player_event_get_type(evtHandle);
            Logger.LogTrace("DeezerPlayer::OnPlayerEvent evtType={0}", evtType);

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
                    if (_isForcedPause)
                    {
                        //Thread.Sleep(10);
                        Logger.LogTrace("DeezerPlayer::OnPlayerEvent: calling dz_player_resume after DZ_PLAYER_EVENT_LIMITATION_FORCED_PAUSE");
                        _isForcedPause = false;
                        dz_error_t err = DeezerApi.dz_player_resume(_dzPlayer, null, IntPtr.Zero);
                        HandleDzErrorCode("dz_player_resume after DZ_PLAYER_EVENT_LIMITATION_FORCED_PAUSE", WorkerError.RenderingError, err);
                    }
                    else
                    {
                        _evtPlayerPaused.Set();
                        FilterState = FilterState.Paused;
                    }
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_END:
                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_REMOVED:
                    if (_needNaturalNext)
                    {
                        // HACK: The Deezer Native SDK told us that we should advance 
                        // to the next item by using a "natural next" (WTF is that anyways ??)
                        _needNaturalNext = false;
                        // So this is exactly what we'll give it ...
                        OPMShortcutEventArgs args = new OPMShortcutEventArgs(OPMShortcut.CmdNext);
                        EventDispatch.DispatchEvent(EventNames.ExecuteShortcut, args);
                    }
                    else
                    {
                        _evtPlayerPlaybackStopped.Set();
                        FilterState = FilterState.Stopped;
                        RenderPosition = 0;
                    }
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_MEDIASTREAM_DATA_READY_AFTER_SEEK:
                    _evtPlayerStreamReadyAfterSeek.Set();
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_QUEUELIST_NEED_NATURAL_NEXT:
                    _needNaturalNext = true;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_QUEUELIST_LOADED:
                    _evtQueueListLoaded.Set();
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_QUEUELIST_NO_RIGHT:
                    _evtQueueListLoaded.Reset();
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_LIMITATION_FORCED_PAUSE:
                    _isForcedPause = true;
                    break;
            }
        }

        public int GetLength()
        {
            return _duration;
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
            Logger.LogTrace("DeezerPlayer::OnRendererEvent info={0}", s ?? "<null>");
        }

        private void OnRenderProgress(IntPtr handle, UInt64 progress, IntPtr userdata)
        {
            int curRenderPos = (int)(progress / 1e6);

            if (curRenderPos != RenderPosition && curRenderPos > 0)
            {
                Logger.LogToConsole("DeezerPlayer::OnRenderProgress curRenderPos={0}", curRenderPos);
                RenderPosition = curRenderPos;
                FilterState = FilterState.Running;
            }
        }

        public int GetMediaPosition()
        {
            return RenderPosition;
        }

        public void SetCommandProcessor(CommandProcessor proc)
        {
            _proc = proc;
        }
        #endregion

        public FilterState GetFilterState()
        {
            return this.FilterState;
        }
    }
}
