using EquipApps.Hardware;
using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using System.Collections.Generic;

namespace B.EK.Controllers
{
    [Case(3, "Контроль стыковки")]
    public class Etap3Controller : Controller, IEnableContext
    {
        public void Action()
        {
            //-- Подключаем контроль стыковки!
            this.RelaySwitch
                (RelayState.Connect, "K17");

            //-- Запрос цифровых состояний
            var digitals = this.DigitalRequest();
 
            DigitalValidate(digitals, DigitalState.One, "F17");
            DigitalValidate(digitals, DigitalState.Null);


            this.RelaySwitch
                (RelayState.Disconnect, "K17");
        }
    }
}
