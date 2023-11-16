using System;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.Themes
{
    public delegate void RenderSmoothGraphicsEventHandler(Graphics g, Rectangle rc, object customData);

    public class SmoothGraphics : IDisposable
    {
        const BufferedGraphics NO_MANAGED_BACK_BUFFER = null;

        BufferedGraphicsContext _graphicManager;
        BufferedGraphics _managedBackBuffer;

        Control _ctl = null;

        public event RenderSmoothGraphicsEventHandler RenderGraphics = null;

        public SmoothGraphics(Control ctl, bool useCustomRenderer = false)
        {
            _ctl = ctl;

            _graphicManager = BufferedGraphicsManager.Current;
            _graphicManager.MaximumBuffer = new Size(ctl.Width, ctl.Height);
            _managedBackBuffer = _graphicManager.Allocate(ctl.CreateGraphics(), ctl.ClientRectangle);

            ctl.HandleDestroyed += Ctl_HandleDestroyed;
            ctl.Resize += Ctl_Resize;

            if (useCustomRenderer == false)
                ctl.Paint += Ctl_Paint;
        }

        private void Ctl_Paint(object sender, PaintEventArgs e)
        {
            CustomRenderer(e.Graphics, e.ClipRectangle, null);
        }

        public void CustomRenderer(Graphics g, Rectangle clipRect, object customData)
        {
            ThemeManager.PrepareGraphics(_managedBackBuffer.Graphics);

            RenderGraphics?.Invoke(_managedBackBuffer.Graphics, clipRect, customData);

            // now we draw the image into the screen
            _managedBackBuffer.Render(g);
        }

        private void Ctl_Resize(object sender, EventArgs e)
        {
            if (_managedBackBuffer != NO_MANAGED_BACK_BUFFER)
                _managedBackBuffer.Dispose();

            _graphicManager.MaximumBuffer = new Size(_ctl.Width, _ctl.Height);

            _managedBackBuffer = _graphicManager.Allocate(_ctl.CreateGraphics(), _ctl.ClientRectangle);

            _ctl.Invalidate();
        }

        private void Ctl_HandleDestroyed(object sender, EventArgs e)
        {
            // clean up the memory
            if (_managedBackBuffer != NO_MANAGED_BACK_BUFFER)
                _managedBackBuffer.Dispose();
        }

        public void Dispose()
        {
            // clean up the memory
            if (_managedBackBuffer != NO_MANAGED_BACK_BUFFER)
                _managedBackBuffer.Dispose();
        }
    }
}
