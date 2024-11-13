using System;
using Nefta.Ads;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace AdDemo
{
    public class BannerController : PlacementController
    {
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _visibilityButton;
        [SerializeField] private Text _visibilityText;
        [SerializeField] private Button _closeButton;

        private Action<int> _adjustOffsets;
        
        public void SetData(AdUnit adUnit, Action<int> adjustOffsets)
        {
            _adjustOffsets = adjustOffsets;
            
            _createButton.onClick.AddListener(OnCreateClick);
            _visibilityButton.onClick.AddListener(OnVisibilityClick);
            _closeButton.onClick.AddListener(OnCloseClick);
            
            _visibilityButton.gameObject.SetActive(false);
            
            base.SetData(adUnit);
        }

        private void OnCreateClick()
        {
            NeftaAds.Instance.CreateBanner(AdUnit._id, NeftaAds.BannerPosition.Top, true);
            NeftaAds.Instance.Show(AdUnit._id);
        }

        private void OnCloseClick()
        {
            NeftaAds.Instance.Close(AdUnit._id);
        }
        
        public override void OnShow()
        {
            base.OnShow();
            _visibilityButton.gameObject.SetActive(true);
            _visibilityText.text = "Hide";
        }

        public override void OnClose()
        {
            base.OnClose();
            _visibilityButton.gameObject.SetActive(false);
        }
        
        private void OnVisibilityClick()
        {
            if (AdUnit._state == AdUnit.State.Showing)
            {
                NeftaAds.Instance.Hide(AdUnit._id);
                _visibilityText.text = "Show";
                _adjustOffsets(0);
            }
            else if (AdUnit._state == AdUnit.State.Hidden)
            {
                NeftaAds.Instance.Show(AdUnit._id);
                _visibilityText.text = "Hide";
                _adjustOffsets(AdUnit.Height);
            }
        }
    }
}