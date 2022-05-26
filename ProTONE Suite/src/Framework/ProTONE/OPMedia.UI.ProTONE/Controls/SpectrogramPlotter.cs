using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using OPMedia.Runtime.DSP;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.AudioMetering;

namespace OPMedia.UI.ProTONE.Controls
{
    public partial class SpectrogramPlotter : GraphPlotter
    {
        Brush _b;
        int _w = 1;
        double _f = 0.85f;

        static readonly double[] DecadeLinesRelativePositions = { 0, 9, 15, 22, 30, 36, 43, 51, 57, 64 };
        static readonly string[] DecadeLinesText = { "20", "50", "100", "200", "500", "1K", "2K", "5K", "10K", "a" };

        public int BandCount { get; set; }

        static SpectrogramPlotter()
        {
            for (int i = 0; i < DecadeLinesRelativePositions.Length; i++)
                DecadeLinesRelativePositions[i] /= WasapiMeter.MAX_SPECTROGRAM_BANDS;
        }

        public SpectrogramPlotter() : base()
        {
            RecreateBrush();

            this.Resize += new EventHandler(SpectrogramPlotter_Resize);
            this.BandCount = WasapiMeter.MAX_SPECTROGRAM_BANDS;
        }

        void SpectrogramPlotter_Resize(object sender, EventArgs e)
        {
            RecreateBrush();
        }

        protected override void OnThemeUpdatedInternal()
        {
            RecreateBrush();
        }

        private void RecreateBrush()
        {
            if (_b != null)
                _b.Dispose();

            _w = (int)(_f * (float)this.Width / (float)this.BandCount);
            if (_w < 1)
                _w = 1;

            int h = Math.Max(1, this.Height - 15);

            _b = BrushHelper.GenerateVuMeterBrush(_w, h, false);
        }

        protected override void DrawCustomHistoBar(Graphics g, Rectangle rc, int w, Point pt)
        {
            Rectangle rcBar = new Rectangle(pt.X - (int)(0.5f * _w), pt.Y, _w, rc.Bottom - pt.Y);
            g.FillRectangle(_b, rcBar);
        }

        protected override void DrawDecadicLines(Graphics g, Rectangle rc)
        {
            using (Pen p = new Pen(ThemeManager.ForeColor))
            using (Brush b = new SolidBrush(ThemeManager.ForeColor))
            {
                for (int i = 0; i < DecadeLinesRelativePositions.Length; i++)
                {
                    double decadeLinePos = DecadeLinesRelativePositions[i];
                    string decadeLineText = DecadeLinesText[i];
                    if (i == DecadeLinesRelativePositions.Length - 1)
                    {
                        try
                        {
                            int maxFq = 24000;// FFTHelper.GetMaxDisplayableFreq(MediaRenderer.DefaultInstance.ActualAudioFormat.nSamplesPerSec / 2,
                                //DsRendererBase.MAX_SPECTROGRAM_BANDS);
                            decadeLineText = string.Format("{0}K", (int)(maxFq / 1000));
                        }
                        catch(Exception ex)
                        {
                            decadeLineText = string.Empty;
                        }
                    }

                    int x = rc.Left + (int)(decadeLinePos * rc.Width);
                    Point pt1 = new Point(x, rc.Top);
                    Point pt2 = new Point(x, rc.Bottom);
                    Point pt3 = new Point(x, rc.Bottom + 5);

                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    if (i > 0 && i < DecadeLinesRelativePositions.Length - 1)
                        g.DrawLine(p, pt1, pt2);

                    SizeF sz = g.MeasureString(decadeLineText, ThemeManager.SmallestFont);
                    if (pt3.X + sz.Width > rc.Right)
                        pt3.X = (int)(rc.Right - sz.Width);

                    Rectangle rc3 = new Rectangle(pt3, sz.ToSize());

                    g.DrawString(decadeLineText, ThemeManager.SmallestFont, b, pt3);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpectrogramPlotter
            // 
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SpectrogramPlotter";
            this.ResumeLayout(false);

        }
    }
}
