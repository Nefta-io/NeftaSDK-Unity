using System;
using Nefta.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NeftaDeveloper : MonoBehaviour
    {
        [MenuItem("Nefta developer/Export packages/Ad SDK")]
        private static void ExportAdSdkPackage()
        {
            NeftaEditorWindow.EnableLogging(true);
            
            var assetPaths = new string[] {
                "Assets/Nefta/Core",
                "Assets/Nefta/AdSdk"
            };
            
            ExportPackage(assetPaths, $"NeftaAdSDK_{Application.version}.unitypackage");
        }
        
        [MenuItem("Nefta developer/Export packages/Toolbox SDK")]
        private static void ExportToolboxSdkPackage()
        {
            var assetPaths = new string[] {
                "Assets/Nefta/Core",
                "Assets/Nefta/ToolboxSdk"
            };
            
            ExportPackage(assetPaths, $"NeftaToolboxSDK_{Application.version}.unitypackage");
        }
        
        [MenuItem("Nefta developer/Open export location")]
        private static void OpenExportLocation()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }
        
        private static void ExportPackage(string[] assetPaths, string packageName)
        {
            var guid = AssetDatabase.FindAssets("NeftaPlugin-debug")[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = (PluginImporter) AssetImporter.GetAtPath(path);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
            importer.SaveAndReimport();
            
            guid = AssetDatabase.FindAssets("NeftaPlugin-release")[0];
            path = AssetDatabase.GUIDToAssetPath(guid);
            importer = (PluginImporter) AssetImporter.GetAtPath(path);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
            importer.SaveAndReimport();
            
            try
            {
                AssetDatabase.ExportPackage(assetPaths, packageName, ExportPackageOptions.Recurse);
                Debug.Log($"Finished exporting {packageName}");   
            }
            catch (Exception e)
            {
                Debug.LogError($"Error exporting {packageName}: {e.Message}");   
            }
        }
    }   
}
