using System;

namespace OPMedia.Runtime.ProTONE.RemoteControl
{
    [Serializable]
    public class ActivateCommand : BasicCommand
    {
        internal ActivateCommand()
            : base(CommandType.Activate, null)
        {
        }
    }
}
