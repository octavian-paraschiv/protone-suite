using OPMedia.DeezerInterop.RestApi;
using OPMedia.Runtime.ProTONE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                string applicationId = ProTONEConfig.DeezerApplicationId;
                string deezerApiEndpoint = ProTONEConfig.DeezerApiEndpoint;
                DeezerRuntime dzr = new DeezerRuntime(deezerApiEndpoint, applicationId, userAccessToken);

                if (_dzr != dzr)
                    _dzr = dzr;
            }

            return _dzr;
        }
    }
}
