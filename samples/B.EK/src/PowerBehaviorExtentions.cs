using EquipApps.Hardware;
using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK
{
    public static class PowerBehaviorExtentions
    {
        public static void PowerOn(this IEnableContext enableContext, string powerName)
        {
            //TODO: Реализовать
            throw new NotImplementedException();
        }

        public static void PowerCheckCurrentLimit(this IEnableContext enableContext, string powerName)
        {
            //TODO: Реализовать
            throw new NotImplementedException();
        }

        /// <summary>
        /// Измернение напряжения питани.
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="hardwareName"></param>
        /// <returns></returns>
        public static double MeasureVoltage(this IEnableContext enableContext, string hardwareName = "Multimetr")
        {
            return enableContext
                .ExtractBehavior<MeasureVoltageBehavior>(hardwareName)
                .RequestToMeasure();
        }
    }

    public class MeasureVoltageBehavior : ValueBehaviorBase<double>, IHardwareBehavior
    {
        public override IHardware Hardware 
        { 
            get; 
            set; 
        }
        public override double Value 
        {
            get;          
            protected set;
        }

        public override void RequestToChangeValue(double value)
        {
            throw new InvalidOperationException(nameof(RequestToChangeValue));
        }


        public override void Attach()
        {
            
        }

        public double RequestToMeasure()
        {
            this.RequestToUpdateValue();
            return Value;
        }
    }
}
