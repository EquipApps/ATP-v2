using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{
    /// <summary>
    /// Инфроструктура источника питания
    /// </summary>
    public interface IPowerSource
    {
        ushort Number { get; }
        ushort Port { get; }

        void Init();
        void PowerOff();
        void PowerOn();
        void Setup(uint voltage, uint limVoltage, uint limCurrent);
        void Status(ref uint volt, ref uint curr);
    }
}