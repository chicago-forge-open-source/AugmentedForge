using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Tests.EditMode
{
    internal class MockInputHandler : InputHandler
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