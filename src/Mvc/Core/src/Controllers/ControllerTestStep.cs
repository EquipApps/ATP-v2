using EquipApps.Mvc;
using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.Objects;
using System;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerTestStep : TestObject
    {
        public ControllerTestStep(MethodModel actionModel, int index, int? indexSecond = null)
        {
            ActionModel = actionModel ?? throw new ArgumentNullException(nameof(actionModel));
            Index       = index;
            IndexSecond = indexSecond;
        }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptor"/>
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; internal set; }

        /// <summary>
        /// Возвращает <see cref="ControllerModel"/>
        /// </summary>
        public MethodModel ActionModel { get; }

        /// <summary>
        /// Возвращает порядковый номер
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Возвращает порядковый номер
        /// </summary>
        public int? IndexSecond { get; }

        /// <summary>
        /// Возвращает <see cref="TestCase"/>
        /// </summary>
        public new ControllerTestCase Parent
        {
            get => (ControllerTestCase)base.Parent;
            set => base.Parent = value;
        }

        #region TestObject

        public override int ChildrenCount => 0;

        public override TreeObject GetChild(int index)
        {
            throw new InvalidOperationException("TestStep (Тестовый шаг) - Не содержит дочерних элементов");
        }

        protected override string GetTitle()
        {
            if (ActionModel.TitleBinder != null)
            {
                var bindingResult = ActionModel.TitleBinder.Bind(this);
                if (bindingResult.IsModelSet)
                    return bindingResult.Model.ToString();
                else
                    return "Ошибка привязки";
            }

            return ActionModel.Title ?? ActionModel.Name ?? ActionModel.Info.Name;
        }

        protected override TestNumber GetNumber()
        {
            if (ActionModel.NumberBinder != null)
            {
                var bindingResult = ActionModel.NumberBinder.Bind(this);
                if (bindingResult.IsModelSet)
                    return bindingResult.Model.ToString();
                else
                    return "Ошибка привязки";
            }

            return ActionModel.Number
                ?? (IndexSecond.HasValue ? string.Format("{0}.{1}", Index, IndexSecond.Value) : Index.ToString());
        }

        #endregion
    }
}