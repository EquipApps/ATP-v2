using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405
{
    /// <summary>
    /// Однокональный источник питания PSP-405
    /// </summary>
    /// 
    /// <remarks>
    /// Использует общую библеотеку с PSP-2010.
    /// Если в ходе эксплуатации возникнут уникальные поведения, то избавляемся от общего класса!
    /// </remarks>
    /// 
    public class Psp405_Device : PS_Device, IDisposable
    {
        /// <summary>
        /// Базовый путь к библеотеке
        /// </summary>
        public static string DLL_Path = "C:\\Windows\\AAPCtrlDev\\PSP_2010_DW_Instek.dll";

        //================================================================================

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="number">Порядковый номер</param>
        /// <param name="port">Номер COM</param>
        public Psp405_Device(ushort number, ushort port)
        {
            InitializeComponent(number, port, DLL_Path);
        }

        //================================================================================

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }


                library = null;

                disposedValue = true;
            }
        }


        ~Psp405_Device()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
