using Microsoft.Extensions.Internal;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Controllers
{
    internal class ControllerActionInvokerCacheEntry
    {
        public ControllerActionInvokerCacheEntry(
            ObjectMethodExecutor objectMethodExecutor,
            ActionMethodExecutor actionMethodExecutor)
        {
            ObjectMethodExecutor = objectMethodExecutor;

            ActionMethodExecutor = actionMethodExecutor;
        }


        internal ObjectMethodExecutor ObjectMethodExecutor { get; }

        internal ActionMethodExecutor ActionMethodExecutor { get; }
    }

    internal abstract class ActionMethodExecutor
    {
        private static readonly ActionMethodExecutor[] Executors = new ActionMethodExecutor[]
        {
            //TODO: Добавить Executors (когда функции возвращаеют Object или другие типы )

            //-- Синхронные методы!
            new VoidExecutor(),
            new ResultExecutor(),    
            
            //-- Асинхронные методы!  
            new TaskExecutor(),
            new TaskResultExecutor()
        };


        public static ActionMethodExecutor GetExecutor(ObjectMethodExecutor executor)
        {
            for (var i = 0; i < Executors.Length; i++)
            {
                if (Executors[i].CanExecute(executor))
                {
                    return Executors[i];
                }
            }
            throw new Exception("Не удается создать ActionMethodExecutor");
        }


        public abstract IActionResult Execute(ObjectMethodExecutor executor, object controller, object[] arguments);

        protected abstract bool CanExecute(ObjectMethodExecutor executor);



        // void Foo(..)
        private class VoidExecutor : ActionMethodExecutor
        {
            public override IActionResult Execute(ObjectMethodExecutor executor, object controller, object[] arguments)
            {
                executor.Execute(controller, arguments);
                return NonActionResult.Instance;
            }

            protected override bool CanExecute(ObjectMethodExecutor executor)
                => !executor.IsMethodAsync && executor.MethodReturnType == typeof(void);
        }

        // IActionResult Foo(..)
        private class ResultExecutor : ActionMethodExecutor
        {
            public override IActionResult Execute(ObjectMethodExecutor executor, object controller, object[] arguments)
            {
                var actionResult = (IActionResult)executor.Execute(controller, arguments);
                EnsureActionResultNotNull(executor, actionResult);
                return actionResult;
            }

            protected override bool CanExecute(ObjectMethodExecutor executor)
                => !executor.IsMethodAsync && typeof(IActionResult).GetTypeInfo().IsAssignableFrom(executor.MethodReturnType.GetTypeInfo());
        }

        // Task Foo(..)
        private class TaskExecutor : ActionMethodExecutor
        {
            public override IActionResult Execute(ObjectMethodExecutor executor, object controller, object[] arguments)
            {
                var actionTask = (Task)executor.Execute(controller, arguments);
                EnsureActionTaskNotNull(executor, actionTask);
                actionTask.Wait();
                return NonActionResult.Instance;
            }

            protected override bool CanExecute(ObjectMethodExecutor executor) => executor.MethodReturnType == typeof(Task);
        }

        // Task<IActionResult> Foo(..)
        private class TaskResultExecutor : ActionMethodExecutor
        {
            public override IActionResult Execute(ObjectMethodExecutor executor, object controller, object[] arguments)
            {
                var actionTask = (Task<IActionResult>)executor.Execute(controller, arguments);
                EnsureActionTaskNotNull(executor, actionTask);
                actionTask.Wait();
                var actionResult = actionTask.Result;
                EnsureActionResultNotNull(executor, actionResult);
                return actionResult;
            }

            protected override bool CanExecute(ObjectMethodExecutor executor)
                => typeof(Task<IActionResult>).GetTypeInfo().IsAssignableFrom(executor.MethodReturnType.GetTypeInfo());
        }


        private static void EnsureActionResultNotNull(ObjectMethodExecutor executor, IActionResult actionResult)
        {
            if (actionResult == null)
            {
                throw new InvalidOperationException(
                    "ActionResult_ActionReturnValueCannotBeNull");
            }
        }

        private static void EnsureActionTaskNotNull(ObjectMethodExecutor executor, Task actionTask)
        {
            if (actionTask == null)
            {
                throw new InvalidOperationException(
                    "ActionResult_ActionReturnTaskCannotBeNull");
            }

            if (actionTask.Status == TaskStatus.Created)
            {
                throw new InvalidOperationException(
                   "ActionResult_ActionReturnTaskCannotNotScheduled");
            }
        }

    }
}
