using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Themes;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    public class OPMPropertyGrid : PropertyGrid
    {
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ViewForeColor { get { return base.ForeColor; } }

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ViewBackColor { get { return base.ForeColor; } }

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color CategoryForeColor { get { return base.ForeColor; } }

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color LineColor { get { return base.ForeColor; } }


        public OPMPropertyGrid()
            : base()
        {
            this.RegisterAsEventSink();
            OnThemeUpdated();
        }


        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            base.ViewForeColor = ThemeManager.WndTextColor;
            base.ViewBackColor = ThemeManager.WndValidColor;
            base.CategoryForeColor = ThemeManager.ForeColor;
            base.LineColor = ThemeManager.BackColor;
        }
    }
}
