using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.Hardware;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Tests
{
    [TestClass()]
    public class HardwareBehaviorCollectionTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void HardwareBehaviorCollectionTest_null()
        {
            var behaviorCollection = new HardwareBehaviorCollection(null);
        }

        [TestMethod()]
        public void HardwareBehaviorCollectionTest()
        {
            var hardware = new MokHardware("hardware");
            var behaviorCollection = new HardwareBehaviorCollection(hardware);
            hardware._internelColection = behaviorCollection;

            Assert.IsNotNull(behaviorCollection);
            Assert.AreEqual(0, behaviorCollection.Count);
            Assert.IsFalse(behaviorCollection.ContainsBehaviorWithKey<MokBehavior>());
            Assert.IsNull(behaviorCollection.Get<MokBehavior>());
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void AddOrUpdateTest1()
        {
            var hardware = new MokHardware("hardware");
            var behaviorCollection = new HardwareBehaviorCollection(hardware);
            hardware._internelColection = behaviorCollection;

            behaviorCollection.AddOrUpdate<MokBehavior>(null);
        }


        [TestMethod()]
        public void AddOrUpdateTest2()
        {
            var behavior1 = new MokBehavior();

            var hardware = new MokHardware("hardware");
            var behaviorCollection = new HardwareBehaviorCollection(hardware);
            hardware._internelColection = behaviorCollection;

            //---
            behaviorCollection.AddOrUpdate(behavior1);

            Assert.AreEqual(1, behaviorCollection.Count);
            Assert.IsTrue(behaviorCollection.ContainsBehaviorWithKey<MokBehavior>());
            Assert.AreEqual(behavior1, behaviorCollection.Get<MokBehavior>());

            Assert.IsTrue(behavior1.IsAttached);
            Assert.AreEqual(behavior1.Hardware, hardware);
            Assert.AreEqual(behavior1.ConuntAttach, 1);

            //---
            behaviorCollection.AddOrUpdate(behavior1);

            Assert.AreEqual(1, behaviorCollection.Count);
            Assert.IsTrue(behaviorCollection.ContainsBehaviorWithKey<MokBehavior>());
            Assert.AreEqual(behavior1, behaviorCollection.Get<MokBehavior>());

            Assert.IsTrue(behavior1.IsAttached);
            Assert.AreEqual(behavior1.Hardware, hardware);
            Assert.AreEqual(behavior1.ConuntAttach, 2);
        }
    }
}