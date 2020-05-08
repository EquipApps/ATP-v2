using EquipApps.Mvc.Reactive;
using EquipApps.Testing;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.ViewModels
{
    public enum SettingResult
    {
        /// <summary>
        /// Отмена
        /// </summary>
        Cancel = 0,

        /// <summary>
        /// Успех.
        /// </summary>
        Accept = 1,
    }

    /// <summary>
    /// Базовая модель представление для ввода данных
    /// </summary>
    public abstract class SettingsViewModelBase : ViewModelBase, IValidatableViewModel, IDisposable
    {
        private IDisposable disposableInteraction;      
        private readonly TestOptions options;

        public SettingsViewModelBase(IOptions<TestOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            //-- Создаем Команды
            Accept = ReactiveCommand.Create(OnAccept, this.IsValid());
            Cancel = ReactiveCommand.Create(OnCancel);

            //-- Регистрация
            disposableInteraction =
            Interactions.InteractionAcceptSettings.RegisterHandler(AcceptSettings);
        }

        /// <summary>
        /// Контекст валидации
        /// </summary>
        public ValidationContext ValidationContext { get; } = new ValidationContext();

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public ReactiveCommand<Unit, Unit> Accept { get; }

        /// <summary>
        /// Отменить изменения
        /// </summary>
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        /// <summary>
        /// Результат
        /// </summary>
        [Reactive] public SettingResult? Result { get; private set; } = SettingResult.Cancel;

        /// <summary>
        /// Индикатор отображенния. Flyout Control 
        /// </summary>
        [Reactive] public bool IsOpen { get; set; } = false;

        /// <summary>
        /// По умолчанию очищает подписку
        /// </summary>
        public virtual void Dispose()
        {
            disposableInteraction?.Dispose();
            disposableInteraction = null;
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task AcceptSettings(InteractionContext<Unit, bool> context)
        {
            Result = null;

            //-- Загружаме опции
            LoadOptinos(options);

            //-- Отображение
            Show();

            //-- Ожидаем заершения
            while (!Result.HasValue)
            {
                await Task.Delay(500);
            }

            if (Result == SettingResult.Accept)
            {
                context.SetOutput(true);
            }
            else
            {
                context.SetOutput(false);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void OnAccept()
        {
            Hide();

            SaveOptinos(options);
            Result = SettingResult.Accept;            
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnCancel()
        {
            Hide();
            Result = SettingResult.Cancel;
        }

        /// <summary>
        /// Отобразить
        /// </summary>
        /// <param name="options"></param>
        protected virtual void Show()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Скрыть
        /// </summary>
        /// <param name="options"></param>
        protected virtual void Hide()
        {
            IsOpen = false;
        }

        /// <summary>
        /// Функция загрузки опций.
        /// </summary>
        /// <param name="options"></param>
        protected abstract void LoadOptinos(TestOptions options);

        /// <summary>
        /// Функция сохранений опций.
        /// </summary>
        /// <param name="options"></param>
        protected abstract void SaveOptinos(TestOptions options);
    }
}
