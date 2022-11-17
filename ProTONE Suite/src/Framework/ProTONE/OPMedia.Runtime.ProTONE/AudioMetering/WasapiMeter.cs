using NAudio.Wave;
using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Runtime.DSP;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.AudioMetering
{
    [DataContract]
    public class AudioSampleData
    {
        [DataMember]
        public double LVOL { get; private set; }

        [DataMember]
        public double RVOL { get; private set; }

        public AudioSampleData(double lVol, double rVol)
        {
            LVOL = lVol;
            RVOL = rVol;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class AudioSample
    {
        public double SampleTime;
        public byte[] RawSamples;
    }

    public delegate void VuMeterDataHandler(AudioSampleData data);
    public delegate void WaveformDataHandler(double[][] data);
    public delegate void SpectrogramDataHandler(double[] data);

    public class WasapiMeter
    {
        private static WasapiMeter _instance;
        private static object _instanceLock = new object();

        public const int FftAmplification = ushort.MaxValue;

        public const int MAX_SPECTROGRAM_BANDS = 64;
        private int _waveformWindowSize = 512;
        
        
        private int _fftWindowSize = 2048;

        private ConcurrentQueue<AudioSampleData> _sampleData = new ConcurrentQueue<AudioSampleData>();
        private int _gatheredSamples = 0;

        private IWaveIn _waveIn;

        private ConcurrentQueue<AudioSample> samples = new ConcurrentQueue<AudioSample>();

        private ManualResetEvent notifySoundDeviceChanged = new ManualResetEvent(false);
        private ManualResetEvent sampleAnalyzerMustStop = new ManualResetEvent(false);

        private Thread sampleAnalyzerThread = null;

        public event VuMeterDataHandler VuMeterData;
        public event WaveformDataHandler WaveformData;
        public event SpectrogramDataHandler SpectrogramData;

        public static WasapiMeter Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        lock (_instanceLock)
                        {
                            _instance = new WasapiMeter();
                        }
                    }

                    return _instance;
                }
            }
        }

        public double MaxLevel
        {
            get
            {
                return 1f;
            }
        }

        public double FFTWindowSize
        {
            get
            {
                return _fftWindowSize;
            }
        }
        public double MaxFFTLevel
        {
            get
            {
                return FftAmplification * FFTWindowSize;
            }
        }

        public bool Start()
        {
            try
            {
                Logger.LogToConsole("WASAPI meter starting");

                _waveIn = new WasapiLoopbackCapture();
                _waveIn.DataAvailable += _waveIn_DataAvailable;
                _waveIn.RecordingStopped += _waveIn_RecordingStopped;
                _waveIn.StartRecording();

                int freq = _waveIn.WaveFormat.SampleRate;
                const int WAVEFORM_WNDSIZEFACTOR = 128;
                const int VU_WNDSIZEFACTOR = 4096;
                const int FFT_WNDSIZEFACTOR = 16;

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
                }
                catch
                {
                    _waveformWindowSize = 512;
                    _fftWindowSize = 4096;
                }

                sampleAnalyzerMustStop.Reset();
                sampleAnalyzerThread = new Thread(new ThreadStart(SampleAnalyzerLoop));
                sampleAnalyzerThread.Priority = ThreadPriority.Normal;
                sampleAnalyzerThread.Start();

                Logger.LogToConsole("WASAPI meter started");

                if (notifySoundDeviceChanged.WaitOne(0))
                {
                    notifySoundDeviceChanged.Reset();
                    PersistenceProxy.SendIpcEvent(EventNames.SoundDeviceChanged);
                }

                return true;
            }
            catch { }

            Logger.LogToConsole("WASAPI meter failed to start");
            return false;
        }

        private void _waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Logger.LogToConsole("WASAPI capture stopped ... ");

            if (sampleAnalyzerMustStop.WaitOne(0) == false)
            {
                Logger.LogToConsole("... this was not requested by us => Restarting WASAPI meter");

                // Capture stopped, but we did not request it to stop
                // Most likely something changed related to the sound device
                // Need to request stop ourselves, then start it again.
                if (Stop())
                {
                    notifySoundDeviceChanged.Set();
                    Start();
                }
            }
        }

        public bool Stop()
        {
            try
            {
                Logger.LogToConsole("Requesting WASAPI meter to stop");

                sampleAnalyzerMustStop?.Set(); // This will cause the thread to stop
                sampleAnalyzerThread?.Join(200);
                sampleAnalyzerThread = null;

                _waveIn?.StopRecording();
                _waveIn = null;

                Logger.LogToConsole("WASAPI meter stopped");

                return true;
            }
            catch
            {
            }

            Logger.LogToConsole("WASAPI meter failed to stop");
            return false;
        }

        private void _waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (ProTONEConfig.IsSignalAnalisysActive())
            {
                try
                {
                    if (e?.BytesRecorded > 0)
                    {
                        AudioSample smp = new AudioSample();
                        smp.RawSamples = new byte[e.BytesRecorded];
                        Array.Copy(e.Buffer, 0, smp.RawSamples, 0, e.BytesRecorded);
                        samples.Enqueue(smp);
                    }
                }
                catch { }
            }
        }

        protected void SampleAnalyzerLoop()
        {
            while (sampleAnalyzerMustStop.WaitOne(0) == false)
            {
                if (ProTONEConfig.IsSignalAnalisysActive())
                {
                    if (samples.TryDequeue(out AudioSample smp) && smp != null)
                    {
                        const int fraction = 128;
                        int pos = 0;
                        int chunkSize = fraction * _waveIn.WaveFormat.BlockAlign;
                        int i = 0;

                        do
                        {
                            int size = Math.Min(chunkSize, smp.RawSamples.Length - pos);
                            byte[] data = new byte[size];
                            Array.Copy(smp.RawSamples, pos, data, 0, size);
                            AnalyzeSamples(data);
                            pos += size;
                            i++;
                        }
                        while (pos < smp.RawSamples.Length);
                    }
                }

                Thread.Sleep(1);
            }
        }

        private void AnalyzeSamples(byte[] data)
        {
            if (data?.Length <= 0 || _waveIn == null || _waveIn.WaveFormat == null)
                return;

            int bytesPerChannel = _waveIn.WaveFormat.BitsPerSample / 8;
            int totalChannels = _waveIn.WaveFormat.Channels;
            var encoding = _waveIn.WaveFormat.Encoding;

            int i = 0;
            while (i < data.Length && !sampleAnalyzerMustStop.WaitOne(0))
            {
                double[] channels = new double[totalChannels];
                Array.Clear(channels, 0, totalChannels);

                int j = 0;
                while (j < totalChannels && !sampleAnalyzerMustStop.WaitOne(0))
                {
                    switch (bytesPerChannel)
                    {
                        case 2:
                            channels[j] = BitConverter.ToInt16(data, i) / 32768f;
                            break;

                        case 3:
                            channels[j] = (((sbyte)data[i + 2] << 16) | (data[i + 1] << 8) | data[i]) / 8388608f;
                            break;

                        case 4:
                            if (encoding == WaveFormatEncoding.IeeeFloat)
                                channels[j] = BitConverter.ToSingle(data, i);
                            else
                                channels[j] = BitConverter.ToInt32(data, i) / (int.MaxValue + 1f);
                            break;
                    }

                    i += bytesPerChannel;
                    j++;
                }

                AudioSampleData asd;

                if (channels.Length >= 2)
                    asd = new AudioSampleData((double)channels[0], (double)channels[1]);
                else
                    asd = new AudioSampleData((double)channels[0], 0);

                _sampleData.Enqueue(asd);

                _gatheredSamples++;
                if (_gatheredSamples % _waveformWindowSize == 0)
                    AnalyzeWaveform(_sampleData.Skip(_sampleData.Count - _waveformWindowSize).Take(_waveformWindowSize).ToArray());
            }

            while (_sampleData.Count > _fftWindowSize)
                _sampleData.TryDequeue(out AudioSampleData lostSample);

            AnalyzeFFT(_sampleData.ToArray());
        }

        private void AnalyzeWaveform(AudioSampleData[] data)
        {
            var shouldRun = ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.VUMeter) ||
                ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Waveform) ||
                ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.ExportInterface);

            if (!shouldRun)
                return;

            double lVal = 0, rVal = 0;

            double[][] dataWaveform = new double[2][];
            dataWaveform[0] = new double[data.Length];
            dataWaveform[1] = new double[data.Length];

            int i = 0;
            for (i = 0; i < data.Length; i++)
            {
                double absL = Math.Abs(data[i].LVOL);
                double absR = Math.Abs(data[i].RVOL);

                if (lVal < absL)
                    lVal = absL;
                if (rVal < absR)
                    rVal = absR;

                dataWaveform[0][i] = data[i].LVOL;
                dataWaveform[1][i] = data[i].RVOL;

                if (i % 32 == 0)
                {
                    var vuData = new AudioSampleData(lVal, rVal);
                    VuMeterData?.Invoke(vuData);
                    lVal = rVal = 0;
                }
            }

            WaveformData?.Invoke(dataWaveform);
        }

        private void AnalyzeFFT(AudioSampleData[] data)
        {
            var shouldRun = ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Spectrogram) ||
               ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.ExportInterface);

            if (!shouldRun)
                return;

            if (data.Length == _fftWindowSize)
            {
                double[] dataIn = new double[data.Length];
                double[] dataOut = new double[data.Length];

                for (int i = 0; i < data.Length; i++)
                    dataIn[i] = FftAmplification * data[i].LVOL;

                FFT.Forward(dataIn, dataOut);

                var linearData = dataOut
                    .Skip(1 /* First band represents the 'total energy' of the signal */ )
                    .Take(_fftWindowSize / 2 /* The spectrum is 'mirrored' horizontally around the sample rate / 2 according to Nyquist theorem */ )
                    .ToArray();

                var spectrogramData = FFTHelper.TranslateFFTIntoBands(linearData, (double)_waveIn?.WaveFormat.SampleRate / 2, MAX_SPECTROGRAM_BANDS);
                SpectrogramData?.Invoke(spectrogramData);
            }
        }
    }
}
