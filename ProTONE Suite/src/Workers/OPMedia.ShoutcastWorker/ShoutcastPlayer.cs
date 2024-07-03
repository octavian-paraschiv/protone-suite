using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;

namespace OPMedia.ShoutcastWorker
{
    public class ShoutcastPlayer : WorkerCommandHandler, IWorkerPlayer
    {
        DSBaseSourceFilter _source = null;

        protected IMediaControl mediaControl = null;
        protected IBasicAudio basicAudio = null;
        protected IMediaPosition mediaPosition = null;

        public ShoutcastPlayer()
        {
        }

        public int GetLength()
        {
            return -1;
        }

        public int GetMediaPosition()
        {
            double val = 0;

            if (mediaPosition != null)
            {
                int hr = mediaPosition.get_CurrentPosition(out val);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }

            return (int)val;
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

            SetVolume(0);
        }

        private void InitMedia(string url)
        {
            mediaControl = BuildMediaControl();

            var filter = new ShoutcastStreamSourceFilter();

            // Create Filter
            _source = new DSBaseSourceFilter(filter);

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

            var ss = filter.GetStream();
            filter.GetStream().StreamPropertyChanged += ShoutcastPlayer_StreamPropertyChanged;

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("TXT_FREQUENCY", ss.SampleRate.ToString());
            data.Add("TXT_BITRATE", ss.Bitrate.ToString());
            data.Add("TXT_TITLE", ss.StreamTitle);
            data.Add("Content-Type", ss.ContentType);
            ShoutcastPlayer_StreamPropertyChanged(data);
        }

        private void ShoutcastPlayer_StreamPropertyChanged(Dictionary<string, string> props)
        {
            if (props?.Count > 0)
            {
                WorkerEvent evt = new WorkerEvent(WorkerEventType.StreamPropertyChanged);
                foreach (var kvp in props)
                {
                    if (kvp.Key?.Length > 0 && kvp.Value?.Length > 0)
                    {
                        evt.AddParameter(kvp.Key);
                        evt.AddParameter(kvp.Value);
                        Logger.LogTrace($"{kvp.Key}={kvp.Value}");
                    }
                }
                Worker.Instance.WriteEvent(evt);
            }
        }

        private IMediaControl BuildMediaControl()
        {
            Type mediaControlType = Type.GetTypeFromCLSID(Filters.FilterGraph, true);
            return Activator.CreateInstance(mediaControlType) as IMediaControl;
        }

        public void Resume(int pos)
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
        }

        public void SetMediaPosition(int pos)
        {
            // Shoutcast stream is NOT seekable ...
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
    }
}
