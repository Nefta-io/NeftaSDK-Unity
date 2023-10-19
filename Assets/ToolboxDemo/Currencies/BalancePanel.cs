using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.GamerManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Currencies
{
    public class BalancePanel : DemoPanel
    {
        [SerializeField] private InputField _currencyIdInputField;
        [SerializeField] private Text _balanceText;
        [SerializeField] private Button _showCurrencyButton;
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _showCurrencyButton.onClick.AddListener(ShowCurrencyBalance);
            _closeButton.onClick.AddListener(OnClose);
        }

        public void ShowBalance(string currencyId)
        {
            Toolbox.Instance.GamerManagement.GetGamerCurrencyBalance(currencyId, OnGetGamerCurrencyBalance);
            _statusPanel.Show("Getting currency balance", StatusPanel.Types.Loading);
        }
        
        private void ShowCurrencyBalance()
        {
            ShowBalance(_currencyIdInputField.text);
        }

        private void OnGetGamerCurrencyBalance(CurrencyBalance balance)
        {
            if (balance != null)
            {
                _balanceText.text = "balance: " + balance._balance;
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error getting currency balance", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            _masterPanel.OpenCurrencies();
        }
    }
}