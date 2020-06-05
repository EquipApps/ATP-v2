using EquipApps.Testing.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public class DigitFeatureProvider : IFeatureProvider
    {
        private ILogger<DigitFeatureProvider> logger;
        private HardwareOptions hardwareOptions;

        public int Order => int.MinValue;


        public DigitFeatureProvider(
            IOptions<HardwareOptions> hardwareOptions,
            ILoggerFactory loggerFactory)
        {
            this.hardwareOptions = hardwareOptions?.Value ?? throw new ArgumentNullException(nameof(hardwareOptions));

            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<DigitFeatureProvider>();
            logger.LogTrace("ctr");
        }

        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            logger.LogTrace(nameof(OnProvidersExecuted));

            /*
             * Идея!
             * После инициализации ядра (EquipApps.Hardware.Core)
             * Ко всем устройствам уже привязанны поведения.
             * Извлекаем устройства с нужным поведением и сохраняем (?)
             * Позволяет быстро опрашивать все цифровые выходы.
             */

            //-- Извлекаем IHardwareFeature
            var hardwareFeature = context.Collection.Get<IHardwareFeature>();
            if (hardwareFeature == null)
                throw new InvalidOperationException(
                    $"{nameof(IHardwareFeature)} - Не найден. " +
                    $"Возможно не зарегитрирован HardwareFeatureProvider. " +
                    $"Проверьте вызывыв AddHardware");


            var digitalFeature = new DigitFeature();

            //-- Конфигурация
            foreach (var virtualDefine in hardwareOptions.VirtualDefines.Where(x => x.BehaviorTypes.Contains(typeof(IDigitBehavior))))
            {
                var hardware = hardwareFeature.HardwareCollection[virtualDefine.Name];
                if (hardware == null)
                {
                    logger.LogError(
                        $"Виртуальное устройство {virtualDefine.Name} не найдено!");

                    continue;
                }

                var behavior = hardware.Behaviors.Get<IDigitBehavior>();
                if (behavior == null)
                {
                    logger.LogError(
                        $"Виртуальное устройство {virtualDefine.Name} не поддерживает поведение!");

                    continue;
                }

                if (!behavior.CanUpdateValue)
                {
                    logger.LogWarning(
                        $"Виртуальное устройство {virtualDefine.Name} не может обновлять значение");
                    continue;
                }


                digitalFeature[virtualDefine.Name] = behavior;
            }

            //-- Сохраняем
            context.Collection.Set(digitalFeature);

        }

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            logger.LogTrace(nameof(OnProvidersExecuted));
            //-- Ничего не делаем!
        }
    }
}
