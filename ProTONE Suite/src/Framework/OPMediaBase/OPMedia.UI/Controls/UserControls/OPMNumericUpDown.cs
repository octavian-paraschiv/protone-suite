using OPMedia.UI.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    public class OPMNumericUpDown : OPMBaseControl
    {
        bool _isHovered = false;
        bool _hasInput = false;
        protected OPMNumericUpDownBase nudField;

        public new event EventHandler ValueChanged = null;

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

        #region NumericUpDown-like properties
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
        public new string Text
        {
            get { return nudField.Text; }
            set { nudField.Text = value; }
        }

        public new Color BackColor
        {
            get { return nudField.BackColor; }
            set { nudField.BackColor = value; }
        }

        public bool ReadOnly
        {
            get { return nudField.ReadOnly; }
            set { nudField.ReadOnly = value; }
        }

        public HorizontalAlignment TextAlign
        {
            get { return nudField.TextAlign; }
            set { nudField.TextAlign = value; }
        }

        public string Unit
        {
            get { return nudField.Unit; }
            set { nudField.Unit = value; }
        }

        public bool UnitFirst
        {
            get { return nudField.UnitFirst; }
            set { nudField.UnitFirst = value; }
        }

        public decimal Value
        {
            get { return nudField.Value; }
            set { nudField.Value = value; }
        }

        public decimal Minimum
        {
            get { return nudField.Minimum; }
            set { nudField.Minimum = value; }
        }

        public decimal Maximum
        {
            get { return nudField.Maximum; }
            set { nudField.Maximum = value; }
        }

        public int DecimalPlaces
        {
            get { return nudField.DecimalPlaces; }
            set { nudField.DecimalPlaces = value; }
        }

        public decimal Increment
        {
            get { return nudField.Increment; }
            set { nudField.Increment = value; }
        }

        #endregion

        public new void Focus()
        {
            base.Select();
            nudField.Select();
            nudField.Focus();
        }

        public OPMNumericUpDown()
            : base()
        {
            InitializeComponent();

            base.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximumSize = new Size(2000, 25);
            this.MinimumSize = new Size(22, 25);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.MouseEnter += new EventHandler(OnMouseEnter);
            this.MouseLeave += new EventHandler(OnMouseLeave);
            this.Enter += new EventHandler(OnEnter);
            this.Leave += new EventHandler(OnLeave);

            nudField.MouseEnter += new EventHandler(OnMouseEnter);
            nudField.MouseLeave += new EventHandler(OnMouseLeave);
            nudField.Enter += new EventHandler(OnEnter);
            nudField.Leave += new EventHandler(OnLeave);

            nudField.ValueChanged += new EventHandler(OnValueChanged);
            nudField.KeyDown += new KeyEventHandler(OnKeyDown);

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

        void OnValueChanged(object sender, EventArgs e)
        {
            this.ValueChanged?.Invoke(sender, e);
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
            nudField.BackColor = ThemeManager.WndValidColor;
            nudField.ForeColor = GetForeColor();
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
            this.nudField = new OPMedia.UI.Controls.OPMNumericUpDownBase();
            ((System.ComponentModel.ISupportInitialize)(this.nudField)).BeginInit();
            this.SuspendLayout();
            // 
            // nudField
            // 
            this.nudField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.nudField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudField.Location = new System.Drawing.Point(4, 3);
            this.nudField.Margin = new System.Windows.Forms.Padding(0);
            this.nudField.Name = "nudField";
            this.nudField.Size = new System.Drawing.Size(186, 19);
            this.nudField.TabIndex = 1;
            this.nudField.Unit = null;
            this.nudField.UnitFirst = false;
            // 
            // OPMNumericUpDown
            // 
            this.Controls.Add(this.nudField);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(3000, 25);
            this.MinimumSize = new System.Drawing.Size(22, 25);
            this.Name = "OPMNumericUpDown";
            this.Size = new System.Drawing.Size(194, 25);
            ((System.ComponentModel.ISupportInitialize)(this.nudField)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
