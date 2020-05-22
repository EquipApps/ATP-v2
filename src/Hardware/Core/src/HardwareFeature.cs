using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Hardware расширение
    /// </summary>
    public class HardwareFeature : IHardwareFeature, IDisposable
    {
        //---
        public HardwareFeature(IHardwareCollection hardwareCollection)
        {
            //---
            HardwareCollection = hardwareCollection
                ?? throw new ArgumentNullException(nameof(hardwareCollection));

            HardwareAdapters = new List<IHardwareAdapter>();
        }


        //---
        public List<IHardwareAdapter> HardwareAdapters { get; }

        //---
        public IHardwareCollection HardwareCollection { get; }

        /// <summary>
        /// Освобождение ресурсов..       
        /// </summary>  
        public void Dispose()
        {
            //-- Удаляем все адаптеры реальных устройств
            //-- (Каждый адаптер сам знает как очищать ресурсы устройства)
            //-- Отлавливаем ошибки и выыодим в лог файл
            foreach (var hardwareAdapter in HardwareAdapters)
            {
                try
                {
                    hardwareAdapter.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.Fail($"Ошибка освобождения адаптера {hardwareAdapter}", ex.ToString());
                }

            }

            //-- Удаляем все виртуальные устройства!
            HardwareCollection.Clear();
        }
    }
}
