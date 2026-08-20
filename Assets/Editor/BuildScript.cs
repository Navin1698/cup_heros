#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    private static string[] GetScenes()
    {
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

        Debug.Log($"Collected {scenes.Length} scene(s) for build:");
        foreach (var scene in scenes)
        {
            Debug.Log($" - {scene}");
        }

        return scenes;
    }

    private static void ExecuteBuild(BuildPlayerOptions buildPlayerOptions)
    {
        Debug.Log($"Starting build to: {buildPlayerOptions.locationPathName} (Target: {buildPlayerOptions.target})");
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

    public static void BuildAndroid()
    {
        Debug.Log("=== Starting Android Build ===");
        string buildPath = "build/Android/Game1-Android.apk";
        string buildDir = Path.GetDirectoryName(buildPath);
        if (!string.IsNullOrEmpty(buildDir) && !Directory.Exists(buildDir))
        {
            Directory.CreateDirectory(buildDir);
        }

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        PlayerSettings.companyName = "DefaultCompany";
        PlayerSettings.productName = "cup_heros";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.DefaultCompany.cup_heros");

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        ExecuteBuild(options);
    }

    public static void BuildWindows()
    {
        Debug.Log("=== Starting Windows Standalone Build ===");
        string buildPath = "build/Windows/Game1.exe";
        string buildDir = Path.GetDirectoryName(buildPath);
        if (!string.IsNullOrEmpty(buildDir) && !Directory.Exists(buildDir))
        {
            Directory.CreateDirectory(buildDir);
        }

        PlayerSettings.companyName = "DefaultCompany";
        PlayerSettings.productName = "cup_heros";

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        ExecuteBuild(options);
    }

    public static void BuildWebGL()
    {
        Debug.Log("=== Starting WebGL Build ===");
        string buildPath = "build/WebGL";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        PlayerSettings.companyName = "DefaultCompany";
        PlayerSettings.productName = "cup_heros";

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        ExecuteBuild(options);
    }
}
#endif
