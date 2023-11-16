using System;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public enum WorkerEventType
    {
        Invalid,

        StreamPropertyChanged,
    }

    public enum WorkerCommandType
    {
        Invalid,

        PlayReq,
        PlayRsp,

        StopReq,
        StopRsp,

        PauseReq,
        PauseRsp,

        ResumeReq,
        ResumeRsp,

        GetPosReq,
        GetPosRsp,

        SetPosReq,
        SetPosRsp,

        GetVolReq,
        GetVolRsp,

        SetVolReq,
        SetVolRsp,

        GetLenReq,
        GetLenRsp,

        GetStateReq,
        GetStateRsp,

        WorkerEvt,
    }

    public static class WorkerCommandTypeMapper
    {
        public static string ToString(WorkerCommandType wct)
        {
            return wct.ToString();
        }

        public static WorkerCommandType FromString(string s)
        {
            WorkerCommandType wct = WorkerCommandType.Invalid;
            Enum.TryParse<WorkerCommandType>(s, out wct);
            return wct;
        }
    }
}
