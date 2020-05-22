using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_2010.Tests
{
    [TestClass()]
    public class PSP2010_Tests
    {
        [TestMethod()]
        public void PSP2010_Test()
        {
            var device = new PSP2010(1, 1);
                device.Dispose();
        }
    }
}