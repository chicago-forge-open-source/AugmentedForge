using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class GraffitiTextureBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public Material material;
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        private readonly List<Vector2> _litPoints = new List<Vector2>();
        private const float PlaneWidthMeters = 10f;
        private const float PlaneHeightMeters = 10f;
        private const int TextureSize = 50;

        public void Start()
        {
            if (File.Exists(Application.persistentDataPath + "/SavedImage.csv"))
            {
                ReadTextureFromFile();
            }

            material.mainTexture = BuildGraffitiTexture();
        }

        private void ReadTextureFromFile()
        {
            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var lines = Encoding.UTF8.GetString(rawBytes)
                .Split('\n');

            foreach (var line in lines)
            {
                if (line == "") continue;
                var pairs = line.Split(',');
                _litPoints.Add(new Vector2(int.Parse(pairs[0]), int.Parse(pairs[1])));
            }
        }

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

        private Tuple<GraffitiTextureBehaviour, Vector3> TouchToRayHit(Vector2 touchPosition)
        {
            return physicsHandler.Raycast<GraffitiTextureBehaviour>(sketcherCamera.ScreenPointToRay(touchPosition));
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

        public void SaveBits()
        {
            var data = string.Join("", _litPoints.Select(point =>
                $"{(int) Math.Round(point.x)},{(int) Math.Round(point.y)}\n"
            ));

            File.WriteAllBytes(Application.persistentDataPath + "/SavedImage.csv",
                Encoding.UTF8.GetBytes(data)
            );
        }
    }
}