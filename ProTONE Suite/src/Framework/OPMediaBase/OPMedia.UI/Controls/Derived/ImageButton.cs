using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Generic;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    public class ImageButton : PictureBox
    {
        bool _hovered = false;
        bool _checked = false;

        private Image _hoverImage = null;
        private Image _normalImage = null;

        public bool Checked
        {
            get
            {
                return _checked;
            }

            set
            {
                _checked = value;
                Invalidate();
            }
        }

        public new Image Image
        {
            get
            {
                return base.Image;
            }

            set
            {
                base.Image = value;
                OnThemeUpdated();
            }
        }

        public Image TipImage { get; set; }

        public ImageButton() : base()
        {
            this.TipImage = null;
            this.MouseEnter += ImageButton_MouseEnter;
            this.MouseLeave += ImageButton_MouseLeave;
            this.MouseHover += ImageButton_MouseHover;

            this.RegisterAsEventSink();
            OnThemeUpdated();
        }


        [EventSink(EventNames.ThemeUpdated)]
        private void OnThemeUpdated()
        {
            if (base.Image == null)
            {
                _hoverImage = null;
                _normalImage = null;
            }
            else
            {
                _hoverImage = ImageProcessing.ColorShift(base.Image, ThemeManager.HighlightColor);
                _normalImage = ImageProcessing.ColorShift(base.Image, ThemeManager.ForeColor);
            }

            Invalidate();
        }

        private void ImageButton_MouseHover(object sender, EventArgs e)
        {
            _hovered = true;
            Invalidate();
        }


        private void ImageButton_MouseLeave(object sender, EventArgs e)
        {
            _hovered = false;
            Invalidate();
        }

        private void ImageButton_MouseEnter(object sender, EventArgs e)
        {
            _hovered = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            DoPaint(pevent.Graphics);
        }

        private void DoPaint(Graphics g)
        {
            ThemeManager.PrepareGraphics(g);

            using (Brush b = new SolidBrush(ThemeManager.BackColor))
            {
                g.FillRectangle(b, this.ClientRectangle);
            }

            Image imgToDraw = null;

            if (_hovered || _checked)
                imgToDraw = _hoverImage;
            else
                imgToDraw = _normalImage;

            if (imgToDraw != null)
            {
                Size szDraw = new Size(Math.Min(imgToDraw.Width, this.Width),
                    Math.Min(imgToDraw.Height, this.Height));

                Rectangle rc = new Rectangle((this.Width - szDraw.Width) / 2, (this.Height - szDraw.Height) / 2, szDraw.Width, szDraw.Height);

                g.DrawImage(imgToDraw, rc);
            }

        }

    }
}
