using System;
using UnityEngine;
using UnityEngine.UI;

namespace SyncPoints
{
    public class SyncPointSelectorBehaviour: MonoBehaviour
    {
        public GameObject buttonPrefab;
        public GameObject scrollContent;
        public void Start()
        {
            CreateScrollViewItems();
        }

        private void CreateScrollViewItems()
        {
            var syncPoints = Repositories.SyncPointRepository.Get();

            foreach (var syncPoint in syncPoints)
            {
                var clonedMarker = Instantiate(buttonPrefab, scrollContent.transform);
                clonedMarker.name = "ScrollItem-" + syncPoint.Name;
                clonedMarker.GetComponentInChildren<Text>().text = syncPoint.Name;
            }
        }
    }
}