using System;
using System.Text;
using Nefta.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    public class AppleProvider : AuthProvider
    {
        protected override string AuthUrl { get; } = "https://appleid.apple.com/auth/authorize";
        protected override string CodeExchangeUrl { get; } = "https://appleid.apple.com/auth/token";

        private UnityWebRequest _getEmailRequest;
        
        public AppleProvider(string id, string secret, Action<string> onAuthComplete, Action<string> onAuthFail) :
            base(id, secret, onAuthComplete, onAuthFail)
        {
        }

        protected override StringBuilder GetAuthenticationUrl()
        {
            var urlBuilder = base.GetAuthenticationUrl();
            urlBuilder.Append("&scope=email");
            return urlBuilder;
        }

        protected override StringBuilder GetCodeExchangeUrl(string code)
        {
            var urlBuilder = base.GetCodeExchangeUrl(code);
            urlBuilder.Append("&grant_type=authorization_code&scope=email");
            return urlBuilder;
        }

        protected override void GetEmail()
        {
            _getEmailRequest = UnityWebRequest.Get("https://graph.facebook.com/me?fields=email");
            _getEmailRequest.SetRequestHeader("Authorization", "Bearer " + Token.access_token);
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
                var emailResponse = Toolbox.Instance.RestHelper.Deserialize<EmailResponse>(data);
                email = emailResponse.email;
            }

            _getEmailRequest.Dispose();
            _getEmailRequest = null;
            
            OnEmail(email);
        }
    }
}