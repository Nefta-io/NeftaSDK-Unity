using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Currencies
{
    public class CurrenciesPanel : DemoPanel
    {
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Button _getBalanceButton;
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _rewardButton.onClick.AddListener(Reward);
            _getBalanceButton.onClick.AddListener(ShowBalance);
            _closeButton.onClick.AddListener(OnClose);
        }
        
        private void Reward()
        {
            _masterPanel.OpenRewardCurrency();
        }

        private void ShowBalance()
        {
            _masterPanel.OpenBalance();
        }

        private void OnClose()
        {
            _masterPanel.OpenUser();
        }
    }
}
