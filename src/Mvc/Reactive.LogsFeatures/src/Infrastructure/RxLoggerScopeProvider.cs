using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure
{
    /// <summary>
    /// Scope provider.
    /// </summary>
    internal class RxLoggerScopeProvider : IExternalScopeProvider
    {
        private RxLoggerScopeProvider()
        {
        }

        private List<Scope> scopes = new List<Scope>();


        /// <summary>
        /// Returns a cached instance of <see cref="RxLoggerScopeProvider"/>.
        /// </summary>
        public static IExternalScopeProvider Instance { get; } = new RxLoggerScopeProvider();

        /// <inheritdoc />
        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
            foreach (var scope in scopes)
            {
                callback(scope.Obj, state);
            }
        }

        /// <inheritdoc />
        IDisposable IExternalScopeProvider.Push(object state)
        {
            return new Scope(state, this);
        }

        public class Scope : IDisposable
        {
            private RxLoggerScopeProvider provider;
            public object Obj;

            public Scope(object state, RxLoggerScopeProvider provider)
            {
                this.provider = provider;
                Obj = state;

                provider.scopes.Add(this);
            }

            public void Dispose()
            {
                provider.scopes.Remove(this);
            }
        }
    }
}
