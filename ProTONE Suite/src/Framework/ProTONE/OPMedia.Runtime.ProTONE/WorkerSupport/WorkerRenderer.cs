﻿using Newtonsoft.Json.Linq;
using OPMedia.Core;
using OPMedia.Core.Configuration;
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
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.ExtendedInfo;

namespace OPMedia.Runtime.ProTONE.Rendering.WorkerSupport
{
    public class WorkerRenderer : DsCustomRenderer
    {
        WorkerProcess _wp = null;
        WorkerType _wt = WorkerType.Deezer;

        public override bool IsStreamedMedia
        {
            get
            {
                switch (_wt)
                {
                    case WorkerType.Shoutcast:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public override bool Valid
        {
            get
            {
                return (_wp != null && !_workerKilledOrCrashed);
            }
        }

        public WorkerRenderer(WorkerType workerType)
        {
            _wt = workerType;
        }

        private void ResetWorker()
        {
            if (_wp != null)
            {
                _wp.WorkerTerminated -= _wp_OnWorkerTerminated;
                _wp.RenderEvent -= _wp_RenderEvent;
                _wp.StateChanged -= _wp_StateChanged;

                _wp?.Dispose();
                _wp = null;
            }

            _workerKilledOrCrashed = false;

            if (_wp == null)
            {
                _wp = new WorkerProcess(_wt);
                _wp.WorkerTerminated += _wp_OnWorkerTerminated;
                _wp.RenderEvent += _wp_RenderEvent;
                _wp.StateChanged += _wp_StateChanged;
            }
        }

        private void _wp_StateChanged(string state)
        {
        }

        private void _wp_RenderEvent(int pos)
        {
        }

        bool _workerKilledOrCrashed = false;

        private void _wp_OnWorkerTerminated(int pid)
        {
            if (_wp?.Pid == pid)
            {
                _workerKilledOrCrashed = true;

                _wp.WorkerTerminated -= _wp_OnWorkerTerminated;
                _wp.RenderEvent -= _wp_RenderEvent;
                _wp.StateChanged -= _wp_StateChanged;

                _wp?.Dispose();
                _wp = null;
            }
        }

        protected override void DoDispose()
        {
            Logger.LogTrace($"BaseWorkerRenderer::~DoDispose => Cleanup ...");
            DoStopRenderer();
        }
        

        protected override void DoStartRendererWithHint(RenderingStartHint startHint)
        {
            Logger.LogTrace("BaseWorkerRenderer::DoStartRendererWithHint startHint={0}", startHint);

            if (renderMediaName == null || renderMediaName.Length <= 0)
                return;

            ResetWorker();

            int delayStart = 0;

            var hh = startHint as BookmarkStartHint;
            if (hh != null)
            {
                delayStart = (int)hh.Bookmark.PlaybackTimeInSeconds;
            }

            string userId = GetNextIdentity();

            _wp.Play(renderMediaName, userId, delayStart);
        }

        static string _userId = string.Empty;

        private string GetNextIdentity()
        {
            if (_wt == WorkerType.Deezer)
            {
                if (ProTONEConfig.XFade)
                {
                    _userId = ProTONEConfig.GetNextDeezerIdentity(_userId);
                }
                else
                {
                    _userId = ProTONEConfig.DeezerApplicationId;
                }
            }

            Logger.LogToConsole($"Next user identity for playback is: {_userId}");
            return _userId;
        }

        protected override void DoStopRenderer()
        {
            StackTrace st = new StackTrace();
            Logger.LogTrace("BaseWorkerRenderer::DoStopRenderer");
            Logger.LogToConsole(st.ToString());

            if (FilterState != FilterState.Stopped)
            {
                if (_wp != null)
                    _wp.Stop();
            }
        }

        protected override void DoPauseRenderer()
        {
            Logger.LogTrace("BaseWorkerRenderer::DoPauseRenderer");

            if (FilterState == FilterState.Running)
                _wp?.Pause();
        }

        protected override void DoResumeRenderer(double fromPosition)
        {
            Logger.LogTrace("BaseWorkerRenderer::DoResumeRenderer fromPosition={0}", fromPosition);

            if (FilterState == FilterState.Paused)
                _wp?.Resume((int)fromPosition);
        }

        protected override void SetMediaPosition(double pos)
        {
            Logger.LogTrace("BaseWorkerRenderer::SetMediaPosition pos={0}", pos);

            if (FilterState != FilterState.Stopped)
                _wp?.SetMediaPosition((int)pos);
        }

        protected override bool IsMediaSeekable()
        {
            return (_wt != WorkerType.Shoutcast);
        }

        protected override void SetAudioVolume(int vol)
        {
            _wp?.SetVolume(vol);
        }


        private bool OnApplicationError()
        {
            return true;
        }

        #region Duration and related

        int _duration = 0;

        protected override bool IsEndOfMedia()
        {
            if (_wt == WorkerType.Shoutcast)
                return false;

            if (_wp == null)
                return true;

            long nMediaPos = (long)GetMediaPosition();
            long nMediaLen = (long)GetMediaLength();
            long nElapsed = base._elapsedSeconds;

            return (nElapsed >= nMediaLen || nMediaPos >= nMediaLen);
        }
        #endregion

        #region Media position and related

        protected override double GetMediaPosition()
        {
            return DoGetMediaPosition();
        }

        protected override int GetAudioVolume()
        {
            int val = 0;

            if (_wp != null)
                val = _wp.GetVolume();

            return val;
        }

        protected override double GetMediaLength()
        {
            double val = 0;

            if (_wp != null)
                val = _wp.GetLength();

            return val;
        }

        protected override double DoGetMediaPosition()
        {
            double val = 0;

            if (_wp != null)
                val = _wp.GetMediaPosition();

            return val;
        }

        #endregion

        protected override bool IsAudioMediaAvailable()
        {
            return true;
        }

        protected override FilterState GetFilterState()
        {
            FilterState fs = FilterState.Stopped;

            if (_wp != null)
                return _wp.GetFilterState();

            return fs;
        }
        protected override int GetScaledVolume(int rawVolume)
        {
            if (_wt == WorkerType.Deezer)
                return rawVolume;

            return base.GetScaledVolume(rawVolume);
        }

        public override double PercentualVolume
        {
            get
            {
                if (_wt == WorkerType.Deezer)
                    return base.PercentualVolume;

                return base.PercentualVolume;
            }
        }

        public override SupportedMeteringData GetSupportedMeteringData()
        {
            if (_wp != null)
                return (SupportedMeteringData.Levels | _wp.GetSupportedMeteringData());

            return SupportedMeteringData.None;
        }

        public override double[] GetLevelsData()
        {
            if (_wp != null)
                return _wp.GetLevels();

            return null;
        }

        public override double[] GetWaveform()
        {
            if (_wp != null)
                return _wp.GetWaveform();

            return null;
        }

        public override double[] GetSpectrogram()
        {
            if (_wp != null)
                return _wp.GetSpectrogram();

            return null;
        }

    }
}