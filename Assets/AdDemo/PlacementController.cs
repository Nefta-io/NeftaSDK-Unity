using Nefta;
using Nefta.Events;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class PlacementController : MonoBehaviour
    {
        [SerializeField] private Text _placementIdText;
        [SerializeField] private Text _placementTypeText;
        
        [SerializeField] private Text _statusText;
        protected AdUnit AdUnit;
        public virtual void SetData(AdUnit adUnit)
        {
            AdUnit = adUnit;

            _placementIdText.text = adUnit._id;
            _placementTypeText.text = adUnit._type.ToString();

            _statusText.text = "";
        }
        
        protected virtual void OnBidClick()
        {
            GameEvent gameEvent = null;
            int value = Random.Range(0, 101);
            if (AdUnit._type == AdUnit.Type.Banner)
            {
                var type = (Type) Random.Range(0, 7);
                var status = (Status)Random.Range(0, 3);
                var source = (Source)Random.Range(0, 7);
                gameEvent = new ProgressionEvent(type, status) { _source = source, _name = $"progression_{type}_{status} {source} {value}" };
            }
            else if (AdUnit._type == AdUnit.Type.Interstitial)
            {
                ResourceCategory category = (ResourceCategory) Random.Range(0, 9);
                ReceiveMethod method = (ReceiveMethod)Random.Range(0, 8);
                gameEvent = new ReceiveEvent(category) { _method = method, _name = $"receive_{category} {method} {value}" };
            }
            else
            {
                ResourceCategory category = (ResourceCategory) Random.Range(0, 9);
                SpendMethod method = (SpendMethod)Random.Range(0, 8);
                gameEvent = new SpendEvent(category) { _method = method, _name = $"spend_{category} {method} {value}" };
            }
            gameEvent._value = value;
            gameEvent.Record();
        }

        public virtual void OnBid()
        {
            _statusText.text = "OnBid";
        }

        public virtual void OnLoadStart()
        {
            _statusText.text = "OnLoadStart";
        }
        
        public virtual void OnLoadFail(string error)
        {
            _statusText.text = $"OnLoadFail: {error}";
        }

        public virtual void OnLoad()
        {
            _statusText.text = "OnLoad";
        }

        public virtual void OnShow()
        {
            _statusText.text = "OnShow";
        }
        
        public virtual void OnShowFail(string error)
        {
            _statusText.text = $"OnShowFail {error}";
        }

        public virtual void OnClose()
        {
            _statusText.text = "OnClose";
        }
    }
}