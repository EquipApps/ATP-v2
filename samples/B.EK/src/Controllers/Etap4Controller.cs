using B.EK.Models;
using EquipApps.Hardware;
using EquipApps.Hardware.Extensions;
using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace B.EK.Controllers
{
    [Case(4, "Проверка <Name>")]
    public class Etap4Controller : Controller<Command>, IEnableContext
    {
        double U30_P = 27;

        //[BindData(nameof(Command.CommandEtalons) +"[0]")]
        [Step("1.1", "1 канал(выдача с замером Uвых)")]
        public void Action11(string W1)
        {
            

            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.One,  Model.W1);
                this.DigitalSwitch(DigitalState.Null, Model.W2);
                this.DigitalSwitch(DigitalState.Null, Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalOFF();

            //-- Аналоговый контроль
            ValidateAnalogOFF();
        }

        

        [Step("1.2", "1 канал(снятие)")]
        public void Action12()
        {
            //-- Снятие
            СommandOff();
        }


        //[BindData(nameof(Command.CommandEtalons) + "[1]")]
        [Step("2.1", "2 канал(выдача с замером Uвых)")]
        public void Action21()
        {
            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.Null, Model.W1);
                this.DigitalSwitch(DigitalState.One,  Model.W2);
                this.DigitalSwitch(DigitalState.Null, Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalOFF();

            //-- Аналоговый контроль
            ValidateAnalogOFF();
        }

        [Step("2.2", "2 канал(снятие)")]
        public void Action22()
        {
            //-- Снятие
            СommandOff();
        }


        //[BindData(nameof(Command.CommandEtalons) + "[2]")]
        [Step("3.1", "3 канал(выдача с замером Uвых)")]
        public void Action31()
        {
            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.Null, Model.W1);
                this.DigitalSwitch(DigitalState.Null, Model.W2);
                this.DigitalSwitch(DigitalState.One, Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Запрос цифровых состояний
            var digitals = this.DigitalRequest();

            //-- Цифровой контроль
            ValidateDigitalOFF();

            //-- Аналоговый контроль
            ValidateAnalogOFF();
        }

        [Step("3.2", "3 канал(снятие)")]
        public void Action32()
        {
            //-- Снятие
            СommandOff();
        }


        //[BindData(nameof(Command.CommandEtalons) + "[3]")]
        [Step("4.1", "1,2 канал(выдача с замером Uвых)")]
        public void Action41()
        {
            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.One,  Model.W1);
                this.DigitalSwitch(DigitalState.One,  Model.W2);
                this.DigitalSwitch(DigitalState.Null, Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalON();

            //-- Аналоговый контроль
            ValidateAnalogON();
        }

        [Step("4.2", "1,2 канал(снятие)")]
        public void Action42()
        {
            //-- Снятие
            СommandOff();
        }


        //[BindData(nameof(Command.CommandEtalons) + "[4]")]
        [Step("5.1", "1,3 канал(выдача с замером Uвых)")]
        public void Action51()
        {
            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.One,    Model.W1);
                this.DigitalSwitch(DigitalState.Null,   Model.W2);
                this.DigitalSwitch(DigitalState.One,    Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalON();

            //-- Аналоговый контроль
            ValidateAnalogON();
        }

        [Step("5.2", "1,3 канал(снятие)")]
        public void Action52()
        {
            //-- Снятие
            СommandOff();
        }


        //[BindData(nameof(Command.CommandEtalons) + "[5]")]
        [Step("6.1", "2,3 канал(выдача с замером Uвых)")]
        public void Action61()
        {
            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.Null,   Model.W1);
                this.DigitalSwitch(DigitalState.One,    Model.W2);
                this.DigitalSwitch(DigitalState.One,    Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalON();

            //-- Аналоговый контроль
            ValidateAnalogON();

        }

        [Step("6.2", "2,3 канал(снятие)")]
        public void Action62()
        {
            //-- Снятие
            СommandOff();
        }


        //[BindData(nameof(Command.CommandEtalons) + "[6]")]
        [Step("7.1", "1,2,3 канал(выдача с замером Uвых)")]
        public void Action71()
        {
            //-- Выдача Цифра
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.One, Model.W1);
                this.DigitalSwitch(DigitalState.One, Model.W2);
                this.DigitalSwitch(DigitalState.One, Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalON();

            //-- Аналоговый контроль
            ValidateAnalogON();
        }

        [Step("7.2", "1,2,3 канал(снятие)")]
        public void Action72()
        {
            //-- Снятие
            СommandOff();
        }




        private void СommandOff()
        {
            using (var transactionScope = new TransactionScope())
            {
                this.DigitalSwitch(DigitalState.Null, Model.W1);
                this.DigitalSwitch(DigitalState.Null, Model.W2);
                this.DigitalSwitch(DigitalState.Null, Model.W3);

                transactionScope.Complete();
            }

            this.RelayTransaction(RelayState.Disconnect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Цифровой контроль
            ValidateDigitalOFF();

            //-- Аналоговый контроль
            ValidateAnalogOFF();
        }

        private void ValidateDigitalON() 
        {
            //-- Запрос цифровых состояний
            var digitals = this.DigitalRequest();

            //-- Цифровой контроль
            DigitalValidate(digitals, DigitalState.One, Model.R1);
            DigitalValidate(digitals, DigitalState.One, Model.R2);
            DigitalValidate(digitals, DigitalState.One, Model.R3);

            //-- Релейный контроль
            DigitalValidate(digitals, DigitalState.One, Model.F);

            //-- Контроль остальных линий
            DigitalValidate(digitals, DigitalState.Null);
        }
        private void ValidateDigitalOFF()
        {
            //-- Запрос цифровых состояний
            var digitals = this.DigitalRequest();

            //-- Цифровой контроль
            DigitalValidate(digitals, DigitalState.Null, Model.R1);
            DigitalValidate(digitals, DigitalState.Null, Model.R2);
            DigitalValidate(digitals, DigitalState.Null, Model.R3);

            //-- Релейный контроль
            DigitalValidate(digitals, DigitalState.Null, Model.F);

            //-- Контроль остальных линий
            DigitalValidate(digitals, DigitalState.Null);
        }
        private void ValidateAnalogON()  
        {
            var voltage = this.MeasureVoltage();
            var voltagelimit = 0.95 * U30_P;

            if (voltage < voltagelimit)
            {
                this.Logger.LogError
                    ("Напряжение: {value} (В); Верх.граница: {llim} (В)", voltage, voltagelimit);

                this.ErrorOK();
            }
            else
            {
                this.Logger.LogInformation
                    ("Напряжение: {value} (В); Верх.граница: {hlim} (В)", voltage, voltagelimit);
            }
        }
        private void ValidateAnalogOFF() 
        {
            var voltage = this.MeasureVoltage();
            if (voltage == 0)
            {
                this.Logger.LogDebug("Напряжение: {value} (В);", voltage);
            }
            else
            {
                this.Logger.LogError("Напряжение: {value} (В);", voltage);
                this.ErrorOK();
            }
        }
    }
}
