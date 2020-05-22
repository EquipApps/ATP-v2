using System;
using System.Runtime.InteropServices;

namespace EquipApps.WorkBench.Tools.External.Internal
{
    internal static class Kernel32
    {
        /// <summary>
        /// Загрузка сборки.
        /// </summary>
        /// <param name="lpFileName">Путь</param>
        /// <returns>Указатель на сборку</returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public extern static IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Выгрузка сборки
        /// </summary>
        /// <param name="hModule">Указатель на сборку</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public extern static bool FreeLibrary(IntPtr hModule);
        
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        public extern static IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    }
}
