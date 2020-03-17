using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Objects;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerActionDescriptor : ActionDescriptor
    {
        private TestNumber _number;
       

        public ControllerActionDescriptor(ControllerTestCase testCase, ControllerTestStep testStep)
            :base(testCase, testStep)
        {
            TestStep.ActionDescriptor = this;

            _number = new TestNumberBuilder()             
                .Append(testCase.Number)
                .Append(testStep.Number)
                .Build();
        }

        public new ControllerTestCase TestCase
        {
            get => (ControllerTestCase)base.TestCase;
        }
        public new ControllerTestStep TestStep
        {
            get => (ControllerTestStep)base.TestStep;
        }


        public override string Area => TestCase.ControllerModel.Area;

        public override TestNumber Number => _number;

        public override string Title    => TestStep.Title;
    }
}
