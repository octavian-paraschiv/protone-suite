using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;
using System.ComponentModel;
using System.Drawing;

using OPMedia.UI.Themes;
using OPMedia.Core.GlobalEvents;

namespace OPMedia.UI.Controls
{
    public class OPMTableLayoutPanel : TableLayoutPanel
    {
        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor
        { get { return base.BackColor; } }

        public OPMTableLayoutPanel()
            : base()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
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
        public new Color BackColor
        { get { return base.BackColor; } }

        public OPMFlowLayoutPanel()
            : base()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
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
