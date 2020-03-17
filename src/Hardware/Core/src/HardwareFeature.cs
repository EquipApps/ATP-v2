using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Hardware расширение
    /// </summary>
    public class HardwareFeature : IHardwareFeature, IDisposable
    {
        private readonly ILogger<HardwareFeature> logger;

        //---
        public HardwareFeature(
            ILogger<HardwareFeature> logger,
            IHardwareCollection hardwareCollection)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
            logger.LogTrace("Dispose");

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
                    logger.LogError(ex, $"Ошибка освобождения адаптера {hardwareAdapter}");
                }

            }

            //-- Удаляем все виртуальные устройства!
            HardwareCollection.Clear();
        }
    }
}
