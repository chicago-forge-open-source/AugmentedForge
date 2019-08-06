using System.Collections;
using DataLoaders;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeApp : MonoBehaviour
{
    public ICompass compass = new RealCompass();
    public DataLoader dataLoader;

    public void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();
    }

    void Start () {
        Branch.initSession(CallbackWithBranchUniversalObject);
    }

    void CallbackWithBranchUniversalObject(BranchUniversalObject buo,
        BranchLinkProperties linkProps,
        string error) {
        if (error != null) {
            System.Console.WriteLine("Error : "
                                     + error);
        } else if (linkProps.controlParams.Count > 0)
        {

            System.Console.WriteLine("BUO YOU-O " + buo.keywords + " " + buo.canonicalUrl +
        " " + buo.metadata.addressPostalCode)

        ;
            System.Console.WriteLine("Deeplink params : "
                                     + buo.ToJsonString()
                                     + linkProps.ToJsonString());
        }
    }
    
    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => compass.IsEnabled);
        SceneManager.LoadScene("ARView");
    }

    public void OnClick_LoadLocationARView(string location)
    {
        if (location.Equals("Iowa"))
        {
            dataLoader = new IowaDataLoader();
        }
        else
        {
            dataLoader = new ChicagoDataLoader();
        }

        dataLoader.DataLoad();
        PlayerPrefs.SetString("location", location);
        StartCoroutine(WaitForCompassEnable());
    }
}