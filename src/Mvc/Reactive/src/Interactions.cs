using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Reactive
{
    public static class Interactions
    {
        //-- Важно чтобы результат был установлен иначе крашется!
        public static Interaction<Exception, Unit> InteractionCreateException { get; }
            = new Interaction<Exception, Unit>(RxApp.MainThreadScheduler);

        public static Interaction<Unit, bool> InteractionAcceptSettings { get; }
           = new Interaction<Unit, bool>(RxApp.MainThreadScheduler);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// </returns>
        public static async Task<bool> HandleAcceptSettings()
        {
            try
            {
                var result = await InteractionAcceptSettings.Handle(Unit.Default);

                return result;
            }
            catch (Exception innerException)
            {
                LogHost.Default.Error(new InvalidOperationException(nameof(InteractionCreateException), innerException));
                return false;
            }
        }

        public static async Task HandleCreateException(Exception exception)
        {
            try
            {
                var result = await InteractionCreateException.Handle(exception);
            }
            catch (Exception innerException)
            {
                LogHost.Default.Error(new InvalidOperationException(nameof(InteractionCreateException), innerException));
            }
        }

    }
}
