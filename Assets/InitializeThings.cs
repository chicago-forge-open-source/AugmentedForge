using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeThings : MonoBehaviour
{
    public void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        
        StartCoroutine(WaitForCompassEnable());
    }

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => Input.compass.enabled);
        SceneManager.LoadScene("MapScene");
    }

}
