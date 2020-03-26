using AtpNetCore.Mvc.Core.Tests.Mok;
using EquipApps.Mvc.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace NLib.AtpNetCore.Mvc.Internal.Tests
{
    [TestClass()]
    public class MvcApplicationModelBuilderTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateControllerModelTest_ArgumentNullException()
        {
            ApplicationModelBuilder.CreateControllerModel(null);
        }

        [TestMethod()]
        public void CreateControllerModelTest()
        {



            //---------------------------------------------------------------------------------
            var controllerType = typeof(Mok1Controller).GetTypeInfo();
            var controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);

            Assert.IsNotNull(controllerModel);
            Assert.IsNull(controllerModel.BindingInfo);


            //---------------------------------------------------------------------------------
            controllerType = typeof(Mok2Controller).GetTypeInfo();
            controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);

            Assert.IsNotNull(controllerModel);
            Assert.IsNull(controllerModel.BindingInfo);


            //---------------------------------------------------------------------------------
            controllerType = typeof(Mok2Controller).GetTypeInfo();
            controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);

            Assert.IsNotNull(controllerModel);
            Assert.IsNull(controllerModel.BindingInfo);

        }











        [TestMethod()]
        public void CreateControllerModelTest_Index()
        {
            var controllerType = typeof(IndexController).GetTypeInfo();
            var controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);

            Assert.AreEqual(controllerModel.Name, "Index");

            Assert.IsNull(controllerModel.Application);
            Assert.IsNull(controllerModel.Index);
            Assert.IsNotNull(controllerModel.Index.HasValue);



            //TODO: Добавить проверку остальных парметров
        }

        [TestMethod()]
        public void CreateControllerModelTest_Index1()
        {
            var controllerType = typeof(Index1Controller).GetTypeInfo();
            var controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);

            Assert.AreEqual(controllerModel.Name, "Index1");

            Assert.IsNull(controllerModel.Application);
            Assert.IsNotNull(controllerModel.Index);
            Assert.IsTrue(controllerModel.Index.HasValue);
            Assert.AreEqual(1, controllerModel.Index.Value);

            //TODO: Добавить проверку остальных парметров
        }

        [TestMethod()]
        public void CreateControllerModelTest_Index2()
        {
            var controllerType = typeof(Index2Controller).GetTypeInfo();
            var controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);

            Assert.AreEqual(controllerModel.Name, "Index2");

            Assert.IsNull(controllerModel.Application);
            Assert.IsNotNull(controllerModel.Index);
            Assert.IsTrue(controllerModel.Index.HasValue);
            Assert.AreEqual(3, controllerModel.Index.Value);

            //TODO: Добавить проверку остальных парметров
        }


    }
}