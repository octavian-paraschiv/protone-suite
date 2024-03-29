﻿using OPMedia.UI.Generic;
using OPMedia.UI.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    [ToolboxBitmap(typeof(Button))]
    public class OPMButton : Button
    {
        bool _isKeyDown = false;
        bool _isMouseDown = false;
        bool _isHovered = false;

        public event EventHandler OnDropDownClicked = null;

        const int ArrowSize = 18;

        #region GUI Properties

        #region Font Size

        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Font Font { get { return base.Font; } }

        FontSizes _fontSizes = FontSizes.Normal;
        [DefaultValue(FontSizes.Normal)]
        public FontSizes FontSize
        {
            get { return _fontSizes; }
            set
            {
                ThemeManager.SetFont(this, value);
                _fontSizes = value;

                Invalidate(true);
            }
        }
        #endregion

        #region Override settings

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor { get { return base.ForeColor; } }

        Color _overrideForeColor = Color.Empty;
        public Color OverrideForeColor
        {
            get { return _overrideForeColor; }
            set { _overrideForeColor = value; Invalidate(true); }
        }

        Color _overrideBackColor = Color.Empty;
        public Color OverrideBackColor
        {
            get { return _overrideBackColor; }
            set { _overrideBackColor = value; Invalidate(true); }
        }

        #endregion

        public bool ShowDropDown { get; set; }

        #endregion

        public OPMButton()
            : base()
        {
            this.FlatStyle = FlatStyle.Flat;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;

            this.MouseEnter += new EventHandler(OnMouseEnter);
            this.MouseLeave += new EventHandler(OnMouseLeave);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.KeyUp += new KeyEventHandler(OnKeyUp);

            this.FontSize = FontSizes.Normal;
            this.ShowDropDown = false;
        }

        protected override void OnClick(EventArgs e)
        {
            if (ShowDropDown && TestDropDownClicked())
            {
                OnDropDownClicked(this, e);
                return;
            }

            base.OnClick(e);
        }

        private bool TestDropDownClicked()
        {
            Rectangle rcDropDown = new Rectangle(this.Width - ArrowSize, 0, ArrowSize, this.Height);
            Point ptMouse = PointToClient(MousePosition);
            return (rcDropDown.Contains(ptMouse));
        }

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            _isKeyDown = false;
            Invalidate(true);
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            _isKeyDown = false;
            if (Enabled && e.Modifiers == Keys.None)
            {
                switch (e.KeyData)
                {
                    case Keys.Space:
                    case Keys.Enter:
                        _isKeyDown = true;
                        break;

                    default:
                        _isKeyDown = false;
                        break;
                }
            }

            Invalidate(true);
        }

        void OnMouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            Invalidate(true);
        }

        void OnMouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = Enabled;
            Invalidate(true);
        }

        void OnMouseLeave(object sender, EventArgs e)
        {
            _isHovered = false;
            Invalidate(true);
        }

        void OnMouseEnter(object sender, EventArgs e)
        {
            _isHovered = Enabled;
            Invalidate(true);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            ThemeManager.PrepareGraphics(g);

            int pw = 1;
            Color c1 = Color.Empty, c2 = Color.Empty, cb = Color.Empty, cText = Color.Empty;

            c1 = Enabled ? ThemeManager.GradientNormalColor1 : ThemeManager.BackColor;
            c2 = Enabled ? ThemeManager.GradientNormalColor2 : ThemeManager.BackColor;
            cb = Enabled ? ThemeManager.BorderColor : ThemeManager.GradientNormalColor2;
            cText = Enabled ? ThemeManager.ForeColor : Color.FromKnownColor(KnownColor.ControlDark);

            if (Enabled && (_isHovered || Focused))
            {
                if (_isHovered && Focused)
                {
                    c1 = ThemeManager.GradientFocusHoverColor1;
                    c2 = ThemeManager.GradientFocusHoverColor2;
                    cb = ThemeManager.FocusBorderColor;
                    //pw = 2;
                }
                else if (Focused)
                {
                    c1 = ThemeManager.GradientFocusColor1;
                    c2 = ThemeManager.GradientFocusColor2;
                    cb = ThemeManager.FocusBorderColor;
                    //pw = 2;
                }
                else
                {
                    c1 = ThemeManager.GradientHoverColor1;
                    c2 = ThemeManager.GradientHoverColor2;
                    cText = ThemeManager.SelectedTextColor;
                }
            }

            if (_overrideBackColor != Color.Empty)
            {
                c1 = c2 = _overrideBackColor;
            }
            if (_overrideForeColor != Color.Empty)
            {
                cText = _overrideForeColor;
            }

            Rectangle rcb = ClientRectangle;
            rcb.Inflate(1, 1);

            using (Brush b = new SolidBrush(ThemeManager.BackColor))
            using (Pen p = new Pen(b, 4))
            {
                g.FillRectangle(b, rcb);
            }

            Rectangle rc = ClientRectangle;

            if (_isMouseDown || _isKeyDown)
            {
                rc = new Rectangle(2, 2, Width - 4, Height - 4);
            }
            else
            {
                rc = new Rectangle(1, 1, Width - 2, Height - 2);
            }

            using (Brush b = new LinearGradientBrush(rc, c1, c2, 90f))
            using (Pen p = new Pen(cb, pw))
            {
                g.FillRectangle(b, rc);
                g.DrawRectangle(p, rc);
            }

            if (this.Image != null)
            {
                rc = ClientRectangle;

                Rectangle rcImage = new Rectangle(
                    (rc.Size.Width - Image.Size.Width) / 2,
                    (rc.Size.Height - Image.Size.Height) / 2,
                    rc.Size.Width, rc.Size.Height);

                int l = rcImage.Left;
                int t = rcImage.Top;
                rcImage.Location = new Point(l, t);

                int w = Math.Min(Image.Width, rcImage.Size.Width);
                int h = Math.Min(Image.Height, rcImage.Size.Height);
                rcImage.Size = new System.Drawing.Size(w, h);

                g.DrawImage(Image, rcImage);
            }
            else
            {
                using (Brush b = new SolidBrush(cText))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignments.FromContentAlignment(TextAlign).Alignment;
                    sf.LineAlignment = StringAlignments.FromContentAlignment(TextAlign).LineAlignment;
                    sf.Trimming = StringTrimming.EllipsisWord;
                    //sf.FormatFlags = StringFormatFlags.NoWrap;

                    sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

                    Rectangle rcText = rc;
                    if (ShowDropDown)
                        rcText = new Rectangle(0, 0, this.Width - ArrowSize, this.Height);

                    g.DrawString(this.Text, this.Font, b, rcText, sf);
                }

                if (ShowDropDown)
                {
                    Rectangle rcArrow = new Rectangle(this.Width - ArrowSize, 0, ArrowSize, this.Height);
                    using (GraphicsPath gp = ImageProcessing.GenerateCenteredArrow(rcArrow))
                    using (Brush b = new SolidBrush(cText))
                    using (Pen p = new Pen(b, 1))
                    {
                        g.FillPath(b, gp);
                        g.DrawPath(p, gp);

                        Point p1 = new Point(this.Width - ArrowSize + 2, 2);
                        Point p2 = new Point(this.Width - ArrowSize + 2, this.Height - 4);
                        g.DrawLine(p, p1, p2);
                    }
                }
            }
        }
    }
}
