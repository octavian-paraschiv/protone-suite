using OPMedia.UI.Properties;
using System.Drawing;

namespace OPMedia.UI.Configuration
{
    public partial class TroubleshootingPanel : MultiPageCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.Troubleshooting;
            }
        }

        public TroubleshootingPanel()
            : base()
        {
            this.Title = "TXT_S_TROUBLESHOOTING";
            InitializeComponent();
        }
    }
}