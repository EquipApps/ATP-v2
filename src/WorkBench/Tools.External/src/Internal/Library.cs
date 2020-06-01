using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EquipApps.WorkBench.Tools.External.Internal
{
    /// <summary>
    /// Библеотека
    /// </summary>
    public abstract class Library : IDisposable
    {
        public string Path { get; private set; }

        /// <summary>
        /// Дискриптор библеотеки
        /// </summary>
        private IntPtr _hDll = IntPtr.Zero;

        /// <summary>      
        /// <para>Загружает библеотеку.</para>
        /// <para>Функция вызывается при создании экземпляра библиотеки в фабрике</para>
        /// </summary>
        internal virtual void InitializeComponent(string dllPath)
        {
            //-- 1) Проверка.
            if (_hDll != IntPtr.Zero)
                throw new InvalidOperationException("Библиотека уже загруженна.");

            //-- 2) Выполнить. Загрузка библеотеки.
            _hDll = Kernel32.LoadLibrary(dllPath);

            //-- 3) Проверка. Загружеена ли библеотека ?
            if (_hDll == IntPtr.Zero)
                throw new InvalidOperationException("При загрузки библиотеки произошла ошибка.");

            //-- Сохраняем путь.
            Path = dllPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FuncName"></param>
        /// <returns></returns>
        protected T GetFunc<T>(string FuncName) where T : class
        {
            var ptr = Kernel32.GetProcAddress(_hDll, FuncName);

            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException($"Ошибка загрузки функции: {FuncName}");

            return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
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
                    Debug.Assert(result, "Ошибка очистки");

                    _hDll = IntPtr.Zero;
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
