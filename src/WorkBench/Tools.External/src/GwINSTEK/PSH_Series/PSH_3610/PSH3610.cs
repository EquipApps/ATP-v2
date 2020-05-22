using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610
{
    /// <summary>
    /// Устройство PSH-3610
    /// </summary>
    public class PSH3610 : PSxDevice, IDisposable
    {
        /// <summary>
        /// Базовый путь к библеотеке
        /// </summary>
        public static string DLL_Path = "C:\\Windows\\AAPCtrlDev\\PSH_3610_GW_Instek.dll";

        //================================================================================

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="number">Порядковый номер</param>
        /// <param name="comport">Номер COM</param>
        public PSH3610(ushort number, ushort comport)
        {
            InitializeComponent(number, comport, DLL_Path);
        }

        //================================================================================

        //TODO: Проверить коды ошибок библеотеки! Сравнить с кодами ошибок PSP!

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


        ~PSH3610()
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
