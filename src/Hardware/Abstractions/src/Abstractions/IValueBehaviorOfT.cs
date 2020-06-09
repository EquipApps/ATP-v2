namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Поведения значения.
    /// </summary>    
    /// 
    /// <remarks>
    /// ОПИСАНИЕ:
    ///  
    ///  |A|                    |B|  <---- (Request)    |E| 1) Расширение отправляет запросы через поведение
    ///  |D| <---- (Event)      |E|                     |X| 2) Поведение отовещает адаптер через событие
    ///  |A|  ---> (Context)    |H|                     |T| 3) Адаптер обрабатывает событие 
    ///  |P|                    |A|  ----> (Value)      |E| и устанавливает результат обработки через контекст события
    ///  |T|                    |V|                     |N| 4) Поведение анализирует состояние контекста и изменяет состояние буферного значения Value
    ///  |E|                    |I|                     |T| 5) Расширение анализирует значение Value
    ///  |R|                    |O|                     |I|
    ///                         |R|                     |O|
    ///                                                 |N|
    /// 
    /// </remarks>
    /// 
    public interface IValueBehavior<TValue> : IValueBehavior
    {
        /// <summary>
        /// Value
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Request To Change Value
        /// </summary>  
        void RequestToChangeValue(TValue value);

        /// <summary>
        /// Request To Update Value.
        /// </summary>
        void RequestToUpdateValue();
    }
}