using EquipApps.Testing;
using System;

namespace EquipApps.Hardware.Extensions
{
    public static class SerchBehaviorExtension
    {
        public static TBehavior[]   ExtractBehavior<TBehavior>(this IEnableContext enableContext, string[] hardwareNames)
           where TBehavior : class, IHardwareBehavior

        {
            var feature = enableContext
                .TestContext
                .TestFeatures.Get<IHardwareFeature>();

            var behaviors = new TBehavior[hardwareNames.Length];

            for (int i = 0; i < hardwareNames.Length; i++)
            {
                var hardwareName = hardwareNames[i];

                //-- Извлекаем устройство
                var hardware = feature.HardwareCollection[hardwareName];

                //-- Поверка на наличие устройства
                if (hardware == null)
                    throw new InvalidOperationException($"Виртуальное устройстов: {hardwareName} - не зарегистрированно.");

                //-- Извлекаем поведение
                var behavior = hardware.Behaviors.Get<TBehavior>();

                //-- Поверка на наличие поведениея
                if (behavior == null)
                    throw new InvalidOperationException($"Виртуальное устройстов: <{hardwareName}> - не поддерживает поведение <{typeof(TBehavior).Name}>.");

                behaviors[i] = behavior;
            }

            return behaviors;
        }

        public static TBehavior     ExtractBehavior<TBehavior>(this IEnableContext enableContext, string hardwareName)
            where TBehavior : class, IHardwareBehavior
        {
            //-- Извлекаем устройство
            var hardware = enableContext
                .TestContext
                .TestFeatures.Get<IHardwareFeature>()
                .HardwareCollection[hardwareName];

            //-- Поверка на наличие устройства
            if (hardware == null)
                throw new InvalidOperationException($"Виртуальное устройстов: {hardwareName} - не зарегистрированно.");

            //-- Извлекаем поведение
            var behavior = hardware.Behaviors.Get<TBehavior>();

            //-- Поверка на наличие поведениея
            if (behavior == null)
                throw new InvalidOperationException($"Виртуальное устройстов: <{hardwareName}> - не поддерживает поведение <{typeof(TBehavior).Name}>.");

            return behavior;
        }
    }
}
