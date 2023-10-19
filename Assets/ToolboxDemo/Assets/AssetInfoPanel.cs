using System.Text;
using Nefta.ToolboxSdk.Nft;
using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Assets
{
    public class AssetInfoPanel : DemoPanel
    {
        [SerializeField] private NftImage _image;
        [SerializeField] private Text _idText;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _urlText;
        [SerializeField] private Text _typeText;
        [SerializeField] private Text _supplyText;
        [SerializeField] private Text _traitsText;
        [SerializeField] private Text _characteristicsText;
        
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _closeButton.onClick.AddListener(OnClose);
        }

        public void SetAsset(NftAsset asset)
        {
            _image.SetNft(asset);
            _idText.text = $"Id: {asset._id}";
            _nameText.text = $"Name: {asset._name}";
            _urlText.text = $"Url: {asset._imageUrl}";
            _typeText.text = $"Type: {asset._assetType}";
            _supplyText.text = $"Supply: {asset._instances - asset._availableInstances}/{asset._instances}";
            
            var text = new StringBuilder();
            text.Append("Traits:");
            if (asset._traits != null)
            {
                foreach (var trait in asset._traits)
                {
                    text.Append($"\n - {trait.Key}: {trait.Value}");
                }
            }
            _traitsText.text = text.ToString();

            text.Clear();
            text.Append("Characteristics:");
            if (asset._characteristics.nft_staking)
            {
                text.Append(" #stalking");
            }
            if (asset._characteristics.burnable)
            {
                text.Append(" #burnable");
            }
            if (asset._characteristics.rentable)
            {
                text.Append(" #rentable");
            }
            if (asset._characteristics.timed_assets)
            {
                text.Append(" #timed");
            }
            if (asset._characteristics.evolvable)
            {
                text.Append(" #evolvable");
            }
            _characteristicsText.text = text.ToString();
        }

        private void OnClose()
        {
            _masterPanel.OpenOwnedAssetsPanel();
        }
    }
}