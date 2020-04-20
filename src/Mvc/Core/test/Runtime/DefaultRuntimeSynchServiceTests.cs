using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EquipApps.Mvc.Reactive.WorkFeatures.Services;

namespace EquipApps.Mvc.Runtime.Tests
{
    [TestClass()]
    public class DefaultRuntimeSynchServiceTests
    {
        [TestMethod()]
        public void TryRepeatTest()
        {
            var service = new DefaultRuntimeSynchService();

            //-- Конфигурация сервиса из вне. 
            service.IsEnabledRepeat = true;     //--
            service.CountRepeat     = 2;

            //Assert.IsTrue();
        }
    }
}