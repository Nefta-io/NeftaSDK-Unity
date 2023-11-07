using System;
using System.Collections.Generic;
using Nefta.Core;

namespace Nefta.AdSdk
{
    public class NeftaAds
    {
        private NeftaCore _neftaCore;
        private bool _isBannerEnabled;
        public static NeftaAds Instance { get; private set; }

        public Dictionary<string, Placement> Placements { get; private set; }
        
        public Action<Dictionary<string, Placement>> OnReady;
        public Action<Placement.Type, Placement> OnBid;
        public Action<Placement.Type, Placement> OnStartLoad;
        public Action<Placement.Type, Placement, string> OnLoadFail;
        public Action<Placement.Type, Placement> OnLoad;
        public Action<Placement.Type, Placement> OnShow;
        public Action<Placement.Type, Placement> OnClick;
        public Action<Placement.Type, Placement> OnClose;
        public Action<Placement.Type, Placement> OnUserRewarded;
        
        public bool IsReady => Placements != null;

        public static NeftaAds Init()
        {
            if (Instance == null)
            {
                Instance = new NeftaAds();
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

        public void IOnReady(Dictionary<string, Placement> placements)
        {
            Placements = placements;
            OnReady?.Invoke(Placements);
        }

        public void IOnBid(Placement.Type type, Placement placement)
        {
            OnBid?.Invoke(type, placement);
        }

        public void IOnStartLoad(Placement.Type type, Placement placement)
        {
            OnStartLoad?.Invoke(type, placement);
        }

        public void IOnLoad(Placement.Type type, Placement placement)
        {
            OnLoad?.Invoke(type, placement);
        }

        public void IOnLoadFail(Placement.Type type, Placement placement, string error)
        {
            OnLoadFail?.Invoke(type, placement, error);
        }

        public void IOnShow(Placement.Type type, Placement placement)
        {
            OnShow?.Invoke(type, placement);
        }

        public void IOnClick(Placement.Type type, Placement placement)
        {
            OnClick?.Invoke(type, placement);
        }

        public void IOnUserRewarded(Placement.Type type, Placement placement)
        {
            OnUserRewarded?.Invoke(type, placement);
        }

        public void IOnClose(Placement.Type type, Placement placement)
        {
            OnClose?.Invoke(type, placement);
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
                
                string[] parameters;
                Placement.Type type;
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
                            Placements.Add(adUnit._id, new Placement(adType, adUnit._id, width, height));
                        }
                        OnReady?.Invoke(Placements);
                        break;
                    case 'b':
                        type = (Placement.Type)(message[1] - '0');
                        float price = 0;
                        parameters = message.Substring(2).Split('|');
                        if (parameters.Length >= 2)
                        {
                            price = float.Parse(parameters[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        
                        if (Placements.TryGetValue(parameters[0], out placement))
                        {
                            placement._availableBid = new BidResponse() { _price = price };
                            placement._isBidding = false;
                        }

                        IOnBid(type, placement);
                        break;
                    case 'p':
                        type = (Placement.Type)(message[1] - '0');
                        if (Placements.TryGetValue(message.Substring(2), out placement))
                        {
                            placement._bufferBid = placement._availableBid;
                            placement._availableBid = null;
                            placement._isLoading = true;
                        }
                        IOnStartLoad(type, placement);
                        break;
                    case 'e':
                        type = (Placement.Type)(message[1] - '0');
                        parameters = message.Substring(2).Split('|');
                        if (Placements.TryGetValue(parameters[0], out placement))
                        {
                            placement._isLoading = false;
                            placement._bufferBid = null;
                        }
                        IOnLoadFail(type, placement, parameters[1]);
                        break;
                    case 'l':
                        type = (Placement.Type)(message[1] - '0');
                        if (Placements.TryGetValue(message.Substring(2), out placement))
                        {
                            placement._isLoading = false;
                        }
                        IOnLoad(type, placement);
                        break;
                    case 's':
                        type = (Placement.Type)(message[1] - '0');
                        parameters = message.Substring(2).Split('|');
                        if (Placements.TryGetValue(parameters[0], out placement))
                        {
                            placement._renderedWidth = int.Parse(parameters[1], System.Globalization.NumberStyles.Integer);
                            placement._renderedHeight = int.Parse(parameters[2], System.Globalization.NumberStyles.Integer);
                            placement._renderedBid = placement._bufferBid;
                            placement._bufferBid = null;
                        }
                        IOnShow(type, placement);
                        break;
                    case 'i':
                        type = (Placement.Type)(message[1] - '0');
                        Placements.TryGetValue(message.Substring(2), out placement);
                        IOnClick(type, placement);
                        break;
                    case 'c':
                        type = (Placement.Type)(message[1] - '0');
                        if (Placements.TryGetValue(message.Substring(2), out placement))
                        {
                            placement._renderedBid = null;
                        }
                        IOnClose(type, placement);
                        break;
                    case 'g':
                        type = (Placement.Type)(message[1] - '0');
                        Placements.TryGetValue(message.Substring(1), out placement);
                        IOnUserRewarded(type, placement);
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