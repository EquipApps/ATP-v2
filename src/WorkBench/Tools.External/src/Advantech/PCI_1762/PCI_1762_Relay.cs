using EquipApps.Hardware;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EquipApps.WorkBench.Tools.External.Advantech.PCI_1762
{
    public class PCI_1762_Relay : RelayBehavior, IRelayBehavior
    {
        public PCI_1762_Relay(byte number)
        {
            this.Number = number;
        }

        public byte Number { get; }

        public string Name { get; set; }

    }

    public class PCI_1762_Relay_Manager : IEnlistmentNotification
    {
        private bool _enlisted;

        public PCI_1762_Relay_Manager()
        {
            //curent = new byte[16];
        }

        /// <summary>
        /// Замкнуть реле
        /// </summary>
        /// <param name="numb">Номер реле</param>
        public void AddRelay(byte numb)
        {
            if (Enlist())
            {
                //-- Транзакция
            }
            else
            {

            }
        }

        /// <summary>
        /// Разомкнуть реле
        /// </summary>
        /// <param name="numb">Номер реле</param>
        public void DelRelay(byte numb)
        {
            
            if(Enlist())
            {
                //-- Транзакция
            }
            else
            {

            }

        }



        private bool Enlist()
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

        void IEnlistmentNotification.Commit(Enlistment enlistment)
        {
            
        }

        void IEnlistmentNotification.InDoubt(Enlistment enlistment)
        {
            
        }

        void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
        {
            
        }

        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            
        }
    }
}
