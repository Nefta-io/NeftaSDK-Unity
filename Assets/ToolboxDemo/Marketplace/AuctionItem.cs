using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.Marketplace;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Marketplace
{
    public class AuctionItem : MonoBehaviour
    {
        [SerializeField] private NftImage _image;
        [SerializeField] private Text _nameText;
        [SerializeField] private Button _bidButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private InputField _priceInputField;
        [SerializeField] private Text _purchasePriceText;
        [SerializeField] private Text _sellPriceText;
        [SerializeField] private Text _startPriceText;
        [SerializeField] private Text _numberOfBidsText;
        [SerializeField] private Text _lastBidPriceText;

        private Auction _auction;
        private MasterPanel _masterPanel;
        private StatusPanel _statusPanel;

        public void SetAuction(Auction auction, MasterPanel masterPanel, StatusPanel statusPanel)
        {
            _auction = auction;
            _masterPanel = masterPanel;
            _statusPanel = statusPanel;
            
            _bidButton.onClick.AddListener(Bid);
            _buyButton.onClick.AddListener(Buy);
            
            UpdateInfo();
        }

        private void Bid()
        {
            var price = float.Parse(_priceInputField.text);
            if (price > 0)
            {
                Toolbox.Instance.Marketplace.CreateAuctionBid(_auction, price, OnBid);
                _statusPanel.Show("Bidding", StatusPanel.Types.Loading);
            }
            else
            {
                var error = "Input correct price";
                Debug.LogError(error);
                _statusPanel.Show(error, StatusPanel.Types.Error);
            }
        }

        private void OnBid(bool isSuccess)
        {
            if (isSuccess)
            {
                Debug.Log("Bid success");
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error bidding", StatusPanel.Types.Error);
            }
        }

        private void Buy()
        {
            Toolbox.Instance.Marketplace.CreateAuctionPurchase(_auction, OnCreateAuctionPurchase);
            _statusPanel.Show("Creating auction purchase", StatusPanel.Types.Loading);
        }

        private void OnCreateAuctionPurchase(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenOwnedAssetsPanel();
                _statusPanel.Hide();
            }
            else
            {
                _statusPanel.Show("Error creating auction purchase", StatusPanel.Types.Error);
            }
        }

        private void UpdateInfo()
        {
            _image.SetNft(_auction._asset);
            _nameText.text = "item name: " + _auction._asset._name;
            _purchasePriceText.text = "purchase price: " + _auction._purchasePrice;
            _sellPriceText.text = "sell price: " + _auction._sellPrice;
            _startPriceText.text = "start price: " + _auction._startPrice;
            _numberOfBidsText.text = "number of bids: " + _auction._numberBids;
            _lastBidPriceText.text = "last bid price: " + _auction._lastBidPrice;

            _buyButton.gameObject.SetActive(_auction._purchasePrice > 0);
            _bidButton.gameObject.SetActive(_auction._startPrice > 0);
        }
    }
}
