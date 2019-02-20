using System;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;

namespace OPMedia.ShoutcastWorker
{
    public class ShoutcastPlayer : IWorkerPlayer
    {
        CommandProcessor _proc = null;

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
                WorkerException.ThrowForHResult(hr);
            }

            return (int)val;
        }

        public int GetVolume()
        {
            int val = -1;

            int hr = basicAudio.get_Volume(out val);
            WorkerException.ThrowForHResult(hr);

            return val;

        }

        public void Pause()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Pause();
                WorkerException.ThrowForHResult(hr);
            }
        }

        public void Play(string url, int delayStart)
        {
            if (url == null || url.Length <= 0)
                return;

            InitMedia(url);

            int hr = mediaPosition.put_Rate(1);
            WorkerException.ThrowForHResult(hr);

            // Run the graph to play the media file
            hr = mediaControl.Run();
            WorkerException.ThrowForHResult(hr);
        }

        private void InitMedia(string url)
        {
            mediaControl = BuildMediaControl();

            // Create Filter
            _source = new DSBaseSourceFilter(new ShoutcastStreamSourceFilter());

            // load the file
            _source.FileName = url;

            // Add to the filter Graph
            _source.FilterGraph = (mediaControl) as IGraphBuilder;

            if (_source.OutputPin == null)
                WorkerException.ThrowForErrorCode(WorkerError.MediaReadError, 
                    $"Unable to stream media from URL: {url}");

            // Render the output pin
            int hr = (int)_source.OutputPin.Render();
            WorkerException.ThrowForHResult(hr);

            mediaPosition = mediaControl as IMediaPosition;
            basicAudio = mediaControl as IBasicAudio;

            hr = basicAudio.put_Volume((int)VolumeRange.Minimum);
            WorkerException.ThrowForHResult(hr);
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
                WorkerException.ThrowForHResult(hr);
            }
        }

        public void SetMediaPosition(int pos)
        {
            // Shoutcast stream is NOT seekable ...
        }

        public void SetVolume(int vol)
        {
            Logger.LogTrace($"SetVolume to {vol}");

            if (vol < -10000)
                vol = -10000;
            else if (vol > 0)
                vol = 0;

            int hr = basicAudio.put_Volume(vol);
            WorkerException.ThrowForHResult(hr);
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
                WorkerException.ThrowForHResult(hr);
            }

            return fs;
        }

        public SupportedMeteringData GetSupportedMeteringData()
        {
            return SupportedMeteringData.Levels;
        }

        public double[] GetLevels()
        {
            return null;
        }

        public double[] GetWaveform()
        {
            return null;
        }

        public double[] GetSpectrogram()
        {
            return null;
        }
    }
}
