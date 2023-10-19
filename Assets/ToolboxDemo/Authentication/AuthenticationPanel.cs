using System.Collections.Generic;
using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Authentication
{
    public class AuthenticationPanel : DemoPanel
    {
        [SerializeField] private Button _metaMaskSignUpButton;
        [SerializeField] private List<OAuthButton> _oAuthButtons;
        [SerializeField] private Button _guestSignUpButton;
        [SerializeField] private Button _fullUserSignUpButton;
        [SerializeField] private Button _loginButton;

        protected override void OnInit()
        {
            _metaMaskSignUpButton.onClick.AddListener(OnMetaMaskSignUpClick);
#if !NEFTA_INTEGRATION_METAMASK 
            _metaMaskSignUpButton.gameObject.SetActive(false);
#endif
            foreach (var oAuthButton in _oAuthButtons)
            {
                oAuthButton.Init(_masterPanel, Toolbox.Instance.Configuration);
            }

            _guestSignUpButton.onClick.AddListener(OnGuestSignUpClick);
            _fullUserSignUpButton.onClick.AddListener(OnFullUserSignUpClick);
            _loginButton.onClick.AddListener(OnLoginClick);
        }
        
        private void OnMetaMaskSignUpClick()
        {
            _masterPanel.OpenMetaMask();
        }

        private void OnGuestSignUpClick()
        {
            _masterPanel.OpenGuest();
        }
        
        private void OnFullUserSignUpClick()
        {
            _masterPanel.OpenFullUser();
        }
        
        private void OnLoginClick()
        {
            _masterPanel.OpenLogin();
        }
    }
}