using ToolboxDemo.Assets;
using ToolboxDemo.Authentication;
using ToolboxDemo.Authentication.MetaMask;
using ToolboxDemo.Currencies;
using ToolboxDemo.Marketplace;
using ToolboxDemo.User;
using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.GamerManagement;
using Nefta.ToolboxSdk.Nft;
using UnityEngine;

namespace ToolboxDemo
{
    public class MasterPanel : MonoBehaviour
    {
        [Header("Authentication")]
        [SerializeField] private AuthenticationPanel _authenticationPanel;
        [SerializeField] private MetaMaskPanel _metaMaskPanel;
        [SerializeField] private OAuthPanel _oAuthPanel;
        [SerializeField] private GuestPanel _guestPanel;
        [SerializeField] private FullUserPanel _fullUserPanel;
        [SerializeField] private ConvertGuestIntoFullUser _convertGuestIntoFullUserPanel;
        [SerializeField] private ConfirmCodePanel _confirmCodePanel;
        [SerializeField] private LoginPanel _loginPanel;
        
        [Header("User")]
        [SerializeField] private UserPanel _userPanel;
        [SerializeField] private UserDetailsPanel _userDetailsPanel;
        [SerializeField] private OwnedAssetsPanel _ownedAssetsPanel;
        [SerializeField] private AssetRewardPanel _assetRewardPanel;
        [SerializeField] private CreateAuctionPanel _createAuctionPanel;
        [SerializeField] private AssetInfoPanel _assetInfoPanel;

        [Header("Marketplace")]
        [SerializeField] private MarketplacePanel _marketplacePanel;
        [SerializeField] private AuctionsPanel _auctionsPanel;
        
        [Header("Currencies")]
        [SerializeField] private CurrenciesPanel _currenciesPanel;
        [SerializeField] private RewardCurrencyPanel _rewardCurrencyPanel;
        [SerializeField] private BalancePanel _balancePanel;
        
        [Header("Misc")]
        [SerializeField] private RectTransform _wrapperRect;
        [SerializeField] private StatusPanel _statusPanel;

        private DemoPanel _currentPanel;
        
        private void Start()
        {
            Toolbox.Init();
            
            var safeArea = Screen.safeArea;
            var left = safeArea.xMin / Screen.width;
            var right = (Screen.width - safeArea.xMax) / Screen.width;
            var top = (Screen.height - safeArea.yMax) / Screen.height;
            var bottom = safeArea.yMin / Screen.height;
            _wrapperRect.anchorMin = new Vector2(left, bottom);
            _wrapperRect.anchorMax = new Vector2(1 - right, 1 - top);

            _statusPanel.Init();
            
            _authenticationPanel.Init(this, _statusPanel);
            
            _metaMaskPanel.Init(this, _statusPanel);
            _oAuthPanel.Init(this, _statusPanel);
            _guestPanel.Init(this, _statusPanel);
            _fullUserPanel.Init(this, _statusPanel);
            _convertGuestIntoFullUserPanel.Init(this, _statusPanel);
            _confirmCodePanel.Init(this, _statusPanel);
            _loginPanel.Init(this, _statusPanel);
            
            _userPanel.Init(this, _statusPanel);
            _userDetailsPanel.Init(this, _statusPanel);
            _ownedAssetsPanel.Init(this, _statusPanel);
            _assetRewardPanel.Init(this, _statusPanel);
            _createAuctionPanel.Init(this, _statusPanel);
            _assetInfoPanel.Init(this, _statusPanel);
            
            _marketplacePanel.Init(this, _statusPanel);
            _auctionsPanel.Init(this, _statusPanel);
            
            _currenciesPanel.Init(this, _statusPanel);
            _rewardCurrencyPanel.Init(this, _statusPanel);
            _balancePanel.Init(this, _statusPanel);

            OpenAuthentication();
            var user = Toolbox.Instance.GetUser();
            if (user != null && !string.IsNullOrEmpty(user._userId))
            {
                OpenUser();
            }
            
        }

        public void OpenAuthentication()
        {
            OpenPanel(_authenticationPanel);
        }
        
        public void OpenMetaMask()
        {
            OpenPanel(_metaMaskPanel);
        }

        public void OpenOAuth2(ToolboxConfiguration.OAuthProvider provider)
        {
            OpenPanel(_oAuthPanel);
            _oAuthPanel.StartAuthentication(provider);
        }
        
        public void OpenGuest()
        {
            OpenPanel(_guestPanel);
        }

        public void OpenFullUser()
        {
            OpenPanel(_fullUserPanel);
        }

        public void OpenConvertGuestIntoFullUser()
        {
            OpenPanel(_convertGuestIntoFullUserPanel);
        }

        public void SignUpOAuthUser(string email)
        {
            _fullUserPanel.SignUpOAuthUser(email);
        }

        public void OpenConfirmCode(string email, bool isLogin)
        {
            _confirmCodePanel.SetData(email, isLogin);
            OpenPanel(_confirmCodePanel);
        }

        public void OpenLogin()
        {
            OpenPanel(_loginPanel);
        }
        
        public void OpenUser()
        {
            OpenPanel(_userPanel);
        }

        public void OpenUserDetails()
        {
            OpenPanel(_userDetailsPanel);
        }

        public void OpenOwnedAssetsPanel()
        {
            OpenPanel(_ownedAssetsPanel);
        }

        public void OpenAssetInfo(NftAsset asset)
        {
            OpenPanel(_assetInfoPanel);
            _assetInfoPanel.SetAsset(asset);
        }

        public void OpenAssetRewardPanel()
        {
            OpenPanel(_assetRewardPanel);
        }

        public void OpenCreateAuction(OwnedAsset item)
        {
            OpenPanel(_createAuctionPanel);
            _createAuctionPanel.SetData(item);
        }

        public void OpenAuctions()
        {
            OpenPanel(_auctionsPanel);
        }

        public void OpenMarketplace()
        {
            OpenPanel(_marketplacePanel);
        }

        public void OpenCurrencies()
        {
            OpenPanel(_currenciesPanel);
        }

        public void OpenRewardCurrency()
        {
            OpenPanel(_rewardCurrencyPanel);
        }

        public void OpenBalance(string currencyId=null)
        {
            OpenPanel(_balancePanel);
            if (currencyId != null)
            {
                _balancePanel.ShowBalance(currencyId);
            }
        }
        
        private void OpenPanel(DemoPanel demoPanel)
        {
            if (_currentPanel != null)
            {
                _currentPanel.Close();
            }

            _statusPanel.Hide();
            
            _currentPanel = demoPanel;
            _currentPanel.Open();
        }
    }
}