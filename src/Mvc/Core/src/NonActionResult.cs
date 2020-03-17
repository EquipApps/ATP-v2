using EquipApps.Mvc;

namespace NLib.AtpNetCore.Mvc
{
    public class NonActionResult : IActionResult
    {
        public static IActionResult Instance = new NonActionResult();

        private NonActionResult()
        {

        }

        public void ExecuteResult(ActionContext context)
        {
            
        }
    }
}
