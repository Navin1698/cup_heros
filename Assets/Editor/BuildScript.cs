#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        Debug.Log("=== Starting Android Build ===");

        string buildPath = "build/Android/Game1-Android.apk";
        string buildDir = Path.GetDirectoryName(buildPath);
        if (!string.IsNullOrEmpty(buildDir) && !Directory.Exists(buildDir))
        {
            Directory.CreateDirectory(buildDir);
        }

        // Configure Android Player Settings
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        PlayerSettings.companyName = "DefaultCompany";
        PlayerSettings.productName = "cup_heros";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.DefaultCompany.cup_heros");

        // Collect scenes from EditorBuildSettings or search for scene files in Assets
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled && !string.IsNullOrEmpty(s.path))
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogWarning("No enabled scenes found in EditorBuildSettings. Scanning Assets/_Game/Scenes...");
            if (Directory.Exists("Assets/_Game/Scenes"))
            {
                scenes = Directory.GetFiles("Assets/_Game/Scenes", "*.unity", SearchOption.AllDirectories)
                    .Select(p => p.Replace('\\', '/'))
                    .ToArray();
            }
        }

        Debug.Log($"Building with {scenes.Length} scene(s):");
        foreach (var scene in scenes)
        {
            Debug.Log($" - {scene}");
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        Debug.Log($"Invoking BuildPipeline.BuildPlayer -> {buildPath}");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        Debug.Log($"=== Build Finished with Result: {summary.result} ===");
        Debug.Log($"Total Errors: {summary.totalErrors}, Warnings: {summary.totalWarnings}, Duration: {summary.totalTime.TotalSeconds:F1}s, Size: {summary.totalSize} bytes");

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build completed successfully!");
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError($"Build failed with result: {summary.result}");
            foreach (var step in report.steps)
            {
                foreach (var message in step.messages)
                {
                    if (message.type == LogType.Error || message.type == LogType.Exception)
                    {
                        Debug.LogError($"[BuildStep: {step.name}] {message.content}");
                    }
                }
            }
            EditorApplication.Exit(1);
        }
    }
}
#endif
