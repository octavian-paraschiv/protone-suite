using Newtonsoft.Json.Linq;
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
using OPMedia.DeezerInterop.WorkerSupport;

namespace OPMedia.Runtime.ProTONE.Rendering.DS
{
    public class DeezerWorkerRenderer : DsCustomRenderer
    {
        WorkerProcess _wp = null;

        public override bool SupportsSampleGrabber => false;

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
                _wp = new WorkerProcess();
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
            Logger.LogTrace("DeezerRenderer::~DoDispose => Cleanup ...");
            DoStopRenderer();
        }
        

        protected override void DoStartRendererWithHint(RenderingStartHint startHint)
        {
            Logger.LogTrace("DeezerRenderer::DoStartRendererWithHint startHint={0}", startHint);

            if (renderMediaName == null || renderMediaName.Length <= 0)
                return;

            ResetWorker();

            _wp.Play(renderMediaName);

            InitAudioSampleCollector();
            CompleteAudioSampleCollectorIntialization();
        }


        protected override void DoStopRenderer()
        {
            StackTrace st = new StackTrace();
            Logger.LogTrace("DeezerRenderer::DoStopRenderer call Stack = {0}", st.ToString());

            if (FilterState != FilterState.Stopped)
            {
                if (_wp != null)
                {
                    _wp.Stop();
                    ReleaseAudioSampleCollector();
                }
            }
        }

        protected override void DoPauseRenderer()
        {
            Logger.LogTrace("DeezerRenderer::DoPauseRenderer");

            if (FilterState == FilterState.Running)
                _wp?.Pause();
        }

        protected override void DoResumeRenderer(double fromPosition)
        {
            Logger.LogTrace("DeezerRenderer::DoResumeRenderer fromPosition={0}", fromPosition);

            if (FilterState == FilterState.Paused)
                _wp?.Resume((int)fromPosition);
        }

        protected override void SetMediaPosition(double pos)
        {
            Logger.LogTrace("DeezerRenderer::SetMediaPosition pos={0}", pos);

            if (FilterState != FilterState.Stopped)
                _wp?.SetMediaPosition((int)pos);
        }

        protected override bool IsMediaSeekable()
        {
            return true;
        }

        volatile int _projVol = -5000;

        protected override int GetProjectedVolume()
        {
            return _projVol;
        }

        protected override void SetAudioVolume(int vol)
        {
            _wp?.SetVolume(vol);
            _projVol = vol;
        }


        private bool OnApplicationError()
        {
            return true;
        }

        #region Duration and related

        int _duration = 0;

        protected override bool IsEndOfMedia()
        {
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
            {
                string s = _wp.GetFilterState();
                Enum.TryParse<FilterState>(s, out fs);
                return fs;
            }

            return fs;
        }

        protected override int GetScaledVolume(int rawVolume)
        {
            int dz_vol = (int)(0.01 * rawVolume);
            return dz_vol;
        }
    }
}
