using OPMedia.UI.Themes;
using System.Drawing;
using System.Windows.Forms;

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
