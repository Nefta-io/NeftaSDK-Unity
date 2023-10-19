using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Authorization
{
    [Serializable]
    public class SignupGuestUserRequest
    {
        [DataMember(Name="public_project_id")] public string _publicProjectId;
        [DataMember(Name="username")] public string _username;
    }
}