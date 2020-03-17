using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Tests
{
    public class MokHardware : IHardware
    {
        public MokHardware(string key)
        {
            this.Key = key;
        }

        public string Key { get; }

        public IHardwareBehaviorCollection Behaviors => _internelColection;

        public string Name { get; set; }
        public string Description { get; set; }

        public IHardwareBehaviorCollection _internelColection;
    }
}
