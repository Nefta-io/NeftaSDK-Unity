using UnityEngine;
using UnityEngine.Serialization;

namespace Nefta.Data
{
    public class NeftaConfiguration : ScriptableObject
    {
        public const string FileName = "NeftaConfiguration";

        [FormerlySerializedAs("_applicationId")] public string _androidAppId;
        public string _iOSAppId;
        
        public bool _isLoggingEnabled;
    }
}