using Nefta.AdSdk;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class PlacementController : MonoBehaviour
    {
        [SerializeField] private Text _placementIdText;
        [SerializeField] private Text _placementTypeText;
        [SerializeField] private Text _availableBidText;
        [SerializeField] private Button _bidButton;
        [SerializeField] private Text _bidButtonText;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Text _loadButtonText;
        [SerializeField] private Text _bufferBidText;
        [SerializeField] private Button _showButton;
        [SerializeField] private Text _renderedBidText;
        [SerializeField] private Button _closeButton;

        private Placement _placement;
        private bool _autoLoad;

        public void SetData(Placement placement, bool autoLoad)
        {
            _placement = placement;

            _placementIdText.text = placement._id;
            _placementTypeText.text = placement._type.ToString();

            _bidButton.onClick.AddListener(OnBidClick);
            _loadButton.onClick.AddListener(OnLoadClick);
            _showButton.onClick.AddListener(OnShowClick);
            _closeButton.onClick.AddListener(OnCloseClick);

            SetAutoLoad(autoLoad);
            
            SyncUi();
        }

        public void SetAutoLoad(bool isEnabled)
        {
            _autoLoad = isEnabled;
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

        public void OnClose()
        {
            SyncUi();
        }
        
        private void SyncUi()
        {
            string bid = "Available Bid:";
            if (_placement._availableBid != null)
            {
                bid += $"loaded ({_placement._availableBid._price})";
            }
            _availableBidText.text = bid;
            _bidButtonText.text = _placement._isBidding ? "Bidding" : "Bid";
            _bidButton.interactable = !_placement._isBidding;
            _loadButtonText.text = _placement._isLoading ? "Loading" : "Load";
            _loadButton.interactable = _placement.CanLoad;

            bid = "Buffer Bid:";
            if (_placement._bufferBid != null)
            {
                bid += $"loaded ({_placement._bufferBid._price})";
            }
            _bufferBidText.text = bid;
            _showButton.interactable = _placement.CanShow;

            bid = "Rendered Bid: ";
            if (_placement._renderedBid != null)
            {
                bid += $"loaded ({_placement._renderedBid._price})";
            }
            _renderedBidText.text = bid;
            _closeButton.interactable = _placement._renderedBid != null;
        }
        
        private void OnBidClick()
        {
            NAds.Instance.Bid(_placement._id, _autoLoad);
        }

        private void OnLoadClick()
        {
            NAds.Instance.Load(_placement._id);
        }
        
        private void OnShowClick()
        {
            NAds.Instance.Show(_placement._id);
        }

        private void OnCloseClick()
        {
            NAds.Instance.Close(_placement._id);
        }
    }
}