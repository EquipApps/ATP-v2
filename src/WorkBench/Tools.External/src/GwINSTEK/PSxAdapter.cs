using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.Toggling;
using System;
using System.Transactions;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{
    /// <summary>
    /// <para>Адаптер источников питания</para>
    /// <para>PSP-2010; PSP-405;</para>
    /// </summary>
    /// <typeparam name="TPowerSource"></typeparam>
    public class PSxAdapter<TPowerSource> : HardwareAdapterBase<TPowerSource>, IEnlistmentNotification
        where TPowerSource : class, IPowerSource
    {
        public PSxAdapter()
        {

        }

        //-------------------------------------------------------------------------------------------

        public TPowerSource Device     
        { 
            get; private set; 
        }

        public string       DeviceName 
        { 
            get; private set; 
        }

        //-------------------------------------------------------------------------------------------

        public ToggleBehavior ToggleBehavior 
        { 
            get; private set; 
        }

        public override void Dispose()
        {
            (Device as IDisposable)?.Dispose();
             Device = null;
        }

        //-------------------------------------------------------------------------------------------

        protected override void Adapt(TPowerSource device, string deviceName)
        {
            Device     = device;
            DeviceName = deviceName;
        }

        protected override void AttachBehaviors()
        {
            //-- Извлекаем виртуальное устройство по имени
            var hardware = this.HardwareFeature.HardwareCollection[DeviceName];
            if (hardware == null)
            {
                //TODO: Добавит лог.
                return;
            }

            ToggleBehavior = new ToggleBehavior();
            ToggleBehavior.ValueChange += ToggleBehavior_ValueChange;

            //-- Сохпвняем поведение
            hardware.Behaviors.AddOrUpdate(ToggleBehavior);
        }

        protected override void InitializeDevice()
        {
            throw new NotImplementedException();
        }

        protected override void ResetDevice()
        {
            throw new NotImplementedException();
        }

        //-------------------------------------------------------------------------------------------

        private void ToggleBehavior_ValueChange(ValueBehaviorBase<Toggle>  behavior, Toggle value)
        {
            if(Enlist())
            {
                //-- Идет транзакция
            }
            else
            {
                ToggleValueChange();
            }
        }

        private void ToggleValueChange()
        {

        }





        /* 
         * Реализация IEnlistmentNotification        
         */

        #region EnlistmentRegion

        private volatile bool _enlisted;        

        private bool Enlist()
        {
            if (_enlisted)
                //-- Идет транзакция
                return true;

            var currentTx = Transaction.Current;
            if (currentTx == null)
            {
                //-- Нет транзакция
                return false;
            }

            //-- Зарегистрировались СИНХРОНИЗАЦИЮ!
            currentTx.EnlistVolatile(this, EnlistmentOptions.None);

            _enlisted = true;

            return true;
        }

        void IEnlistmentNotification.Commit(Enlistment enlistment)
        {
            //-- Убираем флаг подписки      
            _enlisted = false;
            enlistment.Done();
        }

        void IEnlistmentNotification.InDoubt(Enlistment enlistment)
        {
            //TODO: Что это?
            _enlisted = false;
        }

        void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
        {
            //-- ПОДГОТОВКА ДАННЫХ К ИЗМЕНЕНИЮ
            try
            {
                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                preparingEnlistment.ForceRollback(ex);
            }
        }

        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            //-- ОТКАТ            
        }

        #endregion


        //-------------------------------------------------------------------------------------------

        private enum PSxAdapterTransaction
        {
            ValueChange_Toggle
        }
    }
}
