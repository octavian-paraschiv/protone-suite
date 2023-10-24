using OPMedia.UI.Properties;
using System.Drawing;

namespace OPMedia.UI.Configuration
{
    public partial class ControlAppPanel : MultiPageCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.ControlPanel;
            }
        }

        public ControlAppPanel()
            : base()
        {
            this.Title = "TXT_S_CONTROL";
            InitializeComponent();
        }

        public void AddKeyboardPage()
        {
            AddSubPage(new KeyMapCfgPanel());
        }
    }
}