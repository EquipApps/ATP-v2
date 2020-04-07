namespace EquipApps.Mvc.ApplicationModels
{
    public static class ControllerModelEx
    {
        public static string GetRouteValueArea(this ControllerModel controllerModel)
        {
            if (controllerModel.RouteValues.TryGetValue("area", out string area))            
                return area;            
            else
                return null;
        }



        public static string GetOrderValueController(this ControllerModel controllerModel)
        {
            if (controllerModel.OrderValues.TryGetValue("controller", out string controller))
                return controller;
            else
                return null;
        }
    }
}