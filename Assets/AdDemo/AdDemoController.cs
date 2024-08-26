using System.Collections.Generic;
using Nefta.Ads;
using Nefta.Events;
using UnityEngine;

namespace AdDemo
{
    public class AdDemoController : MonoBehaviour
    {
        private const string BannerAdUnitId = "5726295757422592";
        
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private RectTransform _placementRect;

        [SerializeField] private ViewController _viewPrefab;
        [SerializeField] private InteractiveController _interactivePrefab;

        private NeftaAds _neftaAds;
        private bool _isBannerShown;
        private Dictionary<string, PlacementController> _placementControllers;
        private DebugServer _debugServer;
        
        private void Awake()
        {
            _neftaAds = NeftaAds.Init();
            var debugParams = GetDebugParameters();
            if (debugParams != null)
            {
                _neftaAds.PluginWrapper.SetOverride(debugParams[0]);
                
                _debugServer = new DebugServer();
                _debugServer.Init(debugParams[2], debugParams[1]);
            }
            _neftaAds.OnReady = OnReady;
            _neftaAds.OnBid = OnBid;
            _neftaAds.OnLoadStart = OnLoadStart;
            _neftaAds.OnLoadFail = OnLoadFail;
            _neftaAds.OnLoad = OnLoad;
            _neftaAds.OnShow = OnShow;
            _neftaAds.OnClose = OnClose;
            _neftaAds.OnUserRewarded = OnUserRewarded;
            _neftaAds.Enable(true);
            
            _neftaAds.EnableBanner(BannerAdUnitId, true);
            
            _neftaAds.SetFloorPrice("3434234238554", 0.42f);
            
            _neftaAds.Record(new ProgressionEvent(Type.Task, Status.Fail) { _name = "hard boss"});
            _neftaAds.Record(new ReceiveEvent(ResourceCategory.Experience) { _method = ReceiveMethod.Create, _value = 123, _name = "abc"});
            
            AdjustOffsets(0);
        }

        private void Update()
        {
            if (_debugServer != null)
            {
                _debugServer.OnUpdate();
            }
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
                AdjustOffsets(placement.Height);
            }
        }

        private void OnClose(Placement placement)
        {
            _placementControllers[placement._id].OnClose();
            if (placement._type == Placement.Type.Banner)
            {
                AdjustOffsets(placement.Height);
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

        private string[] GetDebugParameters()
        {
            string root = null;
            string dmIp = null;
            string serial = null;
#if UNITY_EDITOR
            root = "localhost";
            dmIp = "localhost";
            serial = "sim4";
#elif UNITY_IOS
            string[] args = System.Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                root = args[1];
            }
            if (args.Length > 2)
            {
                dmIp = args[2];
            }
            if (args.Length > 3)
            {
                serial = args[3];
            }
#elif UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            root = intent.Call<string>("getStringExtra", "override");
            dmIp = intent.Call<string>("getStringExtra", "dmIp");
            serial = intent.Call<string>("getStringExtra", "serial");
#endif
            if (!string.IsNullOrEmpty(root))
            {
                return new []{ root, dmIp, serial };
            }

            return null;
        }
    }
}
