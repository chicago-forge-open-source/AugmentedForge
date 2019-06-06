using System.IO;
using UnityEditor;

namespace Editor
{
    static class Autobuilder
    {
        [MenuItem("File/AutoBuilder/Android")]
        static void PerformBuild()
        {
            string[] scenes = {"./Assets/GoogleARCore/AugmentedForge/Scenes/MapScene.unity"};

            string buildPath = "./Build/Android";

            // Create build folder if not yet exists
            Directory.CreateDirectory(buildPath);

            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.Development);
        }
    }
}