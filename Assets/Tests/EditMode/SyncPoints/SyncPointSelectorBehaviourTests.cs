using AR;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.EditMode.SyncPoints
{
    public class SyncPointSelectorBehaviourTests
    {
        private GameObject _game;
        private SyncPointSelectorBehaviour _selectorBehaviour;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _selectorBehaviour = _game.AddComponent<SyncPointSelectorBehaviour>();
            _selectorBehaviour.scrollContent = new GameObject();
            _selectorBehaviour.calibrationBehaviour = _game.AddComponent<ArCalibrationBehaviour>();
            SetupScrollItemPrefab();
        }

        [Test]
        public void Start_WillCreateScrollViewItemOnScrollView()
        {
            var syncPoint = new SyncPoint("Lone Cowboy", 10, 10, 84);
            Repositories.SyncPointRepository.Save(new[] {syncPoint});

            _selectorBehaviour.Start();

            var content = _selectorBehaviour.scrollContent;

            Assert.AreEqual(1, content.transform.childCount);
        }

        [Test]
        public void Start_WillCreateScrollViewItemFromSyncPoints()
        {
            var marker1 = new SyncPoint("Testing 1", 10, 10, 0);
            var marker2 = new SyncPoint("Testing 2", 20, 2, 180);
            Repositories.SyncPointRepository.Save(new[] {marker1, marker2});

            _selectorBehaviour.Start();

            var content = _selectorBehaviour.scrollContent;

            Assert.AreEqual(2, content.transform.childCount);
            Assert.AreEqual("ScrollItem-" + marker1.Name, content.transform.GetChild(0).name);
            Assert.AreEqual(marker1.Name, GetTextFromScrollItem(content, 0));
            Assert.AreEqual("ScrollItem-" + marker2.Name, content.transform.GetChild(1).name);
            Assert.AreEqual(marker2.Name, GetTextFromScrollItem(content, 1));
        }

        [Test]
        public void OnButtonClick_WillScheduleSyncOnArCalibrationBehaviour()
        {
            var syncPoint = new SyncPoint("Lone Cowboy", 10, 10, 84);
            Repositories.SyncPointRepository.Save(new[] {syncPoint});

            _selectorBehaviour.Start();
            
            _selectorBehaviour.scrollContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
            
            Assert.AreEqual(syncPoint, _selectorBehaviour.calibrationBehaviour.scheduledSyncPoint);
        }

        private static string GetTextFromScrollItem(GameObject content, int index)
        {
            return content.transform.GetChild(index).GetComponentInChildren<Text>().text;
        }

        private void SetupScrollItemPrefab()
        {
            _selectorBehaviour.buttonPrefab = new GameObject("Button Generic");
            _selectorBehaviour.buttonPrefab.AddComponent<Button>();

            var textChild = new GameObject();
            textChild.transform.parent = _selectorBehaviour.buttonPrefab.transform;
            textChild.AddComponent<Text>().text = "Blank Scroll Button";
        }
    }
}