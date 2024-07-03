using System.ComponentModel;

namespace OPMedia.Core.Shortcuts
{
    public class OPMShortcutEventArgs : HandledEventArgs
    {
        public OPMShortcut cmd;

        public override string ToString()
        {
            return string.Format("[Cmd={0}, Handled={1}]", cmd, base.Handled);
        }

        public OPMShortcutEventArgs(OPMShortcut cmd)
        {
            this.cmd = cmd;
        }
    }
}
