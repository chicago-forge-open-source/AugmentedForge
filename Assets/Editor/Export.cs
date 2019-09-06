using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Amazon.IotData;
using Amazon.IotData.Model;

namespace Editor
{
    public static class Export
    {
        [MenuItem("Export/IoTGoGoGo")]
        public static async Task DoIoTThing()
        {
            Debug.Log("Started do thing");

            var awsAccessKeyId = "AKIA6OWBH347S6K5AGZR";
            var awsSecretAccessKey = "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";
            var amazonIotDataConfig = new AmazonIotDataConfig
            {
                ServiceURL = "https://a2soq6ydozn6i0-ats.iot.us-west-2.amazonaws.com/"
            };
            using (var dataClient = new AmazonIotDataClient(awsAccessKeyId, awsSecretAccessKey, amazonIotDataConfig))
            {
                var publishRequest = new PublishRequest
                {
                    Topic = "$aws/things/Flounder/shadow/update",
                    Payload = new MemoryStream(
                        System.Text.Encoding.UTF8.GetBytes(
                            "{ \"state\" : { \"desired\" : { \"flavor\":\"snozzberry\"} } }"
                        )
                    ),
                    Qos = 1
                };

                Debug.Log("Before start new ");
                Debug.Log("Before publish");
                await dataClient.PublishAsync(publishRequest);

                Debug.Log("After publish");
            }
        }

        private static async Task dumbTest()
        {
            Debug.Log("dumb test");
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
            PlayerSettings.bundleVersion = $"LOCAL";
        }
    }
}