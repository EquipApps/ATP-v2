﻿namespace EquipApps.Mvc.Controllers
{
    public class ControllerContext : ActionContext
    {
        public ControllerContext(ActionContext actionContext)
            : base(actionContext)
        {
        }

        public new ControllerActionDescriptor ActionDescriptor
        {
            get => (ControllerActionDescriptor)base.ActionDescriptor;
        }
    }
}