using System;
using System.IO;
using Nefta.Core;
using Nefta.Core.Data;
using Nefta.ToolboxSdk.Authorization;
using Nefta.ToolboxSdk.GamerAssets;
using Nefta.ToolboxSdk.GamerManagement;
using Nefta.ToolboxSdk.Marketplace;
using Nefta.ToolboxSdk.Nft;
using UnityEngine;

namespace Nefta.ToolboxSdk
{
    public class Toolbox : INeftaModule
    {
        public enum PreloadStrategies
        {
            None,
            OnlyFromCache,
            Full
        }

        private NeftaCore _neftaCore;
        internal NeftaUser _neftaUser;

        public static Toolbox Instance;

        public ToolboxConfiguration Configuration;
        public RestHelper RestHelper;
        public AuthorizationHelper Authorization;
        public GamerManagementHelper GamerManagement;
        public GamerAssetsHelper GamerAssets;
        public MarketplaceHelper Marketplace;

        public static void Init()
        {
            var neftaCore = NeftaCore.Init();

            Instance = new Toolbox();
            Instance.Configuration = neftaCore.GetConfiguration<ToolboxConfiguration>();
            Instance.RestHelper = new RestHelper();
            Instance.Authorization = new AuthorizationHelper(Instance);
            Instance.GamerManagement = new GamerManagementHelper();
            Instance.GamerAssets = new GamerAssetsHelper();
            Instance.Marketplace = new MarketplaceHelper();
            
            Instance._neftaCore = neftaCore;
            Instance._neftaUser = neftaCore.GetUser();
        }

        public NeftaUser GetUser()
        {
            return _neftaUser;
        }

        public void SetUser(NeftaUser neftaUser)
        {
            _neftaUser = neftaUser;
            
            NeftaCore.Instance.SetUser(_neftaUser);
        }

        public void SetAddress(string address)
        {
            _neftaUser._address = address;
            
            NeftaCore.Instance.SetUser(_neftaUser);
        }
        
        private static string GetAssetDirectory()
        {
            return $"{Application.persistentDataPath}/NEFTAToolboxSDK/NFTAssets";
        }

        public static void ClearCache()
        {
            var path = GetAssetDirectory();
            if (!Directory.Exists(path))
            {
                return;
            }
            Directory.Delete(path, true);
        }
        
        private string GetAssetPath(NftAsset asset)
        {
            var nameHash = new Hash128();
            nameHash.Append(asset._imageUrl);
            return $"{GetAssetDirectory()}/{nameHash.ToString()}";
        }

        public Texture2D LoadCachedAsset(NftAsset asset)
        {
            Texture2D texture = null;
            var path = GetAssetPath(asset);
            try
            {
                if (!File.Exists(path))
                {
                    return null;
                }

                var textureBytes = File.ReadAllBytes(path);
                texture = new Texture2D(2, 2);
                texture.LoadImage(textureBytes);
            }
            catch (Exception e)
            {
                NeftaCore.Warn($"Error loading texture {asset._name} from {path}: {e.Message}");
            }

            return texture;
        }
        
        public void CacheAsset(NftAsset asset, Texture2D texture)
        {
            var path = GetAssetPath(asset);
            try
            {
                var directory = GetAssetDirectory();
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var textureBytes = texture.EncodeToPNG();
                File.WriteAllBytes(path, textureBytes);
                NeftaCore.Info($"Saved {asset._name} to {path}");
            }
            catch (Exception e)
            {
                NeftaCore.Warn($"Error saving {asset._name} to {path}: {e.Message}");
            }
        }
    }
}