using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NeftaDeveloper : MonoBehaviour
    {
        [MenuItem("Nefta developer/Regenerate resolvers")]
        private static void RegenerateResolvers()
        {
            RegenerateResolversForAssembly("Nefta.Core", "Assets/Nefta/Core/CoreResolvers.cs", "CoreResolvers");
            RegenerateResolversForAssembly("Nefta.AdSdk", "Assets/Nefta/AdSdk/AdResolvers.cs", "AdResolvers");
            RegenerateResolversForAssembly("Nefta.ToolboxSdk", "Assets/Nefta/ToolboxSdk/ToolboxResolvers.cs", "ToolboxResolvers");
        }

        [MenuItem("Nefta developer/Export packages/Ad SDK")]
        private static void ExportAdSdkPackage()
        {
            var assetPaths = new string[] {
                "Assets/Nefta/Core",
                "Assets/Nefta/AdDemo",
                "Assets/Nefta/AdSdk"
            };
            
            ExportPackage(assetPaths, $"NeftaAdSDK_v{Application.version}.unitypackage");
        }

        [MenuItem("Nefta developer/Delete event data")]
        private static void DeleteEventData()
        {
            PlayerPrefs.DeleteKey("nefta.core.eventPrefs");
        }
        
        [MenuItem("Nefta developer/Export packages/Toolbox SDK/Default")]
        private static void ExportToolboxSdkPackageDefault()
        {
            ExportToolboxSdkPackage(false);
        }
        
        [MenuItem("Nefta developer/Export packages/Toolbox SDK/VSA")]
        private static void ExportToolboxSdkPackageVsa()
        {
            ExportToolboxSdkPackage(true);
        }
        
        [MenuItem("Nefta developer/Open export location")]
        private static void OpenExportLocation()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }
        
        private static void ExportToolboxSdkPackage(bool isVsa)
        {
            // Don't include integration test, since they require Unity Test Tools package
            AssetDatabase.MoveAsset("Assets/Nefta/ToolboxDemo/Test", "Test");
            AssetDatabase.MoveAsset("Assets/Nefta/ToolboxDemo/Test.meta", "Test.meta");

            var assetPaths = new string[] {
                "Assets/Nefta/Core",
                "Assets/Nefta/ToolboxDemo",
                "Assets/Nefta/ToolboxSdk"
            };

            if (isVsa)
            {
                ExportPackage(assetPaths, $"NeftaToolboxSDK_VSA_v{Application.version}.unitypackage");
            }
            else
            {
                // For SDK distributed outside Unity ecosystem don't include Verified Solution Attribution
                AssetDatabase.MoveAsset("Assets/Nefta/ToolboxSDK/Editor/VSAttribution.cs", "VSAttribution.cs");
                AssetDatabase.MoveAsset("Assets/Nefta/ToolboxSDK/Editor/VSAttribution.cs.meta", "VSAttribution.cs.meta");
                string editorWindowPath = "Assets/Nefta/ToolboxSDK/Editor/NeftaToolboxEditor.cs";
                var editorWindowLines = File.ReadAllLines(editorWindowPath);
                StripVsaContent(editorWindowLines, editorWindowPath);

                ExportPackage(assetPaths, $"NeftaToolboxSDK_v{Application.version}.unitypackage");
                
                AssetDatabase.MoveAsset("VSAttribution.cs", "Assets/Nefta/Core/Editor/VSAttribution.cs");
                AssetDatabase.MoveAsset("VSAttribution.cs.meta", "Assets/Nefta/Core/Editor/VSAttribution.cs.meta");
                File.WriteAllLines(editorWindowPath, editorWindowLines);
            }

            AssetDatabase.MoveAsset("Test", "Assets/Nefta/ToolboxDemo/Test");
            AssetDatabase.MoveAsset("Test.meta", "Assets/Nefta/ToolboxDemo/Test.meta");
        }

        private static void RegenerateResolversForAssembly(string assemblyName, string resolverPath, string name)
        {
            var paths = "";
            var addSeparator = false;
            var assembly = GetAssembly(assemblyName);
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0)
                {
                    var className = type.Name;
                    var guids = AssetDatabase.FindAssets(className);
                    if (guids.Length == 0)
                    {
                        Debug.Log($"Missing filepath for class {className}");
                        continue;
                    }

                    var path = "";
                    foreach (var guid in guids)
                    {
                        path = AssetDatabase.GUIDToAssetPath(guid);
                        if (Path.GetFileNameWithoutExtension(path) == className)
                        {
                            break;
                        }
                    }
                    
                    if (addSeparator)
                    {
                        paths += ",";
                    }
                    paths += "../" + path;
                    addSeparator = true;
                }
            }

            var projectPath = Directory.GetParent(Application.dataPath).FullName;
            var utf8Path = projectPath + "/Utf8Json";
            var filePath = projectPath + "/" + resolverPath;
            var arguments = $"-i \"{paths}\" -o \"{filePath}\" -r \"{name}\" -n \"{assemblyName}\"";
            Debug.Log($"Generating resolvers..");
            
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = utf8Path + "/Utf8Json.UniversalCodeGenerator";
            processInfo.WorkingDirectory = utf8Path;
            processInfo.Arguments = arguments;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.UseShellExecute = false;
            var process = System.Diagnostics.Process.Start(processInfo);
            var output = process.StandardOutput.ReadToEnd();
            Debug.Log(output);
            
            string generatedCode = File.ReadAllText(filePath);
            generatedCode = generatedCode.Replace("\r\n", "\n");
            File.WriteAllText(filePath, generatedCode);
            
            AssetDatabase.Refresh();
        }

        private static void StripVsaContent(string[] lines, string fileName)
        {
            var strippedLines = new string[lines.Length - 3];
            var i = 0;
            foreach (var line in lines)
            {
                if (!line.Contains("VSAttribution") && !line.Contains("UnityActionName") && !line.Contains("UnityPartnerIdentifier"))
                {
                    strippedLines[i] = line;
                    i++;
                }
            }
            File.WriteAllLines(fileName, strippedLines);
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

        private static Assembly GetAssembly(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == name)
                {
                    return assembly;
                }
            }

            return null;
        }
    }   
}
