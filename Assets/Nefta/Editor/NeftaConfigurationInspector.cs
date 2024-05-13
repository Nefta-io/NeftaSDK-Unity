using System.IO;
using System.IO.Compression;
using System.Xml;
using Nefta.Data;
using UnityEditor;
using UnityEngine;
using UnityEditor.iOS.Xcode;

namespace Nefta.Editor
{
    [CustomEditor(typeof(NeftaConfiguration), false)]
    public class NeftaConfigurationInspector : UnityEditor.Editor
    {
        private NeftaConfiguration _configuration;
        private bool _isLoggingEnabled;
        
        private string _error;
        private string _androidVersion;
        private string _iosVersion;
        
        public void OnEnable()
        {
            _configuration = (NeftaConfiguration)target;
            _isLoggingEnabled = _configuration._isLoggingEnabled;
            
            _error = null;
#if UNITY_2021_1_OR_NEWER
            GetAndroidVersions();
#endif
            GetIosVersions();
        }

        public void OnDisable()
        {
            _configuration = null;
        }

        public override void OnInspectorGUI()
        {
            if (_error != null)
            {
                EditorGUILayout.LabelField(_error, EditorStyles.helpBox);
                return;
            }
            
#if UNITY_2021_1_OR_NEWER
            if (_androidVersion != _iosVersion)
            {
                DrawVersion("Nefta SDK Android version", _androidVersion);
                EditorGUILayout.Space(5);
                DrawVersion("Nefta SDK iOS version", _iosVersion);
            }
            else
#endif
            {
                DrawVersion("Nefta SDK version", _androidVersion);
            }
            EditorGUILayout.Space(5);
            
            base.OnInspectorGUI();
            if (_isLoggingEnabled != _configuration._isLoggingEnabled)
            {
                _isLoggingEnabled = _configuration._isLoggingEnabled;

                EnableLogging(_isLoggingEnabled);
            }
        }
        
        public static void EnableLogging(bool enable)
        {
            var importer = GetImporter(true);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, enable);
            importer.SaveAndReimport();
                
            importer = GetImporter(false);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, !enable);
            importer.SaveAndReimport();
        }
        
        private static PluginImporter GetImporter(bool debug)
        {
            var guid = AssetDatabase.FindAssets(debug ? "NeftaPlugin-debug" : "NeftaPlugin-release")[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return (PluginImporter) AssetImporter.GetAtPath(path);
        }
        
        private static void DrawVersion(string label, string version)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label); 
            EditorGUILayout.LabelField(version, EditorStyles.boldLabel, GUILayout.Width(60)); 
            EditorGUILayout.EndHorizontal();
        }
        
#if UNITY_2021_1_OR_NEWER
        private void GetAndroidVersions()
        {
            var guids = AssetDatabase.FindAssets("NeftaPlugin-");
            if (guids.Length == 0)
            {
                _error = "NeftaPlugin AARs not found in project";
                return;
            }
            if (guids.Length > 2)
            {
                _error = "Multiple instances of NeftaPlugin AARs found in project";
                return;
            }
            var aarPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            using ZipArchive aar = ZipFile.OpenRead(aarPath);
            ZipArchiveEntry manifestEntry = aar.GetEntry("AndroidManifest.xml");
            if (manifestEntry == null)
            {
                _error = "Nefta SDK AAR seems to be corrupted";
                return;
            }
            using Stream manifestStream = manifestEntry.Open();
            XmlDocument manifest = new XmlDocument();
            manifest.Load(manifestStream);
            var root = manifest.DocumentElement;
            if (root == null)
            {
                _error = "Nefta SDK AAR seems to be corrupted";
                return;
            }
            _androidVersion = root.Attributes["android:versionName"].Value;
        }
#endif
        
        private void GetIosVersions()
        {
            var guids = AssetDatabase.FindAssets("NeftaSDK.xcframework");
            if (guids.Length == 0)
            {
                _error = "NeftaSDK.xcframework not found in project";
                return;
            }
            if (guids.Length > 1)
            {
                _error = "Multiple instances of NeftaSDK.xcframework found in project";
                return;
            }
            var frameworkPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            var plist = new PlistDocument();
            plist.ReadFromFile(frameworkPath + "/Info.plist");
            _iosVersion = plist.root["Version"].AsString();
        }
    }
}