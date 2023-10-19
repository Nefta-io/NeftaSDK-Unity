using System;
using Nefta.ToolboxSdk.GamerManagement;
using Nefta.ToolboxSdk.Nft;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Assets
{
    public class OwnershipItem : MonoBehaviour
    {
        [SerializeReference] private NftImage _image;
        [SerializeReference] private Button _assetButton;
        [SerializeReference] private Button _auctionButton;
        
        private OwnedAsset _asset;
        private Action<OwnedAsset> _showAssetInfo;
        private Action<OwnedAsset> _showAuctionPanel;

        public void SetOwnedAsset(OwnedAsset asset, Action<OwnedAsset> showAssetInfo, Action<OwnedAsset> showAuctionPanel)
        {
            _asset = asset;
            
            _showAssetInfo = showAssetInfo;
            _showAuctionPanel = showAuctionPanel;
            
            _image.SetNft(asset._nft);
            
            _assetButton.onClick.AddListener(ShowAssetInfo);
            _auctionButton.onClick.AddListener(ShowAuctionPanel);
        }

        private void ShowAssetInfo()
        {
            _showAssetInfo(_asset);
        }

        private void ShowAuctionPanel()
        {
            _showAuctionPanel(_asset);
        }
    }
}
