using System;
using System.Text;
using Nefta.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    public class TwitchProvider : AuthProvider
    {
        protected override string AuthUrl { get; } = "https://id.twitch.tv/oauth2/authorize";
        protected override string CodeExchangeUrl { get; } = "https://id.twitch.tv/oauth2/token";

        private UnityWebRequest _getEmailRequest;
        
        public TwitchProvider(string id, string secret, Action<string> onAuthComplete, Action<string> onAuthFail) :
            base(id, secret, onAuthComplete, onAuthFail)
        {
        }

        protected override StringBuilder GetAuthenticationUrl()
        {
            var urlBuilder = base.GetAuthenticationUrl();
            urlBuilder.Append("&scope=user:read:email");
            return urlBuilder;
        }

        protected override StringBuilder GetCodeExchangeUrl(string code)
        {
            var urlBuilder = base.GetCodeExchangeUrl(code);
            urlBuilder.Append("&grant_type=authorization_code");
            return urlBuilder;
        }
        
        protected override void GetEmail()
        {
            _getEmailRequest = UnityWebRequest.Get("https://api.twitch.tv/helix/users");
            _getEmailRequest.SetRequestHeader("Authorization", "Bearer " + Token.access_token);
            _getEmailRequest.SetRequestHeader("Client-Id", ClientId);
            _getEmailRequest.SendWebRequest().completed += OnGetEmailResponse;
        }

        private void OnGetEmailResponse(AsyncOperation operation)
        {
            var isSuccess = _getEmailRequest.result == UnityWebRequest.Result.Success;
            var data = _getEmailRequest.downloadHandler.data;
            NeftaCore.Info($"OnGetEmailResponse isSuccess:{isSuccess}, body:{Encoding.UTF8.GetString(data)}");

            string email = null;
            if (isSuccess)
            {
                var emailResponse = Toolbox.Instance.RestHelper.Deserialize<EmailsResponse>(data);
                email = emailResponse.data[0].email;
            }

            _getEmailRequest.Dispose();
            _getEmailRequest = null;
            
            OnEmail(email);
        }
    }
}