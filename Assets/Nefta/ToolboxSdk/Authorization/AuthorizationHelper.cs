using System;
using Nefta.Core;
using Nefta.Core.Data;

namespace Nefta.ToolboxSdk.Authorization
{
    public class AuthorizationHelper
    {
        private readonly Toolbox _toolbox;
        private Action<NeftaUser> _onGuestSignUp;
        private Action<bool> _onGuestFullSignUp;
        private Action<bool> _onSignUpGamer;
        private Action<bool> _onRequestLoginCode;
        private Action<NeftaUser> _onSignUpGamerWithWallet;
        private Action<NeftaUser> _onLoginWithConfirmationCode;
        private string _email;

        public AuthorizationHelper(Toolbox toolbox)
        {
            _toolbox = toolbox;
        }
        
        /// <summary>
        /// Creates a new user with the given name.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="onSignUp"></param>
        /// <returns></returns>
        public void SignUpGuestUser(string userName, Action<NeftaUser> onSignUp)
        {
            _onGuestSignUp += onSignUp;
            if (_onGuestSignUp.GetInvocationList().Length > 1)
            {
                return;
            }
            
            var data = new SignupGuestUserRequest
            {
                _publicProjectId = Toolbox.Instance.Configuration._marketplaceId,
                _username = userName
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/signup", body, OnGuestSignUp);
        }

        private void OnGuestSignUp(RestResponse restResponse)
        {
            NeftaUser neftaUser = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    neftaUser = Toolbox.Instance.RestHelper.Deserialize<NeftaUser>(restResponse.Body);
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing user: {e.Message}");   
                }

                SetNeftaUser(neftaUser, null);
            }
            _onGuestSignUp(neftaUser);
            _onGuestSignUp = null;
        }

        /// <summary>
        /// Converts guest into full user. This will send an email with confirmation code to the gamer
        /// </summary>
        /// <param name="email">Gamer email</param>
        /// <param name="onSignUp"></param>
        /// <returns></returns>
        public void GuestFullSignup(string email, Action<bool> onSignUp)
        {
            _onGuestFullSignUp += onSignUp;
            if (_onGuestFullSignUp.GetInvocationList().Length > 1)
            {
                return;
            }
            _email = email;
            
            var data = new ConvertGuestToFullUserRequest
            {
                _email = _email
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/confirm", body, OnGuestFullSignup);
        }

        private void OnGuestFullSignup(RestResponse restResponse)
        {
            _onGuestFullSignUp(restResponse.IsSuccess);
            _onGuestFullSignUp = null;
        }

        /// <summary>
        /// Creates a full user with the given email and name.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="wallet"></param>
        /// <param name="onSignUpGamer"></param>
        /// <returns></returns>
        public void SignUpGamer(string email, string username, string wallet, Action<bool> onSignUpGamer)
        {
            _onSignUpGamer += onSignUpGamer;
            if (_onSignUpGamer.GetInvocationList().Length > 1)
            {
                return;
            }
            
            _email = email;
            
            var data = new SignUpGamerRequest
            {
                _publicProjectId = Toolbox.Instance.Configuration._marketplaceId,
                _email = _email,
                _wallet = wallet,
                _username = username
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/full-signup", body, OnSignUpGamer);
        }

        private void OnSignUpGamer(RestResponse restResponse)
        {
            _onSignUpGamer(restResponse.IsSuccess);
            _onSignUpGamer = null;
        }

        /// <summary>
        /// Request a login code by given email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="onRequestLoginCode"></param>
        public void RequestLoginCode(string email, Action<bool> onRequestLoginCode)
        {
            _onRequestLoginCode += onRequestLoginCode;
            if (_onRequestLoginCode.GetInvocationList().Length > 1)
            {
                return;
            }

            var data = new RequestLoginCodeRequest
            {
                _email = email
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/confirm/resend", body, OnRequestLoginCode);
        }

        private void OnRequestLoginCode(RestResponse restResponse)
        {
            _onRequestLoginCode(restResponse.IsSuccess);
            _onRequestLoginCode = null;
        }

        /// <summary>
        /// Login by given email and confirmation code (from email).
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <param name="onLoginWithConfirmationCode"></param>
        /// <returns></returns>
        public void LoginWithConfirmationCode(string email, string code, Action<NeftaUser> onLoginWithConfirmationCode)
        {
            _onLoginWithConfirmationCode += onLoginWithConfirmationCode;
            if (_onLoginWithConfirmationCode.GetInvocationList().Length > 1)
            {
                return;
            }
            
            _email = email;
            
            var data = new LoginWithConfirmationCodeRequest
            {
                _email = email, 
                _code = code 
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/login/code", body, OnLoginWithConfirmationCode);
        }

        private void OnLoginWithConfirmationCode(RestResponse restResponse)
        {
            NeftaUser neftaUser = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    neftaUser = Toolbox.Instance.RestHelper.Deserialize<NeftaUser>(restResponse.Body);
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing user: {e.Message}");
                }
                SetNeftaUser(neftaUser, _email);
            }
            _onLoginWithConfirmationCode(neftaUser);
            _onLoginWithConfirmationCode = null;
        }
        
        /// <summary>
        /// Creates a full user based on already existing wallet that Nefta doesn't have custody over.
        /// </summary>
        /// <param name="wallet">Gamer external wallet</param>
        /// <param name="onSignUpGamerWithWallet">Callback to call when the signUp ends</param>
        public void SignUpGamerWithWallet(string wallet, Action<NeftaUser> onSignUpGamerWithWallet)
        {
            _onSignUpGamerWithWallet += onSignUpGamerWithWallet;
            if (_onSignUpGamerWithWallet.GetInvocationList().Length > 1)
            {
                return;
            }

            var data = new LoginWithWalletRequest
            {
                _wallet = wallet, 
                _mid = Toolbox.Instance.Configuration._marketplaceId
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/login/wallet", body, OnSignUpGamerWithWallet);
        }

        private void OnSignUpGamerWithWallet(RestResponse restResponse)
        {
            NeftaUser neftaUser = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    neftaUser = Toolbox.Instance.RestHelper.Deserialize<NeftaUser>(restResponse.Body);
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing user: {e.Message}");
                }
                SetNeftaUser(neftaUser, _email);
            }
            _onSignUpGamerWithWallet(neftaUser);
            _onSignUpGamerWithWallet = null;
        }

        private void SetNeftaUser(NeftaUser user, string email)
        {
            user._email = email;
            _toolbox.SetUser(user);
        }
    }
}