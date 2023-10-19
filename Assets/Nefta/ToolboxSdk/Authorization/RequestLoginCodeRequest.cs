using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization
{
    [Serializable]
    public class RequestLoginCodeRequest
    {
        [DataMember(Name="email")] public string _email;
    }
}