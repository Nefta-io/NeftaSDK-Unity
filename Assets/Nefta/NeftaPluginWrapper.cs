#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;
#endif
using UnityEngine;

namespace Nefta
{
    public class NeftaPluginWrapper : MonoBehaviour
    {
#if UNITY_EDITOR
        private NeftaPlugin _plugin;
#elif UNITY_IOS
        private delegate void OnReadyDelegate(string configuration);
        private delegate void OnBidDelegate(string pId, float price);
        private delegate void OnLoadFailDelegate(string pId, string error);
        private delegate void OnLoadDelegate(string pId, int width, int height);
        private delegate void OnChangeDelegate(string pId);
 
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

        [MonoPInvokeCallback(typeof(OnLoadDelegate))] 
        private static void OnLoad(string pId, int width, int height) {
            _listener?.IOnLoad(pId, width, height);
        }

        [MonoPInvokeCallback(typeof(OnChangeDelegate))] 
        private static void OnShow(string pId) {
            _listener?.IOnShow(pId);
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
        private static extern void NeftaPlugin_EnableLogging(bool enable);

        [DllImport ("__Internal")]
        private static extern IntPtr NeftaPlugin_Init(string appId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_RegisterCallbacks(OnReadyDelegate onReady, OnBidDelegate onBid, OnChangeDelegate onLoadStart, OnLoadFailDelegate onLoadFail, OnLoadDelegate onLoad, OnChangeDelegate onShow, OnChangeDelegate onClick, OnChangeDelegate onReward, OnChangeDelegate onClose);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Record(IntPtr instance, string recordedEvent);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPublisherUserId(IntPtr instance, string publisherUserId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_EnableAds(IntPtr instance, bool enable);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_EnableBannerWithId(IntPtr instance, string pId, bool enable);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPlacementPositionWithId(IntPtr instance, string pId, int position);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPlacementModeWithId(IntPtr instance, string pId, int mode);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_BidWithId(IntPtr instance, string pId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_LoadWithId(IntPtr instance, string pId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_ShowWithId(IntPtr instance, string pId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Close(IntPtr instance);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_CloseWithId(IntPtr instance, string pid);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Mute(IntPtr instance, bool mute);

        [DllImport ("__Internal")]
        private static extern string NeftaPlugin_GetNuid(IntPtr instance, bool present);

        private IntPtr _plugin;
        private static INeftaListener _listener;
#elif UNITY_ANDROID
        private AndroidJavaObject _plugin;
        private AndroidJavaObject _unityActivity;
#endif
        
        public static void EnableLogging(bool enable)
        {
#if UNITY_EDITOR
            NeftaPlugin.EnableLogging(enable);
#elif UNITY_IOS
            NeftaPlugin_EnableLogging(enable);
#endif
        }

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
        
#if !UNITY_EDITOR && UNITY_ANDROID
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                _plugin.Call("OnPause");
            }
            else
            {
                 _plugin.Call("OnResume");
            }
        }
#endif

        public void RegisterListener(NeftaPluginListener listener)
        {
#if UNITY_EDITOR
            _plugin._listener = listener;
#elif UNITY_IOS
            _listener = listener;
            NeftaPlugin_RegisterCallbacks(OnReady, OnBid, OnLoadStart, OnLoadFail, OnLoad, OnShow, OnClick, OnClose, OnReward);
#elif UNITY_ANDROID
            _plugin.Call("SetListener", listener);
#endif
        }

        public string GetNuid(bool present)
        {
            string nuid = null;
#if UNITY_EDITOR
            nuid = _plugin.GetNuid(present);
#elif UNITY_IOS
            nuid = NeftaPlugin_GetNuid(_plugin, present);
#elif UNITY_ANDROID
            nuid = _plugin.Call<string>("GetNuid", present);
#endif
            return nuid;
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
        
        public void SetPlacementPosition(string id, int position)
        {
#if UNITY_EDITOR
            _plugin.SetPlacementPosition(id, position);
#elif UNITY_IOS
            NeftaPlugin_SetPlacementPositionWithId(_plugin, id, position);
#elif UNITY_ANDROID
            _plugin.Call("SetPlacementPosition", id, position);
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
        
        public void Mute(bool mute)
        {
#if UNITY_EDITOR
            _plugin.Mute(mute);
#elif UNITY_IOS
            NeftaPlugin_Mute(_plugin, mute);
#elif UNITY_ANDROID
            _plugin.Call<string>("Mute", mute);
#endif
        }
    }
}