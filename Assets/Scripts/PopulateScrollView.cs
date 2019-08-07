using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopulateScrollView : MonoBehaviour
{
    public GameObject scrollItemPrefab;

    public GameObject scrollContentGrid;
    // Start is called before the first frame update
    public void Start()
    {
        Instantiate(new GameObject(), scrollContentGrid.transform);
    }
}
