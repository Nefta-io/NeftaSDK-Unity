using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Authentication
{
    public class LoginPanel : DemoPanel
    {
        [SerializeField] private InputField _emailInputField;
        [SerializeField] private Button _requestLoginCodeButton;
        [SerializeField] private Button _closeButton;

        private string _email;

        protected override void OnInit()
        {
            _requestLoginCodeButton.onClick.AddListener(RequestLoginCode);
            _closeButton.onClick.AddListener(OnClose);
        }

        public override void Open()
        {
            base.Open();

            _emailInputField.text = null;
        }

        private void RequestLoginCode()
        {
            _email = _emailInputField.text;
            if (string.IsNullOrEmpty(_email))
            {
                var error = "Email is missing";
                Debug.LogError(error);
                _statusPanel.Show(error, StatusPanel.Types.Error);
                return;
            }

            Toolbox.Instance.Authorization.RequestLoginCode(_email, OnLoginCodeComplete);
            _statusPanel.Show("Requesting login code", StatusPanel.Types.Loading);
        }

        private void OnLoginCodeComplete(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenConfirmCode(_email, true);
            }
            else
            {
                _statusPanel.Show("Error requesting code", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            _masterPanel.OpenAuthentication();
        }
    }
}
