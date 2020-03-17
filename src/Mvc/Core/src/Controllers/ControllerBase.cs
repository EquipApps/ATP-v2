using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Objects;
using EquipApps.Testing;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Testing.Mvc.Controllers;
using System;

namespace NLib.AtpNetCore.Testing.Mvc
{
    [Controller]
    public abstract class ControllerBase
    {
        private ControllerContext _controllerContext;
        protected bool init    = false;
        protected uint counter = 0;

        /// <summary>
        /// Возвращает <see cref="Controllers.ControllerContext"/>
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
                counter++;
            }
        }

        /// <summary>
        /// Возвращает <see cref="EquipApps.Testing.TestContext"/>
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




        #region Extantions for Title

        /// <summary>
        /// Возвращает <see cref="ActionDescriptor.Number"/>
        /// </summary>
        public TestNumber TestNumber => ControllerContext.ActionDescriptor.Number;





        /// <summary>
        /// Возвращает <see cref="ControllerTestCase.Title"/>
        /// </summary>
        public string TitleTestCase => ControllerContext.ActionDescriptor.TestCase.Title;


        /// <summary>
        /// Возвращает <see cref="ControllerTestStep.Title"/>
        /// </summary>
        public string TitleTestStep => ControllerContext.ActionDescriptor.TestStep.Title;

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
                this.ControllerContext.ActionDescriptor.Number,
                this.ControllerContext.ActionDescriptor.Title);
        }

        /// <summary>
        /// Функция логирования информации о тестовом шаге.
        /// </summary>
        protected virtual void InitializeComponent_LogTestCaseInformation()
        {
            Logger.LogInformation("{Number} - {Title}",
                this.ControllerContext.ActionDescriptor.TestCase.Number,
                this.ControllerContext.ActionDescriptor.TestCase.Title);
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
        /// Устанавливает результат проверки как <see cref="Result.Passed"/>
        /// если    <see cref="ActionDescriptor.Result"/>    равен <see cref="Result.NotExecuted"/>
        /// и       <see cref="ActionDescriptor.Exception"/> равен Null
        /// </remarks>
        protected virtual void Finaly_PassedIfNotExecutedAndNullException()
        {
            if ((ControllerContext.ActionDescriptor.Result == Result.NotExecuted) &&
                (ControllerContext.ActionDescriptor.Exception == null))
            {
                this.ControllerContext.ActionDescriptor.Result = Result.Passed;
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
