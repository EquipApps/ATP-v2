using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405.Tests
{
    [TestClass()]
    public class Psp405_DeviceTests
    {
        [TestMethod()]
        public void Psp405_DeviceTest()
        {
            var device = new Psp405_Device(1, 1);
            device.Dispose();
        }
    }
}