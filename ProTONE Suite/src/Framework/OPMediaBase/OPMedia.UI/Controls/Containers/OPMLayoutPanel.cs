using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Themes;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    public class OPMTableLayoutPanel : TableLayoutPanel
    {
        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor => base.BackColor;

        public OPMTableLayoutPanel()
            : base()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;

            base.BackColor = ThemeManager.BackColor;

            this.RegisterAsEventSink();
            OnThemeUpdated();
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            base.BackColor = ThemeManager.BackColor;
        }
    }

    public class OPMFlowLayoutPanel : FlowLayoutPanel
    {
        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor => base.BackColor;

        public OPMFlowLayoutPanel()
            : base()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;

            base.BackColor = ThemeManager.BackColor;

            this.RegisterAsEventSink();
            OnThemeUpdated();
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            base.BackColor = ThemeManager.BackColor;
        }
    }
}
