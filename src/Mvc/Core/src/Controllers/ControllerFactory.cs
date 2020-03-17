using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerFactory : IControllerFactory
    {
        private readonly Func<Type, ObjectFactory> _createFactory = (type) => ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);
        private readonly ConcurrentDictionary<Type, ObjectFactory> _typeActivatorCache = new ConcurrentDictionary<Type, ObjectFactory>();
        private readonly IServiceProvider _serviceProvider;

        public ControllerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object CreateController(Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            var createFactory = _typeActivatorCache.GetOrAdd(controllerType, _createFactory);

            return createFactory(_serviceProvider, arguments: null);
        }

        public virtual void ReleaseController(object controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
