using B.EK.Configure;
using EquipApps.Mvc;
using EquipApps.WorkBench;
using NLib.AtpNetCore.Mvc;

namespace B.EK.Controllers.Operate
{
    [Area(Areas.Operate)]
    [Case(1, "Наработка")]
    public class OperatingController : Controller
    {
        [Step(1, "Вкл. питание")]
        public void Action1()
        {
            //TODO: Реализовать включение питания
        }

        [Step(2, "Ожидание")]
        public void Action2()
        {
            //TODO: Реализовать включение питания
        }
    }
}
