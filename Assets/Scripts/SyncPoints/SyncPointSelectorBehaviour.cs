using AR;
using UnityEngine;
using UnityEngine.UI;

namespace SyncPoints
{
    public class SyncPointSelectorBehaviour: MonoBehaviour
    {
        public GameObject buttonPrefab;
        public GameObject scrollContent;
        public ArCalibrationBehaviour calibrationBehaviour;

        public void Start()
        {
            CreateScrollViewItems();
        }

        private void CreateScrollViewItems()
        {
            var syncPoints = Repositories.SyncPointRepository.Get();

            foreach (var syncPoint in syncPoints)
            {
                var syncPointGameObject = Instantiate(buttonPrefab, scrollContent.transform);
                syncPointGameObject.name = "ScrollItem-" + syncPoint.Name;
                syncPointGameObject.GetComponentInChildren<Text>().text = syncPoint.Name;
                syncPointGameObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        calibrationBehaviour.pendingSyncPoint = syncPoint;
                    });
            }
        }
    }
}