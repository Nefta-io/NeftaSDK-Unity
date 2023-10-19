using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo.Marketplace
{
    public class MarketplacePanel : DemoPanel
    {
        [SerializeField] private Button _getAuctionsButton;
        [SerializeField] private Button _closeButton;

        protected override void OnInit()
        {
            _getAuctionsButton.onClick.AddListener(GetAuctions);
            _closeButton.onClick.AddListener(OnClose);
        }
        
        private void GetAuctions()
        {
            _masterPanel.OpenAuctions();
        }

        private void OnClose()
        {
            _masterPanel.OpenUser();
        }

    }
}
