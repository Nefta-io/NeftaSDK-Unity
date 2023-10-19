#if NEFTA_INTEGRATION_METAMASK
using MetaMask.Unity;

namespace ToolboxDemo.Authentication.MetaMask
{
    public class MetaMaskConfigWrapper : MetaMaskConfig
    {
        public void Init()
        {
            appName = "NeftaDemo";
            appUrl = "nefta.io";
            encryptionPassword = "neftasample";
            sessionIdentifier = "nefta.toolboxSDK.demo";


        }
    }
}
#endif