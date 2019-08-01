using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.EditMode
{
    public static class TestHelpers
    {
        private static readonly QuaternionEqualityComparer QuaternionComparer = new QuaternionEqualityComparer(10e-6f);

        public static void AssertQuaternionsAreEqual(Quaternion expected, Quaternion actual)
        {
            Assert.That(actual,
                Is.EqualTo(expected).Using(QuaternionComparer)
            );
        }
    }
}