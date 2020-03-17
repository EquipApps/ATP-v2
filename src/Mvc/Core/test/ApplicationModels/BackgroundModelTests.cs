using AtpNetCore.Mvc.Core.Tests.Mok;
using EquipApps.Mvc.ApplicationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace NLib.AtpNetCore.Mvc.ApplicationModels.Tests
{
    [TestClass()]
    public class BackgroundModelTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void BackgroundModelTest1()
        {
            BackgroundModel model = new BackgroundModel(null, null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void BackgroundModelTest2()
        {
            BackgroundModel model = new BackgroundModel(null, new object[0]);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void BackgroundModelTest3()
        {
            BackgroundModel model = new BackgroundModel(typeof(MokModel).GetTypeInfo(), null);
        }

        [TestMethod()]
        public void BackgroundModelTest4()
        {
            var typeInfo = typeof(MokModel).GetTypeInfo();
            var attr = new object[0];

            BackgroundModel model = new BackgroundModel(typeInfo, attr);

            Assert.AreEqual(attr, model.Attributes);
            Assert.IsNull(model.Controller);
            Assert.AreEqual(typeInfo, model.Info);
            Assert.AreEqual(typeInfo.Name, model.Name);
            Assert.IsNull(model.Number);
            Assert.IsNull(model.NumberBinder);
            Assert.IsNull(model.TitleBinder);
            Assert.IsNull(model.Title);
        }
    }
}