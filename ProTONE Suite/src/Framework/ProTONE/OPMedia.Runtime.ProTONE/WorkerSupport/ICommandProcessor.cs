using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public interface ICommandProcessor
    {
        void SetEventStream(StreamWriter evtStream);

        WorkerCommand Process(WorkerCommand cmd);
        void StateEvt(FilterState state);

        void RenderEvt(int pos);
    }
}
