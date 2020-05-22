using EquipApps.Mvc;
using EquipApps.WorkBench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MPI.Controllers
{
    [OrderController("2")]
    public class Etap2Controller : Controller
    {
        public void Action()
        {
            //--
        }
    }


    public class KadrManager
    {
    }
    
    public static class CRC
    {
        /// <summary>
        /// Расчитывает CRC8 для блока данных
        /// </summary>
        /// <param name="crc">Начальное значение</param>
        /// <param name="poly">Полином</param>
        /// <param name="block">Блок данных</param>
        /// <returns></returns>
        public static byte Crc8(byte crc, byte poly, IEnumerable<byte> block)
        {
            foreach (var item in block)
            {
                crc ^= crc;

                for (int i = 0; i < 8; i++)
                {
                    crc = ((crc & 0x80) != 0)
                        ? (byte)((crc << 1) ^ poly)
                        : (byte)(crc << 1);
                }
            }

            return crc;
        }
    }
}
