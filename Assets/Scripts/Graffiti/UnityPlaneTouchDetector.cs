using System;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class UnityPlaneTouchDetector : PlaneTouchDetector
    {
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        private const float PlaneWidthMeters = 10f;
        private const float PlaneHeightMeters = 10f;

        public Vector2? FindTouchedPoint(Transform transform, Camera camera, int textureSize)
        {
            if (inputHandler.TouchCount == 0)
            {
                return null;
            }

            var touchPosition = inputHandler.GetTouch(0).position;
            var (_, hitPoint) = TouchToRayHit(touchPosition, camera);

            var planeTransform = transform;
            var nominalPosition = hitPoint - planeTransform.position;

            var percentageOfWall = TwoPointsAsPercentToRight(nominalPosition, planeTransform);
            percentageOfWall.Scale(new Vector2(textureSize, textureSize));
            return percentageOfWall;
        }

        private Tuple<TextureBehaviour, Vector3> TouchToRayHit(Vector2 touchPosition, Camera camera)
        {
            return physicsHandler.Raycast<TextureBehaviour>(camera.ScreenPointToRay(touchPosition));
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
    }

    public interface PlaneTouchDetector
    {
        Vector2? FindTouchedPoint(Transform transform, Camera camera, int textureSize);
    }
}