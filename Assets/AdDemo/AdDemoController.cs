using System.Collections.Generic;
using Nefta.AdSdk;
using Nefta.Core;
using Nefta.Core.Events;
using UnityEngine;

namespace AdDemo
{
    public class AdDemoController : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private RectTransform _placementRect;

        [SerializeField] private ViewController _viewPrefab;
        [SerializeField] private InteractiveController _interactivePrefab;

        private NeftaAds _neftaAds;
        private bool _isBannerShown;
        private Dictionary<string, PlacementController> _placementControllers;
        
        private void Awake()
        {
            NeftaCore.EnableLogging(true);
            
            _neftaAds = NeftaAds.Init();
            _neftaAds.SetPublisherUserId("user1");
            _neftaAds.OnReady = OnReady;
            _neftaAds.OnBid = OnBid;
            _neftaAds.OnLoadStart = OnLoadStart;
            _neftaAds.OnLoadFail = OnLoadFail;
            _neftaAds.OnLoad = OnLoad;
            _neftaAds.OnShow = OnShow;
            _neftaAds.OnBannerChange = OnBannerChange;
            _neftaAds.OnClose = OnClose;
            _neftaAds.OnUserRewarded = OnUserRewarded;
            _neftaAds.Enable(true);
            
            _neftaAds.EnableBanner(true);

            NeftaCore.Instance.Record(new ReceiveEvent()
            {
                _resourceCategory = ResourceCategory.CosmeticItem,
                _method = ReceiveMethod.Shop,
                _name = "tesfdunit",
                _quantity = 3
            });

            AdjustOffsets(0);
        }

        private void Update()
        {
            if (_neftaAds != null)
            {
                _neftaAds.OnUpdate();
            }
        }
        
        private void OnReady(Dictionary<string, Placement> placements)
        {
            _placementControllers = new Dictionary<string, PlacementController>();
            foreach (var placement in placements)
            {
                PlacementController prefab = placement.Value._type == Placement.Type.Banner ? _viewPrefab : _interactivePrefab;
                var placementController = Instantiate(prefab, _placementRect);
                placementController.SetData(placement.Value);
                
                _placementControllers.Add(placement.Key, placementController);
            }
        }

        private void OnBid(Placement placement)
        {
            _placementControllers[placement._id].OnBid();
        }
        
        private void OnLoadStart(Placement placement)
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
            if (placement._type == Placement.Type.Banner)
            {
                AdjustOffsets(placement._renderedHeight);
            }
        }

        private void OnBannerChange(Placement placement)
        {
            _placementControllers[placement._id].OnBannerChange();
            AdjustOffsets(placement._renderedHeight);
        }

        private void OnClose(Placement placement)
        {
            _placementControllers[placement._id].OnClose();
            if (placement._type == Placement.Type.Banner)
            {
                AdjustOffsets(placement._renderedHeight);
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
    }
}
