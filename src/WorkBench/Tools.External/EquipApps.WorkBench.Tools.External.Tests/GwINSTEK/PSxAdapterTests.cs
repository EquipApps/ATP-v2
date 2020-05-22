using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using EquipApps.Hardware;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.Tests
{
    [TestClass()]
    public class PSxAdapterTests
    {
        [TestMethod()]
        public void PSxAdapter_InitializeTest()
        {
            var key = "power";
            var hardware = new EquipApps.Hardware.Hardware(key);

            var feature = new TooHardwareFeature();
                feature.HardwareCollection.AddOrUpdate(hardware);

            var device  = new TooPowerSource();

            var adapter = new PSxAdapter<TooPowerSource>();

            adapter.Initialize(feature, device, key);

            Assert.AreEqual (device,   adapter.Device);
            Assert.AreEqual (key,      adapter.DeviceName);
            Assert.AreEqual (feature, adapter.HardwareFeature);

            Assert.IsNotNull (adapter.ToggleBehavior);

            
        }
    }

    

    public class TooHardwareFeature : IHardwareFeature
    {
        public TooHardwareFeature()
        {
            HardwareAdapters = new List<IHardwareAdapter>();
            HardwareCollection = new HardwareCollection();
        }

        public List<IHardwareAdapter> HardwareAdapters { get; }

        public IHardwareCollection HardwareCollection { get; }
    }

    public class TooPowerSource : IPowerSource
    {
        public ushort Number => throw new NotImplementedException();

        public ushort Port => throw new NotImplementedException();

        public void Initalize()
        {
            throw new NotImplementedException();
        }

        public void PowerOff()
        {
            throw new NotImplementedException();
        }

        public void PowerOn()
        {
            throw new NotImplementedException();
        }

        public void Setup(uint voltage, uint limVoltage, uint limCurrent)
        {
            throw new NotImplementedException();
        }

        public void Status(ref uint volt, ref uint curr)
        {
            throw new NotImplementedException();
        }
    }
}