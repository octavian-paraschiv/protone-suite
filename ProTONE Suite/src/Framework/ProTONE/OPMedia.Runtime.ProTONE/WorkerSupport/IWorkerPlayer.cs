using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    [Flags]
    public enum SupportedMeteringData
    {
        None = 0x00,

        OutputLevels = 0x01,
        
        Levels = 0x02,
        Waveform = 0x04,
        Spectrogram = 0x06,

        All = 0xFF
    }

    public interface IWorkerPlayer
    {
        void SetCommandProcessor(CommandProcessor proc);

        void Play(string url, string userId, int delayStart);
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
