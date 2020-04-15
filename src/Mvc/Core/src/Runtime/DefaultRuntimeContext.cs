using EquipApps.Mvc.Infrastructure;
using EquipApps.Testing;
using System;

namespace EquipApps.Mvc.Runtime
{
    public class DefaultRuntimeContext : RuntimeContext
    {
        private readonly IActionDescriptorEnumerator runtimeActionEnumerator;

        public DefaultRuntimeContext(TestContext testContext,
                                     IActionDescriptorEnumerator runtimeActionEnumerator)
            :base(testContext)
        {
            this.runtimeActionEnumerator = runtimeActionEnumerator;
        }

        public override bool JumpTo(ActionDescriptor actionDescriptor)
        {
            return runtimeActionEnumerator.JumpTo(actionDescriptor);
        }

        public override bool JumpTo(RuntimeState runtimeState)
        {
            //TODO: Реализовать функцию
            throw new NotImplementedException(nameof(JumpTo));
        }
    }
}
