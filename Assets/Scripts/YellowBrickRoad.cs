using System.Collections.Generic;
using UnityEngine;

public class YellowBrickRoad : MonoBehaviour
{
    private LineRenderer _road;

    public void Start()
    {
        _road = gameObject.GetComponent<LineRenderer>();
    }

    public void SetPath(GameObject[] vertices)
    {
        _road.positionCount = vertices.Length;
        for (var i = 0; i < vertices.Length; i++)
        {
            _road.SetPosition(i, vertices[i].transform.position);
        }
    }
}