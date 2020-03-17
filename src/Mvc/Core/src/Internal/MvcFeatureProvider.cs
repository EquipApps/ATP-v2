using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Testing;
using EquipApps.Testing.Features;
using System;

namespace NLib.AtpNetCore.Testing.Mvc.Internal
{
    /// <summary>
    /// Реализация  <see cref="IMvcFeature"/> провайдера по умолчанию .
    /// Добавляет <see cref="IMvcFeature"/> в  <see cref="IFeatureCollection"/>.
    /// Заполняет <see cref="IMvcFeature"/> из <see cref="IActionDescripterFactory"/>.
    /// Функции вызыветя при создании тестовой проверки в  <see cref="ITestFactory"/>.
    /// </summary>
    public class MvcFeatureProvider : IFeatureProvider
    {
        private readonly IActionDescripterFactory _actionDescripterService;

        public MvcFeatureProvider(IActionDescripterFactory actionDescripterService)
        {
            _actionDescripterService = actionDescripterService ?? throw new ArgumentNullException(nameof(actionDescripterService));
        }

        public int Order => 0;

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //-- Извлекаем из колллекции
            var actionDescriptorFeature = context.Collection.Get<IMvcFeature>();
            if (actionDescriptorFeature == null)
            {
                //-- Если null (первый запуск).. добавляем!
                actionDescriptorFeature = new MvcFeature();
                actionDescriptorFeature.ActionDescriptors = _actionDescripterService.GetActionDescriptors();
                context.Collection.Set(actionDescriptorFeature);
            }
            else
            {
                //-- Обновляем коллекцию!
                actionDescriptorFeature.ActionDescriptors = null;
                actionDescriptorFeature.ActionDescriptors = _actionDescripterService.GetActionDescriptors();
            }
        }

        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            //-- Ничегоне делаем
        }
    }
}
