using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.PowerSource;
using Microsoft.Extensions.Options;
using System;
using System.Transactions;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610
{
    public class Psh3610_Adapter : HardwareAdapterBase<Psh3610_Library>
    {
        private readonly Psh3610_Options options;
        private Psh3610_Library library;

        public Psh3610_Adapter(IOptions<Psh3610_Options> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override void Dispose()
        {
           
        }

        protected override void Adapt(Psh3610_Library device, string deviceName)
        {
            library = device;
        }

        protected override void AttachBehaviors()
        {
            foreach (var device in options.DeviceCollection)
            {
                //-- Извлекаем виртуальное устройство по имени
                var hardware = this.HardwareFeature.HardwareCollection[device.Name];
                if (hardware == null)
                {
                    //TODO: Добавит лог.
                    continue;
                }

                var sync = new Psh3610_Sync(device, library);

                //-- Создаем поведение.
                var behavior = new PowerSourceBehavior();
                    behavior.ValueChange += sync.Behavior_ValueChange;
                    behavior.ValueUpdate += sync.Behavior_ValueUpdate;

                //-- Сохпвняем поведение
                hardware.Behaviors.AddOrUpdate(behavior);
            }
        }

        /// <summary>
        /// Регистрируем <see cref="PowerSourceBehavior"/>
        /// </summary>
        private void RegisterPowerSourceBehavior()
        {
            
        }





        /// <summary>
        /// Синхронизатор
        /// </summary>
        private class Psh3610_Sync
        {
            private readonly Psh3610_Library library;
            private readonly Psh3610_Device  device;



            public Psh3610_Sync(Psh3610_Device device, Psh3610_Library library)
            {
                this.device  = device;
                this.library = library;
            }


            public void Behavior_ValueUpdate(ValueBehaviorBase<PowerSourceState> behavior)
            {
                throw new NotImplementedException();
            }

            public void Behavior_ValueChange(ValueBehaviorBase<PowerSourceState> behavior, PowerSourceState state)
            {


                //-- Изменяем значение.
                behavior.SetValue(state);

               


                
            }
        }

        private class Psh3610_Enlistment : IEnlistmentNotification
        {
            private bool _enlisted = false;
            private readonly Psh3610_Library library;
            private readonly Psh3610_Device device;
            private PowerSourceState _original;

            public Psh3610_Enlistment(Psh3610_Device device, Psh3610_Library library, PowerSourceState powerSourceState)
            {
                this.device = device;
                this.library = library;
            }

            public void SetValue(PowerSourceState value)
            {
                if (!Enlist())
                {
                    _original = value;
                }

                SetValueCurrent(value);
            }

            protected virtual void SetValueCurrent(PowerSourceState value)
            {
                ushort result = 0;

                //-- Изменяем состояние
                switch (value)
                {
                    //-- ВЫКЛ ИП
                    case PowerSourceState.OFF:
                        {
                            result = library.OUTPUT(device.Number, 1);
                        }
                        break;
                    //-- ВКЛ ИП
                    case PowerSourceState.ON:
                        {
                            result = library.OUTPUT(device.Number, 0);
                        }
                        break;
                    //-- ОШИБКА
                    default:
                        throw new InvalidOperationException();
                }

                //-- Проваерка результата функции

                if (result != 0)
                {
                    //-- Ошибка!
                    throw new InvalidOperationException("ERROR");
                }
            }

            protected bool Enlist()
            {
                if (_enlisted)
                    //-- Идет ранзакция
                    return true;

                var currentTx = Transaction.Current;
                if (currentTx == null)
                {
                    //-- Нет ранзакция
                    return false;
                }

                //- Зарегистрировались
                currentTx.EnlistVolatile(this, EnlistmentOptions.None);
                _enlisted = true;

                return true;
            }

            public void Commit(Enlistment enlistment)
            {
                throw new NotImplementedException();
            }

            public void InDoubt(Enlistment enlistment)
            {
                throw new NotImplementedException();
            }

            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                throw new NotImplementedException();
            }

            public void Rollback(Enlistment enlistment)
            {
                throw new NotImplementedException();
            }
        }
    }
}
