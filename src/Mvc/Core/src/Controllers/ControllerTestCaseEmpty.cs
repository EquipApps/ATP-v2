using EquipApps.Mvc.Objects;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerTestCaseEmpty : ControllerTestCase
    {
        public ControllerTestCaseEmpty()
        {
        }

        protected override string GetTitle()
        {
            return string.Empty;
        }

        protected override TestNumber GetNumber()
        {
            return null;
        }
    }
}
