using EquipApps.WorkBench.Tools.External.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series
{
    public abstract class PshLibrary : Library
    {
        public PshLibrary(string dllPath) :base(dllPath)
        {

        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort IniTdel(ushort numb, ushort boardId);
        private IniTdel _initFunc;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort DeiniTdel(ushort numb);
        private DeiniTdel _deinitFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort GetVerdel();
        private GetVerdel _getVerFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort OutpuTdel(ushort numb, ushort onoff);
        private OutpuTdel _outputFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort SetuPdel(ushort num, uint volt, uint limVolt, uint limCurr);
        private SetuPdel _setupFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort StatuSdel(ushort num, ref uint volt, ref uint curr);
        private StatuSdel _statusFunc;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="numb"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public ushort INIT(ushort numb, ushort boardId)
        {
            var result = (ushort)0xFFFF;
            if (_initFunc != null) result = _initFunc(numb, boardId);
            return result;
        }

        /// <summary>
        /// Деинициализация
        /// </summary>
        /// <param name="numb"></param>
        /// <returns></returns>
        public ushort DEINIT(ushort numb)
        {
            var result = (ushort)0xFFFF;
            if (_deinitFunc != null) result = _deinitFunc(numb);
            return result;
        }

        /// <summary>
        /// Версия
        /// </summary>
        /// <returns></returns>
        public ushort GET_VER()
        {
            return _getVerFunc();
        }

        /// <summary>
        /// Вкл. / ВЫкл
        /// </summary>
        /// <param name="numb"></param>
        /// <param name="onoff">0 - Вкл.  1 - Выкл</param>
        /// <returns></returns>
        public ushort OUTPUT(ushort numb, ushort onoff)
        {
            var result = (ushort)0xFFFF;
            if (_outputFunc != null) result = _outputFunc(numb, onoff);
            return result;
        }

        /// <summary>
        /// Настройка
        /// </summary>
        /// <param name="num"></param>
        /// <param name="volt"></param>
        /// <param name="limVolt"></param>
        /// <param name="limCurr"></param>
        /// <returns></returns>
        public ushort SETUP(ushort num, uint volt, uint limVolt, uint limCurr) 
        {
            var result = (ushort)0xFFFF;
            if (_setupFunc != null) result = _setupFunc(num, volt, limVolt, limCurr);
            return result;
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
            var result = (ushort)0xFFFF;
            if (_statusFunc != null) result = _statusFunc(num, ref volt, ref curr);
            return result;
        }
    }
}
