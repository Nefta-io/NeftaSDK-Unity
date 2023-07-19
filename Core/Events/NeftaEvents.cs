using System;
using System.Collections.Generic;
using Nefta.Core.Resolvers;
using UnityEngine;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Nefta.Core.Events
{
    public class NeftaEvents : IJsonFormatterResolver
    {
        private const string _sequenceNumberPrefKey = "nefta.core.sequenceNumber";
        private const string _analyticsVersion = "1.0.0";
        
        private static bool _isEnabled;

        private List<IJsonFormatterResolver> _resolvers;

        private int _sequenceNumber;
        private NeftaCore _neftaCore;
        private string _language;
        private string _platform;
        private string _deviceType;
        private string _deviceMake;
        private string _hardwareVersion;
        private string _deviceOs;
        private int _screenWidth;
        private int _screenHeight;
        private int _dpi;
        private float _ratio;
        private string _appId;

        private List<byte[]> _events;

        public static NeftaEvents Instance;

        /// <summary>
        /// Call this function to enable or disable tracking of the analytic events
        /// </summary>
        /// <param name="enable">True to enable and False to disable</param>
        public static void Enable(bool enable)
        {
            _isEnabled = enable;
            
            if (Instance == null && _isEnabled)
            {
                Instance = new NeftaEvents();

                Instance._resolvers = new List<IJsonFormatterResolver>()
                {
                    StandardResolver.Default,
                    CoreResolvers.Instance,
                };

                Instance._neftaCore = NeftaCore.Init();

                Instance._sequenceNumber = PlayerPrefs.GetInt(_sequenceNumberPrefKey);
                Instance._language = Application.systemLanguage.ToString();
                Instance._platform = Application.platform.ToString();
                Instance._deviceType = SystemInfo.deviceType.ToString();
                Instance._deviceMake = SystemInfo.deviceModel;
                Instance._hardwareVersion = SystemInfo.graphicsDeviceVersion + SystemInfo.graphicsShaderLevel;
                Instance._deviceOs = SystemInfo.operatingSystem;
                Instance._screenWidth = Screen.width;
                Instance._screenHeight = Screen.height;
                Instance._dpi = (int)Screen.dpi;
                Instance._ratio = (float)Screen.height / Screen.width;
                Instance._appId = Application.identifier;

                Instance._events = new List<byte[]>();
            }
        }

        public static void Record(InterestEvent interestEvent)
        {
            var trackingEvent = interestEvent.GetRecordedEvent();
            trackingEvent._sequenceNumber = Instance._sequenceNumber;
            trackingEvent._userId = Instance._neftaCore.NeftaUser._userId;
            trackingEvent._time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            trackingEvent._isTest = false;
            trackingEvent._appVersion = Application.version;
            trackingEvent._eventVersion = _analyticsVersion;
            trackingEvent._sdkVersion = NeftaCore.SdkVersion;
                
            trackingEvent._language = Instance._language;
            trackingEvent._platform = Instance._platform;
            trackingEvent._deviceType = Instance._deviceType;
            trackingEvent._deviceMake = Instance._deviceMake;
            trackingEvent._hardwareVersion = Instance._hardwareVersion;
            trackingEvent._deviceOs = Instance._deviceOs;
            trackingEvent._screenWidth = Instance._screenWidth;
            trackingEvent._screenHeight = Instance._screenHeight;
            trackingEvent._dpi = Instance._dpi;
            trackingEvent._ratio = Instance._ratio;
            trackingEvent._appId = Instance._appId;
            
            Instance._sequenceNumber++;
            
            var trackingEventS = JsonSerializer.Serialize(trackingEvent, CoreResolvers.Instance);
            Instance._events.Add(trackingEventS);
        }
        
        public byte[] Serialize<T>(T body)
        {
            return JsonSerializer.Serialize(body, this);
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
    }
}