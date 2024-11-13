using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using Nefta.Data;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

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
        
#if UNITY_IOS
        [PostProcessBuild(0)]
        public static void NeftaPostProcessPlist(BuildTarget buildTarget, string path)
        {
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new UnityEditor.iOS.Xcode.PlistDocument();
            plist.ReadFromFile(plistPath);

            plist.root.values.TryGetValue("SKAdNetworkItems", out var skAdNetworkItems);
            var existingSkAdNetworkIds = new HashSet<string>();

            if (skAdNetworkItems != null && skAdNetworkItems.GetType() == typeof(UnityEditor.iOS.Xcode.PlistElementArray))
            {
                var plistElementDictionaries = skAdNetworkItems.AsArray().values.Where(plistElement => plistElement.GetType() == typeof(UnityEditor.iOS.Xcode.PlistElementDict));
                foreach (var plistElement in plistElementDictionaries)
                {
                    UnityEditor.iOS.Xcode.PlistElement existingId;
                    plistElement.AsDict().values.TryGetValue("SKAdNetworkIdentifier", out existingId);
                    if (existingId == null || existingId.GetType() != typeof(UnityEditor.iOS.Xcode.PlistElementString) || string.IsNullOrEmpty(existingId.AsString())) continue;

                    existingSkAdNetworkIds.Add(existingId.AsString());
                }
            }
            else
            {
                skAdNetworkItems = plist.root.CreateArray("SKAdNetworkItems");
            }

            const string neftaSkAdNetworkId = "2lj985962l.adattributionkit";
            if (!existingSkAdNetworkIds.Contains(neftaSkAdNetworkId)) {
                var skAdNetworkItemDict = skAdNetworkItems.AsArray().AddDict();
                skAdNetworkItemDict.SetString("SKAdNetworkIdentifier", neftaSkAdNetworkId);
            }

            plist.WriteToFile(plistPath);
        }
#endif
        
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
            var guids = AssetDatabase.FindAssets("NeftaPlugin");
            string pluginPath = null;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".m"))
                {
                    if (pluginPath != null)
                    {
                        _error = "Multiple instances of NeftaPlugin_iOS found in project";
                        return;
                    }
                    pluginPath = Path.GetDirectoryName(path);
                }
            }
            if (pluginPath == null)
            {
                _error = "iOS NeftaPlugin not found in project";
                return;
            }
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pluginPath + "/NeftaSDK.xcframework/Info.plist");
            var dict = xmlDoc.ChildNodes[2].ChildNodes[0];
            for (var i = 0; i < dict.ChildNodes.Count; i++)
            {
                if (dict.ChildNodes[i].InnerText == "Version")
                {
                    _iosVersion = dict.ChildNodes[i + 1].InnerText;
                    break;
                }
            }
        }
        
        [MenuItem("Window/Select Nefta Configuration", false, int.MaxValue)]
        private static void SelectNeftaConfiguration()
        {
            var configuration = GetNeftaConfiguration();
            if (configuration == null)
            {
                configuration = CreateInstance<NeftaConfiguration>();
                
                var directory = "Assets/Resources";
                var assetPath = $"{directory}/{NeftaConfiguration.FileName}.asset";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                AssetDatabase.CreateAsset(configuration, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Selection.objects = new UnityEngine.Object[] { configuration };
            EditorUtility.FocusProjectWindow();
        }
        
        private static NeftaConfiguration GetNeftaConfiguration()
        {
            NeftaConfiguration configuration = null;
            
            string[] guids = AssetDatabase.FindAssets("t:NeftaConfiguration");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                configuration = AssetDatabase.LoadAssetAtPath<NeftaConfiguration>(path);
            }

            return configuration;
        }
    }
}