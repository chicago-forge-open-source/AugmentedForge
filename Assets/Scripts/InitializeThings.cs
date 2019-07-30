using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class InitializeThings : MonoBehaviour
    {
        public ICompass Compass = new RealCompass();
    
        public void Awake()
        {
            Input.compass.enabled = true;
            Input.location.Start();
            new DataLoader().DataLoad();
        }

        private IEnumerator WaitForCompassEnable()
        {
            yield return new WaitUntil(() => Compass.IsEnabled);
            SceneManager.LoadScene("ARView");
        }

        public void OnClick_LoadLocationARView(string location)
        {
            PlayerPrefs.SetString("location", location);
            StartCoroutine(WaitForCompassEnable());
        }
    }
}


