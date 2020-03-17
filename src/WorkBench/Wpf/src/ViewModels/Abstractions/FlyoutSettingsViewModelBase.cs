using EquipApps.Testing;
using Microsoft.Extensions.Options;
using ReactiveUI.Fody.Helpers;

namespace EquipApps.WorkBench.ViewModels
{
    /// <summary>
    /// Базовая модель ввода данных через Flyout Control
    /// </summary>
    public abstract class FlyoutSettingsViewModelBase : SettingsViewModelBase
    {
        public FlyoutSettingsViewModelBase(IOptions<TestOptions> options) : base(options)
        {
           
        }

        /// <summary>
        /// Индикатор отображенния. Flyout Control 
        /// </summary>
        [Reactive]
        public bool IsOpen { get; set; } = false;


        protected override void Show()
        {
            IsOpen = true;
        }

        
        protected override void Hide()
        {
            IsOpen = false;
        }


    }
}
