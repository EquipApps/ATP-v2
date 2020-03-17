using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK.Controllers
{
    [Suit(1, "Исходное состояние ААП")]
    public class Etap1Controller : Controller, IEnableContext
    {
        public void Action1()
        {
        }
    }
}
