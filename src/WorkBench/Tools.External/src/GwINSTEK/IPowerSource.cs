using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{

    public interface IPowerSource
    {
        ushort Number { get; }
        ushort Port { get; }

        void Initalize();
        void PowerOff();
        void PowerOn();
        void Setup(uint voltage, uint limVoltage, uint limCurrent);
        void Status(ref uint volt, ref uint curr);
    }
}