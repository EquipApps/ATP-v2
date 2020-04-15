using EquipApps.Mvc.Abstractions;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// Главный промежуточный слой.
    /// Отвечает за извлечение и обработку <see cref="ActionDescriptor"/>.
    /// </summary>
    /// 
    public class RuntimeMiddleware
    {
        private readonly DefaultRuntimeSynchService _synchService;
        private readonly IEnumerable<IActionInvokerProvider> _actionInvokerProviders;

        private ActionInvokerFactory actionFactory;
        private ActionDescriptorEnumerator actionEnumerator;

        public RuntimeMiddleware(
            DefaultRuntimeSynchService synchService,
            IEnumerable<IActionInvokerProvider> actionInvokerProviders)
        {
            _synchService = synchService ?? throw new ArgumentNullException(nameof(synchService));
            _actionInvokerProviders = actionInvokerProviders ?? throw new ArgumentNullException(nameof(actionInvokerProviders));
        }

        public Task RunAsync(TestContext testContext)
        {
            return Task.Run(() => Run(testContext));
        }

        private void Run(TestContext testContext)
        {
            //-- 1) Извлечение дескриптеров действий
            var actionDescriptors = testContext.GetActionDescriptors();

            //-- 2) Создаем временные ресурсы (будут уничтожены после окончения проверки)
            using (actionEnumerator = new ActionDescriptorEnumerator(actionDescriptors))
            using (actionFactory = new ActionInvokerFactory(_actionInvokerProviders))
            {
                //-- Создаем контекст. (контекст не должен уничтожать ресурсы, он только передает к ним доступ)
                var runtimeContext = new DefaultRuntimeContext(testContext, actionEnumerator);

                /*
                 * Регистрация действия Next по умолчанию при запросе на прерывание проверки..
                 * Позволяет прерывать проверку погда конвеер состояний находиться в состоянии паузы.
                 */
                testContext.TestAborted.Register(_synchService.Next);

                // `next
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
                            goto case RuntimeState.Begin;
                        }
                        else
                        {
                            //
                            // Первый элемент Null, что делать ?
                            //
                            throw new InvalidOperationException("Проверка прервана.Коллекция пуста!");
                        }
                    }
                case RuntimeState.Begin:
                    {
                        //
                        // Получаем текущий элемент actionDescriptor
                        //
                        var descriptor = actionEnumerator.Current;
                        Debug.Assert(descriptor != null, "Descriptor are equal NULL");

                        //
                        // Если флаг не установлен, то данная проверка будет пропущенна
                        //
                        if (!descriptor.IsCheck)
                        {
                            goto case RuntimeState.Move;                          
                        }

                        //
                        // Если флаг не установлен, то переходим к выполнению
                        //
                        if (!descriptor.IsBreak)
                        {
                            goto case RuntimeState.Invoke;
                        }

                        /*
                         * ВАЖНО.
                         * ПОСЛЕ ОБРБОТКИ ПАУЗЫ НУЖНО ВЫЙТИ ИЗ ФУНКЦИИ.
                         * ЧТОБЫ БЫЛА ВОЗМОЖНОСТЬ ПРЕРВАТЬ ПРОВЕРКУ!
                         */
                        
                        descriptor.SetState(State.BreakPoint);  //-- Изменяем состояние ОСТАНОВКА
                        _synchService.Pause();
                        descriptor.SetState(State.Empy);        //-- Изменяем состояние

                        next = RuntimeState.Invoke;             //-- Следующее состояние по умолчанию
                        break;
                    }
                case RuntimeState.Invoke:
                    {
                        //
                        // Получаем текущий элемент actionDescriptor
                        //
                        var descriptor = actionEnumerator.Current;
                        Debug.Assert(descriptor != null, "Descriptor are equal NULL");

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

                        try
                        {
                            //
                            // Вызываем функцию
                            //
                            invoker.Invoke();

                        }
                        catch (Exception ex)
                        {
                            //TODO: Написать юнит тесты. Не должно быть исключений!

                            Debug.Fail("Не обработанное исключение");

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

                        //
                        // Получаем текущий элемент actionDescriptor
                        //
                        var descriptor = actionEnumerator.Current;
                        if (descriptor == null)
                        {
                            throw new ArgumentNullException(nameof(descriptor));
                        }

                        //-- Изменяем состояние ПАУЗА
                        descriptor.SetState(State.Pause);

                        switch (_synchService.Pause())
                        {
                            case RuntimeCase.Next:
                                {
                                    next = RuntimeState.RepeatOnce;
                                    break;
                                }
                            case RuntimeCase.Replay:
                                {
                                    next = RuntimeState.Begin;
                                    break;
                                }
                            case RuntimeCase.Previous: //TODO: Когданибудь реализовать
                            default:
                                throw new NotImplementedException();
                        }

                        //-- Изменяем состояние
                        descriptor.SetState(State.Empy);

                        return;
                    }
                case RuntimeState.RepeatOnce:
                    {
                        if (_synchService.IsEnabledRepeatOnce)
                        {
                            _synchService.RepeatOnce();
                            goto case RuntimeState.Begin;
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
                            goto case RuntimeState.Begin;
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
