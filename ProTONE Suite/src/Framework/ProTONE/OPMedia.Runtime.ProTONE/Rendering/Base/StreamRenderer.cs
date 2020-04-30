#region Copyright © 2006 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	MediaRenderer.cs
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.ExtendedInfo;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.WorkerSupport;
#endregion

namespace OPMedia.Runtime.ProTONE.Rendering.Base
{
    public abstract class StreamRenderer : IDisposable
    {
        #region Members
        protected Control renderRegion = null;
        protected Control messageDrain = null;
        protected string renderMediaName = string.Empty;
        protected List<string> supportedMediaTypes = null;

        protected MediaFileInfo renderMediaInfo = MediaFileInfo.Empty;

        protected object _sync = new object();
        #endregion

        protected System.Timers.Timer _tmrInternalClock = null;
        protected Int64 _elapsedSeconds = 0;
        protected object _syncElapsedSeconds = new object();

        public virtual bool Valid
        {
            get
            {
                return (IsVideoMediaAvailable() || IsAudioMediaAvailable());
            }
        }


        public override string ToString()
        {
            return $"{GetType().Name} Media={renderMediaName} VOL={AudioVolume}";
        }

        #region Properties

        internal bool MediaSeekable
        { 
            get 
            {
                return IsMediaSeekable(); 
            } 
        }

        internal double DurationScaleFactor
        {
            get
            {
                return GetDurationScaleFactor();
            }
        }


        internal double MediaLength
        { 
            get 
            {
                return GetMediaLength(); 
            } 
        }

        internal double MediaPosition
        { 
            get 
            {
                return DoGetMediaPosition();
                //return GetMediaPosition(); 
            } 
            set 
            {
                SetMediaPosition(value); 
            } 
        }

        protected virtual double DoGetMediaPosition()
        {
            lock (_syncElapsedSeconds)
            {
                return _elapsedSeconds;
            }
        }

        internal bool AudioMediaAvailable
        { 
            get 
            {
                return IsAudioMediaAvailable(); 
            } 
        }

        internal bool VideoMediaAvailable
        {
            get
            {
                return IsVideoMediaAvailable();
            }
        }

        internal OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState FilterState
        { 
            get 
            {
                return GetFilterState(); 
            } 
        }

        public virtual double PercentualVolume
        {
            get
            {
                return (double)ProTONEConfig.LastVolume / 10000f;
            }
        }

        internal int AudioVolume
        {
            get 
            {
                int vol = GetAudioVolume();
                return vol;
            } 
            set 
            {
                int vol = value;
                if (vol < 0)
                    vol = 0;
                if (vol > 100)
                    vol = 100;

                SetAudioVolume(vol); 
            } 
        }

        internal int AudioBalance
        {
            get
            {
                return GetAudioBalance();
            }
            set
            {
                SetAudioBalance(value);
            }
        }

        internal Control MessageDrain
        {
            get { return messageDrain; }
            set { messageDrain = value; }
        }

        internal bool ShowCursor
        {
            get { return IsCursorVisible(); }
            set { DoShowCursor(value); }
        }

        internal bool FullScreen
        {
            get { return IsFullScreen(); }
            set { DoSetFullScreen(value); }
        }

        internal object GraphFilter
        { get { return DoGetGraphFilter(); } }

        #endregion

        #region Methods
        internal string RenderMediaName
        {
            get
            {
                return renderMediaName;
            }

            set
            {
                renderMediaName = value;
            }
        }


        internal MediaFileInfo RenderMediaInfo
        {
            get
            {
                return renderMediaInfo;
            }
        }

        internal void StartRendererWithHint(RenderingStartHint startHint)
        {
            int hintPos = 0;
            if (startHint is BookmarkStartHint)
                hintPos = (int)(startHint as BookmarkStartHint).Bookmark.PlaybackTimeInSeconds;
                
            CreateAndStartInternalClock(hintPos); 
            DoStartRendererWithHint(startHint);
        }

        internal void StartRenderer()
        {
            CreateAndStartInternalClock();
            DoStartRenderer();
        }

        internal void PauseRenderer()
        {
            PauseInternalClock();
            DoPauseRenderer();
        }

        internal void StopRenderer()
        {
            StopInternalClock();
            DoStopRenderer();
        }

        internal void ResumeRenderer(double fromPosition)
        {
            ResumeInternalClock((int)fromPosition);
            DoResumeRenderer(fromPosition);
        }

        private void CreateAndStartInternalClock(int elapsedSeconds = 0)
        {
            lock (_syncElapsedSeconds)
            {
                _elapsedSeconds = elapsedSeconds;
            }

            if (_tmrInternalClock == null)
            {
                _tmrInternalClock = new System.Timers.Timer();
                _tmrInternalClock.AutoReset = true;
                _tmrInternalClock.Interval = 1000;
                _tmrInternalClock.Elapsed += new System.Timers.ElapsedEventHandler(OnInternalClock);
            }

            _tmrInternalClock.Stop();
            _tmrInternalClock.Start();
        }

        void OnInternalClock(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_syncElapsedSeconds)
            {
                _elapsedSeconds++;
            }
        }

        private void ResumeInternalClock(int newPosition)
        {
            if (_tmrInternalClock != null)
                _tmrInternalClock.Start();

            lock (_syncElapsedSeconds)
            {
                _elapsedSeconds = newPosition;
            }
        }

        private void PauseInternalClock()
        {
            if (_tmrInternalClock != null)
                _tmrInternalClock.Stop();
        }

        private void StopInternalClock()
        {
            if (_tmrInternalClock != null)
                _tmrInternalClock.Stop(); 

            lock (_syncElapsedSeconds)
            {
                _elapsedSeconds = 0;
            }
        }

        internal bool EndOfMedia
        {
            get
            {
                bool ret = IsEndOfMedia();
                
                if (ret)
                    StopInternalClock();
                
                return ret;
            }
        }

        internal int GetSubtitleStream()
        {
            return DoGetSubtitleStream();
        }

        internal void SetSubtitleStream(int sid)
        {
            DoSetSubtitleStream(sid);
        }

        internal virtual void SetRenderRegion(IWin32Window renderRegion, IWin32Window notifyRegion)
        {
        }

        #endregion

        #region Construction
        public StreamRenderer()
        {
            //ResetVolumeLevels();
        }
        #endregion

        #region Implementation

        //protected void ResetVolumeLevels()
        //{
        //    VolumeAvgL = 0;
        //    VolumeAvgR = 0;
        //    VolumePeakL = 0;
        //    VolumePeakR = 0;
        //}

        #region IDisposable Members
        public void Dispose()
        {
            DoDispose();
        }

        protected virtual void DoDispose()
        {
            DoStopRenderer();
        }

        #endregion

        protected abstract void DoStartRenderer();
        protected abstract void DoStartRendererWithHint(RenderingStartHint startHint);

        protected abstract void DoPauseRenderer();
        protected abstract void DoStopRenderer();
        protected abstract void DoResumeRenderer(double fromPosition);

        protected abstract double GetMediaLength();
        protected abstract double GetMediaPosition();
        protected abstract double GetDurationScaleFactor();

        protected abstract void SetMediaPosition(double pos);

        protected abstract int GetAudioVolume();
        protected abstract void SetAudioVolume(int b);

        protected abstract int GetAudioBalance();
        protected abstract void SetAudioBalance(int b);

        protected abstract bool IsAudioMediaAvailable();
        protected abstract bool IsVideoMediaAvailable();
        protected abstract bool IsMediaSeekable();

        protected abstract OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState GetFilterState();

        protected abstract bool IsCursorVisible();
        protected abstract void DoShowCursor(bool show);

        protected abstract bool IsEndOfMedia();

        protected abstract int DoGetSubtitleStream();
        protected abstract void DoSetSubtitleStream(int sid);

        protected abstract bool IsFullScreen();
        protected abstract void DoSetFullScreen(bool fullScreen);

        protected abstract object DoGetGraphFilter();
        

        #endregion

        public virtual bool IsStreamedMedia
        {
            get
            {
                return false;
            }
        }
    }
}

