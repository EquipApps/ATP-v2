using System.Collections.Generic;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Расширение..
    /// Доступно через <see cref="Testing.Features.IFeatureCollection"/>.
    /// Позволяет получить доступ к <see cref="IHardwareAdapter"/>
    /// Позволяет получить доступ к <see cref="IHardware"/>
    /// </summary>
    public interface IHardwareFeature
    {
        /// <summary>
        /// Возвращает список <see cref="IHardwareAdapter"/>
        /// </summary>
        List<IHardwareAdapter> HardwareAdapters { get; }

        /// <summary>
        /// Возвращает коллекцию <see cref="IHardware"/>
        /// </summary>
        IHardwareCollection HardwareCollection { get; }
    }
}