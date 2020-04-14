using EquipApps.Mvc.ApplicationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NLib.AtpNetCore.Mvc.ApplicationModels.Tests
{
    [TestClass()]
    public class ApplicationModelTests
    {
        [TestMethod()]
        public void ApplicationModelTest()
        {
            ApplicationModel applicationModel = new ApplicationModel();

            Assert.IsNotNull(applicationModel);           
            Assert.IsNotNull(applicationModel.Controllers);
        }
    }
}