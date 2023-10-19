using System;
using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.Authorization.AuthProviders;
using UnityEngine;

namespace ToolboxDemo.Authentication
{
    public class OAuthPanel : DemoPanel
    {

        [SerializeField] private UnityEngine.UI.Button _closeButton;

        private Action<string> _onDeepLinkActivated;

        protected override void OnInit()
        {
            _closeButton.onClick.AddListener(OnClose);
            
            Application.deepLinkActivated += OnDeepLinkActivated;
        }
        
        public void StartAuthentication(ToolboxConfiguration.OAuthProvider provider)
        {
            AuthProvider authProvider;
            switch (provider._id)
            {
                case ToolboxConfiguration.OAuthProvider.Providers.Google:
                    authProvider = new GoogleProvider(provider._clientId, provider._secret, OnAuthenticationSuccess, OnAuthenticationError);
                    break;
                case ToolboxConfiguration.OAuthProvider.Providers.Apple:
                    authProvider = new AppleProvider(provider._clientId, provider._secret, OnAuthenticationSuccess, OnAuthenticationError);
                    break;
                case ToolboxConfiguration.OAuthProvider.Providers.Facebook:
                    authProvider = new FacebookProvider(provider._clientId, provider._secret, OnAuthenticationSuccess, OnAuthenticationError);
                    break;
                case ToolboxConfiguration.OAuthProvider.Providers.Twitch:
                    authProvider = new TwitchProvider(provider._clientId, provider._secret, OnAuthenticationSuccess, OnAuthenticationError);
                    break;
                case ToolboxConfiguration.OAuthProvider.Providers.Discord:
                    authProvider = new DiscordProvider(provider._clientId, provider._secret, OnAuthenticationSuccess, OnAuthenticationError);
                    break;
                default:
                    throw new Exception("Unsupported client");
            }
            
            _onDeepLinkActivated = authProvider.OnDeepLinkActivated;
            authProvider.InitiateAuthentication();
        }

        private void OnDeepLinkActivated(string url)
        {
            Debug.Log($"OnDeepLinkActivated {url}");
            _onDeepLinkActivated?.Invoke(url);
        }
        
        private void OnAuthenticationSuccess(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                OnAuthenticationError("Could not retrieve email");
            }
            else
            {
                _masterPanel.SignUpOAuthUser(email);
            }
        }

        private void OnAuthenticationError(string error)
        {
            _closeButton.gameObject.SetActive(true);
            _statusPanel.Show(error, StatusPanel.Types.Error);
        }

        private void OnClose()
        {
            _masterPanel.OpenAuthentication();
        }
    }
}
