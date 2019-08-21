using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using OPMedia.UI.Themes;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OPMedia.UI.Generic;

namespace OPMedia.UI.Controls
{
    public class OPMTextBox : OPMBaseControl
    {
        bool _isHovered = false;
        bool _hasInput = false;
        protected TextBox txtField;
        public new event EventHandler TextChanged = null;

        #region GUI Properties


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

        private Color GetForeColor()
        {
            if (_overrideForeColor != Color.Empty)
                return _overrideForeColor;

            return ThemeManager.WndTextColor;
        }
        #endregion

        #endregion

        #region TextBoxBase-like properties
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            get { return base.Padding; }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        public string[] Lines
        {
            get { return txtField.Lines; }
            set { txtField.Lines = value; }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        public new string Text
        {
            get { return txtField.Text; }
            set { txtField.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(false)]
        public bool Multiline
        {
            get { return txtField.Multiline; }
            set
            {
                txtField.Multiline = value;
                this.MaximumSize = value ? new Size(0, 0) : new Size(2000, 22);
                this.MinimumSize = value ? new Size(0, 0) : new Size(22, 22);
            }
        }

        public CharacterCasing CharacterCasing
        {
            get { return txtField.CharacterCasing; }
            set { txtField.CharacterCasing = value; }
        }

        public int MaxLength
        {
            get { return txtField.MaxLength; }
            set { txtField.MaxLength = value; }
        }

        public bool ShortcutsEnabled
        {
            get { return txtField.ShortcutsEnabled; }
            set { txtField.ShortcutsEnabled = value; }
        }

        public char PasswordChar
        {
            get { return txtField.PasswordChar; }
            set { txtField.PasswordChar = value; }
        }

        public new Color BackColor
        {
            get { return txtField.BackColor; }
            set { txtField.BackColor = value; }
        }

        public bool UseSystemPasswordChar
        {
            get { return txtField.UseSystemPasswordChar; }
            set { txtField.UseSystemPasswordChar = value; }
        }

        public ScrollBars ScrollBars
        {
            get { return txtField.ScrollBars; }
            set { txtField.ScrollBars = value; }
        }

        public bool WordWrap
        {
            get { return txtField.WordWrap; }
            set { txtField.WordWrap = value; }
        }

        public bool ReadOnly
        {
            get { return txtField.ReadOnly; }
            set { txtField.ReadOnly = value; }
        }

        public HorizontalAlignment TextAlign
        {
            get { return txtField.TextAlign; }
            set { txtField.TextAlign = value; }
        }

        #endregion

        public new void Focus()
        {
            base.Select();
            txtField.Select();
            txtField.Focus();
        }

        public void SelectAll()
        {
            txtField.SelectAll();
        }

        public OPMTextBox()
            : base()
        {
            InitializeComponent();

            base.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.MouseEnter += new EventHandler(OnMouseEnter);
            this.MouseLeave += new EventHandler(OnMouseLeave);
            this.Enter += new EventHandler(OnEnter);
            this.Leave += new EventHandler(OnLeave);


            txtField.MouseEnter += new EventHandler(OnMouseEnter);
            txtField.MouseLeave += new EventHandler(OnMouseLeave);
            txtField.Enter += new EventHandler(OnEnter);
            txtField.Leave += new EventHandler(OnLeave);

            txtField.TextChanged += new EventHandler(OnTextChanged);
            txtField.KeyDown += new KeyEventHandler(OnKeyDown);

            this.Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            UpdateColors();
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            this.TextChanged?.Invoke(sender, e);
        }

        void OnLeave(object sender, EventArgs e)
        {
            _hasInput = false;
            Invalidate(true);
        }

        void OnEnter(object sender, EventArgs e)
        {
            _hasInput = true;
            UpdateColors();
        }

        void OnMouseLeave(object sender, EventArgs e)
        {
            _isHovered = false;
            UpdateColors();
        }

        void OnMouseEnter(object sender, EventArgs e)
        {
            _isHovered = Enabled;
            UpdateColors();
        }

        protected override void OnThemeUpdatedInternal()
        {
            UpdateColors();
        }

        private void UpdateColors()
        {
            this.OverrideBackColor = Color.Transparent;
            txtField.BackColor = ThemeManager.WndValidColor;
            txtField.ForeColor = GetForeColor();
            Invalidate(true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Color cWnd = Color.Empty, cb = Color.Empty;

            cWnd = Enabled ? ThemeManager.WndValidColor : ThemeManager.BackColor;
            cb = Enabled ? ThemeManager.BorderColor : ThemeManager.GradientNormalColor2;

            bool drawFocus = (Enabled && (_isHovered || _hasInput));

            if (drawFocus)
                cb = ThemeManager.FocusBorderColor;

            ThemeManager.PrepareGraphics(e.Graphics);

            Rectangle rc0 = this.ClientRectangle;
            Rectangle rc1 = new Rectangle(0, 0, Width - 1, Height - 1);
            Rectangle rc2 = new Rectangle(1, 1, Width - 3, Height - 3);

            using (Pen p = new Pen(cb))
            {
                e.Graphics.DrawRectangle(p, rc1);
                if (drawFocus)
                    e.Graphics.DrawRectangle(p, rc2);
            }
        }

        private void InitializeComponent()
        {
            this.txtField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtField
            // 
            this.txtField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtField.Location = new System.Drawing.Point(4, 3);
            this.txtField.Margin = new System.Windows.Forms.Padding(0);
            this.txtField.Name = "txtField";
            this.txtField.Size = new System.Drawing.Size(252, 16);
            this.txtField.TabIndex = 1;
            // 
            // OPMTextBox
            // 
            this.Controls.Add(this.txtField);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(3000, 22);
            this.MinimumSize = new System.Drawing.Size(22, 22);
            this.Name = "OPMTextBox";
            this.Size = new System.Drawing.Size(260, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
