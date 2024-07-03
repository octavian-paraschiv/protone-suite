using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Themes;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;



namespace OPMedia.UI.Controls
{

    public class OPMBaseControl : UserControl
    {
        protected FontSizes _fontSize = FontSizes.Normal;

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor => base.BackColor;

        public FontSizes FontSize
        {
            get
            {
                return _fontSize;
            }

            set
            {
                _fontSize = value;

                base.Font = this.Font;

                foreach (Control ctl in this.Controls)
                {
                    if (ctl is OPMBaseControl)
                    {
                        (ctl as OPMBaseControl).FontSize = value;
                    }
                    else
                    {
                        ctl.Font = this.Font;
                    }
                }
            }
        }

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Font Font
        {
            get
            {
                return ThemeManager.GetFontBySize(_fontSize);
            }
        }

        public OPMBaseControl() : base()
        {
            this.FontSize = FontSizes.Normal;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;

            base.BackColor = ThemeManager.BackColor;

            this.RegisterAsEventSink();
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            base.BackColor = ThemeManager.BackColor;
            OnThemeUpdatedInternal();
            this.Invalidate(true);
        }

        protected virtual void OnThemeUpdatedInternal() { }
    }

}
