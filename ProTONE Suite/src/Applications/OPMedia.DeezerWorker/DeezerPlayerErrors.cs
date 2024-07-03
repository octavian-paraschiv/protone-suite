using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Runtime.ProTONE;

namespace OPMedia.DeezerWorker
{
    public partial class DeezerPlayer
    {
        public static bool IsDzError(dz_error_t err)
        {
            switch (err)
            {
                case dz_error_t.DZ_ERROR_NO_ERROR:
                case dz_error_t.DZ_ERROR_NO_ERROR_ASYNC:
                    return false;

                default:
                    return true;
            }
        }

        public static void HandleDzErrorCode(string operation, WorkerError errorType, dz_error_t errorCode)
        {
            if (IsDzError(errorCode))
            {
                Logger.LogTrace("{0} => {1}", operation, errorCode);
                WorkerException.Throw(errorType, errorCode);
            }
        }
    }
}
