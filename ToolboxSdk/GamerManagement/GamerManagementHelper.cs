using System;
using System.Collections.Generic;
using Nefta.Core;

namespace Nefta.ToolboxSdk.GamerManagement
{
    public class GamerManagementHelper
    {
        private static readonly Queue<Action<OwnedAssetsResponse>> OnAssetsLoadedQueue = new Queue<Action<OwnedAssetsResponse>>();

        private Action<GamerProfile> _onGetGamerProfile;
        private Action<NativeCurrencyBalance> _onGetGamerCryptoBalance;
        private Action<CurrencyBalance> _onGetGamerCurrencyBalance;
        
        /// <summary>
        /// Returns a gamer profile.
        /// </summary>
        /// <param name="onGetGamerProfile"></param>
        /// <returns></returns>
        public void GetGamerProfile(Action<GamerProfile> onGetGamerProfile)
        {
            _onGetGamerProfile += onGetGamerProfile;
            if (_onGetGamerProfile.GetInvocationList().Length > 1)
            {
                return;
            }
            
            NeftaCore.Log("Getting User Profile");
            Toolbox.Instance.RestHelper.SendGetRequest("/gamer/profile", OnGetGamerProfile);
        }

        private void OnGetGamerProfile(RestResponse restResponse)
        {
            GamerProfile profile = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    profile = Toolbox.Instance.RestHelper.Deserialize<GamerProfile>(restResponse.Body);
                    Toolbox.Instance.User._address = profile._address;
                    NeftaCore.Instance.SaveUser();
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing gamer profile: {e.Message}");
                }
            }
            _onGetGamerProfile(profile);
            _onGetGamerProfile = null;
        }

        /// <summary>
        /// Return a list of assets owned by given user.
        /// </summary>
        /// <returns></returns>
        public void GetGamerOwnedAssets(Action<OwnedAssetsResponse> onAssetsLoaded)
        {
            NeftaCore.Log("Getting User Inventory");
            OnAssetsLoadedQueue.Enqueue(onAssetsLoaded);
            Toolbox.Instance.RestHelper.SendGetRequest("/gamer/assets", OnAssetsLoaded);
        }

        private void OnAssetsLoaded(RestResponse restResponse)
        {
            var callback = OnAssetsLoadedQueue.Dequeue();
            OwnedAssetsResponse ownedAssetsResponse = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    ownedAssetsResponse = Toolbox.Instance.RestHelper.Deserialize<OwnedAssetsResponse>(restResponse.Body);
                    foreach (var ownedAsset in ownedAssetsResponse._results)
                    {
                        ownedAsset._nft.Init();
                    }
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing owned assets: {e.Message}");
                }
            }
            
            callback(ownedAssetsResponse);
        }

        /// <summary>
        /// Returns a crypto currency balance of user.
        /// </summary>
        /// <returns></returns>
        public void GetGamerCryptoBalance(Action<NativeCurrencyBalance> onGetGamerCryptoBalance)
        {
            NeftaCore.Log("Getting User Crypto Balance");
            _onGetGamerCryptoBalance += onGetGamerCryptoBalance;
            if (_onGetGamerCryptoBalance.GetInvocationList().Length > 1)
            {
                return;
            }
            Toolbox.Instance.RestHelper.SendGetRequest("/gamer/currencies", GetGamerCryptoBalance);
        }

        private void GetGamerCryptoBalance(RestResponse restResponse)
        {
            NativeCurrencyBalance balance = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    balance = Toolbox.Instance.RestHelper.Deserialize<NativeCurrencyBalance>(restResponse.Body);
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing gamer crypto balance: {e.Message}");
                }
            }

            _onGetGamerCryptoBalance(balance);
            _onGetGamerCryptoBalance = null;
        }

        /// <summary>
        /// Returns a currency balance of user by given currency id.
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="onGetGamerCurrencyBalance"></param>
        /// <returns></returns>
        public void GetGamerCurrencyBalance(string currencyId, Action<CurrencyBalance> onGetGamerCurrencyBalance)
        {
            _onGetGamerCurrencyBalance += onGetGamerCurrencyBalance;
            if (_onGetGamerCurrencyBalance.GetInvocationList().Length > 1)
            {
                return;
            }
            
            NeftaCore.Log("Getting User Custom Currency Balance");
            Toolbox.Instance.RestHelper.SendGetRequest("/gamer/currencies/" + currencyId, OnGetGamerCurrencyBalance);
        }

        private void OnGetGamerCurrencyBalance(RestResponse restResponse)
        {
            CurrencyBalance currencyBalance = null;
            if (restResponse.IsSuccess)
            {
                try
                {
                    currencyBalance = Toolbox.Instance.RestHelper.Deserialize<CurrencyBalance>(restResponse.Body);
                }
                catch (Exception e)
                {
                    NeftaCore.Warn($"Error parsing currency balance: {e.Message}");
                }
            }

            _onGetGamerCurrencyBalance(currencyBalance);
            _onGetGamerCurrencyBalance = null;
        }
        
    }
}