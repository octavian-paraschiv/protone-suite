using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OPMedia.Core;
using OPMedia.UI.Controls;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.GlobalEvents;
using ComponentFactory.Krypton.Toolkit;

namespace OPMedia.UI.Themes
{
    public partial class ThemeFormBase : KryptonForm
    {
        protected static readonly KryptonManager _mng = null;

        public bool IsActive { get => (this == Form.ActiveForm); }

        public bool TitleBarVisible { get; set; }

        static ThemeFormBase()
        {
            _mng = new KryptonManager();

            _mng.GlobalPaletteMode = PaletteModeManager.Custom;

            var pal = new KryptonPalette();

            pal.FormStyles.FormCommon.StateActive.Border.Rounding = 0;
            pal.FormStyles.FormCommon.StateActive.Border.Width = 1;
            pal.FormStyles.FormCommon.StateActive.Border.Color1 = ThemeManager.BorderColor;
            pal.FormStyles.FormCommon.StateActive.Back.Color1 = ThemeManager.BackColor;

            pal.FormStyles.FormCommon.StateInactive.Border.Rounding = 0;
            pal.FormStyles.FormCommon.StateInactive.Border.Width = 1;
            pal.FormStyles.FormCommon.StateInactive.Border.Color1 = ThemeManager.BorderColor;
            pal.FormStyles.FormCommon.StateInactive.Back.Color1 = ThemeManager.BackColor;

            pal.FormStyles.FormCommon.StateCommon.Border.Rounding = 0;
            pal.FormStyles.FormCommon.StateCommon.Border.Width = 1;
            pal.FormStyles.FormCommon.StateCommon.Back.Color1 = ThemeManager.BackColor;

            pal.FormStyles.FormCommon.StateCommon.Border.Rounding = 0;
            pal.FormStyles.FormCommon.StateCommon.Border.Width = 1;
            pal.FormStyles.FormCommon.StateCommon.Border.Color1 = ThemeManager.BorderColor;
            pal.FormStyles.FormCommon.StateCommon.Back.Color1 = ThemeManager.BackColor;

            _mng.GlobalPalette = pal;

        }

        public ThemeFormBase()
        {
            //Initialize the main thread
            if (!DesignMode)
                MainThread.Initialize(this);

            base.ControlBox = true;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            base.AutoScaleDimensions = new SizeF(1, 1);
            base.AutoScaleMode = AutoScaleMode.None;
            base.FormBorderStyle = FormBorderStyle.Sizable;

            this.Text = string.Empty;

            this.StartPosition = FormStartPosition.CenterParent;
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
        //    if (ThemeManager.IsDarkTheme)
        //    {
        //        _msm.Update();
        //    }
        //    else
        //    {
        //        _msm.Update();
        //    }

            OnThemeUpdatedInternal();
        }

        protected virtual void OnThemeUpdatedInternal()
        {
        }

        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            ThemeManager.PrepareGraphics(e.Graphics);

            Rectangle rcTitle = new Rectangle(0, 0, this.Width, 30);
            Rectangle rcText = new Rectangle(0, 0, this.Width - 60, 30);
            Rectangle rcBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            var bc = GetTitleBarColorToDraw();
            var fc = MetroPaint.ForeColor.Label.Normal(Theme);

            using (Brush bb = new SolidBrush(bc))
            using (Brush fb = new SolidBrush(fc))
            using (Pen bp = new Pen(bc, 2))
            {
                e.Graphics.FillRectangle(bb, rcTitle);
                e.Graphics.DrawRectangle(bp, rcBorder);

                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                };
                
                e.Graphics.DrawString(this.Text, ThemeManager.VeryLargeFont, fb, rcText, sf);
            }
        }*/

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ThemeFormBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ThemeFormBase";
            this.ResumeLayout(false);

        }
    }
}
