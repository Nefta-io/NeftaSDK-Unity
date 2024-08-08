using Nefta.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class InteractiveController : PlacementController
    {
        [SerializeField] private Button _bidButton;

        [SerializeField] private Text _bidButtonText;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Text _loadButtonText;
        [SerializeField] private Button _showButton;
        [SerializeField] private Button _closeButton;

        public override void SetData(Placement placement)
        {
            _bidButton.onClick.AddListener(OnBidClick);
            _loadButton.onClick.AddListener(OnLoadClick);
            _showButton.onClick.AddListener(OnShowClick);
            _closeButton.onClick.AddListener(OnCloseClick);

            Debug.Log("test p: " + Nefta.Adapter.Instance.GetPartialBidRequest(placement._id));
            
            base.SetData(placement);
        }

        protected override void SyncUi()
        {
            base.SyncUi();
            
            _bidButtonText.text = _placement._isBidding ? "Bidding" : "Bid";
            _bidButton.interactable = !_placement._isBidding;
            _loadButtonText.text = _placement._isLoading ? "Loading" : "Load";
            _loadButton.interactable = _placement.CanLoad;
            
            _showButton.interactable = _placement.CanShow;
            _closeButton.interactable = _placement._renderedBid != null;
        }
        
        private void OnBidClick()
        {
            NeftaAds.Instance.Bid(_placement._id);
        }

        private void OnLoadClick()
        {
            NeftaAds.Instance.Load(_placement._id);
        }
        
        private void OnShowClick()
        {
            NeftaAds.Instance.Show(_placement._id);
        }

        private void OnCloseClick()
        {
            NeftaAds.Instance.Close(_placement._id);
        }
    }
}