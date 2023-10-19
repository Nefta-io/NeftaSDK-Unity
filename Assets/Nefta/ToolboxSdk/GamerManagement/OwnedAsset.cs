using System;
using System.Runtime.Serialization;
using Nefta.ToolboxSdk.Nft;

namespace Nefta.ToolboxSdk.GamerManagement
{
    [Serializable]
    public class OwnedAsset
    {
        /// <summary>
        /// The ownership ID represents the NFT instance (e.g. serial #2 / 10k ) a user owns
        /// </summary>
        [DataMember(Name = "ownership_id")] public string _ownershipId;
        [DataMember(Name = "uri")] public string _uri;
        [DataMember(Name = "created_at")] public DateTime _createdAt;
        [DataMember(Name = "ownership_amount")] public int _ownershipAmount;
        [DataMember(Name = "purchase_price")] public float _purchasePrice;
        [DataMember(Name = "serial")] public int _serial;
        [DataMember(Name = "asset")] public NftAsset _nft;

        public bool IsOwnerInstanceAuctioned => _ownershipAmount == 0;
    }
}