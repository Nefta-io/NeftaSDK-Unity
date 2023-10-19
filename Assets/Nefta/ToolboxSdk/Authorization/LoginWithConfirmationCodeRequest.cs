using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization
{
    [Serializable]
    public class LoginWithConfirmationCodeRequest
    {
        [DataMember(Name="email")] public string _email;
        [DataMember(Name="code")] public string _code;
    }
}