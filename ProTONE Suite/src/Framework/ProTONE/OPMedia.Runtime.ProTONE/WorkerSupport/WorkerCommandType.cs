﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
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

        StateEvt,
        StateAck,

        RenderEvt,
        RenderAck,
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
