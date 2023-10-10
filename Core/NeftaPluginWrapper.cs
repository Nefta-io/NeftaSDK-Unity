using UnityEngine;

namespace Nefta.Core
{
    public class NeftaPluginWrapper : MonoBehaviour
    {
#if !UNITY_EDITOR && UNITY_IOS

#endif
        
        
#if UNITY_EDITOR
        public NeftaPlugin _plugin;
#elif UNITY_IOS
        private IntPtr _plugin;
#elif UNITY_ANDROID
        private AndroidJavaObject _plugin;
        private AndroidJavaObject _unityActivity;
#endif

        public void Init(string appId)
        {
#if UNITY_EDITOR
            _plugin = NeftaPlugin.Init(gameObject, appId);
#elif UNITY_STANDALONE_OSX
            _plugin = _NeftaPluginMac_Init(name, appId);
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

#elif UNITY_STANDALONE_OSX

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
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            _NeftaPluginMac_EnableAds(_plugin, enable);
#elif UNITY_ANDROID
            _plugin.Call("EnableAds", enable);
            _plugin.Call("PrepareRenderer", _unityActivity);
#endif
        }

        public void SetUser(string neftaUser)
        {
#if UNITY_EDITOR
            _plugin.SetUser(neftaUser);
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

#elif UNITY_ANDROID
            user = _plugin.Call<string>("GetUser");
#endif
            return user;
        }

        public void Record(string recordedEvent)
        {
#if UNITY_EDITOR
            
#elif UNITY_IOS
            
#elif UNITY_ANDROID
            _plugin.Call("Record", recordedEvent);
#endif
        }

        public void SetPublisherUserId(string publisherUserId)
        {
#if UNITY_EDITOR
            _plugin.SetPublisherUserId(publisherUserId);
#elif UNITY_IOS

#elif UNITY_ANDROID
            _plugin.Call("SetPublisherUserId", publisherUserId);
#endif
        }

        public void Bid(string placementId, bool autoLoad)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.Bid(placementId, autoLoad);
#elif UNITY_IOS

#elif UNITY_ANDROID
            _plugin.Call("Bid", placementId, autoLoad);
#endif
        }
        
        public void LoadPlacement(string placementId)
        {
#if UNITY_EDITOR
            NeftaPlugin.Instance.LoadPlacement(placementId);
#elif UNITY_IOS
            if (_plugin == IntPtr.Zero)
            {
                return;
            }
            //_CWebViewPlugin_LoadHTML(placementId);
#elif UNITY_ANDROID
            if (_plugin == null)
            {
                return;
            }
            _plugin.Call("LoadPlacement", placementId);
#endif
        }

        public void ShowPlacement(string placementId)
        {
#if UNITY_EDITOR
            _plugin.ShowPlacement(placementId);
#elif UNITY_IOS

#elif UNITY_ANDROID
            _plugin.Call("ShowPlacement", placementId);
#endif
        }

        public void ClosePlacement(string placementId)
        {
#if UNITY_EDITOR
            _plugin.ClosePlacement(placementId);
#elif UNITY_ANDROID
            _plugin.Call("ClosePlacement", placementId);
#endif
        }
        
        public string CheckMessages()
        {
#if UNITY_EDITOR
            return _plugin.GetMessage();
#elif UNITY_ANDROID
            return _plugin.Call<string>("GetMessage");
#endif
        }
    }
}