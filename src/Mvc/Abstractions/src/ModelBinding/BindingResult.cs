using System;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Результат привязки
    /// </summary>
    public struct BindingResult
    {
        /// <summary>
        /// Результат привязки. Успешно!
        /// </summary> 
        public static BindingResult Success(object model)
        {
            return new BindingResult(model, isModelSet: true);
        }

        /// <summary>
        /// Результат привязки. Ошибка!
        /// </summary>       
        public static BindingResult Failed(Exception exception)
        {
            return new BindingResult(model: null, isModelSet: false, exception: exception);
        }

        /// <summary>
        /// Результат привязки (обединенный).
        /// Используется для привязки модкель провайдера
        /// </summary> 
        public static BindingResult Success(BindingResult[] results)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            return new BindingResult(results, true, true);
        }



        /// <summary>
        /// Конструктор
        /// </summary>     
        private BindingResult(object model, bool isModelSet, bool isMany = false, Exception exception = null)
        {
            Model = model;
            IsModelSet = isModelSet;
            IsModelNull = model == null;
            IsMany = isMany;
            Exception = exception;
        }

        /// <summary>
        /// Возвращает исключение
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Возвращает модель привязки
        /// </summary>
        public object Model { get; }

        /// <summary>
        /// Флаг, модель установленна!
        /// </summary>
        public bool IsModelSet { get; }

        /// <summary>
        /// Флаг, модель NULL!
        /// </summary>
        public bool IsModelNull { get; }

        /// <summary>
        /// Флаг. Множественная привязка
        /// </summary>
        public bool IsMany { get; }

        /// <summary>
        /// Возвращает коллекцию результатов
        /// </summary>       
        public BindingResult[] GetResults()
        {
            return Model as BindingResult[];
        }
    }
}
