using System;
using Nefta;
using Nefta.Events;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Type = Nefta.Events.Type;

namespace AdDemo
{
    public class BannerController : MonoBehaviour, IPlacement
    {
        [SerializeField] private Text _placementIdText;
        [SerializeField] private Text _placementTypeText;
        
        [SerializeField] private Text _statusText;
        
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _visibilityButton;
        [SerializeField] private Text _visibilityText;
        [SerializeField] private Button _closeButton;

        private AdUnit _adUnit;
        private Action<int> _adjustOffsets;
        
        public void SetData(AdUnit adUnit, Action<int> adjustOffsets)
        {
            _adjustOffsets = adjustOffsets;
            
            _createButton.onClick.AddListener(OnCreateClick);
            _visibilityButton.onClick.AddListener(OnVisibilityClick);
            _closeButton.onClick.AddListener(OnCloseClick);
            
            _visibilityButton.gameObject.SetActive(false);
            
            _adUnit = adUnit;

            _placementIdText.text = adUnit._id;
            _placementTypeText.text = adUnit._type.ToString();

            _statusText.text = "";
        }

        private void OnCreateClick()
        {
            AddDemoGameEventsExample();
            
            NeftaAds.Instance.CreateBanner(_adUnit._id, NeftaAds.BannerPosition.Top, true);
            NeftaAds.Instance.Show(_adUnit._id);
        }

        private void OnCloseClick()
        {
            NeftaAds.Instance.Close(_adUnit._id);
        }
        
        public void OnBid()
        {
            _statusText.text = "OnBid";
        }
        
        public void OnLoadStart()
        {
            _statusText.text = "OnLoadStart";
        }
        
        public void OnLoadFail(string error)
        {
            _statusText.text = $"OnLoadFail: {error}";
        }
        
        public void OnLoad()
        {
            _statusText.text = "OnLoad";
        }
        
        public void OnShowFail(string error)
        {
            _statusText.text = $"OnShowFail {error}";
        }
        
        public void OnShow()
        {
            _statusText.text = "OnShow";
            
            _visibilityButton.gameObject.SetActive(true);
            _visibilityText.text = "Hide";
        }

        public void OnClose()
        {
            _statusText.text = "OnClose";
            
            _visibilityButton.gameObject.SetActive(false);
        }
        
        private void OnVisibilityClick()
        {
            if (_adUnit._state == AdUnit.State.Showing)
            {
                NeftaAds.Instance.Hide(_adUnit._id);
                _visibilityText.text = "Show";
                _adjustOffsets(0);
            }
            else if (_adUnit._state == AdUnit.State.Hidden)
            {
                NeftaAds.Instance.Show(_adUnit._id);
                _visibilityText.text = "Hide";
                _adjustOffsets(_adUnit.Height);
            }
        }

        private void AddDemoGameEventsExample()
        {
            new PurchaseEvent("unity_iap0", 24.99M, "USD").Record();
            new PurchaseEvent("unity_iap1", 2.99M, "EUR") { _customString = "cd ?" }.Record();
            
            var type = (Type) Random.Range(0, 7);
            var status = (Status)Random.Range(0, 3);
            var source = (Source)Random.Range(0, 7);
            int value = Random.Range(0, 101);
            new ProgressionEvent(type, status) { _source = source, _value = value, _name = $"progression_{type}_{status} {source} {value}" }.Record();
        }
    }
}