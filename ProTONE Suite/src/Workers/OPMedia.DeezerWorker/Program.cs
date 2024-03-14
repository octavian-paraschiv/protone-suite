using OPMedia.Runtime.ProTONE.WorkerSupport;

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
