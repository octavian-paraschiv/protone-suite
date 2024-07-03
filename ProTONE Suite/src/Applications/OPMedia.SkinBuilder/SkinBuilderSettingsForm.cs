using OPMedia.Runtime.Addons.Configuration;
using System.Windows.Forms;

namespace OPMedia.SkinBuilder
{
    public class SkinBuilderSettingsForm : AddonAppSettingsForm
    {
        public new static DialogResult Show()
        {
            SkinBuilderSettingsForm _instance = new SkinBuilderSettingsForm();
            return _instance.ShowDialog();
        }

        protected override bool DissalowAddonConfigPages()
        {
            return true;
        }
    }
}
