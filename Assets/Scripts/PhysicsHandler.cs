using UnityEngine;

public interface PhysicsHandler
{
    T Raycast<T>(Ray ray) where T : class;
}