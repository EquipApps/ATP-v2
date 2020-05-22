using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405.Tests
{
    [TestClass()]
    public class PSP405_Tests
    {
        [TestMethod()]
        public void PSP405_Test()
        {
            var device = new PSP405(1, 1);
            device.Dispose();
        }
    }
}