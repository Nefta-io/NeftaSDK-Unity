using Nefta.ToolboxSdk;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Assets
{
    public class AssetRewardPanel : DemoPanel
    {
        [SerializeField] private InputField assetIdInputField;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _rewardButton.onClick.AddListener(CreateAssetRewardForUser);
            _closeButton.onClick.AddListener(OnClose);
        }

        public override void Open()
        {
            base.Open();
            
            assetIdInputField.text = null;
        }
        
        private void CreateAssetRewardForUser()
        {
            var assetId = assetIdInputField.text;
            if (string.IsNullOrEmpty(assetId))
            {
                var error = "Enter an asset id from platform/API";
                _statusPanel.Show(error, StatusPanel.Types.Error);
                Debug.LogError(error);
            }
            else
            {
                Toolbox.Instance.GamerAssets.CreateAssetRewardForUser(assetIdInputField.text, OnCreateAssetRewardForUser);
                _statusPanel.Show("Creating asset reward for user", StatusPanel.Types.Error);
            }
        }

        private void OnCreateAssetRewardForUser(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenOwnedAssetsPanel();
                _statusPanel.Hide();
                Debug.Log("User rewarded");
            }
            else
            {
                _statusPanel.Show("Error creating asset reward for user", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            _masterPanel.OpenUser();
        }
    }
}
