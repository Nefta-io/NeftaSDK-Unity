using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Authentication
{
    public class FullUserPanel : DemoPanel
    {
        [SerializeField] private InputField _nameInputField;
        [SerializeField] private InputField _emailInputField;
        [SerializeField] private Button _signUpButton;
        [SerializeField] private Button _closeButton;

        private string _email;

        protected override void OnInit()
        {
            _signUpButton.onClick.AddListener(SignUpFullUser);
            _closeButton.onClick.AddListener(CloseClick);
        }

        public void SignUpOAuthUser(string email)
        {
            _emailInputField.text = email;
            
            SignUpFullUser();
        }

        private void SignUpFullUser()
        {
            var email = _emailInputField.text;
            var username = _nameInputField.text;
            if (string.IsNullOrEmpty(email))
            {
                Debug.LogError("Enter your email");
            }
            else
            {
                _email = email;
                Toolbox.Instance.Authorization.SignUpGamer(email, username, null, OnSignUpFullUser);
                _statusPanel.Show("Signing up full user", StatusPanel.Types.Loading);
            }
        }

        private void OnSignUpFullUser(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenConfirmCode(_email, false);
            }
            else
            {
                if (!gameObject.activeSelf)
                {
                    _masterPanel.OpenFullUser();
                }
                _statusPanel.Show("Error signing up full user", StatusPanel.Types.Error);
            }
        }

        private void CloseClick()
        {
            _masterPanel.OpenAuthentication();
        }
    }
}
