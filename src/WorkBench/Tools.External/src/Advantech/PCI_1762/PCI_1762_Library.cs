using EquipApps.WorkBench.Tools.External.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.Advantech.PCI_1762
{
    public class PCI_1762_Library : Library
    {
        public PCI_1762_Library(IOptions<PCI_1762_Options> options) : 
            base(options?.Value?.DllPath)
        {

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Trelleys : IDisposable
        {
            public IntPtr Relayz;
            public byte Size;

            public Trelleys(params byte[] Nums) : this()
            {
                Alloc(Nums);
            }

            //Выделяем память,... 
            private void Alloc(params byte[] Nums)
            {
                //в рзмере 256 byte
                this.Relayz = Marshal.AllocHGlobal(Nums.Length);

                //Копируем массив
                Marshal.Copy(Nums, 0, this.Relayz, Nums.Length);

                //Сохраняем размер массива
                this.Size = (byte)Nums.Length;

            }
            private void Free()
            {
                if (this.Relayz != null)
                {
                    Marshal.FreeHGlobal(this.Relayz);
                }
            }

            public void Dispose()
            {
                Free();
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort IniTdel(ushort numb, ushort boardId);
        private IniTdel _initFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort DeiniTdel(ushort numb);
        private DeiniTdel _deinitFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort ADD_Rdel(ushort numb, byte relley);
        private ADD_Rdel _addRFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort SetRdel(ushort numb, Trelleys relleys);
        private SetRdel _setRFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort setR_SaveState(ushort numb, Trelleys relleys);
        private setR_SaveState _setR_SaveStateFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort DelRdel(ushort numb, byte relley);
        private DelRdel _delRFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort DelRMasSdel(ushort numb, Trelleys relleys);
        private DelRMasSdel _delRMassFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort DelAlldel(ushort numb);
        private DelAlldel _delAllFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort GetDIdel(ushort numb, byte portNum, ref byte portVal);
        private GetDIdel _getDiFunc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ushort GetVerdel();
        private GetVerdel _getVerFunc;

        protected override void InitializeComponent()
        {
            base.InitializeComponent();


            this._initFunc      = base.GetFunc<IniTdel>("INIT");
            this._deinitFunc    = base.GetFunc<DeiniTdel>("DEINIT");

            this._addRFunc      = base.GetFunc<ADD_Rdel>("ADD_R");
            this._setRFunc      = base.GetFunc<SetRdel>("SET_R");
            this._setR_SaveStateFunc = base.GetFunc<setR_SaveState>("SET_R_SaveState");

            this._delRFunc = base.GetFunc<DelRdel>("DEL_R");
            this._delRMassFunc = base.GetFunc<DelRMasSdel>("DEL_R_MASS");
            this._delAllFunc = base.GetFunc<DelAlldel>("DEL_ALL");

            this._getDiFunc = base.GetFunc<GetDIdel>("GET_DI");
            this._getVerFunc = base.GetFunc<GetVerdel>("GET_VER");

        }

        /// <summary>
        /// Функция инициализации PCI_1762
        /// </summary>
        /// <param name="NUM">Порядковй номер платы</param>
        /// <param name="Board_ID">Номер перемычки на плате</param>
        /// <returns>Результат инициализации</returns>
        public ushort INIT(ushort NUM, ushort Board_ID)
        {
            var result = (ushort)0xFFFF;
            if (_initFunc != null) result = _initFunc(NUM, Board_ID);
            return result;
        }

        /// <summary>
        /// Деинициализация PCI_1762
        /// </summary>        
        public ushort DEINIT(ushort NUM)
        {
            var result = (ushort)0xFFFF;
            if (_deinitFunc != null) result = _deinitFunc(NUM);
            return result;
        }

        /// <summary>
        /// Замкнут реле
        /// </summary>      
        public ushort ADD_R(ushort NUM, byte RELEY)
        {
            var result = (ushort)0xFFFF;
            if (_addRFunc != null) result = _addRFunc(NUM, RELEY);
            return result;
        }

        /// <summary>
        /// Замыкает набор реле. но перед этип размыкает.
        /// </summary>       
        public ushort SET_R(ushort NUM, byte[] RELEYS)
        {
            var result = (ushort)0xFFFF;

            using (Trelleys relley = new Trelleys(RELEYS))
            {
                if (_setRFunc != null) result = _setRFunc(NUM, relley);
            }
            return result;
        }

        /// <summary>
        /// Замыкает набор реле
        /// </summary>            
        public ushort SET_R_SaveState(ushort NUM, byte[] RELEYS)
        {
            var result = (ushort)0xFFFF;

            using (Trelleys relley = new Trelleys(RELEYS))
            {
                if (_setR_SaveStateFunc != null)
                {
                    result = _setR_SaveStateFunc(NUM, relley);
                    System.Threading.Thread.Sleep(5);               //--- Из АТП. Взял задержки
                }
            }
            return result;
        }

        /// <summary>
        /// Разомкнуть реле
        /// </summary>      
        public ushort DEL_R(ushort NUM, byte RELEY)
        {
            var result = (ushort)0xFFFF;
            if (_delRFunc != null) result = _delRFunc(NUM, RELEY);
            return result;
        }

        /// <summary>
        /// Разомкнуть набор реле.
        /// </summary>            
        public ushort DEL_R_MASS(ushort NUM, byte[] RELEYS)
        {
            var result = (ushort)0xFFFF;

            using (Trelleys relley = new Trelleys(RELEYS))
            {
                if (_delRMassFunc != null)
                {
                    result = _delRMassFunc(NUM, relley);
                    System.Threading.Thread.Sleep(5);               //--- Из АТП. Взял задержки
                }
            }
            return result;
        }

        /// <summary>
        /// Разомкнуть все реле.
        /// </summary>               
        public ushort DEL_ALL(ushort NUM)
        {
            var result = (ushort)0xFFFF;
            if (_delAllFunc != null) result = _delAllFunc(NUM);
            return result;
        }

        /// <summary>
        /// Считывание состояние порта
        /// </summary>      
        public ushort GET_DI(ushort NUM, byte PORT_NUM, ref byte PORT_VAL)
        {
            var result = (ushort)0xFFFF;
            if (_getDiFunc != null) result = _getDiFunc(NUM, PORT_NUM, ref PORT_VAL);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort GET_VER()
        {
            return _getVerFunc();
        }
    }
}
