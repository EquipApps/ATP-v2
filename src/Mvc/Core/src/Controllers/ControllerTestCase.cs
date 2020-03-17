using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.Objects;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerTestCase : TestObject
    {
        internal ControllerTestCase()
        {
            Index = 0;
            TestSteps = new List<ControllerTestStep>();
        }

        public ControllerTestCase(BackgroundModel backgroundModel, int index)
        {
            //BackgroundModel = backgroundModel ?? throw new ArgumentNullException(nameof(backgroundModel));
            Index = index;
            TestSteps = new List<ControllerTestStep>();
        }

        public ControllerTestCase(ControllerModel controllerModel, int index, int? indexSecond = null)
        {
            this.ControllerModel = controllerModel;
            this.Index              = index;
            this.IndexSecond        = indexSecond;
            TestSteps = new List<ControllerTestStep>();
        }

        /// <summary>
        /// Возвращает <see cref="BackgroundModel"/>
        /// </summary>
        //public BackgroundModel BackgroundModel { get; }

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
        /// 
        /// </summary>
        public new ControllerTestSuit Parent
        {
            get => (ControllerTestSuit)base.Parent;
            set => base.Parent = value;
        }

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