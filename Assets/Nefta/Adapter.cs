using System.Text;
using System.Web;
using Nefta.Data;
using Nefta.Events;
using UnityEngine;
using UnityEngine.Assertions;

namespace Nefta
{
    public class Adapter
    {
        private StringBuilder _eventBuilder;
        
        public static Adapter Instance;

        public NeftaPluginWrapper Plugin { get; private set; }
        
        public static Adapter Init() 
        {
            if (Instance != null)
            {
                return Instance;
            }

            Instance = new Adapter();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChange;
#endif
            var configuration = Resources.Load<NeftaConfiguration>(NeftaConfiguration.FileName);
            Assert.IsNotNull(configuration, "Missing NeftaConfiguration ScriptableObject");
            var gameObject = new GameObject("_NeftaPlugin");
            Instance.Plugin = gameObject.AddComponent<NeftaPluginWrapper>();
            NeftaPluginWrapper.EnableLogging(configuration._isLoggingEnabled);
#if UNITY_IOS
            var appId = configuration._iOSAppId;
            NeftaPluginWrapper.EnableLogging(configuration._isLoggingEnabled);
#else
            var appId = configuration._androidAppId;
#endif
            Instance.Plugin.Init(appId);
            Instance._eventBuilder = new StringBuilder(128);

            return Instance;
        }

        public void Record(GameEvent gameEvent)
        {
            _eventBuilder.Clear();
            _eventBuilder.Append("{\"event_type\":\"");
            _eventBuilder.Append(gameEvent._eventType);
            _eventBuilder.Append("\",\"event_category\":\"");
            _eventBuilder.Append(gameEvent._category);
            _eventBuilder.Append("\",\"value\":");
            _eventBuilder.Append(gameEvent._value.ToString());
            _eventBuilder.Append(",\"event_sub_category\":\"");
            _eventBuilder.Append(gameEvent._subCategory);
            if (gameEvent._name != null)
            {
                _eventBuilder.Append("\",\"item_name\":\"");
                _eventBuilder.Append(HttpUtility.JavaScriptStringEncode(gameEvent._name));
            }
            if (gameEvent._customString != null)
            {
                _eventBuilder.Append("\",\"custom_publisher_payload\":\"");
                _eventBuilder.Append(HttpUtility.JavaScriptStringEncode(gameEvent._customString));
            }
            _eventBuilder.Append("\"}");
            var eventString = _eventBuilder.ToString();
            Plugin.Record(eventString);
        }
        
#if UNITY_EDITOR
        private static void OnPlayModeChange(UnityEditor.PlayModeStateChange playMode)
        {
            if (playMode == UnityEditor.PlayModeStateChange.EnteredEditMode)
            {
                Instance = null;
            }
        }
#endif
    }
}

