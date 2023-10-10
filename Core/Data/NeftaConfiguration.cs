using System.Collections.Generic;
using UnityEngine;

namespace Nefta.Core.Data
{
    public class NeftaConfiguration : ScriptableObject
    {
        public const string FileName = "NeftaConfiguration";

        public string _applicationId;
        
        [SerializeReference] public List<NeftaModuleConfiguration> _configurations;

        public bool _isEventRecordingEnabledOnStart;
    }
}