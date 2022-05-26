using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.UI.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using OPMedia.UI.Themes;
using OPMedia.Core.GlobalEvents;

namespace OPMedia.UI.ProTONE.Controls
{
    public class VuMeterGauge : GradientGauge
    {
        Brush _b1H, _b1V, _b2H, _b2V;

        protected override void CustomizeBrushes(ref Brush b1H, ref Brush b2H, ref Brush b1V, ref Brush b2V)
        {
            try
            {
                InternalCustomizeBrushes(ref b1H, ref b2H, ref b1V, ref b2V);
            }
            catch
            {
                base.CustomizeBrushes(ref b1H, ref b2H, ref b1V, ref b2V);
            }
        }

        public VuMeterGauge() : base()
        {
            RecreateBrushes();
            this.Resize += new EventHandler(VuMeterGauge_Resize);
        }

        void VuMeterGauge_Resize(object sender, EventArgs e)
        {
            RecreateBrushes();
        }

        protected override void OnThemeUpdatedInternal()
        {
            RecreateBrushes();
        }

        public void RecreateBrushes()
        {
            if (_b1H != null)
                _b1H.Dispose();
            if (_b1V != null)
                _b1V.Dispose();
            if (_b2H != null)
                _b2H.Dispose();
            if (_b2V != null)
                _b2V.Dispose();

            _b1H = BrushHelper.GenerateVuMeterBrush(Width, Height, true);
            _b1V = BrushHelper.GenerateVuMeterBrush(Width, Height, false);

            Color cBack = ThemeManager.BackColor;
            if (_overrideBackColor != Color.Empty)
            {
                cBack = _overrideBackColor;
            }

            _b2H = new SolidBrush(cBack);
            _b2V = new SolidBrush(cBack);
        }

        private void InternalCustomizeBrushes(ref Brush b1H, ref Brush b2H, ref Brush b1V, ref Brush b2V)
        {
            //b1H = _b1H.Clone() as Brush;
            //b1V = _b1V.Clone() as Brush;
            //b2H = _b2H.Clone() as Brush;
            //b2V = _b2V.Clone() as Brush;

            b1H = _b1H;
            b1V = _b1V;
            b2H = _b2H;
            b2V = _b2V;
        }
    }
}
