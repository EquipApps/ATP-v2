using EquipApps.Mvc.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NLib.AtpNetCore.Mvc.Internal.Tests
{
    [TestClass()]
    public class StringExTests
    {
        [TestMethod()]
        public void ToIntFromEndTest()
        {
            string nullString = null;

            int? result = nullString.ToIntFromEnd();

            Assert.IsFalse(result.HasValue);

        }

        [TestMethod()]
        public void ToIntFromEndTest1()
        {
            int? result = "Example".ToIntFromEnd();

            Assert.IsFalse(result.HasValue);
        }

        [TestMethod()]
        public void ToIntFromEndTest2()
        {
            int? result = "Example1".ToIntFromEnd();

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(1, result.Value);
        }

        [TestMethod()]
        public void ToIntFromEndTest3()
        {
            int? result = "Example1ds221".ToIntFromEnd();

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(221, result.Value);
        }
    }
}