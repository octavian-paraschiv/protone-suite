using OPMedia.Core.GlobalEvents;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public interface IWorkerCommandHandler
    {
        void SetCommandProcessor(CommandProcessor proc);
    }

    public interface IWorkerPlayer : IWorkerCommandHandler
    {
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

    public abstract class WorkerCommandHandler : SelfRegisteredEventSinkObject, IWorkerCommandHandler
    {
        protected CommandProcessor _proc = null;

        public void SetCommandProcessor(CommandProcessor proc)
        {
            _proc = proc;
        }
    }
}
