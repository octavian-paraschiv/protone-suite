using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;

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
