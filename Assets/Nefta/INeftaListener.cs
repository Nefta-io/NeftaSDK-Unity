#if !UNITY_EDITOR
namespace Nefta
{
    public interface INeftaListener
    {
        void IOnReady(string configuration);
        void IOnBid(string pId, float price, int expirationTime);
        void IOnLoadStart(string pId);
        void IOnLoadFail(string pId, int code, string error);
        void IOnLoad(string pId, int width, int height);
        void IOnShowFail(string pId, int code, string error);
        void IOnShow(string pId);
        void IOnClick(string pId);
        void IOnReward(string pId);
        void IOnClose(string pId);

        void IOnBehaviourInsight(string behaviourInsight);
    }
}
#endif