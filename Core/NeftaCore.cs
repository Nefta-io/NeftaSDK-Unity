using UnityEngine;

namespace Nefta.Core
{
    public class NeftaCore
    {
        private const string PrefPrefix = "nefta.core.";
        private const string UsernameKey = "username";
        private const string UserTokenKey = "user_token";
        private const string UserIdKey = "user_id";
        private const string AddressKey = "address";
        private const string EmailKey = "email";
        
        public const string BaseUrl = "https://api.Nefta.io/v2.0";

        private NeftaUser _neftaUser;

        public static NeftaCore Instance;
        
        public Analytics Analytics { get; private set; }

        public NeftaUser NeftaUser
        {
            get => _neftaUser;
            set
            {
                _neftaUser = value;
                SaveUser();
            }
        }

        public static NeftaCore Init() 
        {
            if (Instance == null)
            {
                Instance = new NeftaCore();
                Instance.Analytics = new Analytics();

                Instance.LoadUser();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.playModeStateChanged += Instance.OnPlayModeChange;
#endif
            }

            return Instance;
        }
        
        private void LoadUser()
        {
            var usernameKey = PrefPrefix + UsernameKey;
            if (PlayerPrefs.HasKey(usernameKey))
            {
                _neftaUser = new NeftaUser
                {
                    _username = PlayerPrefs.GetString(usernameKey),
                    _token = PlayerPrefs.GetString(PrefPrefix + UserTokenKey),
                    _userId = PlayerPrefs.GetString(PrefPrefix + UserIdKey),
                    _address = PlayerPrefs.GetString(PrefPrefix + AddressKey),
                    _email = PlayerPrefs.GetString(PrefPrefix + EmailKey)
                };
            }
            else if (PlayerPrefs.HasKey(UsernameKey))
            {
                _neftaUser = new NeftaUser
                {
                    _username = PlayerPrefs.GetString(UsernameKey),
                    _token = PlayerPrefs.GetString(UserTokenKey),
                    _userId = PlayerPrefs.GetString(UserIdKey),
                    _address = PlayerPrefs.GetString(AddressKey),
                    _email = PlayerPrefs.GetString(EmailKey)
                };
                SaveUser();
            }
        }
        
        public void SaveUser()
        {
            PlayerPrefs.SetString(PrefPrefix + UsernameKey, NeftaUser._username);
            PlayerPrefs.SetString(PrefPrefix + UserTokenKey, NeftaUser._token);
            PlayerPrefs.SetString(PrefPrefix + UserIdKey, NeftaUser._userId);
            PlayerPrefs.SetString(PrefPrefix + EmailKey, NeftaUser._email);
            PlayerPrefs.SetString(PrefPrefix + AddressKey, NeftaUser._address);
        }

        public static void ClearPrefs()
        {
            if (PlayerPrefs.HasKey(UsernameKey))
            {
                PlayerPrefs.DeleteKey(UsernameKey);
                PlayerPrefs.DeleteKey(UserTokenKey);
                PlayerPrefs.DeleteKey(UserIdKey);
                PlayerPrefs.DeleteKey(EmailKey);
                PlayerPrefs.DeleteKey(AddressKey);
            }

            var usernameKey = PrefPrefix + UsernameKey;
            if (PlayerPrefs.HasKey(usernameKey))
            {
                PlayerPrefs.DeleteKey(usernameKey);
                PlayerPrefs.DeleteKey(PrefPrefix + UserTokenKey);
                PlayerPrefs.DeleteKey(PrefPrefix + UserIdKey);
                PlayerPrefs.DeleteKey(PrefPrefix + EmailKey);
                PlayerPrefs.DeleteKey(PrefPrefix + AddressKey);
            }
        }
        
#if UNITY_EDITOR
        private void OnPlayModeChange(UnityEditor.PlayModeStateChange playMode)
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
            Debug.Log($"[NEFTA] Info: {log}");
        }
        
        [System.Diagnostics.Conditional("NEFTA_SDK_DBG")]
        public static void Log(string log)
        {
            Debug.Log($"[NEFTA] Log: {log}");
        }
        
        [System.Diagnostics.Conditional("NEFTA_SDK_DBG")]
        public static void Warn(string log)
        {
            Debug.LogWarning($"[NEFTA] Warn: {log}");
        }
    }
}

