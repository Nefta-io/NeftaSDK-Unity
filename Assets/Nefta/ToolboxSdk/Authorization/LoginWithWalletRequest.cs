using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization
{
    [Serializable]
    public class LoginWithWalletRequest
    {
        [DataMember(Name="wallet")] public string _wallet;
        [DataMember(Name="mid")] public string _mid;
    }
}