using UnityEngine;

namespace Graffiti
{
    public class SketcherInputBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public UnityPlaneTouchDetector touchDetector = new UnityPlaneTouchDetector();
        public TextureBehaviour sketcherTextureBehaviour;

        private int TextureSize => sketcherTextureBehaviour.textureSize;

        public void Update()
        {
            HandleTouch();
        }

        private void HandleTouch()
        {
            var percentageOfWall = touchDetector.FindTouchedPoint(transform, sketcherCamera, TextureSize);
            if (percentageOfWall.HasValue && NoGameObjectSelectionIsDetected())
            {
                sketcherTextureBehaviour.LitPoints.Add(percentageOfWall.Value);
            }
        }

        public void ClearOnClick()
        {
            sketcherTextureBehaviour.LitPoints.Clear();
        }

        private static bool NoGameObjectSelectionIsDetected()
        {
            return EventSystemIsNotRunning() || NoGameObjectIsSelected();
        }

        private static bool NoGameObjectIsSelected()
        {
            return !UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        }

        private static bool EventSystemIsNotRunning()
        {
            return UnityEngine.EventSystems.EventSystem.current == null;
        }
    }
}