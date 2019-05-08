using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;
using OPMedia.UI.Themes;
using OPMedia.Runtime.Shortcuts;

using OPMedia.Runtime;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.GlobalEvents;

namespace OPMedia.UI.Controls
{
    public class OPMDoubleBufferedControl : OPMBaseControl
    {
        private SmoothGraphics _sg = null;

        public OPMDoubleBufferedControl()
            : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            _sg = new SmoothGraphics(this);
            _sg.RenderGraphics += _sg_RenderGraphics;
        }

        private void _sg_RenderGraphics(Graphics g, Rectangle clipRect, object data)
        {
            OnRenderGraphics(g, clipRect);
        }

        protected virtual void OnRenderGraphics(Graphics g, Rectangle clipRect)
        {
        }
    }
}
