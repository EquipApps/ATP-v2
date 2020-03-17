using System.Threading.Tasks;

namespace EquipApps.Mvc
{
    public abstract class ActionResult : IActionResult
    {
        public virtual Task ExecuteResultAsync(ActionContext context)
        {
            ExecuteResult(context);
            return Task.CompletedTask;
        }

        public virtual void ExecuteResult(ActionContext context)
        {
        }
    }
}
