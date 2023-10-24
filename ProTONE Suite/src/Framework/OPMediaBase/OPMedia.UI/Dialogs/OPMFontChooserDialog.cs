using OPMedia.UI.Themes;
using System.Drawing;

namespace OPMedia.UI.Dialogs
{
    public partial class OPMFontChooserDialog : ToolForm
    {
        public string Description
        {
            get { return ctlFontChooser.Description; }
            set { ctlFontChooser.Description = value; }
        }

        public Font Font
        {
            get { return ctlFontChooser.SelectedFont; }
            set { ctlFontChooser.SelectedFont = value; }
        }

        public OPMFontChooserDialog()
        {
            InitializeComponent();
        }
    }
}
