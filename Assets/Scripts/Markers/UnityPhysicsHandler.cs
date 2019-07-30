using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class UnityPhysicsHandler : PhysicsHandler
    {
        public T Raycast<T>(Ray ray) where T : class
        {
            RaycastHit putItHere;
            if (Physics.Raycast(ray, out putItHere))
            {
                return putItHere.transform.GetComponent<T>();
            }
            else
            {
                return default;
            }
        }
    }
}