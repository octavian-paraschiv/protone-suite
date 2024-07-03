using OPMedia.Core.Logging;
using OPMedia.Runtime.DSP;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.Rendering
{
#if BLA_BLA
    public class SampleGrabberProbe:  IDisposable
    {
        protected IMediaControl mediaControl = null;
        protected IMediaPosition mediaPosition = null;

        protected ISampleGrabber sampleGrabber = null;
        protected WaveFormatEx _actualAudioFormat = null;
        protected Thread sampleAnalyzerThread = null;

        ConcurrentQueue<AudioSample> samples = new ConcurrentQueue<AudioSample>();
        ConcurrentQueue<AudioSample> samples2 = new ConcurrentQueue<AudioSample>();

        protected ManualResetEvent sampleAnalyzerMustStop = new ManualResetEvent(false);
        protected ManualResetEvent sampleGrabberConfigured = new ManualResetEvent(false);

        public const int MAX_SPECTROGRAM_BANDS = 64;

        protected object _vuLock = new object();
        protected object _waveformLock = new object();
        protected object _spectrogramLock = new object();
        protected double[] _levelsData = null;
        protected double[] _waveformData = null;
        protected double[] _spectrogramData = null;

        protected int _waveformWindowSize = 512;
        protected int _vuMeterWindowSize = 64;
        protected int _fftWindowSize = 2048;
        protected double _maxLevel = short.MaxValue;
        protected double _maxLogLevel = Math.Log((double)short.MaxValue);


        public SampleGrabberProbe(IMediaControl mediaControl)
        {
            this.mediaControl = mediaControl;
            this.mediaPosition = mediaControl as IMediaPosition;

            Init();
            CompleteInit();
        }


        protected void Init()
        {
            // Get the graph builder
            IGraphBuilder graphBuilder = (mediaControl as IGraphBuilder);
            if (graphBuilder == null)
                return;

            try
            {
                // Build the sample grabber
                sampleGrabber = Activator.CreateInstance(Type.GetTypeFromCLSID(Filters.SampleGrabber, true))
                    as ISampleGrabber;

                if (sampleGrabber == null)
                    return;

                // Add it to the filter graph
                int hr = graphBuilder.AddFilter(sampleGrabber as IBaseFilter, "ProTONE_SampleGrabber_v2");
                DsError.ThrowExceptionForHR(hr);

                IBaseFilter ffdAudioDecoder = null;

                IPin ffdAudioDecoderOutput = null;
                IPin soundDeviceInput = null;
                IPin sampleGrabberInput = null;
                IPin sampleGrabberOutput = null;
                IntPtr pSoundDeviceInput = IntPtr.Zero;

                // When using FFDShow, typically we'll find
                // a ffdshow Audio Decoder connected to the sound device filter
                // 
                // i.e. [ffdshow Audio Decoder] --> [DirectSound Device]
                //
                // Our audio sample grabber supports only PCM sample input and output.
                // Its entire processing is based on this assumption.
                // 
                // Thus need to insert the audio sample grabber between the ffdshow Audio Decoder and the sound device
                // because this is the only place where we can find PCM samples. The sound device only accepts PCM.
                //
                // So we need to turn this graph:
                //
                // .. -->[ffdshow Audio Decoder]-->[DirectSound Device] 
                //
                // into this:
                //
                // .. -->[ffdshow Audio Decoder]-->[Sample grabber]-->[DirectSound Device] 
                //
                // Actions to do to achieve the graph change:
                //
                // 1. Locate the ffdshow Audio Decoder in the graph
                // 2. Find its output pin and the pin that it's connected to
                // 3. Locate the input and output pins of sample grabber
                // 4. Disconnect the ffdshow Audio Decoder and its correspondent (sound device input pin)
                // 5. Connect the ffdshow Audio Decoder to sample grabber input
                // 6. Connect the sample grabber output to sound device input
                // that's all.

                // --------------
                // 1. Locate the ffdshow Audio Decoder in the graph
                hr = graphBuilder.FindFilterByName("ffdshow Audio Decoder", out ffdAudioDecoder);
                DsError.ThrowExceptionForHR(hr);

                // 2. Find its output pin and the pin that it's connected to
                hr = ffdAudioDecoder.FindPin("Out", out ffdAudioDecoderOutput);
                DsError.ThrowExceptionForHR(hr);

                hr = ffdAudioDecoderOutput.ConnectedTo(out pSoundDeviceInput);
                DsError.ThrowExceptionForHR(hr);

                soundDeviceInput = new DSPin(pSoundDeviceInput).Value;

                // 3. Locate the input and output pins of sample grabber
                hr = (sampleGrabber as IBaseFilter).FindPin("In", out sampleGrabberInput);
                DsError.ThrowExceptionForHR(hr);

                hr = (sampleGrabber as IBaseFilter).FindPin("Out", out sampleGrabberOutput);
                DsError.ThrowExceptionForHR(hr);

                // 4. Disconnect the ffdshow Audio Decoder and its correspondent (sound device input pin)
                hr = ffdAudioDecoderOutput.Disconnect();
                DsError.ThrowExceptionForHR(hr);

                hr = soundDeviceInput.Disconnect();
                DsError.ThrowExceptionForHR(hr);

                // 5. Connect the ffdshow Audio Decoder to sample grabber input
                hr = graphBuilder.Connect(ffdAudioDecoderOutput, sampleGrabberInput);
                DsError.ThrowExceptionForHR(hr);

                // 6. Connect the sample grabber output to sound device input
                hr = graphBuilder.Connect(sampleGrabberOutput, soundDeviceInput);
                DsError.ThrowExceptionForHR(hr);

                AMMediaType mtAudio = new AMMediaType();
                mtAudio.majorType = MediaType.Audio;
                mtAudio.subType = MediaSubType.PCM;
                mtAudio.formatPtr = IntPtr.Zero;

                _actualAudioFormat = null;

                sampleGrabber.SetMediaType(mtAudio);
                sampleGrabber.SetBufferSamples(true);
                sampleGrabber.SetOneShot(false);
                sampleGrabber.SetCallback(this, 1);

                sampleAnalyzerMustStop.Reset();
                sampleAnalyzerThread = new Thread(new ThreadStart(SampleAnalyzerLoop));
                sampleAnalyzerThread.Priority = ThreadPriority.Highest;
                sampleAnalyzerThread.Start();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void CompleteInit()
        {
            _actualAudioFormat = null;
            if (sampleGrabber != null)
            {
                AMMediaType mtAudio = new AMMediaType();
                if (HRESULT.SUCCEEDED(sampleGrabber.GetConnectedMediaType(mtAudio)))
                {
                    _actualAudioFormat = (WaveFormatEx)Marshal.PtrToStructure(mtAudio.formatPtr, typeof(WaveFormatEx));

                    const int WAVEFORM_WNDSIZEFACTOR = 128;
                    const int VU_WNDSIZEFACTOR = 4096;
                    const int FFT_WNDSIZEFACTOR = 16;

                    int freq =
                        (_actualAudioFormat == null) ? 44100 : _actualAudioFormat.nSamplesPerSec;

                    try
                    {
                        int k1 = 0, k2 = 0, k3 = 0;

                        while (freq / (1 << k1) > WAVEFORM_WNDSIZEFACTOR)
                            k1++;
                        while (freq / (1 << k2) > FFT_WNDSIZEFACTOR)
                            k2++;
                        while (freq / (1 << k3) > VU_WNDSIZEFACTOR)
                            k3++;

                        _waveformWindowSize = (1 << k1);
                        _fftWindowSize = (1 << k2);
                        _vuMeterWindowSize = (1 << k3);

                        _maxLevel =
                            (_actualAudioFormat != null) ?
                            (1 << (_actualAudioFormat.wBitsPerSample - 1)) - 1 :
                            short.MaxValue;
                    }
                    catch
                    {
                        _vuMeterWindowSize = 64;
                        _waveformWindowSize = 512;
                        _fftWindowSize = 4096;
                        _maxLevel = short.MaxValue;
                    }
                    finally
                    {
                        _maxLogLevel = Math.Log(_maxLevel);
                    }

                    sampleGrabberConfigured.Set();
                    return;
                }
            }
        }

        protected void Release()
        {
            try
            {
                if (sampleAnalyzerMustStop != null)
                    sampleAnalyzerMustStop.Set(); // This will cause the thread to stop

                if (sampleAnalyzerThread != null)
                    sampleAnalyzerThread.Join(200);

                IBaseFilter filter = sampleGrabber as IBaseFilter;

                if (filter != null)
                {
                    IGraphBuilder graphBuilder = (mediaControl as IGraphBuilder);
                    if (graphBuilder != null)
                    {
                        int hr = graphBuilder.RemoveFilter(filter);
                        DsError.ThrowExceptionForHR(hr);
                    }

                    Marshal.ReleaseComObject(filter);
                    sampleGrabber = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            lock (_vuLock)
            {
                _levelsData = null;
            }
            lock (_waveformLock)
            {
                _waveformData = null;
            }
            lock (_spectrogramLock)
            {
                _spectrogramData = null;
            }

            _actualAudioFormat = null;
            sampleGrabberConfigured.Reset();
        }


        public int BufferCB(double sampleTime, IntPtr pBuffer, int bufferLen)
        {
            if (ProTONEConfig.IsSignalAnalisysActive())
            {
                try
                {
                    if (sampleGrabberConfigured.WaitOne(0) && _actualAudioFormat != null)
                    {
                        const int fraction = 128;

                        byte[] allBytes = new byte[bufferLen];
                        Marshal.Copy(pBuffer, allBytes, 0, bufferLen);

                        int pos = 0;

                        double chunkTimeLen = (double)fraction / (double)_actualAudioFormat.nSamplesPerSec;
                        int chunkSize = fraction * _actualAudioFormat.nBlockAlign;
                        int i = 0;

                        do
                        {
                            int size = Math.Min(chunkSize, bufferLen - pos);

                            AudioSample smp = new AudioSample();
                            smp.RawSamples = new byte[size];
                            smp.SampleTime = sampleTime + i * chunkTimeLen;

                            Array.Copy(allBytes, pos, smp.RawSamples, 0, size);

                            samples.Enqueue(smp);

                            pos += size;
                            i++;
                        }
                        while (pos < bufferLen);
                    }
                }
                catch { }
            }

            return 0;
        }

        public int SampleCB(double SampleTime, IntPtr pSample)
        {
            return 0;
        }

        protected void SampleAnalyzerLoop()
        {
            while (sampleAnalyzerMustStop.WaitOne(0) == false)
            {
                if (ProTONEConfig.IsSignalAnalisysActive())
                {
                    AudioSample smp = null;
                    if (samples.TryDequeue(out smp) && smp != null)
                        ExtractSamples(smp);

                    Thread.Yield();
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private ConcurrentQueue<AudioSampleData> _sampleData = new ConcurrentQueue<AudioSampleData>();
        private int _gatheredSamples = 0;

        private double _signalEnergy = 0;

        int _idx = 0;
        double _avgDelay = 0;

        private void CalculateAverageDelay(double momentaryDelay)
        {
            _avgDelay = ((double)_idx * _avgDelay + momentaryDelay) / (double)(_idx + 1);
            _idx++;

            if (_idx % 100 == 1)
            {
                Debug.WriteLine("[SAMPLE] AvgDelay={0:0.000} msec", _avgDelay);
                Debug.Flush();
            }
        }

        private void ExtractSamples(AudioSample smp)
        {
            if (smp == null || _actualAudioFormat == null || mediaPosition == null)
                return;

            double mediaTime = 0;
            mediaPosition.get_CurrentPosition(out mediaTime);

            double delay = smp.SampleTime - mediaTime;
            double absDelay = Math.Abs(delay);

            // Discard samples too far in time from current media time
            if (absDelay > 1)
                return;

            //CalculateAverageDelay(delay * 1000);

            if (delay > 0)
                Thread.Sleep(TimeSpan.FromSeconds(delay));


            FilterState ms = FilterState.Stopped;
            if (mediaControl != null)
            {
                int hr = mediaControl.GetState(0, out ms);
                WorkerException.ThrowForHResult(hr);
            }


            if (smp.RawSamples.Length <= 0 || ms != FilterState.Running || _actualAudioFormat == null)
                return;

            int bytesPerChannel = _actualAudioFormat.wBitsPerSample / 8;
            int totalChannels = _actualAudioFormat.nChannels;
            int totalChannelsInArray = Math.Min(2, totalChannels);

            int i = 0;
            while (i < smp.RawSamples.Length)
            {
                double[] channels = new double[totalChannelsInArray];
                Array.Clear(channels, 0, totalChannelsInArray);

                int j = 0;
                while (j < totalChannelsInArray)
                {
                    int k = 0;
                    while (k < bytesPerChannel)
                    {
                        if (bytesPerChannel <= 2)
                            channels[j] += (short)(smp.RawSamples[i] << (8 * k));
                        else
                            channels[j] += (int)(smp.RawSamples[i] << (8 * k));

                        i++;
                        k++;
                    }

                    j++;
                }

                if (channels.Length >= 2)
                    _sampleData.Enqueue(new AudioSampleData((double)channels[0], (double)channels[1]));
                else
                    _sampleData.Enqueue(new AudioSampleData((double)channels[0], 0));

                _gatheredSamples++;
                if (_gatheredSamples % _waveformWindowSize == 0)
                {
                    if (ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.VUMeter) ||
                        ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Waveform) ||
                        ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.WCFInterface))
                    {
                        AnalyzeWaveform(_sampleData.Skip(_sampleData.Count - _waveformWindowSize).Take(_waveformWindowSize).ToArray(),
                            smp.SampleTime);
                    }
                }

                Thread.Yield();
            }

            AudioSampleData lostSample = null;
            while (_sampleData.Count > _fftWindowSize)
                _sampleData.TryDequeue(out lostSample);

            if (ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Spectrogram) ||
                ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.WCFInterface))
            {
                AnalyzeFFT(_sampleData.ToArray());
            }

            Thread.Yield();
        }

        private void AnalyzeWaveform(AudioSampleData[] data, double sampleTime)
        {
            double lVal = 0, rVal = 0;

            double[] dataWaveform = new double[data.Length];

            int i = 0;
            for (i = 0; i < data.Length; i++)
            {
                double absL = Math.Abs(data[i].LVOL);
                double absR = Math.Abs(data[i].RVOL);

                if (lVal < absL)
                    lVal = absL;
                if (rVal < absR)
                    rVal = absR;

                dataWaveform[i] = 0.5 * data[i].LVOL + 0.5 * data[i].RVOL;

                if (i % 32 == 0)
                {
                    lock (_vuLock)
                    {
                        _levelsData = new double[] { lVal / _maxLevel, rVal / _maxLevel };
                    }

                    lVal = rVal = 0;
                }
            }

            lock (_waveformLock)
            {
                _waveformData = dataWaveform;
            }
        }

        private void AnalyzeFFT(AudioSampleData[] data)
        {
            if (data.Length == _fftWindowSize)
            {
                double[] dataIn = new double[data.Length];
                double[] dataOut = new double[data.Length];

                for (int i = 0; i < data.Length; i++)
                    dataIn[i] = data[i].LVOL;

                FFT.Forward(dataIn, dataOut);

                    var linearData = dataOut
                        .Skip(1 /* First band represents the 'total energy' of the signal */ )
                        .Take(_fftWindowSize / 2 /* The spectrum is 'mirrored' horizontally around the sample rate / 2 according to Nyquist theorem */ )
                        .ToArray();

                lock (_spectrogramLock)
                {
                    _spectrogramData = FFTHelper.TranslateFFTIntoBands(linearData, _actualAudioFormat.nSamplesPerSec / 2, MAX_SPECTROGRAM_BANDS);
                }
            }
        }

        public double[] GetSpectrogram()
        {
            lock (_spectrogramLock)
            {
                return _spectrogramData;
            }
        }

        public double[] GetWaveform()
        {
            lock (_waveformLock)
            {
                return _waveformData;
            }
        }

        public double[] GetLevels()
        {
            lock (_vuLock)
            {
                return _levelsData;
            }
        }

        public void Dispose()
        {
            Release();
        }
    }
#endif
}
