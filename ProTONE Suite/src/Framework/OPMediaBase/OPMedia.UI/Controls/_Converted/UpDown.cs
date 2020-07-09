using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPMedia.UI.Controls
{
    public class OPMNumericUpDown : MetroFramework.Controls.MetroNumericUpDown
    {
        public OPMNumericUpDown()
           : base()
        {
            this.FontSize = MetroFramework.MetroLabelSize.Small;
            this.FontWeight = MetroFramework.MetroLabelWeight.Regular;
        }
    }

}
