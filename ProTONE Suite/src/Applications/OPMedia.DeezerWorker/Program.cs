﻿using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
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
            DeezerPlayer player = new DeezerPlayer();
            Worker.Run(player);
        }
    }
}
