using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure;
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
    public class RuntimeMiddleware : IRuntimeService
    {
        private readonly IEnumerable<IActionInvokerProvider> _actionInvokerProviders;

        private readonly RuntimeRepeat repeat;
        private readonly RuntimeRepeat repeatOnce;
        private readonly RuntimeLocker locker;

        private volatile bool _isEnabledPause = false;

        private ActionInvokerFactory   actionFactory;
        private ActionObjectEnumerator actionEnumerator;

        IObservable<bool> IRuntimeService.ObservablePause => locker.ObservableLocker;

        public RuntimeMiddleware(IEnumerable<IActionInvokerProvider> actionInvokerProviders)
        {
            _actionInvokerProviders = actionInvokerProviders ?? throw new ArgumentNullException(nameof(actionInvokerProviders));

            repeat       = new RuntimeRepeat();
            repeatOnce   = new RuntimeRepeat();
            locker       = new RuntimeLocker();
        }

        public Task RunAsync(TestContext testContext)
        {
            return Task.Run(() => Run(testContext));
        }

        private void Run(TestContext testContext)
        {
            //-- 1) Извлечение дескриптеров действий
            var actionObjects = testContext.GetActionObjects();

            //-- 2) Создаем временные ресурсы (будут уничтожены после окончения проверки)
            using (actionEnumerator = new ActionObjectEnumerator(actionObjects))
            using (actionFactory = new ActionInvokerFactory(_actionInvokerProviders))
            {
                //-- Создаем контекст. (контекст не должен уничтожать ресурсы, он только передает к ним доступ)
                var runtimeContext = new DefaultRuntimeContext(testContext, actionEnumerator);

                /*
                 * Регистрация действия Next по умолчанию при запросе на прерывание проверки..
                 * Позволяет прерывать проверку погда конвеер состояний находиться в состоянии паузы.
                 */
                testContext.TestAborted.Register(locker.Next);

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
                        // Получаем текущий элемент ActionObjecct
                        //
                        var action = actionEnumerator.Current;
                        Debug.Assert(action != null, "ActionObjecct are equal NULL");

                        //
                        // Если флаг не установлен, то данная проверка будет пропущенна
                        //
                        if (!action.IsCheck)
                        {
                            goto case RuntimeState.Move;                          
                        }

                        //
                        // Если флаг не установлен, то переходим к выполнению
                        //
                        if (!action.IsBreak)
                        {
                            goto case RuntimeState.Invoke;
                        }

                        /*
                         * ВАЖНО.
                         * ПОСЛЕ ОБРБОТКИ ПАУЗЫ НУЖНО ВЫЙТИ ИЗ ФУНКЦИИ.
                         * ЧТОБЫ БЫЛА ВОЗМОЖНОСТЬ ПРЕРВАТЬ ПРОВЕРКУ!
                         */
                        
                        action.SetState(ActionObjectState.BreakPoint);  //-- Изменяем состояние ОСТАНОВКА
                        locker.CaseAwite();
                        action.SetState(ActionObjectState.Empy);        //-- Изменяем состояние

                        next = RuntimeState.Invoke;             //-- Следующее состояние по умолчанию
                        break;
                    }
                case RuntimeState.Invoke:
                    {
                        //
                        // Получаем текущий элемент ActionObjecct
                        //
                        var action = actionEnumerator.Current;
                        Debug.Assert(action != null, "ActionObjecct are equal NULL");

                        //
                        // Создаем ActionContext для текущего ActionObjecct
                        //
                        var actionContext = new ActionContext(context, action);

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
                        //-- Если пауза не включина идем далее..
                        if (!_isEnabledPause)
                            goto case RuntimeState.RepeatOnce;

                        //-- Получаем текущий элемент ActionObjecct                        
                        var action = actionEnumerator.Current;
                        Debug.Assert(action != null, "ActionObjecct are equal NULL");
                       
                        //-- Изменяем состояние ПАУЗА
                        action.SetState(ActionObjectState.Pause);

                        //-- 
                        switch (locker.CaseAwite())
                        {
                            case RuntimeLockerCase.Next:
                                {
                                    next = RuntimeState.RepeatOnce;
                                    break;
                                }
                            case RuntimeLockerCase.Replay:
                                {
                                    next = RuntimeState.Begin;
                                    break;
                                }
                            case RuntimeLockerCase.Previous: //TODO: Когданибудь реализовать
                            default:
                                throw new NotImplementedException();
                        }

                        //-- Изменяем состояние
                        action.SetState(ActionObjectState.Empy);

                        return;
                    }
                case RuntimeState.RepeatOnce:
                    {
                        if (repeatOnce.TryRepeat())
                        {
                            Sleep();

                            next = RuntimeState.Begin;
                        }
                        else
                            next = RuntimeState.Move;

                        break;
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
                        if (repeat.TryRepeat())
                        {
                            Sleep();

                            next = RuntimeState.Reset;
                        }
                        else
                            next = RuntimeState.End;

                        break;
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

        private void Sleep()
        {
            System.Threading.Thread.Sleep(100);
        }

        void IRuntimeService.EnabledRepeat(bool isRepeatEnabled)
        {
            repeat.Enabled(isRepeatEnabled);
        }

        void IRuntimeService.EnabledRepeatOnce(bool isRepeatOnceEnabled)
        {
            repeatOnce.Enabled(isRepeatOnceEnabled);
        }

        void IRuntimeService.EnabledPause(bool isPauseEnabled)
        {
            _isEnabledPause = isPauseEnabled;
        }

        void IRuntimeService.Next()
        {
            locker.Next();
        }

        void IRuntimeService.Replay()
        {
            locker.Replay();
        }

        void IRuntimeService.Previous()
        {
            locker.Previous();
        }
    }
}
