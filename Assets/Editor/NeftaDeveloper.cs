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
            
            NeftaWindow._debugPluginImporter.SetCompatibleWithPlatform(BuildTarget.Android, true);
            NeftaWindow._debugPluginImporter.SaveAndReimport();
            
            NeftaWindow._releasePluginImporter.SetCompatibleWithPlatform(BuildTarget.Android, false);
            NeftaWindow._releasePluginImporter.SaveAndReimport();
            
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
