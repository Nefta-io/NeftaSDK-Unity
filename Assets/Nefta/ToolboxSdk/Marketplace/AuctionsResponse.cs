using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Marketplace
{
    [Serializable]
    public class AuctionsResponse
    {
        [DataMember(Name = "results")] public List<Auction> _results;
        [DataMember(Name = "page")] public int _page;
        [DataMember(Name = "perPage")] public int _perPage;
        [DataMember(Name = "count")] public int _count;
    }
}