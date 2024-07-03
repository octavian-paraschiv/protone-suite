using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public interface IWorkerPlayer
    {
        void SetCommandProcessor(CommandProcessor proc);

        void Play(string url, string userId, int delayStart, long renderHwnd, long notifyHwnd);
        void Stop();
        void Pause();
        void Resume(int pos);
        void SetMediaPosition(int pos);
        int GetVolume();
        void SetVolume(int vol);
        int GetLength();
        int GetMediaPosition();

        FilterState GetFilterState();
    }
}
