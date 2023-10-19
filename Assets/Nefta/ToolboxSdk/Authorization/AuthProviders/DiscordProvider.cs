using System;
using System.Text;
using Nefta.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    public class DiscordProvider : AuthProvider
    {
        protected override string AuthUrl { get; } = "https://discord.com/oauth2/authorize";
        protected override string CodeExchangeUrl { get; } = "https://discord.com/api/oauth2/token";

        private UnityWebRequest _getEmailRequest;
        
        public DiscordProvider(string id, string secret, Action<string> onAuthComplete, Action<string> onAuthFail) :
            base(id, secret, onAuthComplete, onAuthFail)
        {
        }
        
        protected override StringBuilder GetAuthenticationUrl()
        {
            var urlBuilder = base.GetAuthenticationUrl();
            urlBuilder.Append("&scope=email");
            return urlBuilder;
        }

        protected override UnityWebRequest CreateExchangeForTokenRequest(string code)
        {
            WWWForm form = new WWWForm();
            form.AddField("client_id", ClientId);
            form.AddField("client_secret", ClientSecret);
            form.AddField("grant_type", "authorization_code");
            form.AddField("code", code);
            form.AddField("redirect_uri", RedirectLink);
            var uploadHandler = new UploadHandlerRaw(form.data);
            uploadHandler.contentType = "application/x-www-form-urlencoded";
            return new UnityWebRequest(CodeExchangeUrl, CodeExchangeMethod, new DownloadHandlerBuffer(), uploadHandler );
        }

        protected override void GetEmail()
        {
            _getEmailRequest = UnityWebRequest.Get("https://discord.com/api/users/@me");
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