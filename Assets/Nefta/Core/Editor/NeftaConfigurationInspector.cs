using Nefta.Core.Data;
using UnityEditor;

namespace Nefta.Core.Editor
{
    [CustomEditor(typeof(NeftaConfiguration), false)]
    public class NeftaConfigurationInspector : UnityEditor.Editor
    {
        private NeftaConfiguration _configuration;
        private bool _isLoggingEnabled;
        
        public void OnEnable()
        {
            _configuration = (NeftaConfiguration)target;
            _isLoggingEnabled = _configuration._isLoggingEnabled;
        }

        public void OnDisable()
        {
            _configuration = null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_isLoggingEnabled != _configuration._isLoggingEnabled)
            {
                _isLoggingEnabled = _configuration._isLoggingEnabled;
                NeftaEditorWindow.EnableLogging(_isLoggingEnabled);
            }
        }
    }
}