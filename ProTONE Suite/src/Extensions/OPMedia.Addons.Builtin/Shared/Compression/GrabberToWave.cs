using OPMedia.Runtime.ProTONE.Compression;
using OPMedia.Runtime.ProTONE.Rendering.Cdda;
using OPMedia.Runtime.ProTONE.Rendering.Cdda.Freedb;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;

namespace OPMedia.Addons.Builtin.Shared.Compression
{
    class GrabberToWave : CdRipper
    {
        public override void Grab(CDDrive cd, Track track, string destFile, bool generateTags)
        {
            byte[] buff = base.GetTrackData(cd, track);
            WriteBuffer(buff, WaveFormatEx.Cdda, destFile);
        }

        public void WriteBuffer(byte[] buff, WaveFormatEx wfex, string destFile)
        {
            WaveFile.WriteWaveData(buff, wfex, destFile);
        }
    }
}
