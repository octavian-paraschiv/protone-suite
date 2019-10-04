using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;

namespace OPMedia.VideoDVDWorker
{
    public class VideoDVDPlayer : IWorkerPlayer
    {
        CommandProcessor _proc = null;

        protected IMediaControl mediaControl = null;
        protected IBasicAudio basicAudio = null;
        protected IMediaPosition mediaPosition = null;
        protected IVideoWindow videoWindow = null;
        protected IBasicVideo basicVideo = null;
        protected IMediaEventEx mediaEvent = null;

        protected double durationScaleFactor = 1;
        protected MediaFileInfo renderMediaInfo = MediaFileInfo.Empty;

        public VideoDVDPlayer()
        {
        }

        public int GetLength()
        {
            double val = 0;
            bool gotTimeFromMediaInfo = false;
            double actualMediaLength = GetMediaLength();

            if (renderMediaInfo != null)
            {
                val = renderMediaInfo.Duration.GetValueOrDefault().TotalSeconds;
                gotTimeFromMediaInfo = (val > 0f);
            }

            if (!gotTimeFromMediaInfo)
                val = actualMediaLength;

            if (Math.Abs(actualMediaLength) > double.Epsilon)
            {
                durationScaleFactor = val / actualMediaLength;
            }
            else
            {
                durationScaleFactor = 0;
            }

            return (int)val;

        }

        private double GetMediaLength()
        {
            double val = 0;

            if (mediaPosition != null)
            {
                int hr = mediaPosition.get_Duration(out val);
                if (hr >= 0)
                    return (val + double.Epsilon);
            }

            return double.Epsilon;
        }

        public int GetMediaPosition()
        {
            double val = 0;

            if (mediaPosition != null)
            {
                int hr = mediaPosition.get_CurrentPosition(out val);
                DsError.ThrowExceptionForHR(hr);
            }

            return (int)(val * durationScaleFactor);
        }

        public int GetVolume()
        {
            return _vol;
        }

        public void Pause()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Pause();
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
        }

        public void Play(string url, string userId, int delayStart, long renderHwnd, long notifyHwnd)
        {
            if (url == null || url.Length <= 0)
                return;

            InitMedia(url, renderHwnd, notifyHwnd);

            int hr = mediaPosition.put_Rate(1);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            // Run the graph to play the media file
            hr = mediaControl.Run();
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            // HACK: call GetLength once here to ensure that durationScaleFactor is buuilt up
            GetLength();
        }

        private void InitMedia(string url, long renderHwnd, long notifyHwnd)
        {
            mediaControl = BuildMediaControl();
            mediaPosition = mediaControl as IMediaPosition;
            basicAudio = mediaControl as IBasicAudio;
            videoWindow = mediaControl as IVideoWindow;
            basicVideo = mediaControl as IBasicVideo;
            mediaEvent = mediaControl as IMediaEventEx;

            int hr = mediaControl.RenderFile(url);
            WorkerException.ThrowForHResult(WorkerError.MediaReadError, hr);

            hr = basicAudio.put_Volume((int)VolumeRange.Minimum);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            int w = 0;
            hr = basicVideo.get_VideoWidth(out w);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            SetRenderRegion(renderHwnd, notifyHwnd);
        }

        private IMediaControl BuildMediaControl()
        {
            Type mediaControlType = Type.GetTypeFromCLSID(Filters.FilterGraph, true);
            return Activator.CreateInstance(mediaControlType) as IMediaControl;
        }

        public void Resume(int pos)
        {
            SetMediaPosition(pos);

            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
        }

        public void SetMediaPosition(int pos)
        {
            if (mediaPosition != null)
            {
                int hr = mediaPosition.put_CurrentPosition(pos / durationScaleFactor);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        int _vol = 0;

        public void SetVolume(int vol)
        {
            Logger.LogTrace($"SetVolume to {vol}");

            var dsVolume = MapVolume(vol);
            int hr = basicAudio.put_Volume(dsVolume);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            _vol = vol;
        }

        public void Stop()
        {
            mediaControl?.Stop();

            mediaControl = null;
            basicAudio = null;
            mediaPosition = null;
        }

        public void SetCommandProcessor(CommandProcessor proc)
        {
            _proc = proc;
        }

        public FilterState GetFilterState()
        {
            FilterState fs = FilterState.Stopped;
            if (mediaControl != null)
            {
                int hr = mediaControl.GetState(0, out fs);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }

            return fs;
        }

        private int MapVolume(int rawVolume)
        {
            double a = (-1000 / Math.Log10(0.5));
            double b = 0.01;
            double c = 0.0976;
            double x = (double)(rawVolume);
            double logVolume = a * Math.Log10(b * (x + c));
            int scaledVolume = (int)logVolume;
            if (logVolume < -10000)
            {
                scaledVolume = -10000;
            }
            else if (logVolume > 0)
            {
                scaledVolume = 0;
            }

            return scaledVolume;
        }

        IntPtr _renderHandle = IntPtr.Zero;
        IntPtr _notifyHandle = IntPtr.Zero;

        public void SetRenderRegion(long renderHwnd, long notifyHwnd)
        {
            int hr = 0;

            _renderHandle = new IntPtr((int)renderHwnd);
            _notifyHandle = new IntPtr((int)notifyHwnd);

            if (videoWindow != null)
            {
                hr = videoWindow.put_Owner(_renderHandle);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

                hr = videoWindow.put_MessageDrain(_renderHandle);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }

            if (videoWindow != null)
            {
                hr = videoWindow.put_WindowStyle((int)(WindowStyle.Child |
                    WindowStyle.ClipSiblings |
                    WindowStyle.ClipChildren));
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }

            if (mediaEvent != null)
            {
                hr = mediaEvent.SetNotifyWindow(_notifyHandle, (int)Messages.WM_GRAPH_EVENT, IntPtr.Zero);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
        }

        internal int VideoWidth
        {
            get
            {
                int w = 0;
                int hr = basicVideo.get_VideoWidth(out w);
                if (hr >= 0)
                    return w;

                return 0;
            }
        }

        internal int VideoHeight
        {
            get
            {
                int h = 0;
                int hr = basicVideo.get_VideoHeight(out h);
                if (hr >= 0)
                    return h;

                return 0;
            }
        }

        public void ResizeRenderRegion()
        {
            int width = 0, height = 0;
            int left = 0, top = 0;

            Size sz = GetWindowSize(_renderHandle);

            double videoAspectRatio = (double)(VideoWidth) / (double)(VideoHeight);
            double panelAspectRatio = (double)sz.Width / (double)sz.Height;

            if (videoAspectRatio >= panelAspectRatio)
            {
                // "wide" video, use width as basis
                width = sz.Width;
                height = (int)(width / videoAspectRatio);
                left = 0;
                top = (sz.Height - height) / 2;
            }
            else
            {
                // "tall" video, use height as basis
                height = sz.Height;
                width = (int)(videoAspectRatio * height);
                left = (sz.Width - width) / 2;
                top = 0;
            }

            int w, h, l, t;

            int hr = videoWindow.GetWindowPosition(out l, out t, out w, out h);
            if (hr == 0)

            if (w != width || h != height || l != left || h != height)
            {
                hr = videoWindow.SetWindowPosition(left, top, width, height);
            }
        }

        private Size GetWindowSize(IntPtr hwnd)
        {
            Size sz = Size.Empty;

            try
            {
                RECT rc;
                if (User32.GetWindowRect(hwnd, out rc))
                {
                    sz.Width = rc.Width;
                    sz.Height = rc.Height;
                }
            }
            catch
            {
            }

            Logger.LogToConsole($"GetWindowSize: hwnd={hwnd} size={sz}");

            return sz;
        }
    }
}
