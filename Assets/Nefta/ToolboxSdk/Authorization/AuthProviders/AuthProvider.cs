using System;
using System.Net;
using System.Text;
using System.Threading;
using Nefta.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    public abstract class AuthProvider
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private const string TargetRedirectLink = "com.nefta.testbed:/oauth2";
#else
        private const string TargetRedirectLink = "http://localhost:8080/";
#endif
        protected const string RedirectLink = "https://api.nefta.io/sdk/auth/redirect";

        protected virtual string AuthUrl { get; }
        protected virtual string CodeExchangeMethod { get; } = UnityWebRequest.kHttpVerbPOST;
        protected virtual string CodeExchangeUrl { get; }

        protected readonly string ClientId;
        protected readonly string ClientSecret;

        protected readonly SynchronizationContext _synchronizationContext;
        protected HttpListener HttpListener;
        protected UnityWebRequest ExchangeCodeForTokenRequest;
        protected AuthToken Token;
        protected readonly Action<string> OnAuthenticationComplete; 
        protected readonly Action<string> OnAuthenticationError;

        protected AuthProvider(string id, string secret, Action<string> onAuthComplete, Action<string> onAuthError)
        {
            ClientId = id;
            ClientSecret = secret;
            OnAuthenticationComplete = onAuthComplete;
            OnAuthenticationError = onAuthError;

            _synchronizationContext = SynchronizationContext.Current;
            
#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR
            StartLocalWebserver();
#endif
        }

        ~AuthProvider()
        {
            if (HttpListener != null)
            {
                HttpListener.Stop();
                HttpListener = null;
            }
        }

        protected virtual StringBuilder GetAuthenticationUrl()
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(AuthUrl);
            urlBuilder.Append("?client_id=");
            urlBuilder.Append(ClientId);
            urlBuilder.Append("&redirect_uri=");
            urlBuilder.Append(UnityWebRequest.EscapeURL(RedirectLink));
            urlBuilder.Append("&state=");
            urlBuilder.Append(UnityWebRequest.EscapeURL(TargetRedirectLink));
            urlBuilder.Append("&response_type=code");

            return urlBuilder;
        }

        protected virtual StringBuilder GetCodeExchangeUrl(string code)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(CodeExchangeUrl);
            urlBuilder.Append("?client_id=");
            urlBuilder.Append(ClientId);
            urlBuilder.Append("&client_secret=");
            urlBuilder.Append(ClientSecret);
            urlBuilder.Append("&redirect_uri=");
            urlBuilder.Append(UnityWebRequest.EscapeURL(RedirectLink));
            urlBuilder.Append("&code=");
            urlBuilder.Append(code); 
            return urlBuilder;
        }

        public void InitiateAuthentication()
        {
            NeftaCore.Log("InitiateAuthentication");
            var url = GetAuthenticationUrl().ToString();
            Application.OpenURL(url);
        }
        
        private void StartLocalWebserver()
        {
            Debug.Log("server start");
            HttpListener = new HttpListener();
            HttpListener.Prefixes.Add(TargetRedirectLink);
            HttpListener.Start();
            HttpListener.BeginGetContext(OnHttpResponse, HttpListener);
        }

        private void OnHttpResponse(IAsyncResult result)
        {
            var httpContext = HttpListener.EndGetContext(result);
            var httpRequest = httpContext.Request;

            var code = httpRequest.QueryString.Get("code");
            var state = httpRequest.QueryString.Get("state");

            string status;
            if (!string.IsNullOrEmpty(code) && state == TargetRedirectLink)
            {
                status = "Authentication successful";
                _synchronizationContext.Post(ExchangeCodeForToken, code);
            }
            else
            {
                status = "Authentication failed";
                _synchronizationContext.Post(PostError, "Authentication failed");
            }
            
            var httpResponse = httpContext.Response;
            var responseString = $"<html><body><b>{status}!</b><br>(You can close this tab/window now)</body></html>";
            var buffer = Encoding.UTF8.GetBytes(responseString);

            httpResponse.ContentLength64 = buffer.Length;
            var output = httpResponse.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            HttpListener.Stop();
            HttpListener = null;
        }

        public void OnDeepLinkActivated(string deepLink)
        {
            string queryString = new Uri(deepLink).Query;
            var parameters = System.Web.HttpUtility.ParseQueryString(queryString);
            var code = parameters.Get("code");
            var state = parameters.Get("state");
            if (!string.IsNullOrEmpty(code) && state.StartsWith(TargetRedirectLink))
            {
                ExchangeCodeForToken(code);
            }
        }

        private void ExchangeCodeForToken(object codeObject)
        {
            var code = (string)codeObject;
            NeftaCore.Log($"ExchangeCodeForToken {code}");

            ExchangeCodeForTokenRequest = CreateExchangeForTokenRequest(code);
            ExchangeCodeForTokenRequest.SendWebRequest().completed += OnExchangeCodeForTokenResponse;
        }

        protected virtual UnityWebRequest CreateExchangeForTokenRequest(string code)
        {
            var url = GetCodeExchangeUrl(code).ToString();
            return new UnityWebRequest(url, CodeExchangeMethod, new DownloadHandlerBuffer(), new UploadHandlerRaw(null));
        }

        private void PostError(object error)
        {
            OnAuthenticationError((string) error);
        }
    
        private void OnExchangeCodeForTokenResponse(AsyncOperation operation)
        {
            var isSuccess = ExchangeCodeForTokenRequest.result == UnityWebRequest.Result.Success;
            var data = ExchangeCodeForTokenRequest.downloadHandler.data;
            NeftaCore.Info($"OnExchangeCodeForTokenResponse isSuccess:{isSuccess}, body:{Encoding.UTF8.GetString(data)}");
            
            if (isSuccess)
            {
                try
                {
                    Token = Toolbox.Instance.RestHelper.Deserialize<AuthToken>(data);
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing token: {e.Message}");
                }
            }
            
            ExchangeCodeForTokenRequest.Dispose();
            ExchangeCodeForTokenRequest = null;

            if (Token != null)
            {
                GetEmail();   
            }
            else
            {
                OnAuthenticationError("Error exchanging code for token");
            }
        }

        protected virtual void GetEmail()
        {

        }

        protected void OnEmail(string email)
        {
            NeftaCore.Log($"OnEmail email:{email}");
            OnAuthenticationComplete(email);
        }
    }
}