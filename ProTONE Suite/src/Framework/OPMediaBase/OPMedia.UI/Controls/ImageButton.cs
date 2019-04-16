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
                UpdateHoveredImage();
            }
        }

        public Image TipImage { get; set; }

        public ImageButton() : base()
        {
            this.TipImage = null;
            this.MouseEnter += ImageButton_MouseEnter;
            this.MouseLeave += ImageButton_MouseLeave;
            this.MouseHover += ImageButton_MouseHover;
            this.HandleDestroyed += ImageButton_HandleDestroyed;
            EventDispatch.RegisterHandler(this);
        }


        [EventSink(EventNames.ThemeUpdated)]
        private void UpdateHoveredImage()
        {
            if (base.Image == null)
                _hoverImage = null;
            else
                _hoverImage = ImageProcessing.ColorShift(base.Image, ThemeManager.HighlightColor);

            Invalidate();
        }

        private void ImageButton_MouseHover(object sender, EventArgs e)
        {
            _hovered = true;
            Invalidate();
        }

        private void ImageButton_HandleDestroyed(object sender, EventArgs e)
        {
            EventDispatch.UnregisterHandler(this);
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
                imgToDraw = base.Image;

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
