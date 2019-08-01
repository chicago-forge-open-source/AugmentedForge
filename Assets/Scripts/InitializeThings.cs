using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeThings : MonoBehaviour
{
    public ICompass compass = new RealCompass();
    
    public void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        new DataLoader().DataLoad();
    }

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => compass.IsEnabled);
        SceneManager.LoadScene("ARView");
    }

    public void OnClick_LoadLocationARView(string location)
    {
        PlayerPrefs.SetString("location", location);
        StartCoroutine(WaitForCompassEnable());
    }
}