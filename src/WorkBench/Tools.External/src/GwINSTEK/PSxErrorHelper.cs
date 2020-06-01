namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{
    /// <summary>
    /// Позвволяет расшифровать код ошибки
    /// </summary>
    public static class PSxErrorHelper
    {
        public static string GetErrorMessage(ushort errorCode)
        {
            string message = null;

            switch (errorCode)
            {
                case 0:
                    break;
                case 1:
                    message = "Не найдено устройство ";
                    break;
                case 2:
                    message = "Порт не доступен";
                    break;
                case 3:
                    message = "Проблема с таймаутами";
                    break;
                case 4:
                    message = "Устройство уже было инициализировано";
                    break;
                case 5:
                    message = "Не установился максимальный ток";
                    break;
                case 6:
                    message = "Не установилось максимальное напряжение ";
                    break;
                case 7:
                    message = "Не установилось напряжение";
                    break;
                case 8:
                    message = "Напряжение превышает свое максимальное значение";
                    break;
                case 9:
                    message = "Не подалось питание";
                    break;
                case 10:
                    message = "Не прекратилась подача питания ";
                    break;
                case 11:
                    message = "Не произошла настройка по напряжению";
                    break;
                case 12:
                    message = "Не произошла настройка по току";
                    break;
                case 13:
                    message = "Удаляемое устройство не было найдено";
                    break;
                case 14:
                    message = "Подходящее устройство не обнаружено или уже было инициализировано ";
                    break;
                case 15:
                    message = "Идет подача питания";
                    break;
                default:
                    message = "Не известна ошибка";
                    break;
            }

            return message;
        }
    }
}
