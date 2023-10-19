using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.GamerManagement
{
    [Serializable]
    public class GamerProfile
    {
        [DataMember(Name = "user_id")] public string _userId;
        [DataMember(Name = "username")] public string _username;
        [DataMember(Name = "address")] public string _address;
    }

}