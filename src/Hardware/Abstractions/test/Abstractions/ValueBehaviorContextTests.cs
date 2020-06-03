using EquipApps.Hardware.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EquipApps.Hardware.Abstractions.Tests
{
    [TestClass()]
    public class ValueBehaviorContextTests
    {
        [TestMethod()]
        public void ValueBehaviorContextTest()
        {
            var context = new ValueBehaviorContext<int>(10);

            Assert.AreEqual(10, context.Input);
            Assert.AreEqual(false, context.IsHandled);
        }

        [TestMethod()]
        public void SetOutputTest()
        {
            var context = new ValueBehaviorContext<int>(10);
            context.SetOutput(20);

            Assert.AreEqual(10, context.Input);
            Assert.AreEqual(true, context.IsHandled);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void SetOutputTest1()
        {
            var context = new ValueBehaviorContext<int>(10);
            context.SetOutput(20);
            context.SetOutput(20);  //-- Можно вызывать один раз
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void GetOutputTest()
        {
            var context = new ValueBehaviorContext<int>(10);
            context.GetOutput();

        }

        [TestMethod()]
        public void SetGetOutputTest1()
        {
            var context = new ValueBehaviorContext<int>(10);
                context.SetOutput(20);

            Assert.AreEqual(10, context.Input);
            Assert.AreEqual(true, context.IsHandled);
            Assert.AreEqual(20, context.GetOutput());
            Assert.AreEqual(20, context.GetOutput());   //-- Можно вызывать много раз
        }
    }
}