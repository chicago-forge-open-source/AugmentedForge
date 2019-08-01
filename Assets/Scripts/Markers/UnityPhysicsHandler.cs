using Assets.Scripts;
using UnityEngine;

namespace Markers
{
    public class UnityPhysicsHandler : PhysicsHandler
    {
        public T Raycast<T>(Ray ray) where T : class
        {
            return Physics.Raycast(ray, out var raycastHit)
                ? raycastHit.transform.GetComponent<T>()
                : default;
        }
    }
}