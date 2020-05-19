using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EquipApps.WorkBench.Tools.External.Internal
{
    /// <summary>
    /// Библеотека
    /// </summary>
    public class Library : IDisposable
    {
        /// <summary>
        /// Дискриптор библеотеки
        /// </summary>
        private IntPtr _hDll = IntPtr.Zero;

        /// <summary>
        /// Путь к библеотеке
        /// </summary>
        private string _pDll = string.Empty;

        /// <summary>
        /// Конструктор
        /// </summary>       
        public Library(string dllPath)
        {
            if (string.IsNullOrWhiteSpace(dllPath)) 
                throw new InvalidOperationException(nameof(dllPath));

            _pDll = dllPath;
        }

        /// <summary>
        /// Загружает библеотеку
        /// </summary>
        protected virtual void InitializeComponent()
        {
            //-- 1) Проверка.
            if (_hDll != IntPtr.Zero)
                throw new InvalidOperationException("Библиотека уже загруженна.");

            //----- 2) Выполнить. Загрузка библеотеки.
            _hDll = Kernel32.LoadLibrary(_pDll);

            //----- 3) Проверка. Загружеена ли библеотека ?
            if (_hDll == IntPtr.Zero)
                throw new InvalidOperationException($"При загрузки библиотеки \"{_pDll}\" произошла ошибка.");
        }

        protected T GetFunc<T>(string FuncName) where T : class
        {
            var ptr = Kernel32.GetProcAddress(_hDll, FuncName);

            if (ptr != IntPtr.Zero)
            {
                var _initFunc = Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;

                return _initFunc;
            }
            {
                throw new InvalidOperationException($"Библиотека \"{_pDll}\" ошибка загрузки функции: {FuncName}");
            }

        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.

                if (_hDll != IntPtr.Zero)
                {
                    bool result = Kernel32.FreeLibrary(_hDll);
                    Debug.Assert(result, "Ошибка очистки ");
                }

                // Set large fields to null.
            

                disposedValue = true;
            }
        }

        ~Library()
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
