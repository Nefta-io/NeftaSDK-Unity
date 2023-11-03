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