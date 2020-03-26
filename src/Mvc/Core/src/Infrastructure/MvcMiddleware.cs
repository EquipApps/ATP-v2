using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Infrastructure
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
        private readonly DefaultRuntimeSynchService _synchService;
        private readonly IEnumerable<IActionInvokerProvider> _actionInvokerProviders;
        
        private ActionInvokerFactory actionFactory;
        private ActionDescriptorEnumerator actionEnumerator;

        public MvcMiddleware(DefaultRuntimeSynchService synchService,
                             IEnumerable<IActionInvokerProvider> actionInvokerProviders)
        {
            _synchService = synchService ?? throw new ArgumentNullException(nameof(synchService));
            _actionInvokerProviders = actionInvokerProviders ?? throw new ArgumentNullException(nameof(actionInvokerProviders));
        }

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
                actionDescriptor.Result    = Result.NotExecuted;
                actionDescriptor.Exception = null;
            }

            using (actionEnumerator = new ActionDescriptorEnumerator(actionDescriptors))
            using (actionFactory    = new ActionInvokerFactory(_actionInvokerProviders))
            {

                var runtimeContext = new DefaultRuntimeContext(testContext, actionEnumerator);

                testContext.TestAborted.Register(_synchService.Next);

                // `
                var next = RuntimeState.Reset;

                // `isCompleted
                var isCompleted = false;

                while (!isCompleted)
                {
                    if (runtimeContext.TestContext.TestAborted.IsCancellationRequested)
                        break;

                    Next(ref next, ref isCompleted, runtimeContext);
                }
            }
        }

        private void Next(ref RuntimeState next, ref bool isCompleted, RuntimeContext context)
        {
            switch (next)
            {
                case RuntimeState.Reset:
                    {
                        //
                        // Обновляем перечисление
                        //
                        actionEnumerator.Reset();

                        //
                        // Проверка наличия ПЕРВОГО элемента
                        //
                        if (actionEnumerator.MoveNext())
                        {
                            goto case RuntimeState.Invoke;
                        }
                        else
                        {
                            //
                            // Первый элемент Null, что делать ?
                            //
                            throw new InvalidOperationException("Проверка прервана.Коллекция пуста!");
                        }
                    }
                case RuntimeState.Invoke:
                    {
                        //
                        // Получаем текущий элемент actionDescriptor
                        //
                        var descriptor = actionEnumerator.Current;
                        if (descriptor == null)
                        {
                            throw new ArgumentNullException(nameof(descriptor));
                        }

                        //
                        // Если флаг не установлен, то данная проверка будет пропущенна
                        //
                        if (!descriptor.IsCheck)
                        {
                            goto case RuntimeState.Move;
                        }

                        //
                        // Создаем обертку для логера!
                        //
                        try
                        {

                            //descriptor.State = Abstractions.State.Invoke;

                            //
                            // Создаем ActionContext для текущего ActionDescriptor
                            //
                            var actionContext = new ActionContext(context, descriptor);

                            //
                            // Создаем IActionInvoker для текущего ActionContext
                            //
                            var invoker = actionFactory.CreateInvoker(actionContext);
                            if (invoker == null)
                            {
                                throw new ArgumentNullException(nameof(invoker));
                            }

                            //
                            // Вызываем функцию
                            //
                            invoker.Invoke();

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
                            next = RuntimeState.Pause;
                        }
                        break;
                    }
                case RuntimeState.Pause:
                    {
                        // Если пауза не включина идем далее..
                        if (!_synchService.IsEnabledPause)
                            goto case RuntimeState.RepeatOnce;

                        switch (_synchService.Pause())
                        {
                            case RuntimeCase.Next:
                                {
                                    next = RuntimeState.RepeatOnce;
                                    return;
                                }
                            case RuntimeCase.Replay:
                                {
                                    next = RuntimeState.Invoke;
                                    return;
                                }

                            case RuntimeCase.Previous:
                            default:
                                throw new NotImplementedException();
                        }
                    }
                case RuntimeState.RepeatOnce:
                    {
                        if (_synchService.IsEnabledRepeatOnce)
                        {
                            _synchService.RepeatOnce();
                            goto case RuntimeState.Invoke;
                        }
                        else
                            goto case RuntimeState.Move;
                    }
                case RuntimeState.Move:
                    {
                        //
                        // Переход к следующему шагу
                        //
                        if (actionEnumerator.MoveNext())
                        {
                            //
                            // Переходим в состояние      
                            //
                            goto case RuntimeState.Invoke;
                        }
                        else
                        {
                            //
                            // OБОШЛИ ВСЮ КОЛЛЕКЦИЮ!..
                            //
                            goto case RuntimeState.Repeat;
                        }
                    }
                case RuntimeState.Repeat:
                    {
                        if (_synchService.IsEnabledRepeat)
                        {
                            _synchService.Repeat();
                            goto case RuntimeState.Reset;
                        }
                        else
                            goto case RuntimeState.End;
                    }
                case RuntimeState.End:
                    {
                        isCompleted = true;
                        break;
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
