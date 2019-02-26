using System;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;

namespace OPMedia.AudioWorker
{
    public class AudioFilePlayer : IWorkerPlayer
    {
        CommandProcessor _proc = null;

        protected IMediaControl mediaControl = null;
        protected IBasicAudio basicAudio = null;
        protected IMediaPosition mediaPosition = null;

        protected double durationScaleFactor = 1;
        protected MediaFileInfo renderMediaInfo = MediaFileInfo.Empty;

        protected SampleGrabberProbe _probe = null;


        public AudioFilePlayer()
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

        public void Play(string url, string userId, int delayStart)
        {
            if (url == null || url.Length <= 0)
                return;

            InitMedia(url);

            int hr = mediaPosition.put_Rate(1);
            WorkerException.ThrowForHResult(hr);

            // Run the graph to play the media file
            hr = mediaControl.Run();
            WorkerException.ThrowForHResult(hr);

            // HACK: call GetLength once here to ensure that durationScaleFactor is buuilt up
            GetLength();
        }

        private void InitMedia(string url)
        {
            mediaControl = BuildMediaControl();
            mediaPosition = mediaControl as IMediaPosition;
            basicAudio = mediaControl as IBasicAudio;

            int hr = mediaControl.RenderFile(url);


            try
            {
                _probe = new SampleGrabberProbe(mediaControl);
            }
            catch
            {
                _probe = null;
            }

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
            SetMediaPosition(pos);

            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                WorkerException.ThrowForHResult(hr);
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
            if (_probe != null)
            {
                _probe.Dispose();
                _probe = null;
            }

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
            if (_probe != null)
                return SupportedMeteringData.Levels | SupportedMeteringData.Spectrogram | SupportedMeteringData.Waveform;

            return SupportedMeteringData.OutputLevels;
        }

        public double[] GetLevels()
        {
            if (_probe != null)
                return _probe.GetLevels();

            return null;
        }

        public double[] GetWaveform()
        {
            if (_probe != null)
                return _probe.GetWaveform();

            return null;
        }

        public double[] GetSpectrogram()
        {
            if (_probe != null)
                return _probe.GetSpectrogram();

            return null;
        }
    }
}
