using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.Objects;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerTestSuit : TestObject
    {
        private int index1;

        public ControllerTestSuit(ControllerModel controllerModel, int index)
        {
            ControllerModel = controllerModel ?? throw new ArgumentNullException(nameof(controllerModel));
            TestCases = new List<ControllerTestCase>();
            Index = index;
        }

        public ControllerTestSuit(ControllerModel controllerModel, int index, int index1) : this(controllerModel, index)
        {
            this.index1 = index1;
        }

        /// <summary>
        /// Возвращает <see cref="ControllerModel"/>
        /// </summary>
        public ControllerModel ControllerModel { get; }

        /// <summary>
        /// Возвращает порядковый номер
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Возвращает коллекцию <see cref="TestCase"/>
        /// </summary>
        public IList<ControllerTestCase> TestCases { get; }

        #region TestObject

        public override int ChildrenCount => TestCases.Count;

        public override TreeObject GetChild(int index)
        {
            return TestCases[index];
        }

        protected override string GetTitle()
        {
            if (ControllerModel.TitleBinder != null)
            {
                var bindingResult = ControllerModel.TitleBinder.Bind(this);
                if (bindingResult.IsModelSet)
                    return bindingResult.Model.ToString();
                else
                    return "Ошибка привязки";
            }

            return ControllerModel.Title
                ?? ControllerModel.Name
                ?? ControllerModel.Info.Name;
        }

        protected override TestNumber GetNumber()
        {
            if (ControllerModel.Index.HasValue)
                return ControllerModel.Index.Value.ToString(); //TODO TestNumber должен создаваться из INT!?
            else
                return Index.ToString();
        }

        #endregion
    }
}