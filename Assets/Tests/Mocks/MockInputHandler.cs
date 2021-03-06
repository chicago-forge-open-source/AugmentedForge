using System.Collections.Generic;
using UnityEngine;

namespace Tests.Mocks
{
    public class MockInputHandler : InputHandler
    {
        private readonly List<Touch> _touches;

        public MockInputHandler(List<Touch> touches)
        {
            _touches = touches;
        }

        public int TouchCount => _touches.Count;

        public Touch GetTouch(int index)
        {
            return _touches[index];
        }
    }
}