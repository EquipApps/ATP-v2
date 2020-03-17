using System;
using System.Collections.Generic;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Конфигурация виртуальныйх устройств
    /// </summary>
    public class HardwareOptions
    {
        private List<HardwareAdapterMap>    _adapterMaps       = new List<HardwareAdapterMap>();
        private List<HardwareVirtualDefine> _virtualDefines    = new List<HardwareVirtualDefine>();

        /// <summary>
        /// Возвращает список
        /// </summary>
        public IReadOnlyList<HardwareAdapterMap>    AdapterMappings => _adapterMaps;

        /// <summary>
        /// Возвращает список
        /// </summary>
        public IReadOnlyList<HardwareVirtualDefine> VirtualDefines => _virtualDefines;

        /// <summary>
        /// Регистрация виртуального устройства.
        /// Уже зарегистрированные устройства игнорируются.
        /// </summary> 
        public void RegisterHardware(string hardwareName, params Type[] behaviorTypes)
        {
            if (_virtualDefines.Exists(x => x.Name == hardwareName))
            {
                throw new InvalidOperationException($"Устройствро с именем {hardwareName} уже зарегистрированно");
            }

            _virtualDefines.Add(new HardwareVirtualDefine()
            {
                Name          = hardwareName,
                BehaviorTypes = behaviorTypes,
            });
        }
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapterType"></param>
        /// <param name="devicetype"></param>
        /// <param name="mapName"></param>
        public void RegisterMapping(Type adapterType, Type devicetype, string mapName)
        {
            if (adapterType == null)
            {
                throw new ArgumentNullException(nameof(adapterType));
            }

            if (devicetype == null)
            {
                throw new ArgumentNullException(nameof(devicetype));
            }

            if (mapName == null)
            {
                throw new ArgumentNullException(nameof(mapName));
            }

            if (_adapterMaps.Exists(x => x.Name == mapName))
            {
                throw new InvalidOperationException($"Преобразование с именем {mapName} уже зарегистрированно");
            }

            _adapterMaps.Add(new HardwareAdapterMap()
            {
                AdapterType = adapterType,
                DeviceType  = devicetype,
                Name        = mapName,
            });
        }


        /// <summary>
        /// Возвращает свойства. Используются для кастомизациии опций
        /// </summary>
        public Dictionary<object, object> Properties { get; } = new Dictionary<object, object>();
    }
}
