using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Infrastucture;
using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Testing.Mvc.Runtime.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Internal
{
    /// <summary>
    /// Процес выполнения. 
    /// </summary>
    /// 
    /// <remarks>
    /// Извлекает и обрабатывает <see cref="ActionDescriptor"/>. 
    /// Include State Pattern
    /// </remarks>
    public class MvcMiddleware
    {
        private readonly MvcOption options;
        private readonly IEnumerable<IActionInvokerProvider> actionInvokerProviders;

        public MvcMiddleware(
            IOptions<MvcOption> options,
            IEnumerable<IActionInvokerProvider> actionInvokerProviders)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.actionInvokerProviders = actionInvokerProviders ?? throw new ArgumentNullException(nameof(actionInvokerProviders));


            States = RuntimeStateCollectionBuilder.Build(options.Value.RuntimeStates);
        }

        /// <summary>
        /// Возврвщвет <see cref="RuntimeStateCollection"/>
        /// </summary>
        public RuntimeStateCollection States { get; }






        public Task RunAsync(TestContext testContext)
        {
            return Task.Run(() => Run(testContext));
        }


        public void Run(TestContext testContext)
        {
            //-- 1) Извлечение
            var actionDescriptors = testContext.GetActionDescriptors();

            //-- 2) Обновляем состояние
            foreach (var actionDescriptor in actionDescriptors)
            {
                actionDescriptor.Result = Result.NotExecuted;
                actionDescriptor.Exception = null;
            }

            using (var actionEnumerator = new RuntimeActionEnumerator    (actionDescriptors))
            using (var actionFactory    = new RuntimeActionInvokerFactory(actionInvokerProviders))
            using (var stateEnumerator  = new RuntimeStateEnumerator     (States))
            {
                var runtimeContext = new RuntimeContext(
                        testContext,
                        actionEnumerator,
                        actionFactory,
                        stateEnumerator);

                //-- КОНВЕЕР RUNTIME STATE
                runtimeContext.State.Reset();
                runtimeContext.State.MoveNext();

                while (true)
                {
                    if (runtimeContext.Handled)
                        break;

                    if (runtimeContext.TestContext.TestAborted.IsCancellationRequested)
                        break;

                    runtimeContext.State.Current.Handle(runtimeContext);
                }
            }
        }
    }
}
