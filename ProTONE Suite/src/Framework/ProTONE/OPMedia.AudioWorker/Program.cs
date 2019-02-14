using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.AudioWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            AudioFilePlayer player = new AudioFilePlayer();
            Worker.Run(player);
        }
    }
}
