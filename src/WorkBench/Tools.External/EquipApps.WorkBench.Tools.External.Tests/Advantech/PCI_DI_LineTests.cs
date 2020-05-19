using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquipApps.WorkBench.Tools.External.Advantech.Tests
{
    [TestClass()]
    public class PCI_DI_LineTests
    {
        [TestMethod()]
        public void PCI_LineTest_DefaultValue()
        {
            var line = new PCI_DI_Line();

            Assert.IsNotNull(line);
            Assert.AreEqual(0x00, line.Value);
        }
    }
}