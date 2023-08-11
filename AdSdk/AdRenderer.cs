using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Nefta.AdSdk.Data.Vast;
using Nefta.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace Nefta.AdSdk
{
    internal class AdRenderer : MonoBehaviour
    {
        private const string AdClickMessageName = "click";
        
        [SerializeField] private GameObject _renderer;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private GameObject _background;
        [SerializeField] private RectTransform _bannerRect;
        [SerializeField] private RectTransform _closeRect;
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private Camera _camera;

        private AdManager _adManager;
        
        private ScreenOrientation _currentOrientation;
        private AdPlacement _currentPlacement;
        private Dictionary<AdPlacement, WebViewObject> _placementToWebView;
        private Queue<WebViewObject> _availableWebViews;
        private WebViewObject _firstWebView;
        
        private AdPlacement _nextPlacement;
        private bool _isCloseTriggered;

        public void Init(AdManager adManager)
        {
            _adManager = adManager;

#if UNITY_EDITOR
            transform.name = "_NeftaAdRenderer";
#endif
            _currentOrientation = Screen.orientation;

            _placementToWebView = new Dictionary<AdPlacement, WebViewObject>();
            _availableWebViews = new Queue<WebViewObject>();
            
            SetRendererFor(null);

            Application.runInBackground = false;
        }

        public void LoadAd(AdPlacement adPlacement)
        {
            NeftaCore.Log($"LoadAd {adPlacement._type} ({adPlacement._id})");
            
            switch (adPlacement._type)
            {
                case ImpressionType.Banner:
                case ImpressionType.Interstitial:
                    var scaleFactor = Screen.dpi / 160f;
                    if (adPlacement._type == ImpressionType.Interstitial)
                    {
                        if (adPlacement._bid._width > adPlacement._bid._height)
                        {
                            scaleFactor = Screen.safeArea.width / adPlacement._bid._width;
                        }
                        else
                        {
                            scaleFactor = Screen.safeArea.height / adPlacement._bid._height;
                        }
                    }
                    adPlacement._screenWidth = (int)(adPlacement._bid._width * scaleFactor);
                    adPlacement._screenHeight = (int)(adPlacement._bid._height * scaleFactor);
                    var html = $"<html><head><meta name=\"viewport\" content=\"width={adPlacement._bid._width},height={adPlacement._bid._height}\" /></head><body onclick=\"Unity.call('{AdClickMessageName}');\" style=\"margin:0;zoom:{scaleFactor * 100}%\">{adPlacement._bid._adMarkup}</body></html>";
                    var webView = GetAvailableWebViewFor(adPlacement);
                    SetWebViewLayout(adPlacement, webView);
                    webView.LoadAdPlacement(adPlacement, html);
                    break;
                case ImpressionType.VideoAd:
                    var videoAdPlacement = (VideoAdPlacement)adPlacement;
                    
                    var vastBytes = Encoding.UTF8.GetBytes(adPlacement._bid._adMarkup);
                    var vastStream = new MemoryStream(vastBytes);
                    var reader = XmlReader.Create(vastStream);
                    videoAdPlacement._vast.Read(reader);
                    videoAdPlacement._creative = (LinearCreative)videoAdPlacement._vast._creatives[0];
                    if (videoAdPlacement._creative._trackingEvents.TryGetValue("firstQuartile", out var firstQuartileUrl))
                    {
                        videoAdPlacement._callbacks.Add(new VideoAdPlacement.ProgressCallback(videoAdPlacement._creative._duration * 0.25f, firstQuartileUrl));
                    }
                    if (videoAdPlacement._creative._trackingEvents.TryGetValue("midpoint", out var midPointUrl))
                    {
                        videoAdPlacement._callbacks.Add(new VideoAdPlacement.ProgressCallback(videoAdPlacement._creative._duration * 0.5f, midPointUrl));
                    }
                    if (videoAdPlacement._creative._trackingEvents.TryGetValue("thirdQuartile", out var thirdQuartileUrl))
                    {
                        videoAdPlacement._callbacks.Add(new VideoAdPlacement.ProgressCallback(videoAdPlacement._creative._duration * 0.75f, thirdQuartileUrl));
                    }
                    
                    videoAdPlacement.StartLoading(OnLoad);

                    break;
            }
        }

        public void ShowAd(AdPlacement placement)
        {
            if (_currentPlacement != null)
            {
                _isCloseTriggered = true;
            }

            _nextPlacement = placement;
        }

        public void CloseAd()
        {
            TriggerClose();
        }

        public void OnUpdate()
        {
            if (_isCloseTriggered) {
                switch (_currentPlacement._type)
                {
                    case ImpressionType.Banner:
                    case ImpressionType.Interstitial:
                        var webView = _placementToWebView[_currentPlacement];
                        webView.SetVisibility(false);
                        webView.LoadAdPlacement(null, "");
                        webView.ClearCache(true);
                        _placementToWebView.Remove(_currentPlacement);
                        _availableWebViews.Enqueue(webView);
                        break;
                    case ImpressionType.VideoAd:
#if UNITY_ANDROID && !UNITY_EDITOR
                        GetFirstWebView().ShowVideo(null);
#else
                        _videoPlayer.Stop();
                        _videoPlayer.url = null;   
#endif
                        break;
                }

                SetRendererFor(null);
                _adManager.OnAdClose(_currentPlacement);
                _currentPlacement = null;
                _isCloseTriggered = false;
            }
            
            if (_currentPlacement != null)
            {
                HandleInput();
            }

            if (_nextPlacement != null)
            {
                SetRendererFor(_nextPlacement);
                switch (_nextPlacement._type)
                {
                    case ImpressionType.Banner:
                    case ImpressionType.Interstitial:
                        var webView = _placementToWebView[_nextPlacement];
                        webView.SetVisibility(true);
                        break;
                    case ImpressionType.VideoAd:
                        var videoAdPlacement = (VideoAdPlacement)_nextPlacement;
#if UNITY_ANDROID && !UNITY_EDITOR
                        GetFirstWebView().ShowVideo(videoAdPlacement.VideoPath);
#else
                        _videoPlayer.url = videoAdPlacement.VideoPath;
                        _videoPlayer.Play();           
#endif
             

                        if (!string.IsNullOrEmpty(videoAdPlacement._vast._impressionUrl))
                        {
                            UnityWebRequest.Get(videoAdPlacement._vast._impressionUrl).SendWebRequest();
                        }

                        if (videoAdPlacement._creative._trackingEvents.TryGetValue("start", out var onStartUrl))
                        {
                            UnityWebRequest.Get(onStartUrl).SendWebRequest();
                        }
                        break;
                }
                
                _currentPlacement = _nextPlacement;

                _nextPlacement = null;
                _adManager.OnAdShow(_currentPlacement);
            }
            else
            {
                var orientation = Screen.orientation;
                if (_currentOrientation != orientation)
                {
                    _currentOrientation = orientation;
                    if (_currentPlacement != null &&
                        (_currentPlacement._type == ImpressionType.Banner || _currentPlacement._type == ImpressionType.Interstitial))
                    {
                        SetWebViewLayout(_currentPlacement, _placementToWebView[_currentPlacement]);
                    }
                }
            }

            if (_videoPlayer.isPlaying)
            {
                var videoAdPlacement = (VideoAdPlacement) _currentPlacement;
                if (videoAdPlacement._callbacks.Count > 0 && _videoPlayer.time >= videoAdPlacement._callbacks[0]._time)
                {
                    UnityWebRequest.Get(videoAdPlacement._callbacks[0]._url).SendWebRequest();
                    videoAdPlacement._callbacks.RemoveAt(0);
                }

                if (!videoAdPlacement._isRewardSent && _videoPlayer.time >= videoAdPlacement._creative._duration)
                {
                    if (videoAdPlacement._creative._trackingEvents.TryGetValue("complete", out var onCompleteUrl))
                    {
                        UnityWebRequest.Get(onCompleteUrl).SendWebRequest();
                    }
                    
                    _adManager.OnAdReward(videoAdPlacement);
                    videoAdPlacement._isRewardSent = true;
                }
            }
        }

        private void OnLoad(AdPlacement placement)
        {
            _adManager.OnPlacementCreativeLoad(placement);
        }

        private void HandleInput()
        {
            Vector3 clickPosition = Vector3.back;
            if (Input.GetMouseButtonUp(0))
            {
                clickPosition = Input.mousePosition;
            }
            for (var i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    clickPosition = Input.touches[i].position;
                }
            }

            var isVideoOrInterstitial = _currentPlacement._type == ImpressionType.Interstitial ||
                                        _currentPlacement._type == ImpressionType.VideoAd;
            
            if (clickPosition != Vector3.back)
            {
                if (isVideoOrInterstitial)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(_closeRect, clickPosition, _camera))
                    {
                        TriggerClose();
                    }
                    else
                    {
                        OnPlacementClick();
                    }
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(_bannerRect, clickPosition, _camera))
                {
                    OnPlacementClick();
                }
            }
        }

        private void OnPlacementClick()
        {
            NeftaCore.Log($"OnPlacementClick {_currentPlacement._type} ({_currentPlacement._id})");
            var clickUrl = _currentPlacement._bid?._ext?._nefta?._tracking_click_url;
            if (!string.IsNullOrEmpty(clickUrl))
            {
                UnityWebRequest.Get(clickUrl).SendWebRequest();
            }

            var redirectUrl = _currentPlacement._bid?._ext?._nefta?._redirect_click_url;
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                Application.OpenURL(redirectUrl);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            var videoAdPlacement = _currentPlacement as VideoAdPlacement;
            if (videoAdPlacement == null)
            {
                return;
            }
            
            NeftaCore.Log($"OnApplicationPause {pauseStatus}");
            if (pauseStatus)
            {
                if (videoAdPlacement._creative._trackingEvents.TryGetValue("pause", out var onPauseUrl))
                {
                    UnityWebRequest.Get(onPauseUrl).SendWebRequest();
                }
            }
            else
            {
                if (videoAdPlacement._creative._trackingEvents.TryGetValue("resume", out var onResumeUrl))
                {
                    UnityWebRequest.Get(onResumeUrl).SendWebRequest();
                }
            }
        }

        private void TriggerClose()
        {
            if (_currentPlacement == null)
            {
                return;
            }

            NeftaCore.Log($"TriggerClose {_currentPlacement._type} ({_currentPlacement._id})");
            _isCloseTriggered = true;
        }

        private WebViewObject GetAvailableWebViewFor(AdPlacement adPlacement)
        {
            if (!_availableWebViews.TryDequeue(out var webView))
            {
                webView = CreateWebView();
            }
            
            _placementToWebView.Add(adPlacement, webView);
            return webView;
        }

        private WebViewObject GetFirstWebView()
        {
            if (_firstWebView == null)
            {
                var webView = CreateWebView();
                _availableWebViews.Enqueue(webView);
            }

            return _firstWebView;
        }

        private void SetRendererFor(AdPlacement placement)
        {
            if (placement == null)
            {
                _renderer.SetActive(false);
                return;
            }
            
            var topObstruction = Screen.height - Screen.safeArea.height - Screen.safeArea.y;
            var topOffset = topObstruction / Screen.height * _canvasRect.rect.size.y;
            _closeRect.anchoredPosition = new Vector2(0, -topOffset);
            _renderer.SetActive(true);
            var isBanner = placement._type == ImpressionType.Banner;
            _background.SetActive(!isBanner);
            if (isBanner)
            {
                var canvasSize = _canvasRect.rect.size;
                var bannerSize = new Vector2(
                    (float) placement._screenWidth / Screen.width * canvasSize.x,
                    (float) placement._screenHeight / Screen.height * canvasSize.y);
                _bannerRect.sizeDelta = bannerSize;
            }
        }

        private WebViewObject CreateWebView()
        {
            var webView = gameObject.AddComponent<WebViewObject>();
            webView.Init( 
                cb: (msg) =>
                {
                    if (msg == AdClickMessageName)
                    {
                        OnPlacementClick();
                    }
                    else
                    {
                        NeftaCore.Info($"CallFromJS[{msg}]");
                    }
                },
                err: (msg) =>
                {
                    NeftaCore.Warn($"CallOnError[{msg}]");
                },
                httpErr: (msg) =>
                {
                    NeftaCore.Warn($"CallOnHttpError[{msg}]");
                },
                started: (msg) =>
                {
                    NeftaCore.Info($"CallOnStarted[{msg}]");
                },
                ld: OnLoad
            );
            webView.OnClick = OnPlacementClick;
            
            if (_firstWebView == null)
            {
                _firstWebView = webView;
            }

            return webView;
        }
        
        private void SetWebViewLayout(AdPlacement placement, WebViewObject webView)
        {
            var topMargin = (int)(Screen.safeArea.center.y - placement._screenHeight * 0.5f);
            var bottomMargin = Screen.height - topMargin - placement._screenHeight;
            var leftMargin = (int)(Screen.safeArea.center.x - placement._screenWidth * 0.5f);
            var rightMargin = Screen.width - leftMargin - placement._screenWidth;

            switch (placement._position)
            {
                case AdPlacement.Position.Top:
                    topMargin = (int)(Screen.height - Screen.safeArea.height - Screen.safeArea.y);
                    bottomMargin = Screen.height - topMargin - placement._screenHeight;
                    break;
                case AdPlacement.Position.Right:
                    rightMargin = (int)(Screen.width - Screen.safeArea.width - Screen.safeArea.x);
                    leftMargin = Screen.width - rightMargin - placement._screenWidth;
                    break;
                case AdPlacement.Position.Bottom:
                    bottomMargin = (int) Screen.safeArea.y;
                    topMargin = Screen.height - bottomMargin - placement._screenHeight;
                    break;
                case AdPlacement.Position.Left:
                    leftMargin = (int)Screen.safeArea.x;
                    rightMargin = Screen.width - leftMargin - placement._screenWidth;
                    break;
            }

            webView.SetMargins(leftMargin, topMargin, rightMargin, bottomMargin);
        }

        private void OnDestroy()
        {
            var videoCacheDirectory = VideoAdPlacement.GetAssetDirectory();
            if (!Directory.Exists(videoCacheDirectory))
            {
                return;
            }
            
            string[] creativePaths = Directory.GetFiles(videoCacheDirectory);
            foreach (string creativePath in creativePaths)
            {
                File.Delete(creativePath);
            }
        }
    }
}