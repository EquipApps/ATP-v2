using System;
using System.Linq;

namespace EquipApps.Mvc
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
            //if (context is ControllerContext controllerContext)
            //{
            //    
            //    //-- Извлекаем.. 
            //    var actionDescriptor = controllerContext
            //        .ActionDescriptor
            //        .TestCase
            //        .TestSteps.FirstOrDefault(x => x.ActionDescriptor.RouteValues["action"] == _actionName)?.ActionDescriptor;

            //    if (actionDescriptor == null)
            //    {
            //        throw new InvalidOperationException(nameof(ControllerJumpToResult));
            //    }

            //    controllerContext.RuntimeContext.JumpTo(actionDescriptor);
            //}
            //else
                throw new InvalidOperationException(nameof(ControllerJumpToResult));
        }
    }
}
