using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.VideoDVDWorker
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            VideoDVDPlayer player = new VideoDVDPlayer();
            Worker.Run(player);
        }
    }
}
