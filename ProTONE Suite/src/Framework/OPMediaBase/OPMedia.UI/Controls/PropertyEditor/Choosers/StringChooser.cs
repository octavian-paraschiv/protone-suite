using System;

namespace OPMedia.UI.Controls.PropertyEditor.Choosers
{
    public partial class StringChooser : OPMBaseControl, IPropertyChooser
    {
        public event EventHandler PropertyChanged = null;

        public string PropertyName
        {
            get { return lblValueName.Text; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    lblValueName.Text = value;
                }
            }
        }

        public string PropertyValue
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }

        public StringChooser()
        {
            InitializeComponent();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
