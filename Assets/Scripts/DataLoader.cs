using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader
{
    public void DataLoad()
    {
        var markers = LoadMarkers();
        Repositories.MarkerRepository.Save(markers);
    }

    private Marker[] LoadMarkers()
    {
        var makerSpace = new Marker(
            "Maker Space",
            30.371f,
            -26.29f
        );

        var crucible = new Marker(
            "Maker Space",
            27.5f,
            -11.44f
        );

        var focusRoom = new Marker(
            "Maker Space",
            27.5f,
            -8.03f
        );

        var kitchen = new Marker(
            "Maker Space",
            43.175f,
            -1.144f
        );

        var greaterMHub = new Marker(
            "Maker Space",
            28.875f,
            9.262001f
        );

        return new[]
        {
            makerSpace, crucible, focusRoom, kitchen, greaterMHub
        };
    }
}