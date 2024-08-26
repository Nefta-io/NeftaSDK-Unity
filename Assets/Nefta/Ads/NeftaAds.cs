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
                OnShow,
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
        
        private StringBuilder _eventBuilder;
        
        private Queue<Callback> _callbackQueue;
        public static NeftaAds Instance { get; private set; }

        public Dictionary<string, Placement> Placements { get; private set; }
        
        public Action<Dictionary<string, Placement>> OnReady;
        public Action<Placement> OnBid;
        public Action<Placement> OnLoadStart;
        public Action<Placement, string> OnLoadFail;
        public Action<Placement> OnLoad;
        public Action<Placement> OnShow;
        public Action<Placement> OnClick;
        public Action<Placement> OnClose;
        public Action<Placement> OnUserRewarded;
        
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
            Instance.Placements = new Dictionary<string, Placement>();
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
            Instance.PluginWrapper.Init(appId);
            Instance._eventBuilder = new StringBuilder(128);
            return Instance;
        }

        public void Enable(bool enable)
        {
            PluginWrapper.RegisterListener(this);
            PluginWrapper.EnableAds(enable);
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
            PluginWrapper.EnableBanner(placementId, enable);
        }

        public void SetPlacementPosition(string placementId, Placement.Position position)
        {
            PluginWrapper.SetPlacementPosition(placementId, (int) position);   
        }

        public void SetPlacementMode(string placementId, Placement.Mode mode)
        {
            PluginWrapper.SetPlacementMode(placementId, (int) mode);   
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
            PluginWrapper.Show(placementId);
        }

        public void Close()
        {
            PluginWrapper.Close();
        }

        public void Close(string placementId)
        {
            PluginWrapper.Close(placementId);
        }
        
        public void Record(GameEvent gameEvent)
        {
            _eventBuilder.Clear();
            _eventBuilder.Append("{\"event_type\":\"");
            _eventBuilder.Append(gameEvent._eventType);
            _eventBuilder.Append("\",\"event_category\":\"");
            _eventBuilder.Append(gameEvent._category);
            _eventBuilder.Append("\",\"value\":");
            _eventBuilder.Append(gameEvent._value.ToString());
            _eventBuilder.Append(",\"event_sub_category\":\"");
            _eventBuilder.Append(gameEvent._subCategory);
            if (gameEvent._name != null)
            {
                _eventBuilder.Append("\",\"item_name\":\"");
                _eventBuilder.Append(JavaScriptStringEncode(gameEvent._name));
            }
            if (gameEvent._customString != null)
            {
                _eventBuilder.Append("\",\"custom_publisher_payload\":\"");
                _eventBuilder.Append(JavaScriptStringEncode(gameEvent._customString));
            }
            _eventBuilder.Append("\"}");
            var eventString = _eventBuilder.ToString();
            PluginWrapper.Record(eventString);
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