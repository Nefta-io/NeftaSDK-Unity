using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.Marketplace
{
    [Serializable]
    public class CreateAuctionRequest
    {
        [DataMember(Name = "ownership_id")] public string _ownershipId;
        [DataMember(Name = "auction_type")] public string _auctionType;
        [DataMember(Name = "start_price")] public float _startPrice;
        [DataMember(Name = "purchase_price")] public float _purchasePrice;
    }
}