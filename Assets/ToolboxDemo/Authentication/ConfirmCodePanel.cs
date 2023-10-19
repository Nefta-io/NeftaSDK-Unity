using Nefta.Core.Data;
using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Authentication
{
    public class ConfirmCodePanel : DemoPanel
    {
        [SerializeField] private InputField _enterCodeInputField;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Text _confirmText;
        
        private string _email;
        private bool _isLogin;
        
        protected override void OnInit()
        {
            _confirmButton.onClick.AddListener(ConfirmEmail);
            _closeButton.onClick.AddListener(OnClose);
        }

        public void SetData(string email, bool isLogin)
        {
            _enterCodeInputField.text = null;
            _email = email;
            _isLogin = isLogin;
            _confirmText.text = _isLogin ? "Login" : "Confirm";
        }

        private void ConfirmEmail()
        {
            var code = _enterCodeInputField.text;
            Toolbox.Instance.Authorization.LoginWithConfirmationCode(_email, code, OnLoginWithConfirmationCode);
            _statusPanel.Show("Confirm email", StatusPanel.Types.Loading);
        }

        private void OnClose()
        {
            if (_isLogin)
            {
                _masterPanel.OpenLogin();
            }
            else
            {
                _masterPanel.OpenConvertGuestIntoFullUser();
            }
        }
        
        private void OnLoginWithConfirmationCode(NeftaUser user)
        {
            if (user != null)
            {
                _statusPanel.Hide();
                _masterPanel.OpenUser();
            }
            else
            {
                _statusPanel.Show("Error confirming email", StatusPanel.Types.Error);
            }
        }
    }
}
