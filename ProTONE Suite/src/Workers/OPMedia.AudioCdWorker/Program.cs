using OPMedia.Runtime.ProTONE.WorkerSupport;

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
