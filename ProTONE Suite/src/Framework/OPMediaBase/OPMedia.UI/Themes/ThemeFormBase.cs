using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OPMedia.Core;
using OPMedia.UI.Controls;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.GlobalEvents;

namespace OPMedia.UI.Themes
{
    [Flags]
    public enum FormButtons
    {
        None = 0x00,

        Minimize = 0x01,
        Maximize = 0x02,
        Close = 0x04,

        All = 0x07
    }

    public enum ButtonIcons
    {
        Minimize = 0,
        MinimizeHovered,
        Maximize,
        MaximizeHovered,
        Restore,
        RestoreHovered,
        Close,
        CloseHovered,
    }

    public partial class ThemeFormBase : Form
    {
        public const int TitleBarHeight = 24;
        public const int BorderWidth = 3;

        public const int IconOffset = 4;
        public static readonly Font CaptionButtonFont = new Font("Webdings", 10, FontStyle.Bold);

        private Rectangle _rcBorder = Rectangle.Empty;

        string _text = "ABCDE";
        public new string Text
        {
            get { return _text; }
            set
            {
                _text = value;

                try
                {
                    User32.SetWindowText(Handle, _text);
                }
                catch { }

                ApplyWindowParams();
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(BorderWidth + 1, TitleBarHeight + 1, BorderWidth + 1, BorderWidth + 1); }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        public new AutoScaleMode AutoScaleMode
        { get { return base.AutoScaleMode; } }

        [ReadOnly(true)]
        [Browsable(false)]
        public new SizeF AutoScaleDimensions
        { get { return base.AutoScaleDimensions; } }

        [ReadOnly(true)]
        [Browsable(false)]
        public new SizeF AutoScaleFactor
        { get { return base.AutoScaleFactor; } }


        bool _controlBox = true;
        public new bool ControlBox
        { get { return _controlBox; } set { _controlBox = value; } }

        [ReadOnly(true)]
        [Browsable(false)]
        public new FormBorderStyle FormBorderStyle
        { get { return base.FormBorderStyle; } }

        [DefaultValue(FormButtons.All)]
        public FormButtons FormButtons { get; set; }

        private bool _allowResize = true;
        [DefaultValue(true)]
        public bool AllowResize
        {
            get { return _allowResize; }
            set { _allowResize = value; }
        }

        private bool _isToolWindow = false;
        [DefaultValue(false)]
        public bool IsToolWindow
        {
            get { return _isToolWindow; }
            set
            {
                _isToolWindow = value;

                FormButtons = FormButtons.Close;
                _titleBarFont = (_isToolWindow) ? ThemeManager.LargeFont : ThemeManager.LargeFont;
            }
        }

        bool _titleBarVisible = true;
        [Browsable(true)]
        public bool TitleBarVisible
        {
            get { return _titleBarVisible; }

            set
            {
                _titleBarVisible = value;
                ApplyWindowParams();
                Invalidate(true);
            }
        }


        Rectangle _rcTitleBar = Rectangle.Empty;
        Rectangle _rcIcon = Rectangle.Empty;
        Rectangle _rcTitle = Rectangle.Empty;
        Rectangle _rcClose = Rectangle.Empty;
        Rectangle _rcMaximize = Rectangle.Empty;
        Rectangle _rcMinimize = Rectangle.Empty;

        Font _titleBarFont = ThemeManager.LargeFont;

        int _iconLeft = 0;
        int _titleLeft = 0;
        int _btnCloseLeft = 0;
        int _btnMinimizeLeft = 0;
        int _btnMaximizeLeft = 0;
        int _titleWidth = 0;

        FormButtons _hoveredButtons = FormButtons.None;

        private bool _isActive = false;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            private set
            {
                _isActive = value;
                ApplyDrawingValues();
                Invalidate();
            }
        }

        public Size CaptionButtonSize
        {
            get
            {
                return new Size(TitleBarHeight - 2, TitleBarHeight - 2);
            }
        }

        private SmoothGraphics _sg = null;

        public ThemeFormBase()
        {
            //Initialize the main thread
            if (!DesignMode)
                MainThread.Initialize(this);

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            base.AutoScaleDimensions = new SizeF(1, 1);
            base.AutoScaleMode = AutoScaleMode.None;
            base.FormBorderStyle = FormBorderStyle.None;

            _ttm = new OPMToolTipManager(this);

            this.FormButtons = FormButtons.All;
            this.AllowResize = true;

            this.Text = string.Empty;
            this.ControlBox = false;

            this.StartPosition = FormStartPosition.CenterParent;

            this.BackColor = ThemeManager.BackColor;

            this.Shown += new EventHandler(ThemeFormBase_Shown);
            this.Load += new EventHandler(ThemeForm_Load);
            this.Resize += new EventHandler(ThemeForm_Resize);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseDoubleClick += new MouseEventHandler(ThemeForm_MouseDoubleClick);
            this.MouseHover += new EventHandler(OnMouseHover);
            this.MouseLeave += new EventHandler(OnMouseLeave);
            this.Activated += new EventHandler(OnActivated);
            this.Deactivate += new EventHandler(OnDeactivated);
            this.HandleCreated += new EventHandler(ThemeFormBase_HandleCreated);

            _sg = new SmoothGraphics(this);
            _sg.RenderGraphics += OnRenderGraphics;
        }

        void ThemeFormBase_Shown(object sender, EventArgs e)
        {
        }

        void ThemeFormBase_HandleCreated(object sender, EventArgs e)
        {
        }

        void OnDeactivated(object sender, EventArgs e)
        {
            IsActive = false;
        }

        void OnActivated(object sender, EventArgs e)
        {
            IsActive = true;
        }

        protected virtual void OnThemeUpdatedInternal()
        {
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            base.BackColor = ThemeManager.BackColor;
            ApplyWindowParams();
            ApplyTitlebarValues();
            ApplyDrawingValues();
            Invalidate(true);
            OnThemeUpdatedInternal();
        }

        #region Form resize and move operations

        /// <summary>
        /// Stores mouse cursor location at the time when the left button was pressed
        /// </summary>
        private Point _mouseDownLocation = Point.Empty;
        private bool _titleDragOperation = false;

        void ThemeForm_Load(object sender, EventArgs e)
        {
            ControlStyles cs = ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | ControlStyles.CacheText;

            SetStyle(cs, true);

            int minW = Math.Max(200, this.MinimumSize.Width);
            int minH = Math.Max(85, this.MinimumSize.Height);
            this.MinimumSize = new Size(minW, minH);

            ApplyWindowParams();

            ThemeManager.SetDoubleBuffer(this);
        }

        FormWindowState _previousState = FormWindowState.Normal;

        void ThemeForm_Resize(object sender, EventArgs e)
        {
            if (Handle != null)
            {
                ApplyWindowParams();
            }

            if (this.WindowState != _previousState)
            {
                try
                {
                    OnWindowStateChanged(_previousState, this.WindowState);
                }
                finally
                {
                    _previousState = this.WindowState;
                }
            }
        }


        public virtual void OnWindowStateChanged(FormWindowState oldState, FormWindowState newState)
        {
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sender == this && _rcTitle.Contains(e.Location))
                {
                    this.Capture = true;
                    _titleDragOperation = true;
                }

                _mouseDownLocation = e.Location;
            }
        }

        void ThemeForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_rcIcon.Contains(e.Location))
                {
                    Close();
                }
                else if (_rcTitle.Contains(e.Location) && _allowResize &&
                    (FormButtons & FormButtons.Maximize) == FormButtons.Maximize)
                {
                    if (this.WindowState != FormWindowState.Maximized)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sender == this)
                {
                    this.Capture = false;
                    _titleDragOperation = false;

                    if (_rcClose.Width > 0 && _rcClose.Contains(e.Location))
                    {
                        Close();
                    }
                    else if (_rcMinimize.Width > 0 && _rcMinimize.Contains(e.Location))
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else if (_rcMaximize.Width > 0 && _rcMaximize.Contains(e.Location))
                    {
                        if (this.WindowState != FormWindowState.Maximized)
                        {
                            this.WindowState = FormWindowState.Maximized;
                        }
                        else
                        {
                            this.WindowState = FormWindowState.Normal;
                        }
                    }

                }

                _mouseDownLocation = Point.Empty;
            }
        }

        OPMToolTipManager _ttm = null;

        void OnMouseHover(object sender, EventArgs e)
        {
            MainThread.Post(delegate (object x)
            {
                Point pt = PointToClient(MousePosition);
                if (_rcTitle != Rectangle.Empty && _rcTitle.Contains(pt))
                {
                    Graphics g = this.CreateGraphics();
                    SizeF size = g.MeasureString(_text, _titleBarFont);
                    _ttm.ShowSimpleToolTip(_text);
                    return;
                }

                if (_rcMinimize != Rectangle.Empty && _rcMinimize.Contains(pt))
                {
                    _ttm.ShowSimpleToolTip(Translator.Translate("TXT_BTNMINIMIZE"));
                    return;
                }

                if (_rcMaximize != Rectangle.Empty && _rcMaximize.Contains(pt))
                {
                    string tip = (WindowState != FormWindowState.Maximized) ?
                        Translator.Translate("TXT_BTNMAXIMIZE") :
                        Translator.Translate("TXT_BTNRESTOREDOWN");

                    _ttm.ShowSimpleToolTip(tip);
                    return;
                }

                if (_rcClose != Rectangle.Empty && _rcClose.Contains(pt))
                {
                    _ttm.ShowSimpleToolTip(Translator.Translate("TXT_BTNCLOSE"));
                }
            });
        }

        void OnMouseLeave(object sender, EventArgs e)
        {
            _hoveredButtons = Themes.FormButtons.None;
            _ttm.RemoveAll();

            Invalidate(_rcClose);
            Invalidate(_rcMinimize);
            Invalidate(_rcMaximize);
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_titleDragOperation)
                {
                    int dx = e.X - this._mouseDownLocation.X;
                    int dy = e.Y - this._mouseDownLocation.Y;

                    this.Left += dx;
                    this.Top += dy;
                }
            }

            _hoveredButtons = FormButtons.None;
            if (_rcClose != Rectangle.Empty && _rcClose.Contains(e.Location))
            {
                _hoveredButtons = FormButtons.Close;
            }
            else if (_rcMinimize != Rectangle.Empty && _rcMinimize.Contains(e.Location))
            {
                _hoveredButtons = FormButtons.Minimize;
            }
            else if (_rcMaximize != Rectangle.Empty && _rcMaximize.Contains(e.Location))
            {
                _hoveredButtons = FormButtons.Maximize;
            }
            else if (_rcTitle != Rectangle.Empty && _rcTitle.Contains(e.Location))
            {
            }
            else
            {
                //_tip.Hide(this);
            }

            Invalidate(_rcClose);
            Invalidate(_rcMinimize);
            Invalidate(_rcMaximize);

        }

        #endregion

        #region Drawing code



        private void DrawTitleBar(Graphics g)
        {
            g.FillRectangle(_brTitlebar, _rcTitleBar);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            if (_rcIcon.Width > 0)
                g.DrawImage(this.Icon.ToBitmap(), _rcIcon);

            using (Brush b = new SolidBrush(ThemeManager.ForeColor))
            {
                if (_rcTitle.Width > 0)
                {
                    g.DrawString(_text, _titleBarFont, b, _rcTitle, sf);
                }

                if (_rcMinimize.Width > 0)
                {
                    bool hovered = (_hoveredButtons & FormButtons.Minimize) == FormButtons.Minimize;
                    ButtonIcons index = hovered ? ButtonIcons.MinimizeHovered : ButtonIcons.Minimize;
                    DrawButton(g, index, _rcMinimize);
                }

                if (_rcMaximize.Width > 0)
                {
                    bool hovered = (_hoveredButtons & FormButtons.Maximize) == FormButtons.Maximize;
                    ButtonIcons index = ButtonIcons.Minimize;

                    if (WindowState == FormWindowState.Normal)
                    {
                        index = hovered ? ButtonIcons.MaximizeHovered : ButtonIcons.Maximize;
                    }
                    else
                    {
                        index = hovered ? ButtonIcons.RestoreHovered : ButtonIcons.Restore;
                    }

                    DrawButton(g, index, _rcMaximize);
                }

                if (_rcClose.Width > 0)
                {
                    bool hovered = (_hoveredButtons & FormButtons.Close) == FormButtons.Close;
                    ButtonIcons index = hovered ? ButtonIcons.CloseHovered : ButtonIcons.Close;
                    DrawButton(g, index, _rcClose);
                }
            }
        }

        private void DrawButton(Graphics g, ButtonIcons index, Rectangle rc)
        {
            Color cl1 = ThemeManager.CaptionButtonColor1;
            Color cl2 = ThemeManager.CaptionButtonColor2;
            Color clPen = ThemeManager.BorderColor;
            Color clText = ThemeManager.CaptionButtonForeColor;

            string letter = "";
            switch (index)
            {
                case ButtonIcons.Minimize:
                case ButtonIcons.MinimizeHovered:
                    letter = "0";
                    break;

                case ButtonIcons.Maximize:
                case ButtonIcons.MaximizeHovered:
                    letter = "1";
                    break;

                case ButtonIcons.Restore:
                case ButtonIcons.RestoreHovered:
                    letter = "2";
                    break;

                case ButtonIcons.Close:
                case ButtonIcons.CloseHovered:
                    letter = "r";
                    break;
            }

            if (IsActive)
            {
                float percLight = 0.4f;
                switch (index)
                {
                    case ButtonIcons.MinimizeHovered:
                        cl1 = ControlPaint.Light(cl1, percLight);
                        cl2 = ControlPaint.Light(cl2, percLight);
                        break;

                    case ButtonIcons.MaximizeHovered:
                        cl1 = ControlPaint.Light(cl1, percLight);
                        cl2 = ControlPaint.Light(cl2, percLight);
                        break;

                    case ButtonIcons.RestoreHovered:
                        cl1 = ControlPaint.Light(cl1, percLight);
                        cl2 = ControlPaint.Light(cl2, percLight);
                        break;

                    case ButtonIcons.Close:
                        clText = ThemeManager.CaptionCloseButtonForeColor;
                        break;

                    case ButtonIcons.CloseHovered:
                        cl1 = ControlPaint.Light(cl1, percLight);
                        cl2 = ControlPaint.Light(cl2, percLight);
                        clText = ThemeManager.CaptionCloseButtonForeColor;
                        break;
                }
            }
            else
            {
                float percLight = 0.9f;
                cl1 = ControlPaint.Light(cl1, percLight);
                cl2 = ControlPaint.Light(cl2, percLight);
            }

            Rectangle rcBorder = new Rectangle(rc.Location, rc.Size);
            rcBorder.Inflate(1, 0);

            Size szText = g.MeasureString(letter, CaptionButtonFont).ToSize();

            Rectangle rcText = new Rectangle(
                rcBorder.Left + (rcBorder.Width - szText.Width) / 2 + 1,
                rcBorder.Top + (rcBorder.Height - szText.Height) / 2 - 2,
                szText.Width, szText.Height);

            using (Brush bt = new SolidBrush(clText))
            using (Brush br = new LinearGradientBrush(rc, cl1, cl2, 90f))
            {
                g.FillRectangle(br, rcBorder);
                g.DrawString(letter, CaptionButtonFont, bt, rcText);
            }
        }

        #endregion

        #region helper methods

        private void ApplyWindowParams()
        {
            _rcTitleBar = (TitleBarVisible) ? new Rectangle(
                ClientRectangle.Left,
                ClientRectangle.Top,
                ClientRectangle.Width,
                Math.Min(ClientRectangle.Height, CaptionButtonSize.Height)) : Rectangle.Empty;

            _rcBorder = new Rectangle(0, 0, Width - 1, Height - 1);

            if (FormWindowState.Maximized != WindowState)
            {
                Rectangle rcRegion = new Rectangle(0, 0, Width, Height);
                base.Region = new Region(rcRegion);
            }
            else
            {
                base.Region = new Region(ClientRectangle);
            }

            ApplyDrawingValues();
            ApplyTitlebarValues();

            Invalidate(true);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            OnRenderGraphics(e.Graphics, e.ClipRectangle, null);
        }

        private void OnRenderGraphics(Graphics g, Rectangle rc, object customData)
        {
            ThemeManager.PrepareGraphics(g);

            if (_rcTitleBar != Rectangle.Empty)
            {
                g.FillRectangle(_brBackground, _rcBorder);

                if (_rcTitleBar != Rectangle.Empty)
                    DrawTitleBar(g);

                g.DrawRectangle(_penBorder, _rcBorder);
                g.Flush();
            }
            else
            {
                g.FillRectangle(Brushes.Black, _rcBorder);
                g.DrawRectangle(Pens.Black, _rcBorder);
            }
        }

        Brush _brBackground = null;
        Brush _brTitlebar = null;
        Pen _penBorder = null;

        private void ApplyDrawingValues()
        {
            float inactiveLightPercent = 0.8f;

            if (_brBackground != null)
                _brBackground.Dispose();

            _brBackground = new SolidBrush(ThemeManager.BackColor);

            if (_brTitlebar != null)
                _brTitlebar.Dispose();

            if (_rcTitleBar != Rectangle.Empty)
            {
                if (IsActive)
                {
                    _brTitlebar = new LinearGradientBrush(_rcTitleBar,
                        ThemeManager.CaptionBarColor1,
                        ThemeManager.CaptionBarColor2, 90f);



                }
                else
                {
                    _brTitlebar = new LinearGradientBrush(_rcTitleBar,
                        ControlPaint.Light(ThemeManager.CaptionBarColor1, inactiveLightPercent),
                        ControlPaint.Light(ThemeManager.CaptionBarColor2, inactiveLightPercent),
                        90f);
                }
            }

            if (_penBorder != null)
                _penBorder.Dispose();

            if (IsActive)
            {
                _penBorder = new Pen(
                    ThemeManager.BorderColor,
                    ThemeManager.FormBorderWidth);
            }
            else
            {
                _penBorder = new Pen(
                     ControlPaint.Light(ThemeManager.BorderColor, inactiveLightPercent),
                     ThemeManager.FormBorderWidth);
            }
        }

        private void ApplyTitlebarValues()
        {
            _iconLeft = 0;
            _titleLeft = 0;
            _btnCloseLeft = 0;
            _btnMinimizeLeft = 0;
            _btnMaximizeLeft = 0;

            int start = 0;

            if (this.Icon != null)
            {
                _iconLeft = IconOffset;
            }

            if (!string.IsNullOrEmpty(_text))
            {
                _titleLeft = _iconLeft + 16 + IconOffset;
            }

            if ((FormButtons & FormButtons.Close) == FormButtons.Close)
            {
                _btnCloseLeft = Width - CaptionButtonSize.Width - 2 * ThemeManager.FormBorderWidth;
            }
            if ((FormButtons & FormButtons.Maximize) == FormButtons.Maximize)
            {
                if (_btnCloseLeft > 0)
                    start = _btnCloseLeft;
                else
                    start = Width;

                _btnMaximizeLeft = start - CaptionButtonSize.Width - 1;
            }
            if ((FormButtons & FormButtons.Minimize) == FormButtons.Minimize)
            {
                if (_btnMaximizeLeft > 0)
                    start = _btnMaximizeLeft;
                else if (_btnCloseLeft > 0)
                    start = _btnCloseLeft;
                else
                    start = Width;

                _btnMinimizeLeft = start - CaptionButtonSize.Width - 1;
            }

            start = Width;
            if (_btnCloseLeft > 0)
                start = Math.Min(start, _btnCloseLeft);
            if (_btnMaximizeLeft > 0)
                start = Math.Min(start, _btnMaximizeLeft);
            if (_btnMinimizeLeft > 0)
                start = Math.Min(start, _btnMinimizeLeft);

            _titleWidth = start - IconOffset - _titleLeft;

            _rcIcon = (_iconLeft > 0) ?
                new Rectangle(_iconLeft, (CaptionButtonSize.Height + IconOffset - 16) / 2 - ThemeManager.FormBorderWidth, 16, 16) : Rectangle.Empty;
            _rcTitle = (_titleLeft > 0) ?
                new Rectangle(_titleLeft, 0, _titleWidth, CaptionButtonSize.Height) : Rectangle.Empty;

            _rcMinimize = (_btnMinimizeLeft > 0) ?
                new Rectangle(_btnMinimizeLeft, ThemeManager.FormBorderWidth,
                    CaptionButtonSize.Width, CaptionButtonSize.Height - 2 * ThemeManager.FormBorderWidth) : Rectangle.Empty;

            _rcMaximize = (_btnMaximizeLeft > 0) ?
                new Rectangle(_btnMaximizeLeft, ThemeManager.FormBorderWidth,
                    CaptionButtonSize.Width, CaptionButtonSize.Height - 2 * ThemeManager.FormBorderWidth) : Rectangle.Empty;

            _rcClose = (_btnCloseLeft > 0) ?
                new Rectangle(_btnCloseLeft, ThemeManager.FormBorderWidth,
                    CaptionButtonSize.Width, CaptionButtonSize.Height - 2 * ThemeManager.FormBorderWidth) : Rectangle.Empty;
        }

        #endregion

        #region Resize Margins

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode)
            {
                if (m.Msg == (int)Messages.WM_NCHITTEST && (int)m.Result == 0)
                {
                    m.Result = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                    return;
                }
            }
            
            base.WndProc(ref m);
        }

        private IntPtr HitTestNCA(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            Rectangle testRect = Rectangle.Empty;

            Point p = new Point((Int16)lparam, (Int16)((int)lparam >> 16));
            int vPadding = Math.Max(Padding.Right, Padding.Bottom);

            int mw = ThemeManager.FormBorderWidth;

            testRect = RectangleToScreen(new Rectangle(0, 0, mw, mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTTOPLEFT);

            testRect = RectangleToScreen(new Rectangle(Width - mw, 0, mw, mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTTOPRIGHT);

            testRect = RectangleToScreen(new Rectangle(0, Height - mw, mw, mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTBOTTOMLEFT);

            testRect = RectangleToScreen(new Rectangle(Width - mw, Height - mw, mw, mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTBOTTOMRIGHT);

            if (this.AllowResize)
            {
                if (RectangleToScreen(new Rectangle(ClientRectangle.Width - vPadding, ClientRectangle.Height - vPadding, vPadding, vPadding)).Contains(p))
                    return new IntPtr((int)HitTest.HTBOTTOMRIGHT);
            }

            testRect = RectangleToScreen(new Rectangle(0, 0, Width, mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTTOP);

            testRect = RectangleToScreen(new Rectangle(0, mw, Width, mw - mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTCAPTION);

            testRect = RectangleToScreen(new Rectangle(0, 0, mw, Height));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTLEFT);

            testRect = RectangleToScreen(new Rectangle(Width - mw, 0, mw, Height));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTRIGHT);

            testRect = RectangleToScreen(new Rectangle(0, Height - mw, Width, mw));
            if (testRect.Contains(p))
                return new IntPtr((int)HitTest.HTBOTTOM);

            return new IntPtr((int)HitTest.HTCLIENT);
        }
        #endregion
    }
}
