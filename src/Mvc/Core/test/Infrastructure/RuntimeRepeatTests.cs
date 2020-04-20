using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure.Tests
{
    [TestClass()]
    public class RuntimeRepeatTests
    {
        [TestMethod()]
        public void TryRepeatTest1()
        {
            //-- Создали модуль
            var repeatModule = new RuntimeRepeat();

            //-- По умолчанию нелья повторять
            for (int i = 0; i < 1000; i++)
            {
                Assert.IsFalse(repeatModule.TryRepeat());
            }

        }

        [TestMethod()]
        public void TryRepeatTest2()
        {
            //-- Создали модуль
            var repeatModule = new RuntimeRepeat();
            repeatModule.SetCounter(-1);

            //-- Можно повторять хоть сколько
            for (int i = 0; i < 1000; i++)
            {
                Assert.IsTrue(repeatModule.TryRepeat());
            }
        }

        [TestMethod()]
        public void TryRepeatTest3()
        {
            //-- Создали модуль
            var repeatModule = new RuntimeRepeat();
            repeatModule.SetCounter();

            //-- Нелья повторять
            for (int i = 0; i < 1000; i++)
            {
                Assert.IsFalse(repeatModule.TryRepeat());
            }

        }

        [TestMethod()]
        public void TryRepeatTest4()
        {
            int count = 20;

            //-- Создали модуль
            var repeatModule = new RuntimeRepeat();
            repeatModule.SetCounter(count);

            //-- Можно повторять 
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(repeatModule.TryRepeat());
            }

            //-- Нелья повторять
            for (int i = 0; i < 1000; i++)
            {
                Assert.IsFalse(repeatModule.TryRepeat());
            }

        }

        [TestMethod()]
        public void TryRepeatTest5()
        {
            int count = 20;

            //-- Создали модуль
            var repeatModule = new RuntimeRepeat();
            repeatModule.SetCounter(count);

            //-- Можно повторять 
            for (int i = 0; i < count / 2; i++)
            {
                Assert.IsTrue(repeatModule.TryRepeat());
            }

            repeatModule.SetCounter();

            //-- Нелья повторять
            for (int i = 0; i < 1000; i++)
            {
                Assert.IsFalse(repeatModule.TryRepeat());
            }

        }
    }
}