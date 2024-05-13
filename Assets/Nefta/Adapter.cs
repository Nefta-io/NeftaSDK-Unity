using System.Text;
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
                _eventBuilder.Append(JavaScriptStringEncode(gameEvent._name));
            }
            if (gameEvent._customString != null)
            {
                _eventBuilder.Append("\",\"custom_publisher_payload\":\"");
                _eventBuilder.Append(JavaScriptStringEncode(gameEvent._customString));
            }
            _eventBuilder.Append("\"}");
            var eventString = _eventBuilder.ToString();
            Plugin.Record(eventString);
        }
        
        private static string JavaScriptStringEncode(string value)
        {
            int len = value.Length;
            bool needEncode = false;
            char c;
            for (int i = 0; i < len; i++)
            {
                c = value [i];

                if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
                {
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
            {
                return value;
            }
            
            var sb = new StringBuilder ();
            for (int i = 0; i < len; i++)
            {
                c = value [i];
                if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                {
                    sb.AppendFormat ("\\u{0:x4}", (int)c);
                }
                else switch ((int)c)
                {
                    case 8:
                        sb.Append ("\\b");
                        break;

                    case 9:
                        sb.Append ("\\t");
                        break;

                    case 10:
                        sb.Append ("\\n");
                        break;

                    case 12:
                        sb.Append ("\\f");
                        break;

                    case 13:
                        sb.Append ("\\r");
                        break;

                    case 34:
                        sb.Append ("\\\"");
                        break;

                    case 92:
                        sb.Append ("\\\\");
                        break;

                    default:
                        sb.Append (c);
                        break;
                }
            }
            return sb.ToString ();
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

