using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    public class OPMSplitContainer : KryptonSplitContainer
    {
    }

    public class OPMCustomPanel : KryptonPanel
    {
    }

    public class OPMGroupBox : KryptonGroupBox
    {
    }

    public class OPMDateTimePicker : KryptonDateTimePicker
    {
    }

    public class OPMCheckBox : KryptonCheckBox
    {
    }

    public class OPMLabel : KryptonLabel
    {
    }

    public class OPMLinkLabel : KryptonLinkLabel
    {
    }

    public class OPMHeaderLabel : KryptonHeader
    {
        public Image Image
        {
            get { return null; }
            set { }
        }

        public OPMHeaderLabel()
            : base()
        {
        }
    }

    public class OPMButton : KryptonButton
    {
    }

    public class OPMDropButton : KryptonDropButton
    {
        public ContextMenuStrip SplitMenuStrip
        {
            get { return null; }
            set { }
        }

        public OPMDropButton()
            : base()
        {
        }
    }
}
