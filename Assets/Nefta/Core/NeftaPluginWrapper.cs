#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace Nefta.Core
{
    public class NeftaPluginWrapper : MonoBehaviour
    {
#if UNITY_EDITOR
        private NeftaPlugin _plugin;
#elif UNITY_IOS
        [DllImport ("__Internal")]
        private static extern IntPtr NeftaPlugin_Init(string appId, bool useMessages);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_EnableAds(IntPtr instance, bool enable);
         
        [DllImport ("__Internal")]
        private static extern string NeftaPlugin_GetUser(IntPtr instance);   

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetUser(IntPtr instance, string neftaUser);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Record(IntPtr instance, string recordedEvent);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_SetPublisherUserId(IntPtr instance, string publisherUserId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Bid(IntPtr instance, string placementId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_BidWithAutoLoad(IntPtr instance, string placementId);

        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Load(IntPtr instance, string placementId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Show(IntPtr instance, string placementId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_Close(IntPtr instance, string placementId);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_OnResume(IntPtr instance);
            
        [DllImport ("__Internal")]
        private static extern void NeftaPlugin_OnPause(IntPtr instance);
            
        [DllImport ("__Internal")]
        private static extern string NeftaPlugin_GetMessage(IntPtr instance);

        private IntPtr _plugin;
#elif UNITY_ANDROID
        private AndroidJavaObject _plugin;
        private AndroidJavaObject _unityActivity;
#endif

        public void Init(string appId)
        {
#if UNITY_EDITOR
            _plugin = NeftaPlugin.Init(gameObject, appId);
#elif UNITY_IOS
            _plugin = NeftaPlugin_Init(appId, true);
#elif UNITY_ANDROID
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            _plugin = new AndroidJavaObject("com.nefta.sdk.NeftaPlugin");
            _plugin.Call("Init", _unityActivity, appId, true);
#endif

            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationPause(bool pause)
        {
#if UNITY_EDITOR

#elif UNITY_IOS
            if (pause)
            {
                NeftaPlugin_OnPause(_plugin);
            }
            else
            {
                NeftaPlugin_OnResume(_plugin);
            }
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

        public void SetUser(string neftaUser)
        {
#if UNITY_EDITOR
            _plugin.SetUser(neftaUser);
#elif UNITY_IOS
            NeftaPlugin_SetUser(_plugin, neftaUser);
#elif UNITY_ANDROID
            _plugin.Call("SetUser", neftaUser);
#endif
        }

        public string GetUser()
        {
            string user = null;
#if UNITY_EDITOR
            user = _plugin.GetUser();
#elif UNITY_IOS
            user = NeftaPlugin_GetUser(_plugin);
#elif UNITY_ANDROID
            user = _plugin.Call<string>("GetUser");
#endif
            return user;
        }

        public void Record(string recordedEvent)
        {
#if UNITY_EDITOR
            
#elif UNITY_IOS
            NeftaPlugin_Record(_plugin, recordedEvent);
#elif UNITY_ANDROID
            _plugin.Call("Record", recordedEvent);
#endif
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

        public void Bid(string id, bool autoLoad)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Bid(id, autoLoad);
#elif UNITY_IOS
            if (autoLoad)
            {
                NeftaPlugin_BidWithAutoLoad(_plugin, id);
            }
            else
            {
                NeftaPlugin_Bid(_plugin, id);
            }
#elif UNITY_ANDROID
            _plugin.Call("Bid", id, autoLoad);
#endif
        }
        
        public void Load(string id)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Load(id);
#elif UNITY_IOS
            NeftaPlugin_Load(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Load", id);
#endif
        }

        public void Show(string id)
        {
#if UNITY_EDITOR
            _plugin.Show(id);
#elif UNITY_IOS
            NeftaPlugin_Show(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Show", id);
#endif
        }

        public void Close(string id)
        {
#if UNITY_EDITOR
            _plugin.Close(id);
#elif UNITY_IOS
            NeftaPlugin_Close(_plugin, id);
#elif UNITY_ANDROID
            _plugin.Call("Close", id);
#endif
        }
        
        public string CheckMessages()
        {
#if UNITY_EDITOR
            return _plugin.GetMessage();
#elif UNITY_IOS
            return NeftaPlugin_GetMessage(_plugin);
#elif UNITY_ANDROID
            return _plugin.Call<string>("GetMessage");
#endif
        }
    }
}