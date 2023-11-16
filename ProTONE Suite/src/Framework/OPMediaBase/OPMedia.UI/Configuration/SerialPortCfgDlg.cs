using OPMedia.UI.Themes;
using System;

namespace OPMedia.UI.Configuration
{
    public partial class SerialPortCfgDlg : ToolForm
    {
        public string PortName
        {
            get { return cfgPanel.PortName; }
            set { cfgPanel.PortName = value; }
        }

        public SerialPortCfgDlg() : base("TXT_SERIALPORTLCFG")
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            cfgPanel.SaveSettings();
        }
    }
}