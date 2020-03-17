using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Tests
{
    public class MokBehavior : IHardwareBehavior
    {
        public IHardware Hardware { get; set; }

        public void Attach()
        {
            IsAttached = true;
            ConuntAttach++;
        }

        public bool IsAttached { get; private set; } = false;
        public int ConuntAttach { get; private set; } = 0;
    }
}
