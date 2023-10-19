namespace Nefta.AdSdk
{
    public class ProxyPlacement
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
    }
}