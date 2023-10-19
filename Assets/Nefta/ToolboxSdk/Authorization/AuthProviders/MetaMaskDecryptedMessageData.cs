using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization.AuthProviders
{
    [Serializable]
    public class MetaMaskDecryptedMessageData
    {
        [DataMember(Name = "id")] public string _id;
        [DataMember(Name = "jsonrpc")] public string _version;
        [DataMember(Name = "result")] public string _result;
    }
}