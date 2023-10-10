using Nefta.Core;
using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace Nefta.ToolboxDemo.Authentication
{
    public class ConvertGuestIntoFullUser : DemoPanel
    {
        [SerializeField] private InputField _emailInputField;
        [SerializeField] private Button _conformButton;
        [SerializeField] private Button _closeButton;

        private string _email;
        
        protected override void OnInit()
        {
            _conformButton.onClick.AddListener(ConvertGuest);
            _closeButton.onClick.AddListener(OnClose);
        }

        private void ConvertGuest()
        {
            _email = _emailInputField.text;
            if (string.IsNullOrEmpty(_email))
            {
                var error = "Enter your email";
                _statusPanel.Show(error, StatusPanel.Types.Error);
                Debug.LogError(error);
                return;
            }
            
            Toolbox.Instance.Authorization.GuestFullSignup(_email, OnGuestSignUp);
            _statusPanel.Show("Signing up full guest", StatusPanel.Types.Loading);
        }

        private void OnGuestSignUp(bool success)
        {
            if (success)
            {
                _masterPanel.OpenConfirmCode(_email, false);
            }
            else
            {
                _statusPanel.Show("Error signing up full guest", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            var user = Toolbox.Instance.GetUser();
            if (user != null && !string.IsNullOrEmpty(user._userId))
            {
                _masterPanel.OpenAuthentication();
            }
            else
            {
                _masterPanel.OpenUser();
            }
        }
    }
}
