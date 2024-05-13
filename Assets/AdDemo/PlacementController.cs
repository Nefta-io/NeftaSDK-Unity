using Nefta.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class PlacementController : MonoBehaviour
    {
        [SerializeField] private Text _placementIdText;
        [SerializeField] private Text _placementTypeText;
        
        [SerializeField] private Text _availableBidText;
        [SerializeField] private Text _bufferBidText;
        [SerializeField] private Text _renderedBidText;
        
        protected Placement _placement;

        public virtual void SetData(Placement placement)
        {
            _placement = placement;

            _placementIdText.text = placement._id;
            _placementTypeText.text = placement._type.ToString();
            
            SyncUi();
        }

        public void OnBid()
        {
            SyncUi();
        }

        public void OnStartLoad()
        {
            SyncUi();
        }
        
        public void OnLoadFail()
        {
            SyncUi();
        }

        public void OnLoad()
        {
            SyncUi();
        }

        public void OnShow()
        {
            SyncUi();
        }

        public void OnBannerChange()
        {
            SyncUi();
        }

        public void OnClose()
        {
            SyncUi();
        }
        
        protected virtual void SyncUi()
        {
            string bid = "Available Bid:";
            if (_placement._availableBid != null)
            {
                bid += $"[available ({_placement._availableBid.Value})]";
            }
            _availableBidText.text = bid;
            
            bid = "Buffer Bid:";
            if (_placement._bufferBid != null)
            {
                bid += $"[loaded ({_placement._bufferBid.Value})]";
            }
            _bufferBidText.text = bid;
            
            bid = "Rendered Bid: ";
            if (_placement._renderedBid != null)
            {
                bid += $"[rendering ({_placement._renderedBid.Value})]";
            }
            _renderedBidText.text = bid;
        }
    }
}