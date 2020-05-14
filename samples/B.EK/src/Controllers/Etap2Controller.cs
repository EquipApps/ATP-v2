using EquipApps.Hardware;
using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using System;

namespace B.EK.Controllers
{
    [Case(2, "Подключение источников питания ")]
    public class Etap2Controller : Controller, IEnableContext
    {
        double U3_3 = 3.3;
        double U30_C = 27;
        double U30_P = 27;
        double U6 = 6;

        public Etap2Controller()
        {

        }

        [Step(1, "3,3В-1К")]
        public void Step1()
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Connect, "K0", "K1");


            this.RelayTransaction(RelayState.Connect, "K6", "K7");

            //-- Аналоговый контроль, В ≥0,95 Uпит.
            MeasureVoltageAndCheck(U3_3);

            this.RelayTransaction(RelayState.Disconnect, "K6", "K7");
        }

        [Step(2, "3,3В-2К")]
        public void Step2()     
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Connect, "K2", "K3");
 

            this.RelayTransaction(RelayState.Connect, "K8", "K9");

            //-- Аналоговый контроль, В ≥0,95 Uпит.
            MeasureVoltageAndCheck(U3_3);

            this.RelayTransaction(RelayState.Disconnect, "K8", "K9");
        }

        [Step(3, "3,3В-2К")]
        public void Step3()     
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Connect, "K4", "K5");


            this.RelayTransaction(RelayState.Connect, "K10", "K11");

            //-- Аналоговый контроль, В ≥0,95 Uпит.
            MeasureVoltageAndCheck(U3_3);

            this.RelayTransaction(RelayState.Disconnect, "K10", "K11");
        }

        [Step(4, "ИП (+С)")]
        public void Step4()     
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Connect, "K15", "K16");


            this.RelayTransaction(RelayState.Connect, "K18", "K19");

            //-- Аналоговый контроль, В ≥0,95 Uпит.
            MeasureVoltageAndCheck(U30_C);          

            this.RelayTransaction(RelayState.Disconnect, "K18", "K19");
        }

        [Step(5, "ИП (+П)")]
        public void Step5()     
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Connect, "K22", "K23");


            this.RelayTransaction(RelayState.Connect, "Kxx", "Kxx");

            //-- Аналоговый контроль, В ≥0,95 Uпит.
            MeasureVoltageAndCheck(U30_P);           

            this.RelayTransaction(RelayState.Disconnect, "Kxx", "Kxx");
        }

        [Step(6, "ИП (+П)")]
        public void Step6()     
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Connect, "K12");

            this.RelayTransaction(RelayState.Connect, "K13", "K14");

            //-- Аналоговый контроль, В ≥0,95 Uпит.
            MeasureVoltageAndCheck(U6);          

            this.RelayTransaction(RelayState.Disconnect, "K13", "K14");
        }

        private void MeasureVoltageAndCheck(double Upit)
        {
            var voltage      = this.MeasureVoltage();
            var voltagelimit = 0.95 * Upit;

            if (voltage < voltagelimit)
            {
                this.Logger.LogError
                    ("Напряжение: {value} (В); Верх.граница: {llim} (В)", voltage, voltagelimit);

                this.Error();              
            }
            else
            {
                this.Logger.LogDebug
                    ("Напряжение: {value} (В); Верх.граница: {hlim} (В)", voltage, voltagelimit);
            }
        }

        [Step("Отключение всех ИП")]
        [OrderController("5")]
        public void Action()
        {
            //-- Удерживается до п. 5
            this.RelayTransaction(RelayState.Disconnect,
                "K0", "K1",
                "K2", "K3",
                "K4", "K5",

                "K15", "K16",
                "K22", "K23",
                "K12");
        }

    }
}
