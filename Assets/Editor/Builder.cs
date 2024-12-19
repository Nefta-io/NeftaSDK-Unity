using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor
{
    public class Builder
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
        {
            if (target == BuildTarget.iOS)
            {
                string plistPath = Path.Combine(pathToBuildProject, "Info.plist");
                var plist = new UnityEditor.iOS.Xcode.PlistDocument();
                plist.ReadFromFile(plistPath);
                plist.root.SetString("NSUserTrackingUsageDescription", "This allows us to deliver personalized ads and content.");
                plist.WriteToFile(plistPath);
            }
        }
        
        private static void Build(BuildTarget target, string outPath, bool asProject)
        {
            var scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                scenes.Add(scene.path);
            }
            var options = new BuildPlayerOptions
            {
                scenes = scenes.ToArray(),
                locationPathName = outPath,
                target = target,
                options = BuildOptions.StrictMode
            };
            
            EditorUserBuildSettings.exportAsGoogleAndroidProject = asProject;
            
            var report = BuildPipeline.BuildPlayer(options);

            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build successful");
            }
            else if (report.summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build failed");
            }
        }

        public static void BuildAndroid()
        {
            Build(BuildTarget.Android, "out_Android.apk", false);
        }
        
        public static void BuildAndroidProject()
        {
            Build(BuildTarget.Android, "out_tAndroid", true);
        }
        
        public static void Buildios()
        {
            Build(BuildTarget.iOS, "out_iOS", false);
        }

    }
}