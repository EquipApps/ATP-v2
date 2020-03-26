using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;
using NLib.AtpNetCore.Mvc;

namespace B.EK.Controllers
{
    [Case(1, "Исходное состояние ААП")]
    public class Etap1Controller : Controller, IEnableContext
    {
        public void Action1()
        {
        }
    }
}
