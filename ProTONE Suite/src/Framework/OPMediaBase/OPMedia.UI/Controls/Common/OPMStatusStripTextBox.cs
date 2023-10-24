using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OPMedia.UI.Controls
{
    [DesignerCategory("code")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    public class OPMStatusStripTextBox : ToolStripControlHost
    {
        public new string Text
        {
            get
            {
                return (this.Control as OPMTextBox).Text;
            }

            set
            {
                (this.Control as OPMTextBox).Text = value;
            }
        }

        public new void Select()
        {
            (this.Control as OPMTextBox).Select();
        }

        public new void Focus()
        {
            (this.Control as OPMTextBox).Focus();
        }

        public OPMStatusStripTextBox()
            : base(new OPMTextBox())
        {
            (this.Control as OPMTextBox).KeyDown += new KeyEventHandler(OPMStatusStripTextBox_KeyDown);
        }

        void OPMStatusStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
    }

}
