using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DynamicData;
using EquipApps.Mvc;
using EquipApps.Mvc.Infrastructure;

namespace EquipApps.WorkBench.Services.Tests
{
    [TestClass()]
    public class LogLevelCounterTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void LogLevelCounterTest()
        {
            var logLevelCounter = new LogLevelCounter(null);
        }

        [TestMethod()]
        public void LogLevelCounterTest1()
        {
            var sourceList           = new SourceList<LogEntry>();
            var sourceObservable    = sourceList.AsObservableList();
            var sourceConnext       = sourceObservable.Connect();
            var logLevelCounter     = new LogLevelCounter(sourceConnext);
        }

        [TestMethod()]
        public void DisposeTest()
        {
            var itemsCount = 100;
            LogLevelCount logCount = new LogLevelCount();

            var sourceList = new SourceList<LogEntry>();
            var sourceObservable = sourceList.AsObservableList();
            var sourceConnext = sourceObservable.Connect();
            var logLevelCounter = new LogLevelCounter(sourceConnext);
            var logLevelCountDi = logLevelCounter.Subscribe(x => logCount = x);

            //--------------------------------------------
            for (int i = 0; i < itemsCount; i++)
            {
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.dbug
                });
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.warn
                });
            }

            Assert.AreEqual(itemsCount, logCount.Countdbug);
            Assert.AreEqual(itemsCount, logCount.Countwarn);

            Assert.AreEqual(itemsCount, logLevelCounter.Сountdbug);
            Assert.AreEqual(0, logLevelCounter.Сountinfo);
            Assert.AreEqual(itemsCount, logLevelCounter.Сountwarn);
            Assert.AreEqual(0, logLevelCounter.Сountfail);

            sourceList.Clear();
            GC.Collect();

            Assert.AreEqual(0, logCount.Countdbug);
            Assert.AreEqual(0, logCount.Countwarn);

            Assert.AreEqual(0, logLevelCounter.Сountdbug);
            Assert.AreEqual(0, logLevelCounter.Сountinfo);
            Assert.AreEqual(0, logLevelCounter.Сountwarn);
            Assert.AreEqual(0, logLevelCounter.Сountfail);


            //--------------------------------------------
            logLevelCountDi.Dispose();

            for (int i = 0; i < itemsCount; i++)
            {
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.dbug
                });
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.warn
                });
            }

            Assert.AreEqual(0, logCount.Countdbug);
            Assert.AreEqual(0, logCount.Countwarn);

            Assert.AreEqual(itemsCount, logLevelCounter.Сountdbug);
            Assert.AreEqual(0, logLevelCounter.Сountinfo);
            Assert.AreEqual(itemsCount, logLevelCounter.Сountwarn);
            Assert.AreEqual(0, logLevelCounter.Сountfail);

            sourceList.Clear();
            GC.Collect();

            Assert.AreEqual(0, logCount.Countdbug);
            Assert.AreEqual(0, logCount.Countwarn);

            Assert.AreEqual(0, logLevelCounter.Сountdbug);
            Assert.AreEqual(0, logLevelCounter.Сountinfo);
            Assert.AreEqual(0, logLevelCounter.Сountwarn);
            Assert.AreEqual(0, logLevelCounter.Сountfail);


            //--------------------------------------------
            logLevelCounter.Dispose();

            for (int i = 0; i < itemsCount; i++)
            {
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.dbug
                });
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.warn
                });
            }

            Assert.AreEqual(0, logCount.Countdbug);
            Assert.AreEqual(0, logCount.Countwarn);

            Assert.AreEqual(0, logLevelCounter.Сountdbug);
            Assert.AreEqual(0, logLevelCounter.Сountinfo);
            Assert.AreEqual(0, logLevelCounter.Сountwarn);
            Assert.AreEqual(0, logLevelCounter.Сountfail);

            sourceList.Clear();
            GC.Collect();

            Assert.AreEqual(0, logCount.Countdbug);
            Assert.AreEqual(0, logCount.Countwarn);

            Assert.AreEqual(0, logLevelCounter.Сountdbug);
            Assert.AreEqual(0, logLevelCounter.Сountinfo);
            Assert.AreEqual(0, logLevelCounter.Сountwarn);
            Assert.AreEqual(0, logLevelCounter.Сountfail);

        }

        [TestMethod()]
        public void PERFOMANCETest3()
        {
            var itemsCount = 1000000;
            LogLevelCount logCount = new LogLevelCount();

            var sourceList       = new SourceList<LogEntry>();
            var sourceObservable = sourceList.AsObservableList();
            var sourceConnext    = sourceObservable.Connect();
            var logLevelCounter  = new LogLevelCounter(sourceConnext);
            var logLevelCountDi  = logLevelCounter.Subscribe(x => logCount = x);

            while (true)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    sourceList.Add(new LogEntry()
                    {
                        Level = LogLevel.dbug
                    });
                    sourceList.Add(new LogEntry()
                    {
                        Level = LogLevel.warn
                    });
                }

                Assert.AreEqual(itemsCount, logCount.Countdbug);
                Assert.AreEqual(itemsCount, logCount.Countwarn);

                sourceList.Clear();
                GC.Collect();

                Assert.AreEqual(0, logCount.Countdbug);
                Assert.AreEqual(0, logCount.Countwarn);

            }


        }

        [TestMethod()]
        public void SubscribeTest()
        {
            var itemsCount = 100;
            var logerCount = new LogLevelCount();

            //-- Создали спислок
            var sourceList       = new SourceList<LogEntry>();
            var sourceObservable = sourceList.AsObservableList();
            var sourceConnext    = sourceObservable.Connect();

            //-- Заполнили его.
            for (int i = 0; i < itemsCount; i++)
            {
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.dbug
                });
                sourceList.Add(new LogEntry()
                {
                    Level = LogLevel.warn
                });
            }

            //-- Подписались!
            var logLevelCounter = new LogLevelCounter(sourceConnext);

            Assert.AreEqual(0, logerCount.Countdbug);
            Assert.AreEqual(0, logerCount.Countinfo);
            Assert.AreEqual(0, logerCount.Countwarn);
            Assert.AreEqual(0, logerCount.Countfail);

            //-- Счетчики обновились!
            Assert.AreEqual(itemsCount, logLevelCounter.Сountdbug);
            Assert.AreEqual(0,          logLevelCounter.Сountinfo);
            Assert.AreEqual(itemsCount, logLevelCounter.Сountwarn);
            Assert.AreEqual(0,          logLevelCounter.Сountfail);


            //-- Подписались!
            var logLevelCountDi = logLevelCounter.Subscribe(x => logerCount = x);

            //-- Подписка обновилась
            Assert.AreEqual(itemsCount, logerCount.Countdbug);
            Assert.AreEqual(0,          logerCount.Countinfo);
            Assert.AreEqual(itemsCount, logerCount.Countwarn);
            Assert.AreEqual(0,          logerCount.Countfail);

            //-- Счетчики не изменились
            Assert.AreEqual(itemsCount, logLevelCounter.Сountdbug);
            Assert.AreEqual(0,          logLevelCounter.Сountinfo);
            Assert.AreEqual(itemsCount, logLevelCounter.Сountwarn);
            Assert.AreEqual(0,          logLevelCounter.Сountfail);
        }
    }
}