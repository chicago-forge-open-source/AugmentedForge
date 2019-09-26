using System;
using UnityEngine;

namespace Tests.Mocks
{
    public class MockPhysicsHandler<TR> : PhysicsHandler where TR : class
    {
        public TR ValueToReturn { private get; set; }
        public Vector3 HitPointToReturn { private get; set; }
        public Tuple<T, Vector3> Raycast<T>(Ray ray) where T : class
        {
            var componentHit = ValueToReturn as T;
            return Tuple.Create(componentHit, HitPointToReturn);
        }

        public static MockPhysicsHandler<T> ReturnsDetected<T>(T behaviour) where T : class
        {
            return new MockPhysicsHandler<T>
            {
                ValueToReturn = behaviour
            };
        }
    }
}