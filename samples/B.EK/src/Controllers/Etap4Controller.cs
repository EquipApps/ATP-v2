using B.EK.Models;
using EquipApps.Hardware;
using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.Logging;
using System.Threading;
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
                this.LineSwitch(1,  Model.W1);
                this.LineSwitch(0, Model.W2);
                this.LineSwitch(0, Model.W3);
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
                this.LineSwitch(0, Model.W1);
                this.LineSwitch(1, Model.W2);
                this.LineSwitch(0, Model.W3);
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
                this.LineSwitch(0, Model.W1);
                this.LineSwitch(0, Model.W2);
                this.LineSwitch(1, Model.W3);
                transactionScope.Complete();
            }

            //-- Выдача Релейная
            this.RelayTransaction(RelayState.Connect, Model.K1, Model.K2);

            //-- Задержка
            Thread.Sleep(3);

            //-- Запрос цифровых состояний
            var digitals = this.LineRequest();

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
                this.LineSwitch(1, Model.W1);
                this.LineSwitch(1, Model.W2);
                this.LineSwitch(0, Model.W3);
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
                this.LineSwitch(1, Model.W1);
                this.LineSwitch(0, Model.W2);
                this.LineSwitch(1, Model.W3);
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
                this.LineSwitch(0, Model.W1);
                this.LineSwitch(1, Model.W2);
                this.LineSwitch(1, Model.W3);
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
                this.LineSwitch(1, Model.W1);
                this.LineSwitch(1, Model.W2);
                this.LineSwitch(1, Model.W3);
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
                this.LineSwitch(0, Model.W1);
                this.LineSwitch(0, Model.W2);
                this.LineSwitch(0, Model.W3);

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
            var digitals = this.LineRequest();

            //-- Цифровой контроль
            Validate<byte>(digitals, 1, Model.R1);
            Validate<byte>(digitals, 1, Model.R2);
            Validate<byte>(digitals, 1, Model.R3);

            //-- Релейный контроль
            Validate<byte>(digitals,1, Model.F);

            //-- Контроль остальных линий
            Validate<byte>(digitals, 0);
        }
        private void ValidateDigitalOFF()
        {
            //-- Запрос цифровых состояний
            var digitals = this.LineRequest();

            //-- Цифровой контроль
            Validate<byte>(digitals, 0, Model.R1);
            Validate<byte>(digitals, 0, Model.R2);
            Validate<byte>(digitals, 0, Model.R3);

            //-- Релейный контроль
            Validate<byte>(digitals, 0, Model.F);

            //-- Контроль остальных линий
            Validate<byte>(digitals, 0);
        }
        private void ValidateAnalogON()  
        {
            var voltage = this.MeasureVoltage();
            var voltagelimit = 0.95 * U30_P;

            if (voltage < voltagelimit)
            {
                this.Logger.LogError
                    ("Напряжение: {value} (В); Верх.граница: {llim} (В)", voltage, voltagelimit);

                this.Error();
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
                this.Error();
            }
        }
    }
}
