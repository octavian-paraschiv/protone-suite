using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;

namespace OPMedia.VideoWorker
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            VideoFilePlayer player = new VideoFilePlayer();
            Worker.Run(player);
        }
    }
}
