using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Controllers
{
    public class ControllerTestCase : ActionDescriptorObject
    {
        public int? BindIndex { get; }

        public ControllerTestCase(ControllerModel controllerModel, int? bindIndex = null)
        {
            ControllerModel = controllerModel;
            BindIndex       = bindIndex;
            TestSteps       = new List<ControllerTestStep>();
        }

        

        /// <summary>
        /// Возвращает <see cref="ControllerModel"/>
        /// </summary>
        public ControllerModel ControllerModel { get; }


        /// <summary>
        /// Возвращает коллекцию <see cref="TestStep"/>
        /// </summary>
        public IList<ControllerTestStep> TestSteps { get; }


        #region TestObject

        /// <summary>
        /// Возвращает <see cref="TestCase"/>
        /// </summary>
        public override IHierarhicalDataObject Parent
        {
            get;
            set;
        }

        public override int ChildrenCount => TestSteps.Count;

        public override IHierarhicalDataObject GetChild(int index)
        {
            return TestSteps[index];
        }

        #endregion
    }
}