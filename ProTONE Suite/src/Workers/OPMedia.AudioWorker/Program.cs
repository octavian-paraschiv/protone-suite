using OPMedia.Runtime.ProTONE.WorkerSupport;

namespace OPMedia.AudioWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Utility.Program.Main(args) == 0)
            {
                AudioFilePlayer player = new AudioFilePlayer();
                Worker.Run(player);
            }
        }
    }
}
