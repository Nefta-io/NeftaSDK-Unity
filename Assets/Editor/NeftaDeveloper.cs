using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NeftaDeveloper : MonoBehaviour
    {
        [MenuItem("Nefta developer/Export packages/Ad SDK")]
        private static void ExportAdSdkPackage()
        {
            var assetPaths = new string[] {
                "Assets/Nefta/Core",
                "Assets/Nefta/AdSdk"
            };
            
            ExportPackage(assetPaths, $"NeftaAdSDK_v{Application.version}.unitypackage");
        }
        
        [MenuItem("Nefta developer/Export packages/Toolbox SDK")]
        private static void ExportToolboxSdkPackage()
        {
            var assetPaths = new string[] {
                "Assets/Nefta/Core",
                "Assets/Nefta/ToolboxSdk"
            };
            
            ExportPackage(assetPaths, $"NeftaToolboxSDK_v{Application.version}.unitypackage");
        }
        
        [MenuItem("Nefta developer/Open export location")]
        private static void OpenExportLocation()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }
        
        private static void ExportPackage(string[] assetPaths, string packageName)
        {
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
