using System;
using System.Collections.Generic;
using System.Text;
using Nefta.Data;
using Nefta.Events;
using UnityEngine;
using UnityEngine.Assertions;

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
        
        private Queue<Callback> _callbackQueue;
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

        public static NeftaAds Init()
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
            var configuration = Resources.Load<NeftaConfiguration>(NeftaConfiguration.FileName);
            Assert.IsNotNull(configuration, "Missing NeftaConfiguration ScriptableObject");
            var gameObject = new GameObject("_NeftaPlugin");
            Instance.PluginWrapper = gameObject.AddComponent<NeftaPluginWrapper>();
            NeftaPluginWrapper.EnableLogging(configuration._isLoggingEnabled);
#if UNITY_IOS
            var appId = configuration._iOSAppId;
#else
            var appId = configuration._androidAppId;
#endif
            Instance.PluginWrapper.Init(appId, Instance);
            return Instance;
        }

        public void Enable(bool enable)
        {
            PluginWrapper.EnableAds(enable);
        }

        public void CreateBanner(string placementId, BannerPosition position, bool autoRefresh)
        {
            PluginWrapper.CreateBanner(placementId, (int)position, autoRefresh);
        }
        
        public void SetCustomPublisherUserId(string userId)
        {
            PluginWrapper.SetPublisherUserId(userId);
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

        public void Close(string placementId)
        {
            PluginWrapper.Close(placementId);
        }
        
        public void Record(GameEvent gameEvent)
        {
            var name = gameEvent._name;
            if (name != null)
            {
                name = JavaScriptStringEncode(gameEvent._name);
            }
            var customPayload = gameEvent._customString;
            if (customPayload != null)
            {
                customPayload = JavaScriptStringEncode(gameEvent._customString);
            }
            PluginWrapper.Record(gameEvent._eventType, gameEvent._category, gameEvent._subCategory, name, gameEvent._value, customPayload);
        }
        
        private static string JavaScriptStringEncode(string value)
        {
            int len = value.Length;
            bool needEncode = false;
            char c;
            for (int i = 0; i < len; i++)
            {
                c = value [i];

                if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
                {
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
            {
                return value;
            }
            
            var sb = new StringBuilder ();
            for (int i = 0; i < len; i++)
            {
                c = value [i];
                if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                {
                    sb.AppendFormat ("\\u{0:x4}", (int)c);
                }
                else switch ((int)c)
                {
                    case 8:
                        sb.Append ("\\b");
                        break;

                    case 9:
                        sb.Append ("\\t");
                        break;

                    case 10:
                        sb.Append ("\\n");
                        break;

                    case 12:
                        sb.Append ("\\f");
                        break;

                    case 13:
                        sb.Append ("\\r");
                        break;

                    case 34:
                        sb.Append ("\\\"");
                        break;

                    case 92:
                        sb.Append ("\\\\");
                        break;

                    default:
                        sb.Append (c);
                        break;
                }
            }
            return sb.ToString ();
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
        
        public void OnUpdate()
        {
            lock (_callbackQueue)
            {
                while (_callbackQueue.Count > 0)
                {
                    var callback = _callbackQueue.Dequeue();
                    Debug.Log("got: " + callback._action);
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