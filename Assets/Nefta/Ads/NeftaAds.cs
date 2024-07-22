using System;
using System.Collections.Generic;

namespace Nefta.Ads
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
        
        private Adapter _adapter;
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
                Instance._adapter = Adapter.Init();
                Instance._callbackQueue = new Queue<Callback>();
                Instance.Placements = new Dictionary<string, Placement>();
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChange;
#endif

            return Instance;
        }

        public void Enable(bool enable)
        {
            _adapter.Plugin.RegisterListener(this);
            _adapter.Plugin.EnableAds(enable);
        }

        public void SetPublisherUserId(string publisherUserId)
        {
            _adapter.Plugin.SetPublisherUserId(publisherUserId);
        }

        public void EnableBanner(string placementId, bool enable)
        {
            if (Placements.TryGetValue(placementId, out var placement))
            {
                lock (_callbackQueue)
                {
                    placement._isShown = enable;
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnShow, placement));
                }
            }
            _adapter.Plugin.EnableBanner(placementId, enable);
        }

        public void SetPlacementPosition(string placementId, Placement.Position position)
        {
            _adapter.Plugin.SetPlacementPosition(placementId, (int) position);   
        }

        public void SetPlacementMode(string placementId, Placement.Mode mode)
        {
            _adapter.Plugin.SetPlacementMode(placementId, (int) mode);   
        }
        
        public void Bid(string placementId)
        {
            _adapter.Plugin.Bid(placementId);
        }

        public void Load(string placementId)
        {
            _adapter.Plugin.Load(placementId);
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
        
        public void Show(string placementId)
        {
            _adapter.Plugin.Show(placementId);
        }

        public void Close()
        {
            _adapter.Plugin.Close();
        }

        public void Close(string placementId)
        {
            _adapter.Plugin.Close(placementId);
        }

        public override void IOnReady(string configuration)
        {
            lock (_callbackQueue)
            {
                var index = configuration.IndexOf("ad_units\":", StringComparison.InvariantCulture) + 10;
                var adUnitsEnd = configuration.IndexOf("]", index, StringComparison.InvariantCulture);
                
                Placements.Clear();
                while (index < adUnitsEnd)
                {
                    int idStartIndex = configuration.IndexOf("\"id\":", index, StringComparison.InvariantCulture);
                    if (idStartIndex < 0)
                    {
                        break;
                    }
                    idStartIndex += 6;
                    int idLength = configuration.IndexOf("\"", idStartIndex, StringComparison.InvariantCulture) - idStartIndex;
                    var id = configuration.Substring(idStartIndex, idLength);
                    
                    int typeIndex = configuration.IndexOf("\"type\":", index, StringComparison.InvariantCulture) + 8;
                    Placement.Type adType;
                    if (configuration[typeIndex] == 'r')
                    {
                        adType = Placement.Type.VideoAd;
                    }
                    else if (configuration[typeIndex] == 'i')
                    {
                        adType = Placement.Type.Interstitial;
                    }
                    else
                    {
                        adType = Placement.Type.Banner;
                    }
                    Placements.Add(id, new Placement(adType, id));
                    
                    index = typeIndex + 1;
                }
                _callbackQueue.Enqueue(new Callback(Callback.Actions.OnReady, null));
            }
        }

        public override void IOnBid(string pId, float price)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._availableBid = price < 0 ? null : price;
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

        public override void IOnLoad(string pId, int width, int height)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._renderedWidth = width;
                    placement._renderedHeight = height;
                    placement._isLoading = false;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnLoad, placement));
                }
            }
        }

        public override void IOnShow(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._renderedBid = placement._bufferBid;
                    placement._bufferBid = null;
                    placement._isShown = true;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnShow, placement));
                }
            }
        }

        public override void IOnClick(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
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
                    placement._isShown = false;
                    
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