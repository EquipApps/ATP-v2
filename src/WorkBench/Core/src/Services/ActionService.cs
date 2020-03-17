using DynamicData;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.Services
{
    /// <summary>
    /// Реализация <see cref="IActionService"/>.
    /// Также переопределяет реализацию <see cref="IActionDescripterFactory"/> по умолчаеию.
    /// </summary>
    public class ActionService : IActionService, IActionDescripterFactory
    {
        IActionDescriptorProvider[] _providers;
        private readonly SourceCache<ActionDescriptor, TestNumber> sourceCache;

        public ActionService(IEnumerable<IActionDescriptorProvider> actionDescriptorProviders)
        {
            if (actionDescriptorProviders == null)
                throw new ArgumentNullException(nameof(actionDescriptorProviders));

            _providers = actionDescriptorProviders.ToArray();

            if (_providers.Length == 0)
            {
                throw new ArgumentException(nameof(_providers));
            }


            sourceCache = new SourceCache<ActionDescriptor, TestNumber>(x => x.Number);
            Observable  = sourceCache.AsObservableCache();
        }

        public IObservableCache<ActionDescriptor, TestNumber> Observable { get; }

        public Task CleanAsync()
        {
            return Task.Run(() =>
            {
                sourceCache.Clear();
            });
        }

        public IReadOnlyList<ActionDescriptor> GetActionDescriptors()
        {
            var context = new ActionDescriptorContext();

            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].OnProvidersExecuting(context);
            }

            for (var i = _providers.Length - 1; i >= 0; i--)
            {
                _providers[i].OnProvidersExecuted(context);
            }

            //--- Добавляем в коллекцию!
            sourceCache.Edit(updater =>
            {
                updater.Clear();

                foreach (var item in context.Results)
                {
                    updater.AddOrUpdate(item);
                }
            });

            //-- Возвращаем коллекцию!
            return context.Results.ToArray();
        }

        public Task UpdateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
