using AR;
using DefaultNamespace;
using SyncPoints;
using UnityEngine;

namespace QR
{
    public class QRBehaviour : MonoBehaviour
    {
        public ArCalibrationBehaviour arCalibrationBehaviour;
        
        public void Update()
        {
            DetectQrSyncPoint();
        }
        
        private void DetectQrSyncPoint()
        {
            if (PlayerSelections.qrParametersProvided)
            {
                PlayerSelections.qrParametersProvided = false;
                var providedSyncPointPosition = PlayerSelections.qrPoint;
                arCalibrationBehaviour.pendingSyncPoint = new SyncPoint("Chicago", providedSyncPointPosition.x, providedSyncPointPosition.z,
                    PlayerSelections.orientation);
            }
        }
    }
}