using EquipApps.Mvc.Objects;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктура связующей сущноти
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Привязать модель.
        /// ВАЖНО!!
        /// ПРИВЯЗКА НЕ ДОЛЖНА ВЫДОВАТЬ ИСКЛЮЧЕНИЯ!!
        /// </summary>
        BindingResult Bind(TestObject framworkElement, int offset = 0);
    }
}
