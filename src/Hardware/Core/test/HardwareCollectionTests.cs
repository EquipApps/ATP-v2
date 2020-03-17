using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.Hardware;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Tests
{
    [TestClass()]
    public class HardwareCollectionTests
    {
        [TestMethod()]
        public void HardwareCollectionTest()
        {
            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);

            Assert.IsNotNull(hardwares);
            Assert.AreEqual(0, hardwares.Count);
            Assert.IsFalse(hardwares.ContainsHardwareWithKey("hardware1"));
            Assert.IsNull(hardwares["hardware1"]);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void AddOrUpdateTest1()
        {
            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);
            hardwares.AddOrUpdate(null);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod()]
        public void AddOrUpdateTest2()
        {
            MokHardware mokHardware = new MokHardware(null);

            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);
            hardwares.AddOrUpdate(mokHardware);
        }

        [TestMethod()]
        public void AddOrUpdateTest3()
        {
            var hardware1 = new Hardware("hardware1");
            var hardware2 = new Hardware("hardware1");
            var hardware3 = new Hardware("hardware3");

            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);

            //--
            hardwares.AddOrUpdate(hardware1);

            Assert.AreEqual(1, hardwares.Count);
            Assert.AreEqual(hardware1, hardwares["hardware1"]);

            //--
            hardwares.AddOrUpdate(hardware1);

            Assert.AreEqual(1, hardwares.Count);
            Assert.AreEqual(hardware1, hardwares["hardware1"]);

            //--
            hardwares.AddOrUpdate(hardware2);

            Assert.AreEqual(1, hardwares.Count);
            Assert.AreEqual(hardware2, hardwares["hardware1"]);

            //--
            hardwares.AddOrUpdate(hardware3);
            Assert.AreEqual(2, hardwares.Count);
            Assert.AreEqual(hardware2, hardwares["hardware1"]);
            Assert.AreEqual(hardware3, hardwares["hardware3"]);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void RemoveTest()
        {
            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);
            hardwares.Remove(null);
        }

        [TestMethod()]
        public void RemoveTest1()
        {
            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);
            hardwares.Remove("hardware1");
        }

        [TestMethod()]
        public void RemoveTest2()
        {
            var hardware1 = new Hardware("hardware1");
            var hardware2 = new Hardware("hardware2");
            var hardware3 = new Hardware("hardware3");

            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);

            //--
            hardwares.AddOrUpdate(hardware1);
            hardwares.AddOrUpdate(hardware2);
            hardwares.AddOrUpdate(hardware3);

            Assert.AreEqual(3, hardwares.Count);


            hardwares.Remove("hardware2");
            Assert.AreEqual(hardware1, hardwares["hardware1"]);
            Assert.IsNull(hardwares["hardware2"]);
            Assert.AreEqual(hardware3, hardwares["hardware3"]);
        }

        [TestMethod()]
        public void ClearTest()
        {
            var hardware1 = new Hardware("hardware1");
            var hardware2 = new Hardware("hardware2");
            var hardware3 = new Hardware("hardware3");

            //-- Создали пустую коллекцию
            HardwareCollection hardwares = new HardwareCollection(null);

            //--
            hardwares.AddOrUpdate(hardware1);
            hardwares.AddOrUpdate(hardware2);
            hardwares.AddOrUpdate(hardware3);

            Assert.AreEqual(3, hardwares.Count);

            hardwares.Clear();

            Assert.AreEqual(0, hardwares.Count);
            Assert.IsNull(hardwares["hardware1"]);
            Assert.IsNull(hardwares["hardware2"]);
            Assert.IsNull(hardwares["hardware3"]);
        }
    }
}