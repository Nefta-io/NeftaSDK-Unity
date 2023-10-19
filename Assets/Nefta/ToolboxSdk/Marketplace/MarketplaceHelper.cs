using System;
using Nefta.Core;

namespace Nefta.ToolboxSdk.Marketplace
{
    public class MarketplaceHelper
    {
        private Action<bool> _createAuction;
        private Action<AuctionsResponse> _onGetProjectAuctions;
        private Action<bool> _onCreateAuctionBid;
        private Action<bool> _onCreateAuctionPurchase;

        /// <summary>
        /// Creates a gamer auction with given ownership id and start price.
        /// </summary>
        /// <param name="ownershipId"></param>
        /// /// <param name="startPrice"></param>
        /// <param name="purchasePrice"></param>
        /// <param name="onCreateAuction"></param>
        public void CreateAuction(string ownershipId, float startPrice, float purchasePrice, Action<bool> onCreateAuction)
        {
            _createAuction += onCreateAuction;
            if (_createAuction.GetInvocationList().Length > 1)
            {
                return;
            }
            
            var data = new CreateAuctionRequest
            {
                _ownershipId = ownershipId,
                _auctionType = "standard_auction",
                _startPrice = startPrice,
                _purchasePrice = purchasePrice
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest("/gamer/auction", body, OnCreateAuction);
        }

        private void OnCreateAuction(RestResponse restResponse)
        {
            _createAuction(restResponse.IsSuccess);
            _createAuction = null;
        }

        /// <summary>
        /// Returns a list of project auctions.
        /// </summary>
        /// <returns></returns>
        public void GetProjectAuctions(Action<AuctionsResponse> onGetProjectAuctions)
        {
            _onGetProjectAuctions += onGetProjectAuctions;
            if (_onGetProjectAuctions.GetInvocationList().Length > 1)
            {
                return;
            }
            
            var endPoint = "/auctions?mid=" + Toolbox.Instance.Configuration._marketplaceId;     
            NeftaCore.Log("Getting Marketplace Auctions");
            Toolbox.Instance.RestHelper.SendGetRequest(endPoint, OnGetProjectAuctions);
        }

        private void OnGetProjectAuctions(RestResponse restResponse)
        {
            AuctionsResponse auctions = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    auctions = Toolbox.Instance.RestHelper.Deserialize<AuctionsResponse>(restResponse.Body);
                    foreach (var auction in auctions._results)
                    {
                        auction._asset.Init();
                    }
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing auctions: {e.Message}");
                }
            }
            _onGetProjectAuctions(auctions);
            _onGetProjectAuctions = null;
        }

        /// <summary>
        /// Creates an auction big with given bid price.
        /// </summary>
        /// <param name="auction"></param>
        /// <param name="price"></param>
        /// <param name="onCreateAuctionBid"></param>
        /// <returns></returns>
        public void CreateAuctionBid(Auction auction, float price, Action<bool> onCreateAuctionBid)
        {
            _onCreateAuctionBid += onCreateAuctionBid;
            if (_onCreateAuctionBid.GetInvocationList().Length > 1)
            {
                return;
            }
            
            var endPoint = "/gamer/auction/" + auction._auctionId + "/bid";
            var data = new CreateAuctionBidRequest
            {
                _bidPrice = price
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest(endPoint, body, OnCreateAuctionBid);
        }

        private void OnCreateAuctionBid(RestResponse restResponse)
        {
            _onCreateAuctionBid(restResponse.IsSuccess);
            _onCreateAuctionBid = null;
        }

        /// <summary>
        /// Creates an auction purchase.
        /// </summary>
        /// <param name="auction"></param>
        /// <param name="onCreateAuctionPurchase"></param>
        /// <returns></returns>
        public void CreateAuctionPurchase(Auction auction, Action<bool> onCreateAuctionPurchase)
        {
            _onCreateAuctionPurchase += onCreateAuctionPurchase;
            if (_onCreateAuctionPurchase.GetInvocationList().Length > 1)
            {
                return;
            }
            
            var endPoint = "/gamer/auction/" + auction._auctionId + "/purchase";
            NeftaCore.Log("Create auction purchase");
            Toolbox.Instance.RestHelper.SendPostRequest(endPoint, new byte[] {}, OnCreateAuctionPurchase);
        }

        private void OnCreateAuctionPurchase(RestResponse restResponse)
        {
            _onCreateAuctionPurchase(restResponse.IsSuccess);
            _onCreateAuctionPurchase = null;
        }
    }
}