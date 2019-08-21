using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core.Utilities;

namespace OPMedia.UI.Controls
{
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
