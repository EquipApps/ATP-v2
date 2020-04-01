using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.Objects;
using System.Collections.Generic;

namespace EquipApps.Mvc.Controllers
{
    public class ControllerTestCase : TestObject
    {
        public ControllerTestCase(ControllerModel controllerModel, int index, int? indexSecond = null)
        {
            ControllerModel = controllerModel;
            Index = index;
            IndexSecond = indexSecond;
            TestSteps = new List<ControllerTestStep>();
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
        /// Возвращает порядковый номер
        /// </summary>
        public int? IndexSecond { get; }


        /// <summary>
        /// Возвращает коллекцию <see cref="TestStep"/>
        /// </summary>
        public IList<ControllerTestStep> TestSteps { get; }


        #region TestObject

        public override int ChildrenCount => TestSteps.Count;

        public override TreeObject GetChild(int index)
        {
            return TestSteps[index];
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

            return ControllerModel.Title ?? string.Empty;

        }

       
        protected override TestNumber GetNumber()
        {
            /* Последовательность инициализации индексов
             * 
             * 1) 
             * 
             */

            TestNumberBuilder numberBuilder = new TestNumberBuilder();

            if (ControllerModel.Index.HasValue)
                numberBuilder.Append(ControllerModel.Index.ToString());  //TODO TestNumber должен создаваться из INT!?
            else
                numberBuilder.Append(Index.ToString());


            if (ControllerModel.NumberBinder != null)
            {
                var bindingResult = ControllerModel.NumberBinder.Bind(this);
                if (bindingResult.IsModelSet)
                    numberBuilder.Append(bindingResult.Model.ToString());
                else
                    numberBuilder.Append("Ошибка привязки");

            }
            else
            {
                if (IndexSecond.HasValue)
                {
                    numberBuilder.Append(IndexSecond.Value.ToString());
                }
            }


            return numberBuilder.Build();

        }


        #endregion
    }
}