using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPMedia.UI.Controls
{
    public class OPMCheckBox : MetroFramework.Controls.MetroCheckBox
    {
        public OPMCheckBox()
            : base()
        {
            this.FontSize = MetroFramework.MetroCheckBoxSize.Small;
            this.FontWeight = MetroFramework.MetroCheckBoxWeight.Regular;
        }
    }

    public class OPMLabel : MetroFramework.Controls.MetroLabel
    {
        public OPMLabel()
            : base()
        {
            this.FontSize = MetroFramework.MetroLabelSize.Small;
            this.FontWeight = MetroFramework.MetroLabelWeight.Regular;
        }
    }

    public class OPMLinkLabel : MetroFramework.Controls.MetroLink
    {
        public OPMLinkLabel()
            : base()
        {
            this.FontSize = MetroFramework.MetroLinkSize.Small;
            this.FontWeight = MetroFramework.MetroLinkWeight.Regular;
        }
    }

    public class OPMHeaderLabel : MetroFramework.Controls.MetroLabel
    {
        public OPMHeaderLabel()
            : base()
        {
            this.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.FontWeight = MetroFramework.MetroLabelWeight.Bold;
        }
    }

    public class OPMButton : MetroFramework.Controls.MetroDropDownButton
    {
        public OPMButton()
            : base()
        {
            this.FontSize = MetroFramework.MetroButtonSize.Small;
            this.FontWeight = MetroFramework.MetroButtonWeight.Regular;

        }
    }
}
