using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.DeezerWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            ICommandProcessor proc = new CommandProcessor();
            Worker worker = new Worker("OPMedia.DeezerWorker");
            worker.Run(proc);
        }
    }
}
