using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.GamerManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Nefta.ToolboxDemo.User
{
    public class UserDetailsPanel : DemoPanel
    {
        [SerializeField] private Text _userAddressText;
        [SerializeField] private Text _balanceText;
        [SerializeField] private Button _getUserProfileButton;
        [SerializeField] private Button _getOwnedAssetsButton;
        [SerializeField] private Button _getGamerCryptoBalanceButton;
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _getUserProfileButton.onClick.AddListener(GetUserProfile);
            _getOwnedAssetsButton.onClick.AddListener(GetUserOwnedAssets);
            _getGamerCryptoBalanceButton.onClick.AddListener(GetGamerCryptoBalance);
            _closeButton.onClick.AddListener(OnClose);
        }

        public override void Open()
        {
            base.Open();

            SetText(Toolbox.Instance.GetUser()._address);
        }

        private void SetText(string address)
        {
            _userAddressText.text = $"Address: {address}";
        }

        private void GetUserProfile()
        {
            Toolbox.Instance.GamerManagement.GetGamerProfile(OnGetUserProfile);
            _statusPanel.Show("Get user profile", StatusPanel.Types.Loading);
        }

        private void OnGetUserProfile(GamerProfile gamerProfile)
        {
            if (gamerProfile != null)
            {
                SetText(gamerProfile._address);
                Debug.Log("User wallet address: " + gamerProfile._address);
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error getting user profile", StatusPanel.Types.Error);
            }
        }

        private void GetUserOwnedAssets()
        {
            _masterPanel.OpenOwnedAssetsPanel();
        }

        private void GetGamerCryptoBalance()
        {
            Toolbox.Instance.GamerManagement.GetGamerCryptoBalance(OnGetGamerCryptoBalance);
            _statusPanel.Show("Getting gamer crypto balance", StatusPanel.Types.Loading);
        }

        private void OnGetGamerCryptoBalance(NativeCurrencyBalance balance)
        {
            if (balance != null)
            {
                _balanceText.text = "balance: " + balance._coinBalanceName + " " + balance._coinBalance;
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error getting gamer crypto balance", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            _masterPanel.OpenUser();
        }
    }
}
