using EquipApps.Mvc.ApplicationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NLib.AtpNetCore.Mvc.ApplicationModels.Tests
{
    [TestClass()]
    public class AreaModelTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AreaModelTest_ArgumentNullException()
        {
            var model = new AreaModel(1, null);
        }

        [TestMethod()]
        public void AreaModelTest()
        {
            //--------------------------------------------------
            var model = new AreaModel(0, string.Empty);
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Index);
            Assert.AreEqual(string.Empty, model.Name);

            //--------------------------------------------------
            model = new AreaModel(1, "__22__22!");
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Index);
            Assert.AreEqual("__22__22!", model.Name);

            //--------------------------------------------------
            model = new AreaModel(-1, "d3fgf4g4  _3r3");
            Assert.IsNotNull(model);
            Assert.AreEqual(-1, model.Index);
            Assert.AreEqual("d3fgf4g4  _3r3", model.Name);
        }
    }
}