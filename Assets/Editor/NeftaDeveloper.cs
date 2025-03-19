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
            NeftaWindow.TogglePlugins(false);

            var packageName = $"NeftaAdSDK_{Application.version}.unitypackage";
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
