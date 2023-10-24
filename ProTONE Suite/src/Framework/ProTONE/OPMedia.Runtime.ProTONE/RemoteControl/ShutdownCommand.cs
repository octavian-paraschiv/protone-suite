using System;

namespace OPMedia.Runtime.ProTONE.RemoteControl
{
    [Serializable]
    public class TerminateCommand : BasicCommand
    {
        internal TerminateCommand()
            : base(CommandType.Terminate, null)
        {
        }
    }
}
