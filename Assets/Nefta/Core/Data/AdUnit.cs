using System;
using System.Runtime.Serialization;

namespace Nefta.Core.Data
{
    [Serializable]
    public class AdUnit
    {
        [DataMember(Name = "id")] public string _id;
        [DataMember(Name = "app_id")] public string _appId;
        [DataMember(Name = "type")] public string _type;
        [DataMember(Name = "width")] public int? _width;
        [DataMember(Name = "height")] public int? _height;
    }
}