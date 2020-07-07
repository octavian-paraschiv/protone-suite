using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;
using OPMedia.UI.Themes;
using OPMedia.Runtime.Shortcuts;

using OPMedia.Runtime;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.GlobalEvents;



namespace OPMedia.UI.Controls
{
    
    public class OPMBaseControl : UserControl
    {
        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            OnThemeUpdatedInternal();
        }

        protected virtual void OnThemeUpdatedInternal()
        {
            ApplyBackColor();
        }

        private void ApplyBackColor()
        {
            base.BackColor = ThemeManager.BackColor;
            Invalidate(true);
        }
        
        public OPMBaseControl() : base()
        {
            InitializeComponent();

            ApplyBackColor();
            
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            this.DoubleBuffered = true;

            this.RegisterAsEventSink();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OPMUserControl
            // 
            this.Name = "OPMUserControl";
            this.ResumeLayout(false);
        }
    }

}
