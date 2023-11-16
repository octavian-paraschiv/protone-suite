using OPMedia.Runtime.Addons;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.ProTONE.Configuration;

namespace OPMedia.MediaLibrary
{
    public class MediaLibraryForm : AddonHostForm
    {
        public MediaLibraryForm()
            : base(Program.LaunchPath)
        {
        }

        public override void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (!this.DesignMode)
            {
                if (args.Handled)
                    return;

                switch (args.cmd)
                {
                    case OPMShortcut.CmdOpenSettings:
                        ProTONESettingsForm.Show();
                        args.Handled = true;
                        break;

                    case OPMShortcut.CmdEditPath:
                        EnterEditPathMode();
                        break;
                }
            }
        }
    }
}