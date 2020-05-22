﻿using EquipApps.WorkBench.Tools.External.Internal;
using System.Runtime.InteropServices;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{
    /// <summary>
    /// Библеотеки одноконального источника питания
    /// </summary>
    public class PSxLibrary : Library
    {
        private const ushort nullError = 0xFFFF;

        //TODO: Добавить локер!

        public PSxLibrary()
        {

        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort IniTdel(ushort numb, ushort com);
        private IniTdel _initFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort DeiniTdel(ushort numb);
        private DeiniTdel _deinitFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort GetVerdel();
        private GetVerdel _getVerFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort SetuPdel(ushort num, uint volt, uint limVolt, uint limCurr);
        private SetuPdel _setupFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort OutpuTdel(ushort numb, ushort onoff);
        private OutpuTdel _outputFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort StatuSdel(ushort num, ref uint volt, ref uint curr);
        private StatuSdel _statusFunc;

        internal override void InitializeComponent(string dllPath)
        {
            base.InitializeComponent(dllPath);

            _initFunc = GetFunc<IniTdel>("INIT");
            _deinitFunc = GetFunc<DeiniTdel>("DEINIT");
            _getVerFunc = GetFunc<GetVerdel>("GET_VER");

            _outputFunc = GetFunc<OutpuTdel>("OUTPUT");
            _setupFunc = GetFunc<SetuPdel>("SETUP");
            _statusFunc = GetFunc<StatuSdel>("STATUS");
        }
        
        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="numb"></param>
        /// <param name="com"></param>
        /// <returns></returns>
        public ushort INIT(ushort numb, ushort com)
        {
            return _initFunc == null ? nullError : _initFunc(numb, com);
        }

        /// <summary>
        /// Деинициализация
        /// </summary>
        /// <param name="numb"></param>
        /// <returns></returns>
        public ushort DEINIT(ushort numb)
        {
            return _deinitFunc == null ? nullError : _deinitFunc(numb);           
        }

        /// <summary>
        /// Вкл. / ВЫкл
        /// </summary>
        /// <param name="numb"></param>
        /// <param name="onoff">0 - Вкл.  1 - Выкл</param>
        /// <returns></returns>
        public ushort OUTPUT(ushort numb, ushort onoff)
        {
            return _outputFunc == null ? nullError : _outputFunc(numb, onoff);
        }

        /// <summary>
        /// Настройка
        /// </summary>
        /// <param name="num"></param>
        /// <param name="volt"></param>
        /// <param name="limVolt"></param>
        /// <param name="limCurr"></param>
        /// <returns></returns>
        public ushort SETUP(ushort numb, uint volt, uint limVolt, uint limCurr)
        {
            return _setupFunc == null ? nullError : _setupFunc(numb, volt, limVolt, limCurr);            
        }

        /// <summary>
        /// Состояние
        /// </summary>
        /// <param name="num"></param>
        /// <param name="volt"></param>
        /// <param name="curr"></param>
        /// <returns></returns>
        public ushort STATUS(ushort num, ref uint volt, ref uint curr)
        {
            return _statusFunc == null ? nullError : _statusFunc(num, ref volt, ref curr);
        }
    }
}
