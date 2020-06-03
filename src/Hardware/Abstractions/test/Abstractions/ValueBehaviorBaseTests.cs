using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EquipApps.Hardware.Abstractions.Tests
{
    [TestClass()]
    public class ValueBehaviorBaseTests
    {
        public class MocValue : ValueBehaviorBase<int>
        {
            public MocValue()
            {
                Value = -1;
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void RequestToChangeValueTest()
        {
            var behavior = new MocValue();

            //-- Исключение без подписки
            try
            {
                behavior.RequestToChangeValue(10);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(-1, behavior.Value);
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void RequestToChangeValueTest1()
        {
            var behavior = new MocValue();
            behavior.ValueChange += Behavior_ValueChange1;

            try
            {
                behavior.RequestToChangeValue(10);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(-1, behavior.Value);
            }
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod()]
        public void RequestToChangeValueTest2()
        {
            var behavior = new MocValue();
            behavior.ValueChange += Behavior_ValueChange2;

            try
            {
                behavior.RequestToChangeValue(10);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(-1, behavior.Value);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueTest3()
        {
            var behavior = new MocValue();
            behavior.ValueChange += Behavior_ValueChange3;

            try
            {
                behavior.RequestToChangeValue(10);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(10, behavior.Value);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueTest4()
        {
            var behavior = new MocValue();
            behavior.ValueChange += Behavior_ValueChange4;

            try
            {
                behavior.RequestToChangeValue(10);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(20, behavior.Value);
            }
        }

        private void Behavior_ValueChange1(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(10, context.Input);

            //-- Не обрабатываем
        }

        private void Behavior_ValueChange2(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(10, context.Input);

            context.SetOutput(context.Input);

            //-- Исключение даже если значение установили
            throw new NotImplementedException("Ошибка");
        }

        private void Behavior_ValueChange3(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(10, context.Input);

            //-- Значение установлено
            context.SetOutput(context.Input);
        }

        private void Behavior_ValueChange4(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(10, context.Input);

            //-- Значение установлено другое
            context.SetOutput(20);
        }


        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void RequestToUpdateValueTest()
        {
            var behavior = new MocValue();

            try
            {
                behavior.RequestToUpdateValue();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(-1, behavior.Value);
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void RequestToUpdateValueTest1()
        {
            var behavior = new MocValue();
            behavior.ValueUpdate += Behavior_ValueUpdate1;

            try
            {
                behavior.RequestToUpdateValue();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(-1, behavior.Value);
            }
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod()]
        public void RequestToUpdateValueTest2()
        {
            var behavior = new MocValue();
            behavior.ValueUpdate += Behavior_ValueUpdate2;

            try
            {
                behavior.RequestToUpdateValue();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(-1, behavior.Value);
            }
        }

        [TestMethod()]
        public void RequestToUpdateValueTest3()
        {
            var behavior = new MocValue();
            behavior.ValueUpdate += Behavior_ValueUpdate3;

            try
            {
                behavior.RequestToUpdateValue();
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(10, behavior.Value);
            }
        }



        private void Behavior_ValueUpdate1(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(-1, context.Input);

            //-- Не обрабатываем
        }

        private void Behavior_ValueUpdate2(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(-1, context.Input);

            context.SetOutput(10);

            //-- Исключение даже если значение установили
            throw new NotImplementedException("Ошибка");
        }

        private void Behavior_ValueUpdate3(ValueBehaviorContext<int> context)
        {
            Assert.AreEqual(-1, context.Input);

            context.SetOutput(10);

            //-- Значение установлено
        }
    }
}