using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
