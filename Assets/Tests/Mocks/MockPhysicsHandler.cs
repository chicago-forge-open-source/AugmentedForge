using UnityEngine;

namespace Tests.Mocks
{
    public class MockPhysicsHandler<TR> : PhysicsHandler where TR : class
    {
        public TR ValueToReturn { private get; set; }

        public T Raycast<T>(Ray ray) where T : class
        {
            return ValueToReturn as T;
        }
    }
}