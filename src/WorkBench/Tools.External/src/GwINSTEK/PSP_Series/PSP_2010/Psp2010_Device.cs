using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_2010
{
    /// <summary>
    /// Однокональный источник питания PSP-2010
    /// </summary>
    public class Psp2010_Device : PS_Device, IDisposable
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
        /// <param name="comport">Номер COM</param>
        public Psp2010_Device(ushort number, ushort comport)
        {
            InitializeComponent(number, comport, DLL_Path);
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

       
         ~Psp2010_Device()
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
