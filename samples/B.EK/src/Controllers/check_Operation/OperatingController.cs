using EquipApps.WorkBench;
using NLib.AtpNetCore.Mvc;

namespace B.EK.Controllers.check_Operation
{
    [Area("check_Operation")]
    [Suit(1,"Наработка")]
    public class OperatingController : Controller
    {
        [Step(1,"Вкл. питание")]
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
