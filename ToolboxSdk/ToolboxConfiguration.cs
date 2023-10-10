using System;
using System.Collections.Generic;
using Nefta.Core;

namespace Nefta.ToolboxSdk
{
    public class ToolboxConfiguration : NeftaModuleConfiguration
    {
        [Serializable]
        public class OAuthProvider
        {
            public enum Providers
            {
                Facebook,
                Google,
                Apple,
                Twitch,
                Discord
            }

            public Providers _id;
            public string _clientId;
            public string _secret;
        }

        public string _marketplaceId;
        public Toolbox.PreloadStrategies _preloadStrategy;
        public List<OAuthProvider> _oAuthProviders;
    }
}