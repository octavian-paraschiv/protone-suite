using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.PlayerApi;
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
        public WorkerError ErrorType { get; private set; }

        public static void ThrowForHResult(WorkerError errorType, int hr)
        {
            if (hr < 0)
            {
                const int MAX_ERROR_TEXT_LEN = 255;
                StringBuilder sb = new StringBuilder(MAX_ERROR_TEXT_LEN);
                string msg = hr.ToString();

                if (Quartz.AMGetErrorText(hr, sb, MAX_ERROR_TEXT_LEN) > 0)
                    msg = sb.ToString();

                Throw(errorType, msg);
            }
        }

        public static void Throw(WorkerError errorType, object error)
        {
            throw new WorkerException(error.ToString()) { ErrorType = errorType };
        }

        public override string ToString()
        {
            return $"{ErrorType} ({Message})";
        }

        private WorkerException(string msg)
            : base(msg)
        {
        }

    }
}
