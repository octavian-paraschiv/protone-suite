using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace OPMedia.DeezerInterop.OAuth
{
    public class AuthApi : IDisposable
    {
        const string RedirectUrl = "http://ocpa.ro/protone/oauth.aspx";

        static readonly string OAuthUrl = 
            $"https://connect.deezer.com/oauth/auth.php?app_id={DeezerAppConstants.AppId}&" + 
            $"redirect_uri={RedirectUrl}&perms=basic_access,offline_access,manage_library";

        static readonly string TokenUrlBase = 
            $"https://connect.deezer.com/oauth/access_token.php?app_id={DeezerAppConstants.AppId}" + 
            $"&secret={DeezerAppConstants.SecretKey}&code=";

        RedirectHandler _rh = new RedirectHandler();

        public AuthApi()
        {
        }

        public void Dispose()
        {
            _rh.Dispose();
            _rh = null;
        }

        public string GetOAuthAccessToken()
        {
            _rh.Start();

            Process.Start(OAuthUrl);

            string code = WaitCompletionAndGetQueryParameter("code");
            if (string.IsNullOrEmpty(code) == false)
            {
                string tokenUrl = TokenUrlBase + code;

                using (WebClient wc = new WebClient())
                {
                    string rsp = wc.DownloadString(tokenUrl);
                    if (rsp != null)
                    {
                        string[] args = rsp.Split('&');
                        if (args != null && args.Length > 0)
                        {
                            foreach (string arg in args)
                            {
                                if (arg != null)
                                {
                                    string[] keyValuePair = arg.Trim().Split('=');
                                    if (keyValuePair != null && keyValuePair.Length > 1)
                                    {
                                        string key = keyValuePair[0];
                                        if (key != null)
                                        {
                                            key = key.Trim().ToLowerInvariant();
                                            if (key == "access_token")
                                                return keyValuePair[1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        string WaitCompletionAndGetQueryParameter(string key)
        {
            _rh.WaitCompletion(60000);
            return _rh.GetQueryParameter(key);
        }
    }
}
