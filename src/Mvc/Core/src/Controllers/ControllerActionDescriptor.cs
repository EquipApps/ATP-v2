using EquipApps.Mvc.ApplicationModels;
using Microsoft.Extensions.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace EquipApps.Mvc.Controllers
{
    [DebuggerDisplay("{DisplayName}")]
    public class ControllerActionDescriptor : ActionDescriptor
    {
        public string ControllerName { get; set; }

        public virtual string ActionName { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public TypeInfo ControllerTypeInfo { get; set; }

        public ControllerActionDescriptor(ControllerTestCase testCase, ControllerTestStep testStep)
            : base(testCase, testStep)
        {
            TestStep.ActionDescriptor = this;
        }

        public new ControllerTestCase TestCase
        {
            get => (ControllerTestCase)base.TestCase;
        }

        public new ControllerTestStep TestStep
        {
            get => (ControllerTestStep)base.TestStep;
        }

        public override string DisplayName
        {
            get
            {
                if (base.DisplayName == null && ControllerTypeInfo != null && MethodInfo != null)
                {
                    base.DisplayName = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.{1} ({2})",
                        TypeNameHelper.GetTypeDisplayName(ControllerTypeInfo),
                        MethodInfo.Name,
                        ControllerTypeInfo.Assembly.GetName().Name);
                }

                return base.DisplayName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                base.DisplayName = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public BinderItem[] BoundBinderItem => Properties["bindPrope"] as BinderItem[];      

        /// <summary>
        /// 
        /// </summary>
        public BinderItem[] BinderItem      => Properties["bindParam"] as BinderItem[];
    }
}
