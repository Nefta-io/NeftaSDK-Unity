using System;
using Nefta.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NeftaDeveloper : MonoBehaviour
    {
        [MenuItem("Nefta developer/Export Package")]
        private static void ExportPackage()
        {
            NeftaWindow.TryGetPluginImporters();
            NeftaWindow.TogglePlugins(true);

            var packageName = $"NeftaAdSDK_{Application.version}.unitypackage";
            
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
                AssetDatabase.ExportPackage(new [] { "Assets/Nefta" }, packageName, ExportPackageOptions.Recurse);
                Debug.Log($"Finished exporting {packageName}");   
            }
            catch (Exception e)
            {
                Debug.LogError($"Error exporting {packageName}: {e.Message}");   
            }
        }
        
        [MenuItem("Nefta developer/Open export location")]
        private static void OpenExportLocation()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }
    }   
}
