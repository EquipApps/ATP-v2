using EquipApps.Hardware;
using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;

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
            var digitals = this.LineRequest();

            Validate<byte>(digitals, 1, "F17");
            Validate<byte>(digitals, 0);


            this.RelaySwitch
                (RelayState.Disconnect, "K17");
        }
    }
}
