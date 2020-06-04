using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquipApps.Hardware.ValueDecorators.Tests
{
    [TestClass()]
    public class ValueHelperTests
    {
        [TestMethod()]
        public void ToNilTest()
        {
            byte value  = 0x55; //85
            byte expect = 0x45; //69
            byte actual = ValueHelper.ToNil(value, 4);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ToOneTest()
        {
            byte value  = 0x45;
            byte expect = 0x55;
            byte actual = ValueHelper.ToOne(value, 4);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ToNilTest1()
        {
            ushort value  = 0xFF55; //85
            ushort expect = 0xFF45; //69
            ushort actual = ValueHelper.ToNil(value, 4);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ToOneTest1()
        {
            ushort value  = 0xFF45;
            ushort expect = 0xFF55;
            ushort actual = ValueHelper.ToOne(value, 4);

            Assert.AreEqual(expect, actual);
        }
    }
}