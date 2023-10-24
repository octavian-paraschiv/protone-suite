using OPMedia.UI.Themes;
using System.Drawing;

namespace OPMedia.UI.Dialogs
{
    public partial class OPMColorChooserDialog : ToolForm
    {
        public string Description
        {
            get { return ctlColorChooser.Description; }
            set { ctlColorChooser.Description = value; }
        }

        public Color Color
        {
            get { return ctlColorChooser.Color; }
            set { ctlColorChooser.Color = value; }
        }

        public OPMColorChooserDialog()
        {
            InitializeComponent();
        }
    }
}
