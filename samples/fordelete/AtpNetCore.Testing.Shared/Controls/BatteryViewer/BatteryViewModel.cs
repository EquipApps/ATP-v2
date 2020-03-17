using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace EquipApps.WorkBench.Controls.BatteryViewer
{
    /// <summary>
    /// Модель представление источника питания.
    /// </summary>
    /// 
    /// <remarks>
    /// Подписывается на изменение состояния.
    /// </remarks>
    public class BatteryViewModel : ReactiveObject, IDisposable
    {
        private BatteryBehavior _behavior;

        public BatteryViewModel(BatteryBehavior behavior)
        {
            _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
        }

       

        [Reactive]
        public string Name      { get; set; }

        [Reactive]
        public double Current { get; set; }

        [Reactive]
        public double Viltage   { get; set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
