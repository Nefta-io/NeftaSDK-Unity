using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    [Serializable]
    public class MetaMaskResponseMessage
    {
        [DataMember(Name="id")] public string _id;
        [DataMember(Name="message")] public string _message;
    }
}