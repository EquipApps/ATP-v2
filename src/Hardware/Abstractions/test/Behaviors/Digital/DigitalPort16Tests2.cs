using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EquipApps.Hardware.Abstractions;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.Digital.Tests
{
    [TestClass()]
    public class DigitalPort16Tests2
    {
        public class MocValue : DigitalPort16
        {
            public MocValue()
            {
                Value = byte.MaxValue;
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
                Assert.AreEqual(byte.MaxValue, behavior.Value);
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
                Assert.AreEqual(byte.MaxValue, behavior.Value);
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
                Assert.AreEqual(byte.MaxValue, behavior.Value);
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


        private void Behavior_ValueChange1(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(10, context.Input);

            //-- Не обрабатываем
        }

        private void Behavior_ValueChange2(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(10, context.Input);

            context.SetOutput(context.Input);

            //-- Исключение даже если значение установили
            throw new NotImplementedException("Ошибка");
        }

        private void Behavior_ValueChange3(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(10, context.Input);

            //-- Значение установлено
            context.SetOutput(context.Input);
        }

        private void Behavior_ValueChange4(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(10, context.Input);

            //-- Значение установлено другое
            context.SetOutput(20);
        }

        //=======================================================================

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
                Assert.AreEqual(byte.MaxValue, behavior.Value);
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
                Assert.AreEqual(byte.MaxValue, behavior.Value);
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
                Assert.AreEqual(byte.MaxValue, behavior.Value);
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


        private void Behavior_ValueUpdate1(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(byte.MaxValue, context.Input);

            //-- Не обрабатываем
        }

        private void Behavior_ValueUpdate2(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(byte.MaxValue, context.Input);

            context.SetOutput(10);

            //-- Исключение даже если значение установили
            throw new NotImplementedException("Ошибка");
        }

        private void Behavior_ValueUpdate3(object sender, ValueBehaviorContext<ushort> context)
        {
            Assert.AreEqual(byte.MaxValue, context.Input);

            context.SetOutput(10);

            //-- Значение установлено
        }


        //=======================================================================

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToChangeValueTR()
        {
            var behavior = new MocValue();

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueTR_S()
        {
            var behavior = new MocValue();

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
            }
        }

        //============

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToChangeValueT1()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;
                //-- Не обрабатываем
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueT1_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;
                //-- Не обрабатываем
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }

        //============

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToChangeValueT2()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;

                context.SetOutput(context.Input);

                //-- Исключение даже если значение установили
                throw new NotImplementedException("Ошибка");
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueT2_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;

                context.SetOutput(context.Input);

                //-- Исключение даже если значение установили
                throw new NotImplementedException("Ошибка");
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);

                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }

        //============

        [TestMethod()]
        public void RequestToChangeValueT3()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(10, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueT3_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }

        //============

        [TestMethod()]
        public void RequestToChangeValueT4()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;

                //-- Значение установлено
                context.SetOutput(20);
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(20, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToChangeValueT4_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(10, context.Input);
                countCall++;

                //-- Значение установлено
                context.SetOutput(20);
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }

        //=======================================================================



        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToUpdateValueTR()
        {
            var behavior = new MocValue();

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
            }
        }

        [TestMethod()]
        public void RequestToUpdateValueTR_S()
        {
            var behavior = new MocValue();

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
            }
        }

        //============

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToUpdateValueT1()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(byte.MaxValue, context.Input);
                countCall++;
                //-- Не обрабатываем
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToUpdateValueT1_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(byte.MaxValue, context.Input);
                countCall++;
                //-- Не обрабатываем
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }

        //============

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToUpdateValueT2()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(byte.MaxValue, context.Input);
                countCall++;

                context.SetOutput(10);

                //-- Исключение даже если значение установили
                throw new NotImplementedException("Ошибка");
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToUpdateValueT2_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(byte.MaxValue, context.Input);
                countCall++;

                context.SetOutput(10);

                //-- Исключение даже если значение установили
                throw new NotImplementedException("Ошибка");
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }

        //============

        [TestMethod()]
        public void RequestToUpdateValueT3()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(byte.MaxValue, context.Input);
                countCall++;

                context.SetOutput(10);
                //-- Значение установлено
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(10, behavior.Value);
                Assert.AreEqual(1, countCall);
            }
        }

        [TestMethod()]
        public void RequestToUpdateValueT3_S()
        {
            int countCall = 0;

            var behavior = new MocValue();
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                Assert.AreEqual(-1, context.Input);
                countCall++;

                context.SetOutput(10);

                //-- Значение установлено
            };

            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToUpdateValue();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);
                Assert.AreEqual(0, countCall);
            }
        }


        //=======================================================================

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToChangeValueTM()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

            int value = 0;

            var behavior1 = new MocValue();
            behavior1.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count1++;

                value = context.Input;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };

            var behavior2 = new MocValue();
            behavior2.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count2++;

                //-- Не обрабатываем
            };

            var behavior3 = new MocValue();
            behavior3.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count3++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior1.RequestToChangeValue(10);
                    behavior2.RequestToChangeValue(20);
                    behavior2.RequestToChangeValue(30);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, value);

                Assert.AreEqual(byte.MaxValue, behavior1.Value);
                Assert.AreEqual(byte.MaxValue, behavior2.Value);
                Assert.AreEqual(byte.MaxValue, behavior3.Value);

                Assert.AreEqual(2, count1);     //-- Функция вызываетя 2 раза !
                Assert.AreEqual(1, count2);
                Assert.AreEqual(0, count3);
            }


        }

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToChangeValueTM2()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

            int value = 0;

            var behavior1 = new MocValue();
            behavior1.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count1++;

                value = context.Input;

                //-- Значение установлено
                context.SetOutput(context.Input);

            };

            var behavior2 = new MocValue();
            behavior2.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count2++;

                //-- Значение установлено
                context.SetOutput(context.Input);

                throw new InvalidOperationException("Ошибка");
            };

            var behavior3 = new MocValue();
            behavior3.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count3++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior1.RequestToChangeValue(10);
                    behavior2.RequestToChangeValue(20);
                    behavior2.RequestToChangeValue(30);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, value);

                Assert.AreEqual(byte.MaxValue, behavior1.Value);
                Assert.AreEqual(byte.MaxValue, behavior2.Value);
                Assert.AreEqual(byte.MaxValue, behavior3.Value);

                Assert.AreEqual(2, count1);     //-- Функция вызываетя 2 раза !
                Assert.AreEqual(1, count2);
                Assert.AreEqual(0, count3);
            }


        }

        [TestMethod()]
        public void RequestToChangeValueTM_С()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

            int value = 0;

            var behavior1 = new MocValue();
            behavior1.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count1++;

                value = context.Input;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };

            var behavior2 = new MocValue();
            behavior2.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count2++;

                //-- Не обрабатываем
            };

            var behavior3 = new MocValue();
            behavior3.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count3++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior1.RequestToChangeValue(10);
                    behavior2.RequestToChangeValue(20);
                    behavior2.RequestToChangeValue(30);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(0, count1);
                Assert.AreEqual(0, count2);
                Assert.AreEqual(0, count3);

                Assert.AreEqual(0, value);

                Assert.AreEqual(byte.MaxValue, behavior1.Value);
                Assert.AreEqual(byte.MaxValue, behavior2.Value);
                Assert.AreEqual(byte.MaxValue, behavior3.Value);


            }


        }

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void RequestToChangeValueTM_E()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

            int count1EX = 0;

            int value = 0;

            var behavior1 = new MocValue();
            behavior1.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count1++;

                value = context.Input;

                //-- Значение установлено
                context.SetOutput(context.Input);

                if (count1 == 2)
                    throw new Exception("Исключение во 2 Раз");
            };
            behavior1.UnhandledExceptionEvent += (object sender, UnhandledExceptionEventArgs e) =>
            {
                count1EX++;
            };

            var behavior2 = new MocValue();
            behavior2.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count2++;

                //-- Не обрабатываем
            };

            var behavior3 = new MocValue();
            behavior3.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count3++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior1.RequestToChangeValue(10);
                    behavior2.RequestToChangeValue(20);
                    behavior2.RequestToChangeValue(30);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, value);

                Assert.AreEqual(byte.MaxValue, behavior1.Value);
                Assert.AreEqual(byte.MaxValue, behavior2.Value);
                Assert.AreEqual(byte.MaxValue, behavior3.Value);

                Assert.AreEqual(2, count1);     //-- Функция вызываетя 2 раза !
                Assert.AreEqual(1, count2);
                Assert.AreEqual(0, count3);

                Assert.AreEqual(1, count1EX);   //-- Функция вызываетя 2 раза !
            }


        }

        //=======================================================================

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void Requests()
        {
            int countChange = 0;
            int countUpdate = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                countChange++;

                //-- Значение установлено
                context.SetOutput(100);
            };
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                countUpdate++;

                //-- Значение установлено
                context.SetOutput(200);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);
                    behavior.RequestToUpdateValue();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);

                Assert.AreEqual(0, countChange);
                Assert.AreEqual(0, countUpdate);
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void Requests2()
        {
            int countChange = 0;
            int countUpdate = 0;

            var behavior = new MocValue();
            behavior.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                countChange++;

                //-- Значение установлено
                context.SetOutput(100);
            };
            behavior.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                countUpdate++;

                //-- Значение установлено
                context.SetOutput(200);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior.RequestToChangeValue(10);
                    behavior.RequestToUpdateValue();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(byte.MaxValue, behavior.Value);

                Assert.AreEqual(0, countChange);
                Assert.AreEqual(0, countUpdate);
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void Requests3()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int countUpdate = 0;

            int value = 0;

            var behavior1 = new MocValue();
            behavior1.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count1++;

                value = context.Input;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };

            var behavior2 = new MocValue();
            behavior2.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count2++;

                //-- Не обрабатываем
            };
            behavior2.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                countUpdate++;

                //-- Значение установлено
                context.SetOutput(200);
            };

            var behavior3 = new MocValue();
            behavior3.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count3++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior1.RequestToChangeValue(10);
                    behavior2.RequestToChangeValue(20);
                    behavior2.RequestToUpdateValue();
                    behavior2.RequestToChangeValue(30);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(0, count1);
                Assert.AreEqual(0, count2);
                Assert.AreEqual(0, count3);

                Assert.AreEqual(0, countUpdate);

                Assert.AreEqual(0, value);

                Assert.AreEqual(byte.MaxValue, behavior1.Value);
                Assert.AreEqual(byte.MaxValue, behavior2.Value);
                Assert.AreEqual(byte.MaxValue, behavior3.Value);
            }


        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod()]
        public void Requests3_C()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int countUpdate = 0;

            int value = 0;

            var behavior1 = new MocValue();
            behavior1.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count1++;

                value = context.Input;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };

            var behavior2 = new MocValue();
            behavior2.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count2++;

                //-- Не обрабатываем
            };
            behavior2.ValueUpdate += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                countUpdate++;

                //-- Значение установлено
                context.SetOutput(200);
            };

            var behavior3 = new MocValue();
            behavior3.ValueChange += (object sender, ValueBehaviorContext<ushort> context) =>
            {
                count3++;

                //-- Значение установлено
                context.SetOutput(context.Input);
            };


            try
            {
                using (var scope = new TransactionScope())
                {
                    //-- Исключение без подписки
                    behavior1.RequestToChangeValue(10);
                    behavior2.RequestToChangeValue(20);
                    behavior2.RequestToUpdateValue();
                    behavior2.RequestToChangeValue(30);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                Assert.AreEqual(0, count1);
                Assert.AreEqual(0, count2);
                Assert.AreEqual(0, count3);

                Assert.AreEqual(0, countUpdate);

                Assert.AreEqual(0, value);

                Assert.AreEqual(byte.MaxValue, behavior1.Value);
                Assert.AreEqual(byte.MaxValue, behavior2.Value);
                Assert.AreEqual(byte.MaxValue, behavior3.Value);
            }


        }
    }
}