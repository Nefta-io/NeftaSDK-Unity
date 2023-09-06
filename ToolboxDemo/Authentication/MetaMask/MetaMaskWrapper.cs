#if NEFTA_INTEGRATION_METAMASK
using MetaMask.Unity;
using System.Reflection;
#endif
using UnityEngine;

namespace Nefta.ToolboxDemo.Authentication.MetaMask
{
#if NEFTA_INTEGRATION_METAMASK
    public class MetaMaskWrapper : MetaMaskUnity
    {
        public void Init()
        {
            instance = this;
            
            var metaMaskEventHandler = gameObject.AddComponent<MetaMaskUnityEventHandler>();
            var metaMaskInfo = typeof(MetaMaskUnityEventHandler).GetField("_metaMask", BindingFlags.NonPublic | BindingFlags.Instance);
            metaMaskInfo.SetValue(metaMaskEventHandler, this);
            
            var eventInfo = typeof(MetaMaskUnity).GetField("_eventHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            eventInfo.SetValue(this, metaMaskEventHandler);
            
            var configWrapper = ScriptableObject.CreateInstance<MetaMaskConfigWrapper>();
            configWrapper.Init();
            
            config = configWrapper;

            initializeOnAwake = false;
        }
    }
#else
    public class MetaMaskWrapper : MonoBehaviour
    {
        public void Init()
        {
            
        }
    }
#endif
}