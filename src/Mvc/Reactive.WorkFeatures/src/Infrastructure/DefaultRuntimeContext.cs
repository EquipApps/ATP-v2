using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using System;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure
{
    public class DefaultRuntimeContext : RuntimeContext
    {
        private readonly IActionObjectEnumerator actionObjectEnumerator;

        public DefaultRuntimeContext(TestContext testContext,
                                     IActionObjectEnumerator actionObjectEnumerator)
            : base(testContext)
        {
            this.actionObjectEnumerator = actionObjectEnumerator;
        }

        public override bool JumpTo(ActionObject actionObject)
        {
            return actionObjectEnumerator.JumpTo(actionObject);
        }

        public override bool JumpTo(RuntimeState runtimeState)
        {
            //TODO: Реализовать функцию
            throw new NotImplementedException(nameof(JumpTo));
        }
    }
}
