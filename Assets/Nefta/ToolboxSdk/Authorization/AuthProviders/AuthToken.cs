using System;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    [Serializable]
    public class AuthToken
    {
        public string access_token;
        public int expires_in;
        public string refresh_token;
        //public string scope; can be string or string[] depending on the provider
        public string token_type;
    }

}