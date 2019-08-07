using Markers;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests.EditMode
{
    public class PopulateScrollViewTests
    {
        private GameObject _game;
        private PopulateScrollView _populateScrollView;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _populateScrollView = _game.AddComponent<PopulateScrollView>();
            _populateScrollView.scrollContentGrid = new GameObject();
            _populateScrollView.scrollItemPrefab = new GameObject("Scroll Item");
        }

        [Test]
        public void Start_WillCreateScrollViewItemsForSyncPoints()
        {
            var marker = new Marker("Testing", 10, 10);
            Repositories.MarkerRepository.Save(new[] {marker});

            _populateScrollView.Start();

            var content = _populateScrollView.scrollContentGrid;
            
            Assert.AreEqual(1, content.transform.childCount);
        }
    }
}