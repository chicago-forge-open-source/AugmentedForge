using UnityEngine;

namespace Assets.Scripts
{
    public interface PhysicsHandler
    {
        T Raycast<T>(Ray ray) where T : class;
    }
}