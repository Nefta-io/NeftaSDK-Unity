namespace Nefta.Ads
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

        public enum Position
        {
            Top = 0,
            Bottom = 1
        }
        
        public Type _type;
        public string _id;
        internal int _renderedWidth;
        internal int _renderedHeight;
        public bool _isShown;

        public bool _isBidding;
        public bool _isLoading;
        public float? _availableBid;
        public float? _bufferBid;
        public float? _renderedBid;
        public Mode _mode;
        public int _expirationTime;
        public float _auctionTime;

        public int Width => _isShown ? _renderedWidth : 0;

        public int Height => _isShown ? _renderedHeight : 0;

        public bool CanLoad => !_isLoading;
        public bool CanShow
        {
            get
            {
                if (_bufferBid == null || _isLoading)
                {
                    return false;
                }
                if (_expirationTime > 0 && UnityEngine.Time.realtimeSinceStartup - _auctionTime > _expirationTime)
                {
                    return false;
                }
                return true;
            }
        }

        public Placement(Type type, string id)
        {
            _type = type;
            _id = id;
        }
    }
}