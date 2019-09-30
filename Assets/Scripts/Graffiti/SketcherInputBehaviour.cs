using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class SketcherInputBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public TextureBehaviour textureBehaviour;
        private const float PlaneWidthMeters = 10f;
        private const float PlaneHeightMeters = 10f;
        private const int TextureSize = 50;

        public void Update()
        {
            if (inputHandler.TouchCount > 0)
            {
                HandleTouch();
            }
        }

        private void HandleTouch()
        {
            var touchPosition = inputHandler.GetTouch(0).position;
            var (_, hitPoint) = TouchToRayHit(touchPosition);

            var planeTransform = transform;
            var nominalPosition = hitPoint - planeTransform.position;

            Vector2 percentageOfWall = TwoPointsAsPercentToRight(nominalPosition, planeTransform);
            percentageOfWall.Scale(new Vector2(TextureSize, TextureSize));

            textureBehaviour.LitPoints.Add(percentageOfWall);
        }

        private Tuple<TextureBehaviour, Vector3> TouchToRayHit(Vector2 touchPosition)
        {
            return physicsHandler.Raycast<TextureBehaviour>(sketcherCamera.ScreenPointToRay(touchPosition));
        }

        private Vector2 TwoPointsAsPercentToRight(Vector3 nominalPosition, Transform planeTransform)
        {
            var planeScale = planeTransform.localScale;
            var planeWidth = planeScale.x * PlaneWidthMeters;
            var planeHeight = planeScale.z * PlaneHeightMeters;

            var zPercent = 1 - MoveFromCenterToZero(nominalPosition.z / planeWidth);
            var yPercent = MoveFromCenterToZero(nominalPosition.y / planeHeight);

            return new Vector2(zPercent, yPercent);
        }

        private static float MoveFromCenterToZero(float nominalPositionY)
        {
            return nominalPositionY + 0.5f;
        }

        public void SaveBits()
        {
            var data = string.Join("", textureBehaviour.LitPoints.Select(point =>
                $"{(int) Math.Round(point.x)},{(int) Math.Round(point.y)}\n"
            ));

            File.WriteAllBytes(Application.persistentDataPath + "/SavedImage.csv",
                Encoding.UTF8.GetBytes(data)
            );
        }

        public void ClearOnClick()
        {
            textureBehaviour.LitPoints.Clear();
        }
    }
}