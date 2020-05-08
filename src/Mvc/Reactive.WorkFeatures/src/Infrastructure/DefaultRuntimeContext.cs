using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using System;
using System.Linq.Expressions;

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


        /// <inheritdoc/>  
        public override bool JumpTo(RuntimeState runtimeState)
        {
            //TODO: Реализовать функцию
            throw new NotImplementedException(nameof(JumpTo));
        }

        /// <inheritdoc/>        
        public override bool JumpTo(ActionObject actionObject)
        {
            if (actionObject == null)
            {
                throw new ArgumentNullException(nameof(actionObject));
            }

            return actionObjectEnumerator.JumpTo(actionObj =>
            {
                return actionObject.Equals(actionObj);
            });
        }

        /// <inheritdoc/>  
        public override bool JumpTo(ActionDescriptor actionDescriptor)
        {
            if(actionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptor));
            }

            return actionObjectEnumerator.JumpTo(actionObj =>
            {
                return actionDescriptor.Equals(actionObj.ActionDescriptor);
            });
        }
    }
}
