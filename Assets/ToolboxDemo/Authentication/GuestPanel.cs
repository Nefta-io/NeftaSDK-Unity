using Nefta.Core.Data;
using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Authentication
{
    public class GuestPanel : DemoPanel
    {
        [SerializeField] private InputField _nameInputField;
        [SerializeField] private Button _signUpGuestButton;
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _signUpGuestButton.onClick.AddListener(SignUpGuest);
            _closeButton.onClick.AddListener(OnClose);
        }

        private void SignUpGuest()
        {
            Toolbox.Instance.Authorization.SignUpGuestUser(_nameInputField.text, OnGuestSignUp);
            _statusPanel.Show("Signing in as guest", StatusPanel.Types.Loading);
        }

        private void OnClose()
        {
            _masterPanel.OpenAuthentication();
        }

        private void OnGuestSignUp(NeftaUser user)
        {
            if (user != null)
            {
                _masterPanel.OpenUser();
                
                gameObject.SetActive(false);
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error signing in as guest", StatusPanel.Types.Error);
            }
        }
    }
}
