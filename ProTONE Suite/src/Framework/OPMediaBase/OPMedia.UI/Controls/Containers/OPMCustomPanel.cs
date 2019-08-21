using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using OPMedia.UI.Generic;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core;

namespace OPMedia.UI.Controls
{
    public class OPMCustomPanel : OPMBaseControl
    {
        public bool HasBorder { get; set; }

        public int BorderWidth { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle { get; }

        public OPMCustomPanel()
            : base()
        {
            base.BorderStyle = BorderStyle.None;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.HasBorder = true;
        }

        protected override void OnThemeUpdatedInternal()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ThemeManager.PrepareGraphics(g);

            if (HasBorder)
            {
                Color cb =
                    Enabled ? ThemeManager.ForeColor : ThemeManager.GradientNormalColor2;

                Rectangle rcBorder = new Rectangle(1, 1, Width - 2, Height - 2);

                using (Brush b = new SolidBrush(this.BackColor))
                using (Pen p = new Pen(cb, BorderWidth))
                {
                    g.FillRectangle(b, ClientRectangle);
                    g.DrawRectangle(p, rcBorder);
                }
            }
        }
    }
}
