using EquipApps.WorkBench.Tools.External.Internal;
using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PS_Device : IPowerSource
    {
        protected PS_Library library;

        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public ushort Number { get; private set; }

        /// <summary>
        /// Номер COM
        /// </summary>
        public ushort Port { get; private set; }


        protected virtual void InitializeComponent(ushort number, ushort port, string path)
        {
            Number = number;
            Port = port;

            //-- Загрузка Библеотеки
            library = LibraryCache.Instance.GetOrCreate<PS_Library>(path);
        }


        public void Initalize()
        {
            var error = library.INIT(Number, Port);
            if (error != 0)
            {
                throw new InvalidProgramException(nameof(Initalize));
            }
        }

        public void Setup(uint voltage, uint limVoltage, uint limCurrent)
        {
            var error = library.SETUP(Number, voltage, limVoltage, limCurrent);
            if (error != 0)
            {
                throw new InvalidProgramException(nameof(Setup));
            }
        }

        public void Status(ref uint volt, ref uint curr)
        {
            var error = library.STATUS(Number, ref volt, ref curr);
            if (error != 0)
            {
                throw new InvalidProgramException(nameof(Status));
            }
        }

        public void PowerOn()
        {
            var error = library.OUTPUT(Number, 1);
            if (error != 0)
            {
                throw new InvalidProgramException(nameof(PowerOn));
            }
        }

        public void PowerOff()
        {
            var error = library.OUTPUT(Number, 1);
            if (error != 0)
            {
                throw new InvalidProgramException(nameof(PowerOn));
            }
        }
    }
}
