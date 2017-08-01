﻿using Newtonsoft.Json.Linq;
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
            }

            dz_error_t err;

            err = DeezerApi.dz_connect_debug_log_disable(_ctx.dzconnect);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_connect_activate(_ctx.dzconnect, IntPtr.Zero);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            _ctx.activation_count++;
            
            err = DeezerApi.dz_connect_cache_path_set(_ctx.dzconnect, null, IntPtr.Zero, USER_CACHE_PATH);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            _ctx.dzplayer = DeezerApi.dz_player_new(_ctx.dzconnect);

            IntPtr pCtx = Marshal.AllocHGlobal(Marshal.SizeOf(_ctx));
            Marshal.StructureToPtr(_ctx, pCtx, false);

            err = DeezerApi.dz_player_activate(_ctx.dzplayer, pCtx);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_connect_set_access_token(_ctx.dzconnect, null, IntPtr.Zero, ProTONEConfig.DeezerUserAccessToken);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_connect_offline_mode(_ctx.dzconnect, null, IntPtr.Zero, false);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            _ctx.sz_content_url = renderMediaName;

            err = DeezerApi.dz_player_load(_ctx.dzplayer, null, IntPtr.Zero, _ctx.sz_content_url);
            DeezerApi.ThrowExceptionForDzErrorCode(err);
            
            err = DeezerApi.dz_player_set_event_cb(_ctx.dzplayer, _ctx.playerEventCB);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            err = DeezerApi.dz_player_set_render_progress_cb(_ctx.dzplayer, _ctx.renderProgressCB, (UInt64)5e5);
            DeezerApi.ThrowExceptionForDzErrorCode(err);

            Thread.Sleep(2000);

            err = DeezerApi.dz_player_play(_ctx.dzplayer, null, IntPtr.Zero,
                       dz_player_play_command_t.DZ_PLAYER_PLAY_CMD_START_TRACKLIST,
                       DeezerInterop.PlayerApi.Constants.DZ_INDEX_IN_QUEUELIST_CURRENT);

            DeezerApi.ThrowExceptionForDzErrorCode(err);
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

                    err = DeezerApi.dz_player_pause(_ctx.dzplayer, null, IntPtr.Zero);
                    DeezerApi.ThrowExceptionForDzErrorCode(err);
                }
            }
        }

        protected override void DoResumeRenderer(double fromPosition)
        {
            Logger.LogToConsole("DeezerRenderer::DoResumeRenderer fromPosition={0}", fromPosition);

            if (FilterState == FilterState.Paused)
            {
                if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
                {
                    dz_error_t err;

                    int resumePos = (int)fromPosition;
                    if (resumePos != RenderPosition)
                    {
                        Thread.Sleep(2000);

                        err = DeezerApi.dz_player_seek(_ctx.dzplayer, null, IntPtr.Zero, (UInt64)(resumePos * 1e6));
                        DeezerApi.ThrowExceptionForDzErrorCode(err);
                    }
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

                    int resumePos = (int)pos;
                    if (resumePos != RenderPosition)
                    {
                        err = DeezerApi.dz_player_seek(_ctx.dzplayer, null, IntPtr.Zero, (UInt64)(resumePos * 1e6));
                        DeezerApi.ThrowExceptionForDzErrorCode(err);
                    }
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
            Logger.LogToConsole("DeezerRenderer::SetAudioVolume vol={0}", vol);

            if (_ctx != null && _ctx.dzplayer != IntPtr.Zero)
            {
                dz_error_t err;

                int dz_vol = 100 * vol - 10000;

                err = DeezerApi.dz_player_set_output_volume(_ctx.dzplayer, null, IntPtr.Zero, dz_vol);
                DeezerApi.ThrowExceptionForDzErrorCode(err);
            }
        }


        private bool OnApplicationError()
        {
            return true;
        }

        private void OnApplicationConnectEvent(IntPtr handle, IntPtr evtHandle, IntPtr userData)
        {
            dz_connect_event_t evtType = DeezerApi.dz_connect_event_get_type(evtHandle);
            Logger.LogToConsole("DeezerRenderer::OnApplicationConnectEvent evtType={0}", evtType);
        }

        #region Duration and related

        int _duration = 0;

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
                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_RESUMED:
                    FilterState = FilterState.Running;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_PAUSED:
                    FilterState = FilterState.Paused;
                    break;

                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_END:
                case dz_player_event_t.DZ_PLAYER_EVENT_RENDER_TRACK_REMOVED:
                    FilterState = FilterState.Stopped;
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
        double _renderPosition = 0;
        object _renderPosLock = new object();

        private double RenderPosition
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


        private void OnRenderProgress(IntPtr handle, UInt64 progress, IntPtr userdata)
        {
            double curRenderPos = (double)progress / 1e6;

            if (curRenderPos != RenderPosition && curRenderPos > 0)
            {
                Logger.LogToConsole("DeezerRenderer::OnRenderProgress curRenderPos={0} [SET]", curRenderPos);
                RenderPosition = curRenderPos;
                FilterState = FilterState.Running;
            }
            else
                Logger.LogToConsole("DeezerRenderer::OnRenderProgress curRenderPos={0} [IGNORED]", curRenderPos);

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
    }
}
