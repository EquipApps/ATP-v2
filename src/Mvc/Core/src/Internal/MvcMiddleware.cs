using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Testing;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Testing.Mvc.Internal;
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


            States = new MvcRuntimeStateCollection(options.Value.RuntimeStates);
        }

        /// <summary>
        /// Возврвщвет <see cref="MvcRuntimeStateCollection"/>
        /// </summary>
        public MvcRuntimeStateCollection States { get; }






        public Task RunAsync(TestContext testContext)
        {
            return Task.Run(() => Run(testContext));
        }

        public void Run(TestContext testContext)
        {
            if (testContext == null)
            {
                throw new ArgumentNullException(nameof(testContext));
            }

            using (var context = GetRuntimeContext(testContext))
            {
                //-- КОНВЕЕР RUNTIME STATE
                context.StateEnumerator.Reset();
                context.StateEnumerator.MoveNext();

                while (true)
                {
                    if (context.Handled)
                        break;

                    if (context.TestContext.TestAborted.IsCancellationRequested)
                        break;

                    context.StateEnumerator.Current.Run(context);
                }
            }
        }







        private MvcRuntimeContext GetRuntimeContext(TestContext testContext)
        {
            //-- 1) Извлечение
            var actionDescriptors = testContext.GetActionDescriptors();

            //-- 2) Обновляем состояние
            foreach (var actionDescriptor in actionDescriptors)
            {
                actionDescriptor.Result    = Result.NotExecuted;
                actionDescriptor.Exception = null;
            }

            //-- 3) Создаем перечисление
            var enumerator = new RuntimeActionDescriptorEnumerator(actionDescriptors);

            var factory = new RuntimeActionInvokerFactory(actionInvokerProviders);

            //-- 4) Снятие блокировки потока


            return new MvcRuntimeContext(
                testContext,
                States,
                enumerator,
                factory);
        }





    }
}
