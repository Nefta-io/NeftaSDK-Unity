namespace Nefta.AdSdk
{
    public class Placement
    {
        public enum ImpressionType
        {
            Banner,
            Interstitial,
            VideoAd
        }
        
        public ImpressionType _type;
        public string _id;
        public int _width;
        public int _height;
        public int _renderedWidth;
        public int _renderedHeight;

        public bool _isBidding;
        public bool _isLoading;
        public BidResponse _availableBid;
        public BidResponse _bufferBid;
        public BidResponse _renderedBid;
        
        public bool CanLoad => _availableBid != null && !_isLoading;
        public bool CanShow => _bufferBid != null && !_isLoading;

        public Placement(ImpressionType type, string id, int width, int height)
        {
            _type = type;
            _id = id;
            _width = width;
            _height = height;
        }
    }
}