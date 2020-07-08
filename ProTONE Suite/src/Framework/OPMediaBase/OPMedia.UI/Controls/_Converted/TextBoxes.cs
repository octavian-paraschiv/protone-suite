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

    /// <summary>
    /// Implements a text box that only accepts numbers as input.
    /// Only 2, 8, 10, 12 and 16 are supported as numbering bases.
    /// </summary>
    public class OPMNumericTextBox : OPMTextBox
    {
        public event EventHandler ValueChanged = null;

        #region Properties
        public bool AllowDecimals { get; set; }

        private decimal _min = int.MinValue;
        public decimal Minimum 
        {
            get => _min;
            set
            {
                _min = value;
                UpdateValue();
            }
        }

        private decimal _max = int.MaxValue;
        public decimal Maximum
        {
            get => _max;
            set
            {
                _max = value;
                UpdateValue();
            }
        }
        #endregion

        private decimal _value = 0;
        public decimal Value
        {
            get => _value;
            
            set
            {
                _value = value;
                UpdateValue();
            }
        }

        public new string Text
        {
            get => base.Text;
            set { }
        }

        #region Construction
        /// <summary>
        /// Parameter-based constructor that allows specifying the
        /// numbering base.
        /// </summary>
        /// <param name="numBase">The numbering base.</param>
        public OPMNumericTextBox() : base()
        {
            this.AllowDecimals = false;

            // Add event handler to filter the input.
            base.KeyDown += new KeyEventHandler(OnKeyDown);
        }
        #endregion

        private void UpdateValue()
        {
            if (_value < _min)
                _value = _min;
            if (_value > _max)
                _value = _max;

            base.Text = _value.ToString();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Event Handlers
        /// <summary>
        /// Occurs whenever the user presses a key .
        /// </summary>
        /// <param name="sender">The text box object.</param>
        /// <param name="e">The key down event data.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back:
                case Keys.Delete:
                case Keys.Left:
                case Keys.Right:
                case Keys.End:
                case Keys.Home:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    e.SuppressKeyPress = (e.Shift);
                    break;

                case Keys.Decimal:
                    e.SuppressKeyPress = (!AllowDecimals || base.Text.Contains(".") || base.Text.Contains(","));
                    break;

                default:
                    // Other keys are not valid.
                    e.SuppressKeyPress = true;
                    break;
            }
        }
        #endregion
    }
}
