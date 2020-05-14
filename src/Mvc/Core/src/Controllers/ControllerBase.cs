using EquipApps.Testing;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System;

namespace EquipApps.Mvc.Controllers
{
    [Controller]
    public abstract class ControllerBase
    {
        private ControllerContext _controllerContext;

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

        private volatile bool isIsnitialized = false;

        /// <summary>
        /// <para> Инициализация. (Вызывается после установки контекста данных).</para>
        /// </summary>  
        /// 
        [NonAction]
        public abstract void InitializeComponent();

        /// <summary>
        /// Заключительная функция.
        /// Вызывается после завершения действия.
        /// </summary>
        /// 
        [NonAction]
        public abstract void Finally();

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
