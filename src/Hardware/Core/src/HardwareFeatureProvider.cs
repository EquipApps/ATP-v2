using EquipApps.Testing.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// 
    /// </summary>
    public class HardwareFeatureProvider : IFeatureProvider
    {
        private readonly HardwareOptions _hardwareOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHardwareCollection _hardwares;

        public HardwareFeatureProvider(IOptions<HardwareOptions> hardwareOptions,
                                       IServiceProvider serviceProvider,
                                       IHardwareCollection hardwares)
        {
            _hardwareOptions = hardwareOptions?.Value ?? throw new ArgumentNullException(nameof(hardwareOptions));
            _serviceProvider = serviceProvider  ?? throw new ArgumentNullException(nameof(serviceProvider));
            _hardwares       = hardwares        ?? throw new ArgumentNullException(nameof(hardwares));
        }

        public int Order => 0;


        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            //-- Ничего не делаем!
        }

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //-- Создаем фичу
            var hardwareFeature = FactoryHardwareFeature();
            hardwareFeature.HardwareCollection.Clear();

            foreach (var deviceName in _hardwareOptions.GetVirtualDevice())
            {
                var hardware = new Hardware(deviceName);
                hardwareFeature.HardwareCollection.AddOrUpdate(hardware);
            }

            foreach (var adapterMap in _hardwareOptions.AdapterMappings)
            {
                var adapter = AdapterFactory(adapterMap.AdapterType);
                var device  = DeviceFactory (adapterMap.DeviceType);

                adapter.Initialize(hardwareFeature, device, adapterMap.Name);

                hardwareFeature.HardwareAdapters.Add(adapter);

            }

            //-- Регистрируем
            context.Collection.Set(hardwareFeature);
        }
















        private IHardwareFeature FactoryHardwareFeature()
        {
            return _serviceProvider.GetService<IHardwareFeature>();
        }

        private IHardwareAdapter AdapterFactory(Type adapterType)
        {
            if (adapterType == null)
            {
                throw new ArgumentNullException(nameof(adapterType));
            }

            var instance = _serviceProvider.GetService(adapterType);

            if (instance == null)
            {
                throw new InvalidOperationException("Не узается создать адаптер: " + adapterType.Name);
            }

            var adapter = instance as IHardwareAdapter;

            if (adapter == null)
            {
                throw new InvalidOperationException(
                    string.Format("Aдаптер: {0} - не реализует интерфейс", adapterType.Name, nameof(IHardwareAdapter)));
            }

            return adapter;
        }

        private object DeviceFactory(Type deviceType)
        {
            if (deviceType == null)
            {
                throw new ArgumentNullException(nameof(deviceType));
            }

            var instance = _serviceProvider.GetService(deviceType);

            if (instance == null)
            {
                throw new InvalidOperationException("Не удается создать устройство: " + deviceType.Name);
            }

            return instance;
        }
    }
}
