using System;
using UnityEngine;

public interface PhysicsHandler
{
    Tuple<T, Vector3> Raycast<T>(Ray ray) where T : class;
}