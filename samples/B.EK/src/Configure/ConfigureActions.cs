using EquipApps.Mvc.Abstractions;
using EquipApps.Testing;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace B.EK.Configure
{
    /// <summary>
    /// Фильрация алгоритма проверки
    /// </summary>
    public class ConfigureActions : ActionDescripterFilterBase
    {
        private TestOptions options;

        public ConfigureActions(IOptions<TestOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override void OnFilterExecuted(ActionDescriptorContext context)
        {
            var executingMode = options.GetExecutingMode<string>();

            //-- Реализация фильра
            if (executingMode == Settings.ExecutingMode_Operate)
            {
                context.Results = context.Results
                    .Where(x => x.Area == null ? false : x.Area.Contains(Areas.Operate))
                    .ToList();

                return;
            }

            //-- Реализация фильра
            if (executingMode == Settings.ExecutingMode_Power)
            {
                context.Results = context.Results
                    .Where(x => x.Area == null ? false : x.Area.Contains(Areas.Power))
                    .ToList();

                return;
            }

            if (executingMode == Settings.ExecutingMode_Main)
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
