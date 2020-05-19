using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetCurrentProcessId();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetCurrentThreadId();
















        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Access"></param>
        /// <param name="Mode"></param>
        /// <param name="Security"></param>
        /// <param name="Disposition"></param>
        /// <param name="flags"></param>
        /// <param name="hTemp"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            string FileName,
            uint Access,
            uint Mode,
            IntPtr Security,
            uint Disposition,
            uint flags,
            IntPtr hTemp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDev"></param>
        /// <param name="CTRLCode"></param>
        /// <param name="InBuffer"></param>
        /// <param name="InBufferSize"></param>
        /// <param name="OutBuffer"></param>
        /// <param name="OutBufferSize"></param>
        /// <param name="ByteReturn"></param>
        /// <param name="ov"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeviceIoControl(
            IntPtr hDev,
            uint CTRLCode,
            IntPtr InBuffer,
            uint InBufferSize,
            IntPtr OutBuffer,
            uint OutBufferSize,
            out uint ByteReturn,
            IntPtr ov);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDev"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hDev);
    }
}
