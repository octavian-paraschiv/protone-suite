using System.Runtime.InteropServices;
using OPMedia.ShellSupport.Properties;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System.Linq;
using System.Windows.Forms;
using SharpShell.ServerRegistration;

namespace OPMedia.ShellSupport
{
    public enum CommandType
    {
       PlayFiles,
       EnqueueFiles
    }


    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    [COMServerAssociation(AssociationType.Drive)]
    [DisplayName("OPMedia Shell Extension")]
    [RegistrationName("OPMedia Shell Extension")]
    [ComVisible(true)]
    public class ShellIntegrationSuport : SharpContextMenu
    {
        //  lets create the menu strip.
        private ContextMenuStrip menu = new ContextMenuStrip();

        protected override bool CanShowMenu()
        {
            try
            {
                if (SelectedItemPaths?.Count() > 0)
                {
                    for (int i = 0; i < SelectedItemPaths.Count(); i++)
                    {
                        string path = SelectedItemPaths.ElementAt(i) ?? "";
                        if (!SupportedFileProvider.Instance.IsSupportedMedia(path))
                            return false;
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }

            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            menu.Items.Clear();

            menu.Items.Add(new ToolStripSeparator());

            var tsmi = new ToolStripMenuItem("Play with ProTONE Player", Resources.player);
            tsmi.Click += (s, a) => OnCommand(CommandType.PlayFiles);
            menu.Items.Add(tsmi);

            tsmi = new ToolStripMenuItem("Add to ProTONE Player playlist", Resources.player);
            tsmi.Click += (s, a) => OnCommand(CommandType.EnqueueFiles);
            menu.Items.Add(tsmi);

            menu.Items.Add(new ToolStripSeparator());

            // return the menu item
            return menu;
        }

        public void OnCommand(CommandType cmdType)
        {
            try
            {
                if (SelectedItemPaths?.Count() > 0)
                {
                    switch (cmdType)
                    {
                        case CommandType.PlayFiles:
                        case CommandType.EnqueueFiles:
                            PlayerSupport.SendPlayerCommand(cmdType, SelectedItemPaths.ToArray());
                            break;

                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
