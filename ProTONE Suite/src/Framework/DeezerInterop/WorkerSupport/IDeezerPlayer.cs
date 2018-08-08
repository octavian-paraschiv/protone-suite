using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.DeezerInterop.WorkerSupport
{
    public interface IDeezerPlayer
    {
        void Play(string url);
        void Stop();
        void Pause();
        void Resume(int pos);
        void SetMediaPosition(int pos);
        int GetVolume();
        void SetVolume(int vol);
        int GetLength();
        int GetMediaPosition();
    }
}
