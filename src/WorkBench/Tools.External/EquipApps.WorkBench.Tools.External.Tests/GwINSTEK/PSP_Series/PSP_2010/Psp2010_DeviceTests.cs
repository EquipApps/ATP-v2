using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_2010.Tests
{
    [TestClass()]
    public class Psp2010_DeviceTests
    {
        [TestMethod()]
        public void Psp2010_DeviceTest()
        {
            var device = new Psp2010_Device(1, 1);
                device.Dispose();
        }
    }
}