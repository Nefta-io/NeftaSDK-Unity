using System;
using System.Collections.Generic;
using Nefta.Core;

namespace Nefta.AdSdk
{
    public class NAds
    {
        private NeftaCore _neftaCore;
        private bool _isBannerEnabled;
        public static NAds Instance { get; private set; }

        public Dictionary<string, Placement> Placements { get; private set; }
        
        public Action<Dictionary<string, Placement>> OnReady;
        public Action<Placement> OnBid;
        public Action<Placement> OnStartLoad;
        public Action<Placement, string> OnLoadFail;
        public Action<Placement> OnLoad;
        public Action<Placement> OnShow;
        public Action<Placement> OnClick;
        public Action<Placement> OnClose;
        public Action<Placement> OnUserRewarded;
        
        public bool IsReady => Placements != null;
        public Placement CurrentPlacement { get; private set; }

        public static NAds Init()
        {
            if (Instance == null)
            {
                Instance = new NAds();
                Instance._neftaCore = NeftaCore.Init();
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChange;
#endif
            
            return Instance;
        }

        public void Enable(bool enable)
        {
            _neftaCore.Plugin.EnableAds(enable);
        }

        public void SetPublisherUserId(string publisherUserId)
        {
            _neftaCore.Plugin.SetPublisherUserId(publisherUserId);
        }

        public void Bid(string placementId, bool autoLoad)
        {
            _neftaCore.Plugin.Bid(placementId, autoLoad);
        }

        public void Load(string placementId)
        {
            if (Placements.TryGetValue(placementId, out var placement))
            {
                if (placement.CanLoad)
                {
                    _neftaCore.Plugin.Load(placementId);
                }
                else
                {
                    Bid(placementId, true);
                }
            }
        }

        public void Load(Placement.ImpressionType type)
        {
            foreach (var placement in Placements)
            {
                if (placement.Value._type == type)
                {
                    Load(placement.Key);
                    return;
                }
            }
        }
        
        public bool IsPlacementReady(Placement.ImpressionType type)
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

        public void Show(string placementId)
        {
            if (Placements.TryGetValue(placementId, out var placement))
            {
                CurrentPlacement = placement;
                NeftaCore.Instance.Plugin.Show(placementId);
            }
        }

        public void Show(Placement.ImpressionType type)
        {
            foreach (var placement in Placements)
            {
                if (placement.Value._type == type && placement.Value.CanShow)
                {
                    Show(placement.Key);
                    return;
                }
            }
        }

        public void Close(string placementId)
        {
            NeftaCore.Instance.Plugin.Close(placementId);
        }

        public void Close(Placement.ImpressionType type)
        {
            if (CurrentPlacement != null && CurrentPlacement._type == type)
            {
                Close(CurrentPlacement._id);
            }
        }

        public void EnableBanner(bool enable)
        {
            _isBannerEnabled = enable;
            if (_isBannerEnabled)
            {
                if (!IsReady)
                {
                    return;
                }
                
                var isReady = IsPlacementReady(Placement.ImpressionType.Banner);
                if (isReady)
                {
                    Show(Placement.ImpressionType.Banner);
                }
                else
                {
                    foreach (var placement in Placements)
                    {
                        if (placement.Value._type == Placement.ImpressionType.Banner)
                        {
                            if (placement.Value._availableBid != null)
                            {
                                Load(placement.Key);
                            }
                            else
                            {
                                Bid(placement.Key, true);
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                if (CurrentPlacement != null && CurrentPlacement._type == Placement.ImpressionType.Banner)
                {
                    Close(Placement.ImpressionType.Banner);
                }
            }
        }
        
        public void IOnReady(Dictionary<string, Placement> placements)
        {
            Placements = placements;
            OnReady?.Invoke(Placements);
            if (_isBannerEnabled)
            {
                EnableBanner(true);
            }
        }

        public void IOnBid(Placement placement)
        {
            OnBid?.Invoke(placement);
        }

        public void IOnStartLoad(Placement placement)
        {
            OnStartLoad?.Invoke(placement);
        }

        public void IOnLoad(Placement placement)
        {
            OnLoad?.Invoke(placement);
            if (_isBannerEnabled && placement._type == Placement.ImpressionType.Banner)
            {
                Show(placement._id);
            }
        }

        public void IOnLoadFail(Placement placement, string error)
        {
            OnLoadFail?.Invoke(placement, error);
        }

        public void IOnShow(Placement placement)
        {
            OnShow?.Invoke(placement);
        }

        public void IOnClick(Placement placement)
        {
            OnClick?.Invoke(placement);
        }

        public void IOnUserRewarded(Placement placement)
        {
            OnUserRewarded?.Invoke(placement);
        }

        public void IOnClose(Placement placement)
        {
            OnClose?.Invoke(placement);
        }

        public void OnUpdate()
        {
            for (;;)
            {
                string message = _neftaCore.Plugin.CheckMessages();
                if (message == null)
                {
                    return;
                }

                NeftaCore.Log($"New message: {message}");
                string[] parameters = null;
                Placement placement;
                switch (message[0])
                {
                    case 'r':
                        var responseJ = System.Text.Encoding.UTF8.GetBytes(message);
                        var response = NeftaCore.Instance.Deserialize<Core.Data.InitResponse>(responseJ, 1);
                        Placements = new Dictionary<string, Placement>();
                        foreach (var adUnit in response._adUnits)
                        {
                            var width = 0;
                            var height = 0;
                            var adType = Placement.ImpressionType.VideoAd;
                            switch (adUnit._type)
                            {
                                case "rewarded_video":
                                    adType = Placement.ImpressionType.VideoAd;
                                    break;
                                case "interstitial":
                                    adType = Placement.ImpressionType.Interstitial;
                                    width = adUnit._width ?? 320;
                                    height = adUnit._height ?? 480;
                                    break;
                                case "banner":
                                    adType = Placement.ImpressionType.Banner;
                                    width = adUnit._width ?? 320;
                                    height = adUnit._height ?? 50;
                                    break;
                            }
                            Placements.Add(adUnit._id, new Placement(adType, adUnit._id, width, height));
                        }
                        OnReady?.Invoke(Placements);
                        break;
                    case 'b':
                        parameters = message.Substring(1).Split('|');
                        if (Placements.TryGetValue(parameters[0], out placement))
                        {
                            float price = float.Parse(parameters[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                            placement._availableBid = new BidResponse() { _price = price };
                            placement._isBidding = false;
                            OnBid?.Invoke(placement);
                        }
                        break;
                    case 'p':
                        if (Placements.TryGetValue(message.Substring(1), out placement))
                        {
                            placement._bufferBid = placement._availableBid;
                            placement._availableBid = null;
                            placement._isLoading = true;
                            OnStartLoad?.Invoke(placement);
                        }
                        break;
                    case 'e':
                        parameters = message.Substring(1).Split('|');
                        if (Placements.TryGetValue(parameters[0], out placement))
                        {
                            placement._isLoading = false;
                            placement._bufferBid = null;
                            OnLoadFail?.Invoke(placement, parameters[1]);
                        }
                        break;
                    case 'l':
                        if (Placements.TryGetValue(message.Substring(1), out placement))
                        {
                            placement._isLoading = false;
                            OnLoad?.Invoke(placement);
                        }
                        break;
                    case 's':
                        parameters = message.Substring(1).Split('|');
                        if (Placements.TryGetValue(parameters[0], out placement))
                        {
                            placement._renderedWidth = int.Parse(parameters[1], System.Globalization.NumberStyles.Integer);
                            placement._renderedHeight = int.Parse(parameters[2], System.Globalization.NumberStyles.Integer);
                            placement._renderedBid = placement._bufferBid;
                            placement._bufferBid = null;
                            OnShow?.Invoke(placement);
                        }
                        break;
                    case 'i':
                        if (Placements.TryGetValue(message.Substring(1), out placement))
                        {
                            OnClick?.Invoke(placement);
                        }
                        break;
                    case 'c':
                        if (Placements.TryGetValue(message.Substring(1), out placement))
                        {
                            placement._renderedBid = null;
                            OnClose?.Invoke(placement);
                        }
                        break;
                    case 'g':
                        if (Placements.TryGetValue(message.Substring(1), out placement))
                        {
                            OnUserRewarded?.Invoke(placement);
                        }
                        break;
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