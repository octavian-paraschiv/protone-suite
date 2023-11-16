using OPMedia.UI.Themes;
using System;
using System.Drawing;

namespace OPMedia.UI.Controls.PropertyEditor.Choosers
{
    public class SBColorChooser : OPMColorChooserCtl, IPropertyChooser
    {
        ColorConverter cc = new ColorConverter();

        public event EventHandler PropertyChanged;

        public string PropertyName
        {
            get { return base.Description; }
            set { base.Description = value; }
        }

        public string PropertyValue
        {
            get
            {
                return cc.ConvertToInvariantString(base.Color);
            }

            set
            {
                base.Color = ThemeManager.SafeColorFromString(value);
            }
        }

        public SBColorChooser()
            : base()
        {
            base.Description = string.Empty;
            base.ColorChanged += new EventHandler(SBColorChooser_ColorChanged);
        }

        void SBColorChooser_ColorChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
