namespace Nefta
{
#if !UNITY_EDITOR
    public interface INeftaListener
    {
        void IOnReady(string configuration);
        void IOnBid(string pId, float price);
        void IOnLoadStart(string pId);
        void IOnLoadFail(string pId, string error);
        void IOnLoad(string pId);
        void IOnShow(string pId, int width, int height);
        void IOnBannerChange(string pId, int width, int height);
        void IOnClick(string pId);
        void IOnReward(string pId);
        void IOnClose(string pId);
    }
#endif
}