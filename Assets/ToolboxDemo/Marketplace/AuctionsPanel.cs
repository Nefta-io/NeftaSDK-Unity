using System.Collections.Generic;
using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.Marketplace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ToolboxDemo.Marketplace
{
    public class AuctionsPanel : DemoPanel
    {
        [SerializeField] private RectTransform _content;
        [FormerlySerializedAs("auctionItemPrefab")] [SerializeField] private AuctionItem _auctionItemPrefab;
        [SerializeField] private Button _closeButton;
    
        private readonly List<AuctionItem> _auctionItems = new List<AuctionItem>();

        protected override void OnInit()
        {
            _closeButton.onClick.AddListener(OnClose);
        }

        public override void Open()
        {
            base.Open();
            
            foreach (var auction in _auctionItems)
            {
                Destroy(auction.gameObject);
            }
            _auctionItems.Clear();
            
            Toolbox.Instance.Marketplace.GetProjectAuctions(OnGetProjectAuctions);
            _statusPanel.Show("Getting project auctions", StatusPanel.Types.Loading);
        }
        
        private void OnGetProjectAuctions(AuctionsResponse auctions)
        {
            if (auctions == null)
            {
                _statusPanel.Show("Error getting project auctions", StatusPanel.Types.Error);
                return;
            }

            foreach (var auction in auctions._results)
            {
                AuctionItem auctionController = Instantiate(_auctionItemPrefab, _content);
                auctionController.SetAuction(auction, _masterPanel, _statusPanel);
                _auctionItems.Add(auctionController);
            }
            
            _statusPanel.Hide();
        }

        private void OnClose()
        {
            _masterPanel.OpenUser();
        }
    }
}
