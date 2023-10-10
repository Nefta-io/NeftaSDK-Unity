using System;
using System.Runtime.Serialization;

namespace Nefta.Core.Data
{
    [Serializable]
    public class NeftaUser
    {
        [DataMember(Name = "user_token")] public string _token;
        [DataMember(Name = "user_id")] public string _userId;
        [DataMember(Name = "username")] public string _username;
        [DataMember(Name = "email")] public string _email;
        [DataMember(Name = "address")] public string _address;
    }
}