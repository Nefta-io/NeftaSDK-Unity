using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Nefta.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Nefta.ToolboxSdk.Nft
{
    [Serializable]
    public class NftAsset
    {

        private static readonly string[] _supportedExtensions = new[]
        {
            ".png", ".jpeg", ".jpg", ".gif", ".bmp", ".tga", ".tiff", ".hdr"
        };

        [DataMember(Name = "asset_id")] public string _id;
        [DataMember(Name = "asset_name")] public string _name;
        [DataMember(Name = "image")] public string _imageUrl;
        [DataMember(Name = "asset_type")] public string _assetType;
        [DataMember(Name = "available_instances")] public int _availableInstances;
        [DataMember(Name = "instances")] public int _instances;
        [DataMember(Name = "traits")] public Dictionary<string, string> _traits;
        [DataMember(Name = "asset_characteristics")] public NftCharacteristics _characteristics;

        [IgnoreDataMember] public Sprite _assetSprite;
        private UnityWebRequest _request;
        private Action<NftAsset> _onAssetLoadedCallbacks;

        public bool IsAssetLoaded => _assetSprite != null;

        public void Init()
        {
            if (Toolbox.Instance.Configuration._preloadStrategy != Toolbox.PreloadStrategies.None)
            {
                TryLoadCachedAsset();
            }

            if (_assetSprite == null && Toolbox.Instance.Configuration._preloadStrategy == Toolbox.PreloadStrategies.Full)
            {
                StartDownloadingAsset();
            }
        }

        public void LoadAsset(Action<NftAsset> callback)
        {
            if (_assetSprite != null)
            {
                callback(this);
                return;
            }

            TryLoadCachedAsset();
            if (_assetSprite != null)
            {
                callback(this);
                return;
            }

            if (callback != null)
            {
                _onAssetLoadedCallbacks += callback;
            }
            StartDownloadingAsset();
        }

        private void TryLoadCachedAsset()
        {
            var texture = Toolbox.Instance.LoadCachedAsset(this);
            if (texture != null)
            {
                CreateSprite(texture);
            }
        }

        /// <summary>
        /// Download a texture and convert into sprite. Updates a sprite in NFTAsset object.
        /// </summary>
        private void StartDownloadingAsset()
        {
            if (_request != null)
            {
                return;
            }
            
            _request = UnityWebRequestTexture.GetTexture(_imageUrl);

            var extension = Path.GetExtension(_imageUrl);
            bool isSupported = false;
            foreach (var supportedExtension in _supportedExtensions)
            {
                if (extension == supportedExtension)
                {
                    isSupported = true;
                    break;
                }
            }
            if (!isSupported)
            {
                NeftaCore.Warn($"Unsupported image format \"{extension}\" for \"{_imageUrl}\"");
                return;
            }

            var requestOperation = _request.SendWebRequest();
            requestOperation.completed += RequestOperationOnCompleted;
        }

        private void RequestOperationOnCompleted(AsyncOperation obj)
        {
            if (_request.result == UnityWebRequest.Result.Success)
            {
                NeftaCore.Log($"Successfully loaded {_name}");
                
                var texture = (Texture2D) ((DownloadHandlerTexture)_request.downloadHandler).texture;

                Toolbox.Instance.CacheAsset(this, texture);
                CreateSprite(texture);
                _onAssetLoadedCallbacks?.Invoke(this);
                _onAssetLoadedCallbacks = null;
            }
            else
            {
                NeftaCore.Log($"Error loading {_name}: {_request.error}");
            }

            _request = null;
        }

        private void CreateSprite(Texture2D texture)
        {
            _assetSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
    }
}