using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    /// <summary>
    /// Провайдер привязки для источника <see cref="BindingSource.DataContext"/>
    /// </summary>      
    public class DataContextModelBinderProvider : IBinderProvider
    {
        /// <summary>
        /// Возвращает <see cref="IModelBinder"/> в случае прохождения валидации <paramref name="context"/>
        /// </summary>        
        public IModelBinder GetBinder(BinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.BindingInfo.BindingSource != null &&
                context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.DataContext))
            {
                var expectedBindingType = context.BindingInfo.ModelType;
                var expectedBindingPath = context.BindingInfo.BinderModelName;
                var parent = context.BindingModel;
                var index = 0;

                /* Пропускаем 0 элемент.(Чтобы не извлекать данные из самого себя)*/
                index++;
                parent = parent.Parent;

                var isExpectedBindingPathNull = string.IsNullOrEmpty(expectedBindingPath);
                var isExpectedBindingTypeNull = expectedBindingType == null;

                /* Поиск первого*/
                if (isExpectedBindingPathNull & isExpectedBindingTypeNull)
                {
                    for (; parent != null; index++, parent = parent.Parent)
                    {
                        //-- Пропускаем нулевые значения
                        var parentBindingType = parent.BindingInfo?.ModelType;
                        if (parentBindingType == null)
                            continue;

                        return new DataContextModelBinder(index);
                    }
                }

                /* Поиск по типу*/
                else if (isExpectedBindingPathNull & !isExpectedBindingTypeNull)
                {
                    for (; parent != null; index++, parent = parent.Parent)
                    {
                        //-- Пропускаем нулевые значения
                        var parentBindingType = parent.BindingInfo?.ModelType;
                        if (parentBindingType == null)
                            continue;

                        //-- 
                        if (expectedBindingType == parentBindingType ||
                             expectedBindingType.IsAssignableFrom(parentBindingType))
                            return new DataContextModelBinder(index);
                    }
                }

                /*Поиск по пути (Не явная привязка)*/
                else if (!isExpectedBindingPathNull & isExpectedBindingTypeNull)
                {
                    for (; parent != null; index++, parent = parent.Parent)
                    {
                        //-- Пропускаем нулевые значения
                        var parentBindingType = parent.BindingInfo?.ModelType;
                        if (parentBindingType == null)
                            continue;

                        if (!context.PropertyProvider.TryGetModelProperty(parentBindingType, expectedBindingPath, out PropertyEntery property))
                            continue;

                        /* Т.К ТИП ТРЕБУЕМОЙ МОДЕЛИ НЕ ИЗВЕСТЕН (НЕ ЯВНАЯ ПРИВЯЗКА)
                         * то модель нужно обновить!
                         * НЕОБХОДИМО ДЛЯ ФОРМИРОВАНИЯ ИНФОРМАЦИИ О ПРИВЯЗКЕ!
                         */
                        context.BindingModel.BindingInfo.ModelType = property.Type;

                        return new DataContextModelBinderPath(property.Extractor, index);
                    }
                }

                /*Поиск по пути и типу*/
                else
                {
                    for (; parent != null; index++, parent = parent.Parent)
                    {
                        //-- Пропускаем нулевые значения
                        var parentBindingType = parent.BindingInfo?.ModelType;
                        if (parentBindingType == null)
                            continue;

                        if (!context.PropertyProvider.TryGetModelProperty(parentBindingType, expectedBindingPath, out PropertyEntery property))
                            continue;

                        //-- TODO: 
                        if (expectedBindingType == property.Type ||
                             expectedBindingType.IsAssignableFrom(property.Type))
                            return new DataContextModelBinderPath(property.Extractor, index);

                        var targetMeth = context.MetadataProvider.GetMetadataForType(property.Type);

                        //-- Тип совпал как Элемент коллекции!
                        if (targetMeth.IsCollectionType || targetMeth.IsEnumerableType)
                            if (expectedBindingType == targetMeth.ElementType || expectedBindingType.IsAssignableFrom(targetMeth.ElementType))
                                return new DataContextModelBinderCollection(property.Extractor, index);
                    }
                }
            }

            return null;
        }
    }
}
