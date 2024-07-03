using System;

// Ported from original header file deezer-api.h

namespace OPMedia.DeezerInterop.PlayerApi
{
    public class DeezerPlayerException : Exception
    {
        public dz_error_t DzErrorCode { get; private set; }

        public static DeezerPlayerException FromDzErrorCode(dz_error_t err, string msg = "")
        {
            if (string.IsNullOrEmpty(msg))
                msg = err.ToString();

            DeezerPlayerException ex = new DeezerPlayerException(msg);
            ex.DzErrorCode = err;
            throw ex;
        }

        public override string ToString()
        {
            return string.Format("Error raised by the Deezer Player Runtime: {0}", DzErrorCode);
        }

        private DeezerPlayerException()
            : base()
        {
        }

        private DeezerPlayerException(string msg)
            : base(msg)
        {
        }
    }
}
