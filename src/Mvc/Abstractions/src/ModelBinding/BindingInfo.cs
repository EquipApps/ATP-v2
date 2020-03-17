using System;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Информация о привязке.
    /// </summary>
    public class BindingInfo
    {
        /// <summary>
        /// Задает или возвращает источник привязки.
        /// </summary>
        public BindingSource BindingSource { get; set; }

        /// <summary>
        /// Задает или возвращает тип  <see cref="IBinder"/>.
        /// Используется когда свойство <see cref="BindingSource"/> возвращает значение <see cref="BindingSource.Custom"/>
        /// </summary>
        public Type BinderType { get; set; }

        /// <summary>
        /// Задает или возвращает путь. 
        /// Используется для поиска модели.
        /// </summary>       
        public string ModelPath { get; set; }

        /// <summary>
        /// Задает или возвращает тип модели
        /// </summary>
        public Type ModelType { get; set; }
    }
}
