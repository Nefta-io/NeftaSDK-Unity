#if NEFTA_INTEGRATION_METAMASK
using MetaMask.Transports.Unity;
using UnityEngine;

namespace ToolboxDemo.Authentication.MetaMask
{
    public class MetaMaskTransportWrapper : MetaMaskUnityTransportBroadcaster
    {
        public void AssignListener(GameObject listener)
        {
            listeners = new[] { listener };
        }
    }
}
#endif