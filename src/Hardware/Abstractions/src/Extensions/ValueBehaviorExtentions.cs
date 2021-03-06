﻿using EquipApps.Hardware.Abstractions;
using EquipApps.Testing;

namespace EquipApps.Hardware.Extensions
{
    public static class ValueBehaviorExtentions
    {
        public static void RequestToChangeValue<TBehavior, TValue>(this IEnableContext enableContext, TValue value, string hardwareName)
            where TBehavior : class, IValueBehavior<TValue>
        {
            //-- Извлекаем поведение
            var behavior = enableContext.ExtractBehavior<TBehavior>(hardwareName);

            //-- Изменяем состояние
            behavior.RequestToChangeValue(value);
        }
        public static void RequestToChangeValue<TBehavior, TValue>(this IEnableContext enableContext, TValue value, params string[] hardwareNames)
            where TBehavior : class, IValueBehavior<TValue>
        {
            //-- Извлекаем поведениея
            var behaviors = enableContext.ExtractBehavior<TBehavior>(hardwareNames);

            foreach (var behavior in behaviors)
            {
                //-- Изменяем состояние
                behavior.RequestToChangeValue(value);
            }
        }

        public static TValue RequestToUpdateValue<TBehavior, TValue>(this IEnableContext enableContext, string hardwareName)
            where TBehavior : class, IValueBehavior<TValue>
        {
            //-- Извлекаем поведение
            var behavior = enableContext.ExtractBehavior<TBehavior>(hardwareName);

            //-- Изменяем состояние
            behavior.RequestToUpdateValue();

            return behavior.Value;
        }
        public static TValue[] RequestToUpdateValue<TBehavior, TValue>(this IEnableContext enableContext, params string[] hardwareNames)
            where TBehavior : class, IValueBehavior<TValue>
        {
            //-- Извлекаем поведение
            var behaviors = enableContext.ExtractBehavior<TBehavior>(hardwareNames);
            var results = new TValue[hardwareNames.Length];


            for (int i = 0; i < behaviors.Length; i++)
            {
                behaviors[i].RequestToUpdateValue();
            }

            for (int i = 0; i < behaviors.Length; i++)
            {
                results[i] = behaviors[i].Value;
            }

            return results;
        }
    }
}
