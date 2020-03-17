
/* Unmerged change from project 'AtpNetCore.Mvc.Core (netcoreapp3.1)'
Before:
using NLib.AtpNetCore.Testing.Mvc.Controllers;
using System;
using System.Linq;
After:
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Testing.Mvc.Controllers;
using NLib.AtpNetCore.Testing.Mvc.Runtime;
using System;
*/
using EquipApps.Mvc;
using NLib.AtpNetCore.Testing.Mvc.Controllers;

/* Unmerged change from project 'AtpNetCore.Mvc.Core (netcoreapp3.1)'
Before:
using System.Text;
using NLib.AtpNetCore.Testing.Mvc.Runtime;
using NLib.AtpNetCore.Mvc;
After:
using System.Linq;
using System.Text;
*/
using System;
using System.Linq;

namespace NLib.AtpNetCore.Testing.Mvc
{
    public class ControllerJumpToResult : IActionResult
    {
        private string _actionName;

        public ControllerJumpToResult(string actionName)
        {
            _actionName = actionName;
        }

        public void ExecuteResult(ActionContext context)
        {
            var controllerContext = context as ControllerContext;
            if (controllerContext == null)
                throw new ArgumentNullException(nameof(controllerContext));

            //-- Извлекаем.. 
            var actionDescriptor = controllerContext
                .ActionDescriptor
                .TestCase
                .TestSteps.FirstOrDefault(x => x.ActionModel.Name == _actionName)?.ActionDescriptor;

            if (actionDescriptor == null)
            {
                throw new InvalidOperationException(nameof(ControllerJumpToResult));
            }

            controllerContext
                .RuntimeContext
                .Enumerator
                .JumpTo(actionDescriptor);
        }
    }
}
