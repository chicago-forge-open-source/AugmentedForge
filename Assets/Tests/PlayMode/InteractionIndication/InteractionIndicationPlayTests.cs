using System.Collections;
using InteractionIndication;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.InteractionIndication
{
    public class InteractionIndicationPlayTests
    {
        [UnityTest]
        public IEnumerator ObjectBehindCameraHasNegativeDepth()
        {
            SceneManager.LoadScene("ARView");
            yield return null;

            var worldToScreenPointBehaviour = GameObject.Find("IoTLight")
                .GetComponent<InteractionIndicationBehaviour>();

            Vector3 screenPosition = worldToScreenPointBehaviour.screenPoint;

            Assert.LessOrEqual(screenPosition.z, 0);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ObjectInFrontOfCameraHasPositiveDepth()
        {
            SceneManager.LoadScene("ARView");
            yield return null;

            var worldToScreenPointBehaviour = GameObject.Find("IoTLight")
                .GetComponent<InteractionIndicationBehaviour>();

            GameObject camera = GameObject.Find("AR Camera");
            camera.transform.Rotate(0.0f, 180.0f, 0.0f);

            worldToScreenPointBehaviour.Update();
            Vector3 screenPosition = worldToScreenPointBehaviour.screenPoint;

            Assert.GreaterOrEqual(screenPosition.z, 0);
            yield return null;
        }
    }
}