namespace AdDemo
{
    public interface IPlacement
    {
        void OnBid();
        void OnLoadStart();
        void OnLoadFail(string error);
        void OnLoad();
        void OnShow();
        void OnShowFail(string error);
        void OnClose();
    }
}