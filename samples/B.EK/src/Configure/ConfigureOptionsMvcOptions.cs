using EquipApps.Mvc;
using EquipApps.Testing;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace B.EK.Configure
{
    public class ConfigureOptionsMvcOptions : IConfigureOptions<MvcOptions>
    {
        private IOptions<TestOptions> testOptions;

        public ConfigureOptionsMvcOptions(IOptions<TestOptions> options)
        {
            this.testOptions = options;
        }

        public void Configure(MvcOptions options)
        {
            options.FeatureConvetions.Add(new MvcFeatureConvetion(testOptions));
        }
    }


    /// <summary>
    /// Фильрация алгоритма проверки
    /// </summary>
    public class MvcFeatureConvetion : IMvcFeatureConvetion
    {
        private TestOptions options;

        public MvcFeatureConvetion(IOptions<TestOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public void Apply(IMvcFeature feature)
        {
            var executingMode = options.GetExecutingMode<string>();

            //-- Реализация фильра
            if (executingMode == Settings.ExecutingMode_Operate)
            {
                feature.ActionObjects = feature.ActionObjects
                    .Where(x => x.ActionDescriptor.Area == null ? false : x.ActionDescriptor.Area.Contains(Areas.Operate))
                    .ToList();

                return;
            }

            //-- Реализация фильра
            if (executingMode == Settings.ExecutingMode_Power)
            {
                feature.ActionObjects = feature.ActionObjects
                    .Where(x => x.ActionDescriptor.Area == null ? false : x.ActionDescriptor.Area.Contains(Areas.Power))
                    .ToList();

                return;
            }

            if (executingMode == Settings.ExecutingMode_Main)
            {
                //-- Реализация фильра для 
                feature.ActionObjects = feature.ActionObjects
                       .Where(x => x.ActionDescriptor.Area == null)
                       .ToList();

                return;
            }
        }
    }
}
