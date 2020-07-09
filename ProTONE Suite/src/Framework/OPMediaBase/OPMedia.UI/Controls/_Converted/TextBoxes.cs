using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{
    public class OPMTextBox : MetroFramework.Controls.MetroTextBox
    {
        public OPMTextBox()
           : base()
        {
            this.FontSize = MetroFramework.MetroTextBoxSize.Small;
            this.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
    }

    public class MultilineEditTextBox : OPMTextBox
    {
        public string MultiLineText
        {
            get
            {
                return StringUtils.FromStringArray(this.Lines, this.MultiLineTextSeparator);
            }

            set
            {
                base.Lines = StringUtils.ToStringArray(value, MultiLineTextSeparator);
            }
        }

        public new string[] Lines
        {
            get { return base.Lines; }
            set { base.Lines = value; }
        }

        public char MultiLineTextSeparator { get; set; }

        public MultilineEditTextBox()
            : base()
        {
            base.Multiline = true;
            this.MultiLineText = "";
            this.MultiLineTextSeparator = ';';
        }
    }
}
