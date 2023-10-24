using OPMedia.UI.Themes;
using System.Drawing;
using System.Windows.Forms;


namespace OPMedia.UI.ProTONE.Controls.OSD
{
    public class OSDLabel : Label
    {
        public OSDLabel()
            : base()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        int BlurAmt = 4;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ThemeManager.PrepareGraphics(g);
            using (Brush b1 = new SolidBrush(ForeColor))
            {
                for (int x = 0; x <= BlurAmt; x++)
                {
                    for (int y = 0; y <= BlurAmt; y++)
                    {
                        g.DrawString(Text, Font, Brushes.Black, new Point(x, y));
                    }
                }

                g.DrawString(Text, Font, b1, new Point(BlurAmt / 2, BlurAmt / 2));
            }
        }
    }
}
