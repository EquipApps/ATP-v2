using EquipApps.Mvc.Objects;
using System;
using System.Reflection;

namespace EquipApps.Mvc.Controllers
{
    public class ControllerActionDescriptor : ActionDescriptor
    {
        private TestNumber _number;


        public ControllerActionDescriptor(ControllerTestCase testCase, ControllerTestStep testStep)
            : base(testCase, testStep)
        {
            TestStep.ActionDescriptor = this;

            MethodInfo         = testStep.ActionModel.ActionMethod;
            ControllerTypeInfo = testCase.ControllerModel.ControllerType;

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


        public MethodInfo MethodInfo { get; set; }
        public TypeInfo ControllerTypeInfo { get; set; }



        public override TestNumber Number => _number;

        public override string Title => TestStep.Title;
    }
}
