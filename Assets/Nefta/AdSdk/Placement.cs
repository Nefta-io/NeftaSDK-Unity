namespace Nefta.AdSdk
{
    public class Placement
    {
        public enum Type
        {
            Banner = 0,
            Interstitial = 1,
            VideoAd = 2
        }

        public enum Mode
        {
            Manual = 0,
            ScheduledBid = 1,
            ScheduledLoad = 2,
            Continuous = 3
        }
        
        public Type _type;
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
        public Mode _mode;
        
        public bool CanLoad => !_isLoading;
        public bool CanShow => _bufferBid != null && !_isLoading;

        public Placement(Type type, string id, int width, int height)
        {
            _type = type;
            _id = id;
            _width = width;
            _height = height;
        }
    }
}