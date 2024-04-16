using System.Collections.Generic;
using System.Text;
using Nefta.Core.Data;
using Nefta.Core.Events;
using Nefta.Core.Resolvers;
using UnityEngine;
using UnityEngine.Assertions;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Nefta.Core
{
    public class NeftaCore : IJsonFormatterResolver
    {
        private List<IJsonFormatterResolver> _resolvers;
        private NeftaUser _neftaUser;
        private NeftaConfiguration _configuration;

        public static NeftaCore Instance;

        public NeftaPluginWrapper Plugin { get; private set; }

        public static void EnableLogging(bool enable)
        {
            NeftaPluginWrapper.EnableLogging(enable);
        }
        
        public static NeftaCore Init() 
        {
            if (Instance != null)
            {
                return Instance;
            }

            Instance = new NeftaCore();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChange;
#endif
            
            Instance._resolvers = new List<IJsonFormatterResolver>()
            {
                StandardResolver.Default,
                CoreResolvers.Instance
            };
            
            Instance._configuration = Resources.Load<NeftaConfiguration>(NeftaConfiguration.FileName);
            Assert.IsNotNull(Instance._configuration, "Missing NeftaConfiguration ScriptableObject");
            if (Instance._configuration._isLoggingEnabled)
            {
                EnableLogging(true);
            }
            var gameObject = new GameObject("_NeftaPlugin");
            Instance.Plugin = gameObject.AddComponent<NeftaPluginWrapper>();
#if UNITY_ANDROID
            var appId = Instance._configuration._androidAppId;
#else
            var appId = Instance._configuration._iOSAppId;
#endif
            Instance.Plugin.Init(appId);

            return Instance;
        }

        public void Record(GameEvent gameEvent)
        {
            var recordedEvent = gameEvent.GetRecordedEvent();
            var recordedEventB = JsonSerializer.Serialize(recordedEvent, CoreResolvers.Instance);
            var recordedEventS = Encoding.UTF8.GetString(recordedEventB);
            Plugin.Record(recordedEventS);
        }

        public T GetConfiguration<T>() where T : NeftaModuleConfiguration
        {
            foreach (var configuration in _configuration._configurations)
            {
                if (configuration is T moduleConfiguration)
                {
                    return moduleConfiguration;
                }
            }

            return default;
        }
        
        public byte[] Serialize<T>(T body)
        {
            return JsonSerializer.Serialize(body, this);
        }
        
        public T Deserialize<T>(byte[] json)
        {
            var reader = new JsonReader(json);
            return JsonSerializer.Deserialize<T>(ref reader, this);
        }

        public IJsonFormatter<T> GetFormatter<T>()
        {
            foreach (var item in _resolvers)
            {
                var f = item.GetFormatter<T>();
                if (f != null)
                {
                    return f;
                }
            }
            return null;
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

        [System.Diagnostics.Conditional("NEFTA_SDK_DBG")]
        public static void Info(string log)
        {
            Debug.Log($"[Nefta]f{Time.frameCount} Info: {log}");
        }
        
        [System.Diagnostics.Conditional("NEFTA_SDK_DBG")]
        public static void Log(string log)
        {
            Debug.Log($"[Nefta]f{Time.frameCount} Log: {log}");
        }
        
        [System.Diagnostics.Conditional("NEFTA_SDK_DBG")]
        public static void Warn(string log)
        {
            Debug.LogWarning($"[Nefta]f{Time.frameCount} Warn: {log}");
        }
    }
}

