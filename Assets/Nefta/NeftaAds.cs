using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Nefta
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
                OnShowFail,
                OnShow,
                OnClick,
                OnReward,
                OnClose
            }
            
            public Actions _action;
            public AdUnit AdUnit;
            public string _data;

            public Callback(Actions action, AdUnit adUnit, string data=null)
            {
                _action = action;
                AdUnit = adUnit;
                _data = data;
            }
        }

        public enum BannerPosition
        {
            None = 0,
            Top = 1,
            Bottom = 2
        }

        public enum ContentRating
        {
            Unspecified = 0,
            General = 1,
            ParentalGuidance = 2,
            Teen = 3,
            MatureAudience = 4
        }
        
        public delegate void OnInsightsCallback(Insights insights);
        
        private class InsightRequest
        {
            public int _id;
            public IEnumerable<string> _insights;
            public SynchronizationContext _returnContext;
            public OnInsightsCallback _callback;

            public InsightRequest(int id, OnInsightsCallback callback)
            {
                _id = id;
                _returnContext = SynchronizationContext.Current;
                _callback = callback;
            }
        }
        
        private Queue<Callback> _callbackQueue;
        private List<InsightRequest> _insightRequests;
        private static int _insightId;
        public static NeftaAds Instance { get; private set; }

        public Dictionary<string, AdUnit> Placements { get; private set; }
        
        public Action<Dictionary<string, AdUnit>> OnReady;
        public Action<AdUnit> OnBid;
        public Action<AdUnit> OnLoadStart;
        public Action<AdUnit, string> OnLoadFail;
        public Action<AdUnit> OnLoad;
        public Action<AdUnit, string> OnShowFail;
        public Action<AdUnit> OnShow;
        public Action<AdUnit> OnClick;
        public Action<AdUnit> OnClose;
        public Action<AdUnit> OnUserRewarded;
        
        public bool IsReady => Placements != null;
        
        public NeftaPluginWrapper PluginWrapper { get; private set; }

        public static NeftaAds Init(string appId)
        {
            if (Instance != null)
            {
                return Instance;
            }
            
            Instance = new NeftaAds();
            Instance._callbackQueue = new Queue<Callback>();
            Instance.Placements = new Dictionary<string, AdUnit>();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChange;
#endif
            var gameObject = new GameObject("_NeftaPlugin");
            Instance.PluginWrapper = gameObject.AddComponent<NeftaPluginWrapper>();
            Instance.PluginWrapper.Init(appId, Instance);
            Instance._insightRequests = new List<InsightRequest>();
            return Instance;
        }
        
        public void GetInsights(int insights, OnInsightsCallback callback, int timeoutInSeconds=0)
        {
            var id = 0;
            lock (_insightRequests)
            {
                id = _insightId;
                var request = new InsightRequest(id, callback);
                _insightRequests.Add(request);
                _insightId++;
            }
            
            PluginWrapper.GetInsights(id, insights, timeoutInSeconds);
        }

        public void CreateBanner(string placementId, BannerPosition position, bool autoRefresh)
        {
            PluginWrapper.CreateBanner(placementId, (int)position, autoRefresh);
        }
        
        public void SetCustomPublisherUserId(string userId)
        {
            PluginWrapper.SetPublisherUserId(userId);
        }

        public void SetContentRating(ContentRating rating)
        {
            var r = "";
            switch (rating)
            {
                case ContentRating.General:
                    r = "G";
                    break;
                case ContentRating.ParentalGuidance:
                    r = "PG";
                    break;
                case ContentRating.Teen:
                    r = "T";
                    break;
                case ContentRating.MatureAudience:
                    r = "MA";
                    break;
            }
            PluginWrapper.SetContentRating(r);
        }

        public void SetFloorPrice(string placementId, float floorPrice)
        {
            PluginWrapper.SetFloorPrice(placementId, floorPrice);
        }

        public void Bid(string placementId)
        {
            PluginWrapper.Bid(placementId);
        }
        
        public string GetPartialBidRequest(string placementId)
        {
            return PluginWrapper.GetPartialBidRequest(placementId);
        }

        public void Load(string placementId)
        {
            PluginWrapper.Load(placementId);
        }

        public void LoadWithBidResponse(string placementId, string bidResponse)
        {
            PluginWrapper.LoadWithBidResponse(placementId, bidResponse);
        }
        
        public bool CanShow(string placementId)
        {
            if (Placements.TryGetValue(placementId, out var placement))
            {
                return placement.CanShow;
            }

            return false;
        }
        
        public void Show(string placementId)
        {
            if (Placements.TryGetValue(placementId, out var adUnit))
            {
                if (adUnit._state == AdUnit.State.Hidden)
                {
                    adUnit._state = AdUnit.State.Showing;
                }
            }
            PluginWrapper.Show(placementId);
        }

        public void Hide(string placementId)
        {
            if (Placements.TryGetValue(placementId, out var adUnit))
            {
                if (adUnit._state == AdUnit.State.Showing)
                {
                    adUnit._state = AdUnit.State.Hidden;
                    PluginWrapper.Hide(placementId);
                }
            }
        }

        public void Mute(string placementId, bool mute)
        {
            PluginWrapper.Mute(placementId, mute);
        }

        public void Close(string placementId)
        {
            PluginWrapper.Close(placementId);
        }

        public string GetNuid(bool withGui)
        {
            return PluginWrapper.GetNuid(withGui);
        }
        
        public void SetOverride(string overrideUrl)
        {
            PluginWrapper.SetOverride(overrideUrl);    
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
                    int idIndex = configuration.IndexOf("\"id\":", index, StringComparison.InvariantCulture);
                    if (idIndex < 0)
                    {
                        break;
                    }
                    idIndex += 5;
                    idIndex = configuration.IndexOf("\"", idIndex, StringComparison.InvariantCulture) + 1;
                    int idLength = configuration.IndexOf("\"", idIndex, StringComparison.InvariantCulture) - idIndex;
                    var id = configuration.Substring(idIndex, idLength);
                    
                    int typeIndex = configuration.IndexOf("\"type\":", index, StringComparison.InvariantCulture) + 7;
                    typeIndex = configuration.IndexOf("\"", typeIndex, StringComparison.InvariantCulture) + 1;
                    AdUnit.Type adType;
                    if (configuration[typeIndex] == 'r')
                    {
                        adType = AdUnit.Type.VideoAd;
                    }
                    else if (configuration[typeIndex] == 'i')
                    {
                        adType = AdUnit.Type.Interstitial;
                    }
                    else
                    {
                        adType = AdUnit.Type.Banner;
                    }
                    
                    Placements.Add(id, new AdUnit(adType, id));
                    
                    index = typeIndex + 1;
                }
                _callbackQueue.Enqueue(new Callback(Callback.Actions.OnReady, null));
            }
        }

        public override void IOnBid(string pId, float price, int expirationTime)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._bid = price;
                    placement._state = price < 0 ? AdUnit.State.Initialized : AdUnit.State.ReadyToLoad;
                    placement._expirationTime = expirationTime;
                    placement._auctionTime = Time.realtimeSinceStartup;
                    
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
                    if (placement._state < AdUnit.State.Ready)
                    {
                        placement._state = AdUnit.State.Loading;
                        _callbackQueue.Enqueue(new Callback(Callback.Actions.OnLoadStart, placement));
                    }
                }
            }
        }
        
        public override void IOnLoadFail(string pId, int code, string error)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._state = AdUnit.State.Expired;
                    placement._bid = -1;
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
                    placement._state = AdUnit.State.Ready;
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnLoad, placement));
                }
            }
        }
        
        public override void IOnShowFail(string pId, int code, string error)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._state = AdUnit.State.Expired;
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnShowFail, placement, error));
                }
            }
        }

        public override void IOnShow(string pId)
        {
            lock (_callbackQueue)
            {
                if (Placements.TryGetValue(pId, out var placement))
                {
                    placement._state = AdUnit.State.Showing;
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
                    placement._bid = -1;
                    placement._state = AdUnit.State.Initialized;
                    
                    _callbackQueue.Enqueue(new Callback(Callback.Actions.OnClose, placement));
                }
            }
        }

        public override void IOnInsights(int id, string bi)
        {
            var insights = new Insights(JsonUtility.FromJson<InsightsDto>(bi));
            try
            {
                lock (_insightRequests)
                {
                    for (var i = _insightRequests.Count - 1; i >= 0; i--)
                    {
                        var insightRequest = _insightRequests[i];
                        if (insightRequest._id == id)
                        {
                            insightRequest._returnContext.Post(_ => insightRequest._callback(insights), null);
                            _insightRequests.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
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
                            OnBid?.Invoke(callback.AdUnit);
                            break;
                        case Callback.Actions.OnLoadStart:
                            OnLoadStart?.Invoke(callback.AdUnit);
                            break;
                        case Callback.Actions.OnLoadFail:
                            OnLoadFail?.Invoke(callback.AdUnit, callback._data);
                            break;
                        case Callback.Actions.OnLoad:
                            OnLoad?.Invoke(callback.AdUnit);
                            break;
                        case Callback.Actions.OnShowFail:
                            OnShowFail?.Invoke(callback.AdUnit, callback._data);
                            break;
                        case Callback.Actions.OnShow:
                            OnShow?.Invoke(callback.AdUnit);
                            break;
                        case Callback.Actions.OnClick:
                            OnClick?.Invoke(callback.AdUnit);
                            break;
                        case Callback.Actions.OnReward:
                            OnUserRewarded?.Invoke(callback.AdUnit);
                            break;
                        case Callback.Actions.OnClose:
                            OnClose?.Invoke(callback.AdUnit);
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