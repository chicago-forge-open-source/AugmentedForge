using System;
using InteractionIndication;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.InteractionIndication
{
    public class InteractionIndicationBehaviourTests
    {
        private GameObject _gameObject;
        private InteractionIndicationBehaviour _interactionIndicationBehaviour;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _interactionIndicationBehaviour = _gameObject.AddComponent<InteractionIndicationBehaviour>();
        }

        [Test]
        public void OnGUI_WillDrawTextureInBoxWithCorrectColors()
        {
            var expectedBackgroundColor = new Color(1, 1, 1, 0);
            var expectedColor = new Color(1, 1, 1, 1);
            var expectedTexture = new Texture2D(50, 50);
            _interactionIndicationBehaviour.BoxTexture = expectedTexture;

            GuiGuySpy guiGuySpy = new GuiGuySpy();
            _interactionIndicationBehaviour.guiGuy = guiGuySpy;
            var drawIsCalled = false;
            guiGuySpy.onDrawBox += (rect, texture) =>
            {
                Assert.AreEqual(expectedBackgroundColor, guiGuySpy.BackGroundColor);
                Assert.AreEqual(expectedColor, guiGuySpy.Color);
                Assert.AreEqual(expectedTexture, texture);
                drawIsCalled = true;
            };

            _interactionIndicationBehaviour.OnGUI();
            Assert.IsTrue(drawIsCalled);
        }

        [Test]
        public void OnGUI_WillDrawRectangleCenteredOnScreenPointSimpleCase()
        {
            var screenPoint = new Vector3(1,1,1);
            _interactionIndicationBehaviour.screenPoint = screenPoint;
            _interactionIndicationBehaviour.BoxTexture = new Texture2D(50, 50);;

            GuiGuySpy guiGuySpy = new GuiGuySpy();
            _interactionIndicationBehaviour.guiGuy = guiGuySpy;
            var drawIsCalled = false;
            guiGuySpy.onDrawBox += (rect, texture) =>
            {
                Assert.AreEqual(screenPoint.x, rect.center.x);
                Assert.AreEqual(screenPoint.y, rect.center.y);
                Assert.AreEqual(1000, rect.width);
                Assert.AreEqual(1000, rect.height);
                drawIsCalled = true;
            };

            _interactionIndicationBehaviour.OnGUI();
            Assert.IsTrue(drawIsCalled);
        }
        
        [Test]
        public void OnGUI_WillDrawRectangleCenteredOnScreenPointAtRange()
        {
            var screenPoint = new Vector3(300,400,10);
            _interactionIndicationBehaviour.screenPoint = screenPoint;
            _interactionIndicationBehaviour.BoxTexture = new Texture2D(50, 50);;

            GuiGuySpy guiGuySpy = new GuiGuySpy();
            _interactionIndicationBehaviour.guiGuy = guiGuySpy;
            var drawIsCalled = false;
            guiGuySpy.onDrawBox += (rect, texture) =>
            {
                Assert.AreEqual(screenPoint.x, rect.center.x);
                Assert.AreEqual(screenPoint.y, rect.center.y);
                Assert.AreEqual(100, rect.width);
                Assert.AreEqual(100, rect.height);
                drawIsCalled = true;
            };

            _interactionIndicationBehaviour.OnGUI();
            Assert.IsTrue(drawIsCalled);
        }
    }
}

class GuiGuySpy : IGuiGuy
{
    public Color Color { get; set; }
    public Color BackGroundColor { get; set; }
    public event Action<Rect, Texture> onDrawBox = delegate { };

    public void DrawBox(Rect screenRect, Texture texture)
    {
        onDrawBox(screenRect, texture);
    }
}