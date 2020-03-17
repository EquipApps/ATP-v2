using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.Hardware;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Tests
{
    [TestClass()]
    public class HardwareTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void HardwareTest()
        {
            Hardware hardware = new Hardware(null);
        }

        [TestMethod()]
        public void HardwareTest1()
        {
            string value = "Hardware";

            Hardware hardware = new Hardware(value);

            Assert.IsNotNull(hardware);
            Assert.AreEqual(value, hardware.Key);
            Assert.AreEqual(value, hardware.Name);
            Assert.AreEqual(string.Empty, hardware.Description);
            Assert.IsNotNull(hardware.Behaviors);
        }
    }
}