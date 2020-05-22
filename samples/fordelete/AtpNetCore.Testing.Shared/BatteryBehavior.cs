using EquipApps.Hardware;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EquipApps.WorkBench
{
    public class BatteryBehavior : IHardwareBehavior
    {
        private ReplaySubject<BatteryStatus> _valueComonentSubject = new ReplaySubject<BatteryStatus>();

        public IHardware Hardware { get ; set; }

        public IObservable<BatteryStatus> BatteryStatusObservable => _valueComonentSubject.AsObservable();

        public void Attach()
        {
        }
    }


    /// <summary>
    /// Состояние источника питания
    /// </summary>
    public struct BatteryStatus
    {
        /// <summary>
        /// Напряжение источника питания.
        /// </summary>
        public double Voltage { get; set; }

        /// <summary>
        /// Ток источниика питания.
        /// </summary>
        public double Current { get; set; }
    }
}
