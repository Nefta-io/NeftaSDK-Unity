using Nefta.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class ViewController : PlacementController
    {
        [SerializeField] private Button _toggleButton;
        [SerializeField] private GameObject _enableImage;

        private bool _isBannerEnabled;
        
        public override void SetData(Placement placement)
        {
            _toggleButton.onClick.AddListener(OnEnableBannerClick);
            
            base.SetData(placement);
        }
        
        private void OnEnableBannerClick()
        {
            NeftaAds.Instance.EnableBanner(_placement._id, !_isBannerEnabled);
        }

        protected override void SyncUi()
        {
            base.SyncUi();

            _isBannerEnabled = _placement._isShown;
            _enableImage.SetActive(_isBannerEnabled);
        }
    }
}