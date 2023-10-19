using System.Globalization;
using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.GamerManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Assets
{
    public class CreateAuctionPanel : DemoPanel
    {
        [SerializeField] private InputField _priceInputField;
        [SerializeField] private InputField _purchaseInputField;
        [SerializeField] private Button _createAuctionButton;
        [SerializeField] private Button _closeButton;

        private string _ownershipId;

        protected override void OnInit()
        {
            _createAuctionButton.onClick.AddListener(CreateAuction);
            _closeButton.onClick.AddListener(OnClose);
        }

        public override void Open()
        {
            base.Open();
            
            _priceInputField.text = null;
            _purchaseInputField.text = null;
        }
        
        public void SetData(OwnedAsset item)
        {
            _ownershipId = item._ownershipId;
        }

        private void CreateAuction()
        {
            float.TryParse(_priceInputField.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var startPrice);
            float.TryParse(_purchaseInputField.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var purchasePrice);

            if (startPrice <= 0)
            {
                var error = "Enter auction price";
                _statusPanel.Show(error, StatusPanel.Types.Error);
            }
            else
            {
                Toolbox.Instance.Marketplace.CreateAuction(_ownershipId, startPrice, purchasePrice, OnCreateAuction);
                _statusPanel.Show("Creating auction", StatusPanel.Types.Loading);
            }
        }

        private void OnCreateAuction(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenMarketplace();
                Debug.Log("Auction created");
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error creating auction", StatusPanel.Types.Error);
            }
        }

        private void OnClose()
        {
            _masterPanel.OpenAuctions();
        }
    }
}
