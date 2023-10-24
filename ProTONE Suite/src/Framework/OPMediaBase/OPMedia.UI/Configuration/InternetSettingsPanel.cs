using System.Drawing;

namespace OPMedia.UI.Configuration
{
    public partial class InternetSettingsPanel : MultiPageCfgPanel
    {
        public override Image Image
        {
            get
            {
                return OPMedia.Core.Properties.Resources.Internet;
            }
        }

        public InternetSettingsPanel() : base()
        {
            this.Title = "Internet";
            InitializeComponent();
        }



    }
}