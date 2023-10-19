using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nefta.Core.Data
{
    [Serializable]
    public class InitResponse
    {
        [DataMember(Name = "nuid")] public string _nuid;
        [DataMember(Name = "ad_units")] public List<AdUnit> _adUnits;
    }
}