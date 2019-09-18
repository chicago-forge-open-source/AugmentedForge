using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Graffiti;
using UnityEngine;

namespace Editor
{
    public static class Export
    {
        [MenuItem("Export/IoTGoGoGo")]
        public static async Task DoIoTThing()
        {
            await new GraffitiCanvas().UpdateGraffitiCanvasText("No Text Entered");
        }

        [MenuItem("Export/TestGit")]
        public static void LogVersion()
        {
            var thing = CollectTagVersion();

            Debug.Log("version " + thing);
        }

        private static int CollectTagVersion()
        {
            var processStartInfo = new ProcessStartInfo("/usr/bin/git", "describe --abbrev=0")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process process = Process.Start(processStartInfo);
            process?.WaitForExit();

            return Int32.Parse(process.StandardOutput.ReadToEnd());
        }

        [MenuItem("Export/Android")]
        public static void ExportAndroidAab()
        {
            var scenes = new[] {"Assets/Scenes/InitScene.unity", "Assets/Scenes/ARView.unity"};
            const string path = "./android-output/AugmentedForge.aab";

            EditorUserBuildSettings.buildAppBundle = true;

            var tagVersion = CollectTagVersion();
            PlayerSettings.Android.bundleVersionCode = tagVersion;
            PlayerSettings.bundleVersion = $"0.0.{tagVersion}";

            var pass = Environment.GetEnvironmentVariable("KEYSTORE_PASS");
            if (pass != null)
            {
                PlayerSettings.keystorePass = pass;
                PlayerSettings.keyaliasPass = pass;
            }

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);

            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.bundleVersion = $"1";
        }
    }
}