using Nefta;
using Nefta.Events;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class InteractiveController : MonoBehaviour, IPlacement
    {
        [SerializeField] private Button _bidButton;

        [SerializeField] private Text _bidButtonText;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Text _loadButtonText;
        [SerializeField] private Button _showButton;
        [SerializeField] private Button _closeButton;
        
        [SerializeField] private Text _placementIdText;
        [SerializeField] private Text _placementTypeText;
        
        [SerializeField] private Text _statusText;
        
        private AdUnit AdUnit;

        public void SetData(AdUnit adUnit)
        {
            _bidButton.onClick.AddListener(OnBidClick);
            _loadButton.onClick.AddListener(OnLoadClick);
            _showButton.onClick.AddListener(OnShowClick);
            _closeButton.onClick.AddListener(OnCloseClick);
            
            AdUnit = adUnit;

            _placementIdText.text = adUnit._id;
            _placementTypeText.text = adUnit._type.ToString();

            _statusText.text = "";
            
            SyncUi();
        }
        
        private void OnBidClick()
        {
            NeftaAds.Instance.Bid(AdUnit._id);
            
            SyncUi();

            AddDemoGameEventExample();
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

        private void SyncUi()
        {
            _bidButtonText.text = AdUnit._state == AdUnit.State.Bidding ? "Bidding" : "Bid";
            _loadButtonText.text = AdUnit._state == AdUnit.State.Loading ? "Loading" : "Load";

            _bidButton.interactable = AdUnit._state == AdUnit.State.Initialized;
            _loadButton.interactable = AdUnit.CanLoad;
            _showButton.interactable = AdUnit.CanShow;
            _closeButton.interactable = AdUnit._state != AdUnit.State.Initialized;

        }

        public void OnBid()
        {
            _statusText.text = "OnBid";
            
            SyncUi();
        }

        public void OnLoadStart()
        {
            _statusText.text = "OnLoadStart";
            
            SyncUi();
        }
        
        public void OnLoadFail(string error)
        {
            _statusText.text = $"OnLoadFail: {error}";
            
            SyncUi();
        }
        
        public void OnLoad()
        {
            _statusText.text = "OnLoad";
            
            SyncUi();
        }
        
        public void OnShowFail(string error)
        {
            _statusText.text = $"OnShowFail {error}";
            
            SyncUi();
        }

        public void OnShow()
        {
            _statusText.text = "OnShow";
            
            SyncUi();
        }

        public void OnClose()
        {
            _statusText.text = "OnClose";
            
            SyncUi();
        }

        private void AddDemoGameEventExample()
        {
            int value = Random.Range(0, 101);
            if (AdUnit._type == AdUnit.Type.Interstitial)
            {
                ResourceCategory category = (ResourceCategory) Random.Range(0, 9);
                ReceiveMethod method = (ReceiveMethod)Random.Range(0, 8);
                new ReceiveEvent(category) { _method = method, _value = value, _name = $"receive_{category} {method} {value}" }.Record();
            }
            else
            {
                ResourceCategory category = (ResourceCategory) Random.Range(0, 9);
                SpendMethod method = (SpendMethod)Random.Range(0, 8);
                new SpendEvent(category) { _method = method, _value = value, _name = $"spend_{category} {method} {value}" }.Record();
            }
        }
    }
}