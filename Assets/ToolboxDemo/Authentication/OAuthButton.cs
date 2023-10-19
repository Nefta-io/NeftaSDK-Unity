using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Authentication
{
    public class OAuthButton : MonoBehaviour
    {
        [SerializeField] private ToolboxConfiguration.OAuthProvider.Providers _providerId;
        [SerializeField] private Button _button;

        private MasterPanel _masterPanel;
        private ToolboxConfiguration.OAuthProvider _provider;

        public void Init(MasterPanel masterPanel, ToolboxConfiguration configuration)
        {
            _masterPanel = masterPanel;
            
            bool isEnabled = false;
            foreach (var provider in configuration._oAuthProviders)
            {
                if (_providerId == provider._id)
                {
                    _provider = provider;
                    isEnabled = !string.IsNullOrEmpty(provider._clientId) &&
                                !string.IsNullOrEmpty(provider._secret);
                    break;
                }
            }
            
            gameObject.SetActive(isEnabled);
            if (isEnabled)
            {
                _button.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            _masterPanel.OpenOAuth2(_provider);
        }
    }
}