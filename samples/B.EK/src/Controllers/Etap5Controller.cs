using EquipApps.Hardware;
using EquipApps.Testing;
using EquipApps.WorkBench;
using NLib.AtpNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK.Controllers
{
    [Suit(5, "Отключение всех ИП")]
    public class Etap5Controller : Controller, IEnableContext
    {
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
