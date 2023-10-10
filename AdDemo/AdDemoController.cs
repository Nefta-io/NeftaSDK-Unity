using System.Collections.Generic;
using Nefta.AdSdk;
using UnityEngine;
using UnityEngine.UI;

namespace Nefta.AdDemo
{
    public class AdDemoController : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private RectTransform _placementRect;
        [SerializeField] private Toggle _autoLoadToggle;

        [SerializeField] private PlacementController _placementPrefab;

        private NAds _nads;
        private bool _isBannerShown;
        private Dictionary<string, PlacementController> _placementControllers;
        
        private void Awake()
        {
            _nads = NAds.Init();
            _nads.SetPublisherUserId("user1");
            _nads.OnReady = OnReady;
            _nads.OnBid = OnBid;
            _nads.OnStartLoad = OnStartLoad;
            _nads.OnLoadFail = OnLoadFail;
            _nads.OnLoad = OnLoad;
            _nads.OnShow = OnShow;
            _nads.OnClick = OnClick;
            _nads.OnClose = OnClose;
            _nads.OnUserRewarded = OnUserRewarded;
            if (_nads.IsReady)
            {
                OnReady(_nads.Placements);
            }
            _nads.Enable(true);

            _autoLoadToggle.onValueChanged.AddListener(OnAutoLoadChange);
            _autoLoadToggle.isOn = true;

            AdjustOffsets(0);
        }

        private void Update()
        {
            if (_nads != null)
            {
                _nads.OnUpdate();
            }
        }

        private void OnReady(Dictionary<string, Placement> placements)
        {
            _placementControllers = new Dictionary<string, PlacementController>();
            foreach (var placement in placements)
            {
                var placementController = Instantiate(_placementPrefab, _placementRect);
                placementController.SetData(placement.Value, _autoLoadToggle.isOn);
                
                _placementControllers.Add(placement.Key, placementController);
            }
        }

        private void OnBid(Placement placement)
        {
            _placementControllers[placement._id].OnBid();
        }
        
        private void OnStartLoad(Placement placement)
        {
            _placementControllers[placement._id].OnStartLoad();
        }
        
        private void OnLoadFail(Placement placement, string failReason)
        {
            _placementControllers[placement._id].OnLoadFail();
        }

        private void OnLoad(Placement placement)
        {
            _placementControllers[placement._id].OnLoad();
        }

        private void OnShow(Placement placement)
        {
            _placementControllers[placement._id].OnShow();
            if (placement._type == Placement.ImpressionType.Banner)
            {
                AdjustOffsets(placement._height);
            }
        }

        private void OnClick(Placement placement)
        {

        }

        private void OnClose(Placement placement)
        {
            _placementControllers[placement._id].OnClose();
            if (placement._type == Placement.ImpressionType.Banner)
            {
                AdjustOffsets(0);
            }
        }

        private void OnUserRewarded(Placement placement)
        {
            Debug.Log($"OnUserRewarded for placement {placement._id}");
        }
        
        private void AdjustOffsets(int bannerHeight)
        {
            var topObstruction = Screen.height - Screen.safeArea.height - Screen.safeArea.y;
            _contentRect.offsetMax = new Vector2(0, -(topObstruction + bannerHeight) / Screen.height) * ((RectTransform)transform).rect.size.y;
        }

        private void OnAutoLoadChange(bool isEnabled)
        {
            if (_placementControllers == null)
            {
                return;
            }
            
            foreach (var placementController in _placementControllers)
            {
                placementController.Value.SetAutoLoad(isEnabled);
            }
        }
    }
}
