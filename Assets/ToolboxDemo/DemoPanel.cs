using UnityEngine;

namespace ToolboxDemo
{
    public class DemoPanel : MonoBehaviour
    {
        protected MasterPanel _masterPanel;
        protected StatusPanel _statusPanel;
        
        public void Init(MasterPanel masterPanel, StatusPanel statusPanel)
        {
            _masterPanel = masterPanel;
            _statusPanel = statusPanel;

            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
        
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}