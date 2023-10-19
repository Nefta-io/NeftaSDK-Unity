using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization
{
    [Serializable]
    public class ConvertGuestToFullUserRequest
    {
        [DataMember(Name="email")] public string _email;
    }
}