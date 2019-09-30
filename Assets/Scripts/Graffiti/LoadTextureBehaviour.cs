using System.IO;
using System.Text;
using UnityEngine;

namespace Graffiti
{
    public class LoadTextureBehaviour : MonoBehaviour
    {
        public TextureBehaviour textureBehaviour;

        public void Start()
        {
            if (File.Exists(Application.persistentDataPath + "/SavedImage.csv"))
            {
                ReadTextureFromFile();
            }
        }

        private void ReadTextureFromFile()
        {
            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var lines = Encoding.UTF8.GetString(rawBytes)
                .Split('\n');

            foreach (var line in lines)
            {
                if (line == "") continue;
                var pairs = line.Split(',');
                textureBehaviour.LitPoints.Add(new Vector2(int.Parse(pairs[0]), int.Parse(pairs[1])));
            }
        }
    }
}