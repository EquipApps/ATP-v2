using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ModelBinding;
using System;
using System.Linq;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    /// <summary>
    /// Привязывает модель используя <see cref="IModelProvider"/> как источник
    /// </summary>   
    public class ModelProviderModelBinder : IModelBinder
    {
        private ModelProviderFactoryDelagate _modelProviderFactory;

        /// <summary>
        /// Конструктор
        /// </summary>    
        public ModelProviderModelBinder(ModelProviderFactoryDelagate modelProviderFactory)
        {
            _modelProviderFactory = modelProviderFactory ?? throw new ArgumentNullException(nameof(modelProviderFactory));
        }


        public BindingResult Bind(ActionDescriptorObject framworkElement, int offset = 0)
        {
            try
            {
                var result = BindingResult.Success(
                     _modelProviderFactory()             //-- Создаем провайдер
                     .Provide()                          //-- Извлекаем данные
                     .Select(BindingResult.Success)      //-- Формеруем результат для каждого значения
                     .ToArray());                        //-- Обедняем результат.

                return result;                          //-- Сохраняем результат.


            }
            catch (Exception ex)
            {
                return BindingResult.Failed(ex);
            }
        }
    }
}
