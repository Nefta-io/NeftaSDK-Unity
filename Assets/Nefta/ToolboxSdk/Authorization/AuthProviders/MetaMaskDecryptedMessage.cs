using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    [Serializable]
    public class MetaMaskDecryptedMessage
    {
        [DataMember(Name = "name")] public string _name;
        [DataMember(Name = "data")] public MetaMaskDecryptedMessageData _data;
    }
}