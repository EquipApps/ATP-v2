using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Scope provider.
    /// </summary>
    internal class ViewLoggerScopeProvider : IExternalScopeProvider
    {
        private ViewLoggerScopeProvider()
        {
        }

        private List<Scope> scopes = new List<Scope>();


        /// <summary>
        /// Returns a cached instance of <see cref="ViewLoggerScopeProvider"/>.
        /// </summary>
        public static IExternalScopeProvider Instance { get; } = new ViewLoggerScopeProvider();

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
            private ViewLoggerScopeProvider provider;
            public object Obj;

            public Scope(object state, ViewLoggerScopeProvider provider)
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
