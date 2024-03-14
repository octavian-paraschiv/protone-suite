using OPMedia.Runtime.ProTONE.WorkerSupport;

namespace OPMedia.ShoutcastWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            ShoutcastPlayer player = new ShoutcastPlayer();
            Worker.Run(player);
        }
    }
}
