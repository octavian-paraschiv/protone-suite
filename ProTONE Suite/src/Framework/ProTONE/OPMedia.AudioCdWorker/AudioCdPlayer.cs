using System;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;

namespace OPMedia.AudioCdWorker
{
    public class AudioCdPlayer : IWorkerPlayer
    {
        CommandProcessor _proc = null;

        protected IMediaControl mediaControl = null;
        protected IBasicAudio basicAudio = null;
        protected IMediaPosition mediaPosition = null;

        protected double durationScaleFactor = 1;
        protected MediaFileInfo renderMediaInfo = MediaFileInfo.Empty;

        DSBaseSourceFilter _source = null;

        public AudioCdPlayer()
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

            InitMedia(url);

            int hr = mediaPosition.put_Rate(1);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            // Run the graph to play the media file
            hr = mediaControl.Run();
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            // HACK: call GetLength once here to ensure that durationScaleFactor is buuilt up
            GetLength();

            SetVolume(0);
        }

        private void InitMedia(string url)
        {
            GC.Collect();

            mediaControl = BuildMediaControl();

            // Create Filter
            _source = new DSBaseSourceFilter(new AudioCdSourceFilter());

            // load the file
            _source.FileName = url;

            // Add to the filter Graph
            _source.FilterGraph = (mediaControl) as IGraphBuilder;

            if (_source.OutputPin == null)
                WorkerException.Throw(WorkerError.MediaReadError, -1);

            // Render the output pin
            int hr = (int)_source.OutputPin.Render();
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            mediaPosition = mediaControl as IMediaPosition;
            basicAudio = mediaControl as IBasicAudio;

            hr = basicAudio.put_Volume((int)VolumeRange.Minimum);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
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

        public void ResizeRenderRegion()
        {
        }
    }
}
