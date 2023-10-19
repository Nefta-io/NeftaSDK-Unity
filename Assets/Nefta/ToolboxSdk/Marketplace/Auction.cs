using System;
using System.Runtime.Serialization;
using Nefta.ToolboxSdk.Nft;

namespace Nefta.ToolboxSdk.Marketplace
{
    [Serializable]
    public class Auction
    {
        [DataMember(Name = "serial")] public int _serial;
        [DataMember(Name = "auction_type")] public string _auctionType;
        [DataMember(Name = "purchase_price")] public float? _purchasePrice;
        [DataMember(Name = "sell_price")] public float? _sellPrice;
        [DataMember(Name = "sell_date")] public DateTime? _sellDate;
        [DataMember(Name = "start_price")] public float? _startPrice;
        [DataMember(Name = "end_date")] public DateTime _endDate;
        [DataMember(Name = "auction_id")] public string _auctionId;
        [DataMember(Name = "start_date")] public DateTime _startDate;
        [DataMember(Name = "user_auctions")] public bool _userAuctions;
        [DataMember(Name = "status")] public string _status;
        [DataMember(Name = "number_of_bids")] public int? _numberBids;
        [DataMember(Name = "last_bid_price")] public float? _lastBidPrice;
        [DataMember(Name = "asset")] public NftAsset _asset;
    }
}