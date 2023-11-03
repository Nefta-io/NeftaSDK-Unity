using System.Collections.Generic;
using Nefta.AdSdk;
using UnityEngine;
using UnityEngine.UI;

namespace AdDemo
{
    public class AdDemoController : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private RectTransform _placementRect;

        [SerializeField] private PlacementController _placementPrefab;

        private NeftaAds _neftaAds;
        private bool _isBannerShown;
        private Dictionary<string, PlacementController> _placementControllers;
        
        private void Awake()
        {
            _neftaAds = NeftaAds.Init();
            _neftaAds.SetPublisherUserId("user1");
            _neftaAds.OnReady = OnReady;
            _neftaAds.OnBid = OnBid;
            _neftaAds.OnStartLoad = OnStartLoad;
            _neftaAds.OnLoadFail = OnLoadFail;
            _neftaAds.OnLoad = OnLoad;
            _neftaAds.OnShow = OnShow;
            _neftaAds.OnClick = OnClick;
            _neftaAds.OnClose = OnClose;
            _neftaAds.OnUserRewarded = OnUserRewarded;
            if (_neftaAds.IsReady)
            {
                OnReady(_neftaAds.Placements);
            }
            _neftaAds.Enable(true);
            
            _neftaAds.SetPlacementMode(Placement.Type.Banner, Placement.Mode.Continuous);

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
                var placementController = Instantiate(_placementPrefab, _placementRect);
                placementController.SetData(placement.Value);
                
                _placementControllers.Add(placement.Key, placementController);
            }
        }

        private void OnBid(Placement.Type type, Placement placement)
        {
            _placementControllers[placement._id].OnBid();
        }
        
        private void OnStartLoad(Placement.Type type, Placement placement)
        {
            _placementControllers[placement._id].OnStartLoad();
        }
        
        private void OnLoadFail(Placement.Type type, Placement placement, string failReason)
        {
            _placementControllers[placement._id].OnLoadFail();
        }

        private void OnLoad(Placement.Type type, Placement placement)
        {
            _placementControllers[placement._id].OnLoad();
        }

        private void OnShow(Placement.Type type, Placement placement)
        {
            _placementControllers[placement._id].OnShow();
            if (placement._type == Placement.Type.Banner)
            {
                AdjustOffsets(placement._height);
            }
        }

        private void OnClick(Placement.Type type, Placement placement)
        {

        }

        private void OnClose(Placement.Type type, Placement placement)
        {
            _placementControllers[placement._id].OnClose();
            if (placement._type == Placement.Type.Banner)
            {
                AdjustOffsets(0);
            }
        }

        private void OnUserRewarded(Placement.Type type, Placement placement)
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
