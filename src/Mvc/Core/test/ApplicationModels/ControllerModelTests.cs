﻿using AtpNetCore.Mvc.Core.Tests.Mok;
using EquipApps.Mvc.ApplicationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace NLib.AtpNetCore.Mvc.ApplicationModels.Tests
{
    [TestClass()]
    public class ControllerModelTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void ControllerModelTest1()
        {
            ControllerModel controllerModel = new ControllerModel(null, null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void ControllerModelTest2()
        {
            ControllerModel controllerModel = new ControllerModel(null, new object[0]);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void ControllerModelTest3()
        {
            ControllerModel controllerModel = new ControllerModel(typeof(Mok1Controller).GetTypeInfo(), null);
        }

        [TestMethod()]
        public void ControllerModelTest4()
        {
            var typeInfo = typeof(Mok1Controller).GetTypeInfo();
            var attr = new object[0];

            ControllerModel model = new ControllerModel(typeInfo, attr);



            Assert.IsNull(model.Application);
            Assert.IsNull(model.Area);
            Assert.AreEqual(attr, model.Attributes);
            Assert.IsNull(model.Background);
            Assert.IsNull(model.BindingInfo);
            Assert.IsNull(model.Index);
            Assert.AreEqual(typeInfo, model.Info);

            Assert.IsNotNull(model.Methods);
            Assert.AreEqual(0, model.Methods.Count);

            Assert.IsNull(model.ModelBinder);
            Assert.AreEqual(typeInfo.Name, model.Name);
            Assert.IsNull(model.Parent);

            Assert.IsNotNull(model.Properties);
            Assert.AreEqual(0, model.Properties.Count);

            Assert.IsNull(model.Title);
        }
    }
}