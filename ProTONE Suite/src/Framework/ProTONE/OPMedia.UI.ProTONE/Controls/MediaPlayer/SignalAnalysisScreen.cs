using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.AudioMetering;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens
{
    public partial class SignalAnalysisScreen : OPMBaseControl
    {
        public const int BandCount = WasapiMeter.MAX_SPECTROGRAM_BANDS;
        private double[] _bands = new double[BandCount];

        #region Constructor

        public SignalAnalysisScreen()
        {
            InitializeComponent();

            HandleCreated += SignalAnalysisScreen_HandleCreated;
            HandleDestroyed += SignalAnalysisScreen_HandleDestroyed;

            gpWaveform.IsHistogram = true;
            gpWaveform.IsCentered = true;

            if (!DesignMode)
            {
                RenderingEngine.DefaultInstance.FilterStateChanged += OnMediaStateChanged;
                RenderingEngine.DefaultInstance.MediaRendererHeartbeat += OnRenderingEngineHeartbeat;
                RenderingEngine.DefaultInstance.MediaRenderingException += OnMediaRenderingException;

                WasapiMeter.Instance.VuMeterData += OnVuMeterData;
                WasapiMeter.Instance.WaveformData += OnWaveformData;
                WasapiMeter.Instance.SpectrogramData += OnSpectrogramData;
            }
        }

        private void SignalAnalysisScreen_HandleCreated(object sender, EventArgs e)
        {
            OnUpdateMediaScreens();
            OnVuMeterData(null);
            OnWaveformData(null);
            OnSpectrogramData(null);
        }

        private void SignalAnalysisScreen_HandleDestroyed(object sender, EventArgs e)
        {
            WasapiMeter.Instance.VuMeterData -= OnVuMeterData;
            WasapiMeter.Instance.WaveformData -= OnWaveformData;
            WasapiMeter.Instance.SpectrogramData -= OnSpectrogramData;

            RenderingEngine.DefaultInstance.FilterStateChanged -= OnMediaStateChanged;
            RenderingEngine.DefaultInstance.MediaRendererHeartbeat -= OnRenderingEngineHeartbeat;
            RenderingEngine.DefaultInstance.MediaRenderingException -= OnMediaRenderingException;
        }

        private void OnRenderingEngineHeartbeat()
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

            pnlVuMeter.Visible = showVU;

            opmTableLayoutPanel1.RowStyles[0] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, showVU ? 50F : 0F);

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
        }

        List<double> _prevWaveform = new List<double>();

        private void OnVuMeterData(AudioSampleData vuData)
        {
            try
            {
                if (vuData != null)
                {
                    vuLeft.Value = 0.5 * (vuLeft.Value + vuLeft.Maximum * vuData.LVOL);
                    vuRight.Value = 0.5 * (vuRight.Value + vuRight.Maximum * vuData.RVOL);
                }
                else
                {
                    vuLeft.Value = 0;
                    vuRight.Value = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void OnWaveformData(double[][] waveformData)
        {
            try
            {
                gpWaveform.Reset(false);

                if (waveformData != null && waveformData.Length > 0)
                {
                    for (int k = 0; k < waveformData.Length; k++)
                    {
                        double l = waveformData[0][k];
                        double r = waveformData[1][k];
                        double val = Math.Sqrt(l * l + r * r);
                        _prevWaveform.Add(val);
                    }

                    while (_prevWaveform.Count > 500)
                        _prevWaveform.RemoveAt(0);

                    gpWaveform.MinVal = 0;// -1 * WasapiMeter.Instance.MaxLevel;
                    gpWaveform.MaxVal = Math.Sqrt(2) * WasapiMeter.Instance.MaxLevel;

                    gpWaveform.AddDataRange(_prevWaveform.ToArray(), ThemeManager.GradientGaugeColor1);
                }
                else
                {
                    _prevWaveform.Clear();
                    gpWaveform.Reset(true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void OnSpectrogramData(double[] spectrogramData)
        {
            try
            {
                if (spectrogramData != null && spectrogramData.Length > 0)
                {
                    double maxFftLevel = Math.Log(WasapiMeter.Instance.MaxFFTLevel);

                    spSpectrogram.Reset(false);
                    spSpectrogram.MinVal = maxFftLevel / 2; // Min level = -6 dBM
                    spSpectrogram.MaxVal = maxFftLevel; // Max level = 0 dBM

                    double[] spectrogramData2 = new double[spectrogramData.Length];
                    Array.Clear(spectrogramData2, 0, spectrogramData.Length);

                    double[] bands = new double[BandCount];
                    Array.Clear(bands, 0, BandCount);

                    int div = spectrogramData.Length / (BandCount);

                    try
                    {
                        int maxSize = (int)Math.Min(BandCount, spectrogramData.Length);
                        for (int i = 0; i < maxSize; i++)
                        {
                            bands[i] = Math.Max(0, Math.Min(maxFftLevel, Math.Log(spectrogramData[i])));
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
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion
    }
}
