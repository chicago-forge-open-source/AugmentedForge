using AR;
using DefaultNamespace;
using NUnit.Framework;
using QR;
using UnityEngine;

namespace Tests.EditMode
{
    public class QRBehaviourTests
    {
        private QRBehaviour _qrBehaviour;
        private ArCalibrationBehaviour _arCalibrationBehaviour;
        private GameObject _gameObject;
        
        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _qrBehaviour = _gameObject.AddComponent<QRBehaviour>();
            _arCalibrationBehaviour = _gameObject.AddComponent<ArCalibrationBehaviour>();
            _qrBehaviour.arCalibrationBehaviour = _arCalibrationBehaviour;
        }

        [Test]
        public void Update_GivenPlayerSelectionsHasASyncPoint_ArSessionOriginMovesToSyncPoint()
        {
            var expectedPosition = new Vector3(1, 0, 2);
            PlayerSelections.qrPoint = expectedPosition;
            PlayerSelections.orientation = 180;
            PlayerSelections.qrParametersProvided = true;
            
           _qrBehaviour.Update();
           
           Assert.NotNull(_arCalibrationBehaviour.pendingSyncPoint);
           var actualPendingSyncPoint = _arCalibrationBehaviour.pendingSyncPoint;
           Assert.AreEqual(180, actualPendingSyncPoint.Orientation);
           Assert.AreEqual(expectedPosition.x, actualPendingSyncPoint.X);
           Assert.AreEqual(expectedPosition.z, actualPendingSyncPoint.Z);
           Assert.False(PlayerSelections.qrParametersProvided);
        }
        
        [Test]
        public void Update_GivenNoPlayerSelection_PendingSyncPointIsNull()
        {
            PlayerSelections.qrParametersProvided = false;
            
            _qrBehaviour.Update();
           
            Assert.Null(_arCalibrationBehaviour.pendingSyncPoint);
        }
    }
}