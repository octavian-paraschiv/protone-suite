using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OPMedia.UI.Generic;
using System.Drawing;
using OPMedia.UI.Themes;
using OPMedia.Core;
using System.Drawing.Text;
using OPMedia.Core.GlobalEvents;
using System.ComponentModel;

namespace OPMedia.UI.Controls
{
    public class OPMButton : MetroFramework.Controls.MetroButton
    {
        public event EventHandler OnDropDownClicked = null;
        const int ArrowSize = 18;

        public bool ShowDropDown { get; set; }

        public OPMButton()
            : base()
        {
            this.ShowDropDown = false;
        }

        /*
        protected override void OnClick(EventArgs e)
        {
            if (ShowDropDown && TestDropDownClicked())
            {
                OnDropDownClicked(this, e);
                return;
            }

            base.OnClick(e);
        }
        */
    }
}
