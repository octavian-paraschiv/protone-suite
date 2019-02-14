using OPMedia.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE
{
    public enum WorkerError
    {
        Ok = 0,

        Generic = 1,

        RenderingError,

        UnsupportedMediaType,

        CannotConnectToMedia,

        MediaReadError,
    }

    public class WorkerException : Exception
    {
        public WorkerError WorkerErrorCode { get; private set; }

        public new int HResult { get; set; }

        public static void ThrowForErrorCode(WorkerError err, string msg = "")
        {
            if (string.IsNullOrEmpty(msg))
                msg = err.ToString();

            WorkerException ex = new WorkerException(msg);
            ex.WorkerErrorCode = err;
            ex.HResult = WinError.E_FAIL;
            throw ex;
        }

        public static void ThrowForHResult(int hr)
        {
            if (hr < 0)
                throw new WorkerException { HResult = hr, WorkerErrorCode = WorkerError.RenderingError };
        }

        public override string ToString()
        {
            return string.Format("Error raised by Worker Process: {0} / HResult={1}", WorkerErrorCode, HResult);
        }

        private WorkerException()
            : base()
        {
        }

        private WorkerException(string msg)
            : base(msg)
        {
        }
    }
}
