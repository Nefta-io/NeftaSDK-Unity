using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nefta.Core.Data
{
    public class NeftaConfiguration : ScriptableObject
    {
        public const string FileName = "NeftaConfiguration";

        [FormerlySerializedAs("_applicationId")] public string _androidAppId;
        public string _iOSAppId;
        
        [SerializeReference] public List<NeftaModuleConfiguration> _configurations;

        public bool _isEventRecordingEnabledOnStart;
        public bool _isLoggingEnabled;
    }
}