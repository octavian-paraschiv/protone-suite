using OPMedia.DeezerInterop.RestApi;
using OPMedia.Runtime.ProTONE.Configuration;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public static class DeezerRuntimeFactory
    {
        private static DeezerRuntime _dzr = null;


        public static DeezerRuntime GetRuntime()
        {
            if (ProTONEConfig.DeezerHasValidConfig)
            {
                string userAccessToken = ProTONEConfig.DeezerUserAccessToken;
                DeezerRuntime dzr = new DeezerRuntime(userAccessToken);

                if (_dzr != dzr)
                    _dzr = dzr;
            }

            return _dzr;
        }
    }
}
