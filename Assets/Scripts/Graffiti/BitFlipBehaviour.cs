using System;
using System.Collections.Generic;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class BitFlipBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public Material material;
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        private readonly List<Vector2> _litPoints = new List<Vector2>();
        private const float PlaneWidthMeters = 10f;
        private const float PlaneHeightMeters = 10f;
        private const int TextureSize = 50;

        public void Update()
        {
            if (inputHandler.TouchCount > 0)
            {
                HandleTouch();
            }

            material.mainTexture = BuildGraffitiTexture();
        }

        private Texture2D BuildGraffitiTexture()
        {
            var texture = MakeBlackTexture();
            SetDrawingsOnTexture(texture);
            texture.Apply();
            return texture;
        }

        private void SetDrawingsOnTexture(Texture2D texture)
        {
            _litPoints.ForEach(point =>
            {
                var targetX = (int) Math.Round(point.x);
                var targetY = (int) Math.Round(point.y);
                texture.SetPixel(targetX, targetY, Color.white);
            });
        }

        private void HandleTouch()
        {
            var touchPosition = inputHandler.GetTouch(0).position;
            var (_, hitPoint) = TouchToRayHit(touchPosition);

            var planeTransform = transform;
            var nominalPosition = hitPoint - planeTransform.position;

            Vector2 percentageOfWall = TwoPointsAsPercentToRight(nominalPosition, planeTransform);
            percentageOfWall.Scale(new Vector2(TextureSize, TextureSize));

            _litPoints.Add(percentageOfWall);
        }

        private Tuple<BitFlipBehaviour, Vector3> TouchToRayHit(Vector2 touchPosition)
        {
            return physicsHandler.Raycast<BitFlipBehaviour>(sketcherCamera.ScreenPointToRay(touchPosition));
        }

        private static Texture2D MakeBlackTexture()
        {
            var texture = new Texture2D(TextureSize, TextureSize) {filterMode = FilterMode.Point};

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }

            return texture;
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
}