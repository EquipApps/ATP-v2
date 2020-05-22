using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PST_Series.PST_320
{
    /// <summary>   
    /// <para>Устройство PST-320.</para>
    /// <para>Трехканальный источник питания</para>
    /// </summary>
    public class PST320
    {
        /// <summary>
        /// Базовый путь к библеотеке
        /// </summary>
        public static string DLL_Path = "C:\\Windows\\AAPCtrlDev\\PST_320.dll";

        public PST320(ushort number, ushort comport)
        {

        }







        public static void GetError(ushort Number)
        {
            string Error_Message = null;
            switch (Number)
            {
                case 0:
                    return;
                case 1:
                    Error_Message = "Не найдено устройство ";
                    break;
                case 2:
                    Error_Message = "Порт не доступен";
                    break;
                case 3:
                    Error_Message = "Проблема с таймаутами";
                    break;
                case 4:
                    Error_Message = "Устройство уже было инициализировано";
                    break;
                case 5:
                    Error_Message = "Не установился максимальный ток";
                    break;
                case 6:
                    Error_Message = "Не установилось максимальное напряжение ";
                    break;
                case 7:
                    Error_Message = "Не установилось напряжение";
                    break;
                case 8:
                    Error_Message = "Напряжение превышает свое максимальное значение";
                    break;
                case 9:
                    Error_Message = "Не подалось питание";
                    break;
                case 10:
                    Error_Message = "Не прекратилась подача питания ";
                    break;
                case 11:
                    Error_Message = "Не произошла настройка по напряжению";
                    break;
                case 12:
                    Error_Message = "Не произошла настройка по току";
                    break;
                case 13:
                    Error_Message = "Удаляемое устройство не было найдено";
                    break;
                case 14:
                    Error_Message = "Подходящее устройство не обнаружено или уже было инициализировано ";
                    break;
                case 15:
                    Error_Message = "Идет подача питания";
                    break;
                default:
                    Error_Message = "Не известна ошибка";
                    break;
            }
        }
    }
}
