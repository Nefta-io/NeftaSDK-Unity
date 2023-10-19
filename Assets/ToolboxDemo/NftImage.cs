using Nefta.ToolboxSdk.Nft;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ToolboxDemo
{
    public class NftImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [FormerlySerializedAs("_size")] [SerializeField] private float _targetSize;

        public void SetNft(NftAsset asset)
        {
            if (asset.IsAssetLoaded)
            {
                OnAssetLoaded(asset);
            }
            else
            {
                asset.LoadAsset(OnAssetLoaded);
            }
        }
        
        private void OnAssetLoaded(NftAsset asset)
        {
            _image.sprite = asset._assetSprite;
            var size = asset._assetSprite.rect.size;
            if (size.x > size.y)
            {
                size.y = _targetSize / size.x * size.y;
                size.x = _targetSize;
            }
            else
            {
                size.x = _targetSize / size.y * size.x;
                size.y = _targetSize;
            }
            ((RectTransform)_image.transform).sizeDelta = size;

        }
    }
}
