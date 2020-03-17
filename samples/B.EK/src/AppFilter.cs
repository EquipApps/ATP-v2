using B.EK.Configure;
using B.EK.ViewModels;
using EquipApps.Mvc.Abstractions;
using System.Linq;

namespace B.EK
{
    /// <summary>
    /// Фильрация алгоритма проверки
    /// </summary>
    public class AppFilter : ActionDescripterFilterBase
    {
        OptionsViewModel optionsViewModel;

        public AppFilter(OptionsViewModel optionsViewModel)
        {
            this.optionsViewModel = optionsViewModel;
        }

        protected override void OnFilterExecuted(ActionDescriptorContext context)
        {
            var checkMode = optionsViewModel.ExecutingMode;

            //-- Реализация фильра
            if (checkMode == Settings.CHECK_OP)
            {
                context.Results = context.Results
                    .Where(x => x.Area == null ? false : x.Area.Contains("check_Operation"))
                    .ToList();

                return;
            }

            //-- Реализация фильра
            if (checkMode == Settings.CHECK_POW)
            {
                context.Results = context.Results
                    .Where(x => x.Area == null ? false : x.Area.Contains("check_Power"))
                    .ToList();

                return;
            }

            if(checkMode == Settings.CHECK_MAIN)
            {
                //-- Реализация фильра для 
                context.Results = context.Results
                        .Where(x => x.Area == null)
                        .ToList();

                return;
            }
        }
    }
}
