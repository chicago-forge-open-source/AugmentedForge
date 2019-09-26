using System;
using UnityEngine;

namespace Markers
{
    public class UnityPhysicsHandler : PhysicsHandler
    {
        public Tuple<T, Vector3> Raycast<T>(Ray ray) where T : class
        {
            return Physics.Raycast(ray, out var raycastHit)
                ? Tuple.Create(raycastHit.transform.GetComponent<T>(), raycastHit.point)
                : Tuple.Create<T, Vector3>(default, default);
        }
    }
}