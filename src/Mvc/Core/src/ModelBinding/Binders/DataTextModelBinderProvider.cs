using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using NLib.AtpNetCore.Mvc.ModelBinding.Text;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    /// <summary>
    /// Провайдер привязки для источника <see cref="BindingSource.DataText"/>
    /// </summary>   
    public class DataTextModelBinderProvider : IBinderProvider
    {
        private static object ERROR(object obj)
        {
            return "<ERROR>";
        }

        /// <summary>
        /// Возвращает <see cref="IModelBinder"/> в случае прохождения валидации <paramref name="context"/>
        /// </summary>
        /// 
        /// <param name="context">
        /// Контекст привязки.
        /// Уловие прохождения валидации на обработку запроса:
        /// 1) Cвойство <see cref="BindingInfo.BinderModelName"/>     не равно null
        /// 2) Cвойство <see cref="BindingInfo.BindingSource"/> не равно null
        /// 3) Cвойство <see cref="BindingInfo.BindingSource"/> возвращает <see cref="BindingSource.DataText"/>
        /// </param>
        /// 
        /// <returns>
        /// Возвращает <see cref="DataTextModelBinder"/> в случае успешной привязки или NULL если привязка не удалать.
        /// </returns>
        /// 
        /// <remarks>
        /// Логика работы:
        /// 1) Разбор <see cref="BindingInfo.BinderModelName"/>
        ///     
        /// 2) Если значение свойства <see cref="BindingInfo.BinderModelName"/> равно нулю или пустой строке, то объектом привязки является <see cref="DataContext"/>.
        ///     Смотри функцию <see cref="GetBinderForEmptyName"/>
        /// 
        /// 3) Если значение свойства <see cref="BindingInfo.BinderModelName"/> НЕ равно нулю и пустой строке, то объект привязки извлекается из <see cref="DataContext"/>
        ///     Смотри функцию <see cref="GetBinderForProprName"/>
        /// </remarks>
        public IModelBinder GetBinder(BinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.BindingInfo.BinderModelName != null &&
                context.BindingInfo.BindingSource != null &&
                context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.DataText))
            {
                var text = context.BindingInfo.BinderModelName;     //-- Извлекаем текст.
                var textModel = TextModelParser.ParseText(text);   //-- Парсим.

                if (textModel.Inserts == null)                      //-- Нету вставок. выходим
                    return null;


                var tuples = new Tuple<int, PropertyExtractor>[textModel.Inserts.Length];

                for (int i = 0; i < textModel.Inserts.Length; i++)
                {
                    var propertyPath = textModel.Inserts[i];

                    var bindingModel = context.BindingModel;

                    for (int j = 0; bindingModel != null; bindingModel = bindingModel.Parent, j++)
                    {
                        var bindingInfo = bindingModel.BindingInfo;
                        if (bindingInfo == null)
                            continue;

                        var sourceType = bindingInfo.ModelType;
                        if (sourceType == null)
                            continue;

                        if (context.PropertyProvider.TryGetModelProperty(sourceType, propertyPath, out PropertyEntery propertyEntery))
                        {
                            tuples[i] = new Tuple<int, PropertyExtractor>(j, propertyEntery.Extractor);
                            break;
                        }

                        //-- 
                        if (tuples[i] == null)
                        {
                            tuples[i] = new Tuple<int, PropertyExtractor>(-1, ERROR);
                        }
                    }
                }

                return new DataTextModelBinder(textModel.Format, tuples);
            }

            return null;
        }
    }
}
