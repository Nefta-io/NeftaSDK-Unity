using UnityEngine;
#if UNITY_EDITOR
using Nefta.Editor;
#endif

namespace Nefta
{
    public class NeftaPluginListener : AndroidJavaProxy, INeftaListener
    {
        public NeftaPluginListener() : base("com.nefta.sdk.Unity.CallbackInterface")
        {
        }
        
        public virtual void IOnReady(string configuration)
        {
        }

        public virtual void IOnBid(string pId, float price, int expirationTime)
        {
        }

        public virtual void IOnLoadStart(string pId)
        {
        }
        
        public virtual void IOnLoadFail(string pId, int code, string error)
        {
        }

        public virtual void IOnLoad(string pId, int width, int height)
        {
        }

        public virtual void IOnShowFail(string pId, int code, string message)
        {
        }
        
        public virtual void IOnShow(string pId)
        {
        }
        
        public virtual void IOnClick(string pId)
        {
        }
        
        public virtual void IOnReward(string pId)
        {
        }
        
        public virtual void IOnClose(string pId)
        {
        }

        public virtual void IOnBehaviourInsight(int id, string behaviourInsight)
        {
            
        }
    }
}