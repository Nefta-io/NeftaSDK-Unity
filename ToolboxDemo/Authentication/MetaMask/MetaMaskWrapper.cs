#if NEFTA_INTEGRATION_METAMASK
using MetaMask.Unity;
#endif
using UnityEngine;

namespace Nefta.ToolboxDemo.Authentication.MetaMask
{
#if NEFTA_INTEGRATION_METAMASK
    public class MetaMaskWrapper : MetaMaskUnity
    {
        public void Init()
        {
            var configWrapper = ScriptableObject.CreateInstance<MetaMaskConfigWrapper>();
            configWrapper.Init();
            
            config = configWrapper;

            initializeOnStart = false;
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