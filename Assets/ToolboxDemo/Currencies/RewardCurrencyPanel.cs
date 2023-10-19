using System.Globalization;
using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Currencies
{
    public class RewardCurrencyPanel : DemoPanel
    {
        [SerializeField] private InputField _currencyIdInputField;
        [SerializeField] private InputField _amountInputField;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Button _closeButton;

        private string _currencyId;
        
        protected override void OnInit()
        {
            _rewardButton.onClick.AddListener(RewardUser);
            _closeButton.onClick.AddListener(OnClose);
        }
        
        private void RewardUser()
        {
            _currencyId = _currencyIdInputField.text;
            if (string.IsNullOrEmpty(_currencyId))
            {
                var error = "Currency id is missing";
                _statusPanel.Show(error, StatusPanel.Types.Error);
                Debug.LogError(error);
                return;
            }

            var amountString = _amountInputField.text;
            if (string.IsNullOrEmpty(amountString))
            {
                var error = "Currency amount is missing";
                _statusPanel.Show(error, StatusPanel.Types.Error);
                Debug.LogError(error);
                return;
            }

            if (!float.TryParse(amountString, NumberStyles.Float, CultureInfo.InvariantCulture, out var amount))
            {
                var error = "Amount is not a number";
                _statusPanel.Show(error, StatusPanel.Types.Error);
                Debug.LogError(error);
                return;
            }
            
            Toolbox.Instance.GamerAssets.CreateCurrencyRewardForUser(_currencyId, amount, OnCreateCurrencyReward);
            _statusPanel.Show("Creating currency reward", StatusPanel.Types.Loading);
        }

        private void OnCreateCurrencyReward(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenBalance(_currencyId);
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error creating currency reward", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            _masterPanel.OpenCurrencies();
        }
    }
}