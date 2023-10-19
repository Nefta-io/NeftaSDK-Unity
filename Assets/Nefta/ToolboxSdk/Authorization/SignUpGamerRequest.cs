using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization
{
    [Serializable]
    public class SignUpGamerRequest
    {
        [DataMember(Name="public_project_id")] public string _publicProjectId;
        [DataMember(Name="email")] public string _email;
        [DataMember(Name="username")] public string _username;
        [DataMember(Name="wallet")] public string _wallet;
    }
}