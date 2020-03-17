using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Tests
{
    [TestClass()]
    public class RelayBehaviorTests
    {
        [TestMethod()]
        public void RelayBehaviorTest()
        {
            //--
            RelayBehavior relayBehavior = new RelayBehavior();

            Assert.IsNotNull(relayBehavior);
            Assert.AreEqual (RelayState.Disconnect, relayBehavior.Value);
        }
    }
}