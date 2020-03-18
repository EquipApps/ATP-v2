using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Runtime;
using System;
using System.Reflection;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State2_Invoke : IRuntimeState
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="context">
        /// </param>
        public void Run(RuntimeContext context)
        {
            //
            // Получаем текущий элемент actionDescriptor
            //
            var descriptor = context.Enumerator.Current;
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            //
            // Если флаг не установлен, то данная проверка будет пропущенна
            //
            if (!descriptor.IsCheck)
            {
                descriptor.Result = Result.NotExecuted;
                context.StateEnumerator.JumpTo(RuntimeStateType.MOVE);
                context.StateEnumerator.MoveNext();
                return;
            }

           

            //
            // Создаем обертку для логера!
            //
            using (context.TestContext.TestLogger.BeginScope(descriptor.Number))
            {
                NewMethod(context, descriptor);
            }

            
        }







        private static void NewMethod(RuntimeContext context, ActionDescriptor descriptor)
        {
            try
            {

                descriptor.State = State.Invoke;

                //
                // Создаем ActionContext для текущего ActionDescriptor
                //
                var actionContext = new ActionContext(context, descriptor);

                //
                // Создаем IActionInvoker для текущего ActionContext
                //
                var invoker = context.Factory.CreateInvoker(actionContext);
                if (invoker == null)
                {
                    throw new ArgumentNullException(nameof(invoker));
                }

                //
                // Вызываем функцию
                //
                invoker.Invoke();

                //
                // Изменяем состояние
                //
                //descriptor.Result = Result.Passed;

            }
            catch (Exception ex)
            {
                var exception = ex;
                //TODO: Написать юнит тесты для поиска состояния остановка!
                if (exception is TargetInvocationException)
                {
                    exception = ex.InnerException ?? ex;
                }
                if (exception is AggregateException)
                {
                    exception = ex.InnerException ?? ex;
                }

                if (exception is NotImplementedException)
                {
                    descriptor.Result = Result.NotImplemented;
                    descriptor.Exception = exception;
                }
                else if (exception is OperationCanceledException)
                {
                    descriptor.Result = Result.Inconclusive;
                    descriptor.Exception = exception;
                }
                else
                {
                    descriptor.Exception = exception;
                    descriptor.Result = Result.Failed;
                }
            }
            finally
            {
                descriptor.State = State.Empy;
                descriptor.Update();
                context.StateEnumerator.MoveNext();
            }
        }
    }
}
