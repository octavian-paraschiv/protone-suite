using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OPMedia.Core;
using OPMedia.UI.Controls;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.GlobalEvents;
using MetroFramework.Components;

namespace OPMedia.UI.Themes
{
    public partial class ThemeFormBase : MetroFramework.Forms.MetroForm
    {
        MetroStyleManager _msm = null;

        public bool IsActive { get => (this == Form.ActiveForm ); }

        public bool TitleBarVisible { get; set; }

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

     
            this.Text = string.Empty;
            this.ControlBox = false;

            this.StartPosition = FormStartPosition.CenterParent;

            _msm = new MetroStyleManager();
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            if (ThemeManager.IsDarkTheme)
            {
                _msm.Style = MetroFramework.MetroColorStyle.Black;
                _msm.Theme = MetroFramework.MetroThemeStyle.Dark;
                _msm.Update();
            }
            else
            {
                _msm.Style = MetroFramework.MetroColorStyle.White;
                _msm.Theme = MetroFramework.MetroThemeStyle.Light;
                _msm.Update();
            }

            OnThemeUpdatedInternal();
        }

        protected virtual void OnThemeUpdatedInternal()
        {
        }


    }
}
