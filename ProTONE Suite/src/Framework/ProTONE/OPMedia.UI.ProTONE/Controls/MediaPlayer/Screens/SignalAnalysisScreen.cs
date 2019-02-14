using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Core.Logging;
using System.Diagnostics;
using OPMedia.Runtime.DSP;
using OPMedia.UI.Themes;
using OPMedia.Core;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System.Threading;
using OPMedia.Core.Configuration;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;
using OPMedia.Core.GlobalEvents;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.UI.ProTONE.Translations;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using System.Threading.Tasks;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens
{
    public partial class SignalAnalysisScreen : OPMBaseControl
    {
        public const int BandCount = DsRendererBase.MAX_SPECTROGRAM_BANDS;
        private double[] _bands = new double[BandCount];

        private object _updateLock = new object();
        private ManualResetEvent _evt = new ManualResetEvent(false);

        #region Constructor

        public SignalAnalysisScreen()
        {
            InitializeComponent();
            OnUpdateMediaScreens();

            this.HandleCreated += SignalAnalysisScreen_HandleCreated;
            this.HandleDestroyed += SignalAnalysisScreen_HandleDestroyed;

            if (!DesignMode)
            {
                MediaRenderer.DefaultInstance.FilterStateChanged += new FilterStateChangedHandler(OnMediaStateChanged);
                MediaRenderer.DefaultInstance.MediaRendererHeartbeat += new MediaRendererEventHandler(OnMediaRendererHeartbeat);
                MediaRenderer.DefaultInstance.MediaRenderingException += new MediaRenderingExceptionHandler(OnMediaRenderingException);
            }
        }

        private void SignalAnalysisScreen_HandleCreated(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (_evt.WaitOne(5) == false)
                {
                    Invoke(new MethodInvoker(OnUpdate));
                }
            });

        }

        private void SignalAnalysisScreen_HandleDestroyed(object sender, EventArgs e)
        {
            _evt.Set();
            MediaRenderer.DefaultInstance.FilterStateChanged -= new FilterStateChangedHandler(OnMediaStateChanged);
            MediaRenderer.DefaultInstance.MediaRendererHeartbeat -= new MediaRendererEventHandler(OnMediaRendererHeartbeat);
            MediaRenderer.DefaultInstance.MediaRenderingException -= new MediaRenderingExceptionHandler(OnMediaRenderingException);
        }

        private void OnMediaRendererHeartbeat()
        {
            OnUpdateMediaScreens();
        }

        private void OnMediaRenderingException(RenderingExceptionEventArgs args)
        {
            OnUpdateMediaScreens();
        }

        private void OnMediaStateChanged(FilterState oldState, string oldMedia, FilterState newState, string newMedia)
        {
            OnUpdateMediaScreens();
        }

        [EventSink(EventNames.ThemeUpdated)]
        [EventSink(LocalEventNames.UpdateMediaScreens)]
        public void OnUpdateMediaScreens()
        {
            bool showVU = ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.VUMeter);
            bool showWaveform = ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Waveform);
            bool showSpectrogram = ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Spectrogram);

            bool sampleGrabberSupported = false;// MediaRenderer.DefaultInstance.SampleGrabberSupported;

            showWaveform &= sampleGrabberSupported;
            showSpectrogram &= sampleGrabberSupported;

            pnlVuMeter.Visible = showVU;

            opmTableLayoutPanel1.RowStyles[0] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, showVU ? 70F : 0F);

            pnlWaveform.Visible = showWaveform;

            pnlSpectrogram.Visible = showSpectrogram;

            if (showSpectrogram && showWaveform)
            {
                opmTableLayoutPanel1.RowStyles[1] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F);
                opmTableLayoutPanel1.RowStyles[2] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F);
            }
            else if (showWaveform)
            {
                opmTableLayoutPanel1.RowStyles[1] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F);
                opmTableLayoutPanel1.RowStyles[2] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F);
            }
            else if (showSpectrogram)
            {
                opmTableLayoutPanel1.RowStyles[1] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F);
                opmTableLayoutPanel1.RowStyles[2] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F);
            }
            else
            {
                opmTableLayoutPanel1.RowStyles[1] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F);
                opmTableLayoutPanel1.RowStyles[2] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F);
            }

            UpdateLabels();

            lblUnsupportedFunctionsHint.Visible = !sampleGrabberSupported;
        }

        [EventSink(EventNames.PerformTranslation)]
        private void UpdateLabels()
        {
            lblSignalLevel.Text = Translator.Translate("TXT_SIGNALLEVEL");
            lblSignalWaveform.Text = Translator.Translate("TXT_SIGNALWAVEFORM");
            lblSignalSpectrum.Text = Translator.Translate("TXT_SIGNALSPECTRUM");
        }

        double[] _prevWaveform = null;

        void OnUpdate()
        {
            try
            {
                if (ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.VUMeter))
                {
                    AudioSampleData vuData = MediaRenderer.DefaultInstance.VuMeterData;
                    if (vuData != null)
                    {
                        vuLeft.Value = 0.5 * (vuLeft.Value + vuLeft.Maximum * vuData.LVOL);
                        vuRight.Value = 0.5 * (vuRight.Value + vuRight.Maximum * vuData.RVOL);
                        //vuLeft.Value = vuLeft.Maximum * vuData.LVOL;
                        //vuRight.Value = vuRight.Maximum * vuData.RVOL;
                    }
                    else
                    {
                        vuLeft.Value = 0;
                        vuRight.Value = 0;
                    }
                }

                #region disabled code
#if DUMMY
                if (ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Waveform))
                {
                    gpWaveform.Reset(false);

                    double[][] waveformData = MediaRenderer.DefaultInstance.WaveformData;
                    if (waveformData != null && waveformData[0].Length > 0)
                    {
                        if (_prevWaveform == null)
                            _prevWaveform = new double[waveformData[0].Length];

                        for (int k = 0; k < _prevWaveform.Length; k++)
                            _prevWaveform[k] = 0.5 * (_prevWaveform[k] + waveformData[0][k]);

                        gpWaveform.MinVal = -1 * MediaRenderer.DefaultInstance.MaxLevel;
                        gpWaveform.MaxVal = MediaRenderer.DefaultInstance.MaxLevel;
                        gpWaveform.AddDataRange(_prevWaveform, ThemeManager.GradientGaugeColor1);
                    }
                    else
                    {
                        gpWaveform.Reset(true);
                    }
                }

                if (ProTONEConfig.SignalAnalisysFunctionActive(SignalAnalisysFunction.Spectrogram))
                {
                    double maxFftLevel = SpectrogramTransferFunction(MediaRenderer.DefaultInstance.MaxFFTLevel);

                    spSpectrogram.Reset(false);
                    spSpectrogram.MinVal = maxFftLevel / 2; // Min level = -6 dBM
                    spSpectrogram.MaxVal = maxFftLevel; // Max level = 0 dBM

                    double[] spectrogramData = MediaRenderer.DefaultInstance.SpectrogramData;
                    if (spectrogramData != null && spectrogramData.Length > 0)
                    {

                        double[] spectrogramData2 = new double[spectrogramData.Length];
                        Array.Clear(spectrogramData2, 0, spectrogramData.Length);

                        double[] bands = new double[BandCount];
                        Array.Clear(bands, 0, BandCount);

                        int jBand = 0;

                        int div = spectrogramData.Length / (BandCount);

                        try
                        {
                            int maxSize = (int)Math.Min(BandCount, spectrogramData.Length);
                            for (int i = 0; i < maxSize; i++)
                            {
                                bands[i] = Math.Max(0, Math.Min(maxFftLevel, SpectrogramTransferFunction(spectrogramData[i])));
                                _bands[i] = 0.5 * (_bands[i] + bands[i]);
                            }

                            spSpectrogram.AddDataRange(_bands, Color.Transparent);
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                            spSpectrogram.Reset(true);
                            Array.Clear(_bands, 0, _bands.Length);
                        }
                    }
                    else
                    {
                        spSpectrogram.Reset(true);
                        Array.Clear(_bands, 0, _bands.Length);
                    }
                }
#endif
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
            }
        }

        private double SpectrogramTransferFunction(double d)
        {
            return Math.Log(d);
            //return d;
        }

        #endregion

        

    }
}
