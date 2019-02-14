using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using OPMedia.ShoutcastWorker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.ShoutcastWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            ShoutcastPlayer player = new ShoutcastPlayer();
            Worker.Run(player);
        }
    }
}
