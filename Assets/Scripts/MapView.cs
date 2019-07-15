using UnityEngine;
using UnityEngine.SceneManagement;

public class MapView : MonoBehaviour
{
    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = AppDelegate.GetMapSprite();
    }

    public void OnClick_LoadARView(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
