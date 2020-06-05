using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipApps.Hardware.Behaviors.Digital;
using System;
using System.Collections.Generic;
using System.Text;
using EquipApps.Hardware.Abstractions;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.Digital.Tests
{
    [TestClass()]
    public class DigitalPort8Tests
    {
        public class MocValue : DigitalPort8
        {
            public MocValue()
            {
                SetValue(byte.MaxValue);
            }
        }

        private static void Validate(byte etalPort, DigitalPort8 port)
        {
            Assert.AreEqual(etalPort, port.Value);

            for (int i = 0; i < port.Lines.Count; i++)
            {
                var mask = 1 << i;
                var etal = etalPort & mask;

                if (etal == 0)
                {
                    Assert.AreEqual(Digit.Nil, port.Lines[i].Value);
                }
                else
                {
                    Assert.AreEqual(Digit.One, port.Lines[i].Value);
                }
            }
        }


        [TestMethod()]
        public void DigitalPortTest()
        {
            var port = new MocValue();

            Assert.IsNotNull(port);
            Assert.AreEqual (byte.MaxValue, port.Value);

            foreach (var line in port.Lines)
            {
                Assert.AreEqual(Digit.One, line.Value);
            }
        }

        [TestMethod()]
        public void RequestToUpdateValue()
        {
            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                context.SetOutput(0xAA);
            };

            port.RequestToUpdateValue();

            Validate(0xAA, port);
        }

        [TestMethod()]
        public void RequestToUpdateValue_CallCount()
        {
            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            port.RequestToUpdateValue();
            port.RequestToUpdateValue();

            Assert.AreEqual(2, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void RequestToUpdateValue_CallCount_Transaction()
        {
            /*
             * Обертка в транзакцию.
             * Каждый запрос на обновление линии (в рамках транзакции) генерирует ОДНО событие порта.
             */

            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.RequestToUpdateValue();
                port.RequestToUpdateValue();

                transactionScope.Complete();
            }
           

            Assert.AreEqual(1, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void RequestToUpdateValue_CallCount_Transaction_Dispose()
        {
            /*
             * Обертка в транзакцию.
             * Каждый запрос на обновление линии (в рамках транзакции) генерирует ОДНО событие порта.
             */

            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.RequestToUpdateValue();
                port.RequestToUpdateValue();
            }


            Assert.AreEqual(0, callCount);
            Validate(byte.MaxValue, port);
        }


        [TestMethod()]
        public void LineTest_RequestToUpdateValue()
        {
            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            port.Lines[0].RequestToUpdateValue();

            Assert.AreEqual(1, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void LineTest_RequestToUpdateValue_CallCount()
        {
            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            port.Lines[0].RequestToUpdateValue();
            port.Lines[0].RequestToUpdateValue();

            Assert.AreEqual(2, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void LineTest_RequestToUpdateValue_CallCount_Transaction()
        {
            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.Lines[0].RequestToUpdateValue();
                port.Lines[2].RequestToUpdateValue();

                transactionScope.Complete();
            }

            Assert.AreEqual(1, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void LineTest_RequestToUpdateValue_CallCount_Transaction_Dispose()
        {
            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(callCount);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.Lines[0].RequestToUpdateValue();
                port.Lines[2].RequestToUpdateValue();

            }

            Assert.AreEqual(0, callCount);
            Validate(byte.MaxValue, port);
        }
        
        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void LineTest_RequestToUpdateValue_CallCount_Transaction_Exception()
        {
            /*
             * Обертка в транзакцию.
             * Если во время транзакции произлошло исключение, то порт не обновляет значение
             */

            byte callCount = 0;

            var port = new MocValue();
            port.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xAA);
                throw new InvalidOperationException("Ошибка");
            };

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    port.Lines[0].RequestToUpdateValue();
                    port.Lines[2].RequestToUpdateValue();

                    transactionScope.Complete();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }            
            finally
            {
                Assert.AreEqual(1, callCount);
                Validate(byte.MaxValue, port);
            }
        }



        [TestMethod()]
        public void PCI_LineTest_2_RequestToUpdateValue_CallCount()
        {
            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port1 = new MocValue();
            var port2 = new MocValue();

            port1.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xAA);
            };
            port2.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xBB);
            };

            port1.Lines[0].RequestToUpdateValue();
            port2.Lines[0].RequestToUpdateValue();

            Assert.AreEqual(2, callCount);

            Validate(0xAA, port1);
            Validate(0xBB, port2);
        }

        [TestMethod()]
        public void PCI_LineTest_2_RequestToUpdateValue_CallCount_Transaction()
        {
            /*
             * Обертка в транзакцию.
             * Каждый запрос на обновление линии (в рамках транзакции) генерирует ОДНО событие для каждого порта.
             */

            byte callCount = 0;

            var port1 = new MocValue();
            var port2 = new MocValue();

            port1.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xAA);
            };
            port2.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xBB);
            };

            using (var transactionScope = new TransactionScope())
            {
                port1.Lines[0].RequestToUpdateValue();
                port2.Lines[2].RequestToUpdateValue();

                transactionScope.Complete();
            }

            Assert.AreEqual(2, callCount);

            Validate(0xAA, port1);
            Validate(0xBB, port2);
        }

        [TestMethod()]
        public void PCI_LineTest_2_RequestToUpdateValue_CallCount_Transaction_Dispose()
        {
            /*
             * Обертка в транзакцию.
             * Если транзакция отменена, то событие порта не генерируется
             */

            byte callCount = 0;

            var port1 = new MocValue();
            var port2 = new MocValue();

            port1.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xAA);
            };
            port2.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xBB);
            };

            using (var transactionScope = new TransactionScope())
            {
                port1.Lines[0].RequestToUpdateValue();
                port2.Lines[2].RequestToUpdateValue();
            }

            Assert.AreEqual(0, callCount);

            Validate(byte.MaxValue, port1);
            Validate(byte.MaxValue, port2);
        }

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void PCI_LineTest_2_RequestToUpdateValue_CallCount_Transaction_Exception()
        {
            /*
             * Обертка в транзакцию.
             * Если во время транзакции произлошло исключение, то порт не обновляет значение
            */

            byte callCount = 0;

            var port1 = new MocValue();
            var port2 = new MocValue();
            var port3 = new MocValue();

            port1.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                context.SetOutput(0xAA);
            };
            port2.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                callCount++;
                throw new InvalidOperationException("Ошибка");
            };
            port3.ValueUpdate += (object sender, ValueBehaviorContext<byte> context) =>
            {
                //-- Данный вызов не должен произойти!
                callCount++;
                context.SetOutput(0xCC);
            };

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    port1.Lines[0].RequestToUpdateValue();
                    port2.Lines[2].RequestToUpdateValue();
                    port3.Lines[3].RequestToUpdateValue();

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Assert.AreEqual(2, callCount);

                Validate(byte.MaxValue, port1);
                Validate(byte.MaxValue, port2);
                Validate(byte.MaxValue, port3);
            }
        }
    }
}