#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;
#endif
using UnityEngine;

namespace Nefta.Core
{
    public class NeftaPluginWrapper : MonoBehaviour
    {
#if UNITY_EDITOR
        private NeftaPlugin _plugin;
#elif UNITY_IOS
        private delegate void OnReadyDelegate(string configuration);
        private delegate void OnBidDelegate(string pId, float price);
        private delegate void OnChangeDelegate(string pId);
        private delegate void OnLoadFailDelegate(string pId, string error);
        private delegate void OnShowDelegate(string pId, int width, int height);
 
        [MonoPInvokeCallback(typeof(OnReadyDelegate))] 
        private static void OnReady(string configuration) {
            _listener?.IOnReady(configuration);
        }

        [MonoPInvokeCallback(typeof(OnBidDelegate))] 
        private static void OnBid(string pId, float price) {
            _listener?.IOnBid(pId, price);
        }

        [MonoPInvokeCallback(typeof(OnChangeDelegate))] 
        private static void OnLoadStart(string pId) {
            _listener?.IOnLoadStart(pId);
        }

        [MonoPInvokeCallback(typeof(OnLoadFailDelegate))] 
        private static void OnLoadFail(string pId, string error) {
            _listener?.IOnLoadFail(pId, error);
        }

        [MonoPInvokeCallback(typeof(OnChangeDelegate))] 
        private static void OnLoad(string pId) {
            _listener?.IOnLoad(pId);
        }

        [MonoPInvokeCallback(typeof(OnShowDelegate))] 
        private static void OnShow(string pId, int width, int height) {
            _listener?.IOnShow(pId, width, height);
        }

        [MonoPInvokeCallback(typeof(OnShowDelegate))] 
        private static void OnBannerChange(string pId, int width, int height) {
            _listener?.IOnBannerChange(pId, width, height);
        }

        [MonoPInvokeCallback(typeof(OnChangeDelegate))] 
        private static void OnClick(string pId) {
            _listener?.IOnClick(pId);
        }

        [MonoPInvokeCallback(typeof(OnChangeDelegate))] 
        private static void OnClose(string pId) {
            _listener?.IOnClose(pId);
        }

        [MonoPInvokeCallback(typeof(OnChangeDelegate))] 
        private static void OnReward(string pId) {
            _listener?.IOnReward(pId);
        }

        [DllImport ("__Internal")]
        private static extern IntPtr NeftaPlugin_Init(string appId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_RegisterCallbacks(OnReadyDelegate onReady, OnBidDelegate onBid, OnChangeDelegate onLoadStart, OnLoadFailDelegate onLoadFail, OnChangeDelegate onLoad, OnShowDelegate onShow, OnShowDelegate onBannerChange, OnChangeDelegate onClick, OnChangeDelegate onReward, OnChangeDelegate onClose);
         
        [DllImport ("__Internal")]
        private static extern string NeftaPlugin_GetToolboxUser(IntPtr instance);   

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetToolboxUser(IntPtr instance, string neftaUser);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Record(IntPtr instance, string recordedEvent);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPublisherUserId(IntPtr instance, string publisherUserId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_EnableAds(IntPtr instance, bool enable);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_EnableBannerWithType(IntPtr instance, bool enable);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_EnableBannerWithId(IntPtr instance, string pId, bool enable);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPlacementModeWithType(IntPtr instance, int type, int mode);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPlacementModeWithId(IntPtr instance, string pId, int mode);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_BidWithType(IntPtr instance, int type);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_BidWithId(IntPtr instance, string pId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_LoadWithType(IntPtr instance, int type);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_LoadWithId(IntPtr instance, string pId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_ShowWithType(IntPtr instance, int type);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_ShowWithId(IntPtr instance, string pId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Close(IntPtr instance);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_CloseWithId(IntPtr instance, string pid);

        private IntPtr _plugin;
        private static INeftaListener _listener;
#elif UNITY_ANDROID
        private AndroidJavaObject _plugin;
        private AndroidJavaObject _unityActivity;
#endif

        public void Init(string appId)
        {
#if UNITY_EDITOR
            _plugin = NeftaPlugin.Init(gameObject, appId);
#elif UNITY_IOS
            _plugin = NeftaPlugin_Init(appId);
#elif UNITY_ANDROID
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass neftaPluginClass = new AndroidJavaClass("com.nefta.sdk.NeftaPlugin");
            _plugin = neftaPluginClass.CallStatic<AndroidJavaObject>("Init", _unityActivity, appId);
#endif
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterListener(NeftaPluginListener listener)
        {
#if UNITY_EDITOR
            _plugin._listener = listener;
#elif UNITY_IOS
            _listener = listener;
            NeftaPlugin_RegisterCallbacks(OnReady, OnBid, OnLoadStart, OnLoadFail, OnLoad, OnShow, OnBannerChange, OnClick, OnClose, OnReward);
#elif UNITY_ANDROID
            _plugin.Call("SetListener", listener);
#endif
        }

        private void OnApplicationPause(bool pause)
        {
#if UNITY_EDITOR
            if (pause)
            {
                _plugin.OnPause();
            }
            else
            {
                _plugin.OnResume();
            }
#elif UNITY_IOS

#elif UNITY_ANDROID
            if (pause)
            {
                _plugin.Call("OnPause");
            }
            else
            {
                _plugin.Call("OnResume");
            }
#endif
        }

        public void SetToolboxUser(string neftaUser)
        {
#if UNITY_EDITOR
            _plugin.SetToolboxUser(neftaUser);
#elif UNITY_IOS
            NeftaPlugin_SetToolboxUser(_plugin, neftaUser);
#elif UNITY_ANDROID
            _plugin.Call("SetToolboxUser", neftaUser);
#endif
        }

        public string GetToolboxUser()
        {
            string user = null;
#if UNITY_EDITOR
            user = _plugin.GetToolboxUser();
#elif UNITY_IOS
            user = NeftaPlugin_GetToolboxUser(_plugin);
#elif UNITY_ANDROID
            user = _plugin.Call<string>("GetToolboxUser");
#endif
            return user;
        }
        
        public void SetPublisherUserId(string publisherUserId)
        {
#if UNITY_EDITOR
            _plugin.SetPublisherUserId(publisherUserId);
#elif UNITY_IOS
            NeftaPlugin_SetPublisherUserId(_plugin, publisherUserId);
#elif UNITY_ANDROID
            _plugin.Call("SetPublisherUserId", publisherUserId);
#endif
        }

        public void Record(string gameEvent)
        {
#if UNITY_EDITOR
            _plugin.Record(gameEvent);
#elif UNITY_IOS
            NeftaPlugin_Record(_plugin, gameEvent);
#elif UNITY_ANDROID
            _plugin.Call("Record", gameEvent);
#endif
        }
        
        public void EnableAds(bool enable)
        {
#if UNITY_EDITOR
            _plugin.EnableAds(enable);
#elif UNITY_IOS
            NeftaPlugin_EnableAds(_plugin, enable);
#elif UNITY_ANDROID
            _plugin.Call("EnableAds", enable);
            _plugin.Call("PrepareRenderer", _unityActivity);
#endif
        }
        
        public void SetPlacementMode(int type, int mode)
        {
#if UNITY_EDITOR
            _plugin.SetPlacementMode(type, mode);
#elif UNITY_IOS
            NeftaPlugin_SetPlacementModeWithType(_plugin, type, mode);
#elif UNITY_ANDROID
            _plugin.Call("SetPlacementMode", type, mode);
#endif
        }
        
        public void SetPlacementMode(string id, int mode)
        {
#if UNITY_EDITOR
            _plugin.SetPlacementMode(id, mode);
#elif UNITY_IOS
            NeftaPlugin_SetPlacementModeWithId(_plugin, id, mode);
#elif UNITY_ANDROID
            _plugin.Call("SetPlacementMode", id, mode);
#endif
        }

        public void Bid(int type)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Bid(type);
#elif UNITY_IOS
            NeftaPlugin_BidWithType(_plugin, type);
#elif UNITY_ANDROID
            _plugin.Call("Bid", type);
#endif
        }
        
        public void Bid(string id)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Bid(id);
#elif UNITY_IOS
            NeftaPlugin_BidWithId(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Bid", id);
#endif
        }
        
        public void Load(int type)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Load(type);
#elif UNITY_IOS
              NeftaPlugin_LoadWithType(_plugin, type);
#elif UNITY_ANDROID
            _plugin.Call("Load", type);
#endif
        }

        public void EnableBanner(bool isEnabled)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.EnableBanner(isEnabled);
#elif UNITY_IOS
            NeftaPlugin_EnableBannerWithType(_plugin, isEnabled);
#elif UNITY_ANDROID
            _plugin.Call("EnableBanner", isEnabled);
#endif
        }

        public void EnableBanner(string id, bool isEnabled)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.EnableBanner(id, isEnabled);
#elif UNITY_IOS
            NeftaPlugin_EnableBannerWithId(_plugin, id, isEnabled);
#elif UNITY_ANDROID
            _plugin.Call("EnableBanner", id, isEnabled);
#endif
        }
        
        public void Load(string id)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Load(id);
#elif UNITY_IOS
            NeftaPlugin_LoadWithId(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Load", id);
#endif
        }

        public void Show(int type)
        {
#if UNITY_EDITOR
            _plugin.Show(type);
#elif UNITY_IOS
            NeftaPlugin_ShowWithType(_plugin, type);
#elif UNITY_ANDROID
            _plugin.Call("Show", type);
#endif
        }

        public void Show(string id)
        {
#if UNITY_EDITOR
            _plugin.Show(id);
#elif UNITY_IOS
            NeftaPlugin_ShowWithId(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Show", id);
#endif
        }

        public void Close()
        {
#if UNITY_EDITOR
            _plugin.Close();
#elif UNITY_IOS
            NeftaPlugin_Close(_plugin);
#elif UNITY_ANDROID
            _plugin.Call("Close");
#endif
        }
        
        public void Close(string id)
        {
#if UNITY_EDITOR
            _plugin.Close(id);
#elif UNITY_IOS
            NeftaPlugin_CloseWithId(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Close", id);
#endif
        }
    }
}