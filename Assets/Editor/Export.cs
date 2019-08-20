using System;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Editor
{
    public static class Export
    {
        [MenuItem("Export/Android")]
        public static void ExportAndroidAab()
        {
            var scenes = new[] {"Assets/Scenes/InitScene.unity", "Assets/Scenes/ARView.unity"};
            const string path = "./android-output/AugmentedForge.aab";

            Debug.Log("bundle version " + PlayerSettings.bundleVersion);
            Debug.Log("bundle version code" + PlayerSettings.Android.bundleVersionCode);
            
            
            
            EditorUserBuildSettings.buildAppBundle = true;            
            
            var pass = Environment.GetEnvironmentVariable("KEYSTORE_PASS");
            if (pass != null)
            {
                PlayerSettings.keystorePass = pass;
                PlayerSettings.keyaliasPass = pass;
            }

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
    }
}