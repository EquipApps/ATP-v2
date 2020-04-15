using EquipApps.Mvc.Abstractions;
using EquipApps.Testing;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System;

namespace EquipApps.Mvc
{
    [Controller]
    public abstract class ControllerBase
    {
        private ControllerContext _controllerContext;

        /// <summary>
        /// Возвращает <see cref="Mvc.ControllerContext"/>
        /// </summary>         
        public ControllerContext ControllerContext
        {
            get
            {
                return _controllerContext;
            }
            internal set
            {
                _controllerContext = value ?? throw new ArgumentNullException(nameof(ControllerContext));
            }
        }

        /// <summary>
        /// Возвращает <see cref="Testing.TestContext"/>
        /// </summary>
        public TestContext TestContext => ControllerContext.TestContext;

        /// <summary>
        /// Возвращает <see cref="IFullLogger"/>
        /// </summary>
        public ILogger Logger => TestContext?.TestLogger;

        #region RedirectTo

        [NonAction]
        public IActionResult JumpTo(string actionName)
        {
            return new ControllerJumpToResult(actionName);
        }

        #endregion

        #region Null Action
        public IActionResult Null => NonActionResult.Instance;

        #endregion

        #region Initialize Region

        private bool isIsnitialized = false;

        /// <summary>
        /// Инициализация. 
        /// Вызывается после установки контекста данных.
        /// </summary>
        /// 
        /// <remarks>
        /// Во время вызова метода, свойства (с привязкой) уже инициализированны.
        /// </remarks>  
        /// 
        [NonAction]
        public virtual void InitializeComponent()
        {
            //TODO: Проверить что переопределенные методы с пометкой [NonAction] не вызываются!

            if (!isIsnitialized)
            {
                isIsnitialized = true;
                InitializeComponent_LogTestCaseInformation();
            }

            InitializeComponent_LogTestStepInformation();
        }

        /// <summary>
        /// Функция логирования информации о тестовом случае.
        /// Вызвается один раз для каждого экземпляра класса контроллера
        /// </summary>
        protected virtual void InitializeComponent_LogTestStepInformation()
        {
            Logger.LogInformation("{Number} - {Title}",
                                  ControllerContext.ActionDescriptor.TestStep.Number,
                                  ControllerContext.ActionDescriptor.TestStep.Title);
        }

        /// <summary>
        /// Функция логирования информации о тестовом шаге.
        /// </summary>
        protected virtual void InitializeComponent_LogTestCaseInformation()
        {
            
            Logger.LogInformation("{Number} - {Title}",
                                  ControllerContext.ActionDescriptor.TestCase.Number,
                                  ControllerContext.ActionDescriptor.TestCase.Title);
        }

        /// <summary>
        /// Заключительная функция.
        /// Вызывается после завершения действия.
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        [NonAction]
        public virtual void Finally()
        {
            Finaly_PassedIfNotExecutedAndNullException();
        }

        /// <summary>
        /// Функция формерует успешеый результат проверки. 
        /// Если результат не был изменен ранее.
        /// </summary>
        /// 
        /// <remarks>
        /// Устанавливает результат проверки как <see cref="ActionDescriptorResultType.Passed"/>
        /// если <see cref="ActionDescriptor.Result"/> не установлен.      
        /// </remarks>
        protected virtual void Finaly_PassedIfNotExecutedAndNullException()
        {
            if (ControllerContext.ActionDescriptor.Result.IsEmpty) 
            {
                ControllerContext.ActionDescriptor.SetResult(ActionDescriptorResultType.Passed);
            }
        }

        #endregion
    }

    [Controller]
    public abstract class ControllerBase<T> : IModelExpected<T>
        where T : class
    {
        [BindData("")]
        public T Model { get; set; }
    }
}
