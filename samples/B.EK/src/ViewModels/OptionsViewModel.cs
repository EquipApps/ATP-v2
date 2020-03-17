using B.EK.Configure;
using EquipApps.Testing;
using EquipApps.WorkBench.ViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace B.EK.ViewModels
{
    /// <summary>
    /// Модель представление ввода данных.  
    /// </summary>
    public class OptionsViewModel : FlyoutSettingsViewModelBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OptionsViewModel(
            IOptions<TestOptions> options) : base(options)
        {
            //--Создаем Правила
            UserNameRule = this.ValidationRule(vm => vm.UserName,   x => !string.IsNullOrWhiteSpace(x), "Поле не заполнено");
            NumberRule   = this.ValidationRule(vm => vm.Number,     x => !string.IsNullOrWhiteSpace(x), "Поле не заполнено");

            
            this.WhenAnyValue(x => x.WorkingMode)
                .Subscribe   (x =>
                {
                    if(x == Settings.WorkingMode_PC)
                    {
                        ExecutingModeIsEnabled = false;
                        PowerModeIsEnabled = false;

                        ExecutingMode   = Settings.ExecutingMode_Main;
                        PowerMode       = Settings.PowerMode_NOM;
                    }
                    else
                    {
                        ExecutingModeIsEnabled = true;
                        PowerModeIsEnabled = true;
                    }
                });
               

          
        }

        public ValidationHelper UserNameRule { get; }

        public ValidationHelper NumberRule   { get; }

        /// <summary>
        /// Имя оператора
        /// </summary>
        [Reactive] public string UserName   { get; set; }

        /// <summary>
        /// Номер продукции (Заводской номер)
        /// </summary>
        [Reactive] public string Number     { get; set; }

        /// <summary>
        /// Режим работ - выбранный
        /// </summary>
        [Reactive] public string WorkingMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public bool ExecutingModeIsEnabled { get; set; }

        /// <summary>
        /// Режим работ - выбранный
        /// </summary>
        [Reactive] public string ExecutingMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public bool PowerModeIsEnabled { get; set; }

        /// <summary>
        /// Режим работ - выбранный
        /// </summary>
        [Reactive] public string PowerMode { get; set; }



        protected override void LoadOptinos(TestOptions options)
        {
            WorkingMode   = options.GetWorkingMode  <string>();
            ExecutingMode = options.GetExecutingMode<string>();
            PowerMode     = options.GetPowerMode    <string>();
        }

        protected override void SaveOptinos(TestOptions options)
        {
            options.SetWorkingMode  <string>  (WorkingMode);
            options.SetExecutingMode<string>  (ExecutingMode);
            options.SetPowerMode    <string>  (PowerMode);
        }
    }
}

