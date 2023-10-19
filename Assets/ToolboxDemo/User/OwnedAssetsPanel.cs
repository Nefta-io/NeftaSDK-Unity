using System.Collections.Generic;
using ToolboxDemo.Assets;
using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.GamerManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.User
{
    public class OwnedAssetsPanel : DemoPanel
    {
        [SerializeField] private OwnershipItem _ownershipItemPrefab;
        
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button _closeButton;
    
        private readonly List<OwnershipItem> _loadedAssets = new List<OwnershipItem>();

        protected override void OnInit()
        {
            _closeButton.onClick.AddListener(OnClose);
        }
        
        public override void Open()
        {
            base.Open();
            
            foreach (var asset in _loadedAssets)
            {
                Destroy(asset.gameObject);                
            }
            _loadedAssets.Clear();
        
            LoadAssets();
        }

        private void LoadAssets()
        {
            Toolbox.Instance.GamerManagement.GetGamerOwnedAssets(OnAssetsLoaded);
            _statusPanel.Show("Get gamer owned assets", StatusPanel.Types.Loading);
        }
        
        private void OnAssetsLoaded(OwnedAssetsResponse assetsResponse) {
            if (assetsResponse == null)
            {
                _statusPanel.Show("Error getting owned assets", StatusPanel.Types.Error);
                return;
            }
            foreach (var asset in assetsResponse._results)
            {
                var assetInstance = Instantiate(_ownershipItemPrefab, _content);
                assetInstance.SetOwnedAsset(asset, ShowAssetInfo, ShowAuctionPanel);
                _loadedAssets.Add(assetInstance);
            }
            _statusPanel.Hide();
        }

        private void ShowAssetInfo(OwnedAsset asset)
        {
            _masterPanel.OpenAssetInfo(asset._nft);
        }
        
        private void ShowAuctionPanel(OwnedAsset asset)
        {
            _masterPanel.OpenCreateAuction(asset);
        }

        private void OnClose()
        {
            _masterPanel.OpenUserDetails();
        }
    }
}
