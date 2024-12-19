using UnityEngine;

namespace Nefta
{
    public class AdUnit
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
            None = 0,
            Top = 1,
            Bottom = 2
        }
        
        public enum State
        {
            Request = 0,
            Initialized = 1,
            Bidding = 2,
            ReadyToLoad = 3,
            Loading = 4,
            Ready = 5,
            Showing = 6,
            Hidden = 7,
            Expired = 8
        }
        
        public Type _type;
        public string _id;
        internal int _renderedWidth;
        internal int _renderedHeight;

        public State _state;
        public float _bid;
        public int _expirationTime;
        public float _auctionTime;

        public int Width => _state == State.Showing ? _renderedWidth : 0;

        public int Height => _state == State.Showing ? _renderedHeight : 0;

        public bool CanLoad => _state == State.Initialized || _state == State.ReadyToLoad;
        public bool CanShow
        {
            get
            {
                if (_bid < 0 || _state != State.Ready)
                {
                    return false;
                }
                if (_expirationTime > 0 && Time.realtimeSinceStartup - _auctionTime > _expirationTime)
                {
                    _state = State.Expired;
                    return false;
                }
                return true;
            }
        }

        public AdUnit(Type type, string id)
        {
            _type = type;
            _id = id;
            _state = State.Initialized;
        }
    }
}