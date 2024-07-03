using OPMedia.Runtime.ProTONE.WorkerSupport;

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
