using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationModels;
using System;

namespace EquipApps.Mvc.Controllers
{
    public class ControllerTestStep : ActionDescriptorObject
    {
        public int? BindIndex { get; }

        public ControllerTestStep(int? bindIndex = null)
        {
            BindIndex = bindIndex;
        }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptor"/>
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; internal set; }
   
        /// <summary>
        /// Возвращает <see cref="TestCase"/>
        /// </summary>
        public override IHierarhicalDataObject  Parent
        {
            get;
            set; 
        }

        #region TestObject

        public override int ChildrenCount => 0;

        public override IHierarhicalDataObject GetChild(int index)
        {
            throw new InvalidOperationException("TestStep (Тестовый шаг) - Не содержит дочерних элементов");
        }

        #endregion
    }
}