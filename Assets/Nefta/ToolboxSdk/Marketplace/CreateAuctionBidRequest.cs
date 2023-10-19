using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Marketplace
{
    [Serializable]
    public class CreateAuctionBidRequest
    {
        [DataMember(Name = "bid_price")] public float _bidPrice;
    }
}