using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.AudioCdWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            AudioCdPlayer player = new AudioCdPlayer();
            Worker.Run(player);
        }
    }
}
