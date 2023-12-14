using System;
using System.Collections.Generic;
using Nefta.Core;

namespace Nefta.AdSdk
{
    public class NeftaAds : NeftaPluginListener
    {
        private struct Callback
        {
            public enum Actions
            {
                OnReady,
                OnBid,
                OnLoadStart,
                OnLoadFail,
                OnLoad,
                OnShow,
                OnBannerChange,
                OnClick,
                OnReward,
                OnClose
            }
            
            public Actions _action;
            public Placement _placement;
            public string _data;

            public Callback(Actions action, Placement placement, string data=null)
            {
                _action = action;
                _placement = placement;
                _data = data;
            }
        }
        
        private NeftaCore _neftaCore;
        private Queue<Callback> _callbackQueue;
        public static NeftaAds Instance { get; private set; }

        public Dictionary<string, Placement> Placements { get; private set; }
        
        public Action<Dictionary<string, Placement>> OnReady;
        public Action<Placement> OnBid;
        public Action<Placement> OnLoadStart;
        public Action<Placement, string> OnLoadFail;
        public Action<Placement> OnLoad;
        public Action<Placement> OnShow;
        public Action<Placement> OnBannerChange;
        public Action<Placement> OnClick;
        public Action<Placement> OnClose;
        public Action<Placement> OnUserRewarded;
        
        public bool IsReady => Placements != null;

        public static NeftaAds Init()
        {
            if (Instance == null)
            {
                Instance = new NeftaAds();
                Instance._neftaCore = NeftaCore.Init();
                Instance._callbackQueue = new Queue<Callback>();
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChange;
#endif

            return Instance;
        }

        public void Enable(bool enable)
        {
            _neftaCore.Plugin.RegisterListener(this);
            _neftaCore.Plugin.EnableAds(enable);
        }

        public void SetPublisherUserId(string publisherUserId)
        {
            _neftaCore.Plugin.SetPublisherUserId(publisherUserId);
        }

        public void EnableBanner(bool enable)
        {
            _neftaCore.Plugin.EnableBanner(enable);
        }

        public void EnableBanner(string placementId, bool enable)
        {
            _neftaCore.Plugin.EnableBanner(placementId, enable);
        }
        
        public void SetPlacementMode(Placement.Type type, Placement.Mode mode)
        {
            _neftaCore.Plugin.SetPlacementMode((int) type, (int) mode);
        }

        public void SetPlacementMode(string placementId, Placement.Mode mode)
        {
            _neftaCore.Plugin.SetPlacementMode(placementId, (int) mode);   
        }

        public void Bid(Placement.Type type)
        {
            _neftaCore.Plugin.Bid((int)type);
        }
        
        public void Bid(string placementId)
        {
            _neftaCore.Plugin.Bid(placementId);
        }

        public void Load(string placementId)
        {
            _neftaCore.Plugin.Load(placementId);
        }

        public void Load(Placement.Type type)
        {
            _neftaCore.Plugin.Load((int)type);
        }

        public bool IsPlacementReady(Placement.Type type)
        {
            foreach (var placement in Placements)
            {
                if (placement.Value._type == type && placement.Value.CanShow)
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool IsPlacementReady(string placementId)
        {
            if (Placements.TryGetValue(placementId, out var placement))
            {
                return placement.CanShow;
            }

            return false;
        }

        public void Show(Placement.Type type)
        {
            _neftaCore.Plugin.Show((int)type);
        }
        
        public void Show(string placementId)
        {
            _neftaCore.Plugin.Show(placementId);
        }

        public void Close()
        {
            _neftaCore.Plugin.Close();
        }

        public void Close(string placementId)
        {
            _neftaCore.Plugin.Close(placementId);
        }

        public override void IOnReady(string configuration)
        {
            var responseJ = System.Text.Encoding.UTF8.GetBytes(configuration);
            var response = NeftaCore.Instance.Deserialize<Core.Data.InitResponse>(responseJ);
            var placements = new Dictionary<string, Placement>();
            foreach (var adUnit in response._adUnits)
            {
                var width = 0;
                var height = 0;
                var adType = Placement.Type.VideoAd;
                switch (adUnit._type)
                {
                    case "rewarded_video":
                        adType = Placement.Type.VideoAd;
                        break;
                    case "interstitial":
                        adType = Placement.Type.Interstitial;
                        width = adUnit._width ?? 320;
                        height = adUnit._height ?? 480;
                        break;
                    case "banner":
                        adType = Placement.Type.Banner;
                        width = adUnit._width ?? 320;
                        height = adUnit._height ?? 50;
                        break;
                }
                placements.Add(adUnit._id, new Placement(adType, adUnit._id, width, height));
            }

            lock (_callbackQueue)
            {
                Placements = placements;
                _callbackQueue.Enqueue(new Callback(Callback.Actions.OnReady, null));
            }
        }

        public override void IOnBid(string pId, float price)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._availableBid = price < 0 ? null : new BidResponse() { _price = price };
                    placement._isBidding = false;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnBid, placement));
                }
            }
        }

        public override void IOnLoadStart(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._bufferBid = placement._availableBid;
                    placement._availableBid = null;
                    placement._isLoading = true;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnLoadStart, placement));
                }
            }
        }
        
        public override void IOnLoadFail(string pId, string error)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._isLoading = false;
                    placement._bufferBid = null;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnLoadFail, placement, error));
                }
            }
        }

        public override void IOnLoad(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._isLoading = false;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnLoad, placement));
                }
            }
        }

        public override void IOnShow(string pId, int width, int height)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._renderedWidth = width;
                    placement._renderedHeight = height;
                    placement._renderedBid = placement._bufferBid;
                    placement._bufferBid = null;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnShow, placement));
                }
            }
        }

        public override void IOnBannerChange(string pId, int width, int height)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._renderedWidth = width;
                    placement._renderedHeight = height;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnBannerChange, placement));
                }
            }
        }

        public override void IOnClick(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._isLoading = false;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnClick, placement));
                }
            }
        }

        public override void IOnReward(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnReward, placement));
                }
            }
        }

        public override void IOnClose(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._renderedWidth = 0;
                    placement._renderedHeight = 0;
                    placement._renderedBid = null;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnClose, placement));
                }
            }
        }

        public void OnUpdate()
        {
            lock (_callbackQueue)
            {
                while (_callbackQueue.Count > 0)
                {
                    var callback = _callbackQueue.Dequeue();
                    switch (callback._action)
                    {
                        case Callback.Actions.OnReady:
                            OnReady?.Invoke(Placements);
                            break;
                        case Callback.Actions.OnBid:
                            OnBid?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnLoadStart:
                            OnLoadStart?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnLoadFail:
                            OnLoadFail?.Invoke(callback._placement, callback._data);
                            break;
                        case Callback.Actions.OnLoad:
                            OnLoad?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnShow:
                            OnShow?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnBannerChange:
                            OnBannerChange?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnClick:
                            OnClick?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnReward:
                            OnUserRewarded?.Invoke(callback._placement);
                            break;
                        case Callback.Actions.OnClose:
                            OnClose?.Invoke(callback._placement);
                            break;
                    }
                }
            }
        }
        
#if UNITY_EDITOR
        private static void OnPlayModeChange(UnityEditor.PlayModeStateChange playMode)
        {
            if (playMode == UnityEditor.PlayModeStateChange.EnteredEditMode)
            {
                Instance = null;
            }
        }
#endif
    }
}