using Nefta.Core.Data;
using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.User
{
    public class UserPanel : DemoPanel
    {
        [SerializeField] private Text _usernameText;
        [SerializeField] private Text _userTokenText;
        [SerializeField] private Text _userIdText;
        [SerializeField] private Text _emailText;
        [SerializeField] private Button _getUserDetailsButton;
        [SerializeField] private Button _interactWithAssetsButton;
        [SerializeField] private Button _interactWithMarketplaceButton;
        [SerializeField] private Button _interactWithCurrenciesButton;
        [SerializeField] private Button _convertGuestIntoFullUserButton;

        public override void Open()
        {
            base.Open();
            
            _getUserDetailsButton.onClick.AddListener(OnGetUserDetails);
            _interactWithAssetsButton.onClick.AddListener(OnInteractWithAssets);
            _interactWithMarketplaceButton.onClick.AddListener(OnInteractWithMarketplace);
            _interactWithCurrenciesButton.onClick.AddListener(OnInteractWithCurrencies);
            _convertGuestIntoFullUserButton.onClick.AddListener(OnConvertIntoFullUser);
            
            UpdateText(Toolbox.Instance.GetUser());
        }

        private void UpdateText(NeftaUser neftaUser)
        {
            _usernameText.text = "username: " + neftaUser?._username;
            _userTokenText.text = "user_token: " + neftaUser?._token;
            _userIdText.text = "user_id: " + neftaUser?._userId;
            _emailText.text = "user_email: " + neftaUser?._email;
            
            _convertGuestIntoFullUserButton.gameObject.SetActive(string.IsNullOrEmpty(neftaUser?._email));
        }
        
        public void ShowOwnedAssetsPanel()
        {
            _masterPanel.OpenOwnedAssetsPanel();
        }

        private void OnGetUserDetails()
        {
            _masterPanel.OpenUserDetails();
        }

        private void OnInteractWithAssets()
        {
            _masterPanel.OpenAssetRewardPanel();
        }

        private void OnInteractWithMarketplace()
        {
            _masterPanel.OpenMarketplace();
        }

        private void OnInteractWithCurrencies()
        {
            _masterPanel.OpenCurrencies();
        }

        private void OnConvertIntoFullUser()
        {
            _masterPanel.OpenConvertGuestIntoFullUser();
        }
    }
}
