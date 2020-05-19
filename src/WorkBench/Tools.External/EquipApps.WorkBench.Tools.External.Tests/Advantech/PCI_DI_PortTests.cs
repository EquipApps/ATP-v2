using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EquipApps.Hardware;
using System.Transactions;

namespace EquipApps.WorkBench.Tools.External.Advantech.Tests
{
    [TestClass()]
    public class PCI_DI_PortTests
    {

        [TestMethod()]
        public void PCI_PortTest_DefaultValue()
        {
            var port = new PCI_DI_Port(1);

            Assert.IsNotNull(port);
            Assert.AreEqual(0x00, port.Value);

            foreach (var line in port.Lines)
            {
                Assert.AreEqual(0x00, line.Value);
            }
        }

        [TestMethod()]
        public void PCI_PortTest_RequestToUpdateValue()
        {
            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                behavior.SetValue(0xAA);
            };

            port.RequestToUpdateValue();

            Validate(0xAA, port);
        }

        [TestMethod()]
        public void PCI_PortTest_RequestToUpdateValue_CallCount()
        {
            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(callCount);
            };

            port.RequestToUpdateValue();
            port.RequestToUpdateValue();

            Assert.AreEqual(2, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void PCI_PortTest_RequestToUpdateValue_CallCount_Transaction()
        {
            /*
             * Обертка в транзакцию.
             * Каждый запрос на обновление линии (в рамках транзакции) генерирует ОДНО событие порта.
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.RequestToUpdateValue();
                port.RequestToUpdateValue();

                transactionScope.Complete();
            }

            Assert.AreEqual(1, callCount);
            Validate(0xAA, port);
        }

        [TestMethod()]
        public void PCI_PortTest_RequestToUpdateValue_CallCount_Transaction_Dispose()
        {
            /*
             * Обертка в транзакцию.
             * Если транзакция отменена, то событие порта не генерируется
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(callCount);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.RequestToUpdateValue();
                port.RequestToUpdateValue();
            }

            Validate(0, port);
        }

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void PCI_PortTest_RequestToUpdateValue_CallCount_Transaction_Exception()
        {
            /*
             * Обертка в транзакцию.
             * Если во время транзакции произлошло исключение, то порт не обновляет значение
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xFF);
                throw new InvalidOperationException("Ошибка");
            };

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    port.RequestToUpdateValue();
                    port.RequestToUpdateValue();

                    transactionScope.Complete();
                }
            }
            finally
            {
                Validate(0, port);
            }
        }

        [TestMethod()]
        public void PCI_PortTest_2_RequestToUpdateValue_CallCount()
        {
            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xBB);
            };

            port1.RequestToUpdateValue();
            port2.RequestToUpdateValue();

            Assert.AreEqual(2, callCount);

            Validate(0xAA, port1);
            Validate(0xBB, port2);
        }

        [TestMethod()]
        public void PCI_PortTest_2_RequestToUpdateValue_CallCount_Transaction()
        {
            /*
             * Обертка в транзакцию.
             * Каждый запрос на обновление линии (в рамках транзакции) генерирует ОДНО событие для каждого порта.
             */

            byte callCount = 0;

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xBB);
            };

            using (var transactionScope = new TransactionScope())
            {
                port1.RequestToUpdateValue();
                port2.RequestToUpdateValue();

                transactionScope.Complete();
            }

            Assert.AreEqual(2, callCount);

            Validate(0xAA, port1);
            Validate(0xBB, port2);
        }

        [TestMethod()]
        public void PCI_PortTest_2_RequestToUpdateValue_CallCount_Transaction_Dispose()
        {
            /*
             * Обертка в транзакцию.
             * Если транзакция отменена, то событие порта не генерируется
             */

            byte callCount = 0;

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xBB);
            };

            using (var transactionScope = new TransactionScope())
            {
                port1.RequestToUpdateValue();
                port2.RequestToUpdateValue();
            }

            Assert.AreEqual(0, callCount);

            Validate(0x00, port1);
            Validate(0x00, port2);
        }

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void PCI_PortTest_2_RequestToUpdateValue_CallCount_Transaction_Exception()
        {
            /*
             * Обертка в транзакцию.
             * Если во время транзакции произлошло исключение, то порт не обновляет значение
            */

            byte callCount = 0;

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);
            var port3 = new PCI_DI_Port(3);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                throw new InvalidOperationException("Ошибка");
            };
            port3.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                //-- Данный вызов не должен произойти!
                callCount++;
                behavior.SetValue(0xCC);
            };

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    port1.RequestToUpdateValue();
                    port2.RequestToUpdateValue();
                    port3.RequestToUpdateValue();

                    transactionScope.Complete();
                }
            }
            finally
            {
                Assert.AreEqual(2, callCount);

                Validate(0x00, port1);
                Validate(0x00, port2);
                Validate(0x00, port3);
            }
        }




        [TestMethod()]
        public void PCI_LineTest_RequestToUpdateValue()
        {
            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {

                behavior.SetValue(0xAA);
            };

            port.Lines[0].RequestToUpdateValue();

            Validate(0xAA, port);
        }

        [TestMethod()]
        public void PCI_LineTest_RequestToUpdateValue_CallCount()
        {
            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(callCount);
            };

            port.Lines[0].RequestToUpdateValue();
            port.Lines[0].RequestToUpdateValue();

            Assert.AreEqual(2, callCount);
            Validate(callCount, port);
        }

        [TestMethod()]
        public void PCI_LineTest_RequestToUpdateValue_CallCount_Transaction()
        {
            /*
             * Обертка в транзакцию.
             * Каждый запрос на обновление линии (в рамках транзакции) генерирует ОДНО событие порта.
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.Lines[0].RequestToUpdateValue();
                port.Lines[2].RequestToUpdateValue();

                transactionScope.Complete();
            }

            Assert.AreEqual(1, callCount);
            Validate(0xAA, port);
        }

        [TestMethod()]
        public void PCI_LineTest_RequestToUpdateValue_CallCount_Transaction_Dispose()
        {
            /*
             * Обертка в транзакцию.
             * Если транзакция отменена, то событие порта не генерируется
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(callCount);
            };

            using (var transactionScope = new TransactionScope())
            {
                port.Lines[0].RequestToUpdateValue();
                port.Lines[2].RequestToUpdateValue();
            }

            Validate(0, port);
        }

        [ExpectedException(typeof(TransactionAbortedException))]
        [TestMethod()]
        public void PCI_LineTest_RequestToUpdateValue_CallCount_Transaction_Exception()
        {
            /*
             * Обертка в транзакцию.
             * Если во время транзакции произлошло исключение, то порт не обновляет значение
             */

            byte callCount = 0;

            var port = new PCI_DI_Port(1);
            port.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xFF);
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
            finally
            {
                Validate(0, port);
            }
        }

        [TestMethod()]
        public void PCI_LineTest_2_RequestToUpdateValue_CallCount()
        {
            /*
             * Каждый запрос на обновление линии генерирует событие порта.
             */

            byte callCount = 0;

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xBB);
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

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xBB);
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

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xBB);
            };

            using (var transactionScope = new TransactionScope())
            {
                port1.Lines[0].RequestToUpdateValue();
                port2.Lines[2].RequestToUpdateValue();
            }

            Assert.AreEqual(0, callCount);

            Validate(0x00, port1);
            Validate(0x00, port2);
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

            var port1 = new PCI_DI_Port(1);
            var port2 = new PCI_DI_Port(2);
            var port3 = new PCI_DI_Port(3);

            port1.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                behavior.SetValue(0xAA);
            };
            port2.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                callCount++;
                throw new InvalidOperationException("Ошибка");
            };
            port3.ValueUpdate += (ValueBehaviorBase<byte> behavior) =>
            {
                //-- Данный вызов не должен произойти!
                callCount++;
                behavior.SetValue(0xCC);
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
            finally
            {
                Assert.AreEqual(2, callCount);

                Validate(0x00, port1);
                Validate(0x00, port2);
                Validate(0x00, port3);
            }
        }





















        private static void Validate(byte etalPort, PCI_DI_Port port)
        {
            Assert.AreEqual(etalPort, port.Value);

            for (int i = 0; i < port.Lines.Length; i++)
            {
                var mask = 1 >> i;
                var etal = etalPort & mask;

                if (etal == 0)
                {
                    Assert.AreEqual(0, port.Lines[i].Value);
                }
                else
                {
                    Assert.AreEqual(1, port.Lines[i].Value);
                }
            }
        }

        
    }
}