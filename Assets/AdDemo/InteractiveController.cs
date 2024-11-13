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

        public override void SetData(AdUnit adUnit)
        {
            _bidButton.onClick.AddListener(OnBidClick);
            _loadButton.onClick.AddListener(OnLoadClick);
            _showButton.onClick.AddListener(OnShowClick);
            _closeButton.onClick.AddListener(OnCloseClick);
            
            base.SetData(adUnit);
            SyncUi();
        }

        private void SyncUi()
        {
            _bidButtonText.text = AdUnit._state == AdUnit.State.Bidding ? "Bidding" : "Bid";
            _loadButtonText.text = AdUnit._state == AdUnit.State.Loading ? "Loading" : "Load";

            _loadButton.interactable = AdUnit.CanLoad;
            _showButton.interactable = AdUnit.CanShow;
            _closeButton.interactable = AdUnit._state == AdUnit.State.Showing;
            
        }

        public override void OnBid()
        {
            base.OnBid();
            SyncUi();
        }

        public override void OnLoadStart()
        {
            base.OnLoadStart();
            SyncUi();
        }
        
        public override void OnLoadFail(string error)
        {
            base.OnLoadFail(error);
            SyncUi();
        }
        
        public override void OnLoad()
        {
            base.OnLoad();
            SyncUi();
        }
        
        public override void OnShowFail(string error)
        {
            base.OnShowFail(error);
            SyncUi();
        }

        public override void OnShow()
        {
            base.OnShow();
            SyncUi();
        }

        public override void OnClose()
        {
            base.OnClose();
            SyncUi();
        }
        
        protected override void OnBidClick()
        {
            base.OnBidClick();
            
            NeftaAds.Instance.Bid(AdUnit._id);
            SyncUi();
        }

        private void OnLoadClick()
        {
            NeftaAds.Instance.Load(AdUnit._id);
        }
        
        private void OnShowClick()
        {
            NeftaAds.Instance.Show(AdUnit._id);
        }

        private void OnCloseClick()
        {
            NeftaAds.Instance.Close(AdUnit._id);
        }
    }
}