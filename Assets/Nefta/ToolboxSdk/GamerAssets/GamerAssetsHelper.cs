using System;
using Nefta.Core;

namespace Nefta.ToolboxSdk.GamerAssets
{
    public class GamerAssetsHelper
    {
        private Action<bool> _onCreateCurrencyRewardForUser;
        private Action<bool> _onCreateAssetRewardForUser;

        /// <summary>
        /// Creates a currency reward for user with the given currency id and amount.
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="currencyAmount"></param>
        /// <param name="onCreateCurrencyRewardForUser"></param>
        /// <returns></returns>
        public void CreateCurrencyRewardForUser(string currencyId, float currencyAmount, Action<bool> onCreateCurrencyRewardForUser)
        {
            _onCreateCurrencyRewardForUser += onCreateCurrencyRewardForUser;
            if (_onCreateCurrencyRewardForUser.GetInvocationList().Length > 1)
            {
                return;
            }
            
            NeftaCore.Log("Reward Custom Currency to user");
            var data = new RewardUserCurrencyRequest
            {
                _amount = currencyAmount
            };
            var body = Toolbox.Instance.RestHelper.Serialize(data);
            Toolbox.Instance.RestHelper.SendPostRequest($"/gamer/currency/{currencyId}/reward", body, OnCreateCurrencyRewardForUser);
        }

        private void OnCreateCurrencyRewardForUser(RestResponse restResponse)
        {
            _onCreateCurrencyRewardForUser(restResponse.IsSuccess);
            _onCreateCurrencyRewardForUser = null;
        }

        /// <summary>
        /// Gift an asset to a user.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="onCreateAssetRewardForUser"></param>
        /// <returns></returns>
        public void CreateAssetRewardForUser(string assetId, Action<bool> onCreateAssetRewardForUser)
        {
            _onCreateAssetRewardForUser += onCreateAssetRewardForUser;
            if (_onCreateAssetRewardForUser.GetInvocationList().Length > 1)
            {
                return;
            }
            
            var endPoint = "/gamer/asset/" + assetId + "/reward";
            Toolbox.Instance.RestHelper.SendPostRequest(endPoint, System.Text.Encoding.UTF8.GetBytes("{}"), OnCreateAssetRewardForUser);
        }

        private void OnCreateAssetRewardForUser(RestResponse restResponse)
        {
            _onCreateAssetRewardForUser(restResponse.IsSuccess);
            _onCreateAssetRewardForUser = null;
        }
    }
}